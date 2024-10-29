using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLevel : MonoBehaviour
{
    public int levelId;
    protected bool IsLaunched = false;
    protected LevelManager levelManager;
    public Canvas canvas;
    public List<int> ApplistId = new List<int>();
    public CompletePlate completePlate;
    public List<ContentIcon> contentIcons;
    private List<App> appList = new List<App>();

    public virtual void Awake()
    {
        if(canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }

    }
    public virtual void Init(LevelManager manger) //Init在levelmanager的start中启动
    {
        levelManager = manger;
    }

    public void AddApp(App app)
    {
        appList.Add(app);
    }

    public virtual void AddApps()
    {
        for (int i = 0; i < ApplistId.Count; i++)
        {
            App app = levelManager.deskManager.IfNoExistAddApp(ApplistId[i]);
            if(app != null)
            {
                appList.Add(app);
            }
        }
    }
    public App GetFirstApp()
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
    public App GetAppById(int id)
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
    protected IEnumerator WaitAndShowPet(float waitTime, float appearTime, Vector3 position, string str)
    {
        yield return new WaitForSeconds(waitTime);
        levelManager.petController.ShowAtWithTextThenDisappear(position, str, appearTime);
    }
    public App AddFirstApp()
    {
        if(ApplistId.Count > 0)
        {
            App app = levelManager.deskManager.IfNoExistAddApp(ApplistId[0]);
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
    public LevelManager GetLevelManager()
    {
        return levelManager;
    }
    public virtual void DestroyLevel()
    {
        levelManager.explorerManager.explorerController.ClearExplorer();
        DeleteAllApps();
        Destroy(gameObject);
    }
    public virtual void OnHyperLinkClick(string link)
    {

    }
    public virtual void OnChatReplyClick(int replyId)
    {
    }
    public virtual void DeleteAllApps()
    {
        for (int i = 0; i < appList.Count; i++)
        {
            appList[i].DestroyThis();
            appList.RemoveAt(i);
            i--;
        }
    }

    public static string GetTimeStrByFloat(float time)
    {
        string timeStr = "";
        int timeInt = (int)time;
        timeStr += (timeInt / 60).ToString() + "分钟";
        if(timeInt % 60 !=0)
        {
            timeStr +=  (timeInt % 60).ToString() + "秒";
        }
        return timeStr;
    }
}

