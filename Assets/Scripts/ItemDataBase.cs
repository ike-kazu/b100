using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateItemDataBase")]
[System.Serializable]
public class ItemDataBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class ItemData
{
    public string name;
    public int attacker;
    public int speed;
    public int defence;
    public string detail;
    // Head, Body, Hand, Boots,
    public string Place;
}