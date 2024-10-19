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
            completePlate.AddContent(contentIcons[0], "�ķ���   " + GetTimeStrByFloat(LaunchTime));
            completePlate.AddContent(contentIcons[1], "������   " + ErrorCount.ToString() + "������");
            completePlate.AddContent(contentIcons[2], "�ռ���   " + Random.Range(100, 200).ToString() + "��ƿ��");
            completePlate.AddContent(contentIcons[3], "�ҵ���   " + "0���ְ�");
            completePlate.AddContent(contentIcons[4], "������   " + Random.Range(2, 6).ToString() + "%��ͯ��");
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        StartCoroutine(WaitAndShowPet(0.5f, 3f, new Vector3(0, 0, 0),
            "�˳������û���������С�<br>����ϵ�������ṩ�̻�ȡ�ڴ˰汾ϵͳ�����е����°汾��<br>������������Ц����϶���ϵ������"));
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
