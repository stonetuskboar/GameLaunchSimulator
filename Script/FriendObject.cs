using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendObject : BasicSelectLabel
{ 
    public int friendId;
    public GameObject labelObject;
    public Transform labelTransform;
    public Image avatarImage;
    public TextMeshProUGUI friendName;
    public TextMeshProUGUI lastMessage;
    public CanvasGroup MessageListCanvasGroup;
    public RectTransform MessageListRectTrans;
    public List<MessageObject> MessageList = new();
    public List<ChatReplyData> replyDataList = null;
    private ChatController chatController;
    public void Init(ChatController controller, FriendData data)
    {
        chatController = controller;
        labelTransform.name = "FriendLabel_"+ data.Name;
        friendId = data.FriendId;
        avatarImage.sprite = data.AvatarSprite;
        friendName.text = data.Name;
    }

    public void UnShow()
    {
        IsShow = false;
        MessageListCanvasGroup.alpha = 0f;
        MessageListCanvasGroup.blocksRaycasts = false;
        ChangeTitleColor(UnShowTitleColor);

    }
    public void Show()
    {
        IsShow = true;
        MessageListCanvasGroup.alpha = 1f;
        MessageListCanvasGroup.blocksRaycasts = true;
        ChangeTitleColor(ShowTitleColor);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        chatController.SwitchChatToFriend(this);
    }
}