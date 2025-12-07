using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class FountainUIController : MonoBehaviour
{
    [SerializeField] private Slider valueSlider;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button throwCoins;
    [SerializeField] private TMP_Text valueText;

    [SerializeField] private UpgradeSystem.Upgrade[] upgradeOrder;
    [SerializeField] private Button[] upgradeButtons;

    public UpgradeSystem.Upgrade selectedUpgrade = UpgradeSystem.Upgrade.None;

    public int minValue = 0;
    public int maxValue = 10;
    public int startValue = 0;

    [SerializeField] TMP_Text log;

    private void Awake()
    {
        HidePanel();
        selectedUpgrade = UpgradeSystem.Upgrade.None;

        valueSlider.minValue = minValue;
        valueSlider.maxValue = maxValue;
        valueSlider.wholeNumbers = true;
        valueSlider.value = Mathf.Clamp(startValue, minValue, maxValue);

        exitButton.onClick.AddListener(OnExitPressed);
        valueSlider.onValueChanged.AddListener(OnSliderChanged);
        throwCoins.onClick.AddListener(OnThrowCoinsPressed);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i;
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(index));
        }

        UpdateValueText((int)valueSlider.value);
    }

    private void SelectUpgrade(int upgrade)
    {
        selectedUpgrade = upgradeOrder[upgrade];

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = true;
            if (upgradeOrder[i] == selectedUpgrade) upgradeButtons[i].interactable = false;
        }
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
        if (selectedUpgrade == UpgradeSystem.Upgrade.None)
        {
            Debug.Log("Select an upgrade first!");
            log.text = "Select an upgrade first!";
            return;
        }

        int coinsToSpend = (int)valueSlider.value;

        int coinsNeeded = UpgradeSystem.upgradePrices[selectedUpgrade];

        if (coinsToSpend < coinsNeeded)
        {
            Debug.Log("upgrade costs " + coinsNeeded + " coins");
            log.text = "This upgrade costs " + coinsNeeded + " coins";
            return;
        }

        if (Wallet.TrySpendCoins(coinsToSpend))
        {
            Debug.Log("coins in fountain");
            log.text = "Coins threw to fountain";
            if (UpgradeSystem.TryToUpgrade(selectedUpgrade))
            {
                Debug.Log("Upgrade succeed");
                log.text += " and upgraded";
            } else
            {
                Debug.Log("Upgrade failed");
                log.text += " and failed";
            }
        }
        else
        {
            Debug.Log("not enough coins");
            log.text = "Not enough coins";
        }
    }


    private void UpdateValueText(int intValue)
    {
        valueText.text = intValue.ToString();
    }
}