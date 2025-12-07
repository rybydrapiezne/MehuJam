using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Stage3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Stage3Controller : MonoBehaviour
{
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference retractAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] GameObject arm;
    [SerializeField] HandColliderHandler hand;
    [SerializeField] GameObject handPickupPoint;
    [SerializeField] Animator handAnimator;
    [SerializeField] Slider slider;
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] float transitionTime = 0.3f;
    [SerializeField] int maxErrors=3;
    bool _failed = false;
    bool _isExtending = false;
    bool _isRetracting = false;
    bool _retractTheHand = false;
    int errors=0;
    ArmExtend armExtend;

    void Start()
    {
        armExtend = arm.GetComponent<ArmExtend>();
        slider.maxValue = maxErrors;
        slider.value = 0;
    }
    
    void OnEnable()
    {
        if (attackAction == null || retractAction == null || interactAction == null)
        {
            attackAction = GameManager.Instance.attackAction;
            retractAction = GameManager.Instance.retractAction;
            interactAction = GameManager.Instance.interactAction;
        }

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
        errors++;
        if(errors >= maxErrors)
            _failed = true;
        _retractTheHand = true;
        StartCoroutine(ErrorScreen(transitionTime));
    }
    void OnInteractStart(InputAction.CallbackContext obj)
    {
        GameObject pickUpItem = hand.CanPickUp();
        if (pickUpItem != null && !_failed)
        {
            pickUpItem.GetComponent<Rigidbody2D>().simulated = false;
            pickUpItem.transform.position=handPickupPoint.transform.position;
            pickUpItem.transform.parent = hand.transform;
            armExtend.pickedUpObject=pickUpItem;
            audioSources[Random.Range(0,audioSources.Count)].Play();
            handAnimator.SetBool("PickUp",true);
            _retractTheHand = true;

            TryAddCoins(pickUpItem);
        }
            
    }
    void TryAddCoins(GameObject pickUpItem)
    {
        if (pickUpItem.TryGetComponent<ItemType>(out var itemType) && itemType.coinValue > 0)
        {
            Wallet.AddCoins(itemType.coinValue);
            PointPopUp.Instance.ShowPoints("+" + itemType.coinValue, pickUpItem.transform.position);
        }
    }
    IEnumerator ErrorScreen(float time)
    {
        time /= 2f;
        float timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            GameManager.ErrorCanvas.alpha = Mathf.Lerp(0f, 1f, timer / time);
            yield return null;
        }
        timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            GameManager.ErrorCanvas.alpha = Mathf.Lerp(1f, 0f, timer / time);

            yield return null;
        }
        timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Lerp(slider.value, errors, timer);
            yield return null;
        }
        GameManager.ErrorCanvas.alpha = 0f;
    }
    void OnResetHand(ItemCategory? itemCategory)
    {
        ItemCategory? tempItemType=itemCategory;
        if(tempItemType!=null)
            if (itemCategory == ItemCategory.Bad)
            {
                StartCoroutine(ErrorScreen(transitionTime));
                errors++;
                if(errors >= maxErrors)
                    _failed = true;
            }
        if (!_failed)
        {
            _retractTheHand = false;
            _isRetracting = false;
            _isExtending = false;
            handAnimator.SetBool("PickUp", false);
            arm.GetComponent<ArmRotate>().StartRotation();
        }
    }
    void OnHandHit()
    {
        _retractTheHand = true;
        errors++;
        if(errors >= maxErrors)
            _failed = true;
        StartCoroutine(ErrorScreen(transitionTime));

    }
    void OnRetractStart(InputAction.CallbackContext context)
    {
        if(!_isExtending && !_failed)
            _isRetracting = true;
    }

    void OnRetractEnd(InputAction.CallbackContext context)
    {
        _isRetracting = false;
    }
    void OnGrabStart(InputAction.CallbackContext context)
    {
        if (!_isRetracting && !_failed)
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
