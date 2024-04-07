using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; protected set; } = 10;
    [field: SerializeField] public float speed { get; protected set; } = 2f;
    [SerializeField] protected CapsuleCollider _collider;
    [SerializeField] protected CharacterAnimation CharacterAnimation;
    [field: SerializeField] public float Speed { get; protected set; } = 2f;
    public Vector3 Velocity { get; protected set; }

    public void Crouch()
    {
        _collider.center = new Vector3(0, 0.75f, 0);
        _collider.height = 1.5f;
        CharacterAnimation.AnimateCrouch(true);
    }

    public void StandUp()
    {
        _collider.center = new Vector3(0, 1f, 0);
        _collider.height = 2f;
        CharacterAnimation.AnimateCrouch(false);
    }
}