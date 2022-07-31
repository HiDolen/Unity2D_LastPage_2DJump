using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 4.5f;
    public float firstJumpForce = 200.0f;
    public float secondJumpForce = 200.0f;
    public float jumpForce = 12.0f;

    public int maxJumpCount = 1; // 落地之前最大可跳跃次数
    int jumpCount; // 剩余跳跃次数

    private Rigidbody2D _body;
    private BoxCollider2D _box;

    public static class Info
    {
        public static Transform transform; // 玩家的 Transform

        public static bool isJumping = false; // 是否正在跳跃
        public static bool isDashing = false; // 是否正在冲刺
        public static bool isDarkening = false; // 是否正在变暗
        public static bool isDark = false; // 是否处于黑暗状态
        public static Direction currentDirection = Direction.left; // 当前玩家的朝向
        public static bool grounded = false; // 是否接触地面

        private static bool canBeHandled = false;// 玩家此时是否可以操作
        public static bool CanBeHandled
        {
            get => canBeHandled; set
            {
                canBeHandled = value;
                isJumping = false;
                isDashing = false;
            }
        }
    }

    private void Awake()
    {
        Info.transform = gameObject.transform;
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();

        jumpCount = maxJumpCount - 1;
    }

    bool isJumpPressed = false;
    float deltaX;


    private void Update()
    {
        deltaX = Input.GetAxis("Horizontal") * speed;
        if (deltaX > 0)
            Info.currentDirection = Direction.right;
        else if (deltaX < 0)
            Info.currentDirection = Direction.left;

        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;
        Vector2 corner1 = new Vector2(max.x - .01f, min.y + .1f);
        Vector2 corner2 = new Vector2(min.x + .01f, min.y - .21f);
        Collider2D[] hits = Physics2D.OverlapAreaAll(corner1, corner2); // 是否落地

        // if (hits.Length == 3) Info.grounded = true; else Info.grounded = false;

        foreach (Collider2D c in hits)
        {
            if (c.transform.name == "Stage")
            {
                Info.grounded = true;
                break;
            }
            Info.grounded = false;
        }

        if (Info.grounded == true)
        {
            jumpCount = maxJumpCount - 1; // 恢复跳跃次数
        }

        if (Input.GetButtonDown("Jump"))//开始跳跃
        {
            if ((Info.isJumping == false && Info.CanBeHandled == true && jumpCount > 0) || Info.grounded == true)
            {
                jumpCount -= 1;
                if (jumpCount == maxJumpCount - 1)
                    _body.AddForce(Vector2.up * firstJumpForce, ForceMode2D.Impulse);
                else
                {
                    _body.velocity = new Vector2(_body.velocity.x, 0);
                    _body.AddForce(Vector2.up * secondJumpForce, ForceMode2D.Impulse);
                }
                StartCoroutine("Jumping");
                Info.isJumping = true;
            }
        }
        if (Input.GetButtonUp("Jump"))//如果没有按下空格了，就取消施加力
        {
            Info.isJumping = false;
        }
        if (Info.isJumping == false)
        {
            StopCoroutine("Jumping");
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);//保留预先存在的垂直移动
        if (Info.CanBeHandled == true)
        {
            _body.velocity = movement;//水平移动
        }
    }

    /// <summary>
    /// 用来进行灵活跳跃
    /// </summary>
    IEnumerator Jumping()
    {
        float durationInSky = 1;
        while (durationInSky > 0)
        {
            _body.AddForce(Vector2.up * jumpForce * durationInSky, ForceMode2D.Impulse);
            durationInSky = Mathf.Lerp(durationInSky, 0, 0.095f);

            if (durationInSky < float.Epsilon) break;

            yield return new WaitForFixedUpdate();
        }
        Debug.Log(durationInSky);
    }
}
