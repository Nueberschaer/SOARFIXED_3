using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerFlight : MonoBehaviour
{
    
    public Camera playerCamera;        
    public float baseFOV = 60f;
    public float maxFOV = 90f;
    public float fovLerpSpeed = 5f;     
    public float fovAtSpeed = 20f;     

    
    public float baseSpeed = 10f;
    public float speedIncrement = 3f;
    public float maxSpeed = 20f;
    public float stillThreshold = 0.05f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed = 0f;
    private bool hovering = false;


    private float displayedSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 1f;
        rb.angularDamping = 2f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera == null) playerCamera = Camera.main;
        }

        if (playerCamera != null)
            playerCamera.fieldOfView = baseFOV;
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        var key = Keyboard.current;

        // W: forward movement toggle & speed increase
        if (key.wKey.wasPressedThisFrame)
        {
            if (moveDirection == Vector3.forward)
            {
                IncreaseSpeed();
            }
            else
            {
                hovering = false;
                moveDirection = Vector3.forward;
                currentSpeed = baseSpeed;
            }
        }

        // S: hover if moving, backward if stopped
        if (key.sKey.wasPressedThisFrame)
        {
            bool isMoving = rb.velocity.sqrMagnitude > (stillThreshold * stillThreshold);

            if (isMoving)
            {
                // Hover
                hovering = true;
                moveDirection = Vector3.zero;
                currentSpeed = 0f;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                // Backward movement toggle & speed increase
                hovering = false;
                if (moveDirection == Vector3.back)
                {
                    IncreaseSpeed();
                }
                else
                {
                    moveDirection = Vector3.back;
                    currentSpeed = baseSpeed;
                }
            }
        }

        // Lateral/vertical input
        float x = 0f, y = 0f;
        if (key.aKey.isPressed) x -= 1f;
        if (key.dKey.isPressed) x += 1f;
        if (key.spaceKey.isPressed) y += 1f;
        if (key.leftShiftKey.isPressed) y -= 1f;

        Vector3 sideInput = new Vector3(x, y, 0f);
        if (sideInput.sqrMagnitude > 1f) sideInput.Normalize();

        
        Vector3 forwardMovement = moveDirection * currentSpeed;
        Vector3 strafeMovement = new Vector3(sideInput.x, sideInput.y, 0f) * baseSpeed;

       
        moveDirection = (forwardMovement + strafeMovement).normalized;

        //fov adjustment
        if (playerCamera != null)
        {
            
            float actualSpeed = rb.velocity.magnitude;

            
            displayedSpeed = Mathf.Lerp(displayedSpeed, actualSpeed, Time.deltaTime * fovLerpSpeed);

            
            float speedPercent = Mathf.Clamp01(displayedSpeed / Mathf.Max(0.01f, fovAtSpeed));

            
            float targetFOV = Mathf.Lerp(baseFOV, maxFOV, speedPercent);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovLerpSpeed);
        }
    }

    void FixedUpdate()
    {
        if (hovering)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Apply acceleration
        rb.AddForce(moveDirection * currentSpeed, ForceMode.Acceleration);
    }

    private void IncreaseSpeed()
    {
        currentSpeed += speedIncrement;
        if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger: " + other.name);
    }
}