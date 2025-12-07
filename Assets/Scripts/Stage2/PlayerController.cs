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
    public PlayerAnimator playerAnimator;

    public float tiltInput = 0f;

    private IEnumerator tiltingCoroutine;
    [SerializeField]
    SoundAnimationController soundController;
    [HideInInspector]
    public Stage2Manager manager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator.playerController = this;
        externalForcesModifier = UpgradeSystem.characterTiltModifier;
        playerInputModifier += UpgradeSystem.characterTiltModifier - 1f;
    }

    private void Update()
    {
        if (manager.isPlaying)
            GameLoop(Time.deltaTime);
    }

    private void GameLoop(float deltaTime)
    {
        playerInputTiltStrength = Mathf.Lerp(playerInputTiltStrength, -tiltInput, deltaTime * 4f);
        physicsTiltStrength = Mathf.Lerp(physicsTiltStrength, tilt, deltaTime * 2f);

        randomnessTiltStrength = Mathf.Lerp(randomnessTiltStrength, 0, deltaTime / 4f);

        tilt += (tiltSpeed * (playerInputModifier * playerInputTiltStrength + (Mathf.Sign(randomnessTiltStrength) * tiltSpeedCurve.Evaluate(Mathf.Abs(randomnessTiltStrength)) + physicsTiltStrength + 0.05f) / externalForcesModifier )) * deltaTime / 90f;
        tilt = Mathf.Clamp(tilt, -1f, 1f);

        if(Mathf.Abs(tilt) >= 1f)
        {
            GetComponent<AudioController>().PlayFellOver();
            Debug.Log("oops fell over");
            playerAnimator.FallOver(tilt);
            manager.isPlaying = false;
        }

        playerAnimator.SetTilt(tilt);
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
            if (force < 0f)
            {
                GetComponent<AudioController>().PlayHeadHurtClip();
            }
            else
            {
                GetComponent<AudioController>().PlayHitLegClip();
            }
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
            GetComponent<AudioController>().PlaySound();
            soundController.duringJump = true;
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
            soundController.duringJump = false;
            isGrounded = true;
        }
    }
}
