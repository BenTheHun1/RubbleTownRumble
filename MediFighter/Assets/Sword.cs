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

    // Start is called before the first frame update
    void Start()
    {
        /*var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);
        rCon = inputDevices[0];
        rCon.TryGetFeatureValue()*/
        leftHand = left;
        rightHand = right;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude.ToString("N0") + " " + gameObject.GetComponent<Rigidbody>().velocity.magnitude.ToString("N0"));
        Debug.Log(right.transform.position);
    }
}
