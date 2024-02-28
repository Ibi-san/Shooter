using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Player _player;
    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        _player.SetInput(h, v);
    }
}