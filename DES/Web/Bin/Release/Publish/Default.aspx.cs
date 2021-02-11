using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Web;
using System;
using System.Web;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (User.Identity.IsAuthenticated)
        {
            WSFederationAuthenticationModule.FederatedSignOut(null, new Uri(MicrosoftIdentityModelSection.DefaultServiceElement.FederatedAuthentication.WSFederation.SignOutReply));
        }
        Response.Redirect(Constantes.URL_SUBISAI_HOME);
    }
}
