using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : BasicLevel
{
    public Image blackScreen;
    public Image successImage;
    public Color GrayColor;
    public WarnSo warnSo;
    public Warn warn;
    public PropertySetting CorrectSetting;

    private float LaunchTime = 0;
    private int ErrorCount = 0;
    public override void Awake()
    {
        base.Awake();
        warn.UnShowWarn();
        warn.button.onClick.AddListener(Quit);
        successImage.enabled = false;
        canvas.enabled = false;
        completePlate.button.onClick.AddListener(OnCompleteLevel);
    }
    public override void Init(LevelManager manger) //��start�￪ʼ
    {
        base.Init(manger);
        levelManager.chatManager.ShowChat();
        levelManager.explorerManager.ShowTiebaById(0);
        levelManager.chatManager.StartShowMessageSegment(0,0.5f);
        levelManager.chatManager.OnMessageSegmentEnd += OnMessageEnd;
        levelManager.chatManager.OnMessageSegmentEnd += Welcome;
    }

    public void Update()
    {
        LaunchTime += Time.deltaTime;
    }

    public void LaunchGame()
    {
        if(IsLaunched == false)
        {
            IsLaunched = true;
            StartCoroutine(ShowBlackScreen());
        }
    }
    public void Welcome(int segmentId)
    {
        Vector3 position = DesktopManager.GetRectBottomRightPosition(levelManager.chatManager.ChatController.ChatRectTrans);
        levelManager.petController.ShowAtWithTextThenDisappear(position, "���ã�����������������!<br>���յ���һ����Ϣ��������ɫ��ѡ���Իظ���", 20f);
        levelManager.chatManager.OnMessageSegmentEnd -= Welcome;
    }

    public void OnMessageEnd(int segmentId)
    {
        if(segmentId == 1)
        {
            Vector3 position = levelManager.chatManager.ChatController.transform.position;
            position.y += 100f;
            position.x -= 200f;
            StartCoroutine(WaitAndShowPet(0.5f, 3f, position, "��⵽ѹ���ļ���������ļ���ͼ����Զ���ѹ����װ��"));
        }
    }


    IEnumerator ShowBlackScreen()
    {
        canvas.enabled = true;

        yield return new WaitForSeconds(1f);
        if (CheckLevelSuccess() == false) 
        {
            Color og = blackScreen.color;
            blackScreen.color = GrayColor;
            yield return new WaitForSeconds(0.2f);
            blackScreen.color = og;
            yield return new WaitForSeconds(1f);
            warn.ShowWarn();
            StartCoroutine(WaitAndShowPet(0.5f, 3f, warn.transform.position + new Vector3(200, 0, 0), "�˳����Ƿ��������У�<br>�ǡ�����������<br>�񡪡��������������ѽ��<br>������������Ц������û�����ѽ����ĳ���Ҳû���������С�"));
        }
        else
        {
            Color og = blackScreen.color;
            blackScreen.color = GrayColor;
            yield return new WaitForSeconds(0.2f);
            blackScreen.color = og;
            successImage.enabled = true;
            yield return new WaitForSeconds(1f);
            completePlate.AddContent(contentIcons[0],"�ķ���   "+ GetTimeStrByFloat(LaunchTime ));
            completePlate.AddContent(contentIcons[1], "������   " + ErrorCount.ToString() + "�α���");
            completePlate.AddContent(contentIcons[2], "������   " + Random.Range(120,600).ToString()+"������");
            completePlate.AddContent(contentIcons[3], "�ݻ���   " + Random.Range(4, 20).ToString() + "��ս��");
            completePlate.AddContent(contentIcons[4], "������   " + Random.Range(5, 10).ToString() + "%��ͯ��");
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        IsLaunched = false;
        ErrorCount++;
        warn.UnShowWarn();
        canvas.enabled = false;
        StartCoroutine(WaitAndShowPet(0.5f, 3f, levelManager.deskManager.GetApplicationById(1).transform.position + new Vector3(-50, 100, 0), "�������������Ѱ�ҽ��������"));
    }
    public override void OnCompleteLevel()
    {
        levelManager.chatManager.OnMessageSegmentEnd -= OnMessageEnd;
        levelManager.chatManager.OnMessageSegmentEnd -= Welcome;
        levelManager.LoadLevelById(2);
    }
    public bool CheckLevelSuccess()
    {
        PropertySetting setting = GetFirstApp().proertySetting;
        if (setting.IsCompatibility == CorrectSetting.IsCompatibility
            && setting.compatMode == CorrectSetting.compatMode)
        {
            return true;
        }
        else
        {
            warn.SetWarn(warnSo.WarnDatas[1]);
            return false;
        }
    }
    public override void OnChatReplyClick(int replyId)
    {
        levelManager.petController.StartUnShow();
    }

    public override void OnHyperLinkClick(string link)
    {
        if(link == "beachHead2000" && CheckIfAppExist(2) == false)
        {
            AddApps();
            App app = GetAppById(2);
            app.LaunchAddListener(LaunchGame);
            warn.Init(app);
            StartCoroutine(WaitAndShowPet(0f,3f,app.transform.position + new Vector3(0, 100, 0), "�°�װ����Ϸ���Զ����������˫��������м�����ֱ�Ӵ򿪣��Ҽ��򳤰�����չ���˵���"));
        }
    }
}
