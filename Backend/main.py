import requests

url = "https://location-retrieval.p-eu.rapidapi.com/retrieve"

payload = {
    "device": {
        "phoneNumber": "+358311100537",
        "networkAccessIdentifier": "device@testcsp.net",
        "ipv4Address": {
            "publicAddress": "217.140.216.37",
            "privateAddress": "127.0.0.1",
            "publicPort": 80
        },
    },
    "maxAge": 3600
}

headers = {
	"content-type": "application/json",
	"X-RapidAPI-Key": "3fb7fd564bmsh44a4133cd4e7dd0p16aef0jsn388388640dda",
	"X-RapidAPI-Host": "location-retrieval.nokia.rapidapi.com"
}

response = requests.post(url, json=payload, headers=headers)

print("Status Code:", response.status_code)
print("Response:", response.json())
