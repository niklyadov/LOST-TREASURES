using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Base : NetworkBehaviour
{
    public TeamColor Team;

    private void OnTriggerEnter(Collider other)
    {
        var treasure = other.GetComponent<Treasure>();
        
        if (treasure == null)
            return;
        
        if (treasure.Owner != null)
            return;

        CmdAddScore(treasure.Price);        
        Destroy(treasure.gameObject);
    }

    [Command]
    private void CmdAddScore(int points)
    {
        if (isServer)
            GameObject.FindGameObjectWithTag("Global").GetComponent<Match>().CmdAddPoints(Team, points);
    }
}