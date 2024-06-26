using System.Collections.Generic;
using Colyseus.Schema;
using Multiplayer;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _gun;
    private List<float> _receivedTimeInterval = new() { 0, 0, 0, 0, 0 };
    private float AverageInterval
    {
        get
        {
            int receivedTimeIntervalCount = _receivedTimeInterval.Count;
            float sum = 0;
            for (int i = 0; i < receivedTimeIntervalCount; i++)
            {
                sum += _receivedTimeInterval[i];
            }

            return sum / receivedTimeIntervalCount;
        }
    }
    private float _lastReceivedTime;
    private Player _player;

    public void Init(string key, Player player)
    {
        _character.Init(key);
        
        _player = player;
        _character.SetSpeed(player.speed);
        _character.SetMaxHP(player.maxHP);
        player.OnChange += OnChange;
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);
        
        _gun.Shoot(position, velocity);
    }
    
    public void SwitchWeapon(int weaponID)
    {
        Weapon weapon = (Weapon)weaponID;
        _gun.IsRocket = weapon == Weapon.RocketLauncher;
        _character.ChangeWeapon(weapon);
    }

    public void Crouch(in bool isCrouch) => _character.SetCrouch(isCrouch);

    private void SaveReceivedTime()
    {
        float interval = Time.time - _lastReceivedTime;
        _lastReceivedTime = Time.time;
        
        _receivedTimeInterval.Add(interval);
        _receivedTimeInterval.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes)
    {
        SaveReceivedTime();
        
        Vector3 position = _character.TargetPosition;
        Vector3 velocity = _character.Velocity;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "loss":
                    MultiplayerManager.Instance._lossCounter.SetEnemyLoss((byte)dataChange.Value);
                    break;
                case "currentHP":
                    if ((sbyte)dataChange.Value > (sbyte)dataChange.PreviousValue)
                        _character.RestoreHP((sbyte)dataChange.Value);
                    break;
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _character.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _character.SetRotateY((float)dataChange.Value);
                    break;
                /*default:
                    Debug.LogWarning("Doesn't handle field change" + dataChange.Field);
                    break;*/
            }
        }

        _character.SetMovement(position, velocity, AverageInterval);
    }
}