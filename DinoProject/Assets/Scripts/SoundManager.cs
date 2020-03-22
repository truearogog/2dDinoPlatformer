using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip dinoJumpSound;
    public static AudioClip[] hitSounds = new AudioClip[3];
    public static AudioClip[] explosionSounds = new AudioClip[4];
    static AudioSource ac;

    void Start()
    {
        dinoJumpSound = Resources.Load<AudioClip>("Sounds/Jump1");
        for (int i = 0; i < hitSounds.Length; ++i)
        {
            hitSounds[i] = Resources.Load<AudioClip>("Sounds/Hit" + (i + 1).ToString());
        }
        for (int i = 0; i < explosionSounds.Length; ++i)
        {
            explosionSounds[i] = Resources.Load<AudioClip>("Sounds/Explosion" + (i + 1).ToString());
        }

        ac = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public static void PlaySound(string sound)
    {
        switch (sound)
        {
            case "jump":
                ac.PlayOneShot(dinoJumpSound);
                break;
            case "hit":
                ac.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
                break;
            case "explosion":
                ac.PlayOneShot(explosionSounds[Random.Range(0, explosionSounds.Length)]);
                break;
        }
    }

    public static void PlaySound(AudioClip soundClip)
    {
        ac.PlayOneShot(soundClip);
    }
}
