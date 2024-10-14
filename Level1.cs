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
    public override void Awake()
    {
        base.Awake();
        warn.button.onClick.AddListener(Quit);
        successImage.enabled = false;
        canvas.enabled = false;
        completePlate.button.onClick.AddListener(OnCompleteLevel);
    }
    public override void Init(LevelManager manger) //在start里开始
    {
        base.Init(manger);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(0,ShowChatGuide);
        levelManager.petController.ShowAtWithTextThenDisappear(new Vector3(0,0,0), "欢迎！<br>您的开机速度已经打败了99%的家用电器！",5f);
    }

    public void LaunchGame()
    {
        StartCoroutine(ShowBlackScreen());
    }

    public void ShowChatGuide()
    {
        StartCoroutine(WaitAndShow(1f));
    }
    IEnumerator WaitAndShow(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 position = levelManager.chatManager.ChatController.transform.position;
        position.y += 100f;
        position.x -= 200f;
        levelManager.petController.ShowAtWithTextThenDisappear(position, "检测到可安装文件，点击后将自动安装！", 3f);
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
            warn.SetWarn(warnSo.WarnDatas[1]);
            warn.ShowWarn();
        }
        else
        {
            Color og = blackScreen.color;
            blackScreen.color = GrayColor;
            yield return new WaitForSeconds(0.2f);
            blackScreen.color = og;
            successImage.enabled = true;
            yield return new WaitForSeconds(1f);
            completePlate.StartAppear();

        }
    }
    public void Quit()
    {
        warn.UnShowWarn();
        canvas.enabled = false;
    }
    public override void OnCompleteLevel()
    {
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
            return false;
        }
    }

    public override void OnHyperLinkClick(string link)
    {
        if(link == "beachHead2000" && CheckIfAppExist(2) == false)
        {
            AddApps();
            Application app = GetAppById(2);
            app.LaunchAddListener(LaunchGame);
            warn.Init(app);
            levelManager.petController.ShowAtWithTextThenDisappear(app.transform.position + new Vector3(-50,100,0), "新安装的游戏会自动出现在这里！", 3f);
        }
    }
}
