<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalBuscarNotarios.ascx.cs"
    Inherits="UserControls_ModalBuscarNotarios" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc5" %>  
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalConfirmarcion.ascx" TagName="ModalConfirmar" TagPrefix="uc3" %>
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

          function eliminarBlancos(control) {
              if (control != '')
                  control.value = control.value.replace(/^(\s|\&nbsp;)*|(\s|\&nbsp;)*$/g, "");
          }



    </script>
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
<div class="DivPaddingBotton TextCenterMiddle">
    <table class="TablaCabeceraCaja" style="width: 100%; height: 21px;">
        <tr>
            <td style="width: 100%" align="left">
                <asp:Label ID="lblBuscarPersonaModalFiltroTitulo" runat="server"  class="TextLeftTop"
                    Text="Buscar notario" SkinID="None"></asp:Label>
          </td>
            <td style="width: 50%">
                <asp:ImageButton ID="btnBuscarPersonaModalCancelar" runat="server" SkinID="BotonBarraCerrar"
                    ImageAlign="Right" ImageUrl="~/Images/x.gif" CausesValidation="false" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="pnlBucarPersonasModalFiltros" runat="server" SkinID="Detalle" RenderMode="Inline" UpdateMode="Conditional">
       <ContentTemplate>
        <table class="TextLeftMiddle" style="width: 98%">
            <tr>
                            <td colspan="3" align="left">
                                <asp:ValidationSummary ID="vsFiltroNoatrios" runat="server" ValidationGroup="FiltroNotarios" />
                            </td>
                        </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="lblIdNotario" runat="server" SkinID="Titulo2" Text="Nº de notario"></asp:Label>
                </td>
                <td style="width: 380px">
                    <asp:TextBox ID="txtIdNotario" runat="server"  onblur="javascript:eliminarBlancos(this)" Style="width: 98%">
                    </asp:TextBox><asp:RegularExpressionValidator ID="revIdNotario" runat="server"
                                                                  ControlToValidate="txtIdNotario"
                                                                  ValidationExpression="\d*"
                                                                  SetFocusOnError="True"
                                                                  Display="Dynamic"
                                                                  EnableClientScript="true"
                                                                  ErrorMessage="El campo debe ser numérico"
                                                                  ValidationGroup="FiltroNotarios">!</asp:RegularExpressionValidator>
                </td>
                <td class="TextRigthBottom" rowspan="7" style="width: 20px">
                    <asp:ImageButton ID="btnBuscarPersonaModalFiltrar" runat="server" ImageAlign="Right"
                        ImageUrl="~/Images/search.gif" 
                        onclick="btnBuscarPersonaModalFiltrar_Click" ValidationGroup="FiltroNotarios" CausesValidation="true" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="lblBuscarPersonaModalNombre" runat="server" SkinID="Titulo2" Text="Nombre"></asp:Label>
                </td>
                <td style="width: 380px">
                    <asp:TextBox ID="txtNotarioNombre" runat="server" onblur="javascript:eliminarBlancos(this)" Style="width: 98%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="lblNotarioApellidoPaterno" runat="server" SkinID="Titulo2" Text="Apellido Paterno"></asp:Label>
                </td>
                <td style="width: 380px">
                    <asp:TextBox ID="txtNotarioApellidoPaterno" runat="server"  onblur="javascript:eliminarBlancos(this)" Style="width: 98%" >
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="lblNotarioApellidoMaterno" runat="server" SkinID="Titulo2" Text="Apellido Materno"></asp:Label>
                </td>
                <td style="width: 380px">
                    <asp:TextBox ID="txtNotarioApellidoMaterno" runat="server"  onblur="javascript:eliminarBlancos(this)" Style="width: 98%">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="lblNotarioRFC" runat="server" SkinID="Titulo2" Text="RFC" Visible="False"></asp:Label>
                </td>
                <td style="width: 380px">
                    <asp:TextBox ID="txtNotarioRFC" runat="server"  onblur="javascript:eliminarBlancos(this)" Style="width: 98%" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="lblNotarioCURP" runat="server" SkinID="Titulo2" Text="CURP" Visible="False"></asp:Label>
                </td>
                <td style="width: 380px">
                    <asp:TextBox ID="txtNotarioCURP" runat="server"  onblur="javascript:eliminarBlancos(this)" Style="width: 98%" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="lblNotarioIFE" runat="server" SkinID="Titulo2" Text="IFE" Visible="False"></asp:Label>
                </td>
                <td style="width: 380px">
                    <asp:TextBox ID="txtNotarioIFE" runat="server"  onblur="javascript:eliminarBlancos(this)" Style="width: 98%" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="DivPaddingBotton TextLeftMiddle">
    <asp:Label ID="lblBuscarPersonaModalFiltroResultado" runat="server" SkinID="Titulo2" class="TextLeftTop"
        Text="Resultado"></asp:Label>
    <asp:ObjectDataSource ID="odsPorBusquedaNotario" runat="server" 
        EnablePaging="True" MaximumRowsParameterName="pageSize" 
        SelectCountMethod="NumTotalFilasNotarios" 
        SelectMethod="ObtenerNotariosPorBusqueda" SortParameterName="SortExpression" 
        StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtIdNotario" Name="numero" 
                PropertyName="Text" Type="Decimal" />
            <asp:ControlParameter ControlID="txtNotarioNombre" Name="nombre" 
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtNotarioApellidoPaterno" 
                Name="apellidoPaterno" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtNotarioApellidoMaterno" 
                Name="apellidoMaterno" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtNotarioRFC" Name="rfc" PropertyName="Text" 
                Type="String" />
            <asp:ControlParameter ControlID="txtNotarioCURP" Name="curp" 
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtNotarioIFE" Name="ife" PropertyName="Text" 
                Type="String" />
            <asp:Parameter Name="pageSize" Type="Int32" />
            <asp:Parameter Name="indice" Type="Int32" />
            <asp:Parameter Name="SortExpression" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:UpdatePanel  ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscarPersonaModalFiltrar" />
        </Triggers>
