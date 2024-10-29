using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title : BasicSelectLabel
{
    public int pageId;
    public Image hiddenImage;
    public ExplorerController ExplorerController;
    public TextMeshProUGUI titleText;

    public override void ChangeTitleColor(Color color)
    {
        hiddenImage.color = color;
        BackImage.color = color;
    }
    public void Init(ExplorerController expController, string text, int id)
    {
        ExplorerController = expController;
        SetTitle(text, id);
        BackImage.alphaHitTestMinimumThreshold = 0.1f;
        hiddenImage.alphaHitTestMinimumThreshold = 0.1f;
    }

    public void SetTitle(string text, int id)
    {
        titleText.text = text;
        pageId = id;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ExplorerController.SwitchPageById(pageId);
    }
}
