using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3 : BasicLevel
{
    public Canvas launchCanvas;
    public Image gameImage;
    public LaunchMenu launchMenu;
    public PropertySetting CorrectSetting;

    private float LaunchTime = 0;
    private int ErrorCount = 0;
    public override void Awake()
    {
        base.Awake();
        launchCanvas.worldCamera = Camera.main;
        completePlate.button.onClick.AddListener(OnCompleteLevel);
        launchMenu.LaunchButton.onClick.AddListener(LaunchGame);
        Reset();
    }
    public override void Init(LevelManager manger)
    {
        base.Init(manger);
        levelManager.explorerManager.ShowTiebaById(2);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(4);
    }

    public void OpenLaunchMenu()
    {
        launchMenu.Show();
    }

    public void LaunchGame()
    {
        if (IsLaunched == false)
        {
            IsLaunched = true;
            StartCoroutine(ShowBlackScreen());
        }
    }
    public void Update()
    {
        LaunchTime += Time.deltaTime;
    }
    IEnumerator ShowBlackScreen()
    {
        yield return new WaitForSeconds(0.5f);
        canvas.enabled = true;
        if (CheckLevelSuccess() == false)
        {
            gameImage.enabled = true;
            yield return new WaitForSeconds(0.05f);
            Quit();
        }
        else
        {
            gameImage.enabled = true;
            yield return new WaitForSeconds(1f);
            completePlate.AddContent(contentIcons[0], "耗费了   " + GetTimeStrByFloat(LaunchTime));
            completePlate.AddContent(contentIcons[1], "出现了   " + ErrorCount.ToString() + "次闪退");
            completePlate.AddContent(contentIcons[2], "收集了   " + Random.Range(100, 200).ToString() + "个瓶盖");
            completePlate.AddContent(contentIcons[3], "找到了   " + "0个爸爸");
            completePlate.AddContent(contentIcons[4], "回忆了   " + Random.Range(2, 6).ToString() + "%的童年");
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        StartCoroutine(WaitAndShowPet(0.5f, 3f, new Vector3(0, 0, 0),
            "此程序可能没有正常运行。<br>请联系你的软件提供商获取在此版本系统上运行的最新版本。<br>哈哈，开个玩笑，你肯定联系不到。"));
        ErrorCount++;
        Reset();
    }

    public void Reset()
    {
        IsLaunched = false;
        gameImage.enabled = false;
        canvas.enabled = false;
        launchMenu.UnShow();
    }
    public override void OnCompleteLevel()
    {
        levelManager.LoadLevelById(4);
    }
    public bool CheckLevelSuccess()
    {
        PropertySetting setting = GetFirstApp().proertySetting;
        if (setting.IsCompatibility != CorrectSetting.IsCompatibility ||
            (int)setting.compatMode != (int)CorrectSetting.compatMode
            )
        {
            return false;
        }
        else if (setting.IsRunByAdmin != CorrectSetting.IsRunByAdmin)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void OnHyperLinkClick(string link)
    {
        if (link == "fo3" && CheckIfAppExist(4) == false)
        {
            AddApps();
            Application app = GetAppById(4);
            app.LaunchAddListener(OpenLaunchMenu);
        }
    }
}
