using System.Collections.Generic;
using UnityEngine;

public class PlaceCharactersRandom : MonoBehaviour
{
    [SerializeField] List<Items> items;
    
    public GameObject[] characterPrefabs;

    public int charactersToPlace = 3;

    public Transform parentContainer;

    public float minX = -5f, maxX = 5f, minY = -3f, maxY = 3f;
    public float minDistance = 1f;

    void Start()
    {
        if (characterPrefabs == null || characterPrefabs.Length == 0)
        {
            Debug.LogError("Brak prefab�w postaci w inspectorze!");
            return;
        }

        if (charactersToPlace <= 0) charactersToPlace = 1;
        charactersToPlace = Mathf.Min(charactersToPlace, characterPrefabs.Length);

        List<int> indices = GetUniqueRandomIndices(characterPrefabs.Length, charactersToPlace);
        Vector2[] positions = GeneratePositions(charactersToPlace);

        for (int i = 0; i < charactersToPlace; i++)
        {
            GameObject prefab = characterPrefabs[indices[i]];
            Vector2 pos = positions[i];
            var characterItems = GetItemsForCharacter();
            GameObject instance = Instantiate(prefab, pos, Quaternion.identity, parentContainer);
            instance.GetComponent<CharacterData>().items = characterItems;
            instance.transform.rotation = Quaternion.identity;
            Vector3 s = instance.transform.localScale;
            instance.transform.localScale = new Vector3(Mathf.Abs(s.x), Mathf.Abs(s.y), Mathf.Abs(s.z));

            CursorHandler cursorHandler = instance.GetComponent<CursorHandler>();
            if (cursorHandler != null)
            {
                cursorHandler.fountainUI = FindAnyObjectByType<FountainUIController>()?.gameObject;
                cursorHandler.stage1Controller = FindAnyObjectByType<Stage1Controller>();
            }
        }
    }

    List<Items> GetItemsForCharacter()
    {
        List<Items> characterItems = new List<Items>();
        for (int i = 0; i < 3; i++)
        {
            characterItems.Add(items[Random.Range(0, items.Count)]);
        }
        return characterItems;
    }
    private List<int> GetUniqueRandomIndices(int poolSize, int take)
    {
        List<int> list = new List<int>(poolSize);
        for (int i = 0; i < poolSize; i++) { list.Add(i); }

        for (int i = 0; i < take; i++)
        {
            int j = Random.Range(i, poolSize);
            int tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
        return list.GetRange(0, take);
    }

    private Vector2[] GeneratePositions(int count)
    {
        Vector2[] positions = new Vector2[count];
        int placed = 0;
        int attemptsLimit = 500;

        while (placed < count)
        {
            int attempts = 0;
            Vector2 candidate;
            bool ok;
            do
            {
                candidate = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                ok = true;
                for (int i = 0; i < placed; i++)
                {
                    if (Vector2.Distance(candidate, positions[i]) < minDistance)
                    {
                        ok = false;
                        break;
                    }
                }
                attempts++;
            } while (!ok && attempts < attemptsLimit);

            positions[placed] = candidate;
            placed++;
        }

        return positions;
    }
}