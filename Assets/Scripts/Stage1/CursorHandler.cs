using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private GameObject outline;
    [SerializeField] private Collider2D targetCollider;
    [SerializeField] private bool isFountain = false;
    [SerializeField] AudioSource audioSource;

    public GameObject fountainUI;
    public Stage1Controller stage1Controller;

    private bool isOutlineVisible = false;

    [HideInInspector] public int characterIndex;

    private void Awake()
    {
        if (fountainUI == null)
        {
            FountainUIController fountain = FindAnyObjectByType<FountainUIController>();
            if (fountain != null)
                fountainUI = fountain.gameObject;
        }

        if (stage1Controller == null)
            stage1Controller = FindAnyObjectByType<Stage1Controller>();
    }

    private void Update()
    {
        if (outline == null || targetCollider == null) return;

        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            outline.SetActive(false);
            return;
        }

        bool isFountainPanelVisible = false;

        if (fountainUI != null)
        {
            CanvasGroup cg = fountainUI.GetComponent<CanvasGroup>();
            if (cg != null && cg.alpha > 0f)
            {
                isFountainPanelVisible = true;
            }    
        }
        if (isFountainPanelVisible)
        {
            outline.SetActive(false);
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        bool isCursorOnTarget = targetCollider.OverlapPoint(mousePos);

        if (isCursorOnTarget != isOutlineVisible)
        {
            outline.SetActive(isCursorOnTarget);
            if(isCursorOnTarget)
                audioSource.Play();
            isOutlineVisible = isCursorOnTarget;
        }

        if (isCursorOnTarget && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (isFountain && fountainUI != null)
            {
                fountainUI.GetComponent<FountainUIController>().ShowPanel();
            }
            else if (!isFountain)
            {
                var characterItems = GetComponent<CharacterData>().items;
                foreach (Items item in characterItems)
                    GameManager.Items.Add(item);
                GameManager.selectedCharacterIndex = characterIndex;
                stage1Controller.End();
            }

        }
    }
}

