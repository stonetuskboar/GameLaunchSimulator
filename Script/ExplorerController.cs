using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ExplorerController : LayeredCanvas
{
    public List<WebPage> existWebpages;
    private WebPage nowShowPage = null;
    public GameObject titlePrefab;
    public Transform titleLayoutTransform;
    public Transform WebPageTransform;
    public TextMeshProUGUI urlText;
    public override void Start()
    {
        base.Start();
    }

    public WebPage OpenNewPage(WebPageData data)
    {
        GameObject obj = Instantiate(data.PageObject, WebPageTransform);
        WebPage page = new WebPage(data);
        page.pageCanvasGroup = obj.GetComponent<CanvasGroup>();
        page.gameObject = obj;
        page.title = CreateTitle(data.titleText, data.pageId);
        existWebpages.Add(page);
        SwitchPageById(data.pageId);
        return page;
    }
    public Title CreateTitle(string text, int id)
    {
        GameObject obj = Instantiate(titlePrefab, titleLayoutTransform);
        Title title = obj.GetComponent<Title>();
        title.Init(this, text, id);
        return title;
    }

    public void SwitchPageById(int pageId)
    {
        if(nowShowPage != null)
        {
            nowShowPage.UnShow();
            nowShowPage.title.ChangeToUnShowState();
        }
        for(int i = 0; i < existWebpages.Count; i++)
        {
            if (existWebpages[i].pageId == pageId)
            {
                existWebpages[i].Show();
                existWebpages[i].title.ChangeToShowState();
                nowShowPage = existWebpages[i];
                urlText.text = existWebpages[i].url;
                break;
            }
        }
    }

    public void HideExplorerPageById(List<int> unHidePageIds)
    {
        for(int i = 0;i < existWebpages.Count;i++)
        {
            if (true == unHidePageIds.Contains(existWebpages[i].pageId))
            {
                existWebpages[i].UnHide();
            }
            else
            {
                existWebpages[i].Hide();
            }
        }
    }

    public WebPage FindPageById(int id)
    {
        for(int i = 0;i < existWebpages.Count;i++)
        {
            if (existWebpages[i].pageId == id)
            {
                return existWebpages[i];
            }
        }
        return null;
    }

    public void ClearExplorer()
    {
        urlText.text = "";
        for(int i = 0;i < existWebpages.Count;i++)
        {
            existWebpages[i].Clear();
            existWebpages.RemoveAt(i);
            i--;
        }
    }
}
[Serializable]
public class WebPage
{
    public Title title;
    public int pageId;
    public string url;
    public string titleText;
    public CanvasGroup pageCanvasGroup;
    public GameObject gameObject;
    public WebPage(WebPageData data)
    {
        pageId = data.pageId;
        url = data.url;
        titleText = data.titleText;
    }
    public void UnShow()
    {
        pageCanvasGroup.alpha = 0f;
        pageCanvasGroup.blocksRaycasts = false;
    }
    public void Show()
    {
        pageCanvasGroup.alpha = 1f;
        pageCanvasGroup.blocksRaycasts = true;
    }

    public void UnHide()
    {
        title.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        title.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Clear()
    {
        UnityEngine.Object.Destroy(gameObject);
        UnityEngine.Object.Destroy(title.gameObject);
    }
}
[Serializable]
public class WebPageData
{
    public int pageId;
    public string url;
    public string titleText;
    public GameObject PageObject;
}
