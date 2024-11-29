using UnityEngine;
using WebSocketSharp;
using System;

public class LocationWebSocketClient : MonoBehaviour
{
    private WebSocket ws;

    // URL of the WebSocket server
    [SerializeField] private string serverURL = "ws://127.0.0.1:5000/socket.io/?EIO=3&transport=websocket";

    void Start()
    {
        // Initialize WebSocket connection
        ws = new WebSocket(serverURL);

        // Handle connection established
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected to the Python WebSocket server.");
            // Send a request to start receiving location updates
            ws.Send("42[\"start_location_updates\"]");
        };

        // Handle incoming messages
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log($"Received message from server: {e.Data}");
            ProcessData(e.Data);
        };

        // Handle disconnection
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Disconnected from WebSocket server.");
        };

        // Handle errors
        ws.OnError += (sender, e) =>
        {
            Debug.Log($"WebSocket error: {e.Message}");
        };

        // Connect to the server
        ws.Connect();
    }

    void OnApplicationQuit()
    {
        // Clean up WebSocket connection
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    // Process the incoming location data
    private void ProcessData(string data)
    {
        /*string msgId = data[0];
        switch (msgId)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }*/
        // LocationInfo location = JsonUtility.FromJson<LocationInfo>(data);
    }
}

// Example class for location info (if deserializing JSON data)
[System.Serializable]
public class LocationInfo
{
    public float latitude;
    public float longitude;
    public float radius;
}