using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicApplicationLaunch : MonoBehaviour
{
    protected Application application;

    public void Awake()
    {
        application = GetComponent<Application>();
        if (application == null)
        {
            Debug.Log("此类被放置在了没有Application的对象上。请调整位置！");
            return;
        }
        application.OnLaunch += OnLaunchOpenExplorer;
    }
    public abstract void OnLaunchOpenExplorer();
}
