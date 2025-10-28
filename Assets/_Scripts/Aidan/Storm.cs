using UnityEngine;

public class Storm : MonoBehaviour
{

    private float speed = 5f;

    void Update()
    {
        transform.position += new Vector3 (1, 0, 0) * speed * Time.deltaTime; // constanly moves storm forward to chase player
    }

}
