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
        if (other.gameObject.tag == "KillDetection") kills += 1;
        if (other.gameObject.tag == "Fused")
        {
            Debug.Log("Fused");
            _storLightScript.stormLightEnergy += 5;
        }
        if (other.gameObject.tag == "Advancer")
        {
            Debug.Log("Advancer");
            _storLightScript.stormLightEnergy += 10;
        }
        if (other.gameObject.tag == "Follower")
        {
            Debug.Log("Follower");
            _storLightScript.stormLightEnergy += 15;
        }
    }
}
