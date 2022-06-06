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
        tileManager.Move(direction);
        StartCoroutine("Tap");
    }

    IEnumerator Tap()
    {
        GetComponent<SpriteRenderer>().color = new Color32(192, 192, 192, 255);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
}
