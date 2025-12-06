using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HandColliderHandler : MonoBehaviour
{
    [SerializeField] LayerMask edgeLayer;
    [SerializeField] LayerMask itemLayer;
    bool _canPickUp;
    GameObject pickableObject=null;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((itemLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            pickableObject = other.gameObject;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _canPickUp = false;
        pickableObject = null;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if((edgeLayer.value & (1 << other.gameObject.layer))!=0)
            GameEvents.HandColliderHit();
    }
    
    [CanBeNull]
    public GameObject CanPickUp()
    {
            return pickableObject;
    }
}
