#include <Adafruit_PWMServoDriver.h>

const int pwm_a = 8;
const int pwm_b = 6;
const int dir_a = 9;
const int dir_b = 7;

const uint8_t LEFT_FRONT_MOTOR = 0;
const uint8_t RIGHT_FRONT_MOTOR = 3;
const uint8_t LEFT_REAR_MOTOR = 17;
const uint8_t RIGHT_REAR_MOTOR = 4;

const uint8_t LEFT_FRONT_SERVO = 1;
const uint8_t RIGHT_FRONT_SERVO = 6;
const uint8_t LEFT_REAR_SERVO = 2;
const uint8_t RIGHT_REAR_SERVO = 5;

const char START = '{';
const char END = '}';

Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();

void setup()
{
    // start Serial
    Serial.begin(9600);
    while (!Serial)
        ;
    Serial.println("Starting rover");
    Serial.println("VER 1.1");
    // init outputs
    pinMode(pwm_a, OUTPUT);
    pinMode(pwm_b, OUTPUT);
    pinMode(dir_a, OUTPUT);
    pinMode(dir_b, OUTPUT);
    // set motor init state
    analogWrite(pwm_a, 0);
    analogWrite(pwm_b, 0);
    // begin i2c driver
    pwm.begin();
    pwm.setPWMFreq(60);
    pwm.setPWM(LEFT_FRONT_MOTOR, 0, 345);
    pwm.setPWM(RIGHT_FRONT_MOTOR, 0, 330);
    pwm.setPWM(LEFT_REAR_MOTOR, 0, 375);
    pwm.setPWM(RIGHT_REAR_MOTOR, 0, 360);
    Serial.println("Rover ready");
}

void loop()
{
    if (Serial.available() > 1)
    {
        int start = Serial.read();
        if (start == START)
        {
            int command = Serial.parseInt();
            switch (command)
            {
            case 0:
                SetMotor();
                break;
            case 1:
                SetAllMotors();
                break;
            case 2:
                SetAllServos();
                break;
            }
        }
        else
        {
            Serial.print("START wrong: ");
            Serial.println(start);
        }
    }
}

void SetAllMotors()
{
    uint16_t left_front_pulse = (uint16_t)Serial.parseInt();
    uint16_t right_front_pulse = (uint16_t)Serial.parseInt();
    uint16_t left_rear_pulse = (uint16_t)Serial.parseInt();
    uint16_t right_rear_pulse = (uint16_t)Serial.parseInt();
    char end = Serial.read();
    if (end != END)
    {
        Serial.print("END wrong: ");
        Serial.println(end);
        return;
    }
    pwm.setPWM(LEFT_FRONT_MOTOR, 0, left_front_pulse);
    pwm.setPWM(RIGHT_FRONT_MOTOR, 0, right_front_pulse);
    setMotorA(left_rear_pulse);
    pwm.setPWM(RIGHT_REAR_MOTOR, 0, right_rear_pulse);
}

void SetAllServos()
{
    uint16_t left_front_pulse = (uint16_t)Serial.parseInt();
    uint16_t right_front_pulse = (uint16_t)Serial.parseInt();
    uint16_t left_rear_pulse = (uint16_t)Serial.parseInt();
    uint16_t right_rear_pulse = (uint16_t)Serial.parseInt();
    char end = Serial.read();
    if (end != END)
    {
        Serial.print("END wrong: ");
        Serial.println(end);
        return;
    }
    pwm.setPWM(LEFT_FRONT_SERVO, 0, left_front_pulse);
    pwm.setPWM(RIGHT_FRONT_SERVO, 0, right_front_pulse);
    pwm.setPWM(LEFT_REAR_SERVO, 0, left_rear_pulse);
    pwm.setPWM(RIGHT_REAR_SERVO, 0, right_rear_pulse);
}

void SetMotor()
{
    uint8_t index = (uint8_t)Serial.parseInt();
    uint16_t pulse = (uint16_t)Serial.parseInt();
    char end = Serial.read();
    if (end != END)
    {
        Serial.print("END wrong: ");
        Serial.println(end);
        return;
    }
    if (index == 17)
    {
        return;
    }
    pwm.setPWM(index, 0, pulse);
}

void setMotorA(int pulse)
{
    if (pulse >= 375)
    {
        int motorPwm = constrain(map(pulse, 375, 600, 0, 255), 0, 255);
        digitalWrite(dir_a, HIGH);
        analogWrite(pwm_a, motorPwm);
    }
    else
    {
        int motorPwm = constrain(map(pulse, 375, 150, 0, 255), 0, 255);
        digitalWrite(dir_a, LOW);
        analogWrite(pwm_a, motorPwm);
    }
}
