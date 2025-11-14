using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction mouseAction;
    
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _tf;
    [SerializeField] Camera _camera;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] float forceShoot = 10f;


    private void Start()
    {
        mouseAction = InputSystem.actions.FindAction("Shoot");
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(mouseAction.WasPressedThisFrame())
        {
            Vector3 MousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _rb.linearVelocity = Vector2.zero;
            _rb.linearVelocity = (Vector2)(_tf.position - MousePosition).normalized * forceShoot;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, GroundLayer);
    }
}
