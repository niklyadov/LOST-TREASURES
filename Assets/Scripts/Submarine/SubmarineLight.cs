using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineLight : MonoBehaviour
{
    public Transform MovingPartTransform;
    public float RotationAngle = 10f;

    private Light _light;

    private float _offsetX;
    private float _offsetY;

    void Start()
    {
        MovingPartTransform.localRotation = Quaternion.Euler(0, 0, 0);
        _light = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _offsetX = Input.mousePosition.x;
            _offsetY = Input.mousePosition.y;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            MovingPartTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            MovingPartTransform.localRotation = Quaternion.Euler(
                Mathf.Clamp((Input.mousePosition.y - _offsetY) * -0.2f, -RotationAngle, RotationAngle),
                0,
                Mathf.Clamp((Input.mousePosition.x - _offsetX) * -0.2f, -RotationAngle, RotationAngle));
        }       
    }
}
