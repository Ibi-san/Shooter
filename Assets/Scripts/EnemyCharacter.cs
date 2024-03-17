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
        Vector3 targetEulerRotation = _head.localEulerAngles;
        targetEulerRotation.x = _targetRotationX;
        Quaternion newHeadRotation = Quaternion.Euler(targetEulerRotation);
        _head.localRotation =
            Quaternion.RotateTowards(_head.localRotation, newHeadRotation, _rotateSpeed * Time.deltaTime);
    }

    private void RotateBody()
    {
        Vector3 targetEulerRotation = transform.localEulerAngles;
        targetEulerRotation.y = _targetRotationY;
        Quaternion newBodyRotation = Quaternion.Euler(targetEulerRotation);
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, newBodyRotation, _rotateSpeed * Time.deltaTime);
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

    public void SetCrouch(bool isCrouch)
    {
        if (isCrouch) Crouch();
        else StandUp();
        CharacterAnimation.AnimateCrouch(isCrouch);
    }
}