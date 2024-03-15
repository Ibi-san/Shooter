using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSensetivity = 2f;
    private MultiplayerManager _multiplayerManager;
    [SerializeField] private bool _crouchIsToogle;
    private bool _isCrouching;
    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool isShoot = Input.GetMouseButton(0);
        
        bool space = Input.GetKeyDown(KeyCode.Space);

        if (_crouchIsToogle && Input.GetKeyDown(KeyCode.LeftControl))
            _isCrouching = !_isCrouching;
        else if (!_crouchIsToogle)
            _isCrouching = Input.GetKey(KeyCode.LeftControl);

        _player.SetInput(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);
        
        if (space) _player.Jump();
        
        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        if (_isCrouching)
        {
            _player.TryCrouch();
            SendCrouch(true);
        }
        else
        {
            _player.StandUp();
            SendCrouch(false);
        }

        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multiplayerManager.GetSessionId();
        string json = JsonUtility.ToJson(shootInfo);
        
        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendCrouch(bool isCrouch)
    {
        CrouchInfo crouchInfo = new CrouchInfo()
        {
            key = _multiplayerManager.GetSessionId(),
            isC = isCrouch,
        };
        crouchInfo.key = _multiplayerManager.GetSessionId();
        string json = JsonUtility.ToJson(crouchInfo);
        
        _multiplayerManager.SendMessage("crouch", json);
    }

    private void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x },
            { "pY", position.y },
            { "pZ", position.z },
            { "vX", velocity.x },
            { "vY", velocity.y },
            { "vZ", velocity.z },
            { "rX", rotateX },
            { "rY", rotateY }
            
        };
        _multiplayerManager.SendMessage("move", data);
    }
}

[Serializable]
public struct ShootInfo
{
    public string key;
    public float dX;
    public float dY;
    public float dZ;    
    public float pX;
    public float pY;
    public float pZ;
}

[Serializable]
public struct CrouchInfo
{
    public string key;
    public bool isC;
}