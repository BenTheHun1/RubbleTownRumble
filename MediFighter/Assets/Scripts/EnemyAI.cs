using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float movementSpeed;
    private Quaternion qTo;
    private GameObject player;
    private float detectionradius = 10f;
    private float stoppingradius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        qTo = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= detectionradius)
        {

            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            lookDirection.y = 0;
            qTo = Quaternion.LookRotation(lookDirection);
            if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
            }
        }
    }
}
