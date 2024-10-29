using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 控制器_下载弹窗 : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float AppearTime = 0.3f;
    public float downloadTime = 1.5f;
    public float 淡化时间 = 0.5f;

    private FileObject nowFile = null;
    public Image 软件图标;
    public TextMeshProUGUI 软件名称;
    public TextMeshProUGUI 软件大小;
    public GameObject downloading;
    public Image downloadProgressBar;
    public GameObject downloadComplete;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UnActive();
    }

    public void SetThenActive(FileData data, FileObject file)
    {
        nowFile = file;
        this.软件图标.sprite = data.fileSprite;
        this.软件名称.text = data.fileName;
        this.软件大小.text = data.fileSize;
        downloadComplete.SetActive(false);
        downloadProgressBar.fillAmount = 0;
        downloading.SetActive(true);
        Active();
        StartCoroutine(出现后淡化());
    }

    public void OnDisable()
    {
        UnActive();
    }

    public void UnActive()
    {
        gameObject.SetActive(false);
    }
    public void Active()
    {
        gameObject.SetActive(true);
    }
    public bool GetActive()
    {
        return gameObject.activeSelf;
    }

    IEnumerator 出现后淡化()
    {

        canvasGroup.alpha = 1f;

        float ogY = transform.localScale.y;
        Vector3 scale = transform.localScale;
        float time = 0;
        while( time < AppearTime)
        {
            time += Time.deltaTime;
            scale.y = time/ AppearTime;
            transform.localScale = scale;
            yield return null;
        }
        scale.y = ogY;
        transform.localScale = scale;

        time = 0;
        while (time < downloadTime)
        {
            time += Time.deltaTime;
            downloadProgressBar.fillAmount = time /downloadTime;
            yield return null;
        }
        downloadComplete.SetActive(true);
        downloading.SetActive(false);
        AudioManager.Instance.PlaySfxByName("Downloaded");
        yield return new WaitForSeconds(1f);
        if (nowFile != null)
        {
            nowFile.DownloadSuccess();
        }
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / 淡化时间;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        UnActive();
    }
   

}
