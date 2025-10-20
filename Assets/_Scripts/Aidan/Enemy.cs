using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject killDetector;
    private int collisionSpeed = 800;
    //private bool isAlive;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        killDetector = transform.Find("KillDetection").gameObject;
        //isAlive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
       // isAlive = false; // So that player cant get killed by dead falling enemy
        rb.useGravity = true;
        killDetector.SetActive(false);
        if (collision.gameObject.tag == "Floor")
        {
            gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "Player") 
        {
            rb.AddForce(collisionSpeed, 0, 0); // makes the enemy get pushed forward away from the player
        }
    }
}
