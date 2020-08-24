using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayAudio(string audio)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Sound Effects/" + audio);

        audioSource.PlayOneShot(clip);
    }
}
