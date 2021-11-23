using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    public bool kicking;
    public Transform kickPosition;
    public Transform defaultPosition;


    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = gameObject.transform;
    }

    public void Kick()
    {
        kicking = true;
        StartCoroutine("Kicking");
        //gameObject.GetComponent<Rigidbody>().AddForce(kickPosition.position, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator Kicking()
    {
        while (!Mathf.Approximately(kickPosition.position.x, gameObject.transform.position.x))
        {
            gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, kickPosition.position.x, 0.03f), Mathf.Lerp(gameObject.transform.position.y, kickPosition.position.y, 0.03f), Mathf.Lerp(gameObject.transform.position.z, kickPosition.position.z, 0.03f));
            float angle = Mathf.LerpAngle(transform.eulerAngles.x, kickPosition.eulerAngles.x, 0.03f);
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }
        yield return new WaitForSeconds(1f);
        while (!Mathf.Approximately(defaultPosition.position.x, gameObject.transform.position.x))
        {
            gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, defaultPosition.position.x, 0.03f), Mathf.Lerp(gameObject.transform.position.y, defaultPosition.position.y, 0.03f), Mathf.Lerp(gameObject.transform.position.z, defaultPosition.position.z, 0.03f));
            float angle = Mathf.LerpAngle(transform.eulerAngles.x, defaultPosition.eulerAngles.x, 0.03f);
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }
        yield return new WaitForSeconds(1);
        kicking = false;
    }
}
