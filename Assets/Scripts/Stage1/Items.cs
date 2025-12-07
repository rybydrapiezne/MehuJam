using System;
using UnityEngine;

[Serializable]
public class Items
{
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject prefab;

    public Items(Sprite sprite, GameObject prefab)
    {
        this.sprite = sprite;
        this.prefab = prefab;
    }
    public Sprite Sprite => sprite;
    public GameObject Prefab => prefab;
}
