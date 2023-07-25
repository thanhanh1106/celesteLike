using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class JsonManager : Singleton<JsonManager>
{
    public List<int> Cherrys = new List<int>();
    string pathCherryId = Application.dataPath + "/JsonData/cherryData.json";
    private void Awake()
    {
        
    }
    public void SaveListIdCherryToJson()
    {
        string jsonData = JsonUtility.ToJson(Cherrys);
        File.WriteAllText(pathCherryId, jsonData);
    }
    public List<int> LoadListIdCherryToJson()
    {
        
        if (File.Exists(pathCherryId))
        {
            string jsonData = File.ReadAllText(pathCherryId);
            Cherrys = JsonUtility.FromJson<List<int>>(jsonData);
        }
        return Cherrys;
    }
    public void AddIdCherryToJson(int cherry)
    {
        Cherrys.Add(cherry);
        SaveListIdCherryToJson();
    }
    
}
