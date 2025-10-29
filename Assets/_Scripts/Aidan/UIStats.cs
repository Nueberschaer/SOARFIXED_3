using TMPro;
using UnityEngine;

public class UIStats : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI killsText;

    PlayerFlight playerScript;

    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFlight>(); //Reference to the player script
        
    }

    private void FixedUpdate()
    {
        DistanceDisplay();
        KillsDisplay();
    }

    private void DistanceDisplay()
    {
        int distance = playerScript.distance;
        distanceText.text = distance.ToString();
    }

    private void KillsDisplay()
    {
        int kills = playerScript.kills;
        killsText.text = kills.ToString();
    }
}
