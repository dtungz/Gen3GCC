using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private Vector2 MousePos;
    [SerializeField] private Transform _transform;

    private void Update()
    {
        RollingHand();
    }

    private void RollingHand()
    {
        MousePos = _player.GetMousePos();
        Vector2 direction = (MousePos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
