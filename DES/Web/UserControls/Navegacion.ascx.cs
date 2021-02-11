using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIGAPred.Common.Web;

public partial class ISAI_UserControls_Navegacion : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //    SiteMap.SiteMapResolve += new SiteMapResolveEventHandler(SiteMap_SiteMapResolve);  
    }

    private SiteMapNode SiteMap_SiteMapResolve(Object sender, SiteMapResolveEventArgs e)
    { 
        SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
        SiteMapNode tempNode = currentNode;


        
        tempNode.Url = e.Context.Request.Url.PathAndQuery;

        return currentNode;
    }

}
