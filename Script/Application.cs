using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public delegate void BasicDelegate();
public class Application : MonoBehaviour, IPointerClickHandler
{
    public int iconId;
    private DesktopManager deskManager;
    public TextMeshProUGUI titleText;
    public Image iconImage;
    public PropertySetting proertySetting;
    public event BasicDelegate OnLaunch;

    public void Start()
    {
        //deskManager = FindFirstObjectByType<DesktopManager>();
    }

    public void Init(DesktopManager desk) //在DesktopManager处触发
    {
        deskManager = desk;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(eventData.button);
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnLaunchClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            deskManager.ShowClickMenu(this);
        }
        else //可能为鼠标左键或屏幕触控
        {
            deskManager.OnLeftClick(this);
        }
    }
    public DesktopManager GetDeskTopManager()
    {
        return deskManager;
    }

    public void OnLaunchClick()
    {
        OnLaunch?.Invoke();
    }

    public string GetTitleWithoutNewLine()
    {
        string str = titleText.text.Replace("<br>", "");
        str = str.Replace("\n", "");
        return str;
    }
}
