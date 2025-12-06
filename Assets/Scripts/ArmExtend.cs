using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

public class ArmExtend : MonoBehaviour
{
    [SerializeField]
    float extendSpeed=10f;

    [SerializeField] Transform hand;
    [SerializeField] Transform arm;
    Vector3 _baseArmLocalPos;


    public GameObject pickedUpObject=null;

    void Start()
    {
        _baseArmLocalPos = arm.localPosition;
    }

    public void Extend()
    {
        arm.transform.Translate(Vector3.down * (Time.deltaTime*extendSpeed),Space.Self);
        hand.transform.Translate(Vector3.down * (Time.deltaTime*extendSpeed),Space.Self);
    }

    public void Retract()
    {
        if (arm.localPosition.y < _baseArmLocalPos.y)
        {
            arm.transform.Translate(Vector3.up * (Time.deltaTime * extendSpeed), Space.Self);
            hand.transform.Translate(Vector3.up * (Time.deltaTime * extendSpeed), Space.Self);
        }
        else if (pickedUpObject)
        {
            Destroy(pickedUpObject);
            pickedUpObject = null;
            GameEvents.HandReset();
        }

    }
}

