using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletePlate : MonoBehaviour
{
    public Button button;
    public CanvasGroup canvasGroup;
    public RectTransform contentRectTrans;
    public GameObject contentPrefab;
    public void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void StartAppear()
    {
        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        yield return null;
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / 1f;
            yield return null;
        }
        canvasGroup.blocksRaycasts = true;
    }

    public void AddContent(Sprite sprite,Color color, string str)
    {
        GameObject obj = Instantiate(contentPrefab, contentRectTrans);
        CompleteContent content = obj.GetComponent<CompleteContent>();
        content.SetIconText(sprite,color, str);
    }
    public void AddContent(ContentIcon icon, string str)
    {
        GameObject obj = Instantiate(contentPrefab, contentRectTrans);
        CompleteContent content = obj.GetComponent<CompleteContent>();
        content.SetIconText(icon, str);
    }
}

