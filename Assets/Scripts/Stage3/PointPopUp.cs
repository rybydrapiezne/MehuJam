using UnityEngine;
using TMPro;

public class PointPopUp : MonoBehaviour
{
    // to bedzie trzeba dodac do metody po tym jak bedze ustalona wartosc do danego przedmiotu
    [SerializeField] private GameObject pointPopUpPrefab;

    public void GrabItem(int coinWorth)
    {
        ShowPoints(coinWorth.ToString());
    }

    private void ShowPoints(string text)
    {
        if(pointPopUpPrefab)
        {
            GameObject prefab = Instantiate(pointPopUpPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMeshPro>().text = text;
        }
    }
}
