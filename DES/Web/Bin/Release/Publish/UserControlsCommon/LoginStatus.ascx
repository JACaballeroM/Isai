<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginStatus.ascx.cs" Inherits="UserControlsCommon_LoginStatus" %>
<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>    
<asp:LoginView ID="LoginView2" runat="server">
    <AnonymousTemplate>
    </AnonymousTemplate>
    <LoggedInTemplate>
        <br />
        <asp:Label ID="lblTituloUsuario" runat="server" SkinID="Titulo2" Text="Usuario:"></asp:Label><br />
        <asp:LoginName ID="LoginName1" runat="server" CssClass="TextoNormal" />        
        <br />
        <br />
        <asp:Label ID="lblTipoAutenticacion" runat="server" SkinID="Titulo2" Text="Tipo de Autenticación:"></asp:Label><br />      
        <span class="TextoNormal"><%=HttpContext.Current.User.Identity.AuthenticationType%></span><br />        
        <br />  
        <wif:FederatedPassiveSignInStatus ID="SignInStatus1" runat="server" SignInButtonType="Image" 
            SignInText="Iniciar Sesión" SignOutText="Cerrar Sesión"
            SignInImageUrl="~/Images/botonacceder.gif" OnSigningOut="MetodoSignOut" SignOutAction="RedirectToLoginPage" 
            SignOutImageUrl="~/Images/botonsalir.gif" />       
    </LoggedInTemplate>            
</asp:LoginView>  