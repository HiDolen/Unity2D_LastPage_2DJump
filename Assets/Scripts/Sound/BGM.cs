using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class BGM : MonoBehaviour
{
    public AudioClip BGM2;

    AudioSource audioSource;

    private void Awake()
    {
        EventManager.AddListener(EventTypee.BGMChange1, BGMChange1);
        EventManager.AddListener(EventTypee.BGMChange2, BGMChange2);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }



    void BGMChange1()
    {
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, 8f);
    }

    void BGMChange2()
    {
        audioSource.Stop();
        audioSource.clip = BGM2;
        audioSource.timeSamples = 0;
        audioSource.loop = false;
        audioSource.Play();
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0.5f, 8f);
    }
}
