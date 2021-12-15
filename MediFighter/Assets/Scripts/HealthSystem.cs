using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public bool isDamaged;
    public int playerHealth;
    public int maxHealth;
    public Image disHealth;
    public int AttackAmount;
    public int beards;
    private GameObject mimic;
    private RawImage hurtDisplay;
    private GameObject gameOverText;
    private GameObject player;
    private GameObject cam;
    private TextMeshProUGUI disBeards;

    // Start is called before the first frame update
    void Start()
    {
        AttackAmount = 1;
        maxHealth = 5;
        playerHealth = maxHealth;
        mimic = GameObject.Find("Mimic");
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
        disHealth = GameObject.Find("HP").GetComponent<Image>();
        disBeards = GameObject.Find("BeardAmount").GetComponent<TextMeshProUGUI>();
        hurtDisplay = GameObject.Find("Hurt").GetComponent<RawImage>();
        hurtDisplay.gameObject.SetActive(false);
        gameOverText = GameObject.Find("GameOver");
        gameOverText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       disBeards.text = beards.ToString() + " x";
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
                if (playerHealth > 0)
                {
                    playerHealth -= 1;
                    hurtDisplay.gameObject.SetActive(true);
                    disHealth.fillAmount = (float)playerHealth / (float)maxHealth;
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
        gameOverText.gameObject.SetActive(true);
        player.GetComponent<PlayerController>().enabled = false;
        cam.GetComponent<CameraController>().enabled = false;
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.5f);
        hurtDisplay.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        isDamaged = false;
    }

}
