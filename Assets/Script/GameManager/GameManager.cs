using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;

    public JsonListManager<int> CherryIdManager;

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

        // duyệt tất cả các quả cherry đã thu thập và hủy nó đi trên scenes
        CherryIdManager = new JsonListManager<int>("/JsonData/cherryData.json");
        List<int> cherryIdCollected = CherryIdManager.LoadListFromJsonFile();
        Cherry[] cherriesObject = FindObjectsOfType<Cherry>();
        if(cherryIdCollected != null && cherryIdCollected.Count > 0)
        {
            foreach (int cherryId in cherryIdCollected)
            {
                foreach (Cherry cherry in cherriesObject)
                {
                    if (cherry.Id == cherryId)
                        Destroy(cherry.gameObject);
                        //cherry.gameObject.SetActive(false);
                }
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
