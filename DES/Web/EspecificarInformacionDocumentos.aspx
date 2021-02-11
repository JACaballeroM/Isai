<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EspecificarInformacionDocumentos.aspx.cs"
    Inherits="EspecificarInformacionDocumentos" Title="Especificar documento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <iframe id="ifrEspecificar" src="<%=System.Configuration.ConfigurationManager.AppSettings["UrlEspecificarDocumentos"]%>?IdTipoDocumentoDigital=<%=Request.QueryString["IdTipoDocumentoDigital"]%>&IdTipoDocumentoJuridico=<%=Request.QueryString["IdTipoDocumentoJuridico"]%>&Transaccional=<%=Request.QueryString["Transaccional"]%>&IdDocumentoDigital=<%=Request.QueryString["IdDocumentoDigital"]%>&IdNotario=<%=Request.QueryString["IdNotario"]%>&Edicion=<%=Request.QueryString["Edicion"]%>&NombrePagina=<%=Request.QueryString["NombrePagina"]%>"
        frameborder="0" width="640px" height="480px">
        <p>
            Tu navegador no soporta iframe.</p>
    </iframe>
</body>
</html>
