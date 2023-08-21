using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody ballRb;
    public Player player;
    public void SetTrajectory(float initialSpeed, float curveStrength, float angle)
    {
        Vector3 initialVelocity = Quaternion.Euler(0, angle, 0) * Vector3.forward * initialSpeed;
        Vector3 upwardForce = Vector3.up * curveStrength;
        Vector3 totalVelocity = initialVelocity + upwardForce;
        ballRb.velocity = totalVelocity;;
    }

    public Vector3 CalculateFinalDestination(Vector3 initialPosition, Vector3 initialVelocity)
    {
        float timeToReachTargetY = CalculateTimeToReachY(initialPosition.y, 1.9f, initialVelocity.y);

        Vector3 finalPosition = CalculatePositionAtTime(initialPosition, initialVelocity, timeToReachTargetY);

        return finalPosition;
    }

    private float CalculateTimeToReachY(float initialHeight, float targetY, float initialVerticalVelocity)
    {

        float gravity = Physics.gravity.y;
        float discriminant = initialVerticalVelocity * initialVerticalVelocity - 2 * gravity * (initialHeight - targetY);

        if (discriminant < 0)
        {
            return float.NaN;
        }

        float timePositive = (-initialVerticalVelocity + Mathf.Sqrt(discriminant)) / gravity;
        float timeNegative = (-initialVerticalVelocity - Mathf.Sqrt(discriminant)) / gravity;
        float timeToReachY = Mathf.Max(timePositive, timeNegative);

        return timeToReachY;
    }

    private Vector3 CalculatePositionAtTime(Vector3 initialPosition, Vector3 initialVelocity, float time)
    {
        Vector3 position = new Vector3();

        position.x = initialPosition.x + initialVelocity.x * time;
        position.y = initialPosition.y + initialVelocity.y * time + 0.5f * Physics.gravity.y * time * time;
        position.z = initialPosition.z + initialVelocity.z * time;

        return position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BlueTeamField"))
        {
            player.AddReward(-1.0f);
            player.EndEpisode();
            //Debug.Log("Blue team's zone touched by the ball.");
        }
        else if (collision.gameObject.CompareTag("RedTeamField"))
        {
            player.AddReward(2.0f);
            player.EndEpisode();
            //Debug.Log("Red team's zone touched by the ball.");
        }
        else if (collision.gameObject.CompareTag("OuterField"))
        {
            player.AddReward(-1.0f);
            player.EndEpisode();
            //Debug.Log("Outer field touched by the ball.");
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            player.AddReward(-1.0f);
            player.EndEpisode();
            //Debug.Log("Wall touched by the ball.");
        }
    }
}
