using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfClickNotThisHidden : MonoBehaviour
{
    public void Start()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.AddHideNoClickTransform(transform);
        }
    }
    public void OnDestroy()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.DeleteHideNoClickTransform(transform);
        }
    }
}
