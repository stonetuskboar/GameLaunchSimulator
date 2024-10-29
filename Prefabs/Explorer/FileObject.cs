using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FileObject : MonoBehaviour
{
    [Header("属性分配")]
    private int fileId = 0;
    private bool IsDownloaded = false;
    public Image 应用图标;
    public TextMeshProUGUI 应用名称;
    public TextMeshProUGUI 应用大小;
    private patchHomeController patchController;
    public Button 下载按钮;
    private FileData thisData;

    public void Awake()
    {
        下载按钮.onClick.AddListener(OnButtonClick);
    }
    
    public void Init(patchHomeController controller)
    {
        patchController = controller;
    }


    public void SetFile(FileData data)
    {
        thisData = data;
        fileId = data.fileId;
        IsDownloaded = false;
        下载按钮.interactable = true;
        应用图标.sprite = data.fileSprite;
        应用名称.text = data.fileName;
        应用大小.text = data.fileSize;
    }
    public int GetFileId()
    {
        return fileId;
    }
    public bool GetIsDownloaded()
    {
        return IsDownloaded;
    }
    public void OnButtonClick()
    {
        if(patchController.下载(thisData, this) == false)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            //动画播放成功后会触发DownloadSuccess()
        }
    }

    public void ClearToUndownload()
    {
        IsDownloaded = false;
        下载按钮.interactable = true;
    }

    public void DownloadSuccess()
    {
        IsDownloaded = true;
        下载按钮.interactable = false;
    }
 
}
