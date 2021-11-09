using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    public bool kicking;
    public Transform kickPosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Kick()
    {
        kicking = true;
        //gameObject.GetComponent<Rigidbody>().AddForce(kickPosition.position, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameObject.transform.position == kickPosition.position)
        //{
        //    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}

        if (kicking) {
            gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, kickPosition.position.x, 0.03f), Mathf.Lerp(gameObject.transform.position.y, kickPosition.position.y, 0.03f), Mathf.Lerp(gameObject.transform.position.z, kickPosition.position.z, 0.03f));
            float angle = Mathf.LerpAngle(transform.eulerAngles.x, kickPosition.eulerAngles.x, 0.03f);
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }
    }
}
