using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageData", menuName = "ScriptableObjects/MessageData", order = 1)]
public class MessageSo : ScriptableObject
{
    public List<FriendData> friendsList;
    public FriendData PlayerData;
    public List<MessageSegment> messagesList;



    public FriendData GetFriendById(int id)
    {
        if (friendsList[id].FriendId == id)
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

    public MessageSegment GetSegmentById( int id)
    {
        if (messagesList[id].SegmentId == id)
        {
            return messagesList[id];
        }

        for (int i = 0; i < messagesList.Count; i++)
        {
            if (messagesList[i].SegmentId == id)
            {
                return messagesList[i];
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
    public bool IsSend = false;// true == �㷢����Ϣ�� false= �Է�������Ϣ��
    public string text = "";
}

[Serializable]
public class MessageSegment
{
    public int SegmentId = -1;
    public int FriendId = -1;//-1�ǲ����ڡ�
    public List<MessageData> messages;
}
