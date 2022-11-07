using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    public int listenPort = 4444;

    void Start()
    {
        ServerSetup();
    }

    private void ServerSetup()
    {
        NetworkServer.Listen(listenPort);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
    }

    private void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Server: connection happen");
    }
}
