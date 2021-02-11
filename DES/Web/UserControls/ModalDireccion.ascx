<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalDireccion.ascx.cs" Inherits="UserControls_ModalDireccion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ModalErrorObligatorios.ascx" TagName="ModalError" TagPrefix="uc2" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
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

<%--<asp:UpdatePanel ID="UpdatePanelModalDireccion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>--%>
        <asp:Panel ID="pnlDireccionModal" runat="server" Style="width: 560px; display: block;" SkinID="Modal">
            <table class="TablaCaja">
                <tr class="TablaCabeceraCaja">
                    <td colspan="2" class="TextLeftMiddle ">
                        Dirección
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblDelegacion" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Delegación"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <asp:DropDownList ID="ddlDelegacion" runat="server" Width="80%" SkinID="TextBoxObligatorio">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtDelegacion" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtValorDelegacion" runat="server" Visible="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblColoniaAsentamiento" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Colonia / Asentamiento"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtColoniaAsentamiento" runat="server" Width="80%" 
                            SkinID="TextBoxObligatorio" MaxLength="50"></asp:TextBox>
                    </td>  
                </tr>               
                <tr>                                  
                    <td style="width: 25%;">
                        <asp:Label ID="lblTipoAsentamiento" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Tipo asentamiento"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <asp:DropDownList ID="ddlTipoAsentamiento" runat="server" Width="80%" SkinID="TextBoxObligatorio">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtTipoAsentamiento" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtValorTipoAsentamiento" runat="server" Visible="False"></asp:TextBox>
                    </td>
                </tr>               
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblTipoVia" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Tipo via"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:DropDownList ID="ddlTipoVia" runat="server" Width="80%" SkinID="TextBoxObligatorio">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtTipoVia" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtValorTipoVia" runat="server" Visible="False"></asp:TextBox>
                    </td>
                </tr>               
            </table>
        </asp:Panel>
<%--    </ContentTemplate>                    
</asp:UpdatePanel> --%>              
               
<%--<asp:UpdatePanel ID="UpdatePanelVia" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>--%>
        <asp:Panel ID="pnlViaModal" runat="server" Style="width: 560px; display: block;" SkinID="Modal">
            <table class="TablaCaja">   
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblVia" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Vía"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <asp:TextBox ID="txtVia" runat="server" Width="80%" SkinID="TextBoxObligatorio" 
                            Height="50px" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
                    </td>   
                </tr>                 
            </table>
        </asp:Panel>
<%--    </ContentTemplate>                    
</asp:UpdatePanel>--%>

<%--<asp:UpdatePanel ID="UpdatePanelLocalidad" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>--%>
        <asp:Panel ID="pnlLocalidadModal" runat="server" Style="width: 560px; display: block;" SkinID="Modal">
            <table class="TablaCaja">   
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblLocalidad" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Localidad"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <asp:DropDownList ID="ddlLocalidad" runat="server" Width="80%" SkinID="TextBoxObligatorio">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtLocalidad" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtValorLocalidad" runat="server" Visible="False"></asp:TextBox>
                    </td>   
                </tr>  
            </table>
        </asp:Panel>
<%--    </ContentTemplate>                    
</asp:UpdatePanel>--%>

<%--<asp:UpdatePanel ID="UpdatePanelEdificio" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>--%>
        <asp:Panel ID="pnlEdificioModal" runat="server" Style="width: 560px; display: block;" SkinID="Modal">
            <table class="TablaCaja">                 
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblNumExt" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Nº exterior"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtNumExt" runat="server" Width="80%" 
                            SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                    </td>                
                    <td style="width: 25%;">
                        <asp:Label ID="lblNumInt" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Nº interior"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtNumInt" runat="server" Width="80%" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>                                 
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblAndador" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Andador"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtAndador" runat="server" Width="80%" MaxLength="30"></asp:TextBox>
                    </td>                
                    <td style="width: 25%;">
                        <asp:Label ID="lblEdificio" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Edificio"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtEdificio" runat="server" Width="80%" MaxLength="25"></asp:TextBox>
                    </td>
                </tr> 
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblEntrada" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Entrada"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtEntrada" runat="server" Width="80%" MaxLength="10"></asp:TextBox>
                    </td>                
                    <td style="width: 25%;">
                        <asp:Label ID="lblSeccion" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Sección"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtSeccion" runat="server" Width="80%" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>                   
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblCP" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Código postal"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtCP" runat="server" Width="80%" MaxLength="5"></asp:TextBox>
                    </td>                
                    <td style="width: 25%;">
                        <asp:Label ID="lblTelefono" class="TextLeftTop" SkinID="Titulo2" runat="server"
                            Text="Telefono"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtTelefono" runat="server" Width="80%" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>                                                              
            </table>
        </asp:Panel>
