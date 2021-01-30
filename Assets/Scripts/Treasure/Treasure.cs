﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Treasure : MonoBehaviour
{
    public int Price;

    public GameObject Owner { get;  private set; } = null;

    private Rigidbody _rigidbody;

    public void Pickup(GameObject newOwner)
    {
        Owner = newOwner;
        
        if (_rigidbody != null)
        {
            Destroy (_rigidbody);
            _rigidbody = null;
        }
    }
    
    public void Drop()
    {
        Owner = null;
        
        _rigidbody = gameObject.AddComponent<Rigidbody>();
    }
}