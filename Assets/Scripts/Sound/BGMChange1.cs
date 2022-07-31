using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChange1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.name=="Player")
            EventManager.Broadcast(EventTypee.BGMChange1);
    }
}
