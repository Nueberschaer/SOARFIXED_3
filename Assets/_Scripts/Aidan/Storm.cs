using UnityEngine;

public class Storm : MonoBehaviour
{

    private float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3 (-1, 0, 0) * speed * Time.deltaTime; // constanly moves storm forward to chase player
    }

}
