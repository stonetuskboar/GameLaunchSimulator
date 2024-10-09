using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class patchHomeController : MonoBehaviour
{
    public GameObject FilePrefab;
    public Transform FileLayoutTransform;
    public FileSo fileSo;
    public List<������_���ذ�ť> fileList = new();
    public ������_���ص��� downloadReminder;

    private void Start()
    {
        int i = 0;
        while(fileList.Count <10)
        {
            fileList.Add(CreatePrefab(fileSo.fileDataList[i]));
            i++;
        }
    }

    public ������_���ذ�ť CreatePrefab(FileData data)
    {
        if (data == null)
        {
            return null;
        }
        GameObject obj = Instantiate(FilePrefab, FileLayoutTransform);
        ������_���ذ�ť file = obj.GetComponent<������_���ذ�ť>();
        file.Init(this);
        file.SetFile(data);
        return file;
    }

    public bool ����(FileData data, ������_���ذ�ť file)
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
