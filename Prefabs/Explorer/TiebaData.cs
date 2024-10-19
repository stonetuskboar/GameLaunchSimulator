using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TiebaData", menuName = "ScriptableObjects/TiebaData", order = 2)]
public class TiebaData : ScriptableObject
{
    public List<PostData> postData;
}
[Serializable]
public class PostData
{
    public int postId;
    public string MainTitleText;
    public List<TiebaReplyData> replyList = new List<TiebaReplyData>();
}
[Serializable]
public class TiebaReplyData
{
    public Sprite avatar;
    [TextArea(1, 6)]
    public string replyText;
}