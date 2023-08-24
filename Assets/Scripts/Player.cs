using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using TMPro;

public enum Team
{
    Blue = 0,
    Red = 1
}
public class Player : Agent
{
    Rigidbody rBody;
    public Transform ball;
    public GameObject ballGameobject;
    private BallController ballController;  
    public SphereCollider hitAreaCollider;
    BehaviorParameters behaviorParameters;
    GameController gameController;
    float moveSpeed = 10f;
    public Team team;
    public int playerNumber;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        hitAreaCollider = GetComponent<SphereCollider>();
        ballController = ballGameobject.GetComponent<BallController>();
        behaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        gameController = GetComponentInParent<GameController>();
        if (behaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
        }
        else
        {
            team = Team.Red;
        }
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
        //this.transform.localPosition = new Vector3(0, 0.1f, -4);
        //ballController.ballRb.velocity = Vector3.zero;
        //ballController.ballRb.angularVelocity = Vector3.zero;
        //ball.localPosition = new Vector3(Random.value * 8 - 4, 20f, Random.value * 8 - 4);
        //ball.localPosition = new Vector3(Random.value * 9 - 4.5f, 30f, -Random.value * 9);
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
        float[] landingPosition = new float[3];
        landingPosition[0] = ball.localPosition.x;
        landingPosition[1] = ball.localPosition.z;
        float[] PlayerPosition = new float[2];
        PlayerPosition[0] = this.transform.localPosition.x;
        PlayerPosition[1] = this.transform.localPosition.z;
        sensor.AddObservation(landingPosition);
        sensor.AddObservation(PlayerPosition);
        if (this.team == Team.Blue)
        {
            foreach (var i in gameController.bluePlayersList)
            {
                if (i.player.playerNumber != this.playerNumber)
                {
                    float[] PlayerPos = new float[2];
                    PlayerPos[0] = i.player.transform.localPosition.x;
                    PlayerPos[1] = i.player.transform.localPosition.z;
                    sensor.AddObservation(PlayerPos);
                }
            }
            foreach (var i in gameController.redPlayersList)
            {
                float[] PlayerPos = new float[2];
                PlayerPos[0] = i.player.transform.localPosition.x;
                PlayerPos[1] = i.player.transform.localPosition.z;
                sensor.AddObservation(PlayerPos);
            }
        }
        else
        {
            foreach (var i in gameController.redPlayersList)
            {
                if (i.player.playerNumber != this.playerNumber)
                {
                    float[] PlayerPos = new float[2];
                    PlayerPos[0] = i.player.transform.localPosition.x;
                    PlayerPos[1] = i.player.transform.localPosition.z;
                    sensor.AddObservation(PlayerPos);
                }
            }
            foreach (var i in gameController.bluePlayersList)
            {
                float[] PlayerPos = new float[2];
                PlayerPos[0] = i.player.transform.localPosition.x;
                PlayerPos[1] = i.player.transform.localPosition.z;
                sensor.AddObservation(PlayerPos);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int movement = actionBuffers.DiscreteActions[0];
        //int jump = actionBuffers.DiscreteActions[1];
        //int deffence = actionBuffers.DiscreteActions[2];
        int direction = actionBuffers.DiscreteActions[1];
        int strike = actionBuffers.DiscreteActions[2];

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
            //gameController.blueGroup.AddGroupReward(1);
            float initialSpeed = 3;
            float curveStrength = 6;
            int curveStrengthIndex = strike / 4;
            int initialSpeedIndex = strike % 4;

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
                initialSpeed = 2f;
            }
            else if (initialSpeedIndex == 1)
            {
                initialSpeed = 4f;
            }
            else if (initialSpeedIndex == 2)
            {
                initialSpeed = 6f;
            }
            else if (initialSpeedIndex == 3)
            {
                initialSpeed = 10f;
            }

            float angel = (direction - 6) * 15;
            if (this.team == Team.Red)
            {
                angel = angel - 180;
            }

            if (this.team == Team.Blue)
            {
                gameController.lastHit = 0;
            }
            else
            {
                gameController.lastHit = 1;
            }

            ballController.SetTrajectory(initialSpeed, curveStrength, angel);
        }

        //if (IsBallInHitArea())
        //{
        //    ballController.SetTrajectory(5f, 5f, 90f);
        //}
    }

    bool IsBallInHitArea()
    {
        return hitAreaCollider.bounds.Contains(ball.position);
    }
}
