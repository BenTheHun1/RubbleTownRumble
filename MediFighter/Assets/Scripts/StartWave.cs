using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{

    public SpawnManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Spawns").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Sword"))
        {
            sm.startWave = true;
        }
    }
}
