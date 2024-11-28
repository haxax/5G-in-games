import requests

url = "http://127.0.0.1:5000/api/location"

payload = {
    "device": {
        "phoneNumber": "+358311100537"
    },
    "maxAge": 3600
}

headers = {"content-type": "application/json"}

response = requests.post(url, json=payload, headers=headers)
print("Status Code:", response.status_code)
print("Response:", response.json())
