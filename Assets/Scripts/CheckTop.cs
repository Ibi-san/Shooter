using UnityEngine;

public class CheckTop : MonoBehaviour
{
    public bool CanStand { get; private set; }
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;
    
    private void Update()
    {
        if (Physics.CheckSphere(transform.position, _radius, _layerMask))
        {
            CanStand = false;
        }
        else
        {
            CanStand = true;
        }
        
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}