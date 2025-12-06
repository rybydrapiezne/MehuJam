using System;
using System.Collections;
using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    [SerializeField] LayerMask itemLayer;
    [SerializeField] GameObject middle;
    private bool canTriggerCollisions=false;
    void Start()
    {
        StartCoroutine(WaitForSpawn());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(canTriggerCollisions)
        {
            if ((itemLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                GameEvents.ItemCollisionWithBorder();
            }
        
        }
        else
        {
            StartCoroutine(PushItemTowardsCenter(other.gameObject));
        }
    }
    IEnumerator  PushItemTowardsCenter(GameObject item)
    {

        Vector2 directionToCenter = (middle.transform.position - item.transform.position).normalized;

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb)
        {
            float pushForce = 3f;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(directionToCenter * pushForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.15f);
        rb.linearVelocity = Vector2.zero;
    }
    
    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(1f);
        canTriggerCollisions = true;
    }
}
