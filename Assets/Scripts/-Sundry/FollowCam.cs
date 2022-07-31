using UnityEngine;
using System.Collections;

// 相机跟随
public class FollowCam : MonoBehaviour
{
    public Transform target;//指向玩家
    public float smoothTime = 0.2f;

    private Vector3 _velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}