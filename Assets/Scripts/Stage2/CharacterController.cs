using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("Character tilt")]
    [SerializeField]
    private float tilt = 0f;

    [SerializeField] private AnimationCurve tiltTimeCurve; // time for tilting to one direction
    [SerializeField] private AnimationCurve tiltSpeedCurve; // speed for tilting
    private float tiltDirection = 1f;
    public float tiltSpeed = 60f; // in degrees per second

    // for upgrade system, difficulty etc
    public float playerInputModifier = 1.5f;
    public float externalForcesModifier = 1f;
    public float maxTiltFliptime = 3f;

    [SerializeField] private float playerInputTiltStrength = 0f;
    [SerializeField] private float randomnessTiltStrength = 0f;
    [SerializeField] private float physicsTiltStrength = 0f;

    [Header("Character movement")]
    [SerializeField] private float jumpForce = 400f;
    private bool isGrounded = true;

    private Rigidbody2D rb;
    public CharacterAnimator characterAnimator;

    [SerializeField] private InputActionReference tiltAction;
    [SerializeField] private InputActionReference jumpAction;

    private IEnumerator tiltingCoroutine;

    [HideInInspector]
    public Stage2Manager manager;

    private void Awake()
    {
        jumpAction.action.performed += Jump;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (manager.isPlaying)
            GameLoop(Time.deltaTime);
    }

    private void GameLoop(float deltaTime)
    {
        playerInputTiltStrength = Mathf.Lerp(playerInputTiltStrength, -tiltAction.action.ReadValue<float>(), deltaTime * 4f);
        physicsTiltStrength = Mathf.Lerp(physicsTiltStrength, tilt, deltaTime * 4f);

        randomnessTiltStrength = Mathf.Lerp(randomnessTiltStrength, 0, deltaTime / 4f);

        tilt += (tiltSpeed * (playerInputModifier * playerInputTiltStrength + externalForcesModifier * (Mathf.Sign(randomnessTiltStrength) * tiltSpeedCurve.Evaluate(Mathf.Abs(randomnessTiltStrength)) + physicsTiltStrength + 0.05f))) * deltaTime / 90f;
        tilt = Mathf.Clamp(tilt, -1f, 1f);

        if(Mathf.Abs(tilt) >= 1f)
        {
            Debug.Log("oops fell over");
        }

        characterAnimator.SetTilt(tilt);
    }

    public void Init()
    {
        tilt = 0f;
        playerInputTiltStrength = 0f;
        randomnessTiltStrength = 0f;
        physicsTiltStrength = 0f;
        if (tiltingCoroutine != null) StopCoroutine(tiltingCoroutine);
        tiltingCoroutine = ApplyRandomTilt();
        StartCoroutine(tiltingCoroutine);

        enabled = true;
    }

    public void ApplyPhysicalForce(float force)
    {
        if (enabled)
        {
            physicsTiltStrength += force;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            if (Mathf.Abs(tilt) < 0.3f)
            {
                physicsTiltStrength *= 2f;
            }
            else
            {
                physicsTiltStrength *= 20f;
            }
            rb.AddForceY(jumpForce);
            isGrounded = false;
        }
    }

    private IEnumerator ApplyRandomTilt()
    {
        while(true)
        {
            float timeToWait = tiltTimeCurve.Evaluate(Random.Range(0f, 1f)) * maxTiltFliptime;

            yield return new WaitForSeconds(timeToWait);
            if(Mathf.Abs(tilt) < .3f){
                tiltDirection = (Random.Range(0,2) * 2) - 1;
                randomnessTiltStrength = tiltDirection * tiltTimeCurve.Evaluate(Random.Range(0f, 1f));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }
}
