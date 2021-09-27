using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float accelSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    public float driftFactor = 0.99f;

    private Rigidbody2D rigidbody;
    private bool accelerating;
    private bool reversing;
    private float turnDirection;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        accelerating = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        reversing = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (reversing)
            {
                turnDirection = -1.0f;
            }
            else
            {
                turnDirection = 1.0f;
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (reversing)
            {
                turnDirection = 1.0f;
            }
            else
            {
                turnDirection = -1.0f;
            }
        }
        else
        {
            turnDirection = 0.0f;
        }
    }
    
    // mainly used for physics due to fixed time
    private void FixedUpdate()
    {
        if (accelerating)
        {
            rigidbody.AddForce(this.transform.up * this.accelSpeed);
        }

        if (reversing)
        {
            rigidbody.AddForce(-this.transform.up * (this.accelSpeed * 0.50f));
        }

        if (turnDirection != 0.0f)
        {
            rigidbody.AddTorque(this.turnDirection * this.turnSpeed);
        }

        // reduce drift only when letting off of the accelerator
        if (!Input.GetKey(KeyCode.W))
        {
            ReduceOrthogonalVelocity();
        }
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rigidbody.velocity);
    }

    void ReduceOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rigidbody.velocity, transform.right);

        rigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public bool IsTyreSlipping(out float lateralVelocity)
    {
        lateralVelocity = GetLateralVelocity();

        if (Mathf.Abs(GetLateralVelocity()) > 0.5f)
        {
            return true;
        }

        return false;
    }

}
