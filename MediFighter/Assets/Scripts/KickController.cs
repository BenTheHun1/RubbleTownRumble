using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    public int kickForce;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z) * kickForce, ForceMode.Impulse);
        }
    }
}
