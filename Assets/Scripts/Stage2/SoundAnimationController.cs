using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundAnimationController : MonoBehaviour
{
    [SerializeField] AnimationClip clip;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    int index = 0;
    public bool duringJump = false;
    void Awake()
    {
        SetupFootstepEvents();
    }

    void SetupFootstepEvents()
    {
        if (clip == null || audioSource == null) return;
        var events = AnimationUtility.GetAnimationEvents(clip);
    
        bool modified = false;
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i].functionName != "Walk")
            {
                events[i].functionName = "Walk";
                events[i].messageOptions = SendMessageOptions.DontRequireReceiver;
                modified = true;
            }
        }

        if (modified || events.Length > 0)
        {
            AnimationUtility.SetAnimationEvents(clip, events);
            Debug.Log($"Fixed {events.Length} events on clip: {clip.name}");
        
            if (anim != null)
            {
                anim.Rebind();
                anim.Update(0f);
            }
        }
    }
    public void Walk()
    {
        if (audioSource != null && !duringJump)
        {
            audioSource.clip = audioClips[index];
            index++;
            if(index >= audioClips.Count)
                index = 0;
            audioSource.Play();
        }
    }
}