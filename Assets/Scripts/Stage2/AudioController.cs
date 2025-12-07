using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField]
    AudioSource audioSource;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            var audioClip = audioClips[Random.Range(0, audioClips.Count)];
            audioSource.clip = audioClip;
            audioSource.Play();
            float cooldown = Random.Range(1f,3f);
            yield return new WaitForSeconds(cooldown);
        }
    }
}
