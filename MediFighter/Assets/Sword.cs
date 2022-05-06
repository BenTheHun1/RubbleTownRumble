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

    public void grabbed()
    {
        //ab.interactionLayers = nothing;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
    }

    public void letgo()
    {
        //ab.interactionLayers = everything;
        gameObject.transform.parent = rest;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }
}
