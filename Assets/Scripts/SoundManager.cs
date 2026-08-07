using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSoundsSO audioSoundsSO;
    [SerializeField] private AudioEventChannelSO audioChannel;

    private void OnEnable()
    {
        if (audioChannel != null)
        {
            audioChannel.OnEventRaised += PlaySoundEffect;
        }
    }

    private void OnDisable()
    {
        if (audioChannel != null)
        {
            audioChannel.OnEventRaised -= PlaySoundEffect;
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
    }

    private void PlayRandomSoundFromArray(AudioClip[] audioClipArray, float volume = 1)
    {
        if (audioClipArray == null || audioClipArray.Length == 0) return;
        int choice = Random.Range(0, audioClipArray.Length);
        AudioClip sound = audioClipArray[choice];
        PlaySound(sound, volume);
    }

    private void PlaySoundEffect(SoundEffect effect)
    {
        if (audioSoundsSO == null) return;
        
        switch (effect)
        {
            case SoundEffect.AxeSwing: PlayRandomSoundFromArray(audioSoundsSO.axeSwing); break;
            case SoundEffect.AxeChoppingTree: PlayRandomSoundFromArray(audioSoundsSO.axeChoppingTree); break;
            case SoundEffect.PickaxeSwing: PlayRandomSoundFromArray(audioSoundsSO.pickaxeSwing); break;
            case SoundEffect.PickaxeHit: PlayRandomSoundFromArray(audioSoundsSO.pickaxeHit); break;
            case SoundEffect.Watering: PlayRandomSoundFromArray(audioSoundsSO.watering); break;
            case SoundEffect.FillWateringCan: PlayRandomSoundFromArray(audioSoundsSO.fillWateringCan); break;
            case SoundEffect.SwitchItems: PlayRandomSoundFromArray(audioSoundsSO.switchItems); break;
            case SoundEffect.ClickStart: PlayRandomSoundFromArray(audioSoundsSO.clickStart); break;
            case SoundEffect.MilkCow: PlayRandomSoundFromArray(audioSoundsSO.milkCow); break;
            case SoundEffect.CowMoo: PlayRandomSoundFromArray(audioSoundsSO.cowMoo); break;
            case SoundEffect.Footsteps: PlayRandomSoundFromArray(audioSoundsSO.footsteps); break;
            case SoundEffect.ItemPickup: PlayRandomSoundFromArray(audioSoundsSO.itemPickup, 0.9f); break;
            case SoundEffect.ChestUnlock: PlayRandomSoundFromArray(audioSoundsSO.chestUnlock); break;
            case SoundEffect.ChestLocked: PlayRandomSoundFromArray(audioSoundsSO.chestLockedRattle); break;
            case SoundEffect.QuestStarted: PlayRandomSoundFromArray(audioSoundsSO.questStarted); break;
            case SoundEffect.QuestFinished: PlayRandomSoundFromArray(audioSoundsSO.questFinished); break;
            case SoundEffect.QuestPopup: PlayRandomSoundFromArray(audioSoundsSO.questPopup); break;
            case SoundEffect.OpenDialogue: PlayRandomSoundFromArray(audioSoundsSO.openDialogue); break;
            case SoundEffect.TypingDialogue: PlayRandomSoundFromArray(audioSoundsSO.dialogueTyping, 0.9f); break;
        }
    }
}
