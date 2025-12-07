using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] List<PolygonCollider2D> spawnArea;
    //[SerializeField] List<GameObject> goodItems;
    //[SerializeField] List<GameObject> badItems;
    [SerializeField] int itemsToSpawn;
    List<Collider2D> colliders = new List<Collider2D>();
    private List<Items> characterItems = new List<Items>();

    void Start()
    {
        characterItems = new List<Items>(GameManager.Items);
        for (int j = 0; j < itemsToSpawn; j++)
        {

                int x = Random.Range(0, characterItems.Count);
                Vector2 spawnPoint = GetSpawnPoint(characterItems[x].Prefab.GetComponent<ItemType>().type);
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                var item=Instantiate(characterItems[x].Prefab, spawnPoint, randomRotation);
                colliders.Add(item.gameObject.GetComponent<Collider2D>());
        }
    }

    Vector2 GetSpawnPoint(ItemTypeEnum itemType)
    {
        PolygonCollider2D tempSpawnArea = null;
        switch (itemType)
        { 
            case ItemTypeEnum.Small:
                tempSpawnArea = spawnArea[0];
                break;
        case ItemTypeEnum.Medium:
            tempSpawnArea = spawnArea[1];
            break;
        case ItemTypeEnum.Big:
            tempSpawnArea = spawnArea[2];
            break;

        case ItemTypeEnum.Default:
            int k = Random.Range(0, 3);
            tempSpawnArea = spawnArea[k];
            break;
        }

        if (tempSpawnArea != null)
        {
            Bounds bounds = tempSpawnArea.bounds;
            Vector2 point;
            int checks=0;
            bool overlapsWithItem = true;
            do
            {
                overlapsWithItem = false;
                point = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
                checks++;
                foreach (var collidertemp in colliders)
                {
                    if(collidertemp.bounds.Contains(point))
                        overlapsWithItem = true;
                }
                if (checks > 100) break;
            }while(!tempSpawnArea.OverlapPoint(point) && !overlapsWithItem);

            return point;
        }
        throw new Exception("Spawn area is invalid");
    }
}
