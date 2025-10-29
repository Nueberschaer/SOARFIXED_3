using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject killDetector;
    private int collisionSpeed = 2000;
    private PlayerFlight playerFlightScript;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        killDetector = transform.Find("KillDetection").gameObject;
        playerFlightScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFlight>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;
        killDetector.SetActive(false);
        if (collision.gameObject.tag == "Floor")
        {
            gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "Sword") 
        {
            rb.AddForce(0, 0, collisionSpeed); // makes the enemy get pushed forward away from the player
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.position.z < playerFlightScript.gameObject.transform.position.z - 500)
        {
            gameObject.SetActive(false);
        }
    }
}
