import time
import threading
from board import SCL, SDA
import busio
from adafruit_pca9685 import PCA9685
from adafruit_motor import motor, servo
from gpiozero import InputDevice
import paho.mqtt.client as mqtt

# === 모터 / 서보 채널 설정 ===
MOTOR_M1_IN1 = 12
MOTOR_M1_IN2 = 13
MOTOR_M2_IN1 = 15
MOTOR_M2_IN2 = 14
STEERING_SERVO_CH = 0
FREQ_MOTOR = 1000
FREQ_SERVO = 50

M1_Direction = -1
M2_Direction = 1

# === 라인 센서 핀 설정 ===
left_sensor = InputDevice(22)
middle_sensor = InputDevice(27)
right_sensor = InputDevice(17)

# === 하드웨어 전역 변수 ===
motor1 = None
motor2 = None
pwm_motor = None
steering_servo = None
line_tracing_active = False
line_thread = None
current_direction = 1 # 1은 전진, -1은 후진, 0은 정지

# === MQTT 설정 ===
BROKER = "192.168.0.101"  
#TOPIC_SUB = "Robot_To/PiCar"
#TOPIC_PUB = "PiCar_To/Robot"

client = mqtt.Client()

# === 값 매핑 함수 ===
def map_val(x, in_min, in_max, out_min, out_max):
    return (x - in_min) / (in_max - in_min) * (out_max - out_min) + out_min

# === 초기 하드웨어 셋업 함수 ===
def setup():
    global motor1, motor2, pwm_motor, steering_servo
    i2c = busio.I2C(SCL, SDA)
    pwm_motor = PCA9685(i2c, address=0x5f)
    pwm_motor.frequency = FREQ_MOTOR

    motor1 = motor.DCMotor(pwm_motor.channels[MOTOR_M1_IN1], pwm_motor.channels[MOTOR_M1_IN2])
    motor2 = motor.DCMotor(pwm_motor.channels[MOTOR_M2_IN1], pwm_motor.channels[MOTOR_M2_IN2])
    motor1.decay_mode = motor.SLOW_DECAY
    motor2.decay_mode = motor.SLOW_DECAY

    pwm_motor.frequency = FREQ_SERVO
    steering_servo = servo.Servo(pwm_motor.channels[STEERING_SERVO_CH])
    steering_servo.angle = 90  # 기본값은 중앙 (직진 방향)

# === 모터 정지 함수 ===
def motorStop():
    motor1.throttle = 0
    motor2.throttle = 0

# === 모터가 멈춰있는지 체크하는 함수 === 
def check_motor_inactive():
    if motor1.throttle == 0 and motor2.throttle == 0:
        return True
    return False

# === 특정 채널 모터 동작 함수 ===
def Motor(channel, direction, motor_speed):
    motor_speed = max(0, min(motor_speed, 100))  # 속도 범위 0~100으로 제한
    speed = map_val(motor_speed, 0, 100, 0, 1.0)  # 0~1 사이 값으로 매핑
    if direction == -1:
        speed = -speed # 방향 반전
    if channel == 1:
        motor1.throttle = speed
    elif channel == 2:
        motor2.throttle = speed

# === 전진/후진 및 속도 제어 함수 ===
def move(speed, direction):
    if speed == 0:
        motorStop()
    else:
        Motor(1, M1_Direction * direction, speed)
        Motor(2, M2_Direction * direction, speed)

# === 종료 시 하드웨어 정리 함수 ===        
def destroy():
    motorStop()
    pwm_motor.deinit()

