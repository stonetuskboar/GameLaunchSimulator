using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : LayeredCanvas
{
    public RectTransform ChatRectTrans;
    public RectTransform FriendAreaTransform;
    public TextMeshProUGUI NowNameTmp;
    public ScrollRect MessageScroll;
    public RectTransform ChatReplyListTransform;
    [Header("预制件")]
    public GameObject ReceiveMessagePrefab;
    public GameObject SendMessagePrefab;
    public GameObject FriendLabelPrefab;
    public GameObject MessageListPrefab;
    public GameObject ReplyPrefab;
    private MessageSo messageSo;

    private FriendObject nowChatFriend = null; 
    private ChatManager chatManager;
    public List<FriendObject> FriendList = new();
    public List<ChatReplyObject> ChatReplyList = new();

    public override void Awake()
    {
        base.Awake();
        MessageScroll.enabled = true;
    }
    public void Init(ChatManager manager, MessageSo so)
    {
        chatManager = manager;
        messageSo = so;
    }
    public void SwitchChatToFriend(int id)
    {
        FriendObject friend = FindFriendObjectById(id);
        SwitchChatToFriend(friend);
    }
    public void SwitchChatToFriend(FriendObject friend)
    {
        if (friend != nowChatFriend)
        {
            UnshowAllReply();
            nowChatFriend?.UnShow();
            nowChatFriend = friend;
            MessageScroll.content = friend.MessageListRectTrans;
            ShowReplysByFriend(friend);
            friend.Show();
            NowNameTmp.text = nowChatFriend.friendName.text;
        }
    }



    public void TopFriendLabel(int friendId,string text)
    {
        FriendObject friendObject = FindFriendObjectById(friendId);
        if (friendObject != null)
        {
            TopFriendLabel(friendObject,text);
        }
    }

    public void TopFriendLabel(FriendObject friendObj, string text)
    {
        friendObj.labelTransform.SetSiblingIndex(0);
        friendObj.lastMessage.text = text;
    }

    public void IfNewAddFriend(int id)//无中生友！
    {
        if (CheckIfFriendExist(id) == false)
        {
            CreateNewFriendObject(id);
        }
    }

    public FriendObject FindFriendObjectById(int id) //无中生友！
    {
        int fid = FindFriendById(id);
        if (fid != -1)
        {
            return FriendList[fid];
        }
        else
        {
            return null;
        }
    }

    public bool CheckIfFriendExist(int id) //无中生友！
    {
        if (FindFriendById(id) != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int FindFriendById(int id) //无中生友！
    {
        if (id < FriendList.Count &&  FriendList[id].friendId == id)
        {
            return id;
        }

        for (int i = 0; i < FriendList.Count; i++)
        {
            if (FriendList[i].friendId == id)
            {
                return i;
            }
        }
        return -1;
    }
    public MessageObject AddMessage(int messageId, FriendObject friend)
    {
        return AddMessage(messageSo.GetMessageById(messageId) , friend);
    }
    public MessageObject AddMessage(MessageData data, FriendObject friend)
    {
        MessageObject messageObject = CreateNewMessageObject(data.IsSend, friend.MessageListRectTrans);
        messageObject.SetText(data.text);
        messageObject.SetData(data);
        if (data.IsSend == false)
        {
            messageObject.SetAvatar(friend.avatarImage.sprite);
            TopFriendLabel(friend, data.text);
        }
        else
        {
            messageObject.SetAvatar(messageSo.PlayerData.AvatarSprite);
        }
        friend.MessageList.Add(messageObject);
        messageObject.ShowMessage();
        return messageObject;
    }

    public MessageObject CreateNewMessageObject(bool IsSend, Transform ParentTransform)
    {
        GameObject obj;

        if (IsSend == true)
        {
            obj = Instantiate(SendMessagePrefab, ParentTransform);
        }
        else
        {
            obj = Instantiate(ReceiveMessagePrefab, ParentTransform);
        }
        obj.SetActive(false);
        MessageObject messageObject = obj.GetComponent<MessageObject>();
        return messageObject;
    }

    public FriendObject CreateNewFriendObject(int friendId)
    {
        GameObject obj;
        

        obj = Instantiate(FriendLabelPrefab, FriendAreaTransform);
        FriendObject friendObject = obj.GetComponent<FriendObject>();
        friendObject.Init(this,messageSo.GetFriendById(friendId));

        GameObject messageListObj = Instantiate(MessageListPrefab, MessageScroll.transform);
        messageListObj.name = messageListObj.name.Replace("(Clone)", "_"+friendId.ToString());
        friendObject.MessageListCanvasGroup = messageListObj.GetComponent<CanvasGroup>();
        friendObject.MessageListRectTrans = messageListObj.GetComponent<RectTransform>();
        FriendList.Add(friendObject);
        return friendObject;
    }

    public void UnshowAllReply()
    {
        for(int i = 0; i < ChatReplyList.Count; i++)
        {
            ChatReplyList[i].UnShowReply();
        }
    }

    public void ShowReplysByFriend(FriendObject Friend)
    {
        if(Friend.replyDataList == null)
        {
            return;
        }
        List<ChatReplyData> data = Friend.replyDataList;
        while (ChatReplyList.Count < data.Count)
        {
            CreateChatReplyObject();
        }

        if(Friend != nowChatFriend) //如果当前聊天界面的朋友不是正在发消息的朋友，则不更改回复状态。
        {
            return;
        }

        for (int i = 0; i < data.Count; i++)
        {
            ChatReplyList[i].SetReply(data[i], Friend);
            ChatReplyList[i].ShowReply();
        }
        for (int i = data.Count; i < ChatReplyList.Count; i++)
        {
            ChatReplyList[i].UnShowReply();
        }

    }


    public ChatReplyObject CreateChatReplyObject()
    {
        GameObject obj;

        obj = Instantiate(ReplyPrefab, ChatReplyListTransform);
        ChatReplyObject chatObject = obj.GetComponent<ChatReplyObject>();
        chatObject.Init(chatManager);
        ChatReplyList.Add(chatObject);
        return chatObject;
    }

    //被封印的邪恶魔法。在子对象中寻找有对应名字的对象。

    //public static Transform RecursiveFindChild(Transform parent, string childName)
    //{
    //    foreach (Transform child in parent)
    //    {
    //        if (child.name == childName)
    //        {
    //            return child;
    //        }
    //        else
    //        {
    //            Transform found = RecursiveFindChild(child, childName);
    //            if (found != null)
    //            {
    //                return found;
    //            }
    //        }
    //    }
    //    return null;
    //}
    public MessageSo GetMessageSo()
    {
        return messageSo;
    }
}

