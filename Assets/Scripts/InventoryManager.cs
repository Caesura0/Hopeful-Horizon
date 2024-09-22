using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;


    Dictionary<Item, int> itemList = new Dictionary<Item, int>();

    [SerializeField] GameObject inventoryUISlotPrefab;

    List<InventoryUISlot> inventoryUISlotList = new List<InventoryUISlot>();

    int selectedInventoryIndex = 0;

 
        
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        RedrawUI();
    }

    public int GetInventorySlotCount()
    {
        return inventoryUISlotList.Count;
    }

    public void SelectUISlot(int index)
    {

        if (inventoryUISlotList.Count > index)
        {

            SoundManager.Instance.PlaySwitchItemsSound();
            inventoryUISlotList[selectedInventoryIndex].UpdateSelectedVisual(false);
            selectedInventoryIndex = index;

            inventoryUISlotList[index].UpdateSelectedVisual(true); ;
        }
    }


    public void CycleNextInventorySlot()
    {
        if (inventoryUISlotList.Count == 0)
            return;

        int nextIndex = (selectedInventoryIndex + 1) % inventoryUISlotList.Count;
        SelectUISlot(nextIndex);
    }

    public void CyclePreviousInventorySlot()
    {
        if (inventoryUISlotList.Count == 0)
            return;

        // Subtract 1 and wrap around if the index goes below 0
        int previousIndex = (selectedInventoryIndex - 1 + inventoryUISlotList.Count) % inventoryUISlotList.Count;

        SelectUISlot(previousIndex);
    }


    public bool ContainsItem(ItemType type)
    {
        foreach (Item item in itemList.Keys)
        {
            if (item.itemType == type)
            {
                
                return true;
            }

        }
        return false;
    }

    public Item GetSelectedItem()
    {
        if (inventoryUISlotList.Count == 0) return null;
        return inventoryUISlotList[selectedInventoryIndex].GetItemInSlot(out int count);
    }

    public int CountCurrentMilk()
    {
        foreach (KeyValuePair<Item, int> entry in itemList)
        {
            if (entry.Key.itemType == ItemType.FullMilk)
            {
                return entry.Value;
            }
        }

        return 0; // Return 0 if no FullMilk item is found
    }

    void RedrawUI()
    {
        DestroyAllChildren();
        inventoryUISlotList.Clear();

        int index = 0;
        foreach (var item in itemList)
        {
            var itemSlotGO = Instantiate(inventoryUISlotPrefab.gameObject, this.transform);
            var itemSlot = itemSlotGO.GetComponent<InventoryUISlot>();

            // Set up the UI and assign the click handler for the button
            itemSlot.UpdateUI(item.Key, item.Value);
            int currentIndex = index; // Capture the index for the button click event
            itemSlot.SetButtonCallback(() => SelectUISlot(currentIndex));
            inventoryUISlotList.Add(itemSlot);

            index++;
        }

        SelectUISlot(selectedInventoryIndex);
    }


    public void AddItem(Item item, int quantity)
    {

        if (itemList.ContainsKey(item))
        {
            itemList[item] += quantity;
        }
        else
        {
            itemList[item] = quantity;
        }
        RedrawUI();
    }

    public void RemoveItem(Item item, int quantity)
    {
        if (itemList.ContainsKey(item))
        {
            itemList[item] -= quantity;

            if (itemList[item] <= 0)
            {
                itemList.Remove(item);
            }
        }
        RedrawUI();
    }


    public int GetQuantity(Item item)
    {
        return itemList.ContainsKey(item) ? itemList[item] : 0;
    }
    public void DestroyAllChildren()
    {
        // Loop through all child objects
        foreach (Transform child in transform)
        {
            // Destroy each child GameObject
            Destroy(child.gameObject);
        }
    }
}
