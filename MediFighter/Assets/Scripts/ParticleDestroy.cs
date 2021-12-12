using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public ParticleSystem particlePreFab;
    // Start is called before the first frame update
    void Start()
    {
        particlePreFab = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particlePreFab.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
