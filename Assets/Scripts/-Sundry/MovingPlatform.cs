using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会移动的平台
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public Vector3 finishPos = Vector3.zero;//要移动到的位置
    public float speed = 0.5f;

    [HideInInspector] public Vector3 startPos;

    private float _trackPercent = 0;//离 finish 还有多远
    private int _direction = 1;//当前移动的方向。平台能来回移动

    private void Start()
    {
        transform.position = startPos;//起点为当前位置
        speed = speed + Random.Range(-0.08f, 0.08f);
    }

    private void FixedUpdate()
    {
        _trackPercent += _direction * speed * Time.deltaTime;//更新当前的移动进度
        float x = (finishPos.x - startPos.x) * _trackPercent + startPos.x;
        float y = (finishPos.y - startPos.y) * _trackPercent + startPos.y;
        transform.position = new Vector3(x, y, startPos.z);

        if ((_direction == 1 && _trackPercent > .9f) || (_direction == -1 && _trackPercent < .1f)) _direction *= -1;//调转方向
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPos);
    }
}
