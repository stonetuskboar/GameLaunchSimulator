using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DesktopManager : MonoBehaviour
{
    public RectTransform DesktopIconsRectTrans;
    public RectTransform ClickMenuRectTrans;
    private ClickMenu clickMenu;
    public RectTransform PropertyRectTrans;
    private Property property;
    public ExplorerManager explorerManager;
    public ChatManager chatManager;
    private Application nowClickTarget = null;
    private Application ChoosenTarget = null;
    private float clickLeftTime = 0;
    public float doubleClickTime = 0.5f;
    public static Vector3 safetyEdge = new Vector3(60,48,0);
    public List<Application> applications = new List<Application>();

    public void Awake() //这些引用都在awake之前。
    {
        ClickMenuRectTrans.gameObject.SetActive(false);
        clickMenu = ClickMenuRectTrans.GetComponent<ClickMenu>();
        clickMenu.Init(this);
        clickMenu.openPropertyButton.onClick.AddListener(ShowProperty);
        clickMenu.launchButton.onClick.AddListener(LaunchTarget);
        property = PropertyRectTrans.GetComponent<Property>();
        property.Init(this);
        for (int i = 0; i < applications.Count; i++)
        {
            applications[i].Init(this);
        }
    }


    public void Update()
    {
        if (nowClickTarget != null)
        {
            if(clickLeftTime > 0)
            {
                clickLeftTime -= Time.deltaTime;
            }
            else
            {
                ShowClickMenu(nowClickTarget);
            }
        }
    }

    public void OnLeftClick(Application clickTarget)
    {
        if(nowClickTarget == clickTarget) //双击了
        {
            clickTarget.OnLaunchClick();
            StopDoubleClickCheck();
        }
        else
        {
            nowClickTarget = clickTarget;
            clickLeftTime = doubleClickTime;
        }
    }
    public void StopDoubleClickCheck()
    {
        nowClickTarget = null;
    }

    public void ShowClickMenu(Application clickTarget)
    {
        StopDoubleClickCheck();
        ChoosenTarget = clickTarget;
        Vector3 TargetPosition = Camera.main.ScreenToWorldPoint((Vector3)Pointer.current.position.ReadValue());
        TargetPosition.z = 0;
        ClickMenuRectTrans.position = TargetPosition;
        FullyShowAdjust(ClickMenuRectTrans);
        ClickMenuRectTrans.gameObject.SetActive(true);
    }

    public void CloseClickMenu()
    {
        ClickMenuRectTrans.gameObject.SetActive(false);
    }
    public void LaunchTarget()
    {
        if(ChoosenTarget != null)
        {
           ChoosenTarget.OnLaunchClick();
        }
        CloseClickMenu();
    }
    public void ShowProperty()
    {
        CloseClickMenu();
        property.SetProperty(ChoosenTarget);
        Vector3 TargetPosition = Camera.main.ScreenToWorldPoint((Vector3)Pointer.current.position.ReadValue());
        TargetPosition.z = 0;
        PropertyRectTrans.position = TargetPosition;
        FullyShowAdjust(PropertyRectTrans);
        property.Show();
    }

    public static void FullyShowAdjust(RectTransform adjustRect,RectTransform moveRect)
    {
        AdjustButtomLeft(adjustRect, moveRect);
        AdjustTopRight(adjustRect, moveRect);
    }
    public static void FullyShowAdjust(RectTransform adjustRect)
    {
        AdjustButtomLeft(adjustRect, adjustRect);
        AdjustTopRight(adjustRect, adjustRect);
    }
    public static void PartlyShowAdjust(RectTransform adjustRect, RectTransform moveRect)
    {
        PartlyAdjustButtomLeft(adjustRect, moveRect);
        PartlyAdjustTopRight(adjustRect, moveRect);
    }
    public static void PartlyShowAdjust(RectTransform adjustRect)
    {
        PartlyAdjustButtomLeft(adjustRect, adjustRect);
        PartlyAdjustTopRight(adjustRect, adjustRect);
    }
    private static void AdjustButtomLeft(RectTransform adjustRect,RectTransform moveRect)
    {
        Vector3 bottomLeftDiff = new Vector3(adjustRect.rect.x, adjustRect.rect.y,0); //必须将canvas scale 设置为1，1，1，不然要加转换
        Vector3 bottomLeftPosition = adjustRect.position + bottomLeftDiff;
        Vector3 camBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        Vector3 diff = Vector3.zero;
        if (bottomLeftPosition.x < camBottomLeft.x)
        {
            diff.x = camBottomLeft.x - bottomLeftPosition.x;
        }
        if (bottomLeftPosition.y < (camBottomLeft.y + safetyEdge.y))//都是负数,因底部有任务栏所以要往上抬。
        {
            diff.y = camBottomLeft.y + safetyEdge.y - bottomLeftPosition.y;
        }
        moveRect.position += diff;
    }
    private static void AdjustTopRight(RectTransform adjustRect,RectTransform moveRect)
    {
        Vector3 topRightDiff = new Vector3(adjustRect.rect.xMax, adjustRect.rect.yMax,0);
        Vector3 topRightPosition = adjustRect.position + topRightDiff;
        Vector3 camTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector3 diff = Vector3.zero;
        if (topRightPosition.x > camTopRight.x)
        {
            diff.x = camTopRight.x - topRightPosition.x;
        }
        if (topRightPosition.y > camTopRight.y)
        {
            diff.y = camTopRight.y - topRightPosition.y;
        }

        moveRect.position += diff;
    }

    private static void PartlyAdjustButtomLeft(RectTransform adjustRect,RectTransform moveRect)
    {
        Vector3 bottomLeftDiff = new Vector3(adjustRect.rect.x, adjustRect.rect.y, 0); //必须将canvas scale 设置为1，1，1，不然要加转换
        Vector3 bottomLeftPosition = adjustRect.position + bottomLeftDiff;
        Vector3 camTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector3 Edge = safetyEdge;
        if (adjustRect.rect.width < safetyEdge.x)//当UI尺寸特别小，露出UI全部。当UI大，只露出Edge的尺寸
        {
            Edge.x = adjustRect.rect.width;
        }
        if (adjustRect.rect.height < safetyEdge.y)
        {
            Edge.y = adjustRect.rect.height;
        }
        Vector3 diff = Vector2.zero;
        camTopRight -= Edge;                  //屏幕右上角留出edge的大小
        if (bottomLeftPosition.x  > camTopRight.x)
        {
            diff.x = camTopRight.x - bottomLeftPosition.x;
        }
        if (bottomLeftPosition.y > camTopRight.y)
        {
            diff.y = camTopRight.y - bottomLeftPosition.y;
        }
        moveRect.position += diff;
    }

    private static void PartlyAdjustTopRight(RectTransform adjustRect,RectTransform moveRect)
    {
        Vector3 topRightDiff = new Vector3(adjustRect.rect.xMax, adjustRect.rect.yMax, 0);
        Vector3 topRightPosition = adjustRect.position + topRightDiff;
        Vector3 camBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        Vector3 Edge = safetyEdge;
        if (adjustRect.rect.width < safetyEdge.x)//当UI尺寸特别小，露出UI全部。当UI大，只露出Edge的尺寸
        {
            Edge.x = adjustRect.rect.width;
        }
        Edge.y += 16;
        Vector3 diff = Vector3.zero;
        camBottomLeft += Edge;                   //屏幕左下角留出edge的大小
        if (topRightPosition.x < camBottomLeft.x)
        {
            diff.x = camBottomLeft.x - topRightPosition.x;
        }
        if (topRightPosition.y < camBottomLeft.y)
        {
            diff.y = camBottomLeft.y  - topRightPosition.y;
        }
        moveRect.position += diff;
    }
}

