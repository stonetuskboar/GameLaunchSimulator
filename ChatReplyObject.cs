using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatReplyObject : MonoBehaviour
{
    private ChatReplyData nowReplyData;
    private FriendObject nowFriendObject;
    public TextMeshProUGUI replyText;
    public Button button;
    public ChatManager chatManager;

    public void Awake()
    {
        UnShowReply();
        button.onClick.AddListener(OnButtonClick);
    }

    public void Init(ChatManager manager)
    {
        chatManager = manager;
    }
    public void SetReply(ChatReplyData data,FriendObject friend)
    {
        nowFriendObject = friend;
        nowReplyData = data;
        replyText.text = chatManager.messageSo.GetMessageById(nowReplyData.messageId).text;
    }
    public void SetReply(ChatReplyData data)
    {
        nowReplyData = data;
        replyText.text = chatManager.messageSo.GetMessageById(nowReplyData.messageId).text;
    }

    public void ShowReply()
    {
        gameObject.SetActive(true);
    }

    public void UnShowReply()
    {
        gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        nowFriendObject.replyDataList = null;
        chatManager.ChatController.UnshowAllReply();
        LevelManager.instance.OnChatReplyClick(nowReplyData.replyId);
        chatManager.AfterReplyClick(nowReplyData.nextSegmentId ,nowReplyData.messageId, nowFriendObject);
    }
}
