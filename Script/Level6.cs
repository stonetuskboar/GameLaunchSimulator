using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level6 : BasicLevel
{
    public CanvasGroup WholeCanvasGroup;
    public Image successImage;
    public List<CanvasGroup> canvasGroups;
    public GameObject systemError;
    public TextMeshProUGUI CompletePercentText;
    public CanvasGroup RebootCanvasGroup;
    public RectTransform rebootIconTrans;
    public Button BlackScreenButton;
    private float LaunchTime = 0;
    private int ErrorCount = 0;
    private int CoroutineId = 0;
    public override void Awake()
    {
        base.Awake();
        Reset();
        completePlate.button.onClick.AddListener(OnCompleteLevel);
        BlackScreenButton.onClick.AddListener(OnBlackScreenClick);
    }
    public override void Init(LevelManager manger)
    {
        base.Init(manger);
        levelManager.explorerManager.ShowTiebaById(5);
        levelManager.explorerManager.GetPatchHomeController().AllClearToUndownload();
        levelManager.explorerManager.AddPageById(2);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(11);
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
        for (int i = 0; i < canvasGroups.Count; i++)
        {
            canvasGroups[i].alpha = 0f;
            canvasGroups[i].blocksRaycasts = true;
            canvasGroups[i].interactable = true;
            float time = 0;
            CoroutineId++;
            int id = CoroutineId;
            while (time < 1f && id == CoroutineId)
            {
                time += Time.deltaTime;
                canvasGroups[i].alpha = time / 1f;
                yield return null;
            }
            canvasGroups[i].alpha = 1f;
            yield return new WaitForSeconds(0.5f);
            time = 0;
            while (time < 0.5f && id == CoroutineId)
            {
                time += Time.deltaTime;
                canvasGroups[i].alpha = 1 - time / 0.5f;
                yield return null;
            }
            canvasGroups[i].alpha = 0f;
            canvasGroups[i].blocksRaycasts = false;
            canvasGroups[i].interactable = false;
        }

        if (CheckLevelSuccess() == false)
        {
            yield return new WaitForSeconds(1f);
            AudioManager.Instance.PlaySfxByName("Crash");
            CompletePercentText.text = "0%";
            systemError.SetActive(true);
            Button button = systemError.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            for ( float time = 0f; time < 3f; time += Time.deltaTime )
            {
                CompletePercentText.text = (time / 3f * 100f).ToString("F0")+"%";
                yield return null;
            }
            CompletePercentText.text = "100%";
            button.onClick.AddListener(StartReboot);
        }
        else
        {
            successImage.enabled = true;
            yield return new WaitForSeconds(1f);
            completePlate.AddContent(contentIcons[0], "耗费了   " + GetTimeStrByFloat(LaunchTime));
            completePlate.AddContent(contentIcons[1], "看到了   " + (ErrorCount* (canvasGroups.Count + 1 + 1)).ToString() + "遍过场动画");
            completePlate.AddContent(contentIcons[2], "重启了   " + (ErrorCount).ToString() + "次电脑");
            completePlate.AddContent(contentIcons[3], "收集了   " + Random.Range(20, 50).ToString() + "个可收集物");
            completePlate.AddContent(contentIcons[4], "遭遇了   " + Random.Range(1000, 6000).ToString() + "次门闩解谜");
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        ErrorCount++;
        patchHomeController patchHomeController = levelManager.explorerManager.GetPatchHomeController();
        patchHomeController.AllClearToUndownload();
        Reset();
    }

    public void Reset()
    {
        for(int i = 0; i < canvasGroups.Count; i++)
        {
            canvasGroups[i].alpha = 0f;
            canvasGroups[i].blocksRaycasts = false;
            canvasGroups[i].interactable = false;
        }
        RebootCanvasGroup.alpha = 0f;
        RebootCanvasGroup.blocksRaycasts = false;
        RebootCanvasGroup.interactable = false;
        WholeCanvasGroup.alpha = 1f;
        systemError.SetActive(false);
        IsLaunched = false;
        successImage.enabled = false;
        canvas.enabled = false;

    }


    public override void OnCompleteLevel()
    {
        levelManager.LoadLevelById(7);
    }
    public bool CheckLevelSuccess()
    {
        patchHomeController patchHomeController = levelManager.explorerManager.GetPatchHomeController();
        int level6FileId = 6;
        int failDownloadFileId = 0;
        if (true == patchHomeController.CheckIfDownload(level6FileId) && true != patchHomeController.CheckIfDownload(failDownloadFileId))
        {
            return true;
        }
        return false;
    }

    public void OnBlackScreenClick()
    {
        CoroutineId++;
        if(CoroutineId > 100000)
        {
            CoroutineId = 0;
        }
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
        RebootCanvasGroup.alpha = 0f;
        RebootCanvasGroup.blocksRaycasts = true;
        RebootCanvasGroup.interactable = true;
        for (float time = 0f; time < 0.3f; time += Time.deltaTime)
        {
            RebootCanvasGroup.alpha = time / 0.3f;
            yield return null;
        }
        RebootCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(0.5f);
        for (float time = 0f; time < 3f; time += Time.deltaTime)
        {
            Vector3 angel = rebootIconTrans.localEulerAngles;
            angel.z = 180 * (-1 + Mathf.Cos(Mathf.PI * ((time) % 1f))); //在0到-360间波动
            rebootIconTrans.localEulerAngles = angel;
            yield return null;
        }
        Vector3 angel1 = rebootIconTrans.localEulerAngles;
        angel1.z = 0;
        rebootIconTrans.localEulerAngles = angel1;
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySfxByName("Start");
        for (float time = 0f; time < 0.3f; time += Time.deltaTime)
        {
            WholeCanvasGroup.alpha = 1- time / 0.3f;
            yield return null;
        }
        StartCoroutine(WaitAndShowPet(0.5f, 5f, new Vector3(0, 0, 0),
    "系统在未正常关机的情况下重新启动。<br>或需打开浏览器以联网查找解决方式。"));
        Quit();
    }
    public override void OnHyperLinkClick(string link)
    {
        if (link == "assassin" && CheckIfAppExist(5) == false)
        {
            AddApps();
            App app = GetAppById(5);
            app.LaunchAddListener(LaunchGame);
        }
    }
}
