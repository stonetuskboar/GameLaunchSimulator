using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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
    private List<Application> appList = new List<Application>();

    public virtual void Awake()
    {
        canvas.worldCamera = Camera.main;
    }
    public virtual void Init(LevelManager manger) //Init��levelmanager��start������
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
            Debug.Log("��û��app�������ȴ��Ȼ������GetFirstApp");
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
    protected IEnumerator WaitAndShowPet(float waitTime, float appearTime, Vector3 position, string str)
    {
        yield return new WaitForSeconds(waitTime);
        levelManager.petController.ShowAtWithTextThenDisappear(position, str, appearTime);
    }
    public Application AddFirstApp()
    {
        if(ApplistId.Count > 0)
        {
            Application app = levelManager.deskManager.IfNoExistAddApp(ApplistId[0]);
            appList.Add(app);
            return app;
        }
        Debug.Log("��ǰ�ؿ�û�����ӵ�appȴ��Ȼ������AddFirstApp");
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
        }
    }

    public static string GetTimeStrByFloat(float time)
    {
        string timeStr = "";
        int timeInt = (int)time;
        timeStr += (timeInt / 60).ToString() + "����";
        if(timeInt % 60 !=0)
        {
            timeStr +=  (timeInt % 60).ToString() + "��";
        }
        return timeStr;
    }
}
