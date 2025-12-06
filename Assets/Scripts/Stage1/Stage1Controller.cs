using UnityEngine;

public class Stage1Controller : MonoBehaviour
{
    public GameObject stage1;
    void Awake()
    {
        if (stage1 == null) stage1 = this.gameObject;
    }

    public void Run()
    {
        stage1.SetActive(true);
    }

    public void End()
    {
        GameManager.Instance.NextStage();
    }
}