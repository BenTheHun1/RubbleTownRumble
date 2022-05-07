using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{

    public SpawnManager sm;
    public GameObject cam;
    public Vector3 defaultPos;
    public Quaternion defaultRot;
    

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Spawns").GetComponent<SpawnManager>();
        defaultPos = gameObject.transform.position;
        defaultRot = gameObject.transform.rotation;
    }

    private void Update()
    {
        Debug.Log(Vector3.Distance(gameObject.transform.position, cam.transform.position));
        if (Vector3.Distance(gameObject.transform.position, cam.transform.position) < 0.2f && ((gameObject.transform.eulerAngles.x > 90f || gameObject.transform.eulerAngles.x < -90f) || (gameObject.transform.eulerAngles.z > 90f || gameObject.transform.eulerAngles.z < -90f)))
        {
            sm.startWave = true;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Sword"))
        {
           
        }
    }
}
