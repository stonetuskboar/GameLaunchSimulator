using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChat : BasicApplicationLaunch
{
    public override void OnLaunchOpenExplorer()
    {
        ChatManager manager = application.GetDeskTopManager().chatManager;
        manager.ShowChat();
    }
}
