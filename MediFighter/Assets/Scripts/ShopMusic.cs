using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMusic : MonoBehaviour
{
    private AudioSource shopMusic;
    public SpawnManager sm;

    // Start is called before the first frame update
    void Start()
    {
        shopMusic = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !shopMusic.isPlaying && sm.enemiesLeft + sm.enemiesToSpawn <= 0)
        {
            shopMusic.Play();
        }
    }
}
