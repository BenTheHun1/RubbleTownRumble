using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DepositBeards : MonoBehaviour
{
    public Transform dropLocation;
    private HealthSystem hs;
    private bool isDepositing;
    // Start is called before the first frame update
    void Start()
    {
        hs = GameObject.Find("Player").GetComponent<HealthSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beard") && !isDepositing)
        {
            StartCoroutine(Deposit(other));
        }
    }

    IEnumerator Deposit(Collider other)
    {
        isDepositing = true;
        other.transform.position = dropLocation.transform.position;
        other.transform.Rotate(0, 180, 60, Space.World);
        Destroy(other.GetComponent<XRGrabInteractable>());
        Destroy(other.GetComponent<Cloth>());
        Destroy(other.GetComponent<Rigidbody>());
        other.GetComponent<MeshCollider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        other.gameObject.AddComponent<Rigidbody>();
        yield return new WaitForSeconds(0.4f);
        Destroy(other);
        hs.beards++;
        isDepositing = false;
    }
}
