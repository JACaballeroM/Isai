<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" Title="ISAI - Error" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="UserControls/MenuLocal.ascx" tagname="MenuLocal" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDImagen" Runat="Server">
    <asp:Image ID="Image5" runat="server" 
        ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderDRuta" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderDContenido" Runat="Server">
    <asp:Label ID="LabelError" runat="server" CssClass="TextoNormalError" 
        Text=""></asp:Label>
        <br />
        <a class="TextoNormal" href="mailto:administrador@administrador.mex" > Contactar con el administrador</a>
    <br />
     
  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal" Runat="Server">
    
    <uc1:MenuLocal ID="MenuLocal1" runat="server" />
    
</asp:Content>

