import socket
import threading
import json
import requests
import time

# Server Configuration
HOST = '0.0.0.0'  # Listen on all interfaces
PORT = 5000

# Nokia API Configuration
NOKIA_API_URL = "https://location-retrieval.p-eu.rapidapi.com/retrieve"
HEADERS = {
    "content-type": "application/json",
    "X-RapidAPI-Key": "3fb7fd564bmsh44a4133cd4e7dd0p16aef0jsn388388640dda",
    "X-RapidAPI-Host": "location-retrieval.nokia.rapidapi.com"
}
PAYLOAD = {
    "device": {
        "phoneNumber": "+358311100537"
    }
}

# Create a socket
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
PORT = 5000
server_socket.bind((HOST, PORT))
server_socket.listen(5)

print(f"Server started on {HOST}:{PORT}, waiting for clients...")

clients = []


def handle_client(client_socket, address):
    """Handles communication with a single client."""
    print(f"Client connected: {address}")
    clients.append(client_socket)
    try:
        while True:
            # Receive request from the client
            request = client_socket.recv(1024).decode('utf-8')
            if not request:
                break
            print(f"Received request from {address}: {request}")

            if request == "start_location_updates":
                send_location_updates(client_socket)
            else:
                response = {"error": "Unknown command"}
                client_socket.send(json.dumps(response).encode('utf-8'))

    except ConnectionResetError:
        print(f"Client {address} disconnected abruptly.")
    finally:
        print(f"Client {address} disconnected.")
        clients.remove(client_socket)
        client_socket.close()


def send_location_updates(client_socket):
    """Fetches location data from Nokia API and sends it to the client."""
    while True:
        try:
            response = requests.post(NOKIA_API_URL, json=PAYLOAD, headers=HEADERS)
            if response.status_code == 200:
                location_data = response.json()
                client_socket.send(json.dumps(location_data).encode('utf-8'))
                print(f"Sent location data to client: {location_data}")
            else:
                error_message = {"error": f"Failed to retrieve location: {response.status_code}"}
                client_socket.send(json.dumps(error_message).encode('utf-8'))
                print(f"Error: {error_message}")
        except Exception as e:
            error_message = {"error": f"Error fetching location: {str(e)}"}
            client_socket.send(json.dumps(error_message).encode('utf-8'))
            print(f"Error: {error_message}")
            break
        time.sleep(1)


# Accept incoming client connections
def accept_clients():
    while True:
        client_socket, address = server_socket.accept()
        threading.Thread(target=handle_client, args=(client_socket, address)).start()


# Start the server
if __name__ == "__main__":
    accept_clients()
