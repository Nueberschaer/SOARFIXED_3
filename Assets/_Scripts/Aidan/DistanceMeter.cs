using UnityEngine;
using UnityEngine.UI;

public class DistanceMeter : MonoBehaviour
{
    public Image distanceMeter;
    private float _goal = 16500f;
    private PlayerFlight _playerScript;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerScript = GameObject.Find("Player").GetComponent<PlayerFlight>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowDistance();
    }

    private void ShowDistance()
    {
        distanceMeter.fillAmount = (_playerScript.distance + 273) / _goal;
    }
}
