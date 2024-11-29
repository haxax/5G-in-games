using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;

public class TcpClientUnity : MonoBehaviour
{
	public string debugField = "Insert msg here";
	[Space(30)]

	[SerializeField] private string serverIP = "127.0.0.1";
	public string ServerIP { get => serverIP; set { serverIP = value; } }
	[SerializeField] private int serverPort = 5000;
	public int ServerPort { get => serverPort; set { serverPort = value; } }
	[SerializeField] private string playerId = "+358 40 0000000";
	public string PlayerId { get => playerId; set { playerId = value; } }
	[SerializeField] private IncomingMessages incomingMessages = new IncomingMessages();

	public static TcpClientUnity instance;

	TcpClient client;
	StreamReader reader;
	StreamWriter writer;
	NetworkStream stream;
	bool isConnected = false;

	private void Awake()
	{
		if (instance == null)
		{ instance = this; }
		else if (instance != this) { Destroy(gameObject); }
	}
	void Start()
	{
		ConnectToServer();
	}

	void Update()
	{
		if (isConnected && client.Connected)
		{
			if (Input.GetKeyDown(KeyCode.M))
			{ SendMessageToServer(debugField); }
			if (Input.GetKeyDown(KeyCode.P))
			{ OutgoingMessages.SendMessage(PlayerId, 1); }
			if (Input.GetKeyDown(KeyCode.R))
			{ OutgoingMessages.SendRequest(2); }
		}
	}

	async void ConnectToServer()
	{
		try
		{
			client = new TcpClient(serverIP, serverPort);
			stream = client.GetStream();

			reader = new StreamReader(stream, Encoding.UTF8);
			writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

			isConnected = true;
			Debug.Log("Connected to the server.");

			await ListenForMessagesAsync();
		}
		catch (System.Exception e)
		{
			Debug.LogError($"Connection error: {e.Message}");
		}
	}
	async Task ListenForMessagesAsync()
	{
		while (isConnected && client.Connected)
		{
			try
			{
				// Check if there is data available
				if (stream.DataAvailable && client.Available > 0)
				{

					// Read the incoming message
					byte[] buffer = new byte[client.Available];
					stream.Read(buffer, 0, buffer.Length);
					stream.Flush();

					string msg = Encoding.UTF8.GetString(buffer);
					Debug.Log($"incoming: {msg}!");
					// Process the received message
					// incomingMessages.ProcessMessage(buffer);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning($"Error reading from server: {e.Message}");
			}

			await Task.Delay(10);
		}
	}

	public void SendMessageToClient(byte[] data)
	{
		try
		{
			stream = client.GetStream();
			stream.Write(data, 0, data.Length);
			stream.Flush();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error sending message to client: {ex.Message}");
		}
	}
	void SendMessageToServer(string message)
	{
		if (isConnected && client.Connected)
		{
            byte[] arr = Encoding.UTF8.GetBytes(message);
			string utf8 = Encoding.UTF8.GetString(arr);
			writer.WriteLine(utf8);
			Debug.Log($"Sent to server: {utf8}.");
		}
	}

	void OnApplicationQuit()
	{
		if (isConnected)
		{
			stream?.Close();
			writer?.Close();
			reader?.Close();
			client?.Close();
			Debug.Log("Disconnected from server.");
		}
	}
}