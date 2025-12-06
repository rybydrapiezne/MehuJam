using System.Linq;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    //[Header("Settings")]
    //public string spriteSheetName; // File name inside Resources folder

    //private SpriteRenderer _spriteRenderer;
    //private Sprite[] _sprites;
    //public int spriteIndex = 0;

    //void Awake()
    //{
    //    _spriteRenderer = GetComponent<SpriteRenderer>();
        

    //    _sprites = Resources.LoadAll<Sprite>(spriteSheetName)
    //                       .OrderBy(s => s.name)
    //                       .ToArray();
    //}

    //private void Update()
    //{
    //    _spriteRenderer.sprite = _sprites[spriteIndex % _sprites.Length];
    //}

    // Tilt with range of <-1;1>, 0 being straight up
    public void SetTilt(float tilt)
    {
        transform.eulerAngles = new Vector3(0, 0, tilt*90);
    }
}
