using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMusicStop : MonoBehaviour
{
    public AudioSource shopMusic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            shopMusic.Stop();
        }
    }
}
