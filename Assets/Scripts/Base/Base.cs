using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public Team Team;
    
    //private List<Treasure> _treasures = new List<Treasure>();
    
    [SerializeField]
    private int _totalCost = 0;

    public int TotalCost
    {
        get => _totalCost;
    }

    private void OnTriggerEnter(Collider other)
    {
        var treasure = other.GetComponent<Treasure>();
        
        if (treasure == null)
            return;
        
        if (treasure.Owner != null)
            return;
        
        //_treasures.Add(treasure);
        _totalCost += treasure.Price;

        var controller = GameController.GetInstance();
        
        if (Team == Team.Blue)
            controller.BlueTeamScore += treasure.Price;
        else
            controller.RedTeamScore += treasure.Price;
        
        Destroy(treasure.gameObject);
    }
    
    /* TODO: кража ресурсов
     * private void OnTriggerExit(Collider other)
    {
        var treasure = other.GetComponent<Treasure>();
        
        if (treasure == null)
            return;
        
        if (treasure.Owner != null)
            return;

        _treasures.Remove(treasure);
        _totalCost -= treasure.Price;
    }
     */
}