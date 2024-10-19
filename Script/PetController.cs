using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetController : MonoBehaviour
{
    public TextMeshProUGUI PetText;
    public RectTransform rectTrans;
    public  CanvasGroup canvasGroup;
    private int CoroutineId = 0;
    public float appearTime = 0.5f;
    public float disappearTime = 0.5f;
    public Image petImage;
    public void Awake()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
    }

    public void ShowAtWithText(Vector3 position, string text)
    {
        PetText.text = text;
        rectTrans.position = position;
        StartShow();
    }
    public void ShowAtWithTextThenDisappear(Vector3 position, string text,float disappearTime)
    {
        PetText.text = text;
        rectTrans.position = position;
        StartShow();
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
            StartUnShow();
        }
    }

    public void StartShow()
    {
        StartShow(VoidDelegate);
    }
    public void VoidDelegate()
    {

    }
    public void StartShow(BasicDelegate callBack)
    {
        StartCoroutine(Showing(callBack));
        DesktopManager.FullyShowAdjust(rectTrans);
    }

    public IEnumerator Showing(BasicDelegate callBack)
    {
        CoroutineId++;
        int thisCoroutineId = CoroutineId;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        float time = 0;
        while(time < appearTime && thisCoroutineId == CoroutineId)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / appearTime;
            yield return null;
        }
        if(thisCoroutineId == CoroutineId)
        {
            canvasGroup.alpha = 1f;
            callBack?.Invoke(); //如果非正常退出就不执行
        }

    }

    public void StartUnShow()
    {
        StartUnShow(VoidDelegate);
    }

    public void StartUnShow(BasicDelegate callBack)
    {
        StartCoroutine(UnShowing(callBack));
        
    }


    public IEnumerator UnShowing(BasicDelegate callBack)
    {
        CoroutineId++;
        int thisCoroutineId = CoroutineId;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
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
