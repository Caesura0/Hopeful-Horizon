using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FarmableSoil : MonoBehaviour, IItemInteractable
{

    [SerializeField] Sprite[] wateredSoilTiles;
    [SerializeField] Sprite transpartentSprite;
    [SerializeField] Sprite plantSprite;
    [SerializeField] SpriteRenderer wateredRenderer;
    [SerializeField] SpriteRenderer plantRenderer;

    [SerializeField] LayerMask plantable;
    [SerializeField] LayerMask waterable;
    [SerializeField] LayerMask interactable;

    public static event Action OnAnyTilePlantedAndWatered;

    bool isWatered;
    bool isPlanted;



    public bool ItemInteract(Player player)
    {
        if (InventoryManager.Instance.GetSelectedItem() != null )
        {
            var selectedItem = InventoryManager.Instance.GetSelectedItem().itemType;

            switch (selectedItem)
            {
                case ItemType.WateringCan:
                    return RandomWateredTile(); 
                    
                    
                case ItemType.Seed:
                    return SeedPlanted();
                    
            }


        }
        return false;
    }


    bool RandomWateredTile()
    {
        if (!isWatered)
        {
            var range = UnityEngine.Random.Range(0, wateredSoilTiles.Length);
            wateredRenderer.sprite = wateredSoilTiles[range];
            wateredRenderer.gameObject.SetActive(true);
            isWatered = true;
            Mathf.RoundToInt(Mathf.Log(plantable.value, 2));
            if (isPlanted)
            {
                OnAnyTilePlantedAndWatered?.Invoke();
            }
            return true;
        }
        else
            return false;
    }

    public void DewaterTile()
    {
        wateredRenderer.gameObject.SetActive(false);
        isWatered = false;
        gameObject.layer = Mathf.RoundToInt(Mathf.Log(waterable.value, 2)); ;
    }

    bool SeedPlanted()
    {
        if (!isPlanted)
        {
            var item = InventoryManager.Instance.GetSelectedItem();
            if (item.itemType == ItemType.Seed)
            {
                plantRenderer.sprite = plantSprite;
                plantRenderer.gameObject.SetActive(true);
                isPlanted = true;

                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.GetSelectedItem(), 1);

                if (isWatered)
                {
                    OnAnyTilePlantedAndWatered?.Invoke();
                }
                return true;
            }


        }
        return false;


    }





    public void ResetPlantedTile()
    {

        plantRenderer.gameObject.SetActive(false);
        isPlanted = false;
    }

    public void Interact(Player player)
    {
        throw new System.NotImplementedException();
    }
}
