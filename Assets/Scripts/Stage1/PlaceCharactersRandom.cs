using System.Collections.Generic;
using UnityEngine;

public class PlaceCharactersRandom : MonoBehaviour
{
    public Collider2D[] characters;
    public Sprite[] availableSprites;
    public float minX, maxX, minY, maxY;
    public float minDistance = 1f;

    private void Start()
    {
        AssignRandomSprites();
        PositionCharacters();
    }
    private void AssignRandomSprites()
    {
        List<Sprite> spritesPool = new List<Sprite>(availableSprites);
        ShuffleList(spritesPool);

        for (int i = 0; i < characters.Length; i++)
        {
            SpriteRenderer sr = characters[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = spritesPool[i];
            }
            
            Transform outlineChild = characters[i].transform.Find("Outline");
            if (outlineChild != null)
            {
                SpriteRenderer outlineSR = outlineChild.GetComponent<SpriteRenderer>();
                if (outlineSR != null)
                {
                    outlineSR.sprite = spritesPool[i];
                }
            }
        }
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }




    private void PositionCharacters()
    {
        Vector2[] positions = new Vector2[characters.Length];

        for (int i = 0; i < characters.Length; i++)
        {
            Vector2 newPos;
            int attempts = 0;

            do
            {
                newPos = new Vector2(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY)
                );
                attempts++;
            }
            while (!IsPositionValid(newPos, positions, i) && attempts < 100);

            positions[i] = newPos;
            characters[i].transform.position = newPos;
        }
    }

    private bool IsPositionValid(Vector2 pos, Vector2[] existingPositions, int currentIndex)
    {
        for (int i = 0; i < currentIndex; i++)
        {
            if (Vector2.Distance(pos, existingPositions[i]) < minDistance)
                return false;
        }
        return true;
    }
}
