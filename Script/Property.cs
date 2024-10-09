using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Property : LayeredCanvas
{

    private DesktopManager deskManager;
    private Application nowApplication = null;
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public GameObject propertyObject;

    public PropertySetting setting;
    public Toggle CompToggle;
    public TMP_Dropdown ComDropDown;
    public Toggle ColorModeToggle;
    public TMP_Dropdown ColorModeDropDown;
    public Toggle lowResolutionToggle;
    public Toggle fullScreenOptimToggle;
    public Toggle RunByAdminToggle;
    public Toggle ReResgisterToggle;
    public Toggle ICCModeToggle;
    public void Init(DesktopManager manager)
    {
        deskManager = manager;
    }
    public void SetProperty(Application app)
    {
        nowApplication = app;
        iconImage.sprite = app.iconImage.sprite;
        titleText.text = app.GetTitleWithoutNewLine();
    }

    public void SaveSetting()
    {
        setting.IsCompatibility = CompToggle.isOn;
        setting.compatMode = (CompatibilityMode)ComDropDown.value;
        setting.IsColorMode = ColorModeToggle.isOn;
        setting.colorMode = (ColorMode)ColorModeDropDown.value;
        setting.IsLowResolution = lowResolutionToggle.isOn;
        setting.IsfullScreenOptim = fullScreenOptimToggle.isOn;
        setting.IsRunByAdmin = RunByAdminToggle.isOn;
        setting.IsReResgister = ReResgisterToggle.isOn;
        setting.IsIccMode = ICCModeToggle.isOn;
        if(nowApplication == null)
        {
            Debug.Log("未选定Application便打开了property页面。");
        }
        else
        {
            nowApplication.proertySetting = setting;
        }
    }

    public void ApplySettingThenUnShow()
    {
        SaveSetting();
        UnShow();
    }
}

[Serializable]
public struct PropertySetting
{
    public bool IsCompatibility;
    public CompatibilityMode compatMode;
    public bool IsColorMode;
    public ColorMode colorMode;
    public bool IsLowResolution;
    public bool IsfullScreenOptim;
    public bool IsRunByAdmin;
    public bool IsReResgister;
    public bool IsIccMode;

}


public enum CompatibilityMode
{
    No = -1,
    windos95 = 0,
    windos97 = 1,
    windosXP = 2,
    windosVisit = 3,
    windos7 = 4,
    windos8 = 5,
    windos9 = 6,
    windos10 = 7,
}
public enum ColorMode
{
    No = -1,
    _8Bit = 0,
    _16Bit = 1,
}