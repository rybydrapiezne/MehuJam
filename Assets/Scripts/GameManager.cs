using UnityEngine;

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

    public Stage1Controller stage1;
    public Stage2Manager stage2;
    public Stage3Controller stage3;

    private int currentStage = 0;

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
        currentStage++;

        // might change to instantiating/destroying stage controllers

        if (currentStage == 1)
        {
            stage3.gameObject.SetActive(false);
            stage1.gameObject.SetActive(true);
            stage1.Run();
        }
        else if (currentStage == 2)
        {
            stage1.gameObject.SetActive(false);
            stage2.gameObject.SetActive(true);
            //stage2.Run();
        }
        else if (currentStage == 3)
        {
            stage2.gameObject.SetActive(false);
            stage3.gameObject.SetActive(true);
            stage3.enabled = true;
            currentStage = 0;
        }
    }
}
