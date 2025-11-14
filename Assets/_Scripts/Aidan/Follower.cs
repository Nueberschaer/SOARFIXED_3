using UnityEngine;

public class Follower : MonoBehaviour
{

    private Transform player;
    private float speed = 0.025f;


    public Rigidbody rb;
    protected GameObject killDetector;
    protected PlayerFlight playerFlightScript;
    protected StormLight stormLightScript;

    protected int collisionSpeed = 4000;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Body").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        killDetector = GameObject.FindGameObjectWithTag("KillDetection").gameObject;
        playerFlightScript = GameObject.Find("Player").GetComponent<PlayerFlight>();
        stormLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>();
    }

    void Update()
    {
        //float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.position.z < playerFlightScript.gameObject.transform.position.z - 500)
        {
            gameObject.SetActive(false);
        }
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
            Debug.Log("Killed");
            killDetector.SetActive(false);
            rb.AddForce(0, -50, collisionSpeed);
            Debug.Log("Sword");
        }
        if (other.gameObject.tag == "Body")
        {
            stormLightScript.stormLightEnergy -= 15;
            Debug.Log("BODY");
        }
    }
}
