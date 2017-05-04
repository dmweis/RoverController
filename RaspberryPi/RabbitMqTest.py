#!/usr/bin/env python

import pika

params = pika.URLParameters('amqp://zorylslw:kx7vaGv56xDwfvr7fMnowpy9zx49CH_7@penguin.rmq.cloudamqp.com/zorylslw')
params.socket_timeout = 5
connection = pika.BlockingConnection(params)
channel = connection.channel()

channel.exchange_declare(exchange='dweis.chat',
                         type='topic')

result = channel.queue_declare(exclusive=True)
queue_name = result.method.queue

channel.queue_bind(exchange='dweis.chat',
                   queue=queue_name,
                   routing_key='#')


def callback(ch, method, properties, body):
    print(" [x] %r:%r:%r" % (method.routing_key, body, type(body)))

channel.basic_consume(callback,
                      queue=queue_name,
                      no_ack=True)
channel.start_consuming()

raw_input("Press enter to exit")