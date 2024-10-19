using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TiebaController : MonoBehaviour
{
    public GameObject replyPrefab;
    public Transform replyAreaTransform;
    public TiebaData tiebaData;
    public List<TiebaReply> replyList;
    public TextMeshProUGUI titleText;
    public ExplorerController controller;

    //public void Start()
    //{
    //    ShowNewPostById(0);
    //}
    public void Init(TiebaData tiebaSo)
    {
        tiebaData = tiebaSo;
    }

    public void InitAndShowPost(TiebaData tiebaSo, int id)
    {
        Init(tiebaSo);
        ShowNewPostById(id);
    }

    public void ShowNewPostById(int id)
    {
        for(int i = 0; i < tiebaData.postData.Count; i ++)
        {
            if (tiebaData.postData[i].postId == id)
            {
                ShowNewPost(tiebaData.postData[i]);
                break;
            }    
        }
    }
    public void ShowNewPost(PostData data)
    {
        titleText.text = data.MainTitleText;

        int i; //ע��i����forѭ���ڶ��塣����replydata�Ļظ���ʾ����û��replydata�Ļظ����ء�

        while(replyList.Count < data.replyList.Count)
        {
            CreateNewReply();
        }
        for (i = 0; i < data.replyList.Count; i++)
        {
            if (data.replyList[i] != null)
            {
                replyList[i].SetReplyThenActive(data.replyList[i]);
            }
            else
            {
                replyList[i].gameObject.SetActive(false);
            }
        }
        while( i< replyList.Count)
        {
            if (replyList[i].gameObject.activeSelf == true)
            {
                replyList[i].gameObject.SetActive(false);
            }
        }
    }

    public TiebaReply CreateNewReply()
    {
        GameObject obj = Instantiate(replyPrefab, replyAreaTransform);
        TiebaReply reply = obj.GetComponent<TiebaReply>();
        replyList.Add(reply);
        obj.SetActive(false);
        return reply;
    }
}


