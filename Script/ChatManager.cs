using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public ChatController ChatController;
    public MessageSo messageSo;

    public int NextSegmentId = 0;

    public void Start()
    {
        ChatController.Init(messageSo);
        ShowChat();
        StartShowMessageSegment();
    }

    public void ShowChat()
    {
        ChatController.Show();
    }

    public void StartShowMessageSegment()
    {
        StartCoroutine(ShowMessageSegment());
    }

    IEnumerator ShowMessageSegment()
    {
        MessageSegment segment = messageSo.GetSegmentById(NextSegmentId);
        if(segment == null)
        {
            yield break;
        }
        int friendId = segment.FriendId;
        ChatController.IfNewAddFriend(friendId);
        ChatController.ChangeChatToFriend(friendId);
        List <MessageData> messages = segment.messages;
        if(messages.Count > 0)
        {
            ChatController.ShowMessage(messages[0], friendId);
        }
        for (int i = 1; i <segment.messages.Count; i++)
        {
            float basicLength = Mathf.Sqrt(messages[i].text.Length / 4f) + Mathf.Sqrt(messages[i-1].text.Length / 4f) /2;
            float waitTime = basicLength * Random.Range(0.5f,1.5f);
            float time = 0f;

            while (time < waitTime)
            {
                yield return null;
                time += Time.deltaTime;
            }
            ChatController.ShowMessage(messages[i], friendId);
        }
        NextSegmentId++;
    }
}
