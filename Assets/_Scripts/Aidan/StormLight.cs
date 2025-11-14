using UnityEngine;
using UnityEngine.UI;

public class StormLight : MonoBehaviour
{

    public float stormLightEnergy = 100f; //starting stormlight value
    private float stormLightUsage = 1f; // how quickly the player loses stomlight
    public Image stormLightBar;

    private int _reductionHeight = -235;
    private int _reductionHeightSuper = -100;

    private PlayerFlight player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerFlight>();
    }

    // Update is called once per frame
    void Update()
    {
        StormLightReduction();
    }

    private void StormLightReduction() // reduce stormlight over time
    {
        if (player.transform.position.y < _reductionHeight) stormLightUsage = 1;
        if (player.transform.position.y > _reductionHeight && player.transform.position.y < _reductionHeightSuper) stormLightUsage = 5;
        if (player.transform.position.y > _reductionHeightSuper) stormLightUsage = 10;

        stormLightEnergy = Mathf.Clamp(stormLightEnergy, 0, 100); //Stormlight energy can only be between 0-100
        stormLightEnergy -= stormLightUsage * Time.deltaTime; //every second stormlight drops by usage amount
        stormLightBar.fillAmount = stormLightEnergy / 100f; // controls the ui of stormlight bar 
    }

}
