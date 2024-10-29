using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void OnPointerDownHandler(PointerEventData eventData);
public class BackGroundController : MonoBehaviour, IPointerDownHandler
{
    public event OnPointerDownHandler OnPointerDownEvent;
    public Image BackGround;
    public Image Icon;
    private Color ogColor;
    private Color ogIconColor;

    public void Awake()
    {
        ogColor = BackGround.color;
        ogIconColor = Icon.color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent?.Invoke((eventData));
    }

    public void SetColorAlpha(float alpha)
    {
        Color color = ogColor;
        color.a = alpha;
        BackGround.color = color;
    }
    public void SetIconColor(Color color)
    {
        StartCoroutine(ChangeIconColor(color));
    }

    IEnumerator ChangeIconColor(Color color)
    {
        Color diff = color - Icon.color;
        for(float time = 0; time < 3f; time += Time.deltaTime)
        {
            Icon.color += diff * Time.deltaTime / 3;
            yield return null;
        }
        Icon.color = color;
    }
    public void ResetToDefault()
    {
        BackGround.gameObject.SetActive(true);
        Icon.gameObject.SetActive(true);
        BackGround.color = ogColor;
        Icon.color = ogIconColor;
    }
}
