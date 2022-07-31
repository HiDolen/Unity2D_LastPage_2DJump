using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTouched : MonoBehaviour
{
    public EventTypee broadcastType = EventTypee.None;

    ParticleSystem particle;
    bool fadeOut = false;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (fadeOut == false)
        {
            if (other.transform.name == "AbsorbPoint")
            {
                fadeOut = true;
                particle.Stop();
                StartCoroutine("FadeOut");
                other.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                if (broadcastType != EventTypee.None)
                    EventManager.Broadcast(broadcastType);
            }
        }
    }

    IEnumerator FadeOut()
    {
        Player.Info.CanBeHandled = false;

        Material material = GetComponent<Renderer>().material;
        float value = 1;
        do
        {
            value -= 0.01f;
            material.SetFloat("_Disappear", value >= -1 ? value : -1);
            yield return new WaitForFixedUpdate();
        } while (value > -1 || particle.IsAlive() == true);
        Player.Info.CanBeHandled = true;
        gameObject.SetActive(false);
    }
}
