using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public RawImage hurtDisplay;
    public Text playerHealthText;
    public Text gameOverText;
    private GameObject player;
    private GameObject camera;
    private bool isDamaged;
    private int playerHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isDamaged && other.gameObject.GetComponent<EnemyAI>().isDamaged == false)
        {
            isDamaged = true;
            playerHealth -= 1;
            if (playerHealth < 0)
            {
                playerHealth = 0;
            }
            playerHealthText.text = playerHealth.ToString();
            if (playerHealth > 0)
            {
                hurtDisplay.gameObject.SetActive(true);
            }
            StartCoroutine(Damage());
        }
    }

    IEnumerator GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        player.GetComponent<PlayerController>().enabled = false;
        camera.GetComponent<CameraController>().enabled = false;
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
