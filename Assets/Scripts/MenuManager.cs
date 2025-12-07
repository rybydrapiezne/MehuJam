using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] string masterName = "MasterVolume";
    [SerializeField] string sfxName = "SFXVolume";

    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey(masterName)) SetVolume(PlayerPrefs.GetFloat(masterName), masterName);
        if (PlayerPrefs.HasKey(sfxName)) SetVolume(PlayerPrefs.GetFloat(sfxName), sfxName);

        setSlider(masterName, masterSlider);
        setSlider(sfxName, sfxSlider);
    }

    // Main menu
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    // Settings menu
    public void GoToMain()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    // General
    private void setSlider(string name, Slider slider)
    {
        float currentVolume;
        audioMixer.GetFloat(name, out currentVolume);
        slider.value = Mathf.Pow(10, currentVolume / 20);
        slider.onValueChanged.AddListener(v => SetVolume(v, name));
    }

    public void SetVolume(float volume, string name)
    {
        float dB = volume > 0 ? 20 * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat(name, dB);
        PlayerPrefs.SetFloat(name, volume);
    }
}
