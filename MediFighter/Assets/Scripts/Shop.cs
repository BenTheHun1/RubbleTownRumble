using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public HealthSystem hs;
    private Canvas upgradeSwordUI;
    private Canvas upgradeArmorUI;
    private Canvas restoreHealthUI;
    private Canvas buyShieldUI;
    private GameObject buyShield;
    private bool isHovered;
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
        if (hs.beards >= costSword)
        {
            hs.beards -= costSword;
            costSword += 5;
            hs.AttackAmount++;
            Debug.Log("Successfully upgraded sword");
        }
        else
        {
            Debug.Log("Not enough beards");
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
        if (hs.beards >= costArmor)
        {
            hs.beards -= costArmor;
            costArmor += 5;
            hs.maxHealth += 5;
            hs.playerHealth += 5;
            //hs.disHealth.fillAmount = (float)hs.playerHealth / (float)hs.maxHealth;
            Debug.Log("Sucessfully upgraded armor");
        }
        else
        {
            Debug.Log("Not enough beards");
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
        if (hs.beards >= costPotion)
        {
            hs.beards -= costPotion;
            hs.playerHealth = hs.maxHealth;
            //hs.disHealth.fillAmount = (float)hs.playerHealth / (float)hs.maxHealth;
            Debug.Log("Sucessfully restored all health");
        }
        else
        {
            Debug.Log("Not enough beards");
        }
    }

    public void BuyShieldHoverEnter()
    {
        buyShieldUI.enabled = true;
        isHovered = true;
    }

    public void BuyShieldHoverExit()
    {
        buyShieldUI.enabled = false;
        isHovered = false;
    }

    public void BuyShield()
    {
        if (hs.beards >= costShield)
        {
            hs.beards -= costShield;
            hs.playerHealth = hs.maxHealth;
            buyShield.SetActive(false);
            buyShieldUI.gameObject.SetActive(false);
            Debug.Log("Sucessfully bought shield");
        }
        else
        {
            Debug.Log("Not enough beards");
        }
    }
}
