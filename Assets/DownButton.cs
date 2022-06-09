using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownButton : MonoBehaviour
{
    public TileManager tm;
    Vector3 startSize;

    void Start()
    {
        startSize = transform.localScale;
    }

    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        transform.localScale = startSize * 1.1f;
    }

    private void OnMouseExit()
    {
        transform.localScale = startSize;
    }

    private void OnMouseUp()
    {
        transform.localScale = startSize;
        tm.StartCoroutine("Next");
        gameObject.SetActive(false);
    }
}
