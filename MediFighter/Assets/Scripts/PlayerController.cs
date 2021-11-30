using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Variables
    public CharacterController controller;
    public CameraController cc;
    public Animator animSword;

    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float speed = 4f;
    private float gravity = -9.81f * 6;
    private float jumpHeight = 1f;

    public Vector3 velocity;
    public bool isOnGround;

    public RaycastHit ray;

    float desiredHeight;
    bool canKick;
    public GameObject foot;

    public GameObject buyableItem;
    private Text shopInfo;
    public HealthSystem hs;

    void Start()
    {
        shopInfo = GameObject.Find("ShopInfo").GetComponent<Text>();
        canKick = true;
    }

    void Update()
    {
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Checks if you are on a Ground layer object

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f; //Stops y velocity from infinitely decreasing
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Quick Reload of Scene
        }

        //Crouching System
        if (Input.GetKey(KeyCode.LeftControl))
        {
            desiredHeight = 1f;
        }
        else
        {
            desiredHeight = 2f;
        }
        controller.height = Mathf.Lerp(controller.height, desiredHeight, 0.1f);

        //Sword Animatons
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

        //Kicking
        if (Input.GetKeyDown(KeyCode.F) && canKick)
        {
            canKick = false;
            foot.SetActive(true);
            StartCoroutine(FootDissapear());
        }

        //Bringing up info on buyable item you're looking at
        if (ray.transform != null)
        {
            if (ray.transform.gameObject.CompareTag("Item"))
            {
                buyableItem = ray.transform.gameObject;
                if (buyableItem.name == "BuyHelm")
                {
                    shopInfo.text = "A dwarven helmet for extra defense.\n10 Beards\n\nBuy with [E]"; // \n = New line
                }
            }
            else
            {
                buyableItem = null;
                shopInfo.text = "";
            }
        }

        //Buy buyable item you're looking at
        if (Input.GetKeyDown(KeyCode.E) && buyableItem != null)
        {
            if (buyableItem.name == "BuyHelm" && hs.beards >= 10)
            {
                hs.beards -= 10;
                //defense ++
                buyableItem.SetActive(false);
            }
        }
    }

    //Hide foot after done playing anim
    IEnumerator FootDissapear()
    {
        yield return new WaitForSeconds(0.5f); //Change time based on anim speed 1.5 speed = 0.5 seconds
        cc.inControl = true;
        foot.SetActive(false);
        canKick = true;
    }
}
