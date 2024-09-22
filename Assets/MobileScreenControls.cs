using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileScreenControls : MonoBehaviour
{

    public static Action OnInteractButtonScreenPress;
    public static Action OnItemUseButtonScreenPress;

    public static MobileScreenControls Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnInteractPress()
    {
        Debug.Log("interactpress");
        OnInteractButtonScreenPress?.Invoke();
    }

    public void OnItemUsePress()
    {
        OnItemUseButtonScreenPress?.Invoke();
    }
}
