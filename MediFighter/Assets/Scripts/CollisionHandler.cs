using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollisionHandler : MonoBehaviour
{
    public EnemyAICharacterJoints enemyAI;
    //private PlayerController playerController;
    private bool isKICKED;

    // VR Controller Haptics

    public static XRBaseController leftHand, rightHand;
    public ActionBasedController left, right;
    public float defaultAmplitude = 0.2f;
    public float defaultDuration = 0.1f;


    [ContextMenu("Send Haptics")]
    public void SendHaptics()
    {
        leftHand.SendHapticImpulse(defaultAmplitude, defaultDuration);
        rightHand.SendHapticImpulse(defaultAmplitude, defaultDuration);
    }

    public static void SendHaptics(bool isLeftController, float amplitude, float duration)
    {
        if (isLeftController)
        {
            leftHand.SendHapticImpulse(amplitude, duration);
        }
        else
        {
            rightHand.SendHapticImpulse(amplitude, duration);
        }
    }

    public static void SendHaptics(XRBaseController controller, float amplitude, float duration)
    {
        controller.SendHapticImpulse(amplitude, duration);
    }

    // Start is called before the first frame update
    void Start()
    {
        leftHand = left;
        rightHand = right;
        enemyAI = transform.root.GetComponent<EnemyAICharacterJoints>();
        //playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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

            if (other.GetComponent<Rigidbody>().angularVelocity.magnitude > 3f)
            {
                //Debug.Log(other.GetComponent<Rigidbody>().angularVelocity.magnitude);
                SendHaptics(false, 0.2f, 0.5f);
                enemyAI.Hit();
            }

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
