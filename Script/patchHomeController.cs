using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class patchHomeController : MonoBehaviour
{
    public GameObject FilePrefab;
    public Transform FileLayoutTransform;
    public FileSo fileSo;
    public List<控制器_下载按钮> fileList = new();
    public 控制器_下载弹窗 downloadReminder;

    private void Start()
    {
        int i = 0;
        while(fileList.Count <10)
        {
            fileList.Add(CreatePrefab(fileSo.fileDataList[i]));
            i++;
        }
    }

    public 控制器_下载按钮 CreatePrefab(FileData data)
    {
        if (data == null)
        {
            return null;
        }
        GameObject obj = Instantiate(FilePrefab, FileLayoutTransform);
        控制器_下载按钮 file = obj.GetComponent<控制器_下载按钮>();
        file.Init(this);
        file.SetFile(data);
        return file;
    }

    public bool 下载(FileData data, 控制器_下载按钮 file)
    {
        if (downloadReminder.GetActive() == true)
        {
            return false;
        }
        else
        {
            downloadReminder.SetThenActive(data, file);
            return true;
        }
    }

    public bool CheckIfDownload(int id)
    {
        for(int i = 0; i < fileList.Count; i++)
        {
            if (fileList[i].GetFileId() == id)
            {
                if(fileList[i].GetIsDownloaded() == true)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
