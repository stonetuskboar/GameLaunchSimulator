using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerManager : MonoBehaviour
{
    public List<WebPageData> webpagesData;
    public ExplorerController explorerController;
    public TiebaData tiebaSo;
    public void ShowExplorer()
    {
        explorerController.Show();
    }
    public void UnShowExplorer()
    {
        explorerController.UnShow();
    }
    public void Awake()
    {
        //AddTiebaController();
        //AddPatchHomeController();
    }

    public WebPage AddTiebaController()
    {
        return explorerController.OpenNewPage(webpagesData[0]);//0 = Ìù°É 1 = Å®æ´²¹¶¡
    }

    public WebPage AddPatchHomeController()
    {
        return explorerController.OpenNewPage(webpagesData[1]);
    }

    public TiebaController GetTiebaController()
    {
        WebPage page = explorerController.FindPageById(0);
        if (page == null)
        {
            page= AddTiebaController();
        }
        return page.gameObject.GetComponent<TiebaController>();
    }
    public patchHomeController GetPatchHomeController()
    {
        WebPage page = explorerController.FindPageById(1);
        if (page == null)
        {
            page = AddPatchHomeController();
        }
        return page.gameObject.GetComponent<patchHomeController>();
    }

    public void ShowTiebaById(int id)
    {
        AddTiebaController();
        TiebaController tieba = GetTiebaController();
        tieba.InitAndShowPost(tiebaSo, id);
    }
}
