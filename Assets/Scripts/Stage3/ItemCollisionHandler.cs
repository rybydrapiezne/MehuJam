using System;
using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    [SerializeField] LayerMask itemLayer;
    void OnCollisionEnter2D(Collision2D other)
    {
        if ((itemLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            GameEvents.ItemCollisionWithBorder();
        }
    }
}
