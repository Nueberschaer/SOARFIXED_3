using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
   
    private float stormSpeed = -1000f; //speed at which the storm pushes the player towards the ground


    public float speed = 0.5f;
    public float ForceBase = 0.1f;
    public float ForceIncrement = 0.025f;
    public float ForceMax = 1f;
    private Rigidbody rb;
    private float time;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
        PlayerMove();
    }


    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.tag == "Storm") //Pushes player towards the ground if the player enters a storm
        {
            Debug.Log("StormActive");
            rb.AddForce(0, stormSpeed, (float)ForceMode.VelocityChange, 0);

        }
    }
}
