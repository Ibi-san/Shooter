using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private static readonly int shoot = Animator.StringToHash("Shoot");
    
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _gun.ShootAction += Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(shoot);
    }

    private void OnDestroy()
    {
        _gun.ShootAction -= Shoot;
    }
}
