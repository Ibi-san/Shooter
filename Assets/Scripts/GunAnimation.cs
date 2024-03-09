using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private static readonly int _shoot = Animator.StringToHash("Shoot");
    
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _gun.ShootEvent += Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(_shoot);
    }

    private void OnDestroy()
    {
        _gun.ShootEvent -= Shoot;
    }
}
