using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    public class HotkeySelectedEventArgs : EventArgs
    {
        public int HotkeyValue { get; }

        public HotkeySelectedEventArgs(int hotkeyValue)
        {
            HotkeyValue = hotkeyValue;
        }
    }
    

    public static PlayerInput Instance { get; private set; }


    public event EventHandler OnInteractAction;
    public event EventHandler OnItemUseAction;
    public event EventHandler OnQuestUIAction;
    public event EventHandler OnRunCanceledAction;
    public event EventHandler OnRunAction;
    public event EventHandler OnMenuAction;
    public event EventHandler<HotkeySelectedEventArgs> OnHotkeySelectedAction;
    public event EventHandler OnNextItemAction;
    public event EventHandler OnPrevItemAction;

    //public event EventHandler OnPauseAction;
    public Vector2 Movement {  get; private set; }


    private PlayerControls controls;


    // Touch Input fields

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isTouching = false;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        controls = new PlayerControls();


        // Setup input callbacks
        controls.KeyboardGamepad.Movement.performed += ctx => Movement = ctx.ReadValue<Vector2>();
        controls.KeyboardGamepad.Movement.canceled += ctx => Movement = Vector2.zero;

        controls.KeyboardGamepad.Interact.performed += Interact_performed;
        controls.KeyboardGamepad.UseItem.performed += ItemUse_performed;

        MobileScreenControls.OnInteractButtonScreenPress += Interact_Mobile;
        MobileScreenControls.OnItemUseButtonScreenPress += ItemUse_Mobile;



        controls.KeyboardGamepad.Hotkeys.performed += OnHotkeySelected_Performed;
        controls.KeyboardGamepad.NextItem.performed += NextItem_performed;
        controls.KeyboardGamepad.PrevItem.performed += PrevItem_performed;

        controls.KeyboardGamepad.QuestUI.performed += QuestUI_performed; ;

        controls.KeyboardGamepad.Run.performed += Run_performed;
        controls.KeyboardGamepad.Run.canceled += Run_canceled;

        controls.KeyboardGamepad.Menu.performed += Menu_performed;


        controls.Touch.PrimaryContact.started += ctx => StartTouch(ctx);
        controls.Touch.PrimaryContact.canceled += ctx => EndTouch(ctx);
        controls.Touch.PrimaryPosition.performed += ctx => UpdateTouch(ctx);
        controls.Touch.TouchCanceled.performed += EndTouch;
    }

    private void Menu_performed(InputAction.CallbackContext obj)
    {
        OnMenuAction?.Invoke(this, EventArgs.Empty);
    }

    private void PrevItem_performed(InputAction.CallbackContext obj)
    {
        OnPrevItemAction?.Invoke(this, EventArgs.Empty);
    }

    private void NextItem_performed(InputAction.CallbackContext obj)
    {
        OnNextItemAction?.Invoke(this, EventArgs.Empty);
    }

    private void Run_canceled(InputAction.CallbackContext obj)
    {
        OnRunCanceledAction?.Invoke(this, EventArgs.Empty);
    }

    private void Run_performed(InputAction.CallbackContext obj)
    {
        OnRunAction?.Invoke(this, EventArgs.Empty);
    }

    private void QuestUI_performed(InputAction.CallbackContext obj)
    {
        OnQuestUIAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        Debug.Log("normal");
    }

    private void ItemUse_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnItemUseAction?.Invoke(this, EventArgs.Empty);
    }


    private void Interact_Mobile()
    {
        Debug.Log("mobile");
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void ItemUse_Mobile()
    {
        OnItemUseAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }






    private void OnHotkeySelected_Performed(InputAction.CallbackContext context)
    {
        var selectedSlot = (int)context.ReadValue<float>(); // Directly read the value from the input
        OnHotkeySelectedAction?.Invoke(this, new HotkeySelectedEventArgs(selectedSlot));
    }



    //private void StartTouch(InputAction.CallbackContext context)
    //{
    //    Debug.Log("touch");
    //    isSwiping = true;
    //    startTouchPosition = controls.Touch.PrimaryPosition.ReadValue<Vector2>();
    //}

    //private void EndTouch(InputAction.CallbackContext context)
    //{
    //    isSwiping = false;
    //    Vector2 endTouchPosition = controls.Touch.PrimaryPosition.ReadValue<Vector2>();
    //    currentSwipe = endTouchPosition - startTouchPosition;
    //    Debug.Log("end");
    //    if (currentSwipe.magnitude > swipeThreshold)
    //    {
    //        HandleSwipe();
    //    }
    //    else
    //    {
    //        Movement = Vector2.zero;
    //    }
    //}

    //private void HandleSwipe()
    //{
    //    Debug.Log("swipe");
    //    Vector2 moveDirection = Vector2.zero;

    //    if (Mathf.Abs(currentSwipe.x) > Mathf.Abs(currentSwipe.y))
    //    {
    //        // Horizontal swipe
    //        if (currentSwipe.x > 0)
    //        {
    //            moveDirection = Vector2.right; // Swipe right
    //        }
    //        else
    //        {
    //            moveDirection = Vector2.left;  // Swipe left
    //        }
    //    }
    //    else
    //    {
    //        // Vertical swipe
    //        if (currentSwipe.y > 0)
    //        {
    //            moveDirection = Vector2.up; // Swipe up
    //        }
    //        else
    //        {
    //            moveDirection = Vector2.down; // Swipe down
    //        }
    //    }

    //    Movement = moveDirection; // Set the movement direction based on swipe
    //}
    private void StartTouch(InputAction.CallbackContext context)
    {
        isTouching = true;
        startTouchPosition = controls.Touch.PrimaryPosition.ReadValue<Vector2>();
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        isTouching = false;
        Movement = Vector2.zero;
    }

    private void UpdateTouch(InputAction.CallbackContext context)
    {
        // Ignore touch input if it's over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (isTouching)
        {



            currentTouchPosition = controls.Touch.PrimaryPosition.ReadValue<Vector2>();
            Vector2 swipeDirection = currentTouchPosition - startTouchPosition;

            if (swipeDirection.magnitude > 0)  // Check if there's any movement
            {
                Movement = swipeDirection.normalized;  // Normalize the direction to get a unit vector
            }
        }
    }
}