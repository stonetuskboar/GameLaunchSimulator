using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public delegate void MessageDelegate(int segmentId);
public class ChatManager : MonoBehaviour
{
    public ChatController ChatController;
    public MessageSo messageSo;
    public event MessageDelegate OnMessageSegmentEnd;

    public void Awake()
    {
        ChatController.Init(this,messageSo);
    }

    public void ShowChat()
    {
        ChatController.Show();
    }
    public void LoadData()
    {

    }


    public void AfterReplyClick(int segmentId ,int messageId, FriendObject friend)
    {
        ChatController.AddMessage(messageId, friend);
        MessageSegment segment = messageSo.GetSegmentById(segmentId);
        float waitTime;
        if (segment == null || segment.messageIdList == null || segment.messageIdList.Count <= 0 )
        {
            waitTime = 0f;
        }
        else
        {
            MessageData data0 = messageSo.GetMessageById(messageId);
            MessageData data1 = messageSo.GetMessageById(segment.messageIdList[0]);

            float basicLength = Mathf.Sqrt(data1.text.Length / 4f + data0.text.Length / 8f);
            waitTime = basicLength * Random.Range(0.7f, 1.3f);
        }
        StartShowMessageSegment(segmentId, waitTime);
    }
    public void StartShowMessageSegment(int segmentId)
    {
        StartCoroutine(ShowMessageSegment(segmentId , 0f));
    }
    public void StartShowMessageSegment(int segmentId, float firstMessageWaitTime)
    {
        StartCoroutine(ShowMessageSegment(segmentId, firstMessageWaitTime));
    }
    IEnumerator ShowMessageSegment(int segmentId,float firstMessageWaitTime)
    {
        MessageSegment segment = messageSo.GetSegmentById(segmentId);
        if(segment == null || segment.messageIdList == null || segment.messageIdList.Count <= 0)
        {
            yield break;
        }
        yield return new WaitForSeconds(firstMessageWaitTime);

        ChatController.IfNewAddFriend(segment.FriendId);
        FriendObject friend = ChatController.FindFriendObjectById(segment.FriendId);

        List<int> messages = segment.messageIdList;
        if (messages.Count > 0)
        {
            ChatController.SwitchChatToFriend(friend); //在这个方法里，会执行UnshowAllReply();如果Friend对象的replylist没有元素，就不显示reply
            MessageData data0;
            MessageData data1 = messageSo.GetMessageById(messages[0]);
            ChatController.AddMessage(data1, friend);
            for (int i = 1; i < messages.Count; i++)
            {
                data0 = data1;
                data1 = messageSo.GetMessageById(messages[i]);
                float basicLength = Mathf.Sqrt(data1.text.Length / 8f + data0.text.Length / 16f);
                float waitTime = basicLength * Random.Range(0.7f, 1.3f);
                yield return new WaitForSeconds(waitTime);
                ChatController.AddMessage(data1, friend);
            }
        }
        friend.replyDataList = segment.replyList;
        ChatController.ShowReplysByFriend(friend);

        OnMessageSegmentEnd?.Invoke(segmentId);
    }
}
