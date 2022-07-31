using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBroadCast : MonoBehaviour
{
    Player player;
    PlayerDash playerDash;

    private void Awake()
    {
        EventManager.AddListener(EventTypee.GetDoubleJump, GetDoubleJump);
        EventManager.AddListener(EventTypee.GetDash, GetDash);
    }

    void Start()
    {
        player = GetComponent<Player>();
        playerDash = GetComponent<PlayerDash>();
    }

    /// <summary>
    /// 作弊相关
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            player.maxJumpCount++;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            player.maxJumpCount--;
        }
    }

    void GetDoubleJump()
    {
        player.maxJumpCount += 1;
        Debug.Log("获得二段跳");
    }

    void GetDash()
    {
        playerDash.maxDashCount = 1;
        Debug.Log("获得冲刺");
    }
}
