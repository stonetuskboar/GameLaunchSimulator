using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    private Button button;

    public void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Quit);
    }

    public void Quit()
    {
        UnityEngine.Application.Quit();
    }
}
