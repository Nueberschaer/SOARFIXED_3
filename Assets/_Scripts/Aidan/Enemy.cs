using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider boxCollider;
    protected Transform player;
    protected StormLight stormLightScript;

    protected int collisionSpeed = 4000;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponentInChildren<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Body").GetComponent<Transform>();
        stormLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>();

    }


    private void FixedUpdate()
    {
        if (gameObject.transform.position.z < player.transform.position.z - 500)
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
            StartCoroutine(Invulnerability());
            rb.AddForce(1000, -1000, collisionSpeed);
         }
         if (other.gameObject.tag == "Body")
         {
            StartCoroutine(Invulnerability());
             stormLightScript.stormLightEnergy -= 15;
         }
     }

    protected IEnumerator Invulnerability()
    {
        boxCollider.isTrigger = false;
        yield return new WaitForSeconds(2);
        boxCollider.isTrigger = true;
    }
}
