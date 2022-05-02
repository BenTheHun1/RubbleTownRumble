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

    private Vector3 d1, d2;
    public Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        /*var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);
        rCon = inputDevices[0];
        rCon.TryGetFeatureValue()*/
        leftHand = left;
        rightHand = right;
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude.ToString("N0") + " " + gameObject.GetComponent<Rigidbody>().velocity.magnitude.ToString("N0"));
        //Debug.Log(right.transform.position);
        d1 = gameObject.transform.position;
    }

    private void LateUpdate()
    {
        d2 = gameObject.transform.position;
        speed = (d2 - d1) / Time.deltaTime;
        //Debug.Log(speed);
    }
}
