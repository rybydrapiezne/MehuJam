using UnityEngine;
using UnityEngine.InputSystem;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private GameObject outline;
    [SerializeField] private Collider2D targetCollider;

    [SerializeField] private bool isFountain = false;
    [SerializeField] private GameObject fountainUI;

    private bool isOutlineVisible = false;

    private void Update()
    {
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
                fountainUI.SetActive(true);
            }
            else
            {
                Debug.Log("clicked on " + targetCollider.name);
            }

        }
    }
}

