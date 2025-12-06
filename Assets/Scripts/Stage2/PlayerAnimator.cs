using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator hipki;
    public Animator downChar;

    private int tiltParamID;

    private void Awake()
    {
        tiltParamID = Animator.StringToHash("TiltNormal");
    }

    public void SetTilt(float tilt)
    {
        hipki.SetFloat(tiltParamID, Mathf.Clamp((-tilt + 1) / 2f, 0f, 0.99f));
        transform.eulerAngles = new Vector3(0, 0, tilt*30);
    }
}
