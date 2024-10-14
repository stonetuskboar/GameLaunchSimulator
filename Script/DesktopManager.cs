using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DesktopManager : MonoBehaviour
{
    [Header("�Ҽ��˵������Թ���")]
    public RectTransform DesktopIconsRectTrans;
    public ClickMenu clickMenu;
    public Property property;
    public ExplorerManager explorerManager;
    public ChatManager chatManager;
    private Application nowClickTarget = null;
    private Application ChoosenTarget = null;
    private float clickLeftTime = 0;
    public float doubleClickTime = 0.5f;
    public static Vector3 safetyEdge = new Vector3(60,48,0);
    [Header("����Ӧ�ù���")] //����Ͳ���������ˣ�ֻ��ÿ�ؿ�ͷ�ͽ�β�Ż����Ӻ�ɾ��Ӧ�ã�����û���������ܵĿ���
    public GameObject ApplicationPrefab;
    public ApplicationSo appSo;
    public List<Application> applications = new List<Application>();

    public void Awake() //��Щ���ö���awake֮ǰ��
    {
        clickMenu.Init(this);
        clickMenu.openPropertyButton.onClick.AddListener(ShowProperty);
        clickMenu.launchButton.onClick.AddListener(LaunchTarget);
        property.Init(this);
        //App������levelManager����
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

    public Application IfNoExistAddApp(int id)
    {
        for(int i = 0; i < applications.Count; i ++)
        {
            if (applications[i].appId == id)
            {
                return null;
            }
        }
        return CreateAnApplication(id);
    }
    public void IfExistDeleteApp(int id)
    {
        for (int i = 0; i < applications.Count; i++)
        {
            if (applications[i].appId == id)
            {
                applications.RemoveAt(i);
                return;
            }
        }
    }
    public void IfExistDeleteApp(Application app)
    {
        applications.Remove(app);
    }
    public Application GetApplicationById(int id)
    {
        if (applications.Count > id)
        {
            if (applications[id].appId == id)
            {
                return applications[id];
            }
        }

        for (int i = 0; i < applications.Count; i++)
        {
            if (applications[i].appId == id)
            {
                return applications[i];
            }
        }
        return null;
    }

    public Application CreateAnApplication(int id)
    {
        AppData data = appSo.GetAppDataById(id);
        GameObject obj = Instantiate(ApplicationPrefab,DesktopIconsRectTrans);
        Application app = obj.GetComponent<Application>();
        if(app != null)
        {
            app.Init(this,data);
            applications.Add(app);
        }
        return app;
    }


    public void OnLeftClick(Application clickTarget)
    {
        if(nowClickTarget == clickTarget) //˫����
        {
            clickTarget.OnLaunchInvoke();
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
        clickMenu.rectTrans.position = TargetPosition;
        FullyShowAdjust(clickMenu.rectTrans);
        clickMenu.gameObject.SetActive(true);
    }

    public void CloseClickMenu()
    {
        clickMenu.gameObject.SetActive(false);
    }
    public void LaunchTarget()
    {
        if(ChoosenTarget != null)
        {
           ChoosenTarget.OnLaunchInvoke();
        }
        CloseClickMenu();
    }
    public void ShowProperty()
    {
        CloseClickMenu();
        property.SetProperty(ChoosenTarget);
        Vector3 TargetPosition = Camera.main.ScreenToWorldPoint((Vector3)Pointer.current.position.ReadValue());
        TargetPosition.z = 0;
        property.rectTrans.position = TargetPosition;
        FullyShowAdjust(property.rectTrans);
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
        Vector3 bottomLeftDiff = new Vector3(adjustRect.rect.x, adjustRect.rect.y,0); //���뽫canvas scale ����Ϊ1��1��1����ȻҪ��ת��
        Vector3 bottomLeftPosition = adjustRect.position + bottomLeftDiff;
        Vector3 camBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        Vector3 diff = Vector3.zero;
        if (bottomLeftPosition.x < camBottomLeft.x)
        {
            diff.x = camBottomLeft.x - bottomLeftPosition.x;
        }
        if (bottomLeftPosition.y < (camBottomLeft.y + safetyEdge.y))//���Ǹ���,��ײ�������������Ҫ����̧��
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
        Vector3 bottomLeftDiff = new Vector3(adjustRect.rect.x, adjustRect.rect.y, 0); //���뽫canvas scale ����Ϊ1��1��1����ȻҪ��ת��
        Vector3 bottomLeftPosition = adjustRect.position + bottomLeftDiff;
        Vector3 camTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector3 Edge = safetyEdge;
        if (adjustRect.rect.width < safetyEdge.x)//��UI�ߴ��ر�С��¶��UIȫ������UI��ֻ¶��Edge�ĳߴ�
        {
            Edge.x = adjustRect.rect.width;
        }
        if (adjustRect.rect.height < safetyEdge.y)
        {
            Edge.y = adjustRect.rect.height;
        }
        Vector3 diff = Vector2.zero;
        camTopRight -= Edge;                  //��Ļ���Ͻ�����edge�Ĵ�С
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
        if (adjustRect.rect.width < safetyEdge.x)//��UI�ߴ��ر�С��¶��UIȫ������UI��ֻ¶��Edge�ĳߴ�
        {
            Edge.x = adjustRect.rect.width;
        }
        Edge.y += 16;
        Vector3 diff = Vector3.zero;
        camBottomLeft += Edge;                   //��Ļ���½�����edge�Ĵ�С
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

