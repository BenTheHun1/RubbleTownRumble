using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject enemy;
    private GameObject player;
    private float lookSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirectionPlayer = (player.transform.position - transform.position).normalized;
        lookDirectionPlayer.y = 0;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(lookDirectionPlayer), Time.deltaTime * lookSpeed);
        transform.position = enemy.transform.position;
    }
}
