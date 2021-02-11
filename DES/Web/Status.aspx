<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Status.aspx.cs" Inherits="Status" %>
<%@ Register assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="Microsoft.IdentityModel.Web.Controls" tagprefix="wif" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Status</title>
</head>
<body>    
    <form id="form1" runat="server">
    <wif:FederatedPassiveSignIn ID="fpsiStatus" runat="server" 
        AutoSignIn="True" DestinationPageUrl="~/Status.aspx" DisplayRememberMe="False" 
        RequireHttps="False" ShowButtonImage="False" 
        UseFederationPropertiesFromConfiguration="True" VisibleWhenSignedIn="False">
    </wif:FederatedPassiveSignIn>
    </form>
</body>
</html>