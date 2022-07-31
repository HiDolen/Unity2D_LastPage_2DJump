using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using DG.Tweening;
using TMPro;

// 补全了方块，结局完成的判断与演出
public class FinishEndFill : MonoBehaviour
{
    public GameObject virtualCamera;
    public GameObject mainCamera;

    public GameObject finishEndCube;
    public Material playerMat;

    public GameObject lightToSelf;
    public GameObject lightToOthers;

    public GameObject stageBlocks;

    public TextMeshProUGUI text;

    bool positionOK = false;
    bool end = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.name == "Player")
            if (other.transform.position.x >= 399.54f)
            {
                positionOK = true;
            }
            else
            {
                positionOK = false;
            }
    }

    private void Update()
    {
        if (positionOK == true && Player.Info.isDark == true && end == false)
        {
            end = true;
            Player.Info.CanBeHandled = false;
            Player.Info.isDarkening = false;
            CameraMove();
        }
    }

    void CameraMove()
    {
        float totalTime = Time.time - GameManager.time;
        text.text = "Time spend: " + (int)totalTime + "s";

        virtualCamera.SetActive(false);
        finishEndCube.GetComponent<Renderer>().material = playerMat;

        stageBlocks.GetComponent<Renderer>().material.SetVector("_Tiling", new Vector4(0.1f, 0.1f, 0, 0));

        float endLightToSelfIntensity = 1.5f;
        float endLightToOthersIntensity = 0.72f;

        Sequence mainSequence = DOTween.Sequence();
        mainSequence.Append(Camera.main.DOOrthoSize(100, 13f).SetEase(Ease.InOutSine));
        Vector3 newPosition = Camera.main.transform.position + new Vector3(4.5f, 3.5f + 40f - 4f, 0);
        mainSequence.Join(Camera.main.transform.DOLocalMove(newPosition, 13f).SetEase(Ease.InOutSine));
        mainSequence.Join(Camera.main.transform.DOScale(new Vector3(10f, 10f, 1f), 13f).SetEase(Ease.InOutSine));

        lightToSelf.SetActive(true);
        Light2D endLightToSelf = lightToSelf.GetComponent<Light2D>();
        Tween endLightToSelfDo = DOTween.To(() => endLightToSelf.intensity, x => endLightToSelf.intensity = x, endLightToSelfIntensity, 8f).SetEase(Ease.InQuint);
        mainSequence.Append(endLightToSelfDo);

        lightToOthers.SetActive(true);
        Light2D endLightToOthers = lightToOthers.GetComponent<Light2D>();
        Tween endLightToOthersDo = DOTween.To(() => endLightToOthers.intensity, x => endLightToOthers.intensity = x, endLightToOthersIntensity, 7f).SetEase(Ease.InQuint);
        mainSequence.Join(endLightToOthersDo);

        mainSequence.Append(Camera.main.transform.DOBlendableMoveBy(new Vector3(0, 220f, 0), 10f).SetEase(Ease.InOutSine));

        mainSequence.Append(DOTween.To(() => text.color, x => text.color = x, new Color(1, 1, 1, 1), 1f));

        EventManager.Broadcast(EventTypee.BGMChange2);
        mainSequence.SetDelay(1f);
        mainSequence.PlayForward();
    }
}
