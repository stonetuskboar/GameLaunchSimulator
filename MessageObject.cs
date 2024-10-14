using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MessageObject : MonoBehaviour,IPointerClickHandler
{
    public bool IsSend;
    public int messageId;
    public Image avatarImage;
    public TextMeshProUGUI text;


    public void SetText(string str)
    {
        text.text = str;
    }
    public void SetAvatar(Sprite sprite)
    {
        avatarImage.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, Camera.main);
        if(linkIndex >= 0)
        {
            string linkId = text.textInfo.linkInfo[linkIndex].GetLinkID();
            LevelManager.instance.OnHyperLinkClick(linkId);
        }
    }
}
