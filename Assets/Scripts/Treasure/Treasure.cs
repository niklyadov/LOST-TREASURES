using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Treasure : MonoBehaviour
{
    public int Price;

    public GameObject LastOwner { get;  private set; } = null;
    public GameObject Owner { get;  private set; } = null;

    private Rigidbody _rigidbody;

    public void Pickup(GameObject newOwner)
    {
        Owner = newOwner;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (_rigidbody != null)
        {
            Destroy (_rigidbody);
            _rigidbody = null;
        }
    }
    
    public void Drop()
    {
        LastOwner = Owner;
        Owner = null;
        
        _rigidbody = gameObject.AddComponent<Rigidbody>();
    }
}
