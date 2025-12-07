using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PointPopUp : MonoBehaviour
{
    // to bedzie trzeba dodac do metody po tym jak bedze ustalona wartosc do danego przedmiotu
    public static PointPopUp Instance;
    [SerializeField] private GameObject pointPopUpPrefab;
    private void Awake()
    {
        Instance = this;    
    }
    public void ShowPoints(string text, Vector3 position)
    {
        if (!pointPopUpPrefab) return;

        Vector3 spawnPos = position + new Vector3(0, 0.5f, 0);
        GameObject popup = Instantiate(pointPopUpPrefab, spawnPos, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshPro>().text = text;
    }
}
