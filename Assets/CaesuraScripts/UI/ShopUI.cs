using UnityEngine;
using System.Collections.Generic;
using Caesura.Items;

namespace Caesura.UI
{
    public class ShopUI : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject shopPanel;
        public Transform itemsParent;
        public GameObject shopItemUIPrefab;
        
        [Header("Player")]
        public SimpleInventory playerInventory; 

        private Shop currentShop;
        private List<GameObject> activeRows = new List<GameObject>();

        private void OnEnable()
        {
            Shop.OnShopOpened += OpenShop;
        }

        private void OnDisable()
        {
            Shop.OnShopOpened -= OpenShop;
        }

        public void OpenShop(Shop shop)
        {
            currentShop = shop;
            shopPanel.SetActive(true);
            
            if (playerInventory == null)
            {
                // Attempt to auto-find if not assigned
                playerInventory = FindObjectOfType<SimpleInventory>();
            }

            RefreshBuyMenu();
        }

        public void CloseShop()
        {
            currentShop = null;
            shopPanel.SetActive(false);
        }

        public void RefreshBuyMenu()
        {
            ClearRows();
            if (currentShop == null) return;

            for (int i = 0; i < currentShop.itemsForSale.Count; i++)
            {
                int index = i; // capture index for closure
                GameObject row = Instantiate(shopItemUIPrefab, itemsParent);
                ShopItemUI ui = row.GetComponent<ShopItemUI>();
                ui.SetupBuyRow(currentShop.itemsForSale[i], currentShop.currencyItem, () => OnBuyClicked(index));
                activeRows.Add(row);
            }
        }

        public void RefreshSellMenu()
        {
            ClearRows();
            if (playerInventory == null || currentShop == null) return;
            
            foreach (var slot in playerInventory.GetSlots())
            {
                // Don't let the player sell the currency back for currency!
                if (slot.item != null && slot.number > 0 && slot.item != currentShop.currencyItem)
                {
                    Item itemToSell = slot.item;
                    GameObject row = Instantiate(shopItemUIPrefab, itemsParent);
                    ShopItemUI ui = row.GetComponent<ShopItemUI>();
                    // Defaulting to selling 1 at a time for simplicity
                    ui.SetupSellRow(itemToSell, 1, currentShop.currencyItem, () => OnSellClicked(itemToSell));
                    activeRows.Add(row);
                }
            }
        }

        private void ClearRows()
        {
            foreach (var row in activeRows)
            {
                Destroy(row);
            }
            activeRows.Clear();
        }

        private void OnBuyClicked(int index)
        {
            if (currentShop.BuyItem(index, playerInventory))
            {
                RefreshBuyMenu(); 
            }
            else
            {
                Debug.Log("Not enough currency or item out of stock!");
            }
        }

        private void OnSellClicked(Item item)
        {
            if (currentShop.SellItem(item, 1, playerInventory))
            {
                RefreshSellMenu();
            }
        }
    }
}
