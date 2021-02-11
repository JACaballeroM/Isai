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

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelError.Text = string.Format("Se ha producido un error desconocido en la {0}. Si el problema persiste contacte con el administrador del sistema.", Utilidades.GetParametroUrl("aspxerrorpath"));
    }
}
