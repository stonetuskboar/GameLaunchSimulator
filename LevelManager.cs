using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> levelPrefabs;
    public Transform levelsTransform;
    private BasicLevel nowLevel = null;
    public DesktopManager deskManager;
    public ChatManager chatManager;
    public ExplorerManager explorerManager;
    public CameraController cameraController;
    public PetController petController;
    public static LevelManager instance = null;
    public int startLoadLevel = 1;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if( instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        CreateChat();
        CreateExplorer();
        LoadLevelById(startLoadLevel);
    }
    public void CreateChat()
    {
        //App数据由levelManager控制，但0 = 聊天软件，1=浏览器，这俩不变
        Application chat = deskManager.CreateAnApplication(0);
        chat.LaunchAddListener(LaunchChat);
    }
    public void CreateExplorer()
    {
        Application explorer = deskManager.CreateAnApplication(1);
        explorer.LaunchAddListener(LaunchExplorer);
    }
    public void LaunchExplorer()
    {
        explorerManager.ShowExplorer();
    }
    public void LaunchChat()
    {
        chatManager.ShowChat();
    }

    public void OnHyperLinkClick(string link)
    {
        if(nowLevel != null)
        {
           nowLevel.OnHyperLinkClick(link);
        }
    }

    public void LoadLevelById(int id)
    {
        if(nowLevel != null)
        {
            nowLevel.DestroyLevel();
        }
        nowLevel = CreateLevelById(id);
        nowLevel.Init(this);
    }

    public BasicLevel CreateLevelById(int id)
    {
        BasicLevel level = null;
        for (int i = 0; i < levelPrefabs.Count; i++)
        {
            if (levelPrefabs[i].levelId == id)
            {
                GameObject obj  = Instantiate(levelPrefabs[i].levelPrefab,levelsTransform);
                level = obj.GetComponent<BasicLevel>();
                return level;
            }
        }
        Debug.Log("没有下一关！");
        return level;
    }
}


[Serializable]
public class LevelData
{
    public int levelId;
    public GameObject levelPrefab;
}