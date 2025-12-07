using System;
using Stage3;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnItemCollisionWithBorder;
    public static event Action OnHandColliderHit;
    public static event Action<ItemCategory?> OnHandReset;

    public static void HandColliderHit()
    {
        OnHandColliderHit?.Invoke();
    }

    public static void HandReset(ItemCategory? itemType)
    {
        OnHandReset?.Invoke(itemType);
    }

    public static void ItemCollisionWithBorder()
    {
        OnItemCollisionWithBorder?.Invoke();
    }

}
