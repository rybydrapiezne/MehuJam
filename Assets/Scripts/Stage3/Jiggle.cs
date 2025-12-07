using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class Jiggle : MonoBehaviour
{
    [SerializeField] float jiggleRate = 35;
    [SerializeField] GameObject leftEdgePoint;
    [SerializeField] GameObject rightEdgePoint;
    [SerializeField] float maxDistanceToEdge;
    Vector3 _direction;
    Random _rnd=new Random();
    bool _canReverse = true;
    void Start()
    {
        StartCoroutine(JiggleRoutine());
        jiggleRate *= UpgradeSystem.pickpocketJiggleRate;
    }

    void Update()
    {
        
        transform.Translate(_direction * (jiggleRate * Time.deltaTime),Space.World);
        if ((Vector3.Distance(leftEdgePoint.transform.position, transform.position) <= maxDistanceToEdge ||
            Vector3.Distance(rightEdgePoint.transform.position, transform.position) <= maxDistanceToEdge) && _canReverse)
        {
            Debug.Log("PEEP");
            _direction=-_direction;
            _canReverse=false;
            StartCoroutine(ReverseAfterDelay());
        }
    }

    IEnumerator ReverseAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        _canReverse = true;
    }
    IEnumerator JiggleRoutine()
    {
        while (true)
        {
            if(_canReverse)
                _direction = WhatDirection();
            yield return new WaitForSeconds(2);
        }

    }
    Vector3 WhatDirection()
    {
        var randomizedDirection=_rnd.Next(0, 10);
        return randomizedDirection < 5 ? Vector3.left : Vector3.right;
    }
}
