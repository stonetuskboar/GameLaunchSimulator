using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerManager : MonoBehaviour
{
    public List<WebPageData> webpagesData;
    public ExplorerController explorerController;
    public void Start()
    {
        explorerController.OpenNewPage(webpagesData[1]);
        explorerController.OpenNewPage(webpagesData[0]);

    }
    public void ShowExplorer()
    {
        explorerController.Show();
    }
}
