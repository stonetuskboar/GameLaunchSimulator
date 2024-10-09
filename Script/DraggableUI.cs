using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform DragTargetRectTransform;
    private RectTransform draggableRectTrans;
    public UIAdjustType AdjustType = UIAdjustType.fullyDraggable;
    public void Start()
    {
        draggableRectTrans = GetComponent<RectTransform>();
        DesktopManager.FullyShowAdjust(DragTargetRectTransform, DragTargetRectTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 delta = Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(eventData.position - eventData.delta);
        DragTargetRectTransform.position += delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (AdjustType == UIAdjustType.fullyTarget)
        {
            DesktopManager.FullyShowAdjust(DragTargetRectTransform, DragTargetRectTransform);
        }
        else if (AdjustType == UIAdjustType.fullyDraggable)
        {
            DesktopManager.FullyShowAdjust(draggableRectTrans, DragTargetRectTransform);
        }
        else if (AdjustType == UIAdjustType.partlyDraggable)
        {
            DesktopManager.PartlyShowAdjust(draggableRectTrans, DragTargetRectTransform);
        }
    }

}
public enum UIAdjustType //可以理解为按照从小到大的UI大小设定。
{
    fullyTarget = 0,
    fullyDraggable = 1,
    partlyDraggable = 2,
}
