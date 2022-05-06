using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private Canvas upgradeSwordUI;
    private Canvas upgradeArmorUI;
    private Canvas restoreHealthUI;
    private Canvas buyShieldUI;
    private bool isHovered;

    void Start()
    {
        upgradeSwordUI = GameObject.Find("UpgradeSwordUI").GetComponent<Canvas>();
        upgradeArmorUI = GameObject.Find("UpgradeArmorUI").GetComponent<Canvas>();
        restoreHealthUI = GameObject.Find("RestoreHealthUI").GetComponent<Canvas>();
        buyShieldUI = GameObject.Find("BuyShieldUI").GetComponent<Canvas>();
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
        Debug.Log("Successfully upgraded sword");
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
        Debug.Log("Sucessfully upgraded armor");
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
        Debug.Log("Sucessfully restored all health");
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
        Debug.Log("Sucessfully bought shield");
    }
}
