using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageData", menuName = "ScriptableObjects/MessageData", order = 1)]
public class MessageSo : ScriptableObject
{
    public List<FriendData> friendsList;
    public FriendData PlayerData;

    public List<MessageSegment> messageSegmentList;
    public List<MessageData> messageList;


    public FriendData GetFriendById(int id)
    {
        if (id < friendsList.Count && friendsList[id].FriendId == id)
        {
            return friendsList[id];
        }

        for (int i = 0; i < friendsList.Count; i++)
        {
            if (friendsList[i].FriendId == id)
            {
                return friendsList[i];
            }
        }
        return null;
    }
    public MessageData GetMessageById(int id)
    {
        if (id < messageList.Count &&  messageList[id].messageId == id)
        {
            return messageList[id];
        }

        for (int i = 0; i < messageList.Count; i++)
        {
            if (messageList[i].messageId == id)
            {
                return messageList[i];
            }
        }
        return null;
    }
    public MessageSegment GetSegmentById( int id)
    {
        if(id < 0)
        {
            return null;
        }
        if (id < messageSegmentList.Count && messageSegmentList[id].SegmentId == id)
        {
            return messageSegmentList[id];
        }

        for (int i = 0; i < messageSegmentList.Count; i++)
        {
            if (messageSegmentList[i].SegmentId == id)
            {
                return messageSegmentList[i];
            }
        }
        return null;
    }
}


[Serializable]
public class FriendData
{
    public int FriendId = -1; //-1�ǲ����ڡ�
    public Sprite AvatarSprite;
    public string Name ="δȡ��";
}

[Serializable]
public class MessageData
{
    public int messageId;
    public bool IsSend = false;// true == �㷢����Ϣ�� false= �Է�������Ϣ��
    [TextArea(1, 4)]
    public string text = "";
}
[Serializable]
public class ChatReplyData
{
    public int replyId; //�����ڻظ��󴥷��¼�
    public int messageId;
    public int nextSegmentId = -1;// -1�Ļ�����û�лظ�
}

[Serializable]
public class MessageSegment
{
    public int SegmentId = -1;
    public int FriendId = -1;//-1�ǲ����ڡ�
    public List<int> messageIdList = new();
    public List<ChatReplyData> replyList = new();
}

[Serializable]
public class FriendSaveData
{
    public int FriendId = -1; //-1�ǲ����ڡ�
    public List<int> messagesId;
}
