using UnityEngine;
using UnityEngine.Events;

public class SubmarinePickup : MonoBehaviour
{
    [SerializeField]
    private float timeForPickup = 2;

    public Treasure Treasure;
    
    [SerializeField]
    private Transform _treasureSpawnPoint;
    
    private Transform _transform;

    [SerializeField] private float _pickupTimer;

    public UnityAction<Treasure> pickedUpTreasure;
    public UnityAction<Treasure> droppedTreasure;
    
    private bool _pressed;

    #region Timeout Optimize
    // 10 calls per second
    private float updateTimeout = 0.1f;
    private float _updateTimeout;

    #endregion

    private void UpdatePercentage()
    {
        GameController.GetInstance().OverlayUi.SetActionPercentage(_pickupTimer / timeForPickup);
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // drop treasure
            if (Treasure != null)
            {
                droppedTreasure(Treasure);
                Treasure.Drop();
                Treasure = null;
                
                return;
            }
            
            _pressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _pressed = false;
            GameController.GetInstance().OverlayUi.SetActionPercentage(0);
            
            _pickupTimer = 0;
            
            UpdatePercentage();
            
            return;
        }

        if (_updateTimeout >= updateTimeout)
        {
            _updateTimeout = 0;
            if (_pressed)
            {
                if (Physics.Raycast(new Ray(_transform.position, _transform.up * -1), out RaycastHit hitInfo, 4))
                {
                    var treasure = hitInfo.transform.GetComponent<Treasure>();

                    if (treasure != null)
                    {
                        if (_pickupTimer < timeForPickup)
                        {
                            _pickupTimer += updateTimeout;
                            
                            UpdatePercentage();
                            
                            return;
                        }
                        
                        Treasure = treasure;
                        pickedUpTreasure(treasure);
                        treasure.Pickup(gameObject);

                        _pickupTimer = 0;
                        UpdatePercentage();
                        return;   
                    }
                }
            
                if (_pickupTimer > 0)
                {
                    _pickupTimer -= updateTimeout * 2;
                    
                    UpdatePercentage();
                    
                }
            }
            
            return;
        }

        _updateTimeout += Time.deltaTime;
    }
}
