using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DwarfHello : MonoBehaviour
{
    private Canvas hello;
    private TextMeshProUGUI shopTalk;
    private bool isHovered;

    // Start is called before the first frame update
    void Start()
    {
        hello = GameObject.Find("DwarfHello").GetComponent<Canvas>();
        shopTalk = hello.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHover()
    {
        hello.enabled = true;
        isHovered = true;
        int dialogueChoice = Random.Range(1, 8);
        switch (dialogueChoice)
        {
            case 1:
                shopTalk.text = "Welcome.";
                break;
            case 2:
                shopTalk.text = "Keep 'em sober, keep 'em shaven!";
                break;
            case 3:
                shopTalk.text = "I got anything ye' need!";
                break;
            case 4:
                shopTalk.text = "No need to push.";
                break;
            case 5:
                shopTalk.text = "Enjoy the wares!";
                break;
            case 6:
                shopTalk.text = "Need your sword to be razor sharp? Buy a whetstone!";
                break;
            case 7:
                shopTalk.text = "Those jerks like their beards, but I like 'em more.";
                break;
        }
    }

    public void onHoverStop()
    {
        hello.enabled = false;
        isHovered = false;
    }
}
