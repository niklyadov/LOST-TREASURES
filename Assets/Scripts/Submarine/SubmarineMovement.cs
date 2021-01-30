using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;
    
    [SerializeField]
    private float horizontalSpeed;
    
    [SerializeField]
    private float verticalSpeed;
    
    [SerializeField]
    private float rotationSpeed;
    
    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Forward(int modify)
    {
        _rigidbody.AddForce(modify * horizontalSpeed * Time.deltaTime * _transform.forward);
    }
    
    public void Rotate(int modify)
    {
        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(new Vector3(
            0,
            modify* verticalSpeed * Time.deltaTime, 
            0
        )));
    }
    
    public void Raise(int modify)
    {
        _rigidbody.AddForce(modify * rotationSpeed * Time.deltaTime * _transform.up);
    }
}
