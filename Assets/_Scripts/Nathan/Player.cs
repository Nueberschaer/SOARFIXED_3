using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerFlight : MonoBehaviour
{
    //My addition
    public int distance = 0;
    private float stormSpeed = -500f; //speed at which the storm pushes the player towards the ground
    StormLight stormLightScript;
    EnemySpawner enemySpawnerScript;
    //



    public Camera playerCamera;
    public float baseFOV = 60f;
    public float maxFOV = 90f;
    public float fovLerpSpeed = 5f;
    public float fovAtSpeed = 20f;


    public float baseSpeed = 100f; // was 10
    public float speedIncrement = 25f; // was 3
    public float maxSpeed = 250f;// was 20
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
        rb.linearDamping = 0.1f;
        rb.angularDamping = 2f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;


        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera == null) playerCamera = Camera.main;
        }

        if (playerCamera != null)
            playerCamera.fieldOfView = baseFOV;

        //Addition
        stormLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>(); // Reference to the stormlight script
        enemySpawnerScript = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        //
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        var key = Keyboard.current;

        // W: forward movement toggle & speed increase
        if (key.wKey.wasPressedThisFrame && stormLightScript.stormLightEnergy >= 0) // ADDITION added restriction to movement if stormlight is out
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
        if (key.sKey.wasPressedThisFrame && stormLightScript.stormLightEnergy >= 0)  // ADDITION added restriction to movement if stormlight is out
        {
            bool isMoving = rb.linearVelocity.sqrMagnitude > (stillThreshold * stillThreshold);

            if (isMoving)
            {
                // Hover
                hovering = true;
                moveDirection = Vector3.zero;
                currentSpeed = 0f;
                rb.linearVelocity = Vector3.zero;
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

        // ADDITION Added restriction to movement if stormlight is empty

        // Lateral/vertical input
        float x = 0f, y = 0f;
        if (key.aKey.isPressed && stormLightScript.stormLightEnergy >= 0) x -= 1f;
        if (key.dKey.isPressed && stormLightScript.stormLightEnergy >= 0) x += 1f;
        if (key.spaceKey.isPressed && stormLightScript.stormLightEnergy >= 0) y += 1f;
        if (key.leftShiftKey.isPressed && stormLightScript.stormLightEnergy >= 0) y -= 1f;

        Vector3 sideInput = new Vector3(x, y, 0f);
        if (sideInput.sqrMagnitude > 1f) sideInput.Normalize();


        Vector3 forwardMovement = moveDirection * currentSpeed;
        Vector3 strafeMovement = new Vector3(sideInput.x, sideInput.y, 0f) * currentSpeed;


        moveDirection = (forwardMovement + strafeMovement).normalized;

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
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Apply acceleration
        rb.AddForce(moveDirection * currentSpeed, ForceMode.Acceleration);


        //ADDITION
        if (stormLightScript.stormLightEnergy <= 0) rb.AddForce(0, -20, 0);
        if (transform.position.y <= -300) SceneManager.LoadScene(2);
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
        //Debug.Log("Entered trigger: " + other.name);


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
}