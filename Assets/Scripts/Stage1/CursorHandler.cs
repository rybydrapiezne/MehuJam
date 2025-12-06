using UnityEngine;
using UnityEngine.InputSystem;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private GameObject outline;
    [SerializeField] private Collider2D targetCollider;
    [SerializeField] private bool isFountain = false;

    public GameObject fountainUI;
    public Stage1Controller stage1Controller;

    private bool isOutlineVisible = false;

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
                stage1Controller.End();
            }

        }
    }
}

