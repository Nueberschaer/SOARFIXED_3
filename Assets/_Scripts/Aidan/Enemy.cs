using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    protected GameObject killDetector;
    protected PlayerFlight playerFlightScript;
    protected StormLight stormLightScript;

    protected int collisionSpeed = 4000;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        killDetector = GameObject.FindGameObjectWithTag("KillDetection").gameObject;
        playerFlightScript = GameObject.Find("Player").GetComponent<PlayerFlight>();
        stormLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>();

    }

        void OnTriggerEnter(Collider other)
     {
        
         rb.useGravity = true;
         if (other.gameObject.tag == "Floor")
         {
             gameObject.SetActive(false);
         }
         if (other.gameObject.tag == "Sword")
         {
            Debug.Log("KILLEDD");
            killDetector.SetActive(false);
            rb.AddForce(0, -50, collisionSpeed);
             Debug.Log("Sworded");
         }
         if (other.gameObject.tag == "Body")
         {
             stormLightScript.stormLightEnergy -= 15;
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
