using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IncomingMessages
{
	[SerializeField] public UnityEvent<List<ObjectData>> OnObjectsReceived = new UnityEvent<List<ObjectData>>();
	[SerializeField] public UnityEvent<ObjectData> OnObjectLocationReceived = new UnityEvent<ObjectData>();
	[SerializeField] public UnityEvent<ObjectEvent> OnObjectEventReceived = new UnityEvent<ObjectEvent>();
	[SerializeField] public UnityEvent<List<LocationData>> OnLocationHistoryReceived = new UnityEvent<List<LocationData>>();

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
				case 1: // Object list
					ReadObjectList(reader);
					break;

				case 2: // Location update
					ReadLocationUpdate(reader);
					break;

				case 3: // Event update
					ReadEventUpdate(reader);
					break;

				case 4: // Location history
					ReadLocationHistory(reader);
					break;

				default:
					Debug.LogWarning($"Unknown message type: {messageType}");
					break;
			}
		}
	}

	private void ReadObjectList(BinaryReader reader)
	{
		int objectCount = reader.ReadInt32(); // Number of objects
		Debug.Log($"Received: object list with {objectCount} objects");
		List<ObjectData> receivedObjects = new List<ObjectData>();

		for (int i = 0; i < objectCount; i++)
		{
			receivedObjects.Add(new ObjectData(
				reader.ReadByte(),
				reader.ReadInt32(),
				new LocationData(
					reader.ReadInt64(),
					reader.ReadInt64())
				));
		}
		OnObjectsReceived.Invoke(receivedObjects);
	}


	private void ReadLocationUpdate(BinaryReader reader)
	{
		ObjectData receivedLocation = new ObjectData(
				reader.ReadByte(),
			reader.ReadInt32(),
			new LocationData(
				reader.ReadInt64(),
				reader.ReadInt64())
			);

		Debug.Log($"Received: #{receivedLocation.ObjectId} location updated to ({receivedLocation.Location.X}, {receivedLocation.Location.Y})");
		OnObjectLocationReceived.Invoke(receivedLocation);
	}


	private void ReadEventUpdate(BinaryReader reader)
	{
		ObjectEvent receivedEvent = new ObjectEvent(
			reader.ReadInt32(),
			reader.ReadByte());

		Debug.Log($"Received: #{receivedEvent.ObjectId} occurred event {receivedEvent.EventId}");
		OnObjectEventReceived.Invoke(receivedEvent);
	}


	private void ReadLocationHistory(BinaryReader reader)
	{
		int locationCount = reader.ReadInt32(); // Number of history entries
		Debug.Log($"Received: location history with {locationCount} points");
		List<LocationData> receivedLocations = new List<LocationData>();

		for (int i = 0; i < locationCount; i++)
		{
			receivedLocations.Add(new LocationData(
				reader.ReadInt64(),
				reader.ReadInt64()));
		}

		OnLocationHistoryReceived.Invoke(receivedLocations);
	}
}

[System.Serializable]
public class LocationData
{
	public LocationData(long _x, long _y)
	{
		X = _x; Y = _y;
	}

	[SerializeField] private long x;
	public long X { get => x; set => x = value; }
	[SerializeField] private long y;
	public long Y { get => y; set => y = value; }
}

[System.Serializable]
public class ObjectLocation
{
	public ObjectLocation(int _objectId, LocationData _location)
	{
		ObjectId = _objectId; Location = _location;
	}

	[SerializeField] private int objectId;
	public int ObjectId { get => objectId; set => objectId = value; }
	[SerializeField] private LocationData _location;
	public LocationData Location { get => _location; set => _location = value; }
}

[System.Serializable]
public class ObjectEvent
{
	public ObjectEvent(int _objectId, byte _eventId)
	{
		ObjectId = _objectId; EventId = _eventId;
	}

	[SerializeField] private int objectId;
	public int ObjectId { get => objectId; set => objectId = value; }
	[SerializeField] private byte eventId;
	public byte EventId { get => eventId; set => eventId = value; }
}

[System.Serializable]
public class ObjectData
{
	public ObjectData(byte _objectType, int _objectId, LocationData _location)
	{
		ObjectType = _objectType; ObjectId = _objectId; Location = _location;
	}

	[SerializeField] private int objectType;
	public int ObjectType { get => objectType; set => objectType = value; }
	[SerializeField] private int objectId;
	public int ObjectId { get => objectId; set => objectId = value; }
	[SerializeField] private LocationData _location;
	public LocationData Location { get => _location; set => _location = value; }
}