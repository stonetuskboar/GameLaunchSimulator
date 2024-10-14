using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Warn : MonoBehaviour
{
    public Image appIcon;
    public TextMeshProUGUI appName;
    public TextMeshProUGUI warnText;
    public Image warnIcon;
    public Button button;

    public void Awake()
    {
        UnShowWarn();
    }

    public void UnShowWarn()
    {
        Debug.Log("unshow");
        gameObject.SetActive(false);
    }
    public void ShowWarn()
    {
        Debug.Log("show");
        gameObject.SetActive(true);
    }
    public void Init(Sprite appSprite, string appStr)
    {
        appIcon.sprite = appSprite;
        appName.text = appStr;
    }
    public void Init(Application app)
    {
        appIcon.sprite = app.iconImage.sprite;
        appName.text = app.GetTitleWithoutNewLine();
    }
    public void SetWarn(WarnData data)
    {
        warnIcon.sprite = data.WarnIcon;
        warnText.text = data.WarnText;
    }
}



