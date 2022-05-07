using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : MonoBehaviour
{
    private InputDevice rCon;
    public static XRBaseController leftHand, rightHand;
    public ActionBasedController left, right;
    private XRGrabInteractable grab;

    private Vector3 d1, d2;
    public Vector3 speed;

    public bool isSword;
    public Transform rest;

    private InteractionLayerMask nothing = 0;
    private InteractionLayerMask everything = ~0;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        /*var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);
        rCon = inputDevices[0];
        rCon.TryGetFeatureValue()*/
        leftHand = left;
        rightHand = right;
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 50f;
        grab = gameObject.GetComponent<XRGrabInteractable>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.useGravity == false && isSword)
        {
            gameObject.transform.localPosition = Vector3.zero;

        }
    }

    public void grabbed()
    {
        //ab.interactionLayers = nothing;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public void letgo()
    {
        //ab.interactionLayers = everything;
        gameObject.transform.parent = rest;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localEulerAngles = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }
}
