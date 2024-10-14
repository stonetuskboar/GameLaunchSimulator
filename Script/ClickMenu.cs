using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickMenu : MonoBehaviour
{
    private DesktopManager deskManager;
    public RectTransform rectTrans;
    public Button launchButton;
    public Button openPropertyButton;
    public void Init(DesktopManager Manager)
    {
        deskManager = Manager;
        gameObject.SetActive(false);
    }
}
