using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimb : MonoBehaviour
{
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
        this.m_ConfigurableJoint = this.GetComponent<ConfigurableJoint>();
        this.targetInitialRotation = this.targetLimb.transform.localRotation;
    }

    void Update()
    {

    }

    private void FixedUpdate() 
    {
        this.m_ConfigurableJoint.targetRotation = copyRotation();
    }

    private Quaternion copyRotation() {
        return Quaternion.Inverse(this.targetLimb.localRotation) * this.targetInitialRotation;
    }
}
