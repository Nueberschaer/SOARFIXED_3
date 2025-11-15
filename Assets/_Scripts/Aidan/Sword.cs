using UnityEngine;

public class Sword : MonoBehaviour
{
    public int kills = 0;
    private StormLight _storLightScript;

    private void Awake()
    {
        _storLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fused")
        {
            _storLightScript.stormLightEnergy += 5;
            kills += 1;
        }
        if (other.gameObject.tag == "Advancer")
        {
            _storLightScript.stormLightEnergy += 10;
            kills += 1;
        }
        if (other.gameObject.tag == "Follower")
        {
            _storLightScript.stormLightEnergy += 15;
            kills += 1;
        }
    }
}
