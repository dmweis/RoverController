#include <Adafruit_PWMServoDriver.h>

uint8_t LEFT_FRONT_MOTOR  = 4;
uint8_t RIGHT_FRONT_MOTOR = 5;
uint8_t LEFT_REAR_MOTOR   = 6;
uint8_t RIGHT_REAR_MOTOR  = 7

uint8_t LEFT_FRONT_SERVO  = 0;
uint8_t RIGHT_FRONT_SERVO = 1;
uint8_t LEFT_REAR_SERVO   = 2;
uint8_t RIGHT_REAR_SERVO  = 3;

const uint16_t LEFT_FRONT_MOTOR_STOP_VALUE  = 345;
const uint16_t RIGHT_FRONT_MOTOR_STOP_VALUE = 330;
const uint16_t LEFT_REAR_MOTOR_STOP_VALUE   = 375;
const uint16_t RIGHT_REAR_MOTOR_STOP_VALUE  = 360;

const char START = '{';
const char END = '}';

Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();

void setup()
{
    // start Serial
    Serial.begin(9600);
    while (!Serial);
    Serial.println("Starting rover\nVER 1.2");
    // begin i2c driver
    pwm.begin();
    pwm.setPWMFreq(60);
    pwm.setPWM(LEFT_FRONT_MOTOR,  0,  LEFT_FRONT_MOTOR_STOP_VALUE );
    pwm.setPWM(RIGHT_FRONT_MOTOR, 0, RIGHT_FRONT_MOTOR_STOP_VALUE);
    pwm.setPWM(LEFT_REAR_MOTOR,   0,   LEFT_REAR_MOTOR_STOP_VALUE  );
    pwm.setPWM(RIGHT_REAR_MOTOR,  0,  RIGHT_REAR_MOTOR_STOP_VALUE );
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
                Ack(0);
                break;
            case 1:
                SetAllMotors();
                Ack(1);
                break;
            case 2:
                SetAllServos();
                Ack(2);
                break;
            case 3:
                SetMotorIndexes();
                Ack(3);
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

void Ack(uint8_t index)
{
    Serial.println(index);
}

// command 1
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
    pwm.setPWM(LEFT_REAR_MOTOR, 0, left_rear_pulse);
    pwm.setPWM(RIGHT_REAR_MOTOR, 0, right_rear_pulse);
}

// command 2
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

// command 0
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
    pwm.setPWM(index, 0, pulse);
}

// command 3
void SetMotorIndexes(){
    uint8_t LEFT_FRONT_MOTOR_TMP  = (uint8_t)Serial.parseInt();
    uint8_t RIGHT_FRONT_MOTOR_TMP = (uint8_t)Serial.parseInt();
    uint8_t LEFT_REAR_MOTOR_TMP   = (uint8_t)Serial.parseInt();
    uint8_t RIGHT_REAR_MOTOR_TMP  = (uint8_t)Serial.parseInt();

    uint8_t LEFT_FRONT_SERVO_TMP  = (uint8_t)Serial.parseInt();
    uint8_t RIGHT_FRONT_SERVO_TMP = (uint8_t)Serial.parseInt();
    uint8_t LEFT_REAR_SERVO_TMP   = (uint8_t)Serial.parseInt();
    uint8_t RIGHT_REAR_SERVO_TMP  = (uint8_t)Serial.parseInt();
    char end = Serial.read();
    if (end != END)
    {
        Serial.print("END wrong: ");
        Serial.println(end);
        return;
    }
    uint8_t LEFT_FRONT_MOTOR  = LEFT_FRONT_MOTOR_TMP;
    uint8_t RIGHT_FRONT_MOTOR = RIGHT_FRONT_MOTOR_TMP;
    uint8_t LEFT_REAR_MOTOR   = LEFT_REAR_MOTOR_TMP;
    uint8_t RIGHT_REAR_MOTOR  = RIGHT_REAR_MOTOR_TMP;

    uint8_t LEFT_FRONT_SERVO  = LEFT_FRONT_SERVO_TMP;
    uint8_t RIGHT_FRONT_SERVO = RIGHT_FRONT_SERVO_TMP;
    uint8_t LEFT_REAR_SERVO   = LEFT_REAR_SERVO_TMP;
    uint8_t RIGHT_REAR_SERVO  = RIGHT_REAR_SERVO_TMP;
}

