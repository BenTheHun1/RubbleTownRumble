using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public RawImage hurtDisplay;
    public Text gameOverText;
    private GameObject player;
    private GameObject cam;
    private bool isDamaged;
    private int playerHealth;
    private int maxHealth;
    private Image disHealth;

    public int beards;
    private Text disBeards;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        playerHealth = maxHealth;
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
        disHealth = GameObject.Find("HP").GetComponent<Image>();
        disBeards = GameObject.Find("BeardAmount").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        disBeards.text = beards.ToString() + " x";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isDamaged && other.gameObject.GetComponent<EnemyAI>().isRagdoll == false && other.gameObject.GetComponent<EnemyAI>().isAttacking == true && other.gameObject.GetComponent<EnemyAI>().isDamaged == false)
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
        yield return new WaitForSeconds(1);
        hurtDisplay.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        isDamaged = false;
        
    }

}
