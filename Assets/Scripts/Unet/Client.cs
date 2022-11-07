using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    public string server = "127.0.0.1";
    public int port = 4444;
    public bool isLocal = false;

    void Start()
    {
        if (isLocal)
        {
            LocalClientSetup();
        }
        else
        {
            ClientSetup();
        }
    }

    private void ClientSetup()
    {
        NetworkClient client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.Connect(server, port);
    }

    private void LocalClientSetup()
    {
        NetworkClient client = ClientScene.ConnectLocalServer();
        client.RegisterHandler(MsgType.Connect, OnConnected);
    }

    private void OnConnected(NetworkMessage msg)
    {
        Debug.Log(string.Format("Client: connected to server {0}", server));
    }

   
}