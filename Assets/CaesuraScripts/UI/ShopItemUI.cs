using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Caesura.Items;

namespace Caesura.UI
{
    public class ShopItemUI : MonoBehaviour
    {
        public TextMeshProUGUI nameText; 
        public TextMeshProUGUI priceText;
        public Image iconImage;
        public Button actionButton;
        public TextMeshProUGUI buttonText;

        public void SetupBuyRow(ShopItem shopItem, Item currency, Action onClick)
        {
            nameText.text = shopItem.item.GetDisplayName();
            if (iconImage != null && shopItem.item.GetIcon() != null)
                iconImage.sprite = shopItem.item.GetIcon();
            
            if (shopItem.isLimited)
            {
                nameText.text += $" (Stock: {shopItem.currentStock})";
            }

            priceText.text = $"Cost: {shopItem.price} {currency.GetDisplayName()}";
            buttonText.text = "Buy";

            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => onClick?.Invoke());
            
            if (shopItem.isLimited && shopItem.currentStock <= 0)
            {
                actionButton.interactable = false;
                buttonText.text = "Sold Out";
            }
            else
            {
                actionButton.interactable = true;
            }
        }

        public void SetupSellRow(Item item, int quantityToSell, Item currency, Action onClick)
        {
            nameText.text = $"{item.GetDisplayName()}";
            if (iconImage != null && item.GetIcon() != null)
                iconImage.sprite = item.GetIcon();
                
            int totalValue = item.GetSellValue() * quantityToSell;
            priceText.text = $"Value: {totalValue} {currency.GetDisplayName()}";
            buttonText.text = "Sell";

            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => onClick?.Invoke());
            actionButton.interactable = true;
        }
    }
}
