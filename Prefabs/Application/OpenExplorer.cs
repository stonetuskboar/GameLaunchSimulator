using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenExplorer :BasicApplicationLaunch
{


    public override void OnLaunchOpenExplorer()
    {
        ExplorerManager manager = application.GetDeskTopManager().explorerManager;
        manager.ShowExplorer();
    }
}
