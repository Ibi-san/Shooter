using System;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
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

    public void Init(Player player)
    {
        _player = player;
        _character.SetSpeed(player.speed);
        player.OnChange += OnChange;
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

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
                default:
                    Debug.LogWarning("Doesn't handle field change" + dataChange.Field);
                    break;
            }
        }

        _character.SetMovement(position, velocity, AverageInterval);
    }
}