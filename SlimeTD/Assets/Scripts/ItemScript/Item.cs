using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "Custom/Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public int Count;
}
