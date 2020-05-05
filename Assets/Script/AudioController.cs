using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public void Play(AudioSource audioData)
    {
        if (audioData != null)
        {
            audioData.Play(0);
        }
    }
}
