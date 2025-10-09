using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Falling");
        rb.useGravity = true;
    }
}
