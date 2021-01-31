using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkHud : MonoBehaviour
{
    public string IpAddress;
    public string Port;
    public float GuiOffset;
    private bool _started;
 
    public void Start()
    {
        _started = false;
    }

    public void Connect()
    {
        _started = true;
        NetworkManager.singleton.networkAddress = IpAddress;
        NetworkManager.singleton.networkPort = int.Parse(Port);
        NetworkManager.singleton.StartClient();
    }

    public void Disconnect()
    {
        if (!_started)
            return;

        _started = false;
        NetworkManager.singleton.StopHost();
    }

    public void StartHost()
    {
        _started = true;
        NetworkManager.singleton.networkPort = int.Parse(Port);
        NetworkManager.singleton.StartHost();
    }
}