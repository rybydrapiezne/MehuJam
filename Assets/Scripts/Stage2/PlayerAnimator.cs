using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator hipki;
    [SerializeField] Animator hipkiCloak;
    [SerializeField] Animator downChar;

    [SerializeField] GameObject runningAnim;
    [SerializeField] Animator fallingAnim;

    private int tiltParamID;

    [HideInInspector] public PlayerController playerController;

    private void Awake()
    {   
        fallingAnim.gameObject.SetActive(false);
        runningAnim.SetActive(true);
        tiltParamID = Animator.StringToHash("TiltNormal");
    }

    public void SetTilt(float tilt)
    {
        float normalized = Mathf.Clamp((-tilt + 1) / 2f, 0f, 0.99f);
        hipki.SetFloat(tiltParamID, normalized);
        hipkiCloak.SetFloat(tiltParamID, normalized);
        transform.eulerAngles = new Vector3(0, 0, tilt*30);
    }

    public void FallOver(float tilt)
    {
        runningAnim.SetActive(false);

        fallingAnim.gameObject.SetActive(true);
        if (tilt < 0f) fallingAnim.Play("Base Layer.Tumble_R");
        else fallingAnim.Play("Base Layer.Tumble_L");

        foreach (Transform child in playerController.manager.upperSpawn)
        {
            Destroy(child.GetComponent<ObstacleMover>());
        }

        foreach (Transform child in playerController.manager.lowerSpawn)
        {
            Destroy(child.GetComponent<ObstacleMover>());
        }

        playerController.manager.particles.Stop();
        StartCoroutine(waitForStage1ChangeForIdkFewSeconds());
    }

    private IEnumerator waitForStage1ChangeForIdkFewSeconds()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.BackToStage1();
    }
}
