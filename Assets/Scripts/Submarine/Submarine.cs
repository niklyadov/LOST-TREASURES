using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Submarine : NetworkBehaviour
{
    public TeamColor Team 
    {
        get => _team;
        set
        {
            _team = value;
            _ui.SetTeam(value);
        }
    }
    private SubmarineMovement _movement;
    private SubmarinePickup _pickup;
    private SubmarineHealth _health;
    private SubmarineSounds _sounds;
    
    private NetworkIdentity _networkIdentity;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private TeamColor _team = TeamColor.Blue;

    [SerializeField]
    public Transform cameraPoint;

    private bool _lock = false;
    public bool Lock => _lock;

    private OverlayUI _ui;

    private void Awake()
    {
        _movement = GetComponent<SubmarineMovement>();
        
        _pickup = GetComponent<SubmarinePickup>();
        _pickup.droppedTreasure += DroppedTreasure;
        _pickup.pickedUpTreasure += PickedUpTreasure;

        _health = GetComponent<SubmarineHealth>();
        _sounds = GetComponent<SubmarineSounds>();
        
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        
        _networkIdentity = GetComponent<NetworkIdentity>();
    }

    #region Networking treasure
    
    private void DroppedTreasure(Treasure treasure)
    {
    }
    
    private void PickedUpTreasure(Treasure treasure)
    {
        _sounds.PlayPickupSound();
    }    

    #endregion

    private void Start()
    {
        if (_networkIdentity.isLocalPlayer)
        {
            // Spawn camera
            var mainCamera = Instantiate(new GameObject("PlayerCamera"), cameraPoint);
                mainCamera.transform.Rotate(new Vector3(30, 0, 0));
                mainCamera.AddComponent<Camera>();
                mainCamera.AddComponent<AudioListener>();

            
            _ui = GameObject.FindGameObjectWithTag("UI").GetComponent<OverlayUI>();
            _ui.SetHP(_health.MaxHealth, _health.MaxHealth);
            CmdJoinTeam();
        }
    }

    private void FixedUpdate()
    {
        if (_networkIdentity.isLocalPlayer && Input.anyKey)
            TakeControl();
    }

    [Command]
    private void CmdJoinTeam()
    {
        if (isServer)
            GameObject.FindGameObjectWithTag("Global").GetComponent<Match>().CmdJoinTeam(netId);
    }

    [Command]
    private void CmdLeaveTeam()
    {
        if (isServer)
            GameObject.FindGameObjectWithTag("Global").GetComponent<Match>().CmdLeaveTeam(netId);
    }

    [ClientRpc]
    public void RpcSetLock(bool to)
    {
        if (!isLocalPlayer)
            return;
        
        _lock = to;
    }

    [ClientRpc]
    public void RpcSpawn(TeamColor team, int order)
    {
        Team = team;
        
        if (!_networkIdentity.isLocalPlayer)
            return;
        
        var baseTransform = GameObject.FindGameObjectWithTag(team.ToString() + "Base").transform;
        var basePosition = baseTransform.position;
        var baseRotation = baseTransform.rotation;
        basePosition.x += order * 2;
        
        transform.position = basePosition;
        transform.rotation = baseRotation;
    }

    private void TakeControl()
    {
        if (_lock) return;
        
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

        CmdLeaveTeam();
    }

    private void OnCollisionEnter(Collision other)
    {
       _transform.rotation = Quaternion.Euler(0, _transform.rotation.eulerAngles.y, 0);
       if (!_networkIdentity.isLocalPlayer)
            return;

       if (_lock)
           return;

       var submarine = other.gameObject.GetComponent<Submarine>();
       if (submarine == null)
           return;

       var magnitude = other.impulse.magnitude;

       CmdCollision(submarine.netId, magnitude);
    }

    [Command]
    public void CmdCollision(NetworkInstanceId otherId, float force)
    {
        if (isServer)
            ClientScene.FindLocalObject(otherId).GetComponent<Submarine>().RpcCollisionEnter(force);
    }

    [ClientRpc]
    public void RpcCollisionEnter(float force)
    {
        if (_networkIdentity.isLocalPlayer)
            Damage(force);
    }

    private void Damage(float force)
    {
        _health.TakeDamage(force * 15);
        _ui.SetHP(_health.MaxHealth, _health.CurrentHealth);
        if (_health.CurrentHealth <= 0)
        {
            if (_pickup.Treasure != null)
            {
                _pickup.CmdDrop(_pickup.Treasure.netId);
                _pickup.droppedTreasure(_pickup.Treasure);
            }

            _health.Restore();
            _ui.SetHP(_health.MaxHealth, _health.CurrentHealth);

            var baseTransform = GameObject.FindGameObjectWithTag(Team.ToString() + "Base").transform;
            var basePosition = baseTransform.position;
            var baseRotation = baseTransform.rotation;
            basePosition.x += 2 * 2;
        
            transform.position = basePosition;
            transform.rotation = baseRotation;
        }

        if (force > 0.6f && _pickup.Treasure != null)
        {
            _pickup.CmdDrop(_pickup.Treasure.netId);
            _pickup.droppedTreasure(_pickup.Treasure);
        }
    }
}