using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    //private bool isAlive;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //isAlive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
       // isAlive = false; // So that player cant get killed by dead falling enemy
        rb.useGravity = true;
        if (collision.gameObject.tag == "Floor")
        {
            gameObject.SetActive(false);
        }
    }
}
