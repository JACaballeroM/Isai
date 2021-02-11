<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true" CodeFile="Informes.aspx.cs" Inherits="Informes" %>
<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc1" %>
<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderDImagen" Runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderDRuta" Runat="Server">
    <uc1:Navegacion ID="navegacion" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDContenido" Runat="Server" >
    <fieldset class="formulario">
    <legend class="formulario">Informes</legend>
    <br />
    <asp:Label ID="Label2" runat="server" Text="" ><p>Existen varios informes los cuales están orientados a la consulta de datos de las declaraciones de Isai registradas en el sistema, permitiendo establecer conclusiones. Dependiendo de la información a consultar, se debe utilizar uno u otro informe. Los informes existentes son los siguientes:</p>
<p>-	Específica: Este informe muestra el listado de declaraciones ISAI que cumplen con uno o varios criterios de la búsqueda
<p>-	Peritos / Sociedades: Este informe muestra el listado de sociedades o peritos que han realizado avalúos de tipo comercial, aportando ciertos valores, en base a los criterios de búsqueda</p>
<p>-	Notarios: Este informe muestra información basada en declaraciones realizadas en un periodo de tiempo por los diferentes notarios o notario en caso de especificar uno concreto </p>
<p>-	Envíos totales: Este informe muestra información referente al número de avalúos que los peritos han enviado a los notarios y número de declaraciones ISAI realizadas en base a un avalúo (no se consideran las declaraciones de jornada notarial por no basarse en avalúo). </p>
<p>-	Valores unitarios: Este informe muestra información sobre valores unitarios, es decir, del estudio de mercado de rentas y ventas que se realiza en cada avalúo. Muestra un listado de las cuentas que se han utilizado. Como una cuenta puede haberse utilizado en varios avalúos, saldrá la información de cada una de ellas </p>
<p>-	Líneas de captura: Este informe muestra información basada en líneas de captura de declaraciones realizadas por los diferentes notarios, en un periodo de tiempo concreto </p>
<p>-	Pagos: Este informe muestra información del estado de pago de las declaraciones. </p><p>Los informes están accesibles desde el menú lateral izquierda.</p></asp:Label>
    <br />
    </fieldset>

</asp:Content>
<asp:Content ID="ContentMenu" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal"
    runat="Server">
    <uc2:menulocal ID="MenuLocal1" runat="server" />
</asp:Content>

