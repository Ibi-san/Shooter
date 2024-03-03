using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _enemyCharacter;
    
    public void OnChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "x":
                    position.x = (float)dataChange.Value;
                    break;
                case "y":
                    position.z = (float)dataChange.Value;
                    break;
                default:
                    Debug.LogWarning("Doesn't handle field change" + dataChange.Field);
                    break;
            }
        }

        MoveEnemy(position);
    }

    public void MoveEnemy(Vector3 position)
    {
        _enemyCharacter.SetPosition(position);
    }
}