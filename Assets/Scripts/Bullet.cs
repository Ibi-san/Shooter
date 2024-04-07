using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _bulletModel;
    [SerializeField] private GameObject _rocketModel;
    [SerializeField] private float _radius;
    [SerializeField] private GameObject _explosionParticle;
    private int _damage;
    private bool _isRocket;
    
    public void Init(Vector3 velocity, bool rocket = false, int damage = 0)
    {
        _damage = damage;
        _rigidbody.velocity = velocity;

        if (rocket)
        {
            _isRocket = true;
            _bulletModel.SetActive(false);
            _rocketModel.SetActive(true);
            _rigidbody.useGravity = true;
        }
        
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isRocket)
        {
            Explode();
            Destroy();
        }
        else
        {
            if (collision.collider.TryGetComponent(out EnemyCharacter enemy))
                enemy.ApplyDamage(_damage);

            Destroy();
        }
    }
    
    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out EnemyCharacter enemy))
                enemy.ApplyDamage(_damage);
        }

        Instantiate(_explosionParticle, transform.position, Quaternion.identity);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}
