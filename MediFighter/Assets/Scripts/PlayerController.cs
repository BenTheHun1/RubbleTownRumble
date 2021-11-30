using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public CameraController cc;
    public Animator animSword;

    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;


    public GameObject foot;
    private float footHeightDefault; // Same for L & R

    private float speed = 4f;
    private float gravity = -9.81f * 6;
    private float jumpHeight = 1f;

    public Vector3 velocity;
    public bool isOnGround;

    public RaycastHit ray;

    float desiredHeight;
    bool canKick;

    // Start is called before the first frame update
    void Start()
    {
        canKick = true;
    }

    // Update is called once per frame
    void Update()
    {
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }

        controller.height = Mathf.Lerp(controller.height, desiredHeight, 0.1f);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            desiredHeight = 1f;
        }
        else
        {
            desiredHeight = 2f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (animSword.GetCurrentAnimatorStateInfo(0).IsName("Swipe"))
            {
                animSword.SetTrigger("Swing2");
            }
            else
            {
                animSword.SetTrigger("Swing");
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && canKick)
        {
            //cc.inControl = false;
            canKick = false;
            foot.SetActive(true);
            StartCoroutine(FootDissapear());
        }

        IEnumerator FootDissapear()
        {
            yield return new WaitForSeconds(0.5f);
            cc.inControl = true;
            foot.SetActive(false);
            canKick = true;
        }
    }
}
