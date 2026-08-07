using Caesura.Items;
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

    bool isTilled;
    bool isWatered;
    bool isPlanted;
    
    SpriteRenderer baseRenderer;
    Sprite tilledSprite;

    private void Awake()
    {
        baseRenderer = GetComponent<SpriteRenderer>();
        if (baseRenderer != null)
        {
            tilledSprite = baseRenderer.sprite; // Save the dry dirt sprite
            if (transpartentSprite != null)
            {
                baseRenderer.sprite = transpartentSprite; // Start as untilled (transparent)
            }
        }
    }

    private void OnEnable()
    {
        TimeManager.OnDayChanged += OnDayChanged;
    }

    private void OnDisable()
    {
        TimeManager.OnDayChanged -= OnDayChanged;
    }

    private void OnDayChanged(int newDay)
    {
        if (isTilled && isWatered)
        {
            // Soil absorbs the water and dries out overnight
            DewaterTile();
        }
    }



    public bool ItemInteract(Player player)
    {
        if (HotbarManager.Instance.GetSelectedItem() != null )
        {
            var item = HotbarManager.Instance.GetSelectedItem();

            if (item is HoeTool)
            {
                return TillSoil();
            }
            else if (item is WateringCanTool)
            {
                return RandomWateredTile(); 
            }
            else if (item is SeedTool)
            {
                return SeedPlanted();
            }
        }
        return false;
    }

    bool TillSoil()
    {
        if (!isTilled)
        {
            isTilled = true;
            if (baseRenderer != null && tilledSprite != null)
            {
                baseRenderer.sprite = tilledSprite; // Reveal the dry dirt
            }
            return true;
        }
        return false;
    }


    bool RandomWateredTile()
    {
        if (!isTilled) return false;

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
        if (!isTilled) return false;

        if (!isPlanted)
        {
            var item = HotbarManager.Instance.GetSelectedItem();
            if (item is SeedTool)
            {
                plantRenderer.sprite = plantSprite;
                plantRenderer.gameObject.SetActive(true);
                isPlanted = true;

                HotbarManager.Instance.RemoveItem(HotbarManager.Instance.GetSelectedItem(), 1);

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

