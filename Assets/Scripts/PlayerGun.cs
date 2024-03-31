using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private CanvasGroup _reloadCanvas;
    [SerializeField] private Image _reloadProgress;
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
        if (_equippedWeapon == Weapon.RocketLauncher)
        {
            _reloadCanvas.alpha = 1;
            StartCoroutine(Reloading());
            Instantiate(BulletPrefab, _bulletPoint.position, _bulletPoint.rotation).Init(velocity, _damage, true);
        }
        else
        {
            if (_equippedWeapon == Weapon.SniperRifle)
            {
                _reloadCanvas.alpha = 1;
                StartCoroutine(Reloading());
            }
            Instantiate(BulletPrefab, _bulletPoint.position, _bulletPoint.rotation).Init(velocity, _damage);
        }

        ShootEvent?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

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
                StopAllCoroutines();
                _reloadCanvas.alpha = 1;
                _shootDelay = _sniperRifleDelay;
                _bulletSpeed = _sniperBulletSpeed;
                _sniperRifle.SetActive(true);
                _currentWeapon = _sniperRifle;
                StartCoroutine(Reloading());
                break;
            case Weapon.RocketLauncher:
                StopAllCoroutines();
                _reloadCanvas.alpha = 1;
                _shootDelay = _rocketLauncherDelay;
                _bulletSpeed = _rocketLaunchSpeed;
                _rocketLauncher.SetActive(true);
                _currentWeapon = _rocketLauncher;
                StartCoroutine(Reloading());
                break;
        }
    }

    private IEnumerator Reloading()
    {
        while (Time.time - _lastShootTime < _shootDelay)
        {
            _reloadProgress.fillAmount = (Time.time - _lastShootTime) / _shootDelay;
            yield return new WaitForEndOfFrame();
        }

        _reloadCanvas.alpha = 0;
    }
}