using System;
using System.Collections.Generic;
using System.IO;

public static class MsgSerialization
{
	public static byte[] SerializeMessage(string msg, byte msgId = 0)
	{
		using (MemoryStream ms = new MemoryStream())
		using (BinaryWriter writer = new BinaryWriter(ms))
		{
			writer.Write((byte)msgId);
			writer.Write(msg);

			return ms.ToArray();
		}
	}



	public static byte[] SerializeGenericRequest(byte requestId)
	{
		using (MemoryStream ms = new MemoryStream())
		using (BinaryWriter writer = new BinaryWriter(ms))
		{
			writer.Write((byte)requestId);
			return ms.ToArray();
		}
	}
}