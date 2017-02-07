using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : System.Object
{
    public string name;
    [Multiline]
    public string description;
    public Texture2D sprite;
}

public class ItemController : MonoBehaviour
{
    public Item[] items;

    void Update()
    {

    }
}
