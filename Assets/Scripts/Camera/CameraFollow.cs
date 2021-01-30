using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _transform;
    
    public Transform targetFollow;
    public Transform targetLook;

    private void Awake()
    {
        _transform = transform;
    }
    
    void Update()
    {
        if (targetFollow == null || targetLook == null)
            Destroy(gameObject);
            
        _transform.position = 
            Vector3.Lerp (
                transform.position, 
                targetFollow.position, 
                10f * Time.deltaTime
                );
        transform.rotation = 
            Quaternion.RotateTowards(
                _transform.rotation, 
                Quaternion.LookRotation(targetLook.position - _transform.position), 
                Time.deltaTime * 90f
                );
    }
}
