using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public TextMeshProUGUI _text;
    public string textValue;
    public float textSpeed;
    private void Start()
    {
        _text.text = textValue;
        Destroy(gameObject, textSpeed);
    }
}
