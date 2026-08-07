using Caesura.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PickupItem : MonoBehaviour , IInteractable
{

    [SerializeField] Item itemSO;
    [SerializeField] int quantity = 1;
    //[SerializeField] int quantity;

    private void Start()
    {
        if (itemSO != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemSO.GetIcon();
        }
    }

    public void Interact(Player player)
    {
        player.AddItem(itemSO, quantity);
        Destroy(gameObject);
        //playsound;
    }
}

//public enum ItemType
//{
//    Bottle,
//    Hoe,
//    PickAxe,
//    Axe,
//    WateringCan,
//    Seed,
//    FullMilk

//}

//[System.Serializable]
//public struct Item
//{
//    public string itemName;
//    public Sprite itemSprite;
//    public ItemType itemType;

//    public static bool operator ==(Item lhs, Item rhs)
//    {
//        return lhs.Equals(rhs);
//    }

//    // Override the inequality operator
//    public static bool operator !=(Item lhs, Item rhs)
//    {
//        return !lhs.Equals(rhs);
//    }

//    // Override the Equals method
//    public override bool Equals(object obj)
//    {
//        if (!(obj is Item))
//            return false;

//        Item other = (Item)obj;
//        return itemName == other.itemName &&
//               itemSprite == other.itemSprite &&
//               itemType == other.itemType;
//    }

//    // Override the GetHashCode method
//    public override int GetHashCode()
//    {
//        int hash = 17;
//        hash = hash * 31 + (itemName != null ? itemName.GetHashCode() : 0);
//        hash = hash * 31 + (itemSprite != null ? itemSprite.GetHashCode() : 0);
//        hash = hash * 31 + itemType.GetHashCode();
//        return hash;
//    }
//}

