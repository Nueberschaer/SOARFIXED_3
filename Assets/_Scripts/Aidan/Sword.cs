using UnityEngine;

public class Sword : MonoBehaviour
{
    public int kills = 0;
    private StormLight _storLightScript;

    public GameObject popUpPrefab;

    private void Awake()
    {
        _storLightScript = GameObject.FindGameObjectWithTag("StormLight").GetComponent<StormLight>();
    }

    private void KillsText()
    {
        GameObject popUpObject = Instantiate(popUpPrefab);
        popUpObject.GetComponent<PopUp>().textSpeed = 1.5f;

        int luckyNumber = Random.Range(0, 3);
        if (luckyNumber == 0) popUpObject.GetComponent<PopUp>().textValue = "SLAIN";
        else if (luckyNumber == 1) popUpObject.GetComponent<PopUp>().textValue = "TAKEN DOWN";
        else popUpObject.GetComponent<PopUp>().textValue = "ANOTHER ONE";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fused")
        {
           KillsText();
            _storLightScript.stormLightEnergy += 5;
            kills += 1;
        }
        if (other.gameObject.tag == "Advancer")
        {
            KillsText();
            _storLightScript.stormLightEnergy += 10;
            kills += 1;
        }
        if (other.gameObject.tag == "Follower")
        {
            KillsText();
            _storLightScript.stormLightEnergy += 15;
            kills += 1;
        }
    }
}
