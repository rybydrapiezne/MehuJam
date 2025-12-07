using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class TutorialHandler : MonoBehaviour
{
    public GameObject tutorialStage1Prefab;
    public GameObject tutorialStage2Prefab;
    public GameObject tutorialStage3Prefab;

    private bool shownStage1 = false;
    private bool shownStage2 = false;
    private bool shownStage3 = false;

    [SerializeField] private Canvas parentCanvas;

    private void Awake()
    {
        if (parentCanvas == null)
        {
            parentCanvas = FindFirstObjectByType<Canvas>();
        }
    }

    public void ShowTutorial(int stageNumber, Action onComplete)
    {
        StartCoroutine(ShowTutorialCoroutine(stageNumber, onComplete));
    }

    private IEnumerator ShowTutorialCoroutine(int stageNumber, Action onComplete)
    {
        GameObject prefab = null;

        switch (stageNumber)
        {
            case 1:
                if (shownStage1) { onComplete?.Invoke(); yield break; }
                shownStage1 = true;
                prefab = tutorialStage1Prefab;
                break;
            case 2:
                if (shownStage2) { onComplete?.Invoke(); yield break; }
                shownStage2 = true;
                prefab = tutorialStage2Prefab;
                break;
            case 3:
                if (shownStage3) { onComplete?.Invoke(); yield break; }
                shownStage3 = true;
                prefab = tutorialStage3Prefab;
                break;
            default:
                onComplete?.Invoke();
                yield break;
        }

        if (prefab == null)
        {
            onComplete?.Invoke();
            yield break;
        }

        if (parentCanvas == null)
        {
            onComplete?.Invoke();
            yield break;
        }
        

        Time.timeScale = 0f;

        GameObject panel = Instantiate(prefab, parentCanvas.transform);
        panel.SetActive(true);
        panel.transform.SetAsLastSibling();


        bool closed = false;
        Button btn = panel.GetComponentInChildren<Button>();
        if (btn == null)
        {
            float timer = 0f;
            while (timer < 2f)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            closed = true;
        }
        else
        {
            btn.onClick.AddListener(() => closed = true);
        }

        while (!closed)
        {
            yield return null;
        }

        Destroy(panel);
        Time.timeScale = 1f;

        onComplete?.Invoke();
    }
}
