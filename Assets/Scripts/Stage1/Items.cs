using System;
using UnityEngine;

[Serializable]
public class Items
{
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject prefab;
    
    public Sprite Sprite => sprite;
    public GameObject Prefab => prefab;
}
