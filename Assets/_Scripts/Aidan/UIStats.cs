using TMPro;
using UnityEngine;

public class UIStats : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI killsText;
    private PlayerFlight playerScript;
    private Enemy enemyScript;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFlight>(); //Reference to the player script
        enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        
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
        int kills = enemyScript.kills;
        killsText.text = kills.ToString();
    }
}
