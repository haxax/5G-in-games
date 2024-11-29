using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IncomingMessages
{
    [SerializeField] public UnityEvent<Vector2> OnLocationReceived = new UnityEvent<Vector2>();
    [SerializeField] public UnityEvent<float> OnLocationRadiusReceived = new UnityEvent<float>();
    [SerializeField] public UnityEvent<List<Vector2>> OnLocationHistoryReceived = new UnityEvent<List<Vector2>>();

    public void ProcessMessage(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        using (BinaryReader reader = new BinaryReader(ms))
        {
            byte messageType = reader.ReadByte(); // First byte: Message type

            switch (messageType)
            {
                case 0:
                    Debug.Log($"Received: {reader.ReadString()}");
                    break;

                case 1: // Location update
                    ReadLocationUpdate(reader);
                    break;

                case 2: // Location history
                    ReadLocationHistory(reader);
                    break;

                default:
                    Debug.LogWarning($"Unknown message type: {messageType}");
                    break;
            }
        }
    }

    private void ReadLocationUpdate(BinaryReader reader)
    {
        LocationData location = new LocationData(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle());

        Debug.Log($"Received: location long:{location.LongitudeLatitude.x} lat:{location.LongitudeLatitude.y} radius:{location.Radius}");
        OnLocationReceived.Invoke(location.LongitudeLatitude.LocationToMeters());
        OnLocationRadiusReceived.Invoke(location.Radius * WorldManager.Instance.WorldScale);
    }


    private void ReadLocationHistory(BinaryReader reader)
    {
        int locationCount = reader.ReadInt32();
        Debug.Log($"Received: location history with {locationCount} points");
        List<Vector2> receivedLocations = new List<Vector2>();

        for (int i = 0; i < locationCount; i++)
        {
            LocationData visitedLocation = new LocationData(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle());
            receivedLocations.Add(visitedLocation.LongitudeLatitude.LocationToMeters());
        }

        OnLocationHistoryReceived.Invoke(receivedLocations);
    }
}

[System.Serializable]
public class LocationData
{
    public LocationData(float _longitude, float _latitude, float _radius)
    {
        LongitudeLatitude = new Vector2(_longitude, _latitude);
        Radius = _radius;
    }

    [SerializeField] private Vector2 longitudeLatitude;
    public Vector2 LongitudeLatitude { get => longitudeLatitude; set => longitudeLatitude = value; }

    [SerializeField] private float radius;
    public float Radius { get => radius; set => radius = value; }
}