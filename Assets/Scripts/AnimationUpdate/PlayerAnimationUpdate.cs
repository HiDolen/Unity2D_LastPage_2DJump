using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 对玩家 Shader 的材质进行数值更新
// 与玩家和玩家Shader高度绑定
public class PlayerAnimationUpdate : MonoBehaviour
{
    Material material; // 附着在 GameObject 上的材质

    Vector3 currentPosition; // 先前的位置
    public static Vector2 velocity; //速度向量

    [Range(0, 1)] public float recoverRate = 0.5f; // 恢复速度。也受代码控制
    [Range(0, 1)] public float elasticity = 0.5f; // 弹性。弹性越大阻力越小

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material; // 获得材质
        currentPosition = transform.position; // 位置初值
        velocity = new Vector2(0, 1); // 速度向量初值
    }

    private void FixedUpdate()
    {

        velocity *= elasticity;
        Vector3 position = transform.position; // 当前位置
        Vector2 direction = new Vector2(currentPosition.x - position.x, currentPosition.y - position.y); // 新的施加力
        velocity += direction;
        if (Player.Info.isDashing == true)//对冲刺进行额外判断
        {
            velocity.x -= (int)Player.Info.currentDirection * 0.3f;
        }


        Vector3 endPosition = currentPosition - new Vector3(velocity.x, velocity.y, 0); // 新的目的地
        currentPosition = Vector3.Lerp(currentPosition, endPosition, recoverRate); // 持续更新，逼近现在的位置
    }

    private void Update()
    {
        Vector3 position = transform.position;
        Vector4 deltaPosition = new Vector4(currentPosition.x - position.x, currentPosition.y - position.y, position.z, 0);
        deltaPosition.x *= 0.5f; // 降低拉伸与压缩
        deltaPosition.y *= 0.4f; // 降低拉伸与压缩
        material.SetVector("_CurrentPosition", deltaPosition); // 持续传送，提供近期位置
        // material.SetVector("_CurrentPosition", new Vector4(0, 1, 0, 0)); // 持续传送，提供近期位置
    }
}
