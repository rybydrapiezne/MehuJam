using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FountainUIController : MonoBehaviour
{
    [SerializeField] private Slider valueSlider;
    //[SerializeField] private Button plusButton;
    //[SerializeField] private Button minusButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button throwCoins;
    [SerializeField] private TMP_Text valueText;

    [SerializeField] private Button upgradeAButton;
    [SerializeField] private Button upgradeBButton;
    [SerializeField] private Button upgradeCButton;

    public bool upgradeASelected = false;
    public bool upgradeBSelected = false;
    public bool upgradeCSelected = false;

    public int minValue = 0;
    public int maxValue = 10;
    public int startValue = 0;

    [SerializeField] private int coinsNeeded = 5;

    private void Awake()
    {
        valueSlider.minValue = minValue;
        valueSlider.maxValue = maxValue;
        valueSlider.wholeNumbers = true;
        valueSlider.value = Mathf.Clamp(startValue, minValue, maxValue);

        //plusButton.onClick.AddListener(OnPlusPressed);
        //minusButton.onClick.AddListener(OnMinusPressed);
        exitButton.onClick.AddListener(OnExitPressed);
        valueSlider.onValueChanged.AddListener(OnSliderChanged);
        throwCoins.onClick.AddListener(OnThrowCoinsPressed);

        upgradeAButton.onClick.AddListener(() => SelectUpgrade("A"));
        upgradeBButton.onClick.AddListener(() => SelectUpgrade("B"));
        upgradeCButton.onClick.AddListener(() => SelectUpgrade("C"));


        UpdateValueText((int)valueSlider.value);
    }
    private void SelectUpgrade(string upgrade)
    {
        upgradeASelected = upgrade == "A";
        upgradeBSelected = upgrade == "B";
        upgradeCSelected = upgrade == "C";
    }

    private void OnDestroy()
    {
        //plusButton.onClick.RemoveListener(OnPlusPressed);
        //minusButton.onClick.RemoveListener(OnMinusPressed);
        exitButton.onClick.RemoveListener(OnExitPressed);
        valueSlider.onValueChanged.RemoveListener(OnSliderChanged);
        throwCoins.onClick.RemoveListener(OnThrowCoinsPressed);

    }

    //private void OnPlusPressed()
    //{
    //    int newVal = Mathf.Clamp((int)valueSlider.value + 1, minValue, maxValue);
    //    if (newVal != (int)valueSlider.value)
    //    {
    //        valueSlider.value = newVal;
    //    }
    //}

    //private void OnMinusPressed()
    //{
    //    int newVal = Mathf.Clamp((int)valueSlider.value - 1, minValue, maxValue);
    //    if (newVal != (int)valueSlider.value)
    //    {
    //        valueSlider.value = newVal;
    //    }
    //}

    private void OnExitPressed()
    {
        gameObject.SetActive(false);
    }

    private void OnSliderChanged(float value)
    {
        UpdateValueText((int)value);
    }

    private void OnThrowCoinsPressed()
    {
        if (!upgradeASelected && !upgradeBSelected && !upgradeCSelected)
        {
            Debug.Log("Select an upgrade first!");
            return;
        }
        int coinsToSpend = (int)valueSlider.value;

        if (coinsToSpend < coinsNeeded)
        {
            Debug.Log("upgrade costs " + coinsNeeded + " coins");
            return;
        }

        if (WalletPlaceHolder.Instance.TrySpendCoins(coinsToSpend))
        {
            Debug.Log("coins in fountain");
        }
        else
        {
            Debug.Log("not enough coins");
        }
    }


    private void UpdateValueText(int intValue)
    {
        valueText.text = intValue.ToString();
    }
}