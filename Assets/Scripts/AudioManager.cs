using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType { GetMaterial, Fail, Bonus, Finish, HouseStageCompleted, HouseCompleted }

[System.Serializable]
public class SoundByType
{
    public SoundType type;
    public AudioClip sound;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public List<SoundByType> allSounds = new List<SoundByType>();

    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(SoundType type, float volumeScale = 1f)
    {
        if (instance == null)
            return;
        
        var soundByType = instance.allSounds.Find(x => x.type == type);

        if (soundByType != null)
            instance.audioSource.PlayOneShot(soundByType.sound, volumeScale);
    }
}
