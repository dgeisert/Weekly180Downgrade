using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public static Truck instance;

    public float acceleration = 1;
    public float turnSpeed = 1;

    public Trailer trailer;

    Rigidbody rb;

    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = -0.4f * Vector3.up;
    }

    void Update()
    {
        if (Controls.Up)
        {
            rb.AddForce(acceleration * transform.forward);
        }
        else if (Controls.Down)
        {
            rb.AddForce(-acceleration * transform.forward);
        }
        if (Controls.Left)
        {
            transform.eulerAngles -= Vector3.up * Time.deltaTime * turnSpeed;
        }
        else if (Controls.Right)
        {
            transform.eulerAngles += Vector3.up * Time.deltaTime * turnSpeed;
        }
        rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity.magnitude * transform.forward, 0.5f);
        trailer.HingeAngle(rb.velocity);
    }
}