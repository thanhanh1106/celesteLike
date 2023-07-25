using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUiManager : Singleton<GameUiManager>
{
    //public GameObject GameUi;

    //public Dialog GameDialog;

    public TextMeshProUGUI DeathText;
    public TextMeshProUGUI CherryText;

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    public void UpdateDeath(int numOfDeath)
    {
        Debug.Log(numOfDeath);
        DeathText.text = numOfDeath.ToString("000");
    }
    public void UpdateCherry(int numOfCherry)
    {
        CherryText.text = numOfCherry.ToString("000");
    }
}
