using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Transform _transform;

    public bool reverseX = false;
    //public bool reverseY = true;
    public float mouseSensitivityX = 3;
    //public float mouseSensitivityY = 3;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivityX * (reverseX ? -1 : 1), 0);
            //_transform.Rotate(Input.GetAxis("Mouse Y") * mouseSensitivityY * (reverseY ? -1 : 1), 0, 0);
        }
    }
}
