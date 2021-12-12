using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMusic : MonoBehaviour
{
    private AudioSource shopMusic;

    // Start is called before the first frame update
    void Start()
    {
        shopMusic = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !shopMusic.isPlaying)
        {
            shopMusic.Play();
        }
    }
}
