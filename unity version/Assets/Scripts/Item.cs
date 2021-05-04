using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "Item name";
    public string itemDescription = "Item description";
    public ItemType itemType = ItemType.def;
    
}

public enum ItemType {
    type1,
    type2,
    type3,
    def
}