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


    public void Start()
    {
        ShowNewPostById(0);
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

        int i; //注意i不在for循环内定义。将有replydata的回复显示。将没有replydata的回复隐藏。

        while(replyList.Count < data.replies.Count)
        {
            CreateNewReply();
        }
        for (i = 0; i < data.replies.Count; i++)
        {
            if (data.replies[i] != null)
            {
                replyList[i].SetReplyThenActive(data.replies[i]);
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


[Serializable]
public class PostData
{
    public int postId;
    public string MainTitleText;
    public List<ReplyData> replies = new List<ReplyData>();
}

[Serializable]
public class ReplyData
{
    public Sprite avatar;
    public string replyText;
}