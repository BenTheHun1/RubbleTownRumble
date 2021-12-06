using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimb : MonoBehaviour
{
    public EnemyAIConfigurableJoints enemyAI;
    public GameObject player;
    public GameObject mimicker;
    public Transform[] MimickerLimbs;
    [SerializeField] private Transform targetLimb;
    [SerializeField] private ConfigurableJoint m_ConfigurableJoint;

    Quaternion targetInitialRotation;
    void Awake()
    {
        mimicker = GameObject.Find("Mimic");
        MimickerLimbs = mimicker.GetComponentsInChildren<Transform>();
        
        foreach (Transform limb in MimickerLimbs)
        {
            if (gameObject.name == limb.name)
            {
                this.targetLimb = limb;
            }
        }
    }
    void Start()
    {
        player = GameObject.Find("Player");
        enemyAI = transform.root.GetComponent<EnemyAIConfigurableJoints>();
        m_ConfigurableJoint = gameObject.GetComponent<ConfigurableJoint>();
        targetInitialRotation = targetLimb.transform.localRotation;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (enemyAI.isWalking == true && enemyAI.isAttacking == false)
        {
            m_ConfigurableJoint.targetRotation = copyRotation();
        }
        else
        {
            if (enemyAI.isAttacking == true && Vector3.Distance(player.transform.position, enemyAI.transform.position) < 2f)
            {
                m_ConfigurableJoint.targetRotation = copyRotation();
            }
        }
    }

    private Quaternion copyRotation() {
        return Quaternion.Inverse(targetLimb.localRotation) * targetInitialRotation;
    }
}
