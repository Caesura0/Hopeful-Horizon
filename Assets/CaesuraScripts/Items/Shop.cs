using System;
using System.Collections.Generic;
using UnityEngine;

namespace Caesura.Items
{
    [System.Serializable]
    public class ShopItem
    {
        [Tooltip("The item being sold.")]
        public Item item;
        
        [Tooltip("How much currency this item costs.")]
        public int price = 10;
        
        [Tooltip("If true, the shop only has a limited stock of this item.")]
        public bool isLimited = false;
        
        [Tooltip("The current amount in stock (if limited).")]
        public int currentStock = 5;
    }

    public class Shop : MonoBehaviour
    {
        [Tooltip("The currency this shop trades in (e.g., Gold Coin).")]
        public Item currencyItem;
        
        [Tooltip("The list of items this shop offers.")]
        public List<ShopItem> itemsForSale = new List<ShopItem>();

        public static event Action<Shop> OnShopOpened;

        // Call this via your interaction system (e.g., raycast on the NPC or a trigger volume)
        public void Interact()
        {
            if (currencyItem == null)
            {
                Debug.LogError("Shop has no currency item assigned!");
                return;
            }
            OnShopOpened?.Invoke(this);
        }

        public bool BuyItem(int index, SimpleInventory buyerInventory)
        {
            if (index < 0 || index >= itemsForSale.Count) return false;
            
            ShopItem shopItem = itemsForSale[index];
            
            // Check stock
            if (shopItem.isLimited && shopItem.currentStock <= 0) return false;

            // Check currency
            if (!buyerInventory.HasItem(currencyItem, shopItem.price)) return false;

            // Execute transaction
            buyerInventory.RemoveItem(currencyItem, shopItem.price);
            buyerInventory.AddItem(shopItem.item, 1);

            if (shopItem.isLimited)
            {
                shopItem.currentStock--;
            }
            return true;
        }

        public bool SellItem(Item itemToSell, int amount, SimpleInventory sellerInventory)
        {
            // Verify player actually has the item
            if (!sellerInventory.HasItem(itemToSell, amount)) return false;

            int totalValue = itemToSell.GetSellValue() * amount;
            
            // Execute transaction
            sellerInventory.RemoveItem(itemToSell, amount);
            sellerInventory.AddItem(currencyItem, totalValue);
            return true;
        }
    }
}
