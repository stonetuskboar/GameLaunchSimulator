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
    public int FriendId = -1; //-1是不存在。
    public Sprite AvatarSprite;
    public string Name ="未取名";
}

[Serializable]
public class MessageData
{
    public int messageId;
    public bool IsSend = false;// true == 你发的消息。 false= 对方发的消息。
    [TextArea(1, 4)]
    public string text = "";
}
[Serializable]
public class ChatReplyData
{
    public int replyId; //用于在回复后触发事件
    public int messageId;
    public int nextSegmentId = -1;// -1的话代表没有回复
}

[Serializable]
public class MessageSegment
{
    public int SegmentId = -1;
    public int FriendId = -1;//-1是不存在。
    public List<int> messageIdList = new();
    public List<ChatReplyData> replyList = new();
}

[Serializable]
public class FriendSaveData
{
    public int FriendId = -1; //-1是不存在。
    public List<int> messagesId;
}
