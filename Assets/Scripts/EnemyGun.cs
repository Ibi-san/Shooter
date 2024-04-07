using UnityEngine;

public class EnemyGun : Gun
{
    public bool IsRocket;

    public void Shoot(Vector3 position, Vector3 velocity)
    {
        Instantiate(BulletPrefab, position, Quaternion.LookRotation(velocity)).Init(velocity, IsRocket);
        ShootEvent?.Invoke();
    }
}