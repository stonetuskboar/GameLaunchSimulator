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
    public override void Init(LevelManager manger) //在start里开始
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
        levelManager.petController.ShowAtWithTextThenDisappear(position, "您好，我是您的智能助手!<br>您收到了一条信息，请点击蓝色的选项以回复！", 20f);
        levelManager.chatManager.OnMessageSegmentEnd -= Welcome;
    }

    public void OnMessageEnd(int segmentId)
    {
        if(segmentId == 1)
        {
            Vector3 position = levelManager.chatManager.ChatController.transform.position;
            position.y += 100f;
            position.x -= 200f;
            StartCoroutine(WaitAndShowPet(0.5f, 3f, position, "检测到压缩文件，点击该文件的图标可自动解压并安装。"));
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
            StartCoroutine(WaitAndShowPet(0.5f, 3f, warn.transform.position + new Vector3(200, 0, 0), "此程序是否正常运行？<br>是——正常运行<br>否——启动兼容性疑难解答<br>哈哈，开个玩笑，这里没有疑难解答，你的程序也没有正常运行。"));
        }
        else
        {
            Color og = blackScreen.color;
            blackScreen.color = GrayColor;
            yield return new WaitForSeconds(0.2f);
            blackScreen.color = og;
            successImage.enabled = true;
            yield return new WaitForSeconds(1f);
            completePlate.AddContent(contentIcons[0],"耗费了   "+ GetTimeStrByFloat(LaunchTime ));
            completePlate.AddContent(contentIcons[1], "弹出了   " + ErrorCount.ToString() + "次报错");
            completePlate.AddContent(contentIcons[2], "击退了   " + Random.Range(120,600).ToString()+"个敌人");
            completePlate.AddContent(contentIcons[3], "摧毁了   " + Random.Range(4, 20).ToString() + "艘战舰");
            completePlate.AddContent(contentIcons[4], "回忆了   " + Random.Range(5, 10).ToString() + "%的童年");
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        IsLaunched = false;
        ErrorCount++;
        warn.UnShowWarn();
        canvas.enabled = false;
        StartCoroutine(WaitAndShowPet(0.5f, 3f, levelManager.deskManager.GetApplicationById(1).transform.position + new Vector3(-50, 100, 0), "您可在浏览器中寻找解决方案。"));
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
            StartCoroutine(WaitAndShowPet(0f,3f,app.transform.position + new Vector3(0, 100, 0), "新安装的游戏会自动出现在这里！双击或鼠标中键可以直接打开，右键或长按可以展开菜单。"));
        }
    }
}
