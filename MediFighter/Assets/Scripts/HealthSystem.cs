using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public bool isDamaged;
    public bool god;
    public int playerHealth;
    public int maxHealth;
    public Image disHealth;
    public int AttackAmount;
    public int beards;
    private GameObject mimic;
    private RawImage hurtDisplay;
    private Image gameOverText;
    private GameObject player;
    private GameObject cam;
    private TextMeshProUGUI disBeards;

    public AudioClip hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        if (god)
        {
            AttackAmount = 1;
        }
        else
        {
            AttackAmount = 10;
        }
        maxHealth = 5;
        playerHealth = maxHealth;
        mimic = GameObject.Find("Mimic");
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
        //disHealth = GameObject.Find("HP").GetComponent<Image>();
        //disBeards = GameObject.Find("BeardAmount").GetComponent<TextMeshProUGUI>();
        hurtDisplay = GameObject.Find("HurtVR").GetComponent<RawImage>();
        gameOverText = GameObject.Find("GameOverVR").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //disBeards.text = beards.ToString() + " x";
    }

    public void DamagePlayer()
    {
        isDamaged = true;
        if (playerHealth > 0 && !god)
        {
            playerHealth -= 4;
            hurtDisplay.gameObject.SetActive(true);
            disHealth.fillAmount = (float)playerHealth / (float)maxHealth;
        }
        if (playerHealth <= 0)
        {
            StartCoroutine(GameOver());
        }
        StartCoroutine(Damage());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyAxe") && !isDamaged)
        {
            object isEnemyRagdoll = true;
            object isActiveRagdoll = true;
            if (other.transform.root.GetComponent<EnemyAICharacterJoints>())
            {
                isEnemyRagdoll = other.transform.root.GetComponent<EnemyAICharacterJoints>().isRagdoll;
                isActiveRagdoll = other.transform.root.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack");
            }
            else
            {
                if (other.transform.root.GetComponent<EnemyAIConfigurableJoints>())
                {
                    isEnemyRagdoll = other.transform.root.GetComponent<EnemyAIConfigurableJoints>().isRagdoll;
                    isActiveRagdoll = mimic.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack");
                }
            }

                
            if (!(bool)isEnemyRagdoll && (bool)isActiveRagdoll)
            {
                isDamaged = true;
                if (playerHealth > 0 && !god)
                {
                    playerHealth -= 1;
                    hurtDisplay.enabled = true;
                    gameObject.GetComponent<AudioSource>().PlayOneShot(hurtSound);
                    //disHealth.fillAmount = (float)playerHealth / (float)maxHealth;
                }
                if (playerHealth <= 0)
                {
                    StartCoroutine(GameOver());
                }
                StartCoroutine(Damage());
            }    
        }
    }

    IEnumerator GameOver()
    {
        gameOverText.enabled = true;
        //player.GetComponent<PlayerController>().enabled = false;
        //cam.GetComponent<CameraController>().enabled = false;
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.5f);
        hurtDisplay.enabled = false;
        yield return new WaitForSeconds(0.5f);
        isDamaged = false;
    }

}
