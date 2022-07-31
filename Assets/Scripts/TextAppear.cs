using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TextAppear : MonoBehaviour
{
    TextMeshPro textMeshPro;

    public float distanceThreshold;
    public float distanceMaxBrightness;
    float distanceLength_reciprocal; // 中间距离的倒数

    public GameObject dieWithMe = null;
    bool fadeOut = false;

    private void Awake()
    {
        distanceLength_reciprocal = 1 / (distanceThreshold - distanceMaxBrightness);
    }

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        if (fadeOut == false)
        {
            float distance = Vector2.Distance((Vector2)Player.Info.transform.position, (Vector2)transform.position);

            Color color = textMeshPro.color;
            if (distance >= distanceThreshold)
                color.a = 0;
            else
            {
                color.a = Mathf.Lerp(1, 0, Mathf.Clamp(distance - distanceMaxBrightness, 0, distance) * distanceLength_reciprocal);
            }
            textMeshPro.color = color;

            if (dieWithMe != null && dieWithMe.activeSelf == false)
            {
                fadeOut = true;
                StartCoroutine("FadeOut");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceMaxBrightness);
        Gizmos.DrawWireSphere(transform.position, distanceThreshold);
    }

    IEnumerator FadeOut()
    {
        Color color = textMeshPro.color;
        do
        {
            color.a -= 0.02f;
            textMeshPro.color = color;
            yield return new WaitForFixedUpdate();
        } while (color.a > 0);
    }
}
