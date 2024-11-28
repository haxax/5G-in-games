using UnityEngine;

using Unity.Networking.Transport;
using Unity.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private string ipAddress = "127.0.0.0";
    [SerializeField] private ushort port = 7777;

    public string IpAddress { get => ipAddress; set => ipAddress = value; }
    public ushort Port { get => port; set => port = value; }


    NetworkDriver m_Driver;
    NetworkConnection m_Connection;

    void Start()
    {
        Debug.Log("Creating driver...");
        m_Driver = NetworkDriver.Create();
        Debug.Log("parsing address...");
        var endpoint = NetworkEndpoint.Parse(ipAddress, Port);
        Debug.Log("Connecting...");
        m_Connection = m_Driver.Connect(endpoint);
        Debug.Log("Connected?...");
    }

    void OnDestroy()
    { m_Driver.Dispose(); }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            Debug.Log("Connection not created..."); return;
        }
        Debug.Log("Connection created...");
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server.");

                uint value = 1;
                m_Driver.BeginSend(m_Connection, out var writer);
                writer.WriteUInt(value);
                m_Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                ReadMsg(stream);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server.");
                m_Connection = default;
            }
        }
    }

    private void ReadMsg(DataStreamReader stream)
    {
        byte msgType = stream.ReadByte();
        switch (msgType)
        {
            case 0:     // Msg
                MsgReadText(stream);
                break;
            case 2:     // Location
                MsgReadLocation(stream);
                break;
            case 3:     // Location history
                MsgReadLocationHistory(stream);
                break;
            default:
                Debug.LogWarning($"Invalid msg type from server: {msgType} !");
                break;
        }
    }

    private void MsgReadText(DataStreamReader stream)
    {
        Debug.Log(stream.ReadFixedString4096());
    }

    private void MsgReadLocation(DataStreamReader stream)
    {
        byte objType = stream.ReadByte();
        int objId = stream.ReadInt();
        Debug.Log($"received location of {objId} which is {objType} type.");
        Vector2 realLifeLocation = new Vector2(stream.ReadInt(), stream.ReadInt());
        WorldManager.Instance.Update5GLocation(realLifeLocation.LocationToMeters());
    }

    private void MsgReadLocationHistory(DataStreamReader stream)
    {
        List<Vector2> history = new List<Vector2>();
        while (stream.Length > 0)
        {
            history.Add(new Vector2(stream.ReadFloat(), stream.ReadFloat()).LocationToMeters());
        }
        // TODO something
    }
}