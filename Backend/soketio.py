from flask import Flask, request, jsonify
from flask_socketio import SocketIO, emit
import requests

app = Flask(__name__)
socketio = SocketIO(app)  # Initialize SocketIO with the Flask app

# Root route for GET requests
@app.route('/', methods=['GET'])
def home():
    return jsonify({
        "message": "Welcome to the Location Retrieval API",
        "usage": "Send a POST request to /api/location with the required payload."
    })

# Route to handle POST requests for location retrieval (for test.py)
@app.route('/api/location', methods=['POST'])
def retrieve_location():
    # Extract payload from the incoming request
    incoming_data = request.json

    # Validate incoming data
    if not incoming_data:
        return jsonify({"error": "Missing payload"}), 400

    # Base URL for the Nokia API
    url = "https://location-retrieval.p-eu.rapidapi.com/retrieve"

    # Payload to send to the Nokia API
    payload = {
        "device": {
            "phoneNumber": incoming_data.get('phoneNumber', "+358311100537"),
            "networkAccessIdentifier": incoming_data.get('networkAccessIdentifier', "device@testcsp.net"),
            "ipv4Address": {
                "publicAddress": incoming_data.get('publicAddress', "217.140.216.37"),
                "privateAddress": incoming_data.get('privateAddress', "192.168.32.1"),
                "publicPort": incoming_data.get('publicPort', 80),
            },
        },
        "maxAge": incoming_data.get('maxAge', 3600)
    }

    # Headers for the Nokia API request
    headers = {
        "content-type": "application/json",
        "X-RapidAPI-Key": "3fb7fd564bmsh44a4133cd4e7dd0p16aef0jsn388388640dda",
        "X-RapidAPI-Host": "location-retrieval.nokia.rapidapi.com"
    }

    try:
        # Make the POST request to the Nokia API
        response = requests.post(url, json=payload, headers=headers)

        # Return the Nokia API response to the client
        return jsonify({
            "status_code": response.status_code,
            "response": response.json()
        }), response.status_code
    except Exception as e:
        # Handle any exceptions and return an error message
        return jsonify({"error": str(e)}), 500

# WebSocket event handler for connection
@socketio.on('connect')
def handle_connect():
    print("Client connected")
    emit('response', {'message': 'Connected to server'})

# WebSocket event handler for location request
@socketio.on('get_location')
def handle_get_location(incoming_data):
    # Extract data from incoming WebSocket message
    phone_number = incoming_data.get('phoneNumber', '+358311100537')
    network_access_identifier = incoming_data.get('networkAccessIdentifier', 'device@testcsp.net')
    public_address = incoming_data.get('publicAddress', '217.140.216.37')
    private_address = incoming_data.get('privateAddress', '127.0.0.1')
    public_port = incoming_data.get('publicPort', 80)
    max_age = incoming_data.get('maxAge', 3600)

    # Prepare payload for Nokia API
    payload = {
        "device": {
            "phoneNumber": phone_number,
            "networkAccessIdentifier": network_access_identifier,
            "ipv4Address": {
                "publicAddress": public_address,
                "privateAddress": private_address,
                "publicPort": public_port
            },
        },
        "maxAge": max_age
    }

    # Set headers for the Nokia API request
    headers = {
        "content-type": "application/json",
        "X-RapidAPI-Key": "3fb7fd564bmsh44a4133cd4e7dd0p16aef0jsn388388640dda",
        "X-RapidAPI-Host": "location-retrieval.nokia.rapidapi.com"
    }

    # Make the POST request to the Nokia API
    url = "https://location-retrieval.p-eu.rapidapi.com/retrieve"
    try:
        response = requests.post(url, json=payload, headers=headers)

        # If request was successful, emit response to client
        if response.status_code == 200:
            emit('response', {'status': 'success', 'data': response.json()})
        else:
            emit('response', {'status': 'failure', 'message': response.json()})
    except Exception as e:
        emit('response', {'status': 'error', 'message': str(e)})

# Start the Flask app with WebSocket support
if __name__ == '__main__':
    socketio.run(app, host='0.0.0.0', port=5000, debug=True)
