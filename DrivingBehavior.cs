using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingBehavior : MonoBehaviour {

    GameObject car;
    Rigidbody carBody;

    float deadValue;
    
    float acceleration;
    float accelerationCap;
    float accelerationRate;
    float accelerationDeadZone;
    
    float velocity;
    float velocityCap;
    
    float inputAccel;
    float inputBrake;
    [SerializeField]
    float inputTurn;
    [SerializeField]
    float angularVelocity;

    [SerializeField]
    float prevAngle;


	// Use this for initialization
	void Start () {
        car = GameObject.Find("Car");
        carBody = this.GetComponent<Rigidbody>();
        deadValue = 0.5f;
        acceleration = 0.1f;
        velocity = 0f;
        inputAccel = 0;

        accelerationCap = 0f;
        velocityCap = 0.5f;

        accelerationRate = 1.005f;
        accelerationDeadZone = 0.009f;

        angularVelocity = 0;

        prevAngle = 0f;

        //TestStart();
	}

    void TestStart()
    {
        carBody.position = new Vector3(0, 10, 0);
        carBody.useGravity = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //TestLoop();
        RealLoop();
    }

    void TestLoop()
    {
        carBody.angularVelocity = new Vector3(0, 2, 0);
    }

    void RealLoop()
    {
        inputBrake = Input.GetAxisRaw("Brake");
        inputAccel = Input.GetAxisRaw("Accelerate");
        

        if (inputAccel > deadValue)
        {
            Accelerate();
        }
        else if (inputAccel < 0 - deadValue)
        {
            Reverse();
        }
        else if (inputBrake > deadValue)
        {
            Brake();
        }
        else
        {
            Drift();
        }

        inputTurn = Input.GetAxisRaw("Turn");
        if (inputTurn > deadValue || inputTurn < 0 - deadValue)
        {
            Turn();
        }

        this.transform.Translate(Vector3.forward * velocity);
    }

    void Accelerate()
    {
        if (acceleration < accelerationDeadZone)
            acceleration = accelerationDeadZone;
        else if (acceleration < accelerationCap)
            acceleration *= accelerationRate;
        else acceleration = accelerationCap;

        if (velocity < velocityCap)
            velocity += acceleration;
        else
            velocity = velocityCap;
    }

    void Brake()
    {
        if (Mathf.Abs(velocity) > accelerationDeadZone)
            velocity /= (accelerationRate * 1.08f);
        else velocity = 0f;
    }

    void Drift()
    {
        if (Mathf.Abs(velocity) > accelerationDeadZone)
            velocity /= (accelerationRate * 1.01f);
        else
            velocity = 0f;

    }

    void Reverse()
    {
        if (acceleration < accelerationDeadZone)
            acceleration = accelerationDeadZone;
        else if (acceleration < accelerationCap)
            acceleration *= accelerationRate;
        else acceleration = accelerationCap;

        if ((-1*velocity) < velocityCap)
            velocity -= acceleration;
        else
            velocity = 0 - velocityCap;
    }

    void Turn()
    {
        if (Mathf.Abs(velocity) > 0)
        {
            carBody.transform.Rotate(Vector3.up, inputTurn * 2.5f * velocity);
            //carBody.AddRelativeTorque(Vector3.up, inputTurn * 2.2f * velocity);
            //float angle = this.transform.eulerAngles.y;
            //this.transform.Rotate(new Vector3(0, (Vector3.up.y * inputTurn * velocity * 2.2f) * (prevAngle - angle), 0));
            //this.transform.Rotate(new Vector3(0, velocity * (prevAngle - angle), 0));
            //prevAngle = angle;
        }
    }
}
