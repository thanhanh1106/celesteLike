using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GameUiManager.Instance.UpdateCherry(Prefs.NumberOfCherry);

        List<int> cherries = JsonManager.Instance.LoadListIdCherryToJson();
        Cherry[] cherriesObject = FindObjectsOfType<Cherry>();
        foreach (int cherry in cherries)
        {
            foreach(Cherry cherryObject in cherriesObject)
            {
                if(cherryObject.Id == cherry)
                    Destroy(cherryObject);
            }
        }
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
    }
    public void CherryCount()
    {
        Prefs.NumberOfCherry++;
        GameUiManager.Instance.UpdateCherry(Prefs.NumberOfCherry);
    }
}
