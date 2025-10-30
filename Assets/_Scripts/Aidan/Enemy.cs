using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    private GameObject killDetector;
    private PlayerFlight playerFlightScript;
    StormLight stormLightScript;

    private int collisionSpeed = 2000;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        killDetector = transform.Find("KillDetection").gameObject;
        playerFlightScript = GameObject.Find("Player").GetComponent<PlayerFlight>();
        stormLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>();

    }

        void OnTriggerEnter(Collider other)
     {
         rb.useGravity = true;
         killDetector.SetActive(false);
         if (other.gameObject.tag == "Floor")
         {
             gameObject.SetActive(false);
         }
         if (other.gameObject.tag == "Sword")
         {
             rb.AddForce(0, -50, collisionSpeed);
             Debug.Log("Sword");
         }
         if (other.gameObject.tag == "Body")
         {
             stormLightScript.stormLightEnergy -= 50;
             Debug.Log("BODY");
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
