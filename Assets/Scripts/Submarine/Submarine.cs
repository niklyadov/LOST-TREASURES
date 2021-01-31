using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Submarine : NetworkBehaviour
{
    public TeamColor Team;
    private SubmarineMovement _movement;
    private SubmarinePickup _pickup;
    private SubmarineHealth _health;
    private SubmarineSounds _sounds;
    
    private NetworkIdentity _networkIdentity;
    private Rigidbody _rigidbody;
    private TeamColor? _team = null;

    [SerializeField]
    public Transform cameraPoint;

    private bool _lock = false;
    public bool Lock => _lock;

    private Transform _spawnPoint;
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
            var mainCamera = Instantiate(new GameObject("PlayerCamera"), cameraPoint);
                mainCamera.transform.Rotate(new Vector3(30, 0, 0));
                mainCamera.AddComponent<Camera>();
                mainCamera.AddComponent<AudioListener>();
                
            //var cameraFollow = mainCamera.AddComponent<CameraFollow>();
            //    cameraFollow.targetFollow = cameraPoint;
            //    cameraFollow.targetLook = transform;

            // Joining the team
            //_team = GameController.GetInstance().PlayerJoin(this);
            CmdJoinTeam();
            //_spawnPoint = GameObject.FindWithTag(_team + "Base").transform;

            //transform.position = _spawnPoint.position;
            _ui = GameObject.FindGameObjectWithTag("UI").GetComponent<OverlayUI>();
            _ui.SetHP(_health.MaxHealth, _health.MaxHealth);
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
        basePosition.x += order*2;
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
        _health.TakeDamage(force * 5);

        if (force > 0.6f && _pickup.Treasure != null)
        {
            _pickup.Treasure.Drop();
            _pickup.droppedTreasure(_pickup.Treasure);
        }
        _ui.SetHP(_health.MaxHealth, _health.CurrentHealth);
    }
}