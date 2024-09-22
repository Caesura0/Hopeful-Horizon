using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour
{


    public static event Action<int> OnTrashPickedUp;
    public static event Action<int> OnSulferPickedUp;
    public static event Action<int> OnCharcolPickedUp;
    public static event Action<int> OnWoodPickedUp;

    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] LayerMask interactableLayer;


   

    public int TrashPickedUp {  get; private set; }
    public int SulferPickedUp {  get; private set; }
    public int CharcolPickedUp {  get; private set; }
    public int WoodPickedUp {  get; private set; }

    
    public float lastMoveX;
    public float lastMoveY;

    //bool canMove = true;
    bool isMoving;
    bool isRunning = true;

    float directionalThreshold = 0.1f;


    Animator animator;

    public Vector2 movement;


    bool canMove = true;


    Rigidbody2D rb;
    float radius = .3f;

    ItemType itemType;



    private void Awake()
    {

        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        PlayerInput.Instance.OnInteractAction += PlayerInput_OnInteractAction;
        PlayerInput.Instance.OnItemUseAction += PlayerInput_OnItemUseAction;
        PlayerInput.Instance.OnHotkeySelectedAction += Instance_OnHotkeySelectedAction;
        PlayerInput.Instance.OnNextItemAction += Instance_OnNextItemAction;
        PlayerInput.Instance.OnPrevItemAction += Instance_OnPrevItemAction;
        PlayerInput.Instance.OnRunCanceledAction += Instance_OnRunCanceledAction;
        PlayerInput.Instance.OnRunAction += Instance_OnRunAction;
    }

    private void Instance_OnPrevItemAction(object sender, EventArgs e)
    {
        InventoryManager.Instance.CycleNextInventorySlot();
    }

    private void Instance_OnNextItemAction(object sender, EventArgs e)
    {
        InventoryManager.Instance.CyclePreviousInventorySlot();
    }

    private void Instance_OnRunAction(object sender, EventArgs e)
    {
        isRunning = false; ;
    }

    private void Instance_OnRunCanceledAction(object sender, EventArgs e)
    {
        isRunning = true;
    }

    private void Instance_OnHotkeySelectedAction(object sender, PlayerInput.HotkeySelectedEventArgs e)
    {
        InventoryManager.Instance.SelectUISlot(e.HotkeyValue);
    }

    private void PlayerInput_OnItemUseAction(object sender, EventArgs e)
    {
        CheckForItemUse();
    }

    private void PlayerInput_OnInteractAction(object sender, EventArgs e)
    {
        CheckForInteract();
    }



    private void Update()
    {
        if (!SimpleDialogueManager.Instance.InDialogue && canMove)
        {
            Movement();
  
        }
        else
        {
            movement = Vector2.zero;
            rb.velocity = Vector2.zero;
            isMoving = false;
            animator.SetBool("IsMoving", isMoving);
            animator.SetFloat("MovementX", Mathf.Abs(0));
            animator.SetFloat("MovementY", Mathf.Abs(0));
        }

    }

    public void CheckForItemUse()
    {
        if (canMove && !SimpleDialogueManager.Instance.InDialogue)
        {
            Debug.Log("Check for items" + canMove);
            Item? item = InventoryManager.Instance.GetSelectedItem();
            if (item != null)
            {
                itemType = item.itemType;
                UseItem(itemType);
                
            }
        }
    }

    void CheckForItemInteractable()
    {
        Vector2 facingDirection = new Vector2(lastMoveX, lastMoveY);
        float distance = .6f;
        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)transform.position + facingDirection *.5f * 1.8f, radius, facingDirection, distance, interactableLayer);
        // Sort the hits by distance from the center of the circle
        System.Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));
 
        foreach (var hit in hits)
        {
            if (hit.transform != null && hit.transform.TryGetComponent<IItemInteractable>(out IItemInteractable itemInteractable))
            {
                if (itemInteractable.ItemInteract(this))
                {
                    switch (itemType)
                    {
                        case ItemType.Bottle:
                            //SoundManager.Instance.PlayFillWateringCanSound();
                            break;
                        case ItemType.Hoe:
                            SoundManager.Instance.PlayPickaxeHitSound();
                            break;
                        case ItemType.PickAxe:
                            SoundManager.Instance.PlayPickaxeHitSound();
                            break;
                        case ItemType.Axe:
                            
                            break;
                        case ItemType.WateringCan:
                            
                            break;
                        case ItemType.Seed:
                            
                            break;
                        default:
                            
                            break;
                    }
                    break;
                }
            }
        }

    }
    private void UseItem(ItemType itemType)
    {
        canMove = false;
        switch (itemType)
        {
            case ItemType.Bottle:
                BottleLogic();
                break;
            case ItemType.Hoe:
                HoeLogic();
                SoundManager.Instance.PlayAxeSwingSound();
                break;
            case ItemType.PickAxe:
                PickAxeLogic();
                Debug.Log("Pickaxe Use item" + canMove);
                SoundManager.Instance.PlayAxeSwingSound();
                break;
            case ItemType.Axe:
                SoundManager.Instance.PlayAxeSwingSound();
                AxeLogic();
                break;
            case ItemType.WateringCan:
                WateringCanLogic();
                break;
            case ItemType.Seed:
                SeedLogic();
                break;
            default:
                Debug.Log(canMove);
                canMove = true;
                break;
        }
    }

    private void SeedLogic()
    {
        SoundManager.Instance.PlayItemPickupSound();
        OnAnimationEnd();
        CanMoveNow();

       

        //seed planting animation
        //instantiate 
    }

    private void WateringCanLogic()
    {
        SoundManager.Instance.PlayWateringSound();
        AnimationPlay("Watering");

        //watering sound
        //instantiate plant
        //change tile color?
    }

    private void AxeLogic()
    {
        AnimationPlay("Axe");

        //tree life bar?
        //tree falling animation
    }

    private void PickAxeLogic()
    {
        Debug.Log("pickaxe logic before animation" + canMove);
        AnimationPlay("Hoe");


    }

    private void HoeLogic()
    {
        AnimationPlay("Hoe");

    }

    private void BottleLogic()
    {
        CheckForItemInteractable();
        canMove = true;
        //if next to cow
        //and bottle is empty
        //play bottle sound
        //remove one bottle
        //add new item full bottle
    }

    public void CanMove(bool canMove)
    {
        this.canMove = canMove; 
    }

    private void Movement()
    {
        movement = PlayerInput.Instance.Movement.normalized;

        if (movement != Vector2.zero)
        {
            isMoving = true;
            lastMoveX = movement.x;
            if (lastMoveX != 0 && movement.x > movement.y)
            {
                lastMoveY = 0f;
            }
            else
            {
                lastMoveY = movement.y;
            }

        }
        else
        {
            isMoving = false;
        }
        if (isRunning && isMoving && canMove)
        {
            animator.speed = 1.5f;
        }
        else
        {
            animator.speed = 1f;
        }
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MovementX", movement.x);
        animator.SetFloat("MovementY", movement.y);

        if (isMoving)
        {
            animator.SetFloat("LastMovementX", lastMoveX);
            animator.SetFloat("LastMovementY", lastMoveY);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {


        float speed;
        speed = isRunning ? runSpeed : moveSpeed;

        rb.velocity = movement * speed * Time.deltaTime;

    }


    public void CheckForInteract()
    {
        if (canMove && !SimpleDialogueManager.Instance.InDialogue)
        {
            Vector2 facingDirection = new Vector2(lastMoveX, lastMoveY);
            float distance = .6f;
            RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)transform.position + facingDirection * .4f, radius, facingDirection, distance, interactableLayer);
            // Sort the hits by distance from the center of the circle
            System.Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform != null && hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interact(this);
                    return;
                }
            }
  

        }
    }

    public void PickUpGarbage()
    {
        TrashPickedUp++;
        OnTrashPickedUp?.Invoke(TrashPickedUp);
    }



    public int GetGarbagePickedUp()
    {
        return TrashPickedUp;
    }

    public void SulferPickup()
    {
        SulferPickedUp++;
        OnSulferPickedUp?.Invoke(SulferPickedUp);
    }
    public void CharcolPickup()
    {
        CharcolPickedUp += 5;
        Debug.Log("charcol called");
        OnCharcolPickedUp?.Invoke(CharcolPickedUp);
    }
    public void WoodPickup()
    {
        WoodPickedUp++;
        OnWoodPickedUp?.Invoke(WoodPickedUp);
    }


    public void AddItem(Item item, int quantity)
    {
        SoundManager.Instance.PlayItemPickupSound();
        InventoryManager.Instance.AddItem(item, quantity);

    }

    public void RemoveItem(Item item, int quantity)
    {
        InventoryManager.Instance.RemoveItem(item, quantity);
    }


    private void AnimationPlay(string animationName)
    {
        // Play the animation
        animator.Play(animationName);


    }

    public void OnAnimationEnd()
    {
        Debug.Log("OnAnimationEnd" + canMove);
        CheckForItemInteractable();
        Debug.Log("OnAnimationEnd after check for item interactable" + canMove);

    }

    public void CanMoveNow()
    {
        animator.Play("Idle");
        canMove = true;
    }



}

