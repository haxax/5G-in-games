using System;
using System.Collections.Generic;
using UnityEngine;

public static class OutgoingMessages
{
	public static void SendMessage(string msg, byte msgId = 0)
	{
		Debug.Log($"Sending: ({msgId}){msg}");
		byte[] data = MsgSerialization.SerializeMessage(msg, msgId);
		TcpClientUnity.instance.SendMessageToClient(data);
	}
	public static void SendRequest(byte requestId)
	{
		Debug.Log($"Sending: request {requestId}");
		byte[] data = MsgSerialization.SerializeGenericRequest(requestId);
		TcpClientUnity.instance.SendMessageToClient(data);
	}
}