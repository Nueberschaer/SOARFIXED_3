using UnityEngine;
using UnityEngine.UI;

public class StormLight : MonoBehaviour
{

    public float stormLightEnergy = 100f; //starting stormlight value
    private float stormLightUsage = 1f; // how quickly the player loses stomlight
    public Image stormLightBar;


    // Update is called once per frame
    void Update()
    {
        StormLightReduction();
    }

    private void StormLightReduction() // reduce stormlight over time
    {
        stormLightEnergy = Mathf.Clamp(stormLightEnergy, 0, 100); //Stormlight energy can only be between 0-100
        stormLightEnergy -= stormLightUsage * Time.deltaTime; //every second stormlight drops by usage amount which is 4 every second
        stormLightBar.fillAmount = stormLightEnergy / 100f; // controls the ui of stormlight bar 
    }

}
