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
            //ws.Send("42[\"start_location_updates\"]");
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
            ws.Close();
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
    private void OnDestroy()
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
        try
        {
            Root root = JsonUtility.FromJson<Root>(data);
           
            WorldManager.Instance.Queue5gLocation(new Vector2((float)root.location.area.center.longitude, (float)root.location.area.center.latitude).LocationToMeters(), root.location.area.radius);
        }
        catch (Exception ex) { Debug.Log(ex); }


    }
}

[System.Serializable]
public class Location
{
    public string lastLocationTime;
    public Area area;
}

[System.Serializable]
public class Area
{
    public string areaType;
    public Center center;
    public float radius;
}

[System.Serializable]
public class Center
{
    public double latitude;
    public double longitude;
}

[System.Serializable]
public class Root
{
    public Location location;
}