using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasScaler : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform canvasRect;

    public void Awake()
    {
        canvas = GetComponent<Canvas>();
        float screenHeight = Camera.main.orthographicSize * 2;  // orthographicSize为半高
        float screenWidth = screenHeight * Camera.main.aspect;  // 宽度通过高宽比来计算

        // 将屏幕宽高设置到Canvas的RectTransform中
        canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(screenWidth, screenHeight);
        Vector3 position  = Camera.main.transform.position;
        position.z = 0;
        canvasRect.position = position;
    }
}
