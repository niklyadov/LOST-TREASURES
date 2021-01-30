using System;
using UnityEngine;
using UnityEngine.Networking;

public class Submarine : NetworkBehaviour
{
    public Camera Camera;
    private SubmarineMovement _movement;
    private SubmarinePickup _pickup;
    private NetworkIdentity _networkIdentity;
    private Rigidbody _rigidbody;
    
    private float hp = 1000;
    private string _info;

    private void Awake()
    {
        _movement = GetComponent<SubmarineMovement>();
        
        _pickup = GetComponent<SubmarinePickup>();
        _pickup.droppedTreasure += DroppedTreasure;
        _pickup.pickedUpTreasure += PickedUpTreasure;
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
        if (_networkIdentity.isLocalPlayer && treasure.Owner == null)
            CmdSetParent(treasure.gameObject.GetComponent<NetworkIdentity>().netId);
    }    

    #endregion

    private void Start()
    {
        if (_networkIdentity.isLocalPlayer)
        {
            Camera.enabled = true;
        }
    }

    private void Update()
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
    }

    public void HitWithOtherSubmarine(Submarine other, float otherSpeed)
    {
        var localSpeed = _rigidbody.velocity.magnitude;
        var diffSpeed = Mathf.Abs(otherSpeed - localSpeed);

        _info += localSpeed + ", " ;

        hp -= localSpeed * 10;

        if (otherSpeed > localSpeed)
        {
            if (_pickup.Treasure != null)
            {
                _info += " you lost a treasure  ";
                _pickup.Treasure.Drop();
                _pickup.droppedTreasure(_pickup.Treasure);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_networkIdentity.isLocalPlayer)
            return;

        var submarine = other.gameObject.GetComponent<Submarine>();
        if (submarine == null)
            return;
        
        var rb = submarine.gameObject.GetComponent<Rigidbody>();

        var otherSpeed = rb.velocity.magnitude;
        hp -= otherSpeed * 10;
        
        var localSpeed = _rigidbody.velocity.magnitude;

        if (localSpeed < otherSpeed)
        {
            if (_pickup.Treasure != null)
            {
                _info += " you lost a treasure  ";
                _pickup.Treasure.Drop();
                _pickup.droppedTreasure(_pickup.Treasure);
            }   
        }
    }

    private void OnGUI()
    {
        if (!isLocalPlayer)
            return;
        
        GUI.Label(new Rect(0,0, Screen.width, 500), $"Hp:    {hp},     ");
    }
}