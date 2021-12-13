using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    //public EnemyAICharacterJoints enemyAI;
    public EnemyAICharacterJoints enemyAI;
    private CapsuleCollider playerSword;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = transform.root.GetComponent<EnemyAICharacterJoints>();
        playerSword = GameObject.Find("Sword").GetComponent<CapsuleCollider>();
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
        if (other.gameObject.CompareTag("Sword") && !enemyAI.invincible)
        {
            if (playerController.animSword.GetCurrentAnimatorStateInfo(0).IsName("Swipe") || playerController.animSword.GetCurrentAnimatorStateInfo(0).IsName("Swipe"))
            {
                playerSword.enabled = false;
                enemyAI.Slashed();
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Boot") && !enemyAI.isRagdoll)
            {
                enemyAI.isKicked = true;
                enemyAI.Ragdoll();
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
            //other.transform.root.GetComponent<EnemyAICharacterJoints>().isKicked = true;
            //other.transform.root.GetComponent<EnemyAICharacterJoints>().Ragdoll();
        }
    }
}
