<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalInfoExcepcion.ascx.cs" Inherits="UserControlsCommon_ModalInfoExcepcion" %>

<table cellpadding="0" cellspacing="0" class="formularioPopUp">
    <tr id="trCabecera" runat="server" class="TablaCabeceraCaja">
        <td class="TextLeftTop" colspan="2">
            <asp:Label ID="lblTextoTitulo" runat="server" Skinid="None" Text="Error"/>
        </td>
    </tr>
    <tr>
        <td style="padding:10px;">
            <asp:Image ID="imgError" runat="server" ImageUrl="~/Images/error.gif" Width="48px" Height="48px"/>
        </td>
        <td style="width:175px; padding:10px;">
            <asp:Label ID="lblErrorBasico" runat="server"/>
        </td>
    </tr>
    <tr>
        <td style="padding-left:5px; padding-right:5px; padding-bottom:5px; text-align:center;" colspan="2">
            <asp:Button runat="server" ID="btnErrorAceptar" CausesValidation="false" 
                ToolTip="Pulsando aceptará la acción" Text="Aceptar" />
            &nbsp;
            <asp:Button runat="server" ID="btnErrorMostrar" CausesValidation="false" 
            ToolTip="Pulsando mostrará u ocultará los detalles"/>
        </td>  
    </tr>
    <tr>      
        <td colspan="2" style="width:250px; padding:10px;">
            <asp:Panel ID="panError" runat="server">          
                <asp:Label ID="lblErrorAvanzado" runat="server"/>
            </asp:Panel>
        </td>
    </tr>
    
</table>
