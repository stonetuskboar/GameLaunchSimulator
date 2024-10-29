using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public delegate void BasicDelegate();
public class App : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public int appId;
    private DesktopManager deskManager;
    public TextMeshProUGUI titleText;
    public Image iconImage;
    public PropertySetting proertySetting;
    public event BasicDelegate OnLaunchEvent = null;
    private float holdTime = 0f;
    private bool isHold = false;
    public void Init(DesktopManager desk, AppData data) //��DesktopManager������
    {
        transform.name = "App_" + data.AppName;
        deskManager = desk;
        appId = data.AppId;
        iconImage.sprite = data.AppSprite;
        titleText.text = data.AppName;
    }
    public void Update()
    {
        if(isHold == true)
        {
            holdTime += Time.deltaTime;
            if(holdTime > 0.5f)
            {
                deskManager?.ShowClickMenu(this);
                HoldReset();
            }
        }
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
            deskManager.ClickCheck(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Middle 
            && eventData.button != PointerEventData.InputButton.Right)
        {
            isHold = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HoldReset();
    }

    private void HoldReset()
    {
        isHold = false;
        holdTime = 0;
    }
    public DesktopManager GetDeskTopManager()
    {
        return deskManager;
    }

    public void OnLaunchInvoke()
    {
        AudioManager.Instance.PlaySfxByName("Open");
        OnLaunchEvent?.Invoke();
    }

    public void LaunchAddListener(BasicDelegate delega)
    {
        OnLaunchEvent += delega;
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
