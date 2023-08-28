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
    [HideInInspector]
    public int lastHit;
    [HideInInspector]
    public bool blueTeamTurn;
    [HideInInspector]
    public int blueTeamHits;
    [HideInInspector]
    public int redTeamHits;
    [HideInInspector]
    public int blueTeamLastkicker;
    [HideInInspector]
    public int redTeamLastkicker;

    [System.Serializable]
    public class PlayerInfo
    {
        public Player player;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Rigidbody Rb;
        [HideInInspector]
        public Team team;
    }

    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    public List<PlayerInfo> bluePlayersList = new List<PlayerInfo>();
    public List<PlayerInfo> redPlayersList = new List<PlayerInfo>();

    [HideInInspector]
    public SimpleMultiAgentGroup blueGroup;
    [HideInInspector]
    public SimpleMultiAgentGroup redGroup;

    void Start()
    {
        blueGroup = new SimpleMultiAgentGroup();
        redGroup = new SimpleMultiAgentGroup();
        ballRb = ball.GetComponent<Rigidbody>();

        lastHit = 0;
        blueTeamTurn = true;

        foreach (var i in bluePlayersList)
        {
            i.StartingPos = i.player.transform.localPosition;
            i.Rb = i.player.GetComponent<Rigidbody>();
            i.team = Team.Blue;
            blueGroup.RegisterAgent(i.player);
        }
        foreach (var i in redPlayersList)
        {
            i.StartingPos = i.player.transform.localPosition;
            i.Rb = i.player.GetComponent<Rigidbody>();
            i.team = Team.Red;
            redGroup.RegisterAgent(i.player);
        }

        ResetGame();
    }
    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            blueGroup.GroupEpisodeInterrupted();
            redGroup.GroupEpisodeInterrupted();
            ResetGame();
        }
    }
    public void gotPoint(bool blueWon)
    {
        if (blueWon)
        {
            blueGroup.AddGroupReward(1);
            redGroup.AddGroupReward(-1);
        }
        else
        {
            redGroup.AddGroupReward(1);
            blueGroup.AddGroupReward(-1);
        }
        blueGroup.EndGroupEpisode();
        redGroup.EndGroupEpisode();
        ResetGame();
    }
    public void ResetGame()
    {
        resetTimer = 0;

        blueTeamHits = 0;
        redTeamHits = 0;
        blueTeamLastkicker = -1;
        redTeamLastkicker = -1;

        foreach (var i in bluePlayersList)
        {
            i.Rb.velocity = Vector3.zero;
            i.Rb.angularVelocity = Vector3.zero;
            i.player.transform.localPosition = i.StartingPos;
        }
        foreach (var i in redPlayersList)
        {
            i.Rb.velocity = Vector3.zero;
            i.Rb.angularVelocity = Vector3.zero;
            i.player.transform.localPosition = i.StartingPos;
        }

        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        if (blueTeamTurn)
        {
            //ball.transform.localPosition = new Vector3(2f, 10f, -4.5f);
            ball.transform.localPosition = new Vector3(Random.value * 8.5f - 4.25f, 30f, -Random.value * 3.5f - 5f);
            blueTeamTurn = false;
            lastHit = 0;
        }
        else
        {
            //ball.transform.localPosition = new Vector3(2f, 10f, 4.5f);
            ball.transform.localPosition = new Vector3(Random.value * 8.5f - 4.25f, 30f, Random.value * 3.5f + 5f);
            blueTeamTurn = true;
            lastHit = 1;
        }
    }
}
