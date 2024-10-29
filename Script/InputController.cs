using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public InputAction ClickAction;
    public DesktopManager DesktopManager;
    public List<Transform> HideNoClickTransforms = new();
    public List<Canvas> LayeredCanvases = new();
    public static InputController Instance = null;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void OnEnable()
    {
        ClickAction.Enable();
        ClickAction.performed += OnClickPress;
    }

    private void OnClickPress(InputAction.CallbackContext context)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Pointer.current.position.ReadValue() // 使用新的输入系统获取鼠标位置
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        List<Transform> parentsList = GetAllParentsOfResult0(results);
        if(parentsList != null)
        {
            HideNoClickObjects(parentsList);
            TopClickedCanvas(parentsList);
        }
    }
    public void AddHideNoClickTransform(Transform transform)
    {
        if(HideNoClickTransforms.Contains(transform) == false)
        {
            HideNoClickTransforms.Add(transform);
        }
    }
    public void DeleteHideNoClickTransform(Transform transform)
    {
        HideNoClickTransforms.Remove(transform);
    }
    public void TopTargetCanvas(Canvas canvas) //如果之前没遇到过这个Canvas，将其加入List列表中。
    {
        int index = LayeredCanvases.IndexOf(canvas);
        if(index < 0)
        {
            LayeredCanvases.Add(canvas);
            ReOrderLayeredCanvases();
        }
        else
        {
            TopTargetCanvas(index);
        }
    }
    public void TopTargetCanvas(int index)
    {
        if(index < LayeredCanvases.Count)
        {
            Canvas canvas = LayeredCanvases[index];
            LayeredCanvases.RemoveAt(index);
            LayeredCanvases.Add(canvas);
            ReOrderLayeredCanvases();
        }
    }
    public void AddLayedCanvas(Canvas canvas)
    {
        if(LayeredCanvases.Contains(canvas) == false)
        {
            LayeredCanvases.Add(canvas);
            canvas.sortingOrder = LayeredCanvases.Count - 1;
        }
    }
    public void DeleteLayedCanvas(Canvas canvas)
    {
        LayeredCanvases.Remove(canvas);
        ReOrderLayeredCanvases();
    }
    public void ReOrderLayeredCanvases()
    {
        for(int i = 0; i < LayeredCanvases.Count; i++)
        {
            LayeredCanvases[i].sortingOrder = i;
        }
    }
    private void TopClickedCanvas(List<Transform> parentsList) //第一个点击到的canvas显示在最前
    {

        List<Transform> CanvasTransformList = new();
        for (int i = 0; i < LayeredCanvases.Count; i++) 
        {
            CanvasTransformList.Add(LayeredCanvases[i].transform);
        }

        for(int i = 0;  i < parentsList.Count; i ++) //遇到第一个canvas，将它的sortorder抬高到最高
        {
            if (CanvasTransformList.Contains(parentsList[i]) == true)
            {
                TopTargetCanvas(CanvasTransformList.IndexOf(parentsList[i]));
                break;
            }
        }
    }
    private void HideNoClickObjects(List<Transform> parentsList) //隐藏未被点击到的UI
    {

        List<Transform> ActiveTransformList = new ();
        for (int i = 0; i < HideNoClickTransforms.Count; i++) //检查当前显示的UI元素
        {
            if(HideNoClickTransforms[i].gameObject.activeInHierarchy == true)
            {
                ActiveTransformList.Add(HideNoClickTransforms[i]);
            }
        }

        //检查是否点击到这个UI，如果点击到，移出队列。最后剩下的就是没被点击到的，这些都会被隐藏。
        for (int i = 0; i < ActiveTransformList.Count; i++)
        {
            if (parentsList.Contains(ActiveTransformList[i]) == true)
            {
                ActiveTransformList.RemoveAt(i);
                i--;
            }
        }
        foreach (Transform activeTransform in ActiveTransformList)
        {
            activeTransform.gameObject.SetActive(false);
        }
    }

    private List<Transform> GetAllParentsOfResult0(List<RaycastResult> results)
    {
        if (results.Count <= 0)
        {
            return null;
        }
        List<Transform> parentsList = new();
        Transform currentTransform = results[0].gameObject.transform; //寻找UI的所有父UI的transform
        while (currentTransform != null)
        {
            parentsList.Add(currentTransform);
            currentTransform = currentTransform.parent;
        }

        return parentsList;
    }
}
