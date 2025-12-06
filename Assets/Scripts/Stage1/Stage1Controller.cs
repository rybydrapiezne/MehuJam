using UnityEngine;

public class Stage1Controller : MonoBehaviour
{
    public void End()
    {
        GameManager.Instance.NextStage();
    }
}