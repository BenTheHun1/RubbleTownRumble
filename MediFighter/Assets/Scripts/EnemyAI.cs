using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float movementSpeed;
    public bool isDamaged;
    public GameObject enemyObject;
    public Renderer rend;
    private Quaternion qTo;
    private Rigidbody rb;
    private ConfigurableJoint cj;
    private GameObject player;
    private GameObject spawnManager;
    //private float detectionradius = 10f;
    private float stoppingradius = 1.1f;
    private float lookSpeed = 2.0f;
    private Color32 color;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        spawnManager = GameObject.Find("Spawns");
        qTo = transform.rotation;
        rb = gameObject.GetComponent<Rigidbody>();
        cj = gameObject.GetComponent<ConfigurableJoint>();
        color = new Color32(225, 0, 0, 0);
        rend.sharedMaterial.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(player.transform.position, transform.position) <= detectionradius) //&& !isDamaged)
        //{

            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            lookDirection.y = 0;
            qTo = Quaternion.LookRotation(lookDirection);//Vector3.right, player.transform.position - transform.position);
            if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, qTo, Time.deltaTime * lookSpeed);
        }
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword") && !isDamaged)
        {
            isDamaged = true;
            //Debug.Log("Damaged");
            Kill();
            //StartCoroutine(Damage());
        }
    }

    void Kill()
    {
        spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(enemyObject);
        color = new Color32(108, 0, 0, 0);
        rend.material.color = color;
        rb.freezeRotation = false;
        Destroy(cj);
        this.enabled = false;
        StartCoroutine(Remove());
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(6f);
        Destroy(enemyObject);
    }

    /*IEnumerator Damage()
    {
        color = new Color32(108, 0, 0, 0);
        rend.material.color = color;
        yield return new WaitForSeconds(2f);
        color = new Color32(225, 0, 0, 0);
        rend.material.color = color;
        isDamaged = false;
    }*/

}
