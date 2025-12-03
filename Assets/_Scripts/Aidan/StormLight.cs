using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StormLight : MonoBehaviour
{
    public TextMeshProUGUI lowText;
    private bool blinking = false;
    private float flashDuration = 0.2f;

    public float stormLightEnergy = 100f; //starting stormlight value
    private float stormLightUsage = 2f; // how quickly the player loses stomlight
    public Image stormLightBar;

    private int _reductionHeight = -243;
    private int _reductionHeightSuper = -100;

    private PlayerFlight player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerFlight>();
        lowText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        StormLightReduction();
        if (stormLightEnergy <= 20 && blinking == false)
        {
            Debug.Log("IF");
            blinking = true;
            StartCoroutine(Flashing());
        }
        else if (stormLightEnergy >= 20 && blinking == true)
        {
            Debug.Log("Else");
            blinking = false;
            StopCoroutine(Flashing());
            lowText.gameObject.SetActive(false);
        }
    }

    private IEnumerator Flashing()
    {
        while (blinking)
        {
            lowText.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            lowText.gameObject.SetActive(false);
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void StormLightReduction() // reduce stormlight over time
    {
        if (player.transform.position.y < _reductionHeight) stormLightUsage = 1;
        if (player.transform.position.y > _reductionHeight && player.transform.position.y < _reductionHeightSuper) stormLightUsage = 10;
        if (player.transform.position.y > _reductionHeightSuper) stormLightUsage = 20;

        stormLightEnergy = Mathf.Clamp(stormLightEnergy, 0, 100); //Stormlight energy can only be between 0-100
        stormLightEnergy -= stormLightUsage * Time.deltaTime; //every second stormlight drops by usage amount
        stormLightBar.fillAmount = stormLightEnergy / 100f; // controls the ui of stormlight bar 
    }

}
