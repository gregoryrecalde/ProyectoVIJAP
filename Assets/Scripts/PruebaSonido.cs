using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaSonido : MonoBehaviour
{
    public AudioSource audioSource;

    void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

}
