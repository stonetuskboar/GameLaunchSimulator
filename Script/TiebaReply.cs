using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TiebaReply : MonoBehaviour
{
    public Image avatar;
    public TextMeshProUGUI replyText;

    public void SetReply(TiebaReplyData data)
    {
        avatar.sprite = data.avatar;
        replyText.text = data.replyText;
    }
    public void SetReply(Sprite avatarSprite, string text)
    {
        avatar.sprite = avatarSprite;
        replyText.text = text;
    }

    public void SetReplyThenActive(TiebaReplyData data)
    {
        SetReply(data);
        gameObject.SetActive(true);
    }
}
