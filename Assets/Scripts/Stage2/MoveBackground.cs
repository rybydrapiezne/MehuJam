using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] GameObject walkway;

    [SerializeField] Stage2Manager manager;

    [Header("Movement Settings")]
    public float speedBackground = 5f;

    public float speedWalkway = 10f;// Units per second (adjust in Inspector)

    void Update()
    {
        if(manager.isPlaying){
            // Moves left in world space (towards negative X) - this is what you usually want
            background.transform.Translate(Vector3.left * speedBackground * Time.deltaTime, Space.World);
            walkway.transform.Translate(Vector3.left * speedWalkway * Time.deltaTime, Space.World);
        }
    }
}
