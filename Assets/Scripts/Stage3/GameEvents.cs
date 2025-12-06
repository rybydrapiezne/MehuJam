using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnItemCollisionWithBorder;
    public static event Action OnHandColliderHit;
    public static event Action OnHandReset;

    public static void HandColliderHit()
    {
        OnHandColliderHit?.Invoke();
    }

    public static void HandReset()
    {
        OnHandReset?.Invoke();
    }

    public static void ItemCollisionWithBorder()
    {
        OnItemCollisionWithBorder?.Invoke();
    }

}
