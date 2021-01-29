using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ServerCore : MonoBehaviour
{
    private EntitiesController entitiesController;
    void OnServerInitialized()
    {
        entitiesController = new EntitiesController();
    }

    
}
