using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	InputAction mouseAction;
	
	[SerializeField] Rigidbody2D _rb;
	[SerializeField] SpriteRenderer _sr;
	[SerializeField] SpriteRenderer _srGun;
	[SerializeField] Camera _camera;
	[SerializeField] Transform groundCheck;
	[SerializeField] LayerMask GroundLayer;
	[SerializeField] float firstShoot = 10f;
	[SerializeField] float secondShoot = 5f;
	[SerializeField] float reloadTime = 2f;
	[SerializeField] GameObject Bullet1;
	[SerializeField] GameObject Bullet2;
	Coroutine coroutine; // Kiem soat nap dan
	public bool CanShoot = true;
	private Vector2 MousePosition;
	private bool isFacingRight;

	public int countShoot = 0;

	[SerializeField] UnityEvent OnShoot;

	private void Start()
	{
		mouseAction = InputSystem.actions.FindAction("Shoot");
		mouseAction.Enable();
	}

	private void Update()
	{
		SetMousePos();
		Movement();
	}

	private void LateUpdate()
	{
		Flip();
	}

	void Movement()
	{
		if (mouseAction.WasPressedThisFrame() && CanShoot)
		{
			OnShoot.Invoke();
			Vector2 direction = ((Vector2)transform.position - MousePosition).normalized;
			countShoot++;
			if(countShoot == 1)
			{
				_rb.linearVelocity = direction * firstShoot;
				Bullet1.gameObject.SetActive(false);
				coroutine = StartCoroutine(ReloadShotgun());
			}
			else if(countShoot == 2)
			{
				_rb.linearVelocity = direction * secondShoot;
				Bullet2.gameObject.SetActive(false);
				if(coroutine != null)
				{
					StopCoroutine(coroutine);
					coroutine = null;
				}
				CanShoot = false;
				coroutine = StartCoroutine(ReloadShotgun());
			}
		}
	}

	private void SetMousePos()
	{
		MousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	}

	public Vector2 GetMousePos()
	{
		return MousePosition;
	}
	IEnumerator ReloadShotgun()
	{
		yield return new WaitForSeconds(reloadTime);
		countShoot = 0;
		CanShoot = true;
		Bullet1.gameObject.SetActive(true);
		Bullet2.gameObject.SetActive(true);
	}

	void Flip()
	{
		// Check Huong
		isFacingRight = (transform.position.x <= MousePosition.x ? true : false);
		_sr.flipX = !isFacingRight;
		_srGun.flipY = !isFacingRight;
	}
}
