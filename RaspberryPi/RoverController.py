#!/usr/bin/env python

import serial
import glob

port_list = glob.glob('/dev/ttyS*')
if not len(port_list) == 1:
    print "Unexpected number of ports"

port = serial.Serial(port_list[0], 9600)
port.open()
