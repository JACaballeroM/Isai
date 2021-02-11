using System;
using Microsoft.IdentityModel.Configuration;
public partial class Status : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var _homeRealm = MicrosoftIdentityModelSection.DefaultServiceElement.FederatedAuthentication.WSFederation.HomeRealm;
        Response.Write("<script>if (self == top || self.name !== 'frameToken') { window.top.location.href = '" + _homeRealm + "'}</script>");
    }
}