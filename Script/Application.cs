using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public delegate void BasicDelegate();
public class Application : MonoBehaviour, IPointerClickHandler
{
    public int appId;
    private DesktopManager deskManager;
    public TextMeshProUGUI titleText;
    public Image iconImage;
    public PropertySetting proertySetting;
    public event BasicDelegate OnLaunch = null;


    public void Init(DesktopManager desk, AppData data) //��DesktopManager������
    {
        deskManager = desk;
        appId = data.AppId;
        iconImage.sprite = data.AppSprite;
        titleText.text = data.AppName;
    }

    public void OnPointerClick(PointerEventData eventData) //������������Դ򿪣��������������˫�����м�ֱ�Ӵ򿪣��Ҽ��򿪲˵���򿪡�
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnLaunchInvoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            deskManager?.ShowClickMenu(this);
        }
        else //����Ϊ����������Ļ����
        {
            deskManager?.OnLeftClick(this);
        }
    }
    public DesktopManager GetDeskTopManager()
    {
        return deskManager;
    }

    public void OnLaunchInvoke()
    {
        OnLaunch?.Invoke();
    }

    public void LaunchAddListener(BasicDelegate delega)
    {
        OnLaunch += delega;
    }

    public string GetTitleWithoutNewLine()
    {
        string str = titleText.text.Replace("<br>", "");
        str = str.Replace("\n", "");
        return str;
    }

    public void DestroyThis()
    {
        deskManager.IfExistDeleteApp(this);
        Destroy(gameObject);
    }
}
