<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterPrincipalFE.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" Title="FUENTES EXTERNAS - ISAI -" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ModalConsultaValorCat.ascx" TagName="ModalconsultaValorCat"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPDestacados" runat="Server">
    <asp:LinkButton ID="HyperLink1" runat="server" PostBackUrl="~/BandejaEntrada.aspx">Bandeja de entrada</asp:LinkButton>
    <br />
   
    <asp:LinkButton ID="HyperLink2" runat="server" PostBackUrl="~/BandejaEntradaSF.aspx">Bandeja de entrada SF</asp:LinkButton>
    <br />
    <asp:LinkButton ID="HyperLink3" runat="server" PostBackUrl="~/Informes.aspx">Informes</asp:LinkButton>
    <br />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPActualidad" runat="Server">
    <asp:Label ID="lblTituloActualidad" runat="server" SkinID="Titulo2" Text="Actualidad"></asp:Label>

    <br />
     <span style="font-size:10.0pt;font-family:Arial;mso-ascii-font-family:Arial;
mso-fareast-font-family:+mn-ea;mso-bidi-font-family:+mn-cs;mso-fareast-theme-font:
minor-fareast;mso-bidi-theme-font:minor-bidi;color:#5F5F5F;mso-font-kerning:
12.0pt;language:es;mso-style-textfill-type:solid;mso-style-textfill-fill-color:
#5F5F5F;mso-style-textfill-fill-alpha:100.0%">
    Este modulo permite 
    administrar los avalúos asignados a un notario y sobre los que se puede realizar una declaración ISAI.
    <br />
    <br />
    Desde este modulo se podra:
    <br />
    <br />
     <span style="font-size:10.0pt"><span style="mso-special-format:bullet">&nbsp; • </span></span>Realizar el alta de declaraciones. 
    <br />
     <span style="font-size:10.0pt"><span style="mso-special-format:bullet">&nbsp; • </span></span>Editar / 
    Eliminar declaraciones
    <br />
     <span style="font-size:10.0pt"><span style="mso-special-format:bullet">&nbsp; • </span></span>Visualizar declaraciones
    <br />
     <span style="font-size:10.0pt"><span style="mso-special-format:bullet">&nbsp; • </span></span>Obtener justificantes
    <br />
     <span style="font-size:10.0pt"><span style="mso-special-format:bullet">&nbsp; • </span></span>Realizar declaraciones complementarias
    <br />
     <span style="font-size:10.0pt"><span style="mso-special-format:bullet">&nbsp; • </span></span>Obtener la Línea de Captura (mediante integración con el SLC) y disponer del Formato Universal de Tesorería (FUT).
 </span>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPPromoOvica" runat="Server">
    <asp:Label ID="lblTituloPromoOvica" runat="server" SkinID="Titulo2" 
        Text="Destacados"></asp:Label>
    <li>
        <asp:HyperLink ID="LinkOvica" runat="server" Font-Size="Small" 
            NavigateUrl="http://www.finanzas.df.gob.mx/"> 
            Secretaría de Finanzas del Distrito Federal </asp:HyperLink>
    </li>
    <li>
        <asp:HyperLink ID="HyperLink4" runat="server" Font-Size="Small" 
            NavigateUrl="http://www.finanzas.df.gob.mx/servicios/"> 
            Servicios al contribuyente</asp:HyperLink>
    </li>
    <li>
        <asp:HyperLink ID="HyperLink5" runat="server" Font-Size="Small" 
            NavigateUrl="http://ovica.finanzas.df.gob.mx/"> 
            Oficina Virtual del Catastro </asp:HyperLink>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderPPromoInstitucional"
    runat="Server">
    </asp:Content>
