using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FountainUIController : MonoBehaviour
{
    public Slider valueSlider;
    public Button plusButton;
    public Button minusButton;
    public Button exitButton;
    public TMP_Text valueText;

    public int minValue = 0;
    public int maxValue = 10;
    public int startValue = 0;

    private void Awake()
    {
        valueSlider.minValue = minValue;
        valueSlider.maxValue = maxValue;
        valueSlider.wholeNumbers = true;
        valueSlider.value = Mathf.Clamp(startValue, minValue, maxValue);

        plusButton.onClick.AddListener(OnPlusPressed);
        minusButton.onClick.AddListener(OnMinusPressed);
        exitButton.onClick.AddListener(OnExitPressed);
        valueSlider.onValueChanged.AddListener(OnSliderChanged);

        UpdateValueText((int)valueSlider.value);
    }

    private void OnDestroy()
    {
        plusButton.onClick.RemoveListener(OnPlusPressed);
        minusButton.onClick.RemoveListener(OnMinusPressed);
        exitButton.onClick.RemoveListener(OnExitPressed);
        valueSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnPlusPressed()
    {
        int newVal = Mathf.Clamp((int)valueSlider.value + 1, minValue, maxValue);
        if (newVal != (int)valueSlider.value)
        {
            valueSlider.value = newVal;
        }
    }

    private void OnMinusPressed()
    {
        int newVal = Mathf.Clamp((int)valueSlider.value - 1, minValue, maxValue);
        if (newVal != (int)valueSlider.value)
        {
            valueSlider.value = newVal;
        }
    }

    private void OnExitPressed()
    {
        gameObject.SetActive(false);
    }

    private void OnSliderChanged(float value)
    {
        UpdateValueText((int)value);
    }
    
    private void UpdateValueText(int intValue)
    {
        valueText.text = intValue.ToString();
    }
}