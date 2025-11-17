using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PressF : MonoBehaviour
{
    [SerializeField] GameObject Fuck;
    bool canPress = false;
    InputAction action;

    private void Start()
    {
        action = InputSystem.actions.FindAction("F");
        Fuck.SetActive(false);
    }

    private void Update()
    {
        if (canPress && action.WasPressedThisFrame())
        {
            // TODO: Chuyen scene
            Debug.Log("Chuyen Scene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fuck.SetActive(true);
        canPress = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Fuck.SetActive(false);
        canPress = false;
    }
}
