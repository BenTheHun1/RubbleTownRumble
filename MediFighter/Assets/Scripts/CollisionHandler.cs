using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public EnemyAICharacterJoints enemyAI;
    private PlayerController playerController;
    private bool isKICKED;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = transform.root.GetComponent<EnemyAICharacterJoints>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            /*if (playerController.animSword.GetCurrentAnimatorStateInfo(0).IsName("Swipe") || playerController.animSword.GetCurrentAnimatorStateInfo(0).IsName("Swipe"))
            {
                enemyAI.Hit();
            }*/

            if (other.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                enemyAI.Hit();
            }
            //Debug.Log(other.GetComponent<Rigidbody>().velocity.magnitude);

        }
        else
        {
            if (other.gameObject.CompareTag("Boot") && !enemyAI.isRagdoll && !isKICKED)
            {
                isKICKED = true;
                enemyAI.isKicked = true;
                enemyAI.Ragdoll();
                StartCoroutine(cooldown());
            }
        }

        if (other.gameObject.CompareTag("Boot") && enemyAI.isRagdoll && enemyAI.Health <= 0)
        {
            enemyAI.skipDeathStruggle = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        if (other.transform.root != null && other.transform.root != transform.root && other.gameObject.CompareTag("Enemy") && enemyAI.isRagdoll && enemyAI.isKicked == true)
        {
            if (other.transform.root.TryGetComponent(out EnemyAICharacterJoints AIenemy))
            {
                AIenemy.isKicked = true;
                AIenemy.Ragdoll();
            }
        }
    }
    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isKICKED = false;
    }
}
