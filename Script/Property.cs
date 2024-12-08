using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Property : LayeredCanvas
{

    private DesktopManager deskManager;
    private App nowApplication = null;
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public RectTransform rectTrans; //用来在拖动窗口时移动

    [Header("设置属性")]
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
    public List<TextMeshProUGUI> textList = new();
    public List<string> textStringList = new();
    [Header("底部按键")]
    public Button DecideButton;
    public Button CancelButton;
    public Button ApplyButton;

    public override void Awake()
    {
        base.Awake();
        SettingButton();

        CompToggle.onValueChanged.AddListener(OnBoolChanged);
        ComDropDown.onValueChanged.AddListener(OnIntChanged);
        ColorModeToggle.onValueChanged.AddListener(OnBoolChanged);
        ColorModeDropDown.onValueChanged.AddListener(OnIntChanged);
        lowResolutionToggle.onValueChanged.AddListener(OnBoolChanged);
        fullScreenOptimToggle.onValueChanged.AddListener(OnBoolChanged);
        RunByAdminToggle.onValueChanged.AddListener(OnBoolChanged); 
        ReResgisterToggle.onValueChanged.AddListener(OnBoolChanged); 
        ICCModeToggle.onValueChanged.AddListener(OnBoolChanged); 

        for(int i = 0; i < textList.Count; i++)
        {
            textList[i].text = textStringList[i];
        }
    }
    public void Init(DesktopManager manager)
    {
        deskManager = manager;
    }

    public override void Show()
    {
        base.Show();
        ApplyButton.interactable = false;
    }

    public void SettingButton()
    {
        DecideButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();
        ApplyButton.onClick.RemoveAllListeners();
        DecideButton.onClick.AddListener(ApplySettingThenUnShow);
        CancelButton.onClick.AddListener(UnShow);
        ApplyButton.onClick.AddListener(SaveSetting);
    }

    public void SettingButtonForLevel4()
    {
        DecideButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();
        ApplyButton.onClick.RemoveAllListeners();
        DecideButton.onClick.AddListener(UnShow);
        CancelButton.onClick.AddListener(SaveSetting);
        ApplyButton.onClick.AddListener(UnShow);
    }

    public void SetProperty(App app)
    {
        nowApplication = app;
        iconImage.sprite = app.iconImage.sprite;
        titleText.text = app.GetTitleWithoutNewLine() +" 属性";
        ImportSetting(app.proertySetting);
    }

    public void ImportSetting(PropertySetting importSetting)
    {
        CompToggle.isOn = importSetting.IsCompatibility;
        ComDropDown.value = (int)importSetting.compatMode;
        ColorModeToggle.isOn = importSetting.IsColorMode;
        ColorModeDropDown.value = (int)importSetting.colorMode;
        lowResolutionToggle.isOn = importSetting.IsLowResolution;
        fullScreenOptimToggle.isOn = importSetting.IsfullScreenOptim;
        RunByAdminToggle.isOn = importSetting.IsRunByAdmin;
        ReResgisterToggle.isOn = importSetting.IsReResgister;
        ICCModeToggle.isOn = importSetting.IsIccMode;
        setting = importSetting;
        ApplyButton.interactable = false;
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
        ApplyButton.interactable = false;
    }

    public void ApplySettingThenUnShow()
    {
        SaveSetting();
        UnShow();
    }
    public void OnBoolChanged(bool unUsedValue)
    {
        OnAnyValueChanged();
    }
    public void OnIntChanged(int unUsedValue)
    {
        OnAnyValueChanged();
    }
    public void OnAnyValueChanged()
    {
        ApplyButton.interactable = true;
    }
}

[Serializable]
public class PropertySetting
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

    public bool IsLevel4Equal(PropertySetting setting2)
    {
        if (IsCompatibility != setting2.IsCompatibility)
        {
            return false;
        }
        else if (IsCompatibility == false && compatMode != setting2.compatMode)
        {
            return false;
        }

        if (IsColorMode != setting2.IsColorMode)
        {
            return false;
        }
        else if (IsColorMode == false && colorMode != setting2.colorMode)
        {
            return false;
        }

        if (IsLowResolution != setting2.IsLowResolution)
        {
            return false;
        }

        if (IsfullScreenOptim != setting2.IsfullScreenOptim)
        {
            return false;
        }

        if (IsRunByAdmin != setting2.IsRunByAdmin)
        {
            return false;
        }

        if (IsReResgister != setting2.IsReResgister)
        {
            return false;
        }
        if (IsIccMode != setting2.IsIccMode)
        {
            return false;
        }

        return true;
    }
    public bool IsEqual(PropertySetting setting2)
    {
        if(IsCompatibility != setting2.IsCompatibility)
        {
            return false;
        }else if( IsCompatibility == true && compatMode != setting2.compatMode)
        {
            return false;
        }

        if(IsColorMode != setting2.IsColorMode)
        {
            return false;
        }else if(IsColorMode == true &&  colorMode != setting2.colorMode)
        {
            return false;
        }
        
        if (IsLowResolution != setting2.IsLowResolution)
        {
            return false;
        }

        if( IsfullScreenOptim != setting2.IsfullScreenOptim)
        {
            return false;
        }

        if(IsRunByAdmin != setting2.IsRunByAdmin)
        {
            return false;
        }

        if(IsReResgister != setting2.IsReResgister)
        {
            return false;
        }
        if(IsIccMode != setting2.IsIccMode)
        {
            return false;
        }

        return true;
    }

}


public enum CompatibilityMode
{
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
    _8Bit = 0,
    _16Bit = 1,
}