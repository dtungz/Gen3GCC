using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction mouseAction;
    
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Camera _camera;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] float firstShoot = 10f;
    [SerializeField] float secondShoot = 5f;

    private bool onGround = false;
    public int countShoot = 0;

    private void Start()
    {
        mouseAction = InputSystem.actions.FindAction("Shoot");
    }

    private void Update()
    {
        isGrounded();
        Movement();
    }

    void Movement()
    {
        if (mouseAction.WasPressedThisFrame())
        {
            Vector2 MousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = ((Vector2)transform.position - MousePosition).normalized;
            countShoot++;
            if(countShoot == 0)
            {
                _rb.linearVelocity = direction * firstShoot;
            }
            else if(countShoot == 1)
            {
                _rb.linearVelocity = direction * secondShoot;
            }
        }
        if(onGround)
        {
            countShoot = 0;
        }
    }

    private void isGrounded()
    {
        onGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.02f), CapsuleDirection2D.Horizontal, 0, GroundLayer);
    }
}
