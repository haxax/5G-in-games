import json
from flask import Flask, jsonify, request
from flask_sock import Sock
from flask_cors import CORS
import requests
import time
import threading

# Initialize Flask app and Sock
app = Flask(__name__)
CORS(app)
sock = Sock(app)

# Nokia API base URL
NOKIA_API_URL = "https://location-retrieval.p-eu.rapidapi.com/retrieve"

# Nokia API headers
HEADERS = {
    "content-type": "application/json",
    "X-RapidAPI-Key": "3fb7fd564bmsh44a4133cd4e7dd0p16aef0jsn388388640dda",  # Replace with your actual RapidAPI key
    "X-RapidAPI-Host": "location-retrieval.nokia.rapidapi.com"
}

# Payload for Nokia API
PAYLOAD = {
    "device": {
        "phoneNumber": "+358311100537"
    }
    # "maxAge": 3600  # Uncomment if needed
}

# Handle GET requests to the root endpoint
@app.route('/', methods=['GET'])
def home():
    return jsonify({
        "message": "Welcome to the 5G Location Retrieval API",
        "usage": "Use WebSocket for real-time updates or POST to /api/location for one-time retrieval."
    })

# Route to handle POST requests for location retrieval
@app.route('/api/location', methods=['POST'])
def retrieve_location():
    # Log incoming request
    print("Received POST request at /api/location")

    # Fetch location data from Nokia API
    try:
        response = requests.post(NOKIA_API_URL, json=PAYLOAD, headers=HEADERS)
        if response.status_code == 200:
            location_data = response.json()
            return jsonify(location_data), 200
        else:
            return jsonify({"error": f"Failed to retrieve location: {response.status_code}"}), response.status_code
    except Exception as e:
        return jsonify({"error": str(e)}), 500

# WebSocket route
@sock.route('/ws')
def websocket(ws):
    print("Unity client connected")
    ws.send(json.dumps({'data': 'Connected to the server!'}))

    def send_location_updates():
        while True:
            try:
                # Fetch location data from Nokia API
                response = requests.post(NOKIA_API_URL, json=PAYLOAD, headers=HEADERS)
                if response.status_code == 200:
                    location_data = response.json()
                    # Send the location data to the client
                    ws.send(json.dumps({'location': location_data}))
                else:
                    ws.send(json.dumps({'error': f"Failed to retrieve location: {response.status_code}"}))
            except Exception as e:
                ws.send(json.dumps({'error': f"Error: {str(e)}"}))
            time.sleep(1)  # Send updates every 1 second

    # Start the update loop in a separate thread
    thread = threading.Thread(target=send_location_updates)
    thread.daemon = True
    thread.start()

    # Keep the connection open and listen for incoming messages
    try:
        while True:
            data = ws.receive()
            if data is None:
                # Connection closed
                print("Unity client disconnected")
                break
            else:
                print(f"Received data: {data}")
                # Handle received data if necessary
    except Exception as e:
        print(f"WebSocket error: {str(e)}")

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
