using System;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _head;
    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude;

    private float _targetRotationX;
    private float _targetRotationY;
    private float _rotateSpeed = 360;
    private void Start()
    {
        TargetPosition = transform.position;
    }

    private void Update()
    {
        Move();

        RotateHead();

        RotateBody();
    }

    private void Move()
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

    private void RotateHead()
    {
        if (Math.Abs(_head.localEulerAngles.x - _targetRotationX) > 0.1f)
        {
            Vector3 eulerRotationX = _head.localEulerAngles;
            eulerRotationX.x = _targetRotationX;
            Quaternion newRotationX = Quaternion.Euler(eulerRotationX);
            _head.localRotation = Quaternion.RotateTowards(_head.localRotation, newRotationX, _rotateSpeed * Time.deltaTime);
        }
        else
        {
            _head.localEulerAngles = new Vector3(_targetRotationX, 0, 0);
        }
    }

    private void RotateBody()
    {
        if (Math.Abs(transform.localEulerAngles.y - _targetRotationY) > 0.1f)
        {
            Vector3 eulerRotationY = transform.localEulerAngles;
            eulerRotationY.y = _targetRotationY;
            Quaternion newRotationY = Quaternion.Euler(eulerRotationY);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotationY, _rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, _targetRotationY, 0);
        }
    }

    public void SetSpeed(float value) => Speed = value;

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        TargetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;
        this.Velocity = velocity;
    }

    public void SetRotateX(float value)
    {
        _targetRotationX = value;
    }
    
    public void SetRotateY(float value)
    {
        _targetRotationY = value;
    }
}
