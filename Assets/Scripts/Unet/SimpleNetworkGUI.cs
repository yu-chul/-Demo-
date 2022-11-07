using UnityEngine;
using UnityEngine.Networking;

public class SimpleNetworkGUI : MonoBehaviour
{
    bool isHaveNetworkRole = false;
    public string server = "127.0.0.1";
    public int port = 4444;


    void Start()
    {
        isHaveNetworkRole = false;
    }

    private void OnDisconnected(NetworkMessage msg)
    {
        isHaveNetworkRole = false;
        Application.LoadLevel(Application.loadedLevel);
        
    }

    void OnGUI()
    {
 

        if (isHaveNetworkRole)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 12, 160, 24), "Stop"))
            {
                NetworkManager.singleton.StopServer();
                NetworkManager.singleton.StopClient();
                OnDisconnected(null);
            }
            return;
        }

        if (GUI.Button(new Rect(Screen.width / 2f - 80, Screen.height / 2 - 12, 160, 24), "Start Server"))
        {
            ServerSetup();
            isHaveNetworkRole = true;
        }

        if (GUI.Button(new Rect(Screen.width / 2f - 80, Screen.height / 2 + 24, 160, 24), "Start Client"))
        {
            if (isHaveNetworkRole)
            {
                LocalClientSetup();
            }
            else
            {
                ClientSetup();
            }
            isHaveNetworkRole = true;
        }
    }

    private void ServerSetup()
    {
        NetworkServer.Listen(port);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
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











/**
 * void OnGUI()
    {
 

        if (isHaveNetworkRole)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 12, 160, 24), "Stop"))
            {
                NetworkManager.singleton.StopServer();
                NetworkManager.singleton.StopClient();
                OnDisconnected(null);
            }
            return;
        }

        if (GUI.Button(new Rect(Screen.width / 2f - 80, Screen.height / 2 - 12, 160, 24), "Start Server"))
        {
            isHaveNetworkRole = NetworkManager.singleton.StartServer();
        }

        if (GUI.Button(new Rect(Screen.width / 2f - 80, Screen.height / 2 + 24, 160, 24), "Start Client"))
        {
            var client = NetworkManager.singleton.StartClient();
            client.RegisterHandler(MsgType.Disconnect, OnDisconnected);
            isHaveNetworkRole = true;
        }
    }
 */
