using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs 
{
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key,value ? 1 : 0);
    }
    public static bool GetBool(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1; 
    }
    public static int NumberOfDeaths
    {
        set
        {
            PlayerPrefs.SetInt(GameConst.PREFS_DEATH_COUNT, value);
        }
        get => PlayerPrefs.GetInt(GameConst.PREFS_DEATH_COUNT);
    }
    public static int NumberOfCherry
    {
        set
        {
            PlayerPrefs.SetInt(GameConst.PREFS_CHERRY_COUNT, value);
        }
        get => PlayerPrefs.GetInt(GameConst.PREFS_CHERRY_COUNT);
    }
    public static Vector3 CheckPoint
    {
        get
        {
            float x = PlayerPrefs.GetFloat(GameConst.PREFS_CHECK_POINT_X,0f);
            float y = PlayerPrefs.GetFloat(GameConst.PREFS_CHECK_POINT_Y, 0f);
            return new Vector3(x, y, 0);
        }
        set
        {
            PlayerPrefs.SetFloat(GameConst.PREFS_CHECK_POINT_X , value.x);
            PlayerPrefs.SetFloat(GameConst.PREFS_CHECK_POINT_Y , value.y);
        }
    }
    public static bool IsGameEntered
    {
        get => GetBool(GameConst.PREFS_IS_GAME_ENTERED);
        set => SetBool(GameConst.PREFS_IS_GAME_ENTERED, value);
    }
}
