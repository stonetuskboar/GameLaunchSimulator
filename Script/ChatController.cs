using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : LayeredCanvas
{
    public Transform FriendAreaTransform;
    public TextMeshProUGUI NowNameTmp;
    public Transform MessageListTransform;
    public Transform MessagePoolTransform;
    public Canvas ChatCanvas;
    public Transform Chat;

    public GameObject ReceiveMessagePrefab;
    public GameObject SendMessagePrefab;
    public GameObject FriendPrefab;
    private MessageSo messageSo;

    private FriendObject nowChatFriend = null; 

    public List<FriendObject> FriendList = new();
    public List<MessageObject> MessageList = new();
    public List<MessageObject> MessagePoolList = new();
    public void Init(MessageSo so)
    {
        messageSo = so;
    }
    public void ChangeChatToFriend(int id)
    {
        ClearAllMessage();
        nowChatFriend = FindFriendObjectById(id);
        NowNameTmp.text = nowChatFriend.name.text;
    }
    public void ChangeChatToFriend(FriendObject friend)
    {
        ClearAllMessage();
        nowChatFriend = friend;
        NowNameTmp.text = nowChatFriend.name.text;
    }

    public void ShowMessage(MessageData data, int friendId)
    {
        MessageObject messageObject = GetMessageObject(data.IsSend);
        messageObject.SetText(data.text);
        if(data.IsSend == false)
        {
            messageObject.SetAvatar(messageSo.GetFriendById(friendId).AvatarSprite);
            UpdateFriend(friendId, data.text);
        }
        else
        {
            messageObject.SetAvatar(messageSo.PlayerData.AvatarSprite);
        }

        MessagePoolList.Remove(messageObject);
        MessageList.Add(messageObject);
        messageObject.transform.SetParent(MessageListTransform);
        messageObject.gameObject.SetActive(true);
    }

    public MessageObject GetMessageObject(bool IsSend)
    {
        MessageObject messageObject = null;
        for (int i = 0; i < MessagePoolList.Count; i++)
        {
            if (MessagePoolList[i].IsSend == IsSend)
            {
                messageObject = MessagePoolList[i];
                return messageObject;
            }
        }
        messageObject = CreateNewMessageObject(IsSend);
        return messageObject;
    }



    public MessageObject CreateNewMessageObject(bool IsSend)
    {
        GameObject obj;
        MessageObject messageObject = new();
        messageObject.SetIsSend(IsSend);
        if (IsSend == true )
        {
            obj = Instantiate(SendMessagePrefab, MessagePoolTransform);
        }
        else
        {
            obj = Instantiate(ReceiveMessagePrefab, MessagePoolTransform);
        }
        obj.SetActive(false);
        messageObject.gameObject = obj;
        messageObject.transform = obj.transform;
        messageObject.avatarImage = RecursiveFindChild(obj.transform, "avatar").GetComponent<Image>();
        messageObject.text = RecursiveFindChild(obj.transform, "contentText").GetComponent<TextMeshProUGUI>();
        MessagePoolList.Add(messageObject);
        return messageObject;
    }

    public void UpdateFriend(int friendId,string text)
    {
        FriendObject friendObject = FindFriendObjectById(friendId);
        if (friendObject != null)
        {
            friendObject.transform.SetSiblingIndex(0);
            friendObject.message.text = text;
        }
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
        if (FindFriendById(id) != -1)
        {
            return FriendList[id];
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

    public FriendObject CreateNewFriendObject(int friendId)
    {
        GameObject obj;
        FriendObject friendObject = new();

        obj = Instantiate(FriendPrefab, FriendAreaTransform);
        friendObject.gameObject = obj;
        friendObject.transform = obj.transform;
        friendObject.avatarImage = RecursiveFindChild(obj.transform,"avatar").GetComponent<Image>();
        friendObject.name = RecursiveFindChild(obj.transform, "NameText").GetComponent<TextMeshProUGUI>();
        friendObject.message = RecursiveFindChild(obj.transform, "MessageText").GetComponent<TextMeshProUGUI>();
        friendObject.Init(messageSo.GetFriendById(friendId));

        FriendList.Add(friendObject);
        return friendObject;
    }
    public void ClearAllMessage()
    {
        for (int i = MessageList.Count - 1; i >= 0; i++)
        {
            PoolAMessageObject(i);
        }
    }

    public void PoolAMessageObject(int index)
    {
        MessageObject messageObject = MessageList[index];
        messageObject.gameObject.SetActive(false);
        MessageList.RemoveAt(index);
        MessagePoolList.Add(messageObject);
        messageObject.transform.SetParent( MessagePoolTransform);
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
    public int friendId;
    public GameObject gameObject;
    public Transform transform;
    public Image avatarImage;
    public TextMeshProUGUI name;
    public TextMeshProUGUI message;

    public void Init(FriendData data)
    {
        friendId = data.FriendId;
        avatarImage.sprite = data.AvatarSprite;
        name.text = data.Name;
    }
}

public class MessageObject
{
    public bool IsSend;
    public GameObject gameObject;
    public Transform transform;
    public Image avatarImage;
    public TextMeshProUGUI text;

    public void SetIsSend (bool isSend)
    {
        this.IsSend = isSend;
    }

    public void SetText(string str)
    {
        text.text = str;
    }
    public void SetAvatar(Sprite sprite)
    {
        avatarImage.sprite = sprite;
    }

}
