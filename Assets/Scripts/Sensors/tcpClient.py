import serial
import socket

s_left = serial.Serial('COM8')
s_right = serial.Serial('COM9')

# Create a TCP client
ip = "192.168.178.40"
port = 5005
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect((ip, port))

while True:
    data_left = s_left.readline()
    data_right = s_right.readline()

    # if data_left contains the word "shake" send a message to the server
    if "shake" in data_left:
        client.send("shakeleft".encode())

    # if data_right contains the word "shake" send a message to the server
    if "shake" in data_right:
        client.send("shakeright".encode())
