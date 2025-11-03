using UnityEngine;

public class Follower : Enemy
{

    private Transform player;
    private float speed = 0.1f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Body").GetComponent<Transform>();
    }

    void Update()
    {
        //float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
    }
}
