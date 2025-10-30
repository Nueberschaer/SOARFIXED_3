using TMPro;
using UnityEngine;

public class UIStats : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI killsText;
    private PlayerFlight playerScript;
    private Sword swordScript;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFlight>(); //Reference to the player script
        swordScript = GameObject.FindGameObjectWithTag("Sword").GetComponent<Sword>(); //Reference to the player script
        
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
        int kills = swordScript.kills;
        killsText.text = kills.ToString();
    }
}
