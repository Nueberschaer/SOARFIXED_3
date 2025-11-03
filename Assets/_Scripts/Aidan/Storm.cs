using System.Collections;
using UnityEngine;

public class Storm : MonoBehaviour
{

    private float speed = 5f;
    private bool startMoving = false;

    private void Start()
    {
        StartCoroutine(MoveDelay());
    }

    void Update()
    {
        if (startMoving == true) transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime; // constanly moves storm forward to chase player
    }
    IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds (5);
        startMoving = true;
    }
}
