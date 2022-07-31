using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer square;
    public GameObject audioSource;

    static public float time;

    private void Awake()
    {

    }

    private void Start()
    {
        DOTween.To(() => square.color, x => square.color = x, new Color(0, 0, 0, 0), 4f).OnComplete(StartPlay).SetDelay(1f);
    }

    void StartPlay()
    {
        audioSource.SetActive(true);
        Player.Info.CanBeHandled = true;
        time = Time.time;
    }
}
