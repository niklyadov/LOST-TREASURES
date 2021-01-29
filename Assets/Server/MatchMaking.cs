using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMaking : MonoBehaviour
{
    public bool start;
    public List<EntityPlayer> firstTeam, secondTeam;
    public int fscore, sscore;// teams score
    public Vector3 fspawn, sspawn;// teams spawn coords
    public int countRounds;

    public MatchMaking(Vector3 fspawn, Vector3 sspawn) {
        this.start = false;
        this.firstTeam = new List<EntityPlayer>();
        this.secondTeam = new List<EntityPlayer>();
        this.setSpawn(fspawn, sspawn);
        this.countRounds = 0;
    }
    public void setSpawn(Vector3 fspawn, Vector3 sspawn)
    {
        this.fspawn = fspawn;
        this.sspawn = sspawn;
    }


    public void startMatch() {
    // this need to start in new thread
    }

    public void update() { }

    public void stop() { }
}
