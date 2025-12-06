using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stage3Controller : MonoBehaviour
{
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference retractAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] GameObject arm;
    [SerializeField] HandColliderHandler hand;
    
    bool _isExtending = false;
    bool _isRetracting = false;
    bool _retractTheHand = false;
    ArmExtend armExtend;

    void Start()
    {
        armExtend = arm.GetComponent<ArmExtend>();
    }
    
    void OnEnable()
    {
        attackAction.action.started += OnGrabStart;
        attackAction.action.canceled += OnGrabEnd;
        retractAction.action.started += OnRetractStart;
        retractAction.action.canceled += OnRetractEnd;
        interactAction.action.started += OnInteractStart;
        GameEvents.OnHandColliderHit+=OnHandHit;
        GameEvents.OnHandReset+=OnResetHand;
        GameEvents.OnItemCollisionWithBorder += OnItemCollisionWithBorder;
    }

    void OnDestroy()
    {
        attackAction.action.started -= OnGrabStart;
        attackAction.action.canceled -= OnGrabEnd;
        retractAction.action.started -= OnRetractStart;
        retractAction.action.canceled -= OnRetractEnd;
        interactAction.action.started -= OnInteractStart;
        GameEvents.OnHandColliderHit-=OnHandHit;
        GameEvents.OnHandReset-=OnResetHand;
        GameEvents.OnItemCollisionWithBorder -= OnItemCollisionWithBorder;


    }

    void OnItemCollisionWithBorder()
    {
        _retractTheHand = true;
    }
    void OnInteractStart(InputAction.CallbackContext obj)
    {
        GameObject pickUpItem = hand.CanPickUp();
        if (pickUpItem != null)
        {
            pickUpItem.GetComponent<Rigidbody2D>().simulated = false;
            pickUpItem.transform.position=hand.transform.position;
            pickUpItem.transform.parent = hand.transform;
            armExtend.pickedUpObject=pickUpItem;
            _retractTheHand = true;
        }
            
    }

    void OnResetHand()
    {
        _retractTheHand = false;
        _isRetracting = false;
        _isExtending = false;
        arm.GetComponent<ArmRotate>().StartRotation();
    }
    void OnHandHit()
    {
        _retractTheHand = true;
    }
    void OnRetractStart(InputAction.CallbackContext context)
    {
        if(!_isExtending)
            _isRetracting = true;
    }

    void OnRetractEnd(InputAction.CallbackContext context)
    {
        _isRetracting = false;
    }
    void OnGrabStart(InputAction.CallbackContext context)
    {
        if (!_isRetracting)
        { 
            _isExtending = true;
            arm.GetComponent<ArmRotate>().StopRotation();
        }
}

    void OnGrabEnd(InputAction.CallbackContext context)
    {
        _isExtending=false;
    }

    void Update()
    {
        if (!_retractTheHand)
        {
            if (_isExtending)
                armExtend.Extend();
            if (_isRetracting)
                armExtend.Retract();
        }
        else
        {
            armExtend.Retract();
        }
    }
}
