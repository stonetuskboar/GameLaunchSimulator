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
    public Image shadow;
    private float ShadowOgAlpha;
    private int ShakeID = 0;

    public void Awake()
    {
        ShadowOgAlpha = shadow.color.a;
    }
    public void UnShowWarn()
    {
        gameObject.SetActive(false);
    }
    public void ShowWarn()
    {
        AudioManager.Instance.PlaySfxByName("Warn");
        gameObject.SetActive(true);
    }
    public void Init(Sprite appSprite, string appStr)
    {
        appIcon.sprite = appSprite;
        appName.text = appStr;
    }
    public void Init(App app)
    {
        Init(app.iconImage.sprite,app.GetTitleWithoutNewLine());
    }
    public void SetWarn(WarnData data)
    {
        warnIcon.sprite = data.WarnIcon;
        warnText.text = data.WarnText;
    }
    public void EnableShadow()
    {
        shadow.enabled = true;
    }
    public void DisableShadow()
    {
        shadow.enabled = false;
    }
    public void StartShadowShake()
    {
        AudioManager.Instance.PlaySfxByName("ShadowShake");
        StartCoroutine(ShadowShake());
    }

    IEnumerator ShadowShake()
    {
        ShakeID++;
        int id = ShakeID;
        Color color = shadow.color;
        float sinValue = ShadowOgAlpha - 0.5f;
        float startRadians = Mathf.Asin(sinValue);

        for (float time = 0; time < 1f && id == ShakeID; time += Time.deltaTime)
        {
            float value = 0.5f + 0.5f * Mathf.Sin(startRadians +  time * 4 * (Mathf.PI * 2)); //Ò»ÃëÖÓÑ­»·4´Î
            color.a = value;
            shadow.color = color;
            yield return null;
        }

        color.a = ShadowOgAlpha;
        shadow.color = color;
    }


    public void OnDisable()
    {
        Color color = shadow.color;
        color.a = ShadowOgAlpha;
        shadow.color = color;
        ShakeID++;
    }
}



