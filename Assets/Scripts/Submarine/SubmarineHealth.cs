using UnityEngine;

public class SubmarineHealth : MonoBehaviour
{
    [SerializeField]
    private float _health = 100;

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage " + damage);
        _health -= damage;
        
        if (_health <= 0)
        {
            Debug.Log("0 hp");
        }
    }

    public float CurrentHealth
    {
        get => _health;
    }
}