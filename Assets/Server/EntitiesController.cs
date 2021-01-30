
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesController : MonoBehaviour
{

    public List<EntityPlayer> playersList;

    
    public EntitiesController()
    {
        this.playersList = new List<EntityPlayer>();
    }

    public void spawnEntityPlayer(EntityPlayer player, Vector3 pos) {
        //need to do
        string data ="i'm need data from file";
        int id = 0; //i'm need to get new id for everybody
        if (!player.loadData(data))
        {
            player.initNewPlayer(id);
            player.updateCords(pos);
        }
    }

    public void addPlayer(EntityPlayer player) {
        this.playersList.Add(player);
    }

    public void removePlayer(EntityPlayer player)
    {
        this.playersList.Remove(player);
    }

    public int getPlayersSize()
    {
        return this.playersList.Count;
    }

    public void saveData()
    {
        string data;
        for (int i = 0; i < this.getPlayersSize(); i++)
        {
            data = this.playersList[i].writeData();
            //need to write it to file
        }
    }
    public void onUpdate() {// updating for all players
        for (int i = 0; i < this.getPlayersSize(); i++) {
            this.playersList[i].onUpdatePlayer();
        }
    }
}
