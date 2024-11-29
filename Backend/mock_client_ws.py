import socket
import json

HOST = '127.0.0.1'  # Server address
PORT = 5000         # Server port

# Connect to the server
client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client_socket.connect((HOST, PORT))

print("Connected to server")

# Send command to start receiving location updates
client_socket.send("start_location_updates".encode('utf-8'))

try:
    while True:
        # Receive updates from the server
        data = client_socket.recv(1024).decode('utf-8')
        if not data:
            break
        print(f"Received from server: {data}")
except KeyboardInterrupt:
    print("Client stopped.")
finally:
    client_socket.close()
