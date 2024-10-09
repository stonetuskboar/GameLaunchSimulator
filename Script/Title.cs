using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public int pageId;
    public bool IsShow = false;
    public ExplorerController ExplorerController;
    public TextMeshProUGUI titleText;
    public Image BackImage;
    public Color ShowTitleColor;
    public Color UnShowTitleColor;
    public Color HoverColor;

    public void Start()
    {

    }
    public void Init(ExplorerController expController, string text, int id)
    {
        ExplorerController = expController;
        SetTitle(text, id);
    }

    public void SetTitle(string text, int id)
    {
        titleText.text = text;
        pageId = id;
    }

    public void ChangeToShowState()
    {
        IsShow = true;
        ChangeTitleColor(ShowTitleColor);
    }
    public void ChangeToUnShowState()
    {
        IsShow = false;
        ChangeTitleColor(UnShowTitleColor);
    }
    public void ChangeTitleColor(Color color)
    {
        BackImage.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(IsShow == false)
        {
            ChangeTitleColor(HoverColor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsShow == false)
        {
            ChangeTitleColor(UnShowTitleColor);
        }
        else
        {
            ChangeTitleColor(ShowTitleColor);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
            ExplorerController.SwitchPageById(pageId);
    }
}
