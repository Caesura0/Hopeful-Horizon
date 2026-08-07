using System;
using System.Collections.Generic;
using UnityEngine;
using Caesura.Utils;

namespace Caesura.Items
{
    public class SimpleInventory : MonoBehaviour, IConditionEvaluator, ICaesuraSaveable
    {
        [System.Serializable]
        public class InventorySlot
        {
            public Item item;
            public int number;
        }

        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        public event Action onInventoryUpdated;

        public bool AddItem(Item item, int amount)
        {
            if (item == null || amount <= 0) return false;

            if (item.IsStackable())
            {
                // Find an existing slot with the same item
                foreach (var slot in slots)
                {
                    if (slot.item == item)
                    {
                        slot.number += amount;
                        onInventoryUpdated?.Invoke();
                        return true;
                    }
                }
            }

            // Find an existing empty slot
            foreach (var slot in slots)
            {
                if (slot.item == null)
                {
                    slot.item = item;
                    slot.number = amount;
                    onInventoryUpdated?.Invoke();
                    return true;
                }
            }

            // Otherwise, create a new slot
            slots.Add(new InventorySlot { item = item, number = amount });
            onInventoryUpdated?.Invoke();
            return true;
        }

        public bool HasItem(Item item, int amount = 1)
        {
            int currentAmount = 0;
            foreach (var slot in slots)
            {
                if (slot.item == item)
                {
                    currentAmount += slot.number;
                }
            }
            return currentAmount >= amount;
        }

        public void RemoveItem(Item item, int amount)
        {
            int amountLeftToRemove = amount;
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                if (slots[i].item == item)
                {
                    if (slots[i].number > amountLeftToRemove)
                    {
                        slots[i].number -= amountLeftToRemove;
                        amountLeftToRemove = 0;
                        break;
                    }
                    else
                    {
                        amountLeftToRemove -= slots[i].number;
                        slots[i].item = null;
                        slots[i].number = 0;
                    }
                }
            }
            onInventoryUpdated?.Invoke();
        }

        public IEnumerable<InventorySlot> GetSlots()
        {
            return slots;
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            if (predicate == "HasItem")
            {
                // Item Name is parameters[0], quantity is parameters[1]
                Item itemToCheck = null;
                // We need to find the item by ID. Since we are storing string IDs in parameters[0] in PredicatePropertyDrawer
                // We should find the item. But wait, at runtime, how do we get the Item reference by ID?
                // Again, without Resources.LoadAll, we'd need an ItemDatabase.
                // Let's implement a quick fix for this standalone script: we just check our own slots!
                // If the parameter is the item ID, we can just check if ANY item in our inventory has that ID!
                foreach (var slot in slots)
                {
                    if (slot.item.GetItemID() == parameters[0])
                    {
                        itemToCheck = slot.item;
                        break;
                    }
                }

                // If we don't even have the item in inventory at all, then we definitely don't have the quantity
                if (itemToCheck == null) return false;

                int requiredQuantity = 1;
                if (parameters.Length > 1 && int.TryParse(parameters[1], out int qty))
                {
                    requiredQuantity = qty;
                }

                return HasItem(itemToCheck, requiredQuantity);
            }
            return null;
        }

        [System.Serializable]
        public struct InventoryRecord
        {
            public string itemID;
            public int number;
        }

        public object CaptureState()
        {
            List<InventoryRecord> records = new List<InventoryRecord>();
            foreach (var slot in slots)
            {
                if (slot.item != null)
                {
                    records.Add(new InventoryRecord { itemID = slot.item.GetItemID(), number = slot.number });
                }
            }
            return records;
        }

        public void RestoreState(object state, CaesuraDatabase database)
        {
            List<InventoryRecord> records = state as List<InventoryRecord>;
            if (records == null) return;

            slots.Clear();
            foreach (var record in records)
            {
                Item foundItem = null;
                if (database != null)
                {
                    foundItem = database.GetItemByID(record.itemID);
                }
                else
                {
                    foundItem = Item.GetFromID(record.itemID);
                }

                if (foundItem != null)
                {
                    slots.Add(new InventorySlot { item = foundItem, number = record.number });
                }
            }
            onInventoryUpdated?.Invoke();
        }
    }
}
