using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public Application application;
    public Image blackScreen;
    public Color GrayColor;
    public Canvas canvas;
    public WarnSo warnSo;
    public Warn warn;
    public void Awake()
    {
        canvas.enabled = false;
    }

    public void Start()
    {
        warn.Init(application.iconImage.sprite, application.GetTitleWithoutNewLine());
        warn.button.onClick.AddListener(Quit  );
        application.OnLaunch += LaunchGame;
    }

    public void LaunchGame()
    {
        StartCoroutine(BlackScreen());
    }

    IEnumerator BlackScreen()
    {
        canvas.enabled = true;
        float time = 0;
        while(time< 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        if(CheckLevelSuccess() == false) 
        {
            Color og = blackScreen.color;
            blackScreen.color = GrayColor;
            time = 0;
            while (time < 0.3f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            blackScreen.color = og;
            time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            warn.SetWarn(warnSo.WarnDatas[0]);
            warn.Show();
        }
    }
    public void Quit()
    {
        warn.Unshow();
        canvas.enabled = false;
    }

    public bool CheckLevelSuccess()
    {
        return false;
    }


        
}
