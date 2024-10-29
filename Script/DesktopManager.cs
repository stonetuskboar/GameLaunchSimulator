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
    public TaskBar taskBar;
    public ExplorerManager explorerManager;
    public ChatManager chatManager;
    private App ClickMenuTarget = null;
    private App NowClickTarget = null;
    private int ClickCount = 0; //������unity���Դ���eventdata.clickcountֻ֧����������ƶ��˵��ֻ�᷵��1
    private float clickLeftTime = 0;
    public float doubleClickTime = 0.5f;
    public static Vector3 safetyEdge = new Vector3(60,48,0);
    [Header("����Ӧ�ù���")] //����Ͳ���������ˣ�ֻ��ÿ�ؿ�ͷ�ͽ�β�Ż����Ӻ�ɾ��Ӧ�ã�����û���������ܵĿ���
    public GameObject ApplicationPrefab;
    public ApplicationSo appSo;
    public List<App> applications = new List<App>();

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
        if (NowClickTarget != null)
        {
            if (clickLeftTime > 0)
            {
                clickLeftTime -= Time.deltaTime;
            }
            else
            {
                if(ClickCount == 2)
                {
                    NowClickTarget.OnLaunchInvoke();
                }
                StopClickCheck();
            }
        }
    }


    public App IfNoExistAddApp(int id)
    {
        for(int i = 0; i < applications.Count; i ++)
        {
            if (applications[i].appId == id)
            {
                return null;
            }
        }
        AudioManager.Instance.PlaySfxByName("AddApp");
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
    public void IfExistDeleteApp(App app)
    {
        applications.Remove(app);
    }
    public App GetApplicationById(int id)
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

    public App CreateAnApplication(int id)
    {
        AppData data = appSo.GetAppDataById(id);
        GameObject obj = Instantiate(ApplicationPrefab,DesktopIconsRectTrans);
        App app = obj.GetComponent<App>();
        if(app != null)
        {
            app.Init(this,data);
            applications.Add(app);
        }
        return app;
    }

    public App CreateAnApplication(AppData data)
    {
        AudioManager.Instance.PlaySfxByName("AddApp");
        GameObject obj = Instantiate(ApplicationPrefab, DesktopIconsRectTrans);
        App app = obj.GetComponent<App>();
        if (app != null)
        {
            app.Init(this, data);
            applications.Add(app);
        }
        return app;
    }

    public void ClickCheck(App clickTarget)
    {

        if(NowClickTarget != clickTarget) //֮ǰû�е����
        {
            NowClickTarget = clickTarget;
            ClickCount = 1;
            clickLeftTime = doubleClickTime;
        }
        else if (ClickCount == 1) //˫��һ��ʱ���û�б�ĵ��������update���ж�Ϊ��Ӧ��
        {
            ClickCount = 2;
            if (clickLeftTime < (0.5f * doubleClickTime))
            {
                clickLeftTime = 0.5f * doubleClickTime;
            }
        }
        else if (ClickCount == 2) //����Ѿ���������£��򴥷��������¼�
        {
            ShowProperty(clickTarget);
            StopClickCheck();
        }
    }
    public void StopClickCheck()
    {
        NowClickTarget = null;
        ClickCount = 0;
    }
    public void ShowClickMenu(App clickTarget)
    {
        AudioManager.Instance.PlaySfxByName("Open");
        ClickMenuTarget = clickTarget;
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
        if(ClickMenuTarget != null)
        {
           ClickMenuTarget.OnLaunchInvoke();
        }
        CloseClickMenu();
    }

    public void ShowProperty()
    {
        if(ClickMenuTarget == null)
        {
            Debug.Log("����û��ClickMenuTarget������µ����˴˺������������롣");
        }
        else
        {
            ShowProperty(ClickMenuTarget);
        }
    }
    public void ShowProperty(App appTarget)
    {
        AudioManager.Instance.PlaySfxByName("Open");
        CloseClickMenu();
        property.SetProperty(appTarget);
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

    public static Vector3 GetRectBottomLeftPosition(RectTransform rectrans)
    {
        Vector3 bottomLeftDiff = new Vector3(rectrans.rect.x, rectrans.rect.y, 0); //���뽫canvas scale ����Ϊ1��1��1����ȻҪ��ת��
        Vector3 bottomLeftPosition = rectrans.position + bottomLeftDiff;
        return bottomLeftPosition;
    }
    public static Vector3 GetRectBottomRightPosition(RectTransform rectrans)
    {
        //Vector3 bottomLeftDiff = new Vector3(rectrans.rect.x, rectrans.rect.y, 0); //���뽫canvas scale ����Ϊ1��1��1����ȻҪ��ת��
        //Vector3 topRightDiff = new Vector3(rectrans.rect.xMax, rectrans.rect.yMax, 0);
        Vector3 bottomLeftPosition = rectrans.position + new Vector3(rectrans.rect.xMax, rectrans.rect.y,0);
        return bottomLeftPosition;
    }

    public static Vector3 GetRectTopLeftPosition(RectTransform rectrans)
    {
        //Vector3 bottomLeftDiff = new Vector3(rectrans.rect.x, rectrans.rect.y, 0); //���뽫canvas scale ����Ϊ1��1��1����ȻҪ��ת��
        //Vector3 topRightDiff = new Vector3(rectrans.rect.xMax, rectrans.rect.yMax, 0);
        Vector3 bottomLeftPosition = rectrans.position + new Vector3(rectrans.rect.x, rectrans.rect.yMax,0);
        return bottomLeftPosition;
    }
    public static Vector3 GetRectTopRightPosition(RectTransform rectrans)
    {
        //Vector3 bottomLeftDiff = new Vector3(rectrans.rect.x, rectrans.rect.y, 0); //���뽫canvas scale ����Ϊ1��1��1����ȻҪ��ת��
        //Vector3 topRightDiff = new Vector3(rectrans.rect.xMax, rectrans.rect.yMax, 0);
        Vector3 bottomLeftPosition = rectrans.position + new Vector3(rectrans.rect.xMax, rectrans.rect.yMax, 0);
        return bottomLeftPosition;
    }

    public static Vector3 GetCentrePosition(RectTransform rectrans)
    {
        //Vector3 bottomLeftDiff = new Vector3(rectrans.rect.x, rectrans.rect.y, 0); //���뽫canvas scale ����Ϊ1��1��1����ȻҪ��ת��
        //Vector3 topRightDiff = new Vector3(rectrans.rect.xMax, rectrans.rect.yMax, 0);
        Vector3 bottomLeftPosition = rectrans.position + new Vector3((rectrans.rect.x + rectrans.rect.xMax)/2, (rectrans.rect.y + rectrans.rect.yMax)/2, 0);
        return bottomLeftPosition;
    }
}

