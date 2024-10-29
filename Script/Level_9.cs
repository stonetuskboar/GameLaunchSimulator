using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Level_9 : BasicLevel
{
    public BasicPlayer player;
    public Light2D globalLight;
    public EnemyManager enemyManager;
    public Canvas BackGroundCanvas;
    public GameObject BossFight;
    public GameObject ErrorShield;
    public Color wave3Color;
    private CameraClearFlags ogFlags;
    public Vector3 ogCamPosition;
    private CameraController cameraController;
    private bool isFollow = false;
    public Canvas WiningCanvas;
    public GameObject systemError;
    public GameObject winingObj;
    public CanvasGroup RebootCanvasGroup;
    public RectTransform rebootIconTrans;
    public override void Awake()
    {
        BackGroundCanvas.enabled = false;
        BackGroundCanvas.worldCamera = Camera.main;
        WiningCanvas.enabled = false;
        WiningCanvas.worldCamera = Camera.main;
        base.Awake();
        BossFight.SetActive(false);
        ErrorShield.SetActive(false);
        systemError.SetActive(false);
        winingObj.SetActive(false);
    }
    public override void Init(LevelManager manger)
    {
        base.Init(manger);
        List<App> list = levelManager.deskManager.applications;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(false);
        }
        enemyManager.Init(this);
        player.Init(this, levelManager.deskManager.taskBar);
        cameraController = levelManager.cameraController;
        ogFlags = cameraController.Camera.clearFlags;
        ogCamPosition = cameraController.cameraTransform.position;
        App app = levelManager.deskManager.CreateAnApplication(levelManager.deskManager.appSo.simulatorAppdata);
        AddApp(app);
        app.LaunchAddListener(StartBossFight);
        levelManager.chatManager.ChatController.UnShow();
        levelManager.explorerManager.explorerController.UnShow();
    }
    public void StartBossFight()
    {
        AudioManager.Instance.PlayBgm("ToiletStory4");
        player.transform.position = GetFirstApp().iconImage.transform.position;
        DeleteAllApps();
        BossFight.SetActive(true);
        StartCoroutine(WaitAndShowPet(0.5f, 8f, new Vector3(0, 0, 0), "检测到大量病毒威胁！<br>警报级别：<color=#DF2020>高危！</color><br>请做好准备！"));
        StartCoroutine(SetLightIntensity(0.8f));
    }

    public void Update()
    {
        if (isFollow == true)
        {
            if(JudgeDistance(cameraController.cameraTransform, player.transform)  == true) 
            {
                float ogZ = cameraController.cameraTransform.position.z;
                Vector3 position = Vector3.Lerp(cameraController.cameraTransform.position, player.transform.position, Time.deltaTime / 2);
                position.z = ogZ;
                cameraController.cameraTransform.position = position;
            }
        }
    }
    public Image GetSystemIcon()
    {
        return levelManager.backGroundController.Icon;
    }
    public bool JudgeDistance(Transform t1, Transform t2)
    {
        Vector3 position = (t1.position - t2.position);
        if (Mathf.Abs(position.x) > (0.75 * cameraController.Camera.orthographicSize * cameraController.Camera.aspect))
        {
            return true;
        }
        if(Mathf.Abs(position.y) > (0.75 * cameraController.Camera.orthographicSize))
        {
            return true;
        }
        return false;
    }

    public void Wining()
    {
        AudioManager.Instance.PlaySfxByName("Victory");
        WiningCanvas.enabled = true;
        winingObj.SetActive(true);
        BossFight.SetActive(false);
    }

    public void Restart()
    {
        AudioManager.Instance.StopBgm();
        systemError.SetActive(true);
        WiningCanvas.enabled = true;
        BossFight.SetActive(false);
        AudioManager.Instance.PlaySfxByName("Crash");
        Button button = systemError.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StartReboot);
    }

    public void StartReboot()
    {
        systemError.SetActive(false);
        StartCoroutine(Reboot());
    }
    IEnumerator Reboot()
    {
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
        levelManager.LoadLevelById(9);
    }
    public override void  DestroyLevel()
    {
        List<App> list = levelManager.deskManager.applications;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(true);
        }
        cameraController.Camera.clearFlags = ogFlags;
        cameraController.BackToOgSizeWhenFixedLeftTop();
        cameraController.Camera.transform.position = ogCamPosition;
        levelManager.backGroundController.ResetToDefault();
        base.DestroyLevel();
    }



    public void IntoWave_1()
    {
        AudioManager.Instance.PlaySfxByName("Warn");
        ErrorShield.SetActive(true);
        levelManager.backGroundController.SetIconColor(new Color(248f /255f, 202f/255f ,184f/255f, 1f));
        StartCoroutine(SetLightIntensity(0.65f));
    }
    public void IntoWave_2()
    {
        Vector3 pos = cameraController.cameraTransform.position;
        pos.z = 0;
        levelManager.petController.ShowAtWithTextThenDisappear(pos, "更多威胁来袭！<br>请做好准备！", 5f);
        StartCoroutine(SetCameraBigger());
        BackGroundCanvas.enabled = true;
        StartCoroutine(SetLightIntensity(0.5f));
        levelManager.backGroundController.SetColorAlpha(0.1f);
        cameraController.Camera.clearFlags = CameraClearFlags.Nothing;
    }
    public void IntoWave_3()
    {
        Vector3 pos = cameraController.cameraTransform.position;
        pos.z = 0;
        levelManager.petController.ShowAtWithTextThenDisappear(pos, "<color=#DF2020><size=150%>系统内核破损！大量文件遭到感染！</color>", 5f);
        levelManager.backGroundController.SetIconColor(new Color(154f / 255f, 98f / 255f, 219f / 255f, 1f));
        levelManager.backGroundController.SetColorAlpha(0.02f);
        isFollow = true;
        globalLight.color = wave3Color;
    }
    public void IntoWave_4()
    {
        Vector3 pos = cameraController.cameraTransform.position;
        pos.z = 0;
        levelManager.petController.ShowAtWithTextThenDisappear(pos, "<color=#DF2020><size=150%>系统内核已被感染！</color>", 5f);
        enemyManager.CreateBoss();
    }

    IEnumerator SetCameraBigger()
    {
        float ogsize = cameraController.GetOgSize();

        for ( float time = 0; time < 4f; time += Time.deltaTime)
        {
            cameraController.ChangeSizeWhenFixedLeftBottom( Time.deltaTime * ogsize /6 );
            yield return null;
        }
    }
    IEnumerator SetLightIntensity(float target)
    {
        float diff = target - globalLight.intensity;

        for (float time = 0; time < 1f; time += Time.deltaTime)
        {
            globalLight.intensity += diff * Time.deltaTime;
            yield return null;
        }
    }

}
