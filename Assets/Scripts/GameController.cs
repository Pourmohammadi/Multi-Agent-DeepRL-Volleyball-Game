using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class GameController : MonoBehaviour
{
    public GameObject ball;
    [HideInInspector]
    public Rigidbody ballRb;
    private int resetTimer;

    [System.Serializable]
    public class PlayerInfo
    {
        public Player player;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
        [HideInInspector]
        public Rigidbody Rb;
    }

    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    public List<PlayerInfo> playersList = new List<PlayerInfo>();

    [HideInInspector]
    public SimpleMultiAgentGroup blueGroup;

    void Start()
    {
        blueGroup = new SimpleMultiAgentGroup();
        ballRb = ball.GetComponent<Rigidbody>();
        
        foreach (var i in playersList)
        {
            i.StartingPos = i.player.transform.position;
            i.StartingRot = i.player.transform.rotation;
            i.Rb = i.player.GetComponent<Rigidbody>();
            if (i.player.team == Team.Blue)
            {
                blueGroup.RegisterAgent(i.player);
            }
        }
    }
    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            blueGroup.GroupEpisodeInterrupted();
            ResetGame();
        }
    }
    public void FieldTouched(bool win)
    {
        if (win)
        {
            blueGroup.AddGroupReward(1);
        }
        else
        {
            blueGroup.AddGroupReward(-1);
        }
        blueGroup.EndGroupEpisode();
        ResetGame();
    }
    public void ResetGame()
    {
        resetTimer = 0;

        foreach (var i in playersList)
        {
            i.Rb.velocity = Vector3.zero;
            i.Rb.angularVelocity = Vector3.zero;
            i.player.transform.SetPositionAndRotation(i.StartingPos, i.StartingRot);
        }

        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ball.transform.localPosition = new Vector3(Random.value * 9 - 4.5f, 30f, -Random.value * 8.5f - 0.5f);
    }
}
