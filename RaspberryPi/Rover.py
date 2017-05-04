import json
import serial

class Rover:
    def __init__(self):
        self.legs = json.load(open('config.json'))
        self.port = serial.Serial('swag', 9600)

    def set_legs(self, left_front, right_front, left_rear, right_rear):
        front_left_leg = self.legs['FrontLeft']
        front_right_leg = self.legs['FrontLeft']
        rear_left_leg = self.legs['FrontLeft']
        rear_right_leg = self.legs['FrontLeft']
        front_left_pulse = linera_map(left_front, front_left_leg['Servo']['Angle1'], front_left_leg['Servo']['Angle2'], front_left_leg['Servo']['Pwm1'], front_left_leg['Servo']['Pwm2']);
        front_right_pulse = linera_map(right_front, front_right_leg['Servo']['Angle1'], front_right_leg['Servo']['Angle2'], front_right_leg['Servo']['Pwm1'], front_right_leg['Servo']['Pwm2']);
        rear_left_pulse = linera_map(left_rear, rear_left_leg['Servo']['Angle1'], rear_left_leg['Servo']['Angle2'], rear_left_leg['Servo']['Pwm1'], rear_left_leg['Servo']['Pwm2']);
        rear_right_pulse = linera_map(right_rear, rear_right_leg['Servo']['Angle1'], rear_right_leg['Servo']['Angle2'], rear_right_leg['Servo']['Pwm1'], rear_right_leg['Servo']['Pwm2']);
        self.port.write('{2 {} {} {} {}}'.format(front_left_pulse, front_right_pulse, rear_left_pulse, rear_right_pulse));


def linera_map(value, in_min, in_max, out_min, out_max):
    return (value - in_min) * (out_max - out_min) / (in_max - in_min) + out_min