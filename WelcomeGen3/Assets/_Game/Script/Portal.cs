using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform OutPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.position = OutPos.position;
        }
    }
}
