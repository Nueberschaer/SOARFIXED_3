using System.Collections;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TextTime());
    }

    // Update is called once per frame
    private IEnumerator TextTime()
    {
        yield return new WaitForSeconds(6);
        this.gameObject.SetActive(false);
    }
}
