using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public HealthSystem hs;
    public GameObject shield;
    private Canvas upgradeSwordUI;
    private Canvas upgradeArmorUI;
    private Canvas restoreHealthUI;
    private Canvas buyShieldUI;
    private GameObject buyShield;
    private string swordUpgradeText;
    private string armorUpgradeText;
    private string healthPotionText;
    private string buyShieldText;
    private bool isHovered;
    private bool buyCoolDown;
    private int costPotion = 10;
    private int costArmor = 25;
    private int costSword = 20;
    private int costShield = 8;

    void Start()
    {
        hs = GameObject.Find("Player").GetComponent<HealthSystem>();
        upgradeSwordUI = GameObject.Find("UpgradeSwordUI").GetComponent<Canvas>();
        upgradeArmorUI = GameObject.Find("UpgradeArmorUI").GetComponent<Canvas>();
        restoreHealthUI = GameObject.Find("RestoreHealthUI").GetComponent<Canvas>();
        buyShieldUI = GameObject.Find("BuyShieldUI").GetComponent<Canvas>();
        buyShield = GameObject.Find("BuyShield");
        swordUpgradeText = upgradeSwordUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        armorUpgradeText = upgradeArmorUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        healthPotionText = restoreHealthUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        buyShieldText = buyShieldUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }

    public void SwordUpgradeHoverEnter()
    {
        upgradeSwordUI.enabled = true;
        isHovered = true;
    }

    public void SwordUpgradeHoverLeave()
    {
        upgradeSwordUI.enabled = false;
        isHovered = false;
    }

    public void SwordUpgrade()
    {
        if (hs.beards >= costSword && !buyCoolDown && isHovered)
        {
            hs.beards -= costSword;
            costSword += 5;
            hs.AttackAmount--;
            StartCoroutine(AffordableSwordUpgrade());
        }
        else
        {
            StartCoroutine(NotAffordableSwordUpgrade());
        }
    }

    public void ArmorUpgradeHoverEnter()
    {
        upgradeArmorUI.enabled = true;
        isHovered = true;
    }

    public void ArmorUpgradeHoverLeave()
    {
        upgradeArmorUI.enabled = false;
        isHovered = false;
    }

    public void ArmorUpgrade()
    {
        if (hs.beards >= costArmor && !buyCoolDown && isHovered)
        {
            hs.beards -= costArmor;
            costArmor += 5;
            hs.maxHealth += 5;
            hs.playerHealth += 5;
            hs.disHealth.fillAmount = (float)hs.playerHealth / (float)hs.maxHealth;
            StartCoroutine(AffordableArmorUpgrade());
        }
        else
        {
            StartCoroutine(NotAffordableArmorUpgrade());
        }
    }

    public void RestoreHealthHoverEnter()
    {
        restoreHealthUI.enabled = true;
        isHovered = true;
    }

    public void RestoreHealthHoverLeave()
    {
        restoreHealthUI.enabled = false;
        isHovered = false;
    }

    public void RestoreHealth()
    {
        if (hs.beards >= costPotion && !buyCoolDown && isHovered)
        {
            hs.beards -= costPotion;
            hs.playerHealth = hs.maxHealth;
            hs.disHealth.fillAmount = (float)hs.playerHealth / (float)hs.maxHealth;
            StartCoroutine(AffordableHealthPotion());
        }
        else
        {
            StartCoroutine(NotAffordableHealthPotion());
        }
    }

    public void BuyShieldHoverEnter()
    {
        buyShieldUI.enabled = true;
        isHovered = true;
    }

    public void BuyShieldHoverExit()
    {
        if (!buyCoolDown)
        {
            buyShieldUI.enabled = false;
            isHovered = false;
        }
    }

    public void BuyShield()
    {
        if (hs.beards >= costShield && !buyCoolDown && isHovered)
        {
            hs.beards -= costShield;
            hs.playerHealth = hs.maxHealth;
            Instantiate(shield, buyShield.transform.position, Quaternion.identity);
            StartCoroutine(AffordableShield());
        }
        else
        {
            StartCoroutine(NotAffordableShield());
        }
    }


    IEnumerator NotAffordableSwordUpgrade()
    {
        buyCoolDown = true;
        upgradeSwordUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nNOT ENOUGH BEARDS";
        yield return new WaitForSeconds(2f);
        upgradeSwordUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = swordUpgradeText;
        buyCoolDown = false;
    }

    IEnumerator NotAffordableArmorUpgrade()
    {
        buyCoolDown = true;
        upgradeArmorUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nNOT ENOUGH BEARDS";
        yield return new WaitForSeconds(2f);
        upgradeArmorUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = armorUpgradeText;
        buyCoolDown = false;
    }

    IEnumerator NotAffordableHealthPotion()
    {
        buyCoolDown = true;
        restoreHealthUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nNOT ENOUGH BEARDS";
        yield return new WaitForSeconds(2f);
        restoreHealthUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = healthPotionText;
        buyCoolDown = false;
    }

    IEnumerator NotAffordableShield()
    {
        buyCoolDown = true;
        buyShieldUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nNOT ENOUGH BEARDS";
        yield return new WaitForSeconds(2f);
        buyShieldUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = buyShieldText;
        buyCoolDown = false;
    }

    IEnumerator AffordableSwordUpgrade()
    {
        buyCoolDown = true;
        upgradeSwordUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nSUCCESSFULLY UPGRADED SWORD";
        yield return new WaitForSeconds(2f);
        upgradeSwordUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = swordUpgradeText;
        buyCoolDown = false;
    }

    IEnumerator AffordableArmorUpgrade()
    {
        buyCoolDown = true;
        upgradeArmorUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nSUCCESSFULLY UPGRADED ARMOR";
        yield return new WaitForSeconds(2f);
        upgradeArmorUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = armorUpgradeText;
        buyCoolDown = false;
    }

    IEnumerator AffordableHealthPotion()
    {
        buyCoolDown = true;
        restoreHealthUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nSUCCESSFULLY RESTORED HEALTH TO FULL";
        yield return new WaitForSeconds(2f);
        restoreHealthUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = healthPotionText;
        buyCoolDown = false;
    }

    IEnumerator AffordableShield()
    {
        buyCoolDown = true;
        buyShieldUI.gameObject.transform.root.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nSUCCESSFULLY BOUGHT SHIELD";
        buyShield.GetComponent<MeshRenderer>().enabled = false;
        buyShield.GetComponent<MeshCollider>().enabled = false;
        buyShield.GetComponent<XRSimpleInteractable>().enabled = false;
        yield return new WaitForSeconds(2f);
        buyShieldUI.enabled = false;
    }
}
