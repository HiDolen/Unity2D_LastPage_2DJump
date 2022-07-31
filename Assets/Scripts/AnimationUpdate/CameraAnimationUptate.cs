using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 对附着在相机上的材质进行数值更新
// 与相机和背景Shader高度绑定
public class CameraAnimationUptate : MonoBehaviour
{
    Material backgroundMat; // 背景的材质

    private void Awake()
    {
        backgroundMat = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        backgroundMat.SetVector("_CameraPosition", new Vector4(position.x, position.y, 0, 0));
    }
}
