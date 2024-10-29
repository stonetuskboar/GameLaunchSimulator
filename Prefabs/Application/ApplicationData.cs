using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppData", menuName = "ScriptableObjects/AppData", order = 5)]
public class ApplicationSo : ScriptableObject
{
    public List<AppData> apps = new List<AppData>();
    public AppData simulatorAppdata;
    public AppData GetAppDataById(int id)
    {

        if(apps.Count > id)
        {
            if (apps[id].AppId == id)
            {
                return apps[id];
            }
        }

        for(int i = 0; i < apps.Count; i++)
        {
            if(apps[i].AppId == id) { 
            return apps[i];
            }
        }

        Debug.Log("在ApplicationSo中找不到对应AppData。");
        return new AppData("找不到对应AppData");
    }

    public int GetCount()
    {
        return apps.Count;
    }
}

[Serializable]
public class AppData
{
    public int AppId = -1;
    public string AppName;
    public Sprite AppSprite;

    public AppData(string str)
    {
        AppName = str;
    }
}