using System;
using UnityEngine;
using UnityEngine.Networking;

public class Submarine : NetworkBehaviour
{
    private SubmarineMovement _movement;
    private SubmarinePickup _pickup;
    private SubmarineHealth _health;
    private SubmarineSounds _sounds;
    
    private NetworkIdentity _networkIdentity;
    private Rigidbody _rigidbody;
    private Team? _team = null;

    [SerializeField]
    public Transform cameraPoint;

    private string _info;

    private Transform _spawnPoint;

    private void Awake()
    {
        _movement = GetComponent<SubmarineMovement>();
        
        _pickup = GetComponent<SubmarinePickup>();
        _pickup.droppedTreasure += DroppedTreasure;
        _pickup.pickedUpTreasure += PickedUpTreasure;

        _health = GetComponent<SubmarineHealth>();
        _sounds = GetComponent<SubmarineSounds>();
        
        _rigidbody = GetComponent<Rigidbody>();
        
        _networkIdentity = GetComponent<NetworkIdentity>();
    }

    #region Networking treasure

    [Command]
    private void CmdSetParent(NetworkInstanceId idNum)
    {
        // treasure
        var newTransform = ClientScene.FindLocalObject(idNum).transform;
        
        // set treasure transform to current
        newTransform.parent = transform;
    }
    
    [Command]
    private void CmdRemoveParent(NetworkInstanceId idNum)
    {
        // treasure
        var newTransform = ClientScene.FindLocalObject(idNum).transform;
        
        // set treasure transform to null
        newTransform.parent = null;
    }
    
    private void DroppedTreasure(Treasure treasure)
    {
        if (_networkIdentity.isLocalPlayer)
            CmdRemoveParent(treasure.gameObject.GetComponent<NetworkIdentity>().netId);
    }
    
    private void PickedUpTreasure(Treasure treasure)
    {
        
        _sounds.PlayPickupSound();
        
        if (_networkIdentity.isLocalPlayer && treasure.Owner == null)
            CmdSetParent(treasure.gameObject.GetComponent<NetworkIdentity>().netId);
    }    

    #endregion

    private void Start()
    {
        if (_networkIdentity.isLocalPlayer)
        {
            // Spawn camera
            var mainCamera = new GameObject("PlayerCamera");
                mainCamera.AddComponent<Camera>();
                mainCamera.AddComponent<AudioListener>();
                
            var cameraFollow = mainCamera.AddComponent<CameraFollow>();
                cameraFollow.targetFollow = cameraPoint;
                cameraFollow.targetLook = transform;
                
            // Joining the team
            _team = GameController.GetInstance().PlayerJoin(this);
            _spawnPoint = GameObject.FindWithTag(_team + "Base").transform;

            transform.position = _spawnPoint.position;
        }
    }

    private void FixedUpdate()
    {
        if (_networkIdentity.isLocalPlayer && Input.anyKey)
            TakeControl();
    }

    private void TakeControl()
    {
        if (Input.GetKey(KeyCode.W)) 
            _movement.Forward(1);
        else if (Input.GetKey(KeyCode.S))
            _movement.Forward(-1);

        if (Input.GetKey(KeyCode.D))
            _movement.Rotate(1);
        else if (Input.GetKey(KeyCode.A))
            _movement.Rotate(-1);
        
        if (Input.GetKey(KeyCode.LeftShift))
            _movement.Raise(1);
        else if (Input.GetKey(KeyCode.LeftControl))
            _movement.Raise(-1);
    }

    private void OnDestroy()
    {
        // unsubscribe events
        _pickup.droppedTreasure = null;
        _pickup.pickedUpTreasure = null;
        
        
        // leave from team 
        if (_team == null)
            return;

        GameController.GetInstance().PlayerLeave((Team) _team, this);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_networkIdentity.isLocalPlayer)
            return;

        var submarine = other.gameObject.GetComponent<Submarine>();
        if (submarine == null)
            return;

        var magnitude = other.impulse.magnitude;
        _info += $" m: {magnitude} ;  ";
        
        CmdSubmarineCollision(submarine.GetComponent<NetworkIdentity>().netId, magnitude);
    }

    [Command]
    private void CmdSubmarineCollision(NetworkInstanceId idNum, float magnitude)
    {
        var submarine = ClientScene.FindLocalObject(idNum);
            submarine.GetComponent<SubmarineHealth>().TakeDamage(magnitude * 5);
            var pickup = submarine.GetComponent<SubmarinePickup>();
            
        if (magnitude > 0.6f) 
        {
            // lost treasure
            if (pickup.Treasure != null)
            {
                pickup.Treasure.Drop();
                pickup.droppedTreasure(pickup.Treasure);
            } 
        }
    }

    private void OnGUI()
    {
        if (!isLocalPlayer)
            return;
        
        GUI.Label(new Rect(0,0, Screen.width, 500), $"---------------- Hp:    {_health.CurrentHealth},     {_info}");
    }
}