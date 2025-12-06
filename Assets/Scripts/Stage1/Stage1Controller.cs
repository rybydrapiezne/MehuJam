using UnityEngine;

public class Stage1Controller : MonoBehaviour
{
    public static Stage1Controller Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void End()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.NextStage();
        else
            Debug.LogError("GameManager.Instance jest null!");
    }
}