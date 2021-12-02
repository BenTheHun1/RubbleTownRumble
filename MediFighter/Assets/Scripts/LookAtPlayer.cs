using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    private GameObject player;
    private float lookSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        lookDirection.y = 0;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * lookSpeed);
    }
}
