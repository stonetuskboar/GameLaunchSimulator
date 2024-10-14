using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicLevel : MonoBehaviour
{
    public int levelId;
    protected LevelManager levelManager;
    public Canvas canvas;
    public List<int> ApplistId = new List<int>();
    public CompletePlate completePlate;

    private List<Application> appList = new List<Application>();

    public virtual void Awake()
    {
        canvas.worldCamera = Camera.main;
    }
    public virtual void Init(LevelManager manger) //Init在levelmanager的start中启动
    {
        levelManager = manger;
    }



    public virtual void AddApps()
    {
        for (int i = 0; i < ApplistId.Count; i++)
        {
            Application app = levelManager.deskManager.IfNoExistAddApp(ApplistId[i]);
            if(app != null)
            {
                appList.Add(app);
            }
        }
    }
    public Application GetFirstApp()
    {
        if (appList.Count > 0)
        {
            return appList[0];
        }
        else
        {
            Debug.Log("在没有app的情况下却依然调用了GetFirstApp");
            return null;
        }
    }
    public Application GetAppById(int id)
    {
        for( int i = 0;i < appList.Count;i++)
        {
            if (appList[i].appId == id)
            {
                return appList[i];
            }
        }
        return null;
    }

    public Application AddFirstApp()
    {
        if(ApplistId.Count > 0)
        {
            Application app = levelManager.deskManager.IfNoExistAddApp(ApplistId[0]);
            appList.Add(app);
            return app;
        }
        Debug.Log("当前关卡没有增加的app却依然调用了AddFirstApp");
        return null;
    }

    public bool CheckIfAppExist(int id)
    {
        for(int i = 0; i < appList.Count ; i++) 
        {
            if (appList[i].appId == id)
            {
                return true;
            }
        }
        return false;
    }

    public virtual void OnCompleteLevel()
    {
    }

    public virtual void DestroyLevel()
    {
        DeleteAllApps();
        Destroy(gameObject);
    }
    public abstract void OnHyperLinkClick(string link);

    public virtual void DeleteAllApps()
    {
        for (int i = 0; i < appList.Count; i++)
        {
            appList[i].DestroyThis();
        }
    }
}
