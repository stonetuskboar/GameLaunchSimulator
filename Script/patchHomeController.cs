using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class patchHomeController : MonoBehaviour
{
    public GameObject FilePrefab;
    public Transform FileLayoutTransform;
    public FileSo fileSo;
    public List<FileObject> fileList = new();
    public ¿ØÖÆÆ÷_ÏÂÔØµ¯´° downloadReminder;

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

    public FileObject CreatePrefab(FileData data)
    {
        if (data == null)
        {
            return null;
        }
        FileObject file = CreatePrefab();
        file.SetFile(data);
        return file;
    }
    public FileObject CreatePrefab()
    {
        GameObject obj = Instantiate(FilePrefab, FileLayoutTransform);
        FileObject file = obj.GetComponent<FileObject>();
        file.Init(this);
        return file;
    }

    public bool ÏÂÔØ(FileData data, FileObject file)
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

    public void AllClearToUndownload()
    {
        for( int i = 0;i < fileList.Count;i++)
        {
            fileList[i].ClearToUndownload();
        }
    }
}
