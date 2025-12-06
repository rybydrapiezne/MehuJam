using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Stage 2")]
    public InputActionReference tiltAction;
    public InputActionReference jumpAction;

    [Header("Stage 3")]
    public InputActionReference attackAction;
    public InputActionReference retractAction;
    public InputActionReference interactAction;

    [Header("General")]
    public GameObject stage1Prefab;
    public GameObject stage2Prefab;
    public GameObject stage3Prefab;

    private Stage1Controller stage1;
    private Stage2Manager stage2;
    private Stage3Controller stage3;

    private int currentStage = 0;

    [Header("UI")]
    [SerializeField] private CanvasGroup fadeCanvas;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        NextStage();
    }

    public void NextStage()
    {
        StartCoroutine(FadeTransition(1f, ChangeStage));
    }

    private IEnumerator FadeTransition(float time, Action delg)
    {
        time = time / 2f;
        float timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, timer / time);

            yield return null;
        }

        delg();

        timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, timer / time);

            yield return null;
        }
        fadeCanvas.alpha = 0f;
    }

    private void ChangeStage()
    {
        currentStage++;
        if(currentStage == 1) currentStage++;
        Debug.Log("current stage: " + currentStage);

        if (currentStage == 1)
        {
            if (stage3 != null) Destroy(stage3.gameObject);
            stage1 = Instantiate(stage1Prefab).GetComponent<Stage1Controller>();
        }
        else if (currentStage == 2)
        {
            if (stage1 != null) Destroy(stage1.gameObject);
            stage2 = Instantiate(stage2Prefab).GetComponent<Stage2Manager>();
        }
        else if (currentStage == 3)
        {
            if (stage2 != null) Destroy(stage2.gameObject);
            stage3 = Instantiate(stage3Prefab).GetComponent<Stage3Controller>();
            currentStage = 0;
        }
    }
}
