using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Bullet BulletPrefab;
    public Action ShootEvent;
}

public enum Weapon
{
    MachineGun,
    SniperRifle,
    RocketLauncher
}