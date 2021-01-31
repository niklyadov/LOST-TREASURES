using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TreasureSpawn : NetworkBehaviour
{
    private Dictionary<GameObject, Vector3> _treasurePoints;
    void Start()
    {
        _treasurePoints = new Dictionary<GameObject, Vector3>();
        foreach (var treasure in GetComponentsInChildren<Treasure>())
            _treasurePoints[treasure.gameObject] = treasure.transform.position;
    }

    [ClientRpc]
    public void RpcReset()
    {
        foreach (var treasure in _treasurePoints)
        {
            treasure.Key.transform.position = treasure.Value;
            treasure.Key.SetActive(true);
        }
            
    }
}