<ContentTemplate>
    <asp:GridView ID="gridViewPersonas" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="IDPERSONA,NOMBRE,APELLIDOPATERNO,APELLIDOMATERNO,NUMERO"
        GridLines="Horizontal" 
        onselectedindexchanged="gridViewPersonas_SelectedIndexChanged" 
        AllowSorting="True" onpageindexchanging="gridViewPersonas_PageIndexChanging" 
        onsorting="gridViewPersonas_Sorting" >
        <Columns>
            <asp:BoundField DataField="NUMERO" HeaderText="Nº" SortExpression="NUMERO">
                <ItemStyle Width="50px" />
            </asp:BoundField>
            <asp:BoundField DataField="NOMBREAPELLIDOS" HeaderText="Nombre y apellidos" SortExpression="NOMBREAPELLIDOS">
                <ItemStyle Width="550px" />
            </asp:BoundField>
            <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" Visible="False">
                <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="CURP" HeaderText="CURP" SortExpression="CURP" Visible="False">
                <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="CLAVEIFE" HeaderText="IFE" SortExpression="CLAVEIFE" Visible="False">
                <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" 
                        CommandName="Select" Text="Sel" CausesValidation="false" />
                </ItemTemplate>
                <ItemStyle Width="30px" />
            </asp:TemplateField>
            <%--<asp:CommandField CancelText="Cancelar" DeleteText="Eliminar" EditText="Editar" InsertText="Insertar"
                NewText="Nuevo" SelectImageUrl="~/Images/checkmark.gif" SelectText="Sel" ShowCancelButton="False"
                ShowSelectButton="True" UpdateText="Actualizar">
                <ItemStyle Width="20px" />
            </asp:CommandField>--%>
        </Columns>        
    </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div>
    <table width="100%">
        <tr class="TablaCeldaCaja">
            <td> <asp:UpdatePanel ID="updatePanelBotones" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                <div style="display: inline; float: right">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" />
                    &nbsp;&nbsp;
               <%--    <cc1:ModalPopupExtender ID="btnAceptar_ModalPopupExtender" runat="server" DynamicServicePath=""
                            Enabled="True" TargetControlID="btnAceptar" PopupControlID="PnlModalConfirmar"
                            BackgroundCssClass="PanelModalBackground" DropShadow="true">
                   </cc1:ModalPopupExtender> --%>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                </div>
                </ContentTemplate></asp:UpdatePanel>
            </td>
        </tr>
    </table>  
    </div>
<%--    <asp:Panel runat="server" ID="PnlModalConfirmar" 
            SkinID="Modal" Width="376px">
            <uc3:ModalConfirmar ID="ModalConfirmar1" runat="server" OnConfirmClick="confirmar_ConfirmClick"/>
    </asp:Panel>--%>

<%--<div>
<asp:Panel runat="server" ID="PnlModalCargarDatos" Style="width: 500px; display: none"
                    SkinID="Modal">
                    <uc5:Progreso ID="Progreso1" runat="server" />
                </asp:Panel>
</div>--%>