using UnityEngine;

public class FollowingEnemy : Enemy
{
    private float speed = 0.025f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
    }
}
