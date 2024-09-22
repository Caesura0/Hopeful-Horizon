using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPickupTrash : Quest
{

    int trashPickedUp;
    int trashAssigned = 6;


    private void Start()
    {
        Player.OnTrashPickedUp += Player_OnTrashPickedUp;
    }

    private void Player_OnTrashPickedUp(int obj)
    {
        trashPickedUp++;
        SoundManager.Instance.PlayItemPickupSound();

        if(trashPickedUp == trashAssigned && questStatus == QuestStatus.Started)
        {
            FinishQuest();
        }
    }

    public override void StartQuest()
    {
        if (trashPickedUp == trashAssigned)
        {
            FinishQuest();
        }
        base.StartQuest();
    }




}
