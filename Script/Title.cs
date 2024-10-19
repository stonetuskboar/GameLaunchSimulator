using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title : BasicSelectLabel
{
    public int pageId;
    public ExplorerController ExplorerController;
    public TextMeshProUGUI titleText;

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

    public override void OnPointerDown(PointerEventData eventData)
    {
        ExplorerController.SwitchPageById(pageId);
    }
}
