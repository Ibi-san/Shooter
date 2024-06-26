using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class EnemyCharacter : Character
{
    private string _sessionID;
    
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    [SerializeField] private GameObject _currentWeapon;
    [SerializeField] private GameObject _machineGun;
    [SerializeField] private GameObject _sniperRifle;
    [SerializeField] private GameObject _rocketLauncher;
    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude;

    public void Init(string sessionID)
    {
        _sessionID = sessionID;
    }

    private void Start()
    {
        TargetPosition = transform.position;
    }

    private void Update()
    {
        if (_velocityMagnitude > 0.1f)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        }
        else
        {
            transform.position = TargetPosition;
        }
    }

    public void SetSpeed(float value) => speed = value;

    public void SetMaxHP(int value)
    {
        maxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    }

    public void RestoreHP(int newValue)
    {
        _health.SetCurrent(newValue);
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        TargetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;
        this.Velocity = velocity;
    }

    public void SetRotateX(float value)
    {
        _head.localEulerAngles = new Vector3(value, 0, 0);
    }
    
    public void SetRotateY(float value)
    {
        transform.localEulerAngles = new Vector3(0, value, 0);
    }
    
    public void SetCrouch(bool isCrouch)
    {
        if (isCrouch) Crouch();
        else StandUp();
        CharacterAnimation.AnimateCrouch(isCrouch);
    }

    public void ApplyDamage(int damage)
    {
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "id", _sessionID},
            { "value", damage }
        };
        MultiplayerManager.Instance.SendMessage("damage", data);
    }

    public void ChangeWeapon(Weapon weapon)
    {
        _currentWeapon.SetActive(false);
        
        switch (weapon)
        {
            case Weapon.MachineGun:
                _machineGun.SetActive(true);
                _currentWeapon = _machineGun;
                break;
            case Weapon.SniperRifle:
                _sniperRifle.SetActive(true);
                _currentWeapon = _sniperRifle;
                break;
            case Weapon.RocketLauncher:
                _rocketLauncher.SetActive(true);
                _currentWeapon = _rocketLauncher;
                break;
        }
    }
}
