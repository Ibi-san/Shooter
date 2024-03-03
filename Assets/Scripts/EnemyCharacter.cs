using System;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private Vector3 newPosition;
    private Vector3 lastPosition;
    private Vector3 velocity;
    
    public void SetPosition(Vector3 position) => newPosition = position;

    private void Start()
    {
        lastPosition = transform.position;
        velocity = Vector3.zero;
    }

    private void Update()
    {
        Vector3 predictedPosition = lastPosition + velocity * Time.deltaTime;
        
        velocity = (newPosition - lastPosition) / Time.deltaTime;
        
        
        lastPosition = newPosition;
        
        transform.position = Vector3.MoveTowards(transform.position, predictedPosition, _speed * Time.deltaTime);
    }
}
