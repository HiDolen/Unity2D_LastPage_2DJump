using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 对附着在伪装方块的材质进行数值更新
// 与相机和背景Shader高度绑定
// 主要作用是，玩家接近方块时方块逐渐透明
public class DisguiseBlockAnimationUpdate : MonoBehaviour
{
    Material material;

    private void Awake()
    {
        material = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        Vector3 position = Player.Info.transform.position;
        material.SetVector("_PlayerPosition", new Vector4(position.x, position.y, 0, 0));
    }
}
