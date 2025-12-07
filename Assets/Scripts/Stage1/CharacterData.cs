using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour
{
    public List<Items> items;
    public GameObject canvas;

    [SerializeField]
    List<Image> images;
    void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            images[i].sprite = items[i].Sprite;
            images[i].SetNativeSize();
        }
    }
}
