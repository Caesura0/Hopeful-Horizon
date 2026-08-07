using Caesura.Items;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteractor))]
[RequireComponent(typeof(PlayerToolUser))]
[RequireComponent(typeof(PlayerResources))]
public class Player : MonoBehaviour
{
    // The static events are forwarded to PlayerResources, keeping compatibility for other scripts
    // that might be subscribed to Player.OnTrashPickedUp, etc.
    
    public static event Action<int> OnTrashPickedUp
    {
        add { PlayerResources.OnTrashPickedUp += value; }
        remove { PlayerResources.OnTrashPickedUp -= value; }
    }
    public static event Action<int> OnSulferPickedUp
    {
        add { PlayerResources.OnSulferPickedUp += value; }
        remove { PlayerResources.OnSulferPickedUp -= value; }
    }
    public static event Action<int> OnCharcolPickedUp
    {
        add { PlayerResources.OnCharcolPickedUp += value; }
        remove { PlayerResources.OnCharcolPickedUp -= value; }
    }
    public static event Action<int> OnWoodPickedUp
    {
        add { PlayerResources.OnWoodPickedUp += value; }
        remove { PlayerResources.OnWoodPickedUp -= value; }
    }

    private PlayerMovement playerMovement;
    private PlayerInteractor playerInteractor;
    private PlayerToolUser playerToolUser;
    private PlayerResources playerResources;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteractor = GetComponent<PlayerInteractor>();
        playerToolUser = GetComponent<PlayerToolUser>();
        playerResources = GetComponent<PlayerResources>();
    }

    private void Start()
    {
        PlayerInput.Instance.OnHotkeySelectedAction += Instance_OnHotkeySelectedAction;
        PlayerInput.Instance.OnNextItemAction += Instance_OnNextItemAction;
        PlayerInput.Instance.OnPrevItemAction += Instance_OnPrevItemAction;
    }
    
    private void OnDestroy()
    {
        if (PlayerInput.Instance != null)
        {
            PlayerInput.Instance.OnHotkeySelectedAction -= Instance_OnHotkeySelectedAction;
            PlayerInput.Instance.OnNextItemAction -= Instance_OnNextItemAction;
            PlayerInput.Instance.OnPrevItemAction -= Instance_OnPrevItemAction;
        }
    }

    private void Instance_OnPrevItemAction(object sender, EventArgs e)
    {
        HotbarManager.Instance.CycleNextInventorySlot();
    }

    private void Instance_OnNextItemAction(object sender, EventArgs e)
    {
        HotbarManager.Instance.CyclePreviousInventorySlot();
    }

    private void Instance_OnHotkeySelectedAction(object sender, PlayerInput.HotkeySelectedEventArgs e)
    {
        HotbarManager.Instance.SelectUISlot(e.HotkeyValue);
    }

    // Forwarded Methods to not break IInteractable and animation events

    public void OnAnimationEnd()
    {
        playerToolUser.OnAnimationEnd();
    }

    public void PickUpGarbage()
    {
        playerResources.PickUpGarbage();
    }

    public int GetGarbagePickedUp()
    {
        return playerResources.GetGarbagePickedUp();
    }

    public void SulferPickup()
    {
        playerResources.SulferPickup();
    }

    public void CharcolPickup()
    {
        playerResources.CharcolPickup();
    }

    public void WoodPickup()
    {
        playerResources.WoodPickup();
    }

    public void AddItem(Item item, int quantity)
    {
        playerResources.AddItem(item, quantity);
    }

    public void RemoveItem(Item item, int quantity)
    {
        playerResources.RemoveItem(item, quantity);
    }
    
    public void CanMoveNow()
    {
        playerMovement.CanMoveNow();
    }
    
    public void CanMove(bool canMove)
    {
        playerMovement.CanMove(canMove);
    }
}

