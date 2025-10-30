using UnityEngine;

public class Sword : MonoBehaviour
{
    public int kills = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") kills += 1;
    }
}
