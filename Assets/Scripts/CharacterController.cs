using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("Character tilt")]
    [SerializeField]
    private float tilt = 0f;

    public AnimationCurve tiltTimeCurve; // time for tilting to one direction
    public float minTiltFliptime = 0.5f;
    public float maxTiltFliptime = 3f;
    public AnimationCurve tiltSpeedCurve; // speed for tilting, bounces back
    private float varTiltSpeed = 0f;
    private float varTiltSpeedSig = 1f;
    private float tiltDirection = 1f;
    public float tiltSpeed = 30f; // in degrees per second


    [Header("Character movement")]
    public float jumpForce = 200f;
    public float LEVELTIME = 30f; // base time in seconds
    [SerializeField] private float levelTime; // with modifiers

    [SerializeField]
    private float timePassed = 0f;

    [SerializeField]
    private bool isPlaying = false;

    private Rigidbody2D rb;
    private CharacterAnimator characterAnimator;

    private InputAction tiltAction;
    private InputSystem_Actions inputSystem;

    private IEnumerator tiltingCoroutine;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        tiltAction = inputSystem.FindAction("Rotate");
        inputSystem.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<CharacterAnimator>();
        Run();
    }

    private void Update()
    {
        if (isPlaying)
            GameLoop(Time.deltaTime);
    }

    private void GameLoop(float deltaTime)
    {
        timePassed += deltaTime;
        
        // Tilt speed
        varTiltSpeed += deltaTime * varTiltSpeedSig;

        if (varTiltSpeed <= 0f){
            varTiltSpeed = 0f;
            varTiltSpeedSig = 1f;
        }

        if (varTiltSpeed >= 1f)
        {
            varTiltSpeed = 1f;
            varTiltSpeedSig = -1f;
        }

        // Tilt direction choosing
        tilt += tiltSpeed * tiltSpeedCurve.Evaluate(varTiltSpeed) * tiltDirection * deltaTime / 90f;
        tilt += tiltSpeed * 1.2f * -tiltAction.ReadValue<float>() * deltaTime / 90f;
        tilt = Mathf.Clamp(tilt, -1f, 1f);

        characterAnimator.SetTilt(tilt);

        if (timePassed >= levelTime)
        {
            // start next stage
            enabled = false;
            isPlaying = false;
        }
    }

    public void Run()
    {
        enabled = true;
        timePassed = 0;
        levelTime = LEVELTIME;

        tiltingCoroutine = FlipTiltDirection();
        StartCoroutine(tiltingCoroutine);

        isPlaying = true;
    }

    private IEnumerator FlipTiltDirection()
    {
        while(true)
        {
            float timeToWait = minTiltFliptime + tiltTimeCurve.Evaluate(Random.Range(0f, 1f)) * (maxTiltFliptime - minTiltFliptime);

            yield return new WaitForSeconds(timeToWait);
            tiltDirection = -tiltDirection;
        }
    }
}
