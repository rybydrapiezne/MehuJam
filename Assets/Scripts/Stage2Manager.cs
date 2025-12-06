using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Stage2Manager : MonoBehaviour
{
    public float LEVELTIME = 30f; // base time in seconds
    [SerializeField] private float levelTime; // with modifiers

    [SerializeField]
    private float timePassed = 0f;

    public bool isPlaying = false;

    public CharacterController characterController;

    public Slider progressBar;

    public GameObject target;
    private Vector3 targetStartPos;
    private Vector3 targetEndPos;

    [Header("Obstacles")]
    public Transform lowerSpawn;
    public Transform upperSpawn;

    public GameObject[] lowerObstacles;
    public GameObject[] upperObstacles;

    public Vector2 SPAWNCOOLDOWN = new Vector2(3f, 10f);
    private float spawnCooldown = 0f;

    private void Awake()
    {
        characterController.manager = this;
        targetStartPos = target.transform.position;
        targetEndPos = new Vector3(characterController.gameObject.transform.position.x + 2, targetStartPos.y, targetStartPos.z);
    }

    private void Start()
    {
        Run();
    }

    private void Update()
    {
        timePassed += Time.deltaTime;

        if (isPlaying)
        {
            // Obstacle spawning
            spawnCooldown -= Time.deltaTime;
            if (spawnCooldown <= 0f)
            {
                if (Random.Range(0, 2) == 0)
                { //upper
                    GameObject instance = Instantiate(upperObstacles[0], upperSpawn.position, Quaternion.identity);
                    instance.transform.parent = upperSpawn;
                }
                else
                { // lower
                    GameObject instance = Instantiate(lowerObstacles[0], lowerSpawn.position, Quaternion.identity);
                    instance.transform.parent = lowerSpawn;
                }
                spawnCooldown = Random.Range(SPAWNCOOLDOWN.x, SPAWNCOOLDOWN.y);
            }

            progressBar.value = timePassed / levelTime;
            target.transform.position = Vector3.Lerp(targetStartPos, targetEndPos, timePassed / levelTime);
        }

        if (timePassed >= levelTime)
        {
            // start next stage here

            End();
        }
    }

    public void Run()
    {
        target.transform.position = targetStartPos;
        characterController.Init();
        levelTime = LEVELTIME;

        isPlaying = true;
    }

    public void End()
    {
        characterController.enabled = false;
        isPlaying = false;

        foreach(Transform child in upperSpawn)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in lowerSpawn)
        {
            Destroy(child.gameObject);
        }
    }
}
