using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Speed = Animator.StringToHash("Speed");

    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Animator _animator;
    [SerializeField] private Character _character;

    private void Update()
    {
        Vector3 localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
        float speed = localVelocity.magnitude / _character.speed;
        float sign = Mathf.Sign(localVelocity.z);
        
        _animator.SetFloat(Speed, speed * sign);
        _animator.SetBool(Grounded, _checkFly.IsFly == false);
    }
}
