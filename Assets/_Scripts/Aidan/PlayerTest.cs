using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerTest : MonoBehaviour
{
    //MY ADDITION
    private float stormSpeed = -1000f; //speed at which the storm pushes the player towards the ground
    StormLight stormLightScript;
    public int distance = 0;
    public int kills = 0;

    // ORIGINAL CODE
    public float speed = 0.5f;
    public float ForceBase = 0.1f;
    public float ForceIncrement = 0.025f;
    public float ForceMax = 1f;
    private Rigidbody rb;
    private float time;

    void Awake()
    {
        //Original Code
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY; // Added Y rotation Constraint

        //My Addition
        stormLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>(); // Reference to the stormlight script
    }
    void PlayerMove()
    {
        float currentForce = ForceBase + ForceIncrement * time;
        if (ForceMax > 0f && currentForce > ForceMax) currentForce = ForceMax;

        rb.AddForce(transform.forward * currentForce, ForceMode.Acceleration);

        if (Keyboard.current == null) return;

        time += Time.deltaTime;

        var curKey = Keyboard.current;

        float x = 0f, y = 0f, z = 0f;


        if (curKey.aKey.isPressed) x -= 1f;
        if (curKey.dKey.isPressed) x += 1f;


        if (curKey.wKey.isPressed) z += 1f;
        if (curKey.sKey.isPressed) z -= 1f;


        if (curKey.spaceKey.isPressed) y += 1f;
        if (curKey.leftShiftKey.isPressed) y -= 1f;


        Vector3 curInput = new Vector3(x, y, z);
        if (curInput.sqrMagnitude > 1f) curInput.Normalize();

        Vector3 moveDir = transform.right * curInput.x + transform.up * curInput.y + transform.forward * curInput.z;

        Vector3 Force = transform.right * curInput.x + transform.up * curInput.y + transform.forward * curInput.z;

        rb.AddForce(Force * speed, ForceMode.Acceleration);
    }

    void Start()
    {

    }

    void Update()
    {
        //Original Code
        PlayerMove();

        //My addition
        distance = (int)transform.position.x;
    }


    //My Addition
    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.tag == "Storm") //Pushes player towards the ground if the player enters a storm
        {
           //Debug.Log("StormActive");
            rb.AddForce(0, stormSpeed, (float)ForceMode.VelocityChange, 0);

        }

        if (collision.tag == "Orb") // if player collides with stormlight orb they regenerate to max
        {
            //Debug.Log("stormlight orb claimed");
            stormLightScript.stormLightEnergy = 100;
        }
        if (collision.tag == "Enemy")
        {
            kills += 1;
        }
    }
}
