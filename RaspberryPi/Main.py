#!/usr/bin/env python3

import serial
import serial.tools.list_ports

print("Welcome!")
for port in serial.tools.list_ports.comports():
    print(port)

selected_port_name = input("Select port:\n")
