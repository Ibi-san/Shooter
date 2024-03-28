using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSensetivity = 2f;
    private MultiplayerManager _multiplayerManager;
    private bool _hold = false;
    private bool _hideCursor;
    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
        _hideCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _gun.Equip(Weapon.MachineGun);
    }

    private void Update()
    {
        SwitchWeapon();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _hideCursor = !_hideCursor;
            Cursor.lockState = _hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !_hideCursor;
        }
        if(_hold) return;
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = 0;
        float mouseY = 0;
        bool isShoot = false;
        
        if (_hideCursor)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            isShoot = Input.GetMouseButton(0);
        }
        
        bool space = Input.GetKeyDown(KeyCode.Space);
        
        _player.SetInput(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);
        if (space) _player.Jump();
        
        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        SendMove();
    }

    private void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _gun.Equip(Weapon.MachineGun);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _gun.Equip(Weapon.SniperRifle);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _gun.Equip(Weapon.RocketLauncher);
    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multiplayerManager.GetSessionId();
        string json = JsonUtility.ToJson(shootInfo);
        
        _multiplayerManager.SendMessage("shoot", json);
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

    public void Restart(int spawnIndex)
    {
        _multiplayerManager._spawnPoints.GetPoint(spawnIndex, out Vector3 position, out Vector3 rotation);
        StartCoroutine(Hold());
        
        _player.transform.position = position;
        rotation.z = 0;
        rotation.x = 0;
        _player.transform.eulerAngles = rotation;
        _player.SetInput(0, 0, 0);
        
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x },
            { "pY", position.y },
            { "pZ", position.z },
            { "vX", 0 },
            { "vY", 0 },
            { "vZ", 0 },
            { "rX", 0 },
            { "rY", rotation.y }
        };
        _multiplayerManager.SendMessage("move", data);
    }

    private IEnumerator Hold()
    {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;
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