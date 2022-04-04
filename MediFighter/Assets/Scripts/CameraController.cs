using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public float mouseSensitivity;
    public Transform playerBody;
    private float xRotation = 0f;
    private RaycastHit hit;


    public bool inControl;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            playerBody.gameObject.GetComponent<PlayerController>().ray = hit;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (inControl)
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
        }
        else
        {
            float angle = Mathf.LerpAngle(transform.eulerAngles.x, 20f, 0.1f);
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
            if (Mathf.Approximately(transform.localRotation.eulerAngles.x, 20f))
            {
                xRotation = angle;
                inControl = true;
            }
        }
        playerBody.Rotate(Vector3.up * mouseX);*/
        

    }
}
