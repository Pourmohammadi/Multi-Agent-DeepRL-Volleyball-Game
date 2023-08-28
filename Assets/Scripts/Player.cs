using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using TMPro;
using System;

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
        //        movementVector.y = 0.7f;
        //    }

        //    Vector3 velocity = movementVector * moveSpeed;
        //    rBody.velocity = velocity;
        //}

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //float[] landingPosition = new float[3];
        //landingPosition[0] = ball.localPosition.x;
        //landingPosition[1] = ball.localPosition.z;
        if (this.team == Team.Blue)
        {
            if ((gameController.blueTeamLastkicker != this.playerNumber) && (gameController.blueTeamHits < 2))
            {
                sensor.AddObservation(1);
            }
            else
            {
                sensor.AddObservation(0);
            }

            if (gameController.blueTeamHits == 0)
            {
                sensor.AddObservation(0);
            }
            else if (gameController.blueTeamHits == 1)
            {
                sensor.AddObservation(1);
            }
            else
            {
                sensor.AddObservation(2);
            }
        }
        else if (this.team == Team.Red)
        {
            if ((gameController.redTeamLastkicker != this.playerNumber) && (gameController.redTeamHits < 2))
            {
                sensor.AddObservation(1);
            }
            else
            {
                sensor.AddObservation(0);
            }

            if (gameController.redTeamHits == 0)
            {
                sensor.AddObservation(0);
            }
            else if (gameController.redTeamHits == 1)
            {
                sensor.AddObservation(1);
            }
            else
            {
                sensor.AddObservation(2);
            }
        }
        sensor.AddObservation(ball.localPosition);
        sensor.AddObservation(ballController.ballRb.velocity);
        float[] PlayerPosition = new float[2];
        PlayerPosition[0] = this.transform.localPosition.x;
        PlayerPosition[1] = this.transform.localPosition.z;
        //sensor.AddObservation(landingPosition);
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
            else if (movement == 5)
            {
                movementVector.y = 0.5f;
            }

            Vector3 velocity = movementVector * moveSpeed;
            rBody.velocity = velocity;
        }

        if (IsBallInHitArea())
        {
            if ((this.team == Team.Blue) && (gameController.blueTeamLastkicker != this.playerNumber) && (gameController.blueTeamHits < 2))
            {
                gameController.redTeamLastkicker = -1;
                gameController.redTeamHits = 0;

                if (gameController.blueTeamHits == 0)
                {
                    AddReward(0.1f);
                    HitTheBall(strike, direction, false);
                    gameController.blueTeamLastkicker = this.playerNumber;
                    gameController.blueTeamHits = 1;
                }
                else if (gameController.blueTeamHits == 1)
                {
                    AddReward(0.1f);
                    HitTheBall(strike, direction, true);
                    gameController.blueTeamLastkicker = this.playerNumber;
                    gameController.blueTeamHits = 2;
                }
            }
            else if ((this.team == Team.Red) && (gameController.redTeamLastkicker != this.playerNumber) && (gameController.redTeamHits < 2))
            {
                gameController.blueTeamLastkicker = -1;
                gameController.blueTeamHits = 0;

                if (gameController.redTeamHits == 0)
                {
                    AddReward(0.1f);
                    HitTheBall(strike, direction, false);
                    gameController.redTeamLastkicker = this.playerNumber;
                    gameController.redTeamHits = 1;
                }
                else if (gameController.redTeamHits == 1)
                {
                    AddReward(0.1f);
                    HitTheBall(strike, direction, true);
                    gameController.redTeamLastkicker = this.playerNumber;
                    gameController.redTeamHits = 2;
                }
            }
        }

        //if (IsBallInHitArea())
        //{
        //    if ((this.team == Team.Blue) && (gameController.blueTeamLastkicker != this.playerNumber))
        //    {
        //        print(1);
        //        ballController.SetTrajectory(2.5f, 8f, 0f);
        //        gameController.blueTeamLastkicker = this.playerNumber;
        //    }
        //}
    }

    void HitTheBall(int Strike, int Direction, bool isSet)
    {
        float initialSpeed = 3;
        float curveStrength = 6;
        int curveStrengthIndex = Strike / 4;
        int initialSpeedIndex = Strike % 4;

        if (isSet)
        {
            if (curveStrengthIndex == 0)
            {
                curveStrength = 1f;
            }
            else if (curveStrengthIndex == 1)
            {
                curveStrength = 4f;
            }
            else if (curveStrengthIndex == 2)
            {
                curveStrength = 6f;
            }

            if (initialSpeedIndex == 0)
            {
                initialSpeed = 5f;
            }
            else if (initialSpeedIndex == 1)
            {
                initialSpeed = 7f;
            }
            else if (initialSpeedIndex == 2)
            {
                initialSpeed = 9f;
            }
            else if (initialSpeedIndex == 3)
            {
                initialSpeed = 11f;
            }
        }
        else
        {
            if (curveStrengthIndex == 0)
            {
                curveStrength = 4f;
            }
            else if (curveStrengthIndex == 1)
            {
                curveStrength = 6f;
            }
            else if (curveStrengthIndex == 2)
            {
                curveStrength = 8f;
            }

            if (initialSpeedIndex == 0)
            {
                initialSpeed = 1f;
            }
            else if (initialSpeedIndex == 1)
            {
                initialSpeed = 1.5f;
            }
            else if (initialSpeedIndex == 2)
            {
                initialSpeed = 2f;
            }
            else if (initialSpeedIndex == 3)
            {
                initialSpeed = 2.5f;
            }
        }
        

        float angel = (Direction - 6) * 15;
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

    bool IsBallInHitArea()
    {
        return hitAreaCollider.bounds.Contains(ball.position);
    }
}
