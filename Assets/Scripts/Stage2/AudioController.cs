using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField]
    AudioClip headHurtClip;

    [SerializeField] AudioClip fellOverClip;
    [SerializeField] AudioClip hitLegClip;
    [SerializeField]
    AudioSource audioSource;

    public void PlaySound()
    {
       
            var audioClip = audioClips[Random.Range(0, audioClips.Count)];
            audioSource.clip = audioClip;
            audioSource.Play();
    }

    public void PlayHeadHurtClip()
    {
        audioSource.clip = headHurtClip;
        audioSource.Play();
    }

    public void PlayFellOver()
    {
        audioSource.clip = fellOverClip;
        audioSource.Play();
    }

    public void PlayHitLegClip()
    {
        audioSource.clip = hitLegClip;
        audioSource.Play();
    }
}
