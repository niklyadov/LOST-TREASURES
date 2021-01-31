using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public enum TeamColor
    {
        Blue,
        Red
    }

    public class Team : NetworkBehaviour
    {
        [SyncVar] 
        public int Score;
        public TeamColor Color;
        public List<Submarine> Members;

        [Command]
        public void CmdJoinTeam(NetworkInstanceId playerId)
        {
            var player = ClientScene.FindLocalObject(playerId).GetComponent<Submarine>();
            Members.Add(player);
            player.Team = Color;

            Debug.LogError(Color.ToString() + " Team: " + Members.Count);
        }
    }
}
