import cv2
import numpy as np
import time
from Robot_Arm_Control import execute_action, return_to_initial, reset_servos, action_floor1, action_floor2
from picamera2 import Picamera2
import paho.mqtt.client as mqtt

import threading
import RPi.GPIO as GPIO

# 템플릿 이미지 로드 및 전처리
template = cv2.imread('1.jpg', cv2.IMREAD_GRAYSCALE)
template = cv2.resize(template, (0, 0), fx=0.75, fy=0.75, interpolation=cv2.INTER_CUBIC)
w, h = template.shape[::-1]

was_detected = False  # 이전 프레임에서 감지 여부 저장
locked = False        # 동작 중 중복 실행 방지 플래그
command = ""          # 수신된 명령 저장용 변수

vision_active_flag = True  # 비전 동작 활성화 플래그
vision_thread = None       # 비전 스레드 객체

BROKER_IP = "192.168.0.101"  # MQTT 브로커 IP 주소
client = mqtt.Client()       # MQTT 클라이언트 생성


# === 초음파 센서 설정 ===
SENSORS = [
    {"TRIG": 17, "ECHO": 18, "name": "Sensor 1"},
    {"TRIG": 23, "ECHO": 24, "name": "Sensor 2"}
]

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)

# 각 센서 핀 초기화
for sensor in SENSORS:
    GPIO.setup(sensor["TRIG"], GPIO.OUT)
    GPIO.setup(sensor["ECHO"], GPIO.IN)
    GPIO.output(sensor["TRIG"], False)
time.sleep(2)

# === 초음파 센서 거리 측정 함수 ===
def measure_distance(TRIG, ECHO):
    GPIO.output(TRIG, True)
    time.sleep(0.00001)
    GPIO.output(TRIG, False)

    pulse_start = pulse_end = None
    timeout = time.time() + 1
    
    # 펄스 시작 신호 대기
    while GPIO.input(ECHO) == 0:
        pulse_start = time.time()
        if time.time() > timeout:
            return None

    # 펄스 끝 신호 대기
    timeout = time.time() + 1
    while GPIO.input(ECHO) == 1:
        pulse_end = time.time()
        if time.time() > timeout:
            return None

    if pulse_start is None or pulse_end is None:
        return None

    #=== 거리 계산 (cm) ===
    pulse_duration = pulse_end - pulse_start
    distance = pulse_duration * 17150
    return round(distance, 2)


# 초음파 센서 상태를 체크하며 동작을 제어하는 반복 루프
def sensor_loop():
    sensor_states = {
        "Sensor 1": {"blocked": False, "resume_start_time": None, "stop_msg": "STOP_FW", "resume_msg": "RESUME"},
        "Sensor 2": {"blocked": False, "resume_start_time": None, "stop_msg": "STOP_BW", "resume_msg": "RESUME"}
    }

    while True:
        for sensor in SENSORS:
            dist = measure_distance(sensor["TRIG"], sensor["ECHO"])
            if dist is None:
                continue

            current_time = time.time()
            name = sensor["name"]
            state = sensor_states[name]

            # 장애물 감지 시 정지 명령 발행
            if dist <= 10:
                if not state["blocked"]:
                    client.publish("Robot_To/PiCar", state["stop_msg"])
                    print(f"{name}: {dist} cm → {state['stop_msg']} sent")
                    state["blocked"] = True
                state["resume_start_time"] = None

            # 장애물이 사라졌을 때 재개 명령 발행
            elif dist > 12:
                if state["blocked"]:
                    if state["resume_start_time"] is None:
                        state["resume_start_time"] = current_time
                    elif current_time - state["resume_start_time"] >= 0.5:
                        client.publish("Robot_To/PiCar", state["resume_msg"])
                        print(f"{name}: {dist} cm → {state['resume_msg']} sent")
                        state["blocked"] = False
                        state["resume_start_time"] = None
            else:
                state["resume_start_time"] = None

        time.sleep(0.2)


# === MQTT 연결 성공 콜백 함수 ===
def on_connect(client, userdata, flags, rc):
    print("✅ Connected to MQTT broker")
    # 필요한 토픽 구독
    client.subscribe("PC_To/Robot")     # PC(MQTT broker)에서 로봇팔로 
    client.subscribe("Arduino_To/Pi")   # 아두이노에서 로봇팔로
    client.subscribe("PiCar_To/Robot")  # PiCar에서 로봇팔로

# 전역 변수 선언    
floor_target = 2        # 현재 적재할 층 (1 또는 2)
awaiting_put = False    # 적재 대기 상태 플래그

