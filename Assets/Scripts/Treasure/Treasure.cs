using UnityEngine;
using UnityEngine.Networking;

public class Treasure : NetworkBehaviour
{
    public int Price;
    private Rigidbody _rigidbody;
    
    [ClientRpc]
    public void RpcPickup(NetworkInstanceId playerId)
    {
        Debug.LogError("Pick up player " + playerId + " treasure " + netId);
        if (_rigidbody != null)
        {
            Destroy(_rigidbody);
            _rigidbody = null;
        }

        transform.parent = ClientScene.FindLocalObject(playerId).transform;
    }

    [ClientRpc]
    public void RpcDrop()
    {
        Debug.LogError("Drop treasure " + netId);
        transform.parent = null;
        _rigidbody = gameObject.AddComponent<Rigidbody>();
    }
}
