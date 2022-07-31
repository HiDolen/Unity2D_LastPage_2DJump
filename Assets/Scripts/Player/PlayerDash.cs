using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashForce;
    public int maxDashCount = 1; // 落地之前最大可冲刺次数
    int dashCount; // 剩余冲刺次数

    private Rigidbody2D _body;
    private BoxCollider2D _box;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();

        dashCount = maxDashCount;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Dash") && Player.Info.CanBeHandled && dashCount > 0)
        {
            --dashCount;
            Player.Info.CanBeHandled = false;
            StartCoroutine("Dashing");
        }
        if (Player.Info.grounded == true)
        {
            dashCount = maxDashCount;
        }
    }

    /// <summary>
    /// 冲刺
    /// </summary>
    IEnumerator Dashing()
    {
        Player.Info.isDashing = true;
        float movement = (int)Player.Info.currentDirection * dashForce; // 运动方向与力度

        Vector2 velocity = PlayerAnimationUpdate.velocity;

        float durationTime = 0.3f;
        while (durationTime > 0)
        {
            if (Player.Info.isDashing == false) yield break; // 如果有外来之力阻止，则直接停止冲刺
            
            _body.velocity = new Vector2(movement, 0);
            durationTime -= Time.fixedDeltaTime;

            if (durationTime < float.Epsilon) break;


            yield return new WaitForFixedUpdate();
        }
        Player.Info.CanBeHandled = true;
        Player.Info.isDashing = false;
    }
}
