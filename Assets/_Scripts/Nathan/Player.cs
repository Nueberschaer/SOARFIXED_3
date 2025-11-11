using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerFlight : MonoBehaviour
{
    //My addition
    public int distance = 0;
    private float stormSpeed = -10000f; //speed at which the storm pushes the player towards the ground
    StormLight stormLightScript;
    EnemySpawner enemySpawnerScript;
    //

    public Camera playerCamera;
    public float baseFOV = 60f;
    public float maxFOV = 90f;
    public float fovLerpSpeed = 3f;
    public float fovAtSpeed = 50f;

    public float baseSpeed = 50f; // was 10
    public float speedIncrement = 20f; // was 3
    public float maxSpeed = 250f;// was 20
    public float stillThreshold = 0.2f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed = 0f;
    private bool hovering = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isMovingUp = false;
    private bool isMovingDown = false;

    private float displayedSpeed;

    
    public float hoverBrake = 6f;          // how quickly S brakes you to hover
    public float accelPerSecond = 125f;    // how fast speed ramps up/down

    //Mouse Look
    public float mouseSensitivity = 80f;   
    public float minPitch = -80f;
    public float maxPitch = 80f;
    public bool invertY = false;
    public bool lockCursor = true;

    private float camYaw;
    private float camPitch;

    private bool mouseLookActive = false;      
    private float ignoreMouseUntilTime = 0f;   
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 4f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera == null) playerCamera = Camera.main;
        }

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = baseFOV;

            

            SetCursorLocked(lockCursor); 
        }

        //Addition
        stormLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>();
        enemySpawnerScript = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        //
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        var key = Keyboard.current;

        
        if (playerCamera != null && Mouse.current != null && Cursor.lockState == CursorLockMode.Locked)
        {
            
            if (Time.unscaledTime >= ignoreMouseUntilTime)
            {
                Vector2 md = Mouse.current.delta.ReadValue();

                
                if (!mouseLookActive)
                {
                    if (md.sqrMagnitude > 0.000001f)
                    {
                        Vector3 e = playerCamera.transform.rotation.eulerAngles;
                        camYaw = e.y;
                        
                        float px = e.x; if (px > 180f) px -= 360f;
                        camPitch = px;
                        mouseLookActive = true;
                    }
                }

                if (mouseLookActive)
                {
                    float s = mouseSensitivity * Time.unscaledDeltaTime;
                    camYaw += md.x * s;
                    float dy = (invertY ? md.y : -md.y) * s; 
                    camPitch = Mathf.Clamp(camPitch + dy, minPitch, maxPitch);
                    playerCamera.transform.rotation = Quaternion.Euler(camPitch, camYaw, 0f);
                }
            }
        }

       
        if (key.escapeKey.wasPressedThisFrame)
        {
            lockCursor = !lockCursor;
            SetCursorLocked(lockCursor);
        }
       

        
        Vector3 v = rb.linearVelocity;
        isMovingLeft = v.x < -stillThreshold;
        isMovingRight = v.x > stillThreshold;
        isMovingUp = v.y > stillThreshold;
        isMovingDown = v.y < -stillThreshold;

        float x = 0f, y = 0f, z = 0f;

        if (key.aKey.isPressed && stormLightScript.stormLightEnergy >= 0) x -= 1f;
        if (key.dKey.isPressed && stormLightScript.stormLightEnergy >= 0) x += 1f;
        if (key.spaceKey.isPressed && stormLightScript.stormLightEnergy >= 0) y += 1f;
        if (key.leftShiftKey.isPressed && stormLightScript.stormLightEnergy >= 0) y -= 1f;
        if (key.wKey.isPressed && stormLightScript.stormLightEnergy >= 0) z += 1f;

        Vector3 desiredInput = new Vector3(x, y, z);
        if (desiredInput.sqrMagnitude > 1f) desiredInput.Normalize();

        // smooth hovering logic:
        bool sPressed = key.sKey.isPressed && stormLightScript.stormLightEnergy >= 0;
        if (sPressed && rb.linearVelocity.sqrMagnitude > (stillThreshold * stillThreshold))
        {
            hovering = true;
            moveDirection = Vector3.zero;
        }
        else
        {
            if (desiredInput.sqrMagnitude > 0f)
            {
                hovering = false;
                moveDirection = desiredInput;
            }
        }
        float targetSpeed = (hovering || desiredInput.sqrMagnitude == 0f) ? 0f : baseSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelPerSecond * Time.deltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        //fov adjustment
        if (playerCamera != null)
        {
            float actualSpeed = rb.linearVelocity.magnitude;
            displayedSpeed = Mathf.Lerp(displayedSpeed, actualSpeed, Time.deltaTime * fovLerpSpeed);
            float speedPercent = Mathf.Clamp01(displayedSpeed / Mathf.Max(0.01f, fovAtSpeed));
            float targetFOV = Mathf.Lerp(baseFOV, maxFOV, speedPercent);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovLerpSpeed);
        }

        //My addition
        distance = (int)transform.position.z;
        //
    }

    void FixedUpdate()
    {
        if (hovering)
        {
            // Smoothly brake toward a hover instead of snapping to zero
            rb.AddForce(-rb.linearVelocity * hoverBrake, ForceMode.Acceleration);
            return;
        }

        // Apply acceleration 
        rb.AddForce(moveDirection * currentSpeed, ForceMode.Acceleration);

        //ADDITION
        if (stormLightScript.stormLightEnergy <= 0) rb.AddForce(0, -20, 0);

        
        if (transform.position.y <= -300)
        {
            SetCursorLocked(false); // make sure cursor is available for the next scene
            SceneManager.LoadScene(2);
        }
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
        //Addition
        if (other.tag == "Storm") //Pushes player towards the ground if the player enters a storm
        {
            Debug.Log("StormActive");
            rb.AddForce(0, stormSpeed, 0);
        }

        if (other.tag == "Orb") // if player collides with stormlight orb they regenerate to max
        {
            Debug.Log("stormlight orb claimed");
            stormLightScript.stormLightEnergy = 100;
            other.gameObject.SetActive(false);
        }

        //EnemySpawning based on player reaching level distance
        if (other.tag == "Level2")
        {
            enemySpawnerScript.LevelTwoEnemies();
        }
        if (other.tag == "Level3")
        {
            enemySpawnerScript.LevelThreeEnemies();
        }
        if (other.tag == "Level4")
        {
            enemySpawnerScript.LevelFourEnemies();
        }
        if (other.tag == "Level5")
        {
            enemySpawnerScript.LevelFiveEnemies();
        }
        //
    }

    void OnDisable()
    {
        SetCursorLocked(false);
    }

    
    private void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;

        
        if (locked)
        {
            ignoreMouseUntilTime = Time.unscaledTime + 0.1f; 
            mouseLookActive = false;
        }
    }
}
