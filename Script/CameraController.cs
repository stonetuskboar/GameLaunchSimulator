using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;
    public Transform cameraTransform;
    private float ogSize;
    public void Awake()
    {
        cameraTransform = Camera.transform;
        ogSize = Camera.orthographicSize;
    }

    public float GetOgSize()
    {
        return ogSize;
    }
    public float GetSize()
    {
        return Camera.orthographicSize;
    }
    public void MultiSizeFixedLeftTop(float multi)
    {
        float addSize = Camera.orthographicSize * (multi - 1);
        ChangeSizeWhenFixedLeftTop(addSize);
    }

    public void ChangeSizeWhenFixedLeftTop(float AddSize) {
        Camera.orthographicSize += AddSize;
        float screenHeight = AddSize;  // orthographicSizeΪ���
        float screenWidth = screenHeight * Camera.aspect;  // ���ͨ���߿��������
        Vector3 position = Camera.transform.position;
        position.x += screenWidth;
        position.y -= screenHeight;
        Camera.transform.position = position;
    }
    public void ChangeSizeWhenFixedLeftBottom(float AddSize)
    {
        Camera.orthographicSize += AddSize;
        float screenHeight = AddSize;  // orthographicSizeΪ���
        float screenWidth = screenHeight * Camera.aspect;  // ���ͨ���߿��������
        Vector3 position = Camera.transform.position;
        position.x += screenWidth;
        position.y += screenHeight;
        Camera.transform.position = position;
    }
    public void BackToOgSizeWhenFixedLeftTop()
    {
        float diff = ogSize - Camera.orthographicSize;
        Camera.orthographicSize = ogSize;

        float screenHeight = diff;  // orthographicSizeΪ���
        float screenWidth = screenHeight * Camera.aspect;  // ���ͨ���߿��������

        Vector3 position = Camera.transform.position;
        position.x += screenWidth;
        position.y -= screenHeight;
        Camera.transform.position = position;
    }
}
