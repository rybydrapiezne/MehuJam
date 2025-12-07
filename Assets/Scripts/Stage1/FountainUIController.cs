using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

        upgradeAButton.interactable = true;
        upgradeBButton.interactable = true;
        upgradeCButton.interactable = true;

        if (upgradeASelected) upgradeAButton.interactable = false;
        if (upgradeBSelected) upgradeBButton.interactable = false;
        if (upgradeCSelected) upgradeCButton.interactable = false;
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

        Func<UpgradeSystem.Upgrade> decideUpgrade = () =>
        {
            if (upgradeASelected) return UpgradeSystem.Upgrade.MovementSpeed;
            if (upgradeBSelected) return UpgradeSystem.Upgrade.CharacterTilt;
            return UpgradeSystem.Upgrade.PickpocketTime;
        };

        UpgradeSystem.Upgrade selectedUpgrade = decideUpgrade();

        int coinsToSpend = (int)valueSlider.value;

        int coinsNeeded = UpgradeSystem.upgradePrices[selectedUpgrade];

        if (coinsToSpend < coinsNeeded)
        {
            Debug.Log("upgrade costs " + coinsNeeded + " coins");
            return;
        }

        if (Wallet.TrySpendCoins(coinsToSpend))
        {
            Debug.Log("coins in fountain");
            if (UpgradeSystem.TryToUpgrade(selectedUpgrade))
            {
                Debug.Log("Upgrade succeed");
            } else
            {
                Debug.Log("Upgrade failed");
            }
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