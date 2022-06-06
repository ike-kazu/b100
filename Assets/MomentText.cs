using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MomentText : MonoBehaviour
{
    public byte[] color;
    Vector3 startPos;
    void Start()
    {
        startPos = transform.localPosition;   
    }

    void Update()
    {
        
    }

    public IEnumerator Moment()
    {
        transform.localPosition = startPos;
        GetComponent<Text>().enabled = true;

        byte fade = 255;
        float x = 0.02f;
        for (float t = 0; t < 50; t++)
        {
            x *= 0.8f;
            transform.Translate(0, x * 0.5f, 0);
            GetComponent<Text>().color = new Color32(color[0], color[1], color[2], fade);
            fade -= 5;
            yield return null;
        }

        GetComponent<Text>().enabled = false;
    }
}
