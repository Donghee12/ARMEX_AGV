#include <SoftwareSerial.h>
#include <Servo.h>

SoftwareSerial espSerial(11, 10); // TX - 11, RX - 10


const int motor1DirPin = 2; // main_belt direction
const int motor1PWMPin = 3; // main_belt speed

const int motor2DirPin = 5; // second_belt direction 
const int motor2PWMPin = 4; // second_belt speed

const int servoPin = 6;  // 서보모터 핀

const int redPin = 7;     // RGB LED 핀
const int greenPin = 8;
const int bluePin = 9;

const int trigPin = 12; //초음파 trig 핀
const int echoPin = 13; //초음파 echo 핀

static int motor1Speed = 0;
static int motor2Speed = 0;
Servo myServo;

long getDistance() {
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW); 

  long duration = pulseIn(echoPin, HIGH, 20000);
  long distance = duration * 0.034 / 2;
  return distance;
}

void setup() {
  espSerial.begin(9600);
  Serial.begin(9600);

  pinMode(motor1DirPin, OUTPUT);
  pinMode(motor1PWMPin, OUTPUT);
  digitalWrite(motor1DirPin, HIGH);

  pinMode(motor2DirPin, OUTPUT);
  pinMode(motor2PWMPin, OUTPUT);
  digitalWrite(motor2DirPin, HIGH);

  myServo.attach(servoPin);
  myServo.write(0);

  pinMode(redPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(bluePin, OUTPUT);
  setColor(0,255,0);    // 초기 : green

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);

}

int secondStopped = 0;  // 정지 상태 여부 추적

void stopSecondBelt() {

    analogWrite(motor2PWMPin, 255);        
    Serial.println("SECOND 벨트 정지됨");  // 메시지 1회 출력
    setColor(255,0,0);    // red
    secondStopped = 1;                   // 플래그 ON
  
}

// -- RGB LED 색 변환 함수
void setColor(int red, int green, int blue){
  analogWrite(redPin, red);
  analogWrite(greenPin, green);
  analogWrite(bluePin, blue);
}

void loop() {
  long distance = getDistance();
  delay(50);
  
  if (( distance > 5 && distance <= 7 )&& secondStopped == 0) { // 물체 집기 실패 시 PC
    stopSecondBelt();
    espSerial.println("STOP_ACTION");
    delay(50);
    Serial.println("SONIC: "+ String(distance)+"cm");
    delay(50);
  }


// ------------------- RECEIVING SIDE ------------------------------------
  if (espSerial.available()) {
    String command = espSerial.readStringUntil('\n');
    command.trim();

    if (command.startsWith("MAIN:")) {
      motor1Speed = command.substring(5).toInt();
      motor1Speed = constrain(motor1Speed, 0, 255);
      analogWrite(motor1PWMPin, motor1Speed);
      Serial.print("Set Main Speed: "+ String(motor1Speed));
      delay(50);

    }
      
    else if (command.startsWith("SECOND:")) { // 2번 동작 버튼 
      motor2Speed = command.substring(7).toInt();
      motor2Speed = constrain(motor2Speed, 0, 255);
      analogWrite(motor2PWMPin, motor2Speed);
      delay(50);

      if(motor2Speed == 255)
      {
        setColor(255,0,0);
      }
      else if(motor2Speed == 0)
      {
        setColor(0,255,0);
      }

      if(secondStopped) // 2번 벨트 멈춤 상태 해지 필요
      {
        secondStopped = 0;
        Serial.print("Status CHANGED: "+ String(secondStopped));
        setColor(0,255,0);
        delay(50);
      }
    }

    else if(command.startsWith("SERVO:"))
    {
      int input = command.substring(6).toInt();
      if (input <= 180) {
          myServo.write(input);
          Serial.print("서보모터 각도 설정됨: ");
          Serial.println(input);
          delay(50);
        } else {
          Serial.println("⚠ 서보모터 각도는 0~180 사이여야 합니다.");
          delay(50);
        }
    }
  }
}
