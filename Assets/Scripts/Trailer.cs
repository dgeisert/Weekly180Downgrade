using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer : MonoBehaviour
{
    [SerializeField] Transform back;
    Vector3 previousBack;

    void Start()
    {
        previousBack = back.position;
    }

    public void HingeAngle(Vector3 velocity)
    {
        if (velocity != Vector3.zero)
        {
            transform.LookAt(transform.position + Vector3.Lerp(previousBack, transform.position + velocity, 0.5f));
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            previousBack = back.position;
        }
    }
}