using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stage3Controller : MonoBehaviour
{
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference retractAction;
    [SerializeField] GameObject arm;
    bool _isExtending = false;
    bool _isRetracting = false;
    void OnEnable()
    {
        attackAction.action.started += OnGrabStart;
        attackAction.action.canceled += OnGrabEnd;
        retractAction.action.started += OnRetractStart;
        retractAction.action.canceled += OnRetractEnd;
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
        if (_isExtending)
            arm.GetComponent<ArmExtend>().Extend();
        if(_isRetracting)
            arm.GetComponent<ArmExtend>().Retract();
    }
}
