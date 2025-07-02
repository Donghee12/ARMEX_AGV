#include <SoftwareSerial.h>

const int buttonPin = 8;   // 버튼 핀
SoftwareSerial espSerial(11, 12);  // RX, TX (ESP-01 연결)

// 모터 제어 핀
const int motor1Pin1 = 4;
const int motor1Pin2 = 5;
const int motor2Pin1 = 6;
const int motor2Pin2 = 7;

int numbers[9];   // 12~20 중 셔플할 숫자들
int index = -1;   // -1이면 첫 번째로 11 출력
bool lastState = HIGH;

void setup() {
  pinMode(buttonPin, INPUT_PULLUP);  // 버튼 입력 (내부 풀업)

  // 모터 핀 출력 설정
  pinMode(motor1Pin1, OUTPUT);
  pinMode(motor1Pin2, OUTPUT);
  pinMode(motor2Pin1, OUTPUT);
  pinMode(motor2Pin2, OUTPUT);

  Serial.begin(9600);
  espSerial.begin(9600);
  randomSeed(analogRead(A0));  // 랜덤 시드 초기화
  shuffleNumbers();            // 숫자 셔플
}

void loop() {
  int currentState = digitalRead(buttonPin);

  if (lastState == HIGH && currentState == LOW) {
    int value;

    if (index == -1) {
      value = 11;
    } else {
      value = numbers[index];
    }

    Serial.print("✅ Button Pressed → Send: ");
    Serial.println(value);
    espSerial.println(value);  // ESP로 전송

    index++;

    if (index >= 9) {
      Serial.println("🔁 모든 숫자 출력됨 → 자동 셔플 시작");
      shuffleNumbers();
      index = -1;
    }

    delay(300);  // 디바운스
  }

  lastState = currentState;

  // ESP 수신 확인
  if (espSerial.available()) {
    String msg = espSerial.readStringUntil('\n');
    msg.trim();
    Serial.print("📥 From ESP: ");
    Serial.println(msg);

    if (msg == "GO") {
      Serial.println("🚗 GO 명령 수신 → 전진 시작");
      moveForward(2000);  // 2초 전진
      stopMotors();
    }
  }
}

// 숫자 셔플
void shuffleNumbers() {
  int pool[9] = {12, 13, 14, 15, 16, 17, 18, 19, 20};

  for (int i = 8; i > 0; i--) {
    int j = random(i + 1);
    int temp = pool[i];
    pool[i] = pool[j];
    pool[j] = temp;
  }

  for (int i = 0; i < 9; i++) {
    numbers[i] = pool[i];
  }

  Serial.println("🔀 숫자 배열 셔플 완료 (11 고정, 12~20 랜덤)");
}

// 모터 앞으로 전진
void moveForward(int duration) {
  digitalWrite(motor1Pin1, HIGH);
  digitalWrite(motor1Pin2, LOW);
  digitalWrite(motor2Pin1, HIGH);
  digitalWrite(motor2Pin2, LOW);

  delay(duration);
}

// 모터 정지
void stopMotors() {
  digitalWrite(motor1Pin1, LOW);
  digitalWrite(motor1Pin2, LOW);
  digitalWrite(motor2Pin1, LOW);
  digitalWrite(motor2Pin2, LOW);

  Serial.println("⛔ 모터 정지");
}
