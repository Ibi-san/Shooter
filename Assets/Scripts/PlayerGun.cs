using System;
using UnityEngine;

public class PlayerGun : Gun
{
    private Weapon _equippedWeapon;
    [SerializeField] private int _damage;
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private GameObject _machineGun;
    [SerializeField] private GameObject _rocketLauncher;
    [SerializeField] private GameObject _sniperRifle;
    [SerializeField] private float _machineGunDelay = 0.2f;
    [SerializeField] private float _rocketLauncherDelay = 3f;
    [SerializeField] private float _sniperRifleDelay = 2f;
    [SerializeField] private float _machineBulletSpeed = 10f;
    [SerializeField] private float _rocketLaunchSpeed = 30f;
    [SerializeField] private float _sniperBulletSpeed = 80f;
    private GameObject _currentWeapon;
    private float _shootDelay;
    private float _bulletSpeed;
    private float _lastShootTime;

    private void Awake()
    {
        _currentWeapon = _machineGun;
    }

    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();
        
        if(Time.time - _lastShootTime < _shootDelay) return false;
        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;

        _lastShootTime = Time.time;
        Instantiate(BulletPrefab, _bulletPoint.position, _bulletPoint.rotation).Init(velocity, _damage);
        ShootEvent?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;
        
        // if (_equippedWeapon == Weapon.RocketLauncher)
        //     ShootRocketLauncher(ref info);
        // if (_equippedWeapon == Weapon.SniperRifle)
        //     ShootSniperRifle(ref info);

        return true;
    }

    public void Equip(Weapon weapon)
    {
        _equippedWeapon = weapon;
        _currentWeapon.SetActive(false);
        switch (weapon)
        {
            case Weapon.MachineGun:
                _shootDelay = _machineGunDelay;
                _bulletSpeed = _machineBulletSpeed;
                _machineGun.SetActive(true);
                _currentWeapon = _machineGun;
                break;
            case Weapon.SniperRifle:
                _shootDelay = _sniperRifleDelay;
                _bulletSpeed = _sniperBulletSpeed;
                _sniperRifle.SetActive(true);
                _currentWeapon = _sniperRifle;
                break;
            case Weapon.RocketLauncher:
                _shootDelay = _rocketLauncherDelay;
                _bulletSpeed = _rocketLaunchSpeed;
                _rocketLauncher.SetActive(true);
                _currentWeapon = _rocketLauncher;
                break;
        }
    }

    private void ShootRocketLauncher(ref ShootInfo info)
    {
        
    }

    private void ShootSniperRifle(ref ShootInfo info)
    {
        
    }
}