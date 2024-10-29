
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Level_8 : BasicLevel
{
    public Canvas launchCanvas;
    public LaunchMenu launchMenu;

    public GameObject ErrorBackGroundObj;
    public Warn warn;
    public WarnSo warnSo;
    public Image gameImage;
    public Image LoadingBar;
    public TextMeshProUGUI LoadingText;
    public List<LoadingText> LoadingTextList;
    private float LaunchTime = 0;
    private int ErrorCount = 0;
    public PropertySetting startSetting;
    public PropertySetting CorrectSetting;
    public override void Awake()
    {
        base.Awake();
        warn.button.onClick.AddListener(Quit);
        launchCanvas.worldCamera = Camera.main;
        completePlate.button.onClick.AddListener(OnCompleteLevel);
        launchMenu.LaunchButton.onClick.AddListener(LaunchGame);
        warn.EnableShadow();
        Reset();
    }
    public override void Init(LevelManager manger)
    {
        base.Init(manger);
        levelManager.explorerManager.ShowTiebaById(7);
        levelManager.explorerManager.AddPatchHomeController();
        levelManager.explorerManager.GetPatchHomeController().AllClearToUndownload();
        levelManager.explorerManager.AddPageById(2);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(18);
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
            StartCoroutine(LoadingGame());
        }
    }
    public void Update()
    {
        LaunchTime += Time.deltaTime;
    }
    IEnumerator LoadingGame()
    {
        yield return new WaitForSeconds(0.5f);

        canvas.enabled = true;
        patchHomeController patchHomeController = levelManager.explorerManager.GetPatchHomeController();
        PropertySetting setting = GetFirstApp().proertySetting;

        float percent = 0;
        for (float time = 0; time < 2f; time += Time.deltaTime)
        {
            SetTextByPercent(percent, percent + Time.deltaTime / 10);
            percent += Time.deltaTime / 10;
            LoadingBar.fillAmount = percent;
            yield return null;
        }
        if (setting.IsCompatibility != CorrectSetting.IsCompatibility || setting.compatMode != CorrectSetting.compatMode)
        {
            warn.SetWarn(warnSo.WarnDatas[3]);
            ErrorBackGroundObj.SetActive(true);
            warn.ShowWarn();
            yield break;
        }
        else if (setting.IsLowResolution != CorrectSetting.IsLowResolution)
        {
            warn.SetWarn(warnSo.WarnDatas[7]);
            ErrorBackGroundObj.SetActive(true);
            warn.ShowWarn();
            yield break;
        }

        for (float time = 0; time < 2f; time += Time.deltaTime)
        {
            SetTextByPercent(percent, percent + Time.deltaTime / 5);
            percent += Time.deltaTime / 5;
            LoadingBar.fillAmount = percent;
            yield return null;
        }

        if (true != patchHomeController.CheckIfDownload(8))
        {
            warn.SetWarn(warnSo.WarnDatas[6]);
            ErrorBackGroundObj.SetActive(true);
            warn.ShowWarn();
            yield break;
        }

        for (float time = 0; time < 4f; time += Time.deltaTime)
        {
            SetTextByPercent(percent, percent + Time.deltaTime / 20);
            percent += Time.deltaTime / 20;
            LoadingBar.fillAmount = percent;
            yield return null;
        }

        if (true != patchHomeController.CheckIfDownload(4))
        {
            warn.SetWarn(warnSo.WarnDatas[8]);
            ErrorBackGroundObj.SetActive(true);
            warn.ShowWarn();
            yield break;
        }
        while(percent < 1f)
        {
            SetTextByPercent(percent, percent + Time.deltaTime / 5);
            percent += Time.deltaTime / 5;
            LoadingBar.fillAmount = percent;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        gameImage.enabled = true;
        yield return new WaitForSeconds(1f);
        completePlate.AddContent(contentIcons[0], "耗费了   " + GetTimeStrByFloat(LaunchTime));
        completePlate.AddContent(contentIcons[1], "出现了   " + ErrorCount.ToString() + "次闪退");
        completePlate.AddContent(contentIcons[2], "解决了   " + "刁民投诉");
        completePlate.AddContent(contentIcons[3], "处理了   " + "污水排放");
        completePlate.AddContent(contentIcons[4], "恭喜你，这是最后一关了");
        completePlate.StartAppear();
    }


    public void Quit()
    {
        ErrorCount++;
        levelManager.explorerManager.GetPatchHomeController().AllClearToUndownload();
        Reset();
    }

    public void Reset()
    {
        ErrorBackGroundObj.SetActive(false);
        warn.UnShowWarn();
        IsLaunched = false;
        canvas.enabled = false;
        launchMenu.UnShow();
        gameImage.enabled = false;
        LoadingBar.fillAmount = 0;
        LoadingText.text = "";
    }


    public override void OnCompleteLevel()
    {
        levelManager.LoadLevelById(9);
    }

    public override void OnHyperLinkClick(string link)
    {
        if (link == "CitySkyline2" && CheckIfAppExist(6) == false)
        {
            AddApps();
            App app = GetAppById(6);
            warn.Init(app);
            app.LaunchAddListener(OpenLaunchMenu);
            GetFirstApp().proertySetting = startSetting;
        }
    }

    public void SetTextByPercent(float beforePercent, float percent)
    {
        for(int i = 0; i < LoadingTextList.Count; i++)
        {
            if(LoadingTextList[i].Percent >= beforePercent)
            {
                if(LoadingTextList[i].Percent <= percent)
                {
                    LoadingText.text = LoadingTextList[i].text;
                }
                break;
            }
        }
    }
}

[System.Serializable]
public class LoadingText
{
    public float Percent;
    public string text;
}