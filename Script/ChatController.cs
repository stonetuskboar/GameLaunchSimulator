using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : LayeredCanvas
{

    public Transform FriendAreaTransform;
    public TextMeshProUGUI NowNameTmp;
    public ScrollRect MessageScroll;
    [Header("预制件")]
    public GameObject ReceiveMessagePrefab;
    public GameObject SendMessagePrefab;
    public GameObject FriendPrefab;
    public GameObject MessageListPrefab;
    private MessageSo messageSo;

    private FriendObject nowChatFriend = null; 

    public List<FriendObject> FriendList = new();

    public override void Awake()
    {
        base.Awake();
        MessageScroll.enabled = true;
    }
    public void Init(MessageSo so)
    {
        messageSo = so;
    }
    public FriendObject SwitchChatToFriend(int id)
    {
        FriendObject friend = FindFriendObjectById(id);
        SwitchChatToFriend(friend);
        return friend;
    }
    public FriendObject SwitchChatToFriend(FriendObject friend)
    {
        //ClearAllMessage();
        nowChatFriend?.UnShow();
        nowChatFriend = friend;
        MessageScroll.content = friend.MessageListRectTrans;
        friend.Show();
        NowNameTmp.text = nowChatFriend.name.text;
        return friend;
    }



    public void UpdateFriend(int friendId,string text)
    {
        FriendObject friendObject = FindFriendObjectById(friendId);
        if (friendObject != null)
        {
            UpdateFriend(friendObject,text);
        }
    }

    public void UpdateFriend(FriendObject friendObj, string text)
    {
        friendObj.transform.SetSiblingIndex(0);
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
        if (data.IsSend == false)
        {
            messageObject.SetAvatar(friend.avatarImage.sprite);
            UpdateFriend(friend, data.text);
        }
        else
        {
            messageObject.SetAvatar(messageSo.PlayerData.AvatarSprite);
        }
        friend.MessageList.Add(messageObject);
        messageObject.gameObject.SetActive(true);
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
        FriendObject friendObject = new();

        obj = Instantiate(FriendPrefab, FriendAreaTransform);
        friendObject.gameObject = obj;
        friendObject.transform = obj.transform;
        friendObject.avatarImage = RecursiveFindChild(obj.transform,"avatar").GetComponent<Image>();
        friendObject.name = RecursiveFindChild(obj.transform, "NameText").GetComponent<TextMeshProUGUI>();
        friendObject.lastMessage = RecursiveFindChild(obj.transform, "MessageText").GetComponent<TextMeshProUGUI>();
        friendObject.Init(messageSo.GetFriendById(friendId));

        GameObject messageListObj = Instantiate(MessageListPrefab, MessageScroll.transform);
        messageListObj.name = messageListObj.name.Replace("(Clone)", "_"+friendId.ToString());
        friendObject.MessageListCanvasGroup = messageListObj.GetComponent<CanvasGroup>();
        friendObject.MessageListRectTrans = messageListObj.GetComponent<RectTransform>();
        FriendList.Add(friendObject);
        return friendObject;
    }


    public static Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
}
public class FriendObject
{
    public bool IsNowChatting;
    public int friendId;
    public GameObject gameObject;
    public Transform transform;
    public Image avatarImage;
    public TextMeshProUGUI name;
    public TextMeshProUGUI lastMessage;
    public CanvasGroup MessageListCanvasGroup;
    public RectTransform MessageListRectTrans;
    public List<MessageObject> MessageList = new List<MessageObject>();
    public void Init(FriendData data)
    {
        friendId = data.FriendId;
        avatarImage.sprite = data.AvatarSprite;
        name.text = data.Name;
    }

    public void UnShow()
    {
        MessageListCanvasGroup.alpha = 0f;
        MessageListCanvasGroup.blocksRaycasts = false;
    }
    public void Show()
    {
        MessageListCanvasGroup.alpha = 1f;
        MessageListCanvasGroup.blocksRaycasts = true;
    }
}
