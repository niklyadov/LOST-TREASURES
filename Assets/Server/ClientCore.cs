using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientCore : NetworkBehaviour
{
    public string nickname = "kekich";

    public override void OnStartClient()
    {

        ServerCore.entitiesController.addPlayer(new EntityPlayer(nickname));
    }
}
