using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerCore : NetworkBehaviour
{
    [SyncVar]
    public static EntitiesController entitiesController;

    public override void OnStartServer()
    {
     
        entitiesController = new EntitiesController();
    }


    
    public void OnStopServer()
    {

        entitiesController.saveData();
    }


    [ServerCallback]
    void Update()
    {
        // engine invoked callback - will only run on server
    }
}
