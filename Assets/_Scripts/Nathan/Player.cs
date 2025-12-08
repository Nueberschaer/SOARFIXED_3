using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerFlight : MonoBehaviour
{
    public GameObject popUpDeathPrefab;
    public GameObject popUpSkyPrefab;
    public GameObject popUpOrbPrefab;
    
    public int distance = 0;
    private float stormSpeed = -10000f;
    StormLight stormLightScript;
    EnemySpawner enemySpawnerScript;

    public Camera playerCamera;
    public float baseFOV = 60f;
    public float maxFOV = 90f;
    public float fovLerpSpeed = 3f;
    public float fovAtSpeed = 50f;

    public float baseSpeed = 50f;
    public float speedIncrement = 20f;
    public float maxSpeed = 250f;
    public float stillThreshold = 0.2f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed = 0f;
    private bool hovering = false;
    private float displayedSpeed;

    public float hoverBrake = 6f;
    public float accelPerSecond = 125f;

    public float mouseSensitivity = 40f;
    public float minPitch = -80f;
    public float maxPitch = 80f;
    public bool invertY = false;

    private float camYaw;
    private float camPitch;
    private float camRoll;
    private float ignoreMouseUntilTime = 0f;
    private bool isOrbTextActive = false;

    private bool deathStarted, winStarted, loadingScene;
    private static readonly WaitForSeconds Wait3 = new WaitForSeconds(3f);

    private int skipMouseFrames = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 4f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        GameObject camGO = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (camGO != null)
            playerCamera = camGO.GetComponent<Camera>();
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>() ?? Camera.main;

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = baseFOV;
            Transform t = playerCamera.transform;
            Vector3 e = t.parent ? t.localEulerAngles : t.eulerAngles;
            camYaw = e.y;
            float px = e.x; if (px > 180f) px -= 360f;
            camPitch = Mathf.Clamp(px, minPitch, maxPitch);
            camRoll = e.z;
        }

        var stormGO = GameObject.FindGameObjectWithTag("StormLight");
        if (stormGO) stormLightScript = stormGO.GetComponent<StormLight>();
        var spawnerGO = GameObject.FindGameObjectWithTag("EnemySpawner");
        if (spawnerGO) enemySpawnerScript = spawnerGO.GetComponent<EnemySpawner>();

        ignoreMouseUntilTime = Time.unscaledTime + 0.25f;
    }

    void Start()
    {
        StartCoroutine(InitialLock());
    }

    private IEnumerator InitialLock()
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        if (Time.timeScale > 0f)
        {
            SetCursorLocked(true);
            ignoreMouseUntilTime = Time.unscaledTime + 0.15f;
            skipMouseFrames = 2;
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Time.timeScale == 0f)
        {
            if (Cursor.lockState != CursorLockMode.None) SetCursorLocked(false);
        }
        else
        {
            if (Cursor.lockState != CursorLockMode.Locked) SetCursorLocked(true);
            #if UNITY_EDITOR
            if (Cursor.visible) Cursor.visible = false; 
            #endif
        }

        if (playerCamera != null && Mouse.current != null && Cursor.lockState == CursorLockMode.Locked)
        {
            Transform t = playerCamera.transform;
            Quaternion q = Quaternion.Euler(camPitch, camYaw, camRoll);
            if (t.parent) t.localRotation = q; else t.rotation = q;

            if (Time.unscaledTime >= ignoreMouseUntilTime)
            {
                if (skipMouseFrames > 0)
                {
                    _ = Mouse.current.delta.ReadValue();
                    skipMouseFrames--;
                }
                else
                {
                    Vector2 md = Mouse.current.delta.ReadValue();
                    float s = mouseSensitivity * 0.001f;
                    camYaw += md.x * s;
                    float dy = (invertY ? md.y : -md.y) * s;
                    camPitch = Mathf.Clamp(camPitch + dy, minPitch, maxPitch);
                }
            }
        }

        Vector3 v = rb.linearVelocity;
        float x = 0f, y = 0f, z = 0f;
        if (Keyboard.current.aKey.isPressed && (stormLightScript == null || stormLightScript.stormLightEnergy >= 0)) x -= 1f;
        if (Keyboard.current.dKey.isPressed && (stormLightScript == null || stormLightScript.stormLightEnergy >= 0)) x += 1f;
        if (Keyboard.current.spaceKey.isPressed && (stormLightScript == null || stormLightScript.stormLightEnergy >= 0)) y += 1f;
        if (Keyboard.current.leftShiftKey.isPressed && (stormLightScript == null || stormLightScript.stormLightEnergy >= 0)) y -= 1f;
        if (Keyboard.current.wKey.isPressed && (stormLightScript == null || stormLightScript.stormLightEnergy >= 0)) z += 1f;

        Vector3 desiredInput = new Vector3(x, y, z);
        if (desiredInput.sqrMagnitude > 1f) desiredInput.Normalize();

        bool sPressed = Keyboard.current.sKey.isPressed && (stormLightScript == null || stormLightScript.stormLightEnergy >= 0);
        if (sPressed && rb.linearVelocity.sqrMagnitude > (stillThreshold * stillThreshold))
        {
            hovering = true;
            moveDirection = Vector3.zero;
        }
        else if (desiredInput.sqrMagnitude > 0f)
        {
            hovering = false;
            moveDirection = desiredInput;
        }

        distance = (int)transform.position.z;
    }

    void FixedUpdate()
    {
        float targetSpeed = (hovering || moveDirection == Vector3.zero) ? 0f : baseSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelPerSecond * Time.fixedDeltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        if (hovering)
            rb.AddForce(-rb.linearVelocity * hoverBrake, ForceMode.Acceleration);
        else
            rb.AddForce(moveDirection * currentSpeed, ForceMode.Acceleration);

        if (stormLightScript != null && stormLightScript.stormLightEnergy <= 0)
            rb.AddForce(Vector3.down * 50f, ForceMode.Acceleration);

        if (!deathStarted && stormLightScript != null &&
            stormLightScript.stormLightEnergy <= 0 && transform.position.z <= 16200f)
        {
            deathStarted = true;
            StartCoroutine(Death());
        }

        if (!winStarted && transform.position.z >= 16450f)
        {
            winStarted = true;
            StartCoroutine(Winner());
        }

        if (!loadingScene && transform.position.y <= -400f)
        {
            loadingScene = true;
            SceneManager.LoadScene(2);
        }
    }

    void LateUpdate()
    {
        if (!playerCamera) return;
        float actualSpeed = rb.linearVelocity.magnitude;
        displayedSpeed = Mathf.Lerp(displayedSpeed, actualSpeed, Time.deltaTime * fovLerpSpeed);
        float speedPercent = Mathf.Clamp01(displayedSpeed / Mathf.Max(0.01f, fovAtSpeed));
        float targetFOV = Mathf.Lerp(baseFOV, maxFOV, speedPercent);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovLerpSpeed);
    }

    private IEnumerator Death()
    {
        GameObject popUpObject = Instantiate(popUpDeathPrefab);
        popUpObject.GetComponent<PopUp>().textSpeed = 3f;
        popUpObject.GetComponent<PopUp>().textValue = "GOODBYE ROSHAR";
        yield return Wait3;
        deathStarted = false;
        if (stormLightScript != null && stormLightScript.stormLightEnergy <= 0)
        {
            SetCursorLocked(false);
            SceneManager.LoadScene(2);
        }
    }

    private IEnumerator Winner()
    {
        yield return Wait3;
        SceneManager.LoadScene(3);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Storm"))
            rb.AddForce(0f, stormSpeed, 0f);
        if (other.CompareTag("Orb"))
        {
            
            if (stormLightScript != null) stormLightScript.stormLightEnergy = 100;
            other.gameObject.SetActive(false);
            if (isOrbTextActive == false)
            {
                isOrbTextActive = true;
                GameObject popUpOrb = Instantiate(popUpOrbPrefab);
                popUpOrb.GetComponent<PopUp>().textSpeed = 1f;
                popUpOrb.GetComponent<PopUp>().textValue = "STORMLIGHT REPLENISH ME";
                isOrbTextActive = false;
            }
            
        }
        if (other.CompareTag("Level2")) enemySpawnerScript?.LevelTwoEnemies();
        if (other.CompareTag("Level3")) enemySpawnerScript?.LevelThreeEnemies();
        if (other.CompareTag("Level4")) enemySpawnerScript?.LevelFourEnemies();
        if (other.CompareTag("Level5")) enemySpawnerScript?.LevelFiveEnemies();

        if (other.CompareTag("SkyDetector"))
        {
            other.gameObject.SetActive(false);
            GameObject popUpObject = Instantiate(popUpSkyPrefab);
            popUpObject.GetComponent<PopUp>().textSpeed = 5f;
            popUpObject.GetComponent<PopUp>().textValue = "So calm, if only I could stay up here forever...";
        }
    }

    void OnDisable()
    {
        SetCursorLocked(false);
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && Time.timeScale > 0f && Cursor.lockState != CursorLockMode.Locked)
            SetCursorLocked(true);
        if (!hasFocus && Cursor.lockState != CursorLockMode.None && Time.timeScale == 0f)
            SetCursorLocked(false);
    }

    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        SetCursorLocked(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetCursorLocked(true);
        ignoreMouseUntilTime = Time.unscaledTime + 0.15f;
        skipMouseFrames = 2;
    }
    
    private void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
        if (locked)
        {
            ignoreMouseUntilTime = Time.unscaledTime + 0.15f;
            skipMouseFrames = 2;
            #if ENABLE_INPUT_SYSTEM
            try { UnityEngine.InputSystem.InputSystem.ResetDevice(Mouse.current); } catch { }
            #endif
        }
        else
        {
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
