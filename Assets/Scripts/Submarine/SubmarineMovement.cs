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
        _rigidbody.AddForce(modify * horizontalSpeed * Time.fixedDeltaTime * _transform.forward);
    }
    
    public void Rotate(int modify)
    {
        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(new Vector3(
            0,
            modify* rotationSpeed * Time.fixedDeltaTime, 
            0
        )));
    }
    
    public void Raise(int modify)
    {
        // limits
        if (modify == 1 && transform.position.x > 64)
            return;
        
        
        _rigidbody.AddForce(modify * verticalSpeed * Time.fixedDeltaTime * _transform.up);
    }
}
