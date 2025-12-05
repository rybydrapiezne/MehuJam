using System;
using UnityEngine;

public class ArmRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed=5f;
    [SerializeField] float maxRotationAmount = 50f;
    private bool _shouldRotate=true;
    void Update()
    {
        if (_shouldRotate)
        {
            if (Math.Abs(NormalizeAngle(transform.rotation.eulerAngles.z)) >= maxRotationAmount)
                rotateSpeed = -rotateSpeed;
            transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotateSpeed);
        }
    }

    float NormalizeAngle(float angle)
    {
        while(angle>180) return angle-360;
        while(angle<=-180) return angle+360;
        return angle;
    }

    public void StopRotation()
    {
        _shouldRotate = false;
    }
}
