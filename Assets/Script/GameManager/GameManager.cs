using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;

    protected void Awake()
    {
        MakeSingleton(false);
    }
    private void Start()
    {
        if (!Prefs.IsGameEntered)
        {
            Prefs.CheckPoint = player.transform.position;
            Prefs.IsGameEntered = true;
        }
        GameUiManager.Instance.UpdateDeath(Prefs.NumberOfDeaths);
    }
    public void SetCheckPoint(Vector3 position)
    {
        Prefs.CheckPoint = position;
    }
    public Vector3 GetCheckPoint()
    {
        return Prefs.CheckPoint;
    }
    public void DeathCount()
    {
        Prefs.NumberOfDeaths++;
        GameUiManager.Instance.UpdateDeath(Prefs.NumberOfDeaths);
        Debug.Log(Prefs.NumberOfDeaths);
    }
}
