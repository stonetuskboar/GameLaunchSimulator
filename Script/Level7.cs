using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level7 : BasicLevel
{
    public Canvas launchCanvas;
    public LaunchMenu launchMenu;
    public PropertySetting CorrectSetting;

    public GameObject systemError;
    public TextMeshProUGUI CompletePercentText;
    public CanvasGroup RebootCanvasGroup;
    public RectTransform rebootIconTrans;
    public List<CanvasGroup> buttonCGList;

    public GameObject gameImageObj;
    public Image boomImage;
    public CanvasGroup BlackScreenCG;

    private float LaunchTime = 0;
    private int ErrorCount = 0;
    public override void Awake()
    {
        base.Awake();
        launchCanvas.worldCamera = Camera.main;
        completePlate.button.onClick.AddListener(OnCompleteLevel);
        launchMenu.LaunchButton.onClick.AddListener(LaunchGame);
        for(int i = 0; i < buttonCGList.Count; i++)
        {
            buttonCGList[i].GetComponent<Button>().onClick.AddListener(OnNuclearButtonClick);
        }
        Reset();
    }
    public override void Init(LevelManager manger)
    {
        base.Init(manger);
        levelManager.explorerManager.ShowTiebaById(6);
        levelManager.explorerManager.AddPatchHomeController();
        levelManager.explorerManager.GetPatchHomeController().AllClearToUndownload();
        levelManager.explorerManager.AddPageById(2);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(14);
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
        gameImageObj.SetActive(true);
        canvas.enabled = true;
        if (CheckLevelSuccess_1() == false)
        {
            yield return new WaitForSeconds(0.05f);
            Quit();
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < buttonCGList.Count; i++)
            {
                buttonCGList[i].alpha = 0;
                buttonCGList[i].blocksRaycasts = true;
            }
            for (float time = 0f; time < 0.5f; time += Time.deltaTime)
            {
                for (int i = 0; i < buttonCGList.Count; i++)
                {
                    buttonCGList[i].alpha = time/0.5f;
                }
                yield return null;
            }
            for (int i = 0; i < buttonCGList.Count; i++)
            {
                buttonCGList[i].alpha = 1f;
            }
        }
    }

    public void OnNuclearButtonClick()
    {
        StartCoroutine(ShowBlueScreenOrSuccess());
    }

    IEnumerator ShowBlueScreenOrSuccess()
    {
        for (int i = 0; i < buttonCGList.Count; i++)
        {
            buttonCGList[i].blocksRaycasts = false;
        }
        for (float time = 0f; time < 0.3f; time += Time.deltaTime)
        {
            for (int i = 0; i < buttonCGList.Count; i++)
            {
                buttonCGList[i].alpha = 1 - time / 0.3f;
            }
            yield return null;
        }
        if (CheckLevelSuccess_2() == false)
        {
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.PlaySfxByName("Crash");
            CompletePercentText.text = "0%";
            systemError.SetActive(true);
            Button button = systemError.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            for (float time = 0f; time < 3f; time += Time.deltaTime)
            {
                CompletePercentText.text = (time / 3f * 100f).ToString("F0") + "%";
                yield return null;
            }
            CompletePercentText.text = "100%";
            button.onClick.AddListener(StartReboot);
        }
        else
        {
            boomImage.enabled = true;
            yield return new WaitForSeconds(1f);
            completePlate.AddContent(contentIcons[0], "耗费了   " + GetTimeStrByFloat(LaunchTime));
            completePlate.AddContent(contentIcons[1], "出现了   " + ErrorCount.ToString() + "次闪退");
            completePlate.AddContent(contentIcons[2], "引爆了   " + "1个核弹");
            completePlate.AddContent(contentIcons[3], "遭遇了   " + Random.Range(20, 60).ToString() + "个道德抉择");
            completePlate.AddContent(contentIcons[4], "回忆了   " + Random.Range(2, 7).ToString() + "%的童年");
            completePlate.StartAppear();
        }
    }

    public void Quit()
    {
        ErrorCount++;
        Reset();
        levelManager.explorerManager.GetPatchHomeController().AllClearToUndownload();

    }

    public void Reset()
    {
        for (int i = 0; i < buttonCGList.Count; i++)
        {
            buttonCGList[i].alpha = 0;
            buttonCGList[i].blocksRaycasts = false;
        }
        RebootCanvasGroup.alpha = 0f;
        RebootCanvasGroup.blocksRaycasts = false;
        RebootCanvasGroup.interactable = false;
        systemError.SetActive(false);
        IsLaunched = false;
        canvas.enabled = false;
        boomImage.enabled = false;
        launchMenu.UnShow();
        gameImageObj.SetActive(false);
        BlackScreenCG.alpha = 1f;
        Button button = systemError.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void StartReboot()
    {
        systemError.SetActive(false);
        StartCoroutine(Reboot());
    }

    IEnumerator Reboot()
    {
        //执行这个函数需要三个参数
        //public CanvasGroup RebootCanvasGroup;
        //public RectTransform rebootIconTrans;
        //public CanvasGroup WholeCanvasGroup;
        RebootCanvasGroup.blocksRaycasts = true;
        RebootCanvasGroup.interactable = true;
        RebootCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(0.5f);
        Vector2 ogPosition = rebootIconTrans.anchoredPosition;
        for (float time = 0f; time < 3f; time += Time.deltaTime)
        {
            Vector3 angel = rebootIconTrans.localEulerAngles;
            angel.z = 360 * time; //在0到-360间波动
            rebootIconTrans.localEulerAngles = angel;
            rebootIconTrans.anchoredPosition = new Vector2(Mathf.Sin(Mathf.PI * time * 2 / 3) * ogPosition.y, Mathf.Cos(Mathf.PI * time * 2 / 3) * ogPosition.y);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        gameImageObj.SetActive(false);
        launchMenu.UnShow();

        for (float time = 0f; time < 0.3f; time += Time.deltaTime)
        {
            BlackScreenCG.alpha = 1 - time / 0.3f;
            yield return null;
        }

        Vector3 angel1 = rebootIconTrans.localEulerAngles;
        angel1.z = 0;
        rebootIconTrans.localEulerAngles = angel1;
        rebootIconTrans.anchoredPosition = ogPosition;
        AudioManager.Instance.PlaySfxByName("Start");
        StartCoroutine(WaitAndShowPet(0.5f, 5f, new Vector3(0, 0, 0),
        "系统在未正常关机的情况下重新启动。<br>或需打开浏览器以联网查找解决方式。"));
        Quit();
    }

    public override void OnCompleteLevel()
    {
        levelManager.LoadLevelById(8);
    }
    public bool CheckLevelSuccess_1()
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
    public bool CheckLevelSuccess_2()
    {
        patchHomeController patchHomeController = levelManager.explorerManager.GetPatchHomeController();
        int level7FileId = 8;
        int level7FileId2 = 5;
        int failDownloadFileId = 0;
        if (true == patchHomeController.CheckIfDownload(level7FileId) 
            && true == patchHomeController.CheckIfDownload(level7FileId2)
            && true != patchHomeController.CheckIfDownload(failDownloadFileId))
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
        if (link == "fo3" && CheckIfAppExist(4) == false)
        {
            AddApps();
            App app = GetAppById(4);
            app.LaunchAddListener(OpenLaunchMenu);
        }
    }
}
