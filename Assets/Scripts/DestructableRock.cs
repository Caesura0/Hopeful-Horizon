using Caesura.Items;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.DemiLib;


public class DestructableRock : MonoBehaviour, IItemInteractable
{
    [SerializeField] Item neededItem;
    int hitCounter;

    [SerializeField] float strength = .1f;
    [SerializeField] float shakeTime = .8f;
    [SerializeField] int vibrato = 6;
    [SerializeField] GameObject visual;



    public bool ItemInteract(Player player)
    {
        if (HotbarManager.Instance.GetSelectedItem() != null && HotbarManager.Instance.GetSelectedItem() == neededItem)
        {
            hitCounter++;
            visual.transform.DOShakePosition(shakeTime, strength, vibrato, 0);
            if (hitCounter > 2)
            {
                Destroy(gameObject);
                //playsound
                //playparticleeffect
                player.SulferPickup();
            }
            return true;
            //play hit sound
        }
        return false;

    }


}

