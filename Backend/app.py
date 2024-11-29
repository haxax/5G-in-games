import eventlet
eventlet.monkey_patch()

from flask import Flask, jsonify, request
from flask_socketio import SocketIO, emit
import requests
import time
import threading

# Initialize Flask app and SocketIO
app = Flask(__name__)
socketio = SocketIO(app, cors_allowed_origins="*")

# Nokia API base URL
NOKIA_API_URL = "https://location-retrieval.p-eu.rapidapi.com/retrieve"

# Nokia API headers
HEADERS = {
    "content-type": "application/json",
    "X-RapidAPI-Key": "3fb7fd564bmsh44a4133cd4e7dd0p16aef0jsn388388640dda",
    "X-RapidAPI-Host": "location-retrieval.nokia.rapidapi.com"
}

# payload for Nokia API
PAYLOAD = {
    "device": {
        "phoneNumber": "+358311100537"
    },
    "maxAge": 60
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

# WebSocket: Client connected
@socketio.on('connect')
def handle_connect():
    print("Unity client connected")
    emit('message', {'data': 'Connected to the server!'})

# WebSocket: Client disconnected
@socketio.on('disconnect')
def handle_disconnect():
    print("Unity client disconnected")

# WebSocket: Handle location request
@socketio.on('start_location_updates')
def handle_location_updates():
    print("Received request to start location updates")
    def send_location_updates():
        while True:
            try:
                # Fetch location data from Nokia API
                response = requests.post(NOKIA_API_URL, json=PAYLOAD, headers=HEADERS)
                if response.status_code == 200:
                    location_data = response.json()
                    # Emit the location data to the Unity client
                    socketio.emit('location_update', {'location': location_data})
                else:
                    socketio.emit('error', {'message': f"Failed to retrieve location: {response.status_code}"})
            except Exception as e:
                socketio.emit('error', {'message': f"Error: {str(e)}"})

            time.sleep(1)  # Send updates every 1 second

    # Run the update loop in a separate thread
    thread = threading.Thread(target=send_location_updates)
    thread.daemon = True
    thread.start()


if __name__ == '__main__':
    socketio.run(app, host='0.0.0.0', port=5000, debug=True)
