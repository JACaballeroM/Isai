<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IFrameEspecificarDireccion.aspx.cs" Inherits="IFrameEspecificarDireccion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Especificar Información Dirección</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <iframe id="ifrEspecificar" src ="<%=System.Web.Configuration.WebConfigurationManager.AppSettings["UrlDireccion"].ToString()%>?ReadOnly=<%=Request.QueryString["ReadOnly"]%>&IdDireccion=<%=Request.QueryString["IdDireccion"]%>" frameborder="0" width="640px" height="550px">
            <p>Tu navegador no soporta iframe.</p>
        </iframe>
    </div>
    </form>
</body>
</html>
