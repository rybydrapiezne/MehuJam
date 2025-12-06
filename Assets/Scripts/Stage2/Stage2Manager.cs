using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Stage2Manager : MonoBehaviour
{
    [SerializeField] AnimationCurve translateCurve;

    public float LEVELTIME = 30f; // base time in seconds
    [SerializeField] private float levelTime; // with modifiers

    [SerializeField]
    private float timePassed = 0f;

    public bool isPlaying = false;

    public PlayerController playerController;

    public Slider progressBar;

    public GameObject target;
    private Vector3 targetStartPos;
    public Transform targetEndPos;

    [Header("Obstacles")]
    public Transform lowerSpawn;
    public Transform upperSpawn;

    public GameObject[] lowerObstacles;
    public GameObject[] upperObstacles;

    public Vector2 SPAWNCOOLDOWN = new Vector2(3f, 10f);
    private float spawnCooldown = 0f;

    [SerializeField] private InputActionReference tiltAction;
    public InputActionReference jumpAction;

    private Coroutine endingCoroutine;

    private void Start()
    {
        if(tiltAction == null || jumpAction == null)
        {
            tiltAction = GameManager.Instance.tiltAction;
            jumpAction = GameManager.Instance.jumpAction;
        }

        playerController.manager = this;
        targetStartPos = target.transform.position;

        target.transform.position = targetStartPos;
        playerController.Init();
        levelTime = LEVELTIME;

        StartCoroutine(EnterScene());

        isPlaying = true;
    }

    private void OnDestroy()
    {
        if (endingCoroutine != null) StopCoroutine(endingCoroutine);
    }

    private void Update()
    {
        timePassed += Time.deltaTime;

        if (isPlaying)
        {
            // Player input
            playerController.tiltInput = tiltAction.action.ReadValue<float>();

            // Obstacle spawning
            spawnCooldown -= Time.deltaTime;
            if (spawnCooldown <= 0f)
            {
                if (Random.Range(0, 2) == 0)
                { //upper
                    GameObject instance = Instantiate(upperObstacles[0], upperSpawn.position, Quaternion.identity);
                    instance.transform.parent = upperSpawn;
                    instance.GetComponent<ObstacleMover>().knockbackForce = 1f;
                }
                else
                { // lower
                    GameObject instance = Instantiate(lowerObstacles[0], lowerSpawn.position, Quaternion.identity);
                    instance.transform.parent = lowerSpawn;
                    instance.GetComponent<ObstacleMover>().knockbackForce = -1f;
                }
                spawnCooldown = Random.Range(SPAWNCOOLDOWN.x, SPAWNCOOLDOWN.y);
            }

            progressBar.value = timePassed / levelTime;

            if (timePassed >= levelTime)
            {
                End();
                endingCoroutine = StartCoroutine(ReachDestination());
            }
        }
    }

    public void End()
    {
        playerController.enabled = false;
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

    private IEnumerator EnterScene()
    {
        float time1 = 2f;
        float timer = 0f;
        Vector3 playerEndPos = playerController.transform.position;
        while (time1 > timer)
        {
            timer += Time.deltaTime;
            playerController.transform.position = Vector3.Lerp(playerEndPos + new Vector3(-100, 0, 0), playerEndPos, translateCurve.Evaluate(timer / time1));

            yield return null;
        }

        playerController.transform.position = playerEndPos;

        yield return null;
    }

    private IEnumerator ReachDestination()
    {
        float time1 = 1f;
        float time2 = 2f;
        float time3 = 1.7f;
        float timer = 0f;
        bool isChangingScene = false;
        Vector3 playerStartPos = playerController.transform.position;
        while (time2 > timer)
        {
            timer += Time.deltaTime;
            if (time1 > timer)
            {
                target.transform.position = Vector3.Lerp(targetStartPos, targetEndPos.position, translateCurve.Evaluate(timer / time1));

                playerController.tilt = Mathf.Lerp(playerController.tilt, -.1f, timer / time1);
                playerController.characterAnimator.SetTilt(playerController.tilt);
            }

            playerController.transform.position = Vector3.Lerp(playerStartPos, target.transform.position + new Vector3(-30, 11.5f, 0), translateCurve.Evaluate(timer / time2));

            if (timer > time3 && !isChangingScene)
            {
                GameManager.Instance.NextStage();
                isChangingScene = true;
            }

            yield return null;
        }

        yield return null;
    }
}
