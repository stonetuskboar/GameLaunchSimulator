using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public ChatController ChatController;
    public MessageSo messageSo;

    public void Awake()
    {
        ChatController.Init(messageSo);
    }

    public void ShowChat()
    {
        ChatController.Show();
    }
    public void LoadData()
    {

    }
    public void StartShowMessageSegment(int segmentId)
    {
        StartCoroutine(ShowMessageSegment(segmentId, VoidDelegate));
    }

    public void VoidDelegate()
    {
        //没有回调
    }

    public void StartShowMessageSegment(int segmentId, BasicDelegate callBack)
    {
        StartCoroutine(ShowMessageSegment(segmentId,callBack));
    }

    IEnumerator ShowMessageSegment(int segmentId, BasicDelegate callBack)
    {
        MessageSegment segment = messageSo.GetSegmentById(segmentId);
        if(segment == null)
        {
            yield break;
        }
        ChatController.IfNewAddFriend(segment.FriendId);
        FriendObject friend = ChatController.SwitchChatToFriend(segment.FriendId);
        List<int> messages = segment.messageIdList;

        if (messages.Count > 0)
        {
            MessageData data0;
            MessageData data1 = messageSo.GetMessageById(messages[0]);
            ChatController.AddMessage(data1, friend);
            for (int i = 1; i < messages.Count; i++)
            {
                data0 = data1;
                data1 = messageSo.GetMessageById(messages[i]);
                float basicLength = Mathf.Sqrt(data1.text.Length / 8f + data0.text.Length / 16f);
                float waitTime = basicLength * Random.Range(0.7f, 1.3f);
                float time = 0f;
                while (time < waitTime)
                {
                    yield return null;
                    time += Time.deltaTime;
                }
                ChatController.AddMessage(data1, friend);
            }
        }

        callBack?.Invoke();
    }
}
