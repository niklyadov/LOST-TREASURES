using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _transform;
    
    public Transform targetFollow;
    public Transform targetLook;

    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private float rotationSpeed = 90f;

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
                _transform.position, 
                targetFollow.position, 
                movementSpeed * Time.deltaTime
                );
        transform.rotation = 
            Quaternion.RotateTowards(
                _transform.rotation, 
                Quaternion.LookRotation(targetLook.position - _transform.position), 
                rotationSpeed* Time.deltaTime
                );
    }
}
