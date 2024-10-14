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
    public Canvas blackScreenCanvas;
    public WarnSo warnSo;
    public Warn warn;
    public PropertySetting startSetting;
    public PropertySetting CorrectSetting;
    public override void Awake()
    {
        base.Awake();
        warn.button.onClick.AddListener(Quit);
        completePlate.button.onClick.AddListener(OnCompleteLevel);
        Reset();
    }
    public override void Init(LevelManager manger)
    {
        base.Init(manger);
        blackScreenCanvas.worldCamera = Camera.main;
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(1);
    }

    public void LaunchGame()
    {
        StartCoroutine(ShowBlackScreen());
    }

    IEnumerator ShowBlackScreen()
    {
        blackScreenCanvas.enabled = true;
        CameraController controller = levelManager.cameraController;
        canvas.enabled = true;
        controller.MultiSizeFixedLeftTop(0.5f);
        yield return new WaitForSeconds(0.5f);

        if (CheckLevelSuccess() == false)
        {
            gameImage.enabled = true;
            yield return new WaitForSeconds(0.7f);
            gameImage.enabled = false;
            yield return new WaitForSeconds(0.3f);
            controller.BackToOgSizeWhenFixedLeftTop();

            yield return new WaitForSeconds(0.2f);

            controller.MultiSizeFixedLeftTop(2f);

            yield return new WaitForSeconds(0.5f);
            BlackScreenImage.enabled = true;
            yield return new WaitForSeconds(0.5f);
            warn.ShowWarn();
        }
        else
        {
            controller.BackToOgSizeWhenFixedLeftTop();
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
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        levelManager.cameraController.BackToOgSizeWhenFixedLeftTop();
        Reset();
    }

    public void Reset()
    {
        warn.UnShowWarn();
        successImage.enabled = false;
        gameImage.enabled = false;
        canvas.enabled = false;
        blackScreenCanvas.enabled = false;
        BlackScreenImage.enabled = false;
    }
    public override void OnCompleteLevel()
    {
        levelManager.LoadLevelById(1);
    }
    public bool CheckLevelSuccess()
    {
        PropertySetting setting = GetFirstApp().proertySetting;
        if (setting.IsCompatibility != CorrectSetting.IsCompatibility
            || setting.compatMode != CorrectSetting.compatMode)
        {
            warn.SetWarn(warnSo.WarnDatas[3]);
            return false;
        }
        else if( setting.IsfullScreenOptim != false)
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
            Application app = GetAppById(3);
            app.LaunchAddListener(LaunchGame);
            warn.Init(app);
            GetFirstApp().proertySetting = startSetting;
        }
    }
}