<%--    </ContentTemplate>                    
</asp:UpdatePanel>--%>

<%--<asp:UpdatePanel ID="UpdatePanelEntreCalles" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>--%>
        <asp:Panel ID="pnlEntreCallesModal" runat="server" Style="width: 560px; display: block;" SkinID="Modal">
            <table class="TablaCaja">                                      
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblEntreCalle1" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Entre calle 1"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <asp:TextBox ID="txtEntreCalle1" runat="server" Width="80%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                    </td>   
                </tr>    
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblEntreCalle2" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Entre calle 2"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <asp:TextBox ID="txtEntreCalle2" runat="server" Width="80%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                    </td>   
                </tr>      
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblIndAdi" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Indicaciones adicionales"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <asp:TextBox ID="txtIndAdi" runat="server" Width="80%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                    </td>   
                </tr>                                             
            </table>
        </asp:Panel>
<%--    </ContentTemplate>                    
</asp:UpdatePanel>--%>

<%--<asp:UpdatePanel ID="UpdatePanelSalir" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>--%>
        <asp:Panel ID="pnlSalirModal" runat="server" Style="width: 560px; display: block;" SkinID="Modal">
            <table class="TablaCaja">                                      
                <tr>
                    <td align="right">
                        <asp:Button ID="btnAceptar" runat="server" CssClass="boton" 
                         OnClick="btnAceptar_Click" Text="Aceptar"  />
                         &nbsp;
                        <asp:Button ID="btnVolver" runat="server" CssClass="boton" CausesValidation="false" 
                        Text="Volver" onclick="btnVolver_Click"></asp:Button>
                           
                        <cc1:ModalPopupExtender ID="btn_ModalPopupExtenderError" runat="server"
                            DynamicServicePath="" Enabled="True" TargetControlID="HiddenError" BackgroundCssClass="PanelModalBackground"
                            PopupControlID="PnlError"></cc1:ModalPopupExtender>       
                        <asp:HiddenField runat="server" ID="HiddenError" />
                        <asp:Panel ID="PnlError" Style="display: none;" runat="server" SkinID="Modal">
                            <div class="DivPaddingBotton TextCenterMiddle">
                                <uc2:ModalError ID="ModalError" runat="server" OnConfirmClick="ModalError_Ok_Click" />
                            </div>
                        </asp:Panel>
                    </td>    
                </tr>                 
            </table>
        </asp:Panel>
<%--    </ContentTemplate>                    
</asp:UpdatePanel>--%>
<asp:UpdatePanel ID="uppErrorTareas" runat="server" UpdateMode="Conditional" RenderMode="Inline"> 
    <ContentTemplate> 
        <cc1:modalpopupextender ID="mpeErrorTareas" runat="server"  Enabled="True" 
        TargetControlID="hlnErrorTareas" PopupControlID="panErrorTareas" 
        Dropshadow="false" BackgroundCssClass="PanelModalBackground" />
        <asp:HyperLink ID="hlnErrorTareas" runat="server" Style="Display:none" />
        <asp:Panel ID="panErrorTareas" runat="server" Style="Display:none" SkinID="Modal">
            <uc11:ModalInfoExcepcion ID="errorTareas" runat="server" /> 
        </asp:Panel>
    </ContentTemplate> 
</asp:UpdatePanel>
