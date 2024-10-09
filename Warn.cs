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
        Unshow();
    }

    public void Unshow()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Init(Sprite app, string appStr)
    {
        appIcon.sprite = app;
        appName.text = appStr;
    }
    public void SetWarn(WarnData data)
    {
        warnIcon.sprite = data.WarnIcon;
        warnText.text = data.WarnText;
    }
}



