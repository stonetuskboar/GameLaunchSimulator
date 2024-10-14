using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletePlate : MonoBehaviour
{
    public Button button;

    public CanvasGroup canvasGroup;
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
}
