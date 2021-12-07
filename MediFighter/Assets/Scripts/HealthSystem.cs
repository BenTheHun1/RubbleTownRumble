using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    private RawImage hurtDisplay;
    private GameObject gameOverText;
    private GameObject player;
    private GameObject cam;
    private bool isDamaged;
    public int playerHealth;
    public int maxHealth;
    private Image disHealth;

    public int beards;
    private Text disBeards;

    // Start is called before the first frame update
    void Start()
    {
        beards += 100; //debug
        maxHealth = 5;
        playerHealth = maxHealth;
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
        disHealth = GameObject.Find("HP").GetComponent<Image>();
        disBeards = GameObject.Find("BeardAmount").GetComponent<Text>();
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
        if (other.gameObject.CompareTag("EnemyAxe") && !isDamaged && other.transform.root.GetComponent<EnemyAICharacterJoints>().isRagdoll == false && other.transform.root.GetComponent<EnemyAICharacterJoints>().isAttacking == true && other.transform.root.GetComponent<EnemyAICharacterJoints>().isDamaged == false && other.transform.root.GetComponent<EnemyAICharacterJoints>().animEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
