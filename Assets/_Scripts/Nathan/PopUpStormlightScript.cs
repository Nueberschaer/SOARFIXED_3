using TMPro;
using UnityEngine;


public class PopUpStormlightScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI text;
    public string textValue;
    public float textSpeed;
    void Start()
    {
        text.text = textValue;
        Destroy(gameObject, textSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
