import pigpio
import time

# 핀 번호와 초기 PWM 펄스 폭 설정
pins = [5, 6, 13, 19]
angles = [2500, 2000, 600, 500]

# pigpio 초기화
pi = pigpio.pi()

# 초기 PWM 펄스 폭 설정
for i in range(4):
    pi.set_servo_pulsewidth(pins[i], angles[i])

# ==== 함수들 ====


# 물체 집기 동작 (로봇팔 집는 위치로 이동)
def execute_action():
    # 각도 초기화
    angles[0] = 2500
    angles[1] = 2000
    angles[2] = 600
    pi.set_servo_pulsewidth(pins[0], angles[0])
    pi.set_servo_pulsewidth(pins[1], angles[1])
    pi.set_servo_pulsewidth(pins[2], angles[2])

    target_2 = 1300
    target_3 = 1250
    current_2 = angles[1]
    current_3 = angles[2]
    step = 1

     # 2번(어깨), 3번(손목) 서보모터를 천천히 목표 위치로 이동
    while current_2 != target_2 or current_3 != target_3:
        if current_2 != target_2:
            current_2 = min(current_2 + step, target_2) if current_2 < target_2 else max(current_2 - step, target_2)
            angles[1] = current_2
            pi.set_servo_pulsewidth(pins[1], current_2)

        if current_3 != target_3:
            current_3 = min(current_3 + step, target_3) if current_3 < target_3 else max(current_3 - step, target_3)
            angles[2] = current_3
            pi.set_servo_pulsewidth(pins[2], current_3)
            
    # 4번 서보(집게)모터를 닫음
    angles[3] = 1500
    pi.set_servo_pulsewidth(pins[3], angles[3])
    return "Action completed"

# 집기 동작 후 기본 위치로 복귀
def return_to_initial():
    # 목표 펄스 폭
    target_0 = 2500
    target_1 = 2000
    target_2 = 600

    current_0 = angles[0]
    current_1 = angles[1]
    current_2 = angles[2]

    step = 1

    # 모든 관절을 초기 위치로 이동
    while current_0 != target_0 or current_1 != target_1 or current_2 != target_2:
        if current_0 != target_0:
            current_0 = min(current_0 + step, target_0) if current_0 < target_0 else max(current_0 - step, target_0)
            angles[0] = current_0
            pi.set_servo_pulsewidth(pins[0], current_0)

        if current_1 != target_1:
            current_1 = min(current_1 + step, target_1) if current_1 < target_1 else max(current_1 - step, target_1)
            angles[1] = current_1
            pi.set_servo_pulsewidth(pins[1], current_1)

        if current_2 != target_2:
            current_2 = min(current_2 + step, target_2) if current_2 < target_2 else max(current_2 - step, target_2)
            angles[2] = current_2
            pi.set_servo_pulsewidth(pins[2], current_2)

        time.sleep(0.0006)

    return "Return completed"
    
# 1층에 물건 올리기
def action_floor1():
    seq = [
        (3, 500) ,(2, 1800),(3, 600) ,(2, 1700),(3, 650) ,
        (2, 1650),(3, 700) ,(3, 650) ,(3, 750) ,(2, 1550),
        (3, 800) ,(2,1500) ,(3, 850) ,(2,1450) ,(3, 900) ,
        (2,1400) ,(3, 950) ,(2,1350) ,(3, 1000),(2,1300) ,
        (3, 1050),(2,1250) ,(2, 1200),(3, 1100),(4, 500)
    ]
    # 서보 단계별 움직임 실행
    for axis, target in seq:
        index = axis - 1
        while angles[index] != target:
            if angles[index] < target:
                angles[index] += 1
            elif angles[index] > target:
                angles[index] -= 1
            pi.set_servo_pulsewidth(pins[index], angles[index])
            time.sleep(0.00025)

    return "floor1"

# 2층에 물건 올리기
def action_floor2():
    angles[0] = 2500
    pi.set_servo_pulsewidth(pins[0], angles[0])

    target_2 = 1350
    target_3 = 1300
    # 2번(어깨), 3번(손목) 서보 목표 위치로 이동
    while angles[1] != target_2 or angles[2] != target_3:
        if angles[1] < target_2:
            angles[1] += 1
        elif angles[1] > target_2:
            angles[1] -= 1
        pi.set_servo_pulsewidth(pins[1], angles[1])

        if angles[2] < target_3:
            angles[2] += 2
        elif angles[2] > target_3:
            angles[2] -= 2
        pi.set_servo_pulsewidth(pins[2], angles[2])
        time.sleep(0.00035)
        
    time.sleep(0.2)
    target_4 = 500
    # 4번 서보(집게게) 목표 위치로 이동
    while angles[3] != target_4:
        if angles[3] < target_4:
            angles[3] += 1
        elif angles[3] > target_4:
            angles[3] -= 1
        pi.set_servo_pulsewidth(pins[3], angles[3])
        time.sleep(0.00035)

    return "floor2"

# 모든 서보를 기본 홈 위치로 리셋
def reset_servos():
    # 기본 각도로 초기화
    default_angles = [2500, 2000, 600, 500]
    print("enter resdasf")
    for i in range(4):
        angles[i] = default_angles[i]
        pi.set_servo_pulsewidth(pins[i], angles[i])
    return "Servos reset to default"

# 현재 PWM 값 반환 함수
def get_angles():
    return angles
