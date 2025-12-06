using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FountainUIController : MonoBehaviour
{
    [SerializeField] private Slider valueSlider;
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
        HidePanel();

        valueSlider.minValue = minValue;
        valueSlider.maxValue = maxValue;
        valueSlider.wholeNumbers = true;
        valueSlider.value = Mathf.Clamp(startValue, minValue, maxValue);

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
        exitButton.onClick.RemoveListener(OnExitPressed);
        valueSlider.onValueChanged.RemoveListener(OnSliderChanged);
        throwCoins.onClick.RemoveListener(OnThrowCoinsPressed);

    }
    public void ShowPanel()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }

    private void OnExitPressed()
    {
        HidePanel();
    }
    public void HidePanel()
    {
        CanvasGroup canvGroup = GetComponent<CanvasGroup>();
        if (canvGroup != null)
        {
            canvGroup.alpha = 0f;
            canvGroup.interactable = false;
            canvGroup.blocksRaycasts = false;
        }
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