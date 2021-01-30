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
    public void OnGUI()
    {
        GUILayout.Space(GuiOffset);
        if (_started)
        {
           if (GUILayout.Button("Disconnect"))
           {
               _started = false;
               NetworkManager.singleton.StopHost();
           } 
           return;
        }
        
        if (GUILayout.Button("Host"))
        {
            _started = true;
            NetworkManager.singleton.networkPort = int.Parse(Port);
            NetworkManager.singleton.StartHost();
        }
        if (GUILayout.Button("Connect"))
        {
            _started = true;
            NetworkManager.singleton.networkAddress = IpAddress;
            NetworkManager.singleton.networkPort = int.Parse(Port);
            NetworkManager.singleton.StartClient();
        }
    }
}