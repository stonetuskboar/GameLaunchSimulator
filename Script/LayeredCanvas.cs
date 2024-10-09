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
        Transform currentTransform = transform; //寻找UI的所有父UI的transform
        while (currentTransform != null)
        {
            canvas = currentTransform.GetComponent<Canvas>();
            if (canvas != null)
            {
                return canvas;
            }
            currentTransform = currentTransform.parent;
        }
        Debug.Log("你将本脚本挂靠在了父对象中没有canvas的对象上！");
        return null;
    }
}
