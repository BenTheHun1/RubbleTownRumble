using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    public int kickForce;
    public GameObject player;
    public AudioSource audioSorc;
    public AudioClip hitSound;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(player.transform.forward * kickForce, ForceMode.Impulse);
            audioSorc.PlayOneShot(hitSound);
        }
    }
}
