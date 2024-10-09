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
            Debug.Log("���౻��������û��Application�Ķ����ϡ������λ�ã�");
            return;
        }
        application.OnLaunch += OnLaunchOpenExplorer;
    }
    public abstract void OnLaunchOpenExplorer();
}
