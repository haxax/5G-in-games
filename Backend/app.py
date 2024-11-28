from flask import Flask, request, jsonify
import requests

app = Flask(__name__)


@app.route('/api/location', methods=['POST'])
def retrieve_location():
    # Extract payload from the incoming request
    incoming_data = request.json

    # Base URL for the Nokia API
    url = "https://location-retrieval.p-eu.rapidapi.com/retrieve"

    # Payload to send to the Nokia API
    payload = {
        "device": {
            "phoneNumber": incoming_data.get("phoneNumber", "+358311100537"),
            "networkAccessIdentifier": incoming_data.get("networkAccessIdentifier", "device@testcsp.net"),
            "ipv4Address": {
                "publicAddress": incoming_data.get("publicAddress", "217.140.216.37"),
                "privateAddress": incoming_data.get("privateAddress", "127.0.0.1"),
                "publicPort": incoming_data.get("publicPort", 80),
            },
        },
        "maxAge": incoming_data.get("maxAge", 3600)
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


if __name__ == '__main__':
    # Run the Flask app locally
    app.run(host='0.0.0.0', port=5000, debug=True)
