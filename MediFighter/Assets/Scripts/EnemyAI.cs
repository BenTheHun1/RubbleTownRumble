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
    private bool isDamaged;
    private Renderer rend;
    //private Color color = Color.red;
    public Material damaged;
    public Material normal;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        qTo = transform.rotation;
        normal = gameObject.GetComponent<Renderer>().material;
        rend = gameObject.GetComponent<Renderer>();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword") && !isDamaged)
        {
            isDamaged = true;
            Debug.Log("Damaged");
            StartCoroutine(Damage());
        }
    }

    IEnumerator Damage()
    {
        //color = new Color32(color.r - 147, color.g, color.b, 0);
        rend.material = damaged;
        //rend.material.color = color;
        yield return new WaitForSeconds(2f);
        //color = new Color32(color.r + 147, color.g, color.b, 0);
        //rend.material.color = color;
        rend.material = normal;
        isDamaged = false;
    }
}
