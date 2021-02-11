using Microsoft.IdentityModel.Web;
using System;
using System.ComponentModel;
using Microsoft.IdentityModel.Configuration;

public partial class UserControlsCommon_LoginStatus : System.Web.UI.UserControl
{
    protected void MetodoSignOut(object sender, CancelEventArgs e)
    {
        WSFederationAuthenticationModule.FederatedSignOut(null, new Uri(MicrosoftIdentityModelSection.DefaultServiceElement.FederatedAuthentication.WSFederation.SignOutReply));
    }
}
