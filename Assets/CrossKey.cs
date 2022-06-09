using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossKey : MonoBehaviour
{
    public int direction;
    public TileManager tileManager;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void OnMouseDown()
    {
        StartCoroutine("Tap");
    }

    void OnMouseUp()
    {
        StopCoroutine("Tap");
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    IEnumerator Tap()
    {
        while (true)
        {
            tileManager.Move(direction);
            GetComponent<SpriteRenderer>().color = new Color32(192, 192, 192, 255);
            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
    }
}
