using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using TMPro;

public class Player : Agent
{
    Rigidbody rBody;
    public Transform ball;
    public GameObject ballGameobject;  
    private BallController ballController;  
    public SphereCollider hitAreaCollider; 
    float moveSpeed = 13f; 

    
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        hitAreaCollider = GetComponent<SphereCollider>();
        ballController = ballGameobject.GetComponent<BallController>();
    }

    public override void OnEpisodeBegin()
    {

        //if (this.transform.localPosition.x < -9.5 || this.transform.localPosition.x > 9.5
        //    || this.transform.localPosition.z < -15.5 || this.transform.localPosition.z > 15.5)
        //{
        //    this.rBody.angularVelocity = Vector3.zero;
        //    this.rBody.velocity = Vector3.zero;
        //    this.transform.localPosition = new Vector3(-3, 0.5f, 5);
        //}
        this.transform.localPosition = new Vector3(0, 0.1f, -4);
        ballController.ballRb.velocity = Vector3.zero;
        ballController.ballRb.angularVelocity = Vector3.zero;
        //ball.localPosition = new Vector3(Random.value * 8 - 4, 20f, Random.value * 8 - 4);
        ball.localPosition = new Vector3(Random.value * 9 - 4.5f, 30f, -Random.value * 9);
        //ball.localPosition = new Vector3(0, 20f, -4);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //var discreteActionsOut = actionsOut.DiscreteActions;
        //if (Input.GetKey(KeyCode.W))
        //{
        //    print(2);
        //    ballController.ApplyCurveTrajectory();
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    discreteActionsOut[0] = 2;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    discreteActionsOut[2] = 1;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    discreteActionsOut[2] = 2;
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    discreteActionsOut[1] = 1;
        //}
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    discreteActionsOut[1] = 2;
        //}
        //Vector3 movementVector = Vector3.zero;


        //Vector3 movementVector = Vector3.zero;

        //if (transform.localPosition.y < 0.1f)
        //{
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        movementVector.x = 0.8f;
        //    }
        //    else if (Input.GetKey(KeyCode.W))
        //    {
        //        movementVector.x = -0.8f;
        //    }
        //    else if (Input.GetKey(KeyCode.D))
        //    {
        //        movementVector.z = 1;
        //    }
        //    else if (Input.GetKey(KeyCode.A))
        //    {
        //        movementVector.z = -0.8f;
        //    }
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        movementVector.y = 1;
        //    }

        //    Vector3 velocity = movementVector * moveSpeed;
        //    rBody.velocity = velocity;
        //}

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float[] landingPosition = new float[2];
        landingPosition[0] = ball.localPosition.x;
        landingPosition[1] = ball.localPosition.z;
        float[] PlayerPosition = new float[2];
        PlayerPosition[0] = this.transform.localPosition.x;
        PlayerPosition[1] = this.transform.localPosition.z;
        sensor.AddObservation(landingPosition);
        sensor.AddObservation(PlayerPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int movement = actionBuffers.DiscreteActions[0];
        int jump = actionBuffers.DiscreteActions[1];
        int deffence = actionBuffers.DiscreteActions[2];
        int direction = actionBuffers.DiscreteActions[3];
        int set = actionBuffers.DiscreteActions[4];

        Vector3 movementVector = Vector3.zero;

        if (this.transform.localPosition.y < 0.1f)
        {
            if (movement == 1)
            {
                movementVector.x = 1f;
            }
            else if (movement == 2)
            {
                movementVector.x = -1f;
            }
            else if (movement == 3)
            {
                movementVector.z = 1;
            }
            else if (movement == 4)
            {
                movementVector.z = -1f;
            }

            //if (jump == 1)
            //{
            //    movementVector.y = 1f;
            //}

            Vector3 velocity = movementVector * moveSpeed;
            rBody.velocity = velocity;
        }

        if (IsBallInHitArea())
        {
            AddReward(0.3f);
            float initialSpeed = 3;
            float curveStrength = 6;
            float angel = (direction - 6) * 15;
            int curveStrengthIndex = set / 4;
            int initialSpeedIndex = set % 4;

            if (curveStrengthIndex == 0)
            {
                curveStrength = 6f;
            }
            else if (curveStrengthIndex == 1)
            {
                curveStrength = 8f;
            }
            //else if (curveStrengthIndex == 2)
            //{
            //    curveStrength = 10f;
            //}

            if (initialSpeedIndex == 0)
            {
                initialSpeed = 0f;
            }
            else if (initialSpeedIndex == 1)
            {
                initialSpeed = 3f;
            }
            else if (initialSpeedIndex == 2)
            {
                initialSpeed = 6f;
            }
            else if (initialSpeedIndex == 3)
            {
                initialSpeed = 10f;
            }

            ballController.SetTrajectory(initialSpeed, curveStrength, angel);
        }

        //if (IsBallInHitArea())
        //{
        //    ballController.SetTrajectory(10f, 8f, 90f);
        //}

        //float distanceToTarget = Vector3.Distance(this.transform.localPosition, ball.localPosition);
        //if (distanceToTarget < 0.7f)
        //{
        //    EndEpisode();
        //}
        //if (ball.localPosition.y < 0.5f)
        //{
        //    EndEpisode();
        //}
        //if (this.transform.localPosition.x < -9.5 || this.transform.localPosition.x > 9.5 
        //    || this.transform.localPosition.z < -15.5 || this.transform.localPosition.z > 15.5)
        //{
        //    SetReward(-1.0f);
        //    EndEpisode();
        //}
        //if (this.transform.localPosition.x < 0 || this.transform.localPosition.x > 5
        //    || this.transform.localPosition.z < -9 || this.transform.localPosition.z > 0)
        //{
        //    SetReward(-0.1f);
        //    EndEpisode();
        //}
    }

    bool IsBallInHitArea()
    {
        return hitAreaCollider.bounds.Contains(ball.position);
    }
}
