using UnityEngine;

public class SubmarineHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;
    private float _health;

    public void Start()
    {
        _health = maxHealth;
        GameController.GetInstance().OverlayUi.SetHP(_health / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        GameController.GetInstance().OverlayUi.SetHP(maxHealth, _health);
    }

    public float CurrentHealth
    {
        get => _health;
    }
}