<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuLocal.ascx.cs" Inherits="ISAI_UserControls_MenuLocal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<%@ Register Src="~/UserControls/ModalConsultaValorCat.ascx" TagName="ModalConsultaValorCat"
    TagPrefix="uc1" %>
<script type="text/javascript">
    function ocultarPanelError(boton, panel) {
        if (panel.style.display == "block") {
            panel.style.display = "none";
            boton.value = "Mostrar Detalles";
        }
        else {
            panel.style.display = "block";
            boton.value = "Ocultar Detalles";
        }
    }
</script>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:TreeView ID="TreeView1" runat="server" SkinID="MenuLocal" 
            onselectednodechanged="TreeView1_SelectedNodeChanged" >
             <SelectedNodeStyle Font-Underline="True" /> 
             <NodeStyle Font-Underline="True" /> 
            <Nodes>
                <asp:TreeNode Text="Bandeja de entrada" Value="Bandeja de entrada" NavigateUrl="~/BandejaEntrada.aspx">
                </asp:TreeNode>
                <asp:TreeNode Text="Bandeja de entrada (SF)" Value="Bandeja de entrada (SF)" NavigateUrl="~/BandejaEntradaSF.aspx">
                </asp:TreeNode>
                <asp:TreeNode Text="Jornada Notarial" Value="Jornada Notarial" NavigateUrl="~/BandejaJornada.aspx">
                </asp:TreeNode>
                <asp:TreeNode Text="Obtener Nº Presentación" Value="Numero Presentacion" NavigateUrl="~/NumeroPresentacion.aspx">
                </asp:TreeNode>
                <asp:TreeNode Text="Informes" Value="Informes" NavigateUrl="~/Informes.aspx">
                    <asp:TreeNode Text="Específica" Value="Específica" NavigateUrl="~/InformeConsultaEspecifica.aspx"
                        Target="_blank"></asp:TreeNode>
                    <asp:TreeNode Text="Peritos / sociedades" Value="Peritos / sociedades" NavigateUrl="~/InformeSociedadPeritos.aspx"
                        Target="_blank"></asp:TreeNode>
                    <asp:TreeNode Text="Notarios" Value="Notarios" NavigateUrl="~/InformeNotarios.aspx"
                        Target="_blank"></asp:TreeNode>
                    <asp:TreeNode Text="Envíos totales" Value="Envíos totales" NavigateUrl="~/InformeEnviosTotales.aspx"
                        Target="_blank"></asp:TreeNode>
                    <asp:TreeNode Text="Valores unitarios" Value="Valores unitarios" NavigateUrl="~/InformeValores.aspx"
                        Target="_blank"></asp:TreeNode>
                    <asp:TreeNode Text="Líneas de captura" Value="Líneas de captura" NavigateUrl="~/InformeLineasCaptura.aspx"
                        Target="_blank"></asp:TreeNode>
                    <asp:TreeNode Text="Pagos" Value="Pagos" NavigateUrl="~/InformeConsultaPagos.aspx"
                        Target="_blank"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="Consulta valor catastral" Value="ConsultaValorCatastral" NavigateUrl="">
                </asp:TreeNode>
            </Nodes>
        </asp:TreeView>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hidValCat" />
        <cc1:ModalPopupExtender ID="ModalPopValCat" runat="server" Enabled="True" PopupControlID="pnlValCat"
            TargetControlID="hidValCat" BackgroundCssClass="PanelModalBackground " DropShadow="True">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlValCat" runat="server" Style="width: 375px; display: none" SkinID="Modal">
            <uc1:ModalConsultaValorCat ID="ModalconsultaValorCat" runat="server" OnEventClick="ValCat_EventClick" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uppErrorTareas" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <cc1:ModalPopupExtender ID="mpeErrorTareas" runat="server" Enabled="True" TargetControlID="hlnErrorTareas"
            PopupControlID="panErrorTareas" DropShadow="false" BackgroundCssClass="PanelModalBackground" />
        <asp:HyperLink ID="hlnErrorTareas" runat="server" Style="display: none" />
        <asp:Panel ID="panErrorTareas" runat="server" Style="display: none" SkinID="Modal">
            <uc11:ModalInfoExcepcion ID="errorTareas" runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
