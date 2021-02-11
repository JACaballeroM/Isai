<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DireccionControl.ascx.cs" Inherits="UserControls_DireccionControl" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="UpdatePanelDomicilio" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>
        <uc3:progreso id="progresoDomicilio" runat="server" associatedupdatepanelid="UpdatePanelDomicilio"
            displayafter="0" dynamiclayout="false" mensaje="Cargando datos..." />
        <input type="hidden" runat="server" id="hidXmlDireccion" />
        <div class="DivPaddingBotton TextLeftMiddle">
            <cc1:collapsiblepanelextender id="domicilio_CollapsiblePanelExtender" runat="server"
                collapsecontrolid="imgCollapseDomicilio" collapsed="True" collapsedimage="~/Images/label.gif"
                expandcontrolid="imgCollapseDomicilio" expandedimage="~/Images/label_open.gif"
                imagecontrolid="imgCollapseDomicilio" suppresspostback="True" targetcontrolid="PanelDomicilio"
                collapsedtext="Expandir" expandedtext="Colapsar">
                        </cc1:collapsiblepanelextender>
            <fieldset class="formulario">
                <legend class="formulario">
                    <asp:ImageButton ID="imgCollapseDomicilio" runat="server" CausesValidation="False"
                                    ImageUrl="~/Images/label.gif" />
                    &nbsp;Domicilio
                                <asp:TextBox ID="txtEspDireccion" runat="server" OnTextChanged="txtEspDireccion_ValueChanged"
                                    Style="display: none;" AutoPostBack="true" />
                
                </legend>
                <asp:Panel ID="PanelDomicilio" runat="server">
                    <table class="TextLeftMiddle" style="width: 100%;">
                        <tr>
                            <td colspan="4"></td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="Label1" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Estado"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="TextBoxObligatorio" Width="300" Enabled="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvEstado" runat="server" ControlToValidate="ddlEstado" SetFocusOnError="True"
                                    Enabled="true" InitialValue="-1" ErrorMessage="Seleccione un estado." ValidationGroup="ValidarDeclaracionPersona"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvEstadoVacio" runat="server" ControlToValidate="ddlEstado" Display="Dynamic" Enabled="true" ErrorMessage="Error, verifique el estado(Domicilio)" SetFocusOnError="True" ValidationGroup="ValidarDeclaracionPersona">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblEstadoDato" class="TextLeftTop" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="Label3" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Delegación/Municipio"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlDelegacion" runat="server" CssClass="TextBoxObligatorio" Width="300px" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlDelegacion_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvDelegacion" runat="server" ControlToValidate="ddlDelegacion" SetFocusOnError="True"
                                    Enabled="true" InitialValue="-1" ErrorMessage="Seleccione un municipio." ValidationGroup="ValidarDeclaracionPersona"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblDelegacionDato" class="TextLeftTop" runat="server" Visible="false"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblCol" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Colonia"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlColonia" runat="server" CssClass="TextBoxObligatorio" Width="267px" Enabled="true" Visible="false" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvColonia" runat="server" ControlToValidate="ddlColonia" SetFocusOnError="True"
                                    Enabled="true" InitialValue="-1" ErrorMessage="Seleccione una colonia." ValidationGroup="ValidarDeclaracionPersona"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                 <asp:TextBox ID="txtColonia" runat="server" MaxLength="20" Width="267px" CssClass="TextBoxObligatorio"></asp:TextBox>
                                
                                <asp:RequiredFieldValidator
                                    ID="rfvTxtColonia" runat="server" ControlToValidate="txtColonia" SetFocusOnError="True"
                                    Enabled="true" ErrorMessage="Escriba al menos 3 letras." InitialValue="" ValidationGroup="ValidarDeclaracionPersona1"
                                    Display="Dynamic" >*</asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator
                                    ID="rfvColonia2" runat="server" ControlToValidate="txtColonia" SetFocusOnError="True"
                                    Enabled="true" ErrorMessage="Escriba al menos 3 letras." InitialValue="" ValidationGroup="ValidarDeclaracionPersona"
                                    Display="Dynamic" >*</asp:RequiredFieldValidator>
                                <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" OnClick="btnBuscar_Click"
                                                ImageUrl="~/Images/search.gif" ValidationGroup="ValidarDeclaracionPersona1" />
                                <asp:ImageButton ID="btnEliminarBusqueda" runat="server" ImageUrl="~/Images/trash.gif"
                                                CausesValidation="False" AlternateText="Eliminar filtro busqueda" Enabled="true"
                                                ToolTip="Eliminar filtro busqueda" OnClick="btnEliminarBusqueda_Click" visible="false"/>
                                <asp:Label ID="lblColoniaDato" class="TextLeftTop" runat="server" Visible="false"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblTipo" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Tipo asentamiento"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlTipoAsentamiento" runat="server" CssClass="TextBoxObligatorio" Width="300px" Enabled="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvTipoAsentamiento" runat="server" ControlToValidate="ddlTipoAsentamiento" SetFocusOnError="True"
                                    Enabled="true" InitialValue="-1" ErrorMessage="Seleccione un asentamiento." ValidationGroup="ValidarDeclaracionPersona"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblTipoAsentamientoDato" class="TextLeftTop" runat="server" Visible="false"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblTipoVia" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Tipo vía"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlTipoVia" runat="server" CssClass="TextBoxObligatorio" Width="300px" Enabled="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvTipoVia" runat="server" ControlToValidate="ddlTipoVia" SetFocusOnError="True"
                                    Enabled="true" InitialValue="-1" ErrorMessage="Seleccione un tipo de via." ValidationGroup="ValidarDeclaracionPersona"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblTipoViaDato" class="TextLeftTop" runat="server" Visible="false"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblVia" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="via"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtVia" runat="server" CssClass="TextBoxObligatorio" MaxLength="50" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvVia" runat="server" ErrorMessage="El campo es obligatorio"
                                    Text="[*]" ControlToValidate="txtVia" SetFocusOnError="true" Display="Dynamic" ValidationGroup="ValidarDeclaracionPersona" />
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblCP" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Código postal"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtCP" runat="server" MaxLength="5" Width="75px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revCP" runat="server" ErrorMessage="RegularExpressionValidator"
                                ControlToValidate="txtCP" Display="Dynamic" Text="[!]" ValidationExpression="[\s\S]{5}$" ValidationGroup="ValidarDeclaracionPersona" />
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblNumExt" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="N° Exterior"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtNumExt" runat="server" CssClass="TextBoxObligatorio" MaxLength="5" Width="75px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNumExt" runat="server" ErrorMessage="El campo es obligatorio"
                                    Text="[*]" ControlToValidate="txtNumExt" SetFocusOnError="true" Display="Dynamic" ValidationGroup="ValidarDeclaracionPersona" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblTipoLoc" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Tipo localidad"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:DropDownList ID="ddlTipoLocalidad" runat="server" CssClass="TextLeftTop" Width="130px" Enabled="true">
                                </asp:DropDownList>
                                <asp:Label ID="lblTipoLocalidadDato" class="TextLeftTop" runat="server" Visible="false"></asp:Label>

                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblNumInt" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="N° interior"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtNumInt" runat="server" MaxLength="30" Width="75px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblAndador" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Andador"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtAndador" runat="server" MaxLength="30" Width="100px"></asp:TextBox>
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblEdif" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Edificio"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtEdificio" runat="server" MaxLength="25" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblEntra" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Entrada"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtEntrada" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblSexion" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Sección"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtSeccion" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblEntreCalle1" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Entre Calle 1"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtCalle1" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblEntreCalle2" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Entre Calle 2"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtCalle2" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblTelephon" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Telefono"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtTelefono" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                            </td>
                        </tr>


                        <tr>
                            <td style="width: 25%;">
                                <asp:Label ID="lblIndicacionesAdicionales" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                    Text="Ind. Adicionales"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtIndicaciones" runat="server" SkinID="TextAreaNormal" TextMode="MultiLine" />
                                <asp:RegularExpressionValidator ID="revIndicaciones" runat="server" ErrorMessage="Superado el número máximo de caracteres"
                                    ControlToValidate="txtIndicaciones" Display="Dynamic" Text="[!]" ValidationExpression="[\s\S]{0,100}$" ValidationGroup="ValidarDeclaracionPersona" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" colspan="4">
                                <asp:Button ID="btnEspecificar" runat="server" Text="Especificar"
                                    OnClick="btnEspecificar_Click" ValidationGroup="ValidarDeclaracionPersona" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>
