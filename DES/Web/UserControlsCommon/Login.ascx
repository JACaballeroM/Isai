<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login.ascx.cs" Inherits="UserControlsCommon_Login" %>
<%@ Register src="LoginStatus.ascx" tagname="LoginStatus" tagprefix="control" %>
<asp:LoginView ID="LoginViewStatus" runat="server">
    <AnonymousTemplate>
        <iframe id="frameToken" style="vertical-align:top" 
            src="Status.aspx" 
            width="185px" height="220px" 
            frameborder="0" 
            scrolling="no"         
            title="Login de usuario">
        </iframe>
    </AnonymousTemplate>
    <LoggedInTemplate>
        <control:LoginStatus ID="lsLogout" runat="server" />
    </LoggedInTemplate>
</asp:LoginView>
