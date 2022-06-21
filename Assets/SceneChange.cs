using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public bool start;

    void Start()
    {
        if (start) StartCoroutine("StartBehaviour");
        if (!start) StartCoroutine("EndBehaviour");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartBehaviour()
    {
        byte fade = 255;
        float x = 0.02f;
        for (float t = 0; t < 25; t++)
        {
            GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, fade);
            fade -= 10;
            yield return null;
        }

        GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, 0);
    }

    public IEnumerator EndBehaviour()
    {
        byte fade = 0;
        float x = 0.02f;
        for (float t = 0; t < 25; t++)
        {
            GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, fade);
            fade += 10;
            yield return null;
        }

        GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, 255);
    }
}
