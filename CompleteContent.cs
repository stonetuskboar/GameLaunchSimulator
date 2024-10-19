using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompleteContent : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI text;

    public void SetIconText(ContentIcon icon, string str)
    {
        SetIconText(icon.sprite, icon.color, str);
    }
    public void SetIconText(Sprite sprite,Color color, string str)
    {
        Icon.sprite = sprite;
        Icon.color = color; 
        text.text = str;
    }
}

[Serializable]
public class ContentIcon //在basiclevel处设置。 每个关卡有不同的过关提示
{
    public Sprite sprite;
    public Color color = Color.white;
}

