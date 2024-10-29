using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level4 : BasicLevel
{
    public Image BlackScreenImage;
    public Image gameImage;
    public Image successImage;
    public Canvas blackBackGroundCanvas;
    public WarnSo warnSo;
    public Warn warn;
    public PropertySetting startSetting;
    public PropertySetting CorrectSetting;

    public List<string> CorrectPropertyTextList;
    public List<string> propertyTextReplaceList;
    private List<Sprite> ogSpriteList = new();
    private float LaunchTime = 0;
    private int ErrorCount = 0;
    private bool isOnVirus = false;
    public override void Awake()
    {
        base.Awake();
        blackBackGroundCanvas.worldCamera = Camera.main;
        Reset();
        warn.button.onClick.AddListener(Quit);
        completePlate.button.onClick.AddListener(OnCompleteLevel);
    }
    public override void Init(LevelManager manger) //在start里开始
    {
        base.Init(manger);
        levelManager.explorerManager.ShowTiebaById(3);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(7, 0.5f);
        levelManager.chatManager.OnMessageSegmentEnd += OnMessageEnd;
    }

    public void Update()
    {
        LaunchTime += Time.deltaTime;
    }

    public void LaunchGame()
    {
        if (IsLaunched == false)
        {
            IsLaunched = true;
            StartCoroutine(ShowBlackScreen());
        }
    }

    public void OnMessageEnd(int segmentId)
    {
        if (segmentId == 8)
        {
            Vector3 position = levelManager.chatManager.ChatController.transform.position;
            position.y += 100f;
            position.x -= 200f;
            StartCoroutine(WaitAndShowPet(0.5f, 3f, position, "检测到可执行文件，此文件类型可能产生危害。"));
        }
        if (segmentId == 9)
        {
            StartCoroutine(ChangeComputer());
        }
    }

    public IEnumerator ChangeComputer()
    {
        yield return new WaitForSeconds(1f);
        canvas.enabled = true;
        BlackScreenImage.enabled = true;
        AddApps();
        App app = GetAppById(3);
        app.LaunchAddListener(LaunchGame);
        warn.Init(app);

        Property property = levelManager.deskManager.property;

        app.proertySetting = startSetting;
        property.SettingButtonForLevel4();

        CorrectPropertyTextList = property.textStringList;
        for (int i = 0; i < property.textList.Count; i++)
        {
            property.textList[i].text = propertyTextReplaceList[i];
        }
        yield return new WaitForSeconds(1f);
        canvas.enabled = false;
        BlackScreenImage.enabled = false;
    }

    IEnumerator ShowBlackScreen()
    {
        blackBackGroundCanvas.enabled = true;
        CameraController camController = levelManager.cameraController;
        canvas.enabled = true;
        camController.MultiSizeFixedLeftTop(0.5f);
        yield return new WaitForSeconds(0.5f);

        if (CheckLevelSuccess() == false)
        {
            gameImage.enabled = true;
            yield return new WaitForSeconds(0.7f);
            gameImage.enabled = false;
            yield return new WaitForSeconds(0.3f);
            camController.BackToOgSizeWhenFixedLeftTop();

            yield return new WaitForSeconds(0.2f);

            camController.MultiSizeFixedLeftTop(2f);

            yield return new WaitForSeconds(0.5f);
            BlackScreenImage.enabled = true;
            yield return new WaitForSeconds(0.5f);
            warn.ShowWarn();
        }
        else
        {
            camController.BackToOgSizeWhenFixedLeftTop();
            gameImage.enabled = true;
            yield return new WaitForSeconds(1f);
            Color color = successImage.color;
            color.a = 0f;
            successImage.color = color;
            successImage.enabled = true;
            float time = 0f;
            while (time < 2f)
            {
                time += Time.deltaTime;
                color.a = time / 1f;
                successImage.color = color;
                yield return null;
            }
            completePlate.AddContent(contentIcons[0], "耗费了   " + GetTimeStrByFloat(LaunchTime));
            completePlate.AddContent(contentIcons[1], "弹出了   " + ErrorCount.ToString() + "次报错");
            completePlate.AddContent(contentIcons[4], "感染了   " + Random.Range(4, 15).ToString() + "%的文件");
            completePlate.AddContent(contentIcons[4], "感染了   " + Random.Range(4, 15).ToString() + "%的文件");
            completePlate.AddContent(contentIcons[4], "感染了   " + Random.Range(4, 15).ToString() + "%的文件");
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        ErrorCount++;
        levelManager.cameraController.BackToOgSizeWhenFixedLeftTop();
        Reset();
    }
    public void Reset()
    {
        IsLaunched = false;
        warn.UnShowWarn();
        successImage.enabled = false;
        gameImage.enabled = false;
        canvas.enabled = false;
        blackBackGroundCanvas.enabled = false;
        BlackScreenImage.enabled = false;
    }
    public override void OnCompleteLevel()
    {
        Property property = levelManager.deskManager.property;
        for (int i = 0; i < property.textList.Count; i++)
        {
            property.textList[i].text = CorrectPropertyTextList[i];
        }
        levelManager.deskManager.property.SettingButton();
        levelManager.chatManager.OnMessageSegmentEnd -= OnMessageEnd;
        levelManager.LoadLevelById(5);
    }
    public bool CheckLevelSuccess()
    {
        PropertySetting setting = GetFirstApp().proertySetting;
        if (setting.IsCompatibility != CorrectSetting.IsCompatibility)
        {
            warn.SetWarn(warnSo.WarnDatas[3]);
            return false;
        }
        else if (setting.IsfullScreenOptim != CorrectSetting.IsfullScreenOptim)
        {
            warn.SetWarn(warnSo.WarnDatas[2]);
            return false;
        }
        else if ( setting.IsLevel4Equal(CorrectSetting) != true)
        {
            warn.SetWarn(warnSo.WarnDatas[5]);
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void OnHyperLinkClick(string link)
    {
        if (link == "aoe2Virus" && isOnVirus == false)
        {
            isOnVirus = true;
            levelManager.chatManager.StartShowMessageSegment(9, 0.5f);

        }
    }
}
