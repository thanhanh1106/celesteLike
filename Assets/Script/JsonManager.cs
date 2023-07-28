using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class JsonListManager<T>
{
    [System.Serializable]
    public class ListJson
    {
        public List<T> list = new List<T>();
    }

    ListJson listObject;
    string filePath;

    public JsonListManager(string filePath)
    {
        listObject = new ListJson();
        this.filePath = Application.dataPath + filePath;
    }

    void SaveListToJson()
    {
        string json = JsonUtility.ToJson(listObject);
        File.WriteAllText(filePath, json);
    }
    public List<T> LoadListFromJsonFile()
    {
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            listObject = JsonUtility.FromJson<ListJson>(json);
            return listObject.list;
        }
        else
        {
            Debug.Log("Không có file này!");
            return null;
        }   
    }
    public void AddElementToListInJson(T element)
    {
        listObject.list.Add(element);
        SaveListToJson();
    }
    public void RemoveElementToListInJson(T element)
    {
        listObject.list.Remove(element);
        SaveListToJson();
    }
}