# === MQTT 메시지 수신 콜백 함수 ===
def on_message(client, userdata, msg):
    global command, floor_target, awaiting_put, vision_active_flag
    payload = msg.payload.decode("utf-8", errors="ignore")
    print(f"Received [{msg.topic}]: {payload}")
    time.sleep(0.2)
	
    if msg.topic == "PC_To/Robot":
        print("From PC →", payload)
        if(payload == "RESET"):
           vision_active_flag = True
           start_vision()
           awaiting_put = True
			
    elif msg.topic == "Arduino_To/Pi":
        print(f"FROM ARDUINO: {payload}")
        if(payload == "STOP_ACTION"):
            reset_servos()                  # 서보 초기화
            print("reset Robot arm")
            awaiting_put = False
            vision_active_flag = False
		
    elif msg.topic == "PiCar_To/Robot":
        time.sleep(2)
        if(payload == "GRAB"):
          print("recevied GRAB")
          vision_active_flag = True
          start_vision()
          awaiting_put = True
          print("Grab break")
        elif(payload == "PUT"):
          if awaiting_put:	
            if floor_target == 1:
              print("Putting to floor 1...")
              action_floor1()
              time.sleep(0.5)
              reset_servos()
              floor_target = 2
              print("put to floor 1 done")
              client.publish("Robot_To/PiCar","MOVE_FORWARD")
            elif floor_target == 2:
              action_floor2()
              time.sleep(0.5)
              reset_servos() 
              floor_target = 1
              print("put to floor 2 done")
              client.publish("Robot_To/PiCar","MOVE_FORWARD")
              print("robot send to picar  '"'MOVE_FORWARD'"'")
            awaiting_put =False
          else:
            print("No GRAB just PUT")
            client.publish("Robot_To/PiCar","MOVE_FORWARD")

    else:
        print("Error GRAB")
        
        
# MQTT 브로커 연결 및 백그라운드 루프 시작
client.on_connect = on_connect
client.on_message = on_message
client.connect(BROKER_IP, 1883)
client.loop_start()

# === 비전 스레드 시작 함수 ===
def start_vision():
	global vision_thread
	if vision_thread is None or not vision_thread.is_alive():
		print("Thread in")
		vision_thread = threading.Thread(target=process_vision)
		vision_thread.start()


# === 카메라 영상 처리 및 템플릿 매칭 기반 물체 감지 루프 ===
def process_vision():
    global was_detected, locked
	
    picam2 = Picamera2()
    video_config = picam2.create_video_configuration(
        main={"size": (320, 240)},
        controls={"FrameDurationLimits": (11111, 11111)}  # 약 90 FPS 설정
    )
    picam2.configure(video_config)
    picam2.start()

    print("Camera started")

    while vision_active_flag:
        frame = picam2.capture_array("main")
        frame = cv2.cvtColor(frame, cv2.COLOR_RGB2BGR)
        frame = cv2.rotate(frame, cv2.ROTATE_180)

        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        height, width = frame.shape[:2]
        center = width // 2
        roi_left, roi_right = center - 60, center + 60

        # 관심 영역(ROI) 사각형 그리기
        cv2.rectangle(frame, (roi_left, 0), (roi_right, height), (0, 0, 255), 2)

        # 템플릿 매칭 수행
        res = cv2.matchTemplate(gray, template, cv2.TM_CCOEFF_NORMED)
        _, max_val, _, max_loc = cv2.minMaxLoc(res)

        is_detected = False
        # 임계값 이상일 때만 검출 유효
        if max_val >= 0.7:      
            top_left = max_loc
            bottom_right = (top_left[0] + w, top_left[1] + h)
            cv2.rectangle(frame, top_left, bottom_right, (0, 255, 0), 2)
            
            # 검출이 ROI 내부에 있는지 확인
            if roi_left <= top_left[0] <= roi_right and roi_left <= bottom_right[0] <= roi_right:
                is_detected = True
                cv2.putText(frame, "DETECT", (30, 50), cv2.FONT_HERSHEY_SIMPLEX, 1.2, (0, 0, 255), 3)
                # 집기 동작 실행
                if not was_detected and not locked: 
                    print("DETECTED - Executing robot arm")
                    execute_action()
                    time.sleep(0.5)
                    return_to_initial()
                    #time.sleep(0.5)   
                    client.publish("Robot_To/PiCar","MOVE_BACKWARD")
                    print("robot send to picar  '"'MOVE_BACKWARD'"'") 
                    break               

        was_detected = is_detected
        cv2.waitKey(1)
    
    picam2.close()
    cv2.destroyAllWindows()
    
if __name__ == "__main__":
    print("MQTT listening for vision commannd...")
    # 초음파 센서 스레드 시작
    threading.Thread(target=sensor_loop, daemon=True).start()
    while True:
     time.sleep(0.5)
