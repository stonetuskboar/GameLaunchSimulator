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

    public void UpdateFileList(List<int> fileDataIdList)
    {
        while(fileList.Count < fileDataIdList.Count)
        {
            fileList.Add(CreatePrefab());
        }

        for(int i = 0; i < fileDataIdList.Count; i++)
        {
            FileData data = fileSo.fileDataList[fileDataIdList[i]];
            fileList[i].SetFile(data);
            fileList[i].gameObject.SetActive(true);
        }
        for(int i = fileDataIdList.Count; i < fileList.Count; i++)
        {
            fileList[i].gameObject.SetActive(false);
        }
    }

    public ������_���ذ�ť CreatePrefab(FileData data)
    {
        if (data == null)
        {
            return null;
        }
        ������_���ذ�ť file = CreatePrefab();
        file.SetFile(data);
        return file;
    }
    public ������_���ذ�ť CreatePrefab()
    {
        GameObject obj = Instantiate(FilePrefab, FileLayoutTransform);
        ������_���ذ�ť file = obj.GetComponent<������_���ذ�ť>();
        file.Init(this);
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
