using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EntityPlayer : MonoBehaviour
{
    public int idItemHand;
    public static string spliter = ":";
    public int id = 0;//useful, but in future it may become a uuid
    public Vector3 pos, yaw;
    private int score;
    private bool isDead;//thats like visible or not
    public string name;
    public int health;

    public EntityPlayer(string name) {
        this.name = name;
    }

    public void initNewPlayer(int id) {
        this.idItemHand = -1;
        this.id = id;
        this.score = 0;
        this.isDead = false;
        this.health = 100;
    }


    public void onUpdatePlayer() {
        // logic about dead when health < 0 etc
        if (this.health <= 0)
            this.setDead(true);
    }

 

    public bool loadData(string data) {
        string[] arr = data.Split(spliter.ToCharArray());
        if (arr.Length == 9)
        {
            this.id = Int32.Parse(arr[0]);
            this.name = arr[1];
            this.score = Int32.Parse(arr[2]);
            this.isDead = "0" == arr[3];
            updateCords(Int32.Parse(arr[4]), Int32.Parse(arr[5]), Int32.Parse(arr[6]));
            this.health = Int32.Parse(arr[7]);
            this.idItemHand = Int32.Parse(arr[8]);
            return true;
        }
        else
            ;//error
        return false;
 
    }

    public string writeData() {
        string res = "";
        addDataToWrite(res,this.id);
        addDataToWrite(res, this.name);
        addDataToWrite(res, this.score);
        addDataToWrite(res, this.isDead);
        addDataToWrite(res, this.pos.x);
        addDataToWrite(res, this.pos.y);
        addDataToWrite(res, this.pos.z);
        addDataToWrite(res, this.health);
        res += this.idItemHand;
        return res;
    }







    // Start of updaters, getters and setters
    public void setDead(bool dead)
    {
        this.isDead = dead;
    }

    public void addDataToWrite(string data, float value)
    {
        data += value + spliter;
    }
    public void addDataToWrite(string data, bool value)
    {
        data += value + spliter;
    }
    public void addDataToWrite(string data, int value)
    {
        data += value + spliter;
    }
    public void addDataToWrite(string data, string value) { 
        data+= value + spliter;
    }

    public int getScore()
    {
        return this.score;
    }

    public void setScore(int score)
    {
        this.score = score;
    }

    public void updateCords(Vector3 vec)
    {
        this.pos = vec;
    }
    public void updateCords(int x, int y, int z) {
        this.pos = new Vector3(x,y,z);
    }

    public void updateYaw(int x, int y, int z)
    {
        this.yaw = new Vector3(x, y, z);
    }

    public void updateYaw(Vector3 vec)
    {
        this.yaw = vec;
    }

}
