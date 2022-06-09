using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTile : MonoBehaviour
{
    public GameObject[] wall;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Create(int d)
    {
        wall[d].SetActive(true);
    }
}
