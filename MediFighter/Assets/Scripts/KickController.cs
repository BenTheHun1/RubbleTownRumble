using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    public int kickForce;

    public AudioSource audioSorc;
    public AudioClip hitSound;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(gameObject.transform.position.x, 0, 0) * kickForce, ForceMode.Impulse);
            audioSorc.PlayOneShot(hitSound);
        }
    }
}
