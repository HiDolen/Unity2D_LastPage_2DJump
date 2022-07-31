using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class PlayerDark : MonoBehaviour
{
    public bool canDarken = false;

    Light2D[] lightsToSelf;
    Light2D[] lightsToOthers;

    float[] lightsToSelfOriginal;
    float[] lightsToOthersOriginal;

    private void Awake()
    {
        EventManager.AddListener(EventTypee.GetDark, GetDark);
    }

    void GetDark()
    {
        canDarken = true;
    }

    private void Start()
    {
        lightsToSelf = GetComponents<Light2D>();
        lightsToOthers = GetComponentsInChildren<Light2D>();
        lightsToSelfOriginal = new float[lightsToSelf.Length];
        lightsToOthersOriginal = new float[lightsToOthers.Length];
        for (int i = 0; i < lightsToSelf.Length; i++)
            lightsToSelfOriginal[i] = lightsToSelf[i].intensity;
        for (int i = 0; i < lightsToOthers.Length; i++)
            lightsToOthersOriginal[i] = lightsToOthers[i].intensity;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Darken") && Player.Info.CanBeHandled && canDarken)
        {
            Player.Info.CanBeHandled = false;
            Player.Info.isDarkening = true;
            StopCoroutine("DarkenRecover");
            StartCoroutine("Darken");
        }
        if (Input.GetButtonUp("Darken") && Player.Info.isDarkening == true && canDarken)
        {
            Player.Info.CanBeHandled = true;
            Player.Info.isDarkening = false;
            StopCoroutine("Darken");
            StartCoroutine("DarkenRecover");
        }
    }

    IEnumerator Darken()
    {
        bool allOK;
        do
        {
            allOK = true;
            foreach (Light2D l in lightsToSelf)
            {
                if (l.intensity > 0)
                {
                    l.intensity -= 0.01f;
                    allOK = false;
                }
                else
                    continue;
            }
            foreach (Light2D l in lightsToOthers)
            {
                if (l.intensity > 0)
                {
                    l.intensity -= 0.01f;
                    allOK = false;
                }
                else
                    continue;
            }
            yield return new WaitForFixedUpdate();
        } while (allOK == false);
        // 准确对准
        for (int i = 0; i < lightsToSelf.Length; i++)
            lightsToSelf[i].intensity = 0;
        for (int i = 0; i < lightsToOthers.Length; i++)
            lightsToOthers[i].intensity = 0;
        Player.Info.isDark = true;
    }

    IEnumerator DarkenRecover()
    {
        Player.Info.isDark = false;
        bool allOK;
        do
        {
            allOK = true;
            for (int i = 0; i < lightsToSelf.Length; i++)
            {
                if (lightsToSelf[i].intensity >= lightsToSelfOriginal[i])
                    continue;
                allOK = false;
                lightsToSelf[i].intensity += 0.05f;
            }
            for (int i = 0; i < lightsToOthers.Length; i++)
            {
                if (lightsToOthers[i].intensity >= lightsToOthersOriginal[i])
                    continue;
                allOK = false;
                lightsToOthers[i].intensity += 0.05f;
            }
            yield return new WaitForFixedUpdate();
        } while (allOK == false);
        // 准确对准
        for (int i = 0; i < lightsToSelf.Length; i++)
            lightsToSelf[i].intensity = lightsToSelfOriginal[i];
        for (int i = 0; i < lightsToOthers.Length; i++)
            lightsToOthers[i].intensity = lightsToOthersOriginal[i];
    }
}
