using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Character tilt")]
    public float tilt = 0f;

    [SerializeField] private AnimationCurve tiltTimeCurve; // time for tilting to one direction
    [SerializeField] private AnimationCurve tiltSpeedCurve; // speed for tilting
    private float tiltDirection = 1f;
    public float tiltSpeed = 70f; // in degrees per second

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
    public PlayerAnimator characterAnimator;

    public float tiltInput = 0f;

    private IEnumerator tiltingCoroutine;

    [HideInInspector]
    public Stage2Manager manager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        externalForcesModifier = UpgradeSystem.CharacterTiltModifier;
        playerInputModifier += 1f - UpgradeSystem.CharacterTiltModifier;
    }

    private void Update()
    {
        if (manager.isPlaying)
            GameLoop(Time.deltaTime);
    }

    private void GameLoop(float deltaTime)
    {
        playerInputTiltStrength = Mathf.Lerp(playerInputTiltStrength, -tiltInput, deltaTime * 4f);
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

        manager.jumpAction.action.performed += Jump;

        if (tiltingCoroutine != null) StopCoroutine(tiltingCoroutine);
        tiltingCoroutine = ApplyRandomTilt();
        StartCoroutine(tiltingCoroutine);
    }

    private void OnDestroy()
    {
        manager.jumpAction.action.performed -= Jump;
    }

    public void ApplyPhysicalForce(float force)
    {
        if (manager.isPlaying)
        {
            physicsTiltStrength += force;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && manager.isPlaying)
        {
            if (Mathf.Abs(tilt) < 0.7f)
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
