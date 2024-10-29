using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Level2 : BasicLevel
{
    public Image BlackScreenImage;
    public Image gameImage;
    public Image successImage;
    public Canvas blackBackGroundCanvas;
    public WarnSo warnSo;
    public Warn warn;
    public PropertySetting startSetting;
    public PropertySetting CorrectSetting;

    private float LaunchTime = 0;
    private int ErrorCount = 0;
    public override void Awake()
    {
        base.Awake();
        Reset();
        warn.button.onClick.AddListener(Quit);
        completePlate.button.onClick.AddListener(OnCompleteLevel);
    }
    public override void Init(LevelManager manger)
    {
        base.Init(manger);
        blackBackGroundCanvas.worldCamera = Camera.main;
        levelManager.explorerManager.ShowTiebaById(1);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(2);
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
            while(time < 2f)
            {
                time += Time.deltaTime;
                color.a = time / 1f;
                successImage.color = color;
                yield return null;
            }
            completePlate.AddContent(contentIcons[0], "�ķ���   " + GetTimeStrByFloat(LaunchTime));
            completePlate.AddContent(contentIcons[1], "������   " + ErrorCount.ToString() + "�α���");
            completePlate.AddContent(contentIcons[2], "������   " + Random.Range(20, 40).ToString() + "��ĥ��");
            completePlate.AddContent(contentIcons[3], "������   " + Random.Range(200, 600).ToString() + "Ƭũ��");
            completePlate.AddContent(contentIcons[4], "������   " + Random.Range(5, 10).ToString() + "%��ͯ��");
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
        levelManager.LoadLevelById(3);
    }
    public bool CheckLevelSuccess()
    {
        PropertySetting setting = GetFirstApp().proertySetting;
        if (setting.IsCompatibility != CorrectSetting.IsCompatibility)
        {
            warn.SetWarn(warnSo.WarnDatas[3]);
            return false;
        }
        else if( setting.IsfullScreenOptim != CorrectSetting.IsfullScreenOptim)
        {
            warn.SetWarn(warnSo.WarnDatas[2]);
            return false;
        }
        else {
            return true;
        }
    }

    public override void OnHyperLinkClick(string link)
    {
        if (link == "aoe2" && CheckIfAppExist(3) == false)
        {
            AddApps();
            App app = GetAppById(3);
            app.LaunchAddListener(LaunchGame);
            warn.Init(app);
            GetFirstApp().proertySetting = startSetting;
        }
    }
}