# === 라인 트레이싱 동작을 수행하는 스레드 함수 ===
def line_tracing():
    global current_direction, line_tracing_active
    line_tracing_active = True

    while line_tracing_active:
        left = left_sensor.value
        middle = middle_sensor.value
        right = right_sensor.value

        if not line_tracing_active:
            print("line tracing is not acitve")
            break  

        # 세 센서 모두 라인을 벗어나면(0이면 라인 없음)    
        if left == 0 and middle == 0 and right == 0:
            print("Line lost stopping")
            motorStop()
            steering_servo.angle = 90

            # 현재 방향에 따라 MQTT 메시지 전송
            if current_direction == 1:
                client.publish("PiCar_To/PC", "READY")
                client.publish("PiCar_To/Robot", "GRAB")
                print("SENT GRAB to Robot Arm")
                time.sleep(0.5)
                break

            elif current_direction == -1:
                client.publish("PiCar_To/Robot", "PUT")
                print("SENT PUT to Robot Arm")
                break

         # 라인 따라가며 서보 조향
        if current_direction == 1:
            if middle == 1 and left == 0 and right == 0:
                steering_servo.angle = 90   # 직진
            elif left == 1:
                steering_servo.angle = 110  # 왼쪽 조향
            elif right == 1:
                steering_servo.angle = 70    # 오른쪽 조향
            else:
                steering_servo.angle = 90    # 기본 직진

        time.sleep(0.05)

    motorStop()

# === 라인 트레이싱 스레드 시작 함수 ===
def start_line_tracing():
    global line_thread
    if line_thread is None or not line_thread.is_alive():
        line_thread = threading.Thread(target=line_tracing, daemon=True)
        line_thread.start()

# === 라인 트레이싱 중지 함수 ===
def stop_line_tracing():
    global line_tracing_active
    line_tracing_active = False
    
# === MQTT 브로커 연결 시 호출되는 콜백 함수 ===
def on_connect(client, userdata, flags, rc):
    print("Connected to MQTT Broker")
    client.subscribe("Robot_To/PiCar")
    client.subscribe("PC_To/PiCar")
    client.subscribe("Arduino_To/Pi")

# === MQTT 메시지 수신 시 호출되는 콜백 함수 ===
def on_message(client, userdata, msg):
    global current_direction
    payload = msg.payload.decode("utf-8", errors="ignore")
    print(f"Received [{msg.topic}]: {payload}")

    if payload == "MOVE_FORWARD":
        print("Picar MOVE_FORWARD")
        move(30, 1)
        time.sleep(0.5)
        current_direction = 1  # 전진 방향 설정
        start_line_tracing()
       
    elif payload == "MOVE_BACKWARD":
        print("Picar MOVE_BACKWARD")
        current_direction = -1  # 후진 방향 설정
        steering_servo.angle = 90
        move(30, -1)
        time.sleep(0.5)  # 후진 0.5초 유지
        start_line_tracing()  ## 라인 트레이싱 재개
    
    elif payload == "STOP_FW" and current_direction == 1:
        print("STOP_FW command received")
        motorStop()
        stop_line_tracing()
    
    elif payload == "STOP_BW" and current_direction == -1:
        print("STOP_BW command received")
        motorStop()
        stop_line_tracing()

    elif payload == "STOP_ACTION":
        print("STOP_ACTION command received")
        motorStop()
        stop_line_tracing()

    elif payload == "RESUME":
        print("RESUME command received")
        if current_direction == 1:
            print(f"direction is {current_direction}")
            move(30, 1)
            start_line_tracing()
        elif current_direction == -1:
            print(f"direction is {current_direction}")
            move(30, -1)
            start_line_tracing()
            
    elif payload == "RESET" : 
        print("RESET command received")
        current_direction = 1
        print(f"CD: {current_direction} ")
        if left_sensor.value == 1 or right_sensor.value ==1 or middle_sensor.value == 1 :
            print(f"----------------------------------")
            start_line_tracing()
            move(30,1)
            
        

# === 메인 실행부 ===
if __name__ == "__main__":
    try:
        setup()
        move(30,1)                      # 기본 전진 명령
        start_line_tracing()            # 라인 트레이싱 시작
        client.on_connect = on_connect
        client.on_message = on_message
        client.connect(BROKER, 1883)    # MQTT 브로커 연결
        client.loop_forever()           # 무한 루프 실행 (MQTT 이벤트 처리)
    except KeyboardInterrupt:
        destroy()                       # 종료 시 정리 작업