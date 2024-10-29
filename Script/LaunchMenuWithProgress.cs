using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchMenuWithProgress : LayeredCanvas
{
    public Button LaunchButton;
    public Button ExitButton;

    public override void Start()
    {
        base.Start();
        ExitButton.onClick.AddListener(UnShow);
    }
}
