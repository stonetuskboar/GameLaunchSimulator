using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level5 : BasicLevel
{
    public Image BlackScreenImage;
    public Image successImage;
    public Color GrayColor;
    public WarnSo warnSo;
    public Warn warn;
    public PropertySetting CorrectSetting;
    public List<string> CorrectPropertyTextList;
    public List<string> propertyTextReplaceList;
    public Sprite VirusSprite;
    private List<Sprite> ogSpriteList = new();
    private float LaunchTime = 0;
    private int ErrorCount = 0;
    private bool isOnVirus = false;
    public override void Awake()
    {
        base.Awake();
        Reset();
        warn.button.onClick.AddListener(Quit);
        completePlate.button.onClick.AddListener(OnCompleteLevel);
    }
    public override void Init(LevelManager manger) //在start里开始
    {
        base.Init(manger);
        levelManager.explorerManager.ShowTiebaById(3);
        levelManager.chatManager.ShowChat();
        levelManager.chatManager.StartShowMessageSegment(5, 0.5f);
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
        if (segmentId == 5)
        {
            Vector3 position = levelManager.chatManager.ChatController.transform.position;
            position.y += 100f;
            position.x -= 200f;
            StartCoroutine(WaitAndShowPet(0.5f, 3f, position, "检测到可执行文件，此文件类型可能产生危害。"));
        }
        if (segmentId == 6)
        {
            StartCoroutine(ChangeComputer());
        }
    }

    public IEnumerator ChangeComputer()
    {
        yield return new WaitForSeconds(1f);
        canvas.enabled = true;
        AddApps();
        App app = GetAppById(2);
        app.LaunchAddListener(LaunchGame);

        Property property = levelManager.deskManager.property;
        CorrectPropertyTextList = property.textStringList;
        for (int i = 0; i < property.textList.Count; i++)
        {
            property.textList[i].text = propertyTextReplaceList[i];
        }
        warn.appIcon.sprite = VirusSprite;
        warn.appName.text = "锟斤拷锟斤拷2000 烫烫";
        DesktopManager manager = levelManager.deskManager;
        for(int j = 0; j < manager.applications.Count; j++)
        {
            ogSpriteList.Add(manager.applications[j].iconImage.sprite);
            manager.applications[j].iconImage.sprite = VirusSprite;
        }
        ogSpriteList.Add(levelManager.petController.petImage.sprite);
        levelManager.petController.petImage.sprite = VirusSprite;

        levelManager.explorerManager.GetTiebaController().ShowNewPostById(4);
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySfxByName("Dangerous");
        canvas.enabled = false;
        StartCoroutine(WaitAndShowPet(0f, 3f, app.transform.position + new Vector3(0, 100, 0), 
            "噺鮟裝哋遊戱浍洎憅炪哯茬適里！叒击戓癙摽狆楗妸姒矗帹咑閞，祐楗戓萇洝妸姒蹍閞婇啴。"));
    }

    IEnumerator ShowBlackScreen()
    {
        canvas.enabled = true;

        yield return new WaitForSeconds(1f);
        if (CheckLevelSuccess() == false)
        {
            Color og = BlackScreenImage.color;
            BlackScreenImage.color = GrayColor;
            yield return new WaitForSeconds(0.2f);
            BlackScreenImage.color = og;
            yield return new WaitForSeconds(1f);
            warn.ShowWarn();
            StartCoroutine(WaitAndShowPet(0.5f, 3f, warn.transform.position + new Vector3(200, 0, 0), "铪铪，閞個琓笑，適里莈洧寲難解荅，沵哋珵垿竾莈洧囸瑺運垳。"));
        }
        else
        {
            Color og = BlackScreenImage.color;
            BlackScreenImage.color = GrayColor;
            yield return new WaitForSeconds(0.2f);
            BlackScreenImage.color = og;
            successImage.enabled = true;
            yield return new WaitForSeconds(1f);
            completePlate.AddContent(contentIcons[0], "秏曊孒   " + GetTimeStrByFloat(LaunchTime));
            completePlate.AddContent(contentIcons[1], "弹炪孒   " + ErrorCount.ToString() + "佽蕔措");
            completePlate.AddContent(contentIcons[2], "击蹆孒   " + Random.Range(120, 600).ToString() + "個敵亾");
            completePlate.AddContent(contentIcons[3], "凗毇孒   " + Random.Range(4, 20).ToString() + "艘戰舰");
            completePlate.AddContent(contentIcons[4], "憾媣孒   " + Random.Range(5, 10).ToString() + "%哋妏件");
            completePlate.StartAppear();
        }
    }
    public void Quit()
    {
        ErrorCount++;
        Reset();
    }
    public void Reset()
    {
        IsLaunched = false;
        warn.UnShowWarn();
        successImage.enabled = false;
        canvas.enabled = false;
    }
    public override void OnCompleteLevel()
    {
        Property property = levelManager.deskManager.property;
        for (int i = 0; i < property.textList.Count; i++)
        {
            property.textList[i].text = CorrectPropertyTextList[i];
        }
        DesktopManager manager = levelManager.deskManager;
        int j = 0;
        for (; j < manager.applications.Count; j++)
        {
            manager.applications[j].iconImage.sprite = ogSpriteList[j];
        }
        levelManager.petController.petImage.sprite = ogSpriteList[j];
        levelManager.chatManager.OnMessageSegmentEnd -= OnMessageEnd;
        levelManager.LoadLevelById(6);
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
            warn.SetWarn(warnSo.WarnDatas[4]);
            return false;
        }
    }

    public override void OnHyperLinkClick(string link)
    {
        if (link == "beachHead2000Virus" && isOnVirus == false)
        {
            isOnVirus = true;
            levelManager.chatManager.StartShowMessageSegment(6, 0.5f);
        }
    }
}
