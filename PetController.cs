using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public TextMeshProUGUI PetText;
    public RectTransform rectTrans;
    public  CanvasGroup canvasGroup;
    private int CoroutineId = 0;
    public float appearTime = 0.5f;
    public float disappearTime = 0.5f;
    public void Awake()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    public void ShowAtWithText(Vector3 position, string text)
    {
        PetText.text = text;
        rectTrans.position = position;
        Show();
    }
    public void ShowAtWithTextThenDisappear(Vector3 position, string text,float disappearTime)
    {
        PetText.text = text;
        rectTrans.position = position;
        Show();
        StartWaitAndDisapper(disappearTime);
    }
    public void StartWaitAndDisapper(float disappearTime)
    {
        StartCoroutine(WaitAndDisapper(disappearTime));
    }

    public IEnumerator WaitAndDisapper(float disappearTime)
    {
        yield return null;//等待一帧数
        int thisid = CoroutineId;
        yield return new WaitForSeconds(disappearTime);
        if(thisid == CoroutineId)
        {
            UnShow();
        }
    }

    public void Show()
    {
        Show(VoidDelegate);
    }
    public void VoidDelegate()
    {

    }
    public void Show(BasicDelegate callBack)
    {
        StartCoroutine(StartShow(callBack));
        DesktopManager.FullyShowAdjust(rectTrans);
    }

    public IEnumerator StartShow(BasicDelegate callBack)
    {
        CoroutineId++;
        int thisCoroutineId = CoroutineId;
        canvasGroup.blocksRaycasts = true;
        float ogAlpha = canvasGroup.alpha;
        float diff = 1f - ogAlpha;
        float time = 0;
        while(time < appearTime && thisCoroutineId == CoroutineId)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = ogAlpha + diff * time / appearTime;
            yield return null;
        }
        if(thisCoroutineId == CoroutineId)
        {
            canvasGroup.alpha = 1f;
            callBack?.Invoke(); //如果非正常退出就不执行
        }

    }

    public void UnShow()
    {
        UnShow(VoidDelegate);
    }

    public void UnShow(BasicDelegate callBack)
    {
        StartCoroutine(StartUnShow(callBack));
        
    }


    public IEnumerator StartUnShow(BasicDelegate callBack)
    {
        CoroutineId++;
        int thisCoroutineId = CoroutineId;
        canvasGroup.blocksRaycasts = false;
        float ogAlpha = canvasGroup.alpha;
        float time = 0;
        while (time < disappearTime && thisCoroutineId == CoroutineId)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = ogAlpha *(1 - time / appearTime);
            yield return null;
        }
        if (thisCoroutineId == CoroutineId)
        {
            canvasGroup.alpha = 0f;
            callBack?.Invoke();
        }

    }
}
