using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicSelectLabel : MonoBehaviour,IPointerDownHandler,IPointerExitHandler,IPointerEnterHandler
{
    [Header("自定义Hover，Select变色")]
    public bool IsShow = false;
    public Image BackImage;
    public Color ShowTitleColor;
    public Color UnShowTitleColor;
    public Color HoverColor;
    // Start is called before the first frame update
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
    public virtual void ChangeTitleColor(Color color)
    {
        BackImage.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsShow == false)
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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }
}
