using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredCanvas : MonoBehaviour
{
    protected Canvas canvas = null;

    public virtual void Awake()
    {
        canvas = FindParentCanvas();
        UnShow();
    }

    public virtual void Start()
    {
        if(InputController.Instance != null && canvas != null)
        {
            InputController.Instance.AddLayedCanvas(canvas);
        }
    }

    public virtual void OnDestroy()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.DeleteLayedCanvas(canvas);
        }
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
    public virtual void Show()
    {
        if(canvas == null)
        {
            canvas = FindParentCanvas();
        }
        canvas.enabled = true;
        InputController.Instance.TopTargetCanvas(canvas);
    }

    public virtual void UnShow()
    {
        if (canvas == null)
        {
            canvas = FindParentCanvas();
        }
        canvas.enabled = false;
    }

    public Canvas FindParentCanvas()
    {
        Canvas canvas;
        Transform currentTransform = transform; //Ѱ��UI�����и�UI��transform
        while (currentTransform != null)
        {
            canvas = currentTransform.GetComponent<Canvas>();
            if (canvas != null)
            {
                return canvas;
            }
            currentTransform = currentTransform.parent;
        }
        Debug.Log("�㽫���ű��ҿ����˸�������û��canvas�Ķ����ϣ�");
        return null;
    }
}
