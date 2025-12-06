using UnityEngine;

public class OutlineFollower : MonoBehaviour
{
    private SpriteRenderer outlineSR;
    private SpriteRenderer mainSR;

    private void Awake()
    {
        outlineSR = GetComponent<SpriteRenderer>();
        mainSR = GetComponentInParent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (mainSR == null || outlineSR == null) return;
        outlineSR.sprite = mainSR.sprite;

        outlineSR.flipX = mainSR.flipX;
        outlineSR.flipY = mainSR.flipY;
    }
}
