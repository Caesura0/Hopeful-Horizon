using UnityEngine;

namespace Caesura.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Caesura/Item", order = 0)]
    public class Item : ScriptableObject
    {
        [Tooltip("A unique identifier for the item.")]
        [SerializeField] private string itemID = System.Guid.NewGuid().ToString();
        
        [Tooltip("The display name of the item.")]
        [SerializeField] private string displayName = "New Item";
        
        [Tooltip("The description of the item.")]
        [TextArea]
        [SerializeField] private string description = "A simple item.";
        
        [Tooltip("The icon to display in UI.")]
        [SerializeField] private Sprite icon;
        
        [Tooltip("Whether multiple of this item can stack in a single inventory slot.")]
        [SerializeField] private bool isStackable = true;

        [Tooltip("The base value of this item if sold to a shop.")]
        [SerializeField] private int sellValue = 10;

        public string GetItemID() => itemID;
        public string GetDisplayName() => displayName;
        public string GetDescription() => description;
        public Sprite GetIcon() => icon;
        public bool IsStackable() => isStackable;
        public int GetSellValue() => sellValue;

        public static Item GetFromID(string itemID)
        {
            foreach (Item item in Resources.LoadAll<Item>(""))
            {
                if (item.GetItemID() == itemID)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
