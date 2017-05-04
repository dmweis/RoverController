import json
import serial
import pika


class Rover:
    def __init__(self):
        self.legs = json.load(open('config.json'))
        self.port = serial.Serial('swag', 9600)
        self.connection = pika.BlockingConnection(pika.ConnectionParameters(host="localhost"))
        self.channel = self.connection.channel()
        self.channel.exchange_declare(exchange="rover_exchange", type='topic')
        result = self.channel.queue_declare(exclusive=True)
        self.queue_name = result.method.queue
        self.channel.queue_bind(exchange='rover_exchange',
                                queue=self.queue_name,
                                routing_key='#.rover')
        self.channel.basic_consume(self.send_message,
                                   queue=self.queue_name,
                                   no_ack=True)
        self.channel.start_consuming()

    def send_message(self, ch, method, properties, body):
        self.port.write(body)
        print " [x] %r:%r" % (method.routing_key, body)

    def set_legs(self, left_front, right_front, left_rear, right_rear):
        front_left_leg = self.legs['FrontLeft']
        front_right_leg = self.legs['FrontRight']
        rear_left_leg = self.legs['RearLeft']
        rear_right_leg = self.legs['RearRight']
        front_left_pulse = linera_map(left_front, front_left_leg['Servo']['Angle1'], front_left_leg['Servo']['Angle2'], front_left_leg['Servo']['Pwm1'], front_left_leg['Servo']['Pwm2'])
        front_right_pulse = linera_map(right_front, front_right_leg['Servo']['Angle1'], front_right_leg['Servo']['Angle2'], front_right_leg['Servo']['Pwm1'], front_right_leg['Servo']['Pwm2'])
        rear_left_pulse = linera_map(left_rear, rear_left_leg['Servo']['Angle1'], rear_left_leg['Servo']['Angle2'], rear_left_leg['Servo']['Pwm1'], rear_left_leg['Servo']['Pwm2'])
        rear_right_pulse = linera_map(right_rear, rear_right_leg['Servo']['Angle1'], rear_right_leg['Servo']['Angle2'], rear_right_leg['Servo']['Pwm1'], rear_right_leg['Servo']['Pwm2'])
        self.port.write('{2 {} {} {} {}}'.format(front_left_pulse, front_right_pulse, rear_left_pulse, rear_right_pulse))

    def set_speed(self, left_front, right_front, left_rear, right_rear):
        max = 100
        min = -max
        front_left_pulse = self.set_speed_to_leg(left_front, self.legs["FrontLeft"], min, max)
        front_right_pulse = self.set_speed_to_leg(right_front, self.legs["FrontRight"], min, max)
        rear_left_pulse = self.set_speed_to_leg(left_rear, self.legs["RearLeft"], min, max)
        rear_right_pulse = self.set_speed_to_leg(right_rear, self.legs["RearRight"], min, max)
        self.port.write('{1 {} {} {} {}}'.format(front_left_pulse, front_right_pulse, rear_left_pulse, rear_right_pulse))


def set_speed_to_leg(speed, leg, min, max):
    if  speed < min or speed > max:
        raise ValueError()
    if speed > 0:
        return linera_map(speed, 0, 100, leg["Motor"]["StationaryValue"], leg["Motor"]["MaxForward"])
    else:
        return linera_map(speed, 0, -100, leg["Motor"]["StationaryValue"], leg["Motor"]["MaxBackwards"])


def linera_map(value, in_min, in_max, out_min, out_max):
    return (value - in_min) * (out_max - out_min) / (in_max - in_min) + out_min