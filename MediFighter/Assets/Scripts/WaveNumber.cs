using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveNumber : MonoBehaviour
{
    private Canvas waveStartUI;
    private SpawnManager sm;
    private int newWaveNum;

    // Start is called before the first frame update
    void Start()
    {
        waveStartUI = GameObject.Find("WaveNumberUI").GetComponent<Canvas>();
        sm = GameObject.Find("Spawns").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        waveStartUI.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "\n\nSTART WAVE " + (sm.waveNum + 1).ToString();
    }

    public void WaveNumberUIHoverEnter()
    {
        waveStartUI.enabled = true;
    }

    public void WaveNumberUIHoverLeave()
    {
        waveStartUI.enabled = false;
    }

    public void Grabbed()
    {
        waveStartUI.enabled = false;
    }
}
