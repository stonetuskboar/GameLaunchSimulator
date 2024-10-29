using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (CheckIsMobilePhone() == true)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = Mathf.CeilToInt((float)Screen.currentResolution.refreshRateRatio.value);
        }
    }

    bool CheckIsMobilePhone()//没有webgl
    {
        if (Application.platform == RuntimePlatform.Android ||
               Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return true;
        }
        //else if (Application.platform == RuntimePlatform.WebGLPlayer)
        //{
        //    return SystemInfo.deviceType == DeviceType.Handheld || IsMobileUserAgent();
        //}
        else
        {
            return false;
        }
    }
    bool IsMobileUserAgent() //对应于webgl版本
    {
        string userAgent = SystemInfo.operatingSystem;
        return userAgent.Contains("Android") || userAgent.Contains("iPhone") || userAgent.Contains("iPad") || userAgent.Contains("iPod");
    }
}
