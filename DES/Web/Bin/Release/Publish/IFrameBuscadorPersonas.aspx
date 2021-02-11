<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IFrameBuscadorPersonas.aspx.cs" Inherits="IFrameBuscadorPersonas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Buscador Personas</title>
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <iframe id="ifrBuscadorPersonas" src="<%=System.Web.Configuration.WebConfigurationManager.AppSettings["UrlBuscadorPersona"].ToString()%>?TipoBusqueda=<%=Request.QueryString["TipoBusqueda"]%>"
                frameborder="0" width="580px" height="600px">
            <p>Tu navegador no soporta iframe.</p>
        </iframe>
    </div>
    </form>
</body>
</html>
