using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject ContinueBtn;
    private void Start()
    {
        if (!Prefs.IsGameEntered)
            ContinueBtn.SetActive(false);
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        File.WriteAllText(Application.dataPath + GameConst.PATH_JSON_CHERRYCOLECTION, "{}");
        SceneManager.LoadScene("Game");
    }
    public void Continue()
    {
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
