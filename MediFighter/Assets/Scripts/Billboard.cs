using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    public Transform cameraToLookAt;
    public Text healthDisplay;

    // Start is called before the first frame update
    void Start()
    {
        cameraToLookAt = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.root.GetComponent<EnemyAICharacterJoints>().Health < 0)
        {
            healthDisplay.text = "0";
        }
        else
        {
            healthDisplay.text = transform.root.GetComponent<EnemyAICharacterJoints>().Health.ToString();
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraToLookAt.forward);
    }


}