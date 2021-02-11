using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;


/// <summary>
/// Clase encargada de generar el informe sobre peritos y sociedades
/// </summary>
public partial class InformeSociedadPeritos : PageBaseISAI
{
    /// <summary>
    /// Load de la página InformeSociedadPeritos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!(Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION) || Condiciones.Web(Constantes.FUN_INFORMES)))
                {
                    Panel2.Visible = false;
                    Panel3.Visible = false;
                    //rpvPeritosSociedades.Visible = false;
                }
                //Obtenemos el valor de los parametros
                HiddenTipoEntidad.Value = Utilidades.GetParametroUrl(Constantes.PAR_CODTIPOPERSONA);
                HiddenEstadoDeclaracion.Value = Utilidades.GetParametroUrl(Constantes.PAR_CODESTADODECLARACION);
                HiddenFechaIni.Value = Utilidades.GetParametroUrl(Constantes.PAR_FECHAINI);
                HiddenFechaFin.Value = Utilidades.GetParametroUrl(Constantes.PAR_FECHAFIN);
                HiddenTipoFecha.Value = Utilidades.GetParametroUrl(Constantes.PAR_TIPOFECHA);
                if (string.IsNullOrEmpty(HiddenFechaFin.Value))
                {
                    //Seleccionamos un rango de fechas por defecto de 1 mes
                    txtFechaIni.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                    txtFechaFin.Text = DateTime.Now.ToShortDateString();
                    //Ocultamos el report hasta que se agrege un DataSource
                    //rpvPeritosSociedades.Visible = false;
                }
                else
                {
                    if (HiddenTipoEntidad.Value == Convert.ToString(0))
                    {
                        rbPerito.Checked = true;
                        rbSociedad.Checked = false;
                    }
                    else
                    {
                        rbSociedad.Checked = true;
                        rbPerito.Checked = false;
                    }
                    if (HiddenEstadoDeclaracion.Value == Convert.ToString(0))
                    {
                        rbPagados.Checked = true;
                        rbEnviados.Checked = false;
                        divFechaPago.Visible = true;
                    }
                    else
                    {
                        rbEnviados.Checked = true;
                        rbPagados.Checked = false;
                        divFechaPago.Visible = false;
                    }
                    if (HiddenTipoFecha.Value == Convert.ToString(0))
                    {
                        rbFechaAvaluo.Checked = true;
                        rbFechaPago.Checked = false;
                        rbFechaPres.Checked = false;
                    }
                    else if (HiddenTipoFecha.Value == Convert.ToString(1))
                    {
                        rbFechaAvaluo.Checked = false;
                        rbFechaPago.Checked = false;
                        rbFechaPres.Checked = true;
                    }
                    else
                    {
                        rbFechaAvaluo.Checked = false;
                        rbFechaPago.Checked = true;
                        rbFechaPres.Checked = false;
                    }

                    txtFechaIni.Text = HiddenFechaIni.Value;
                    txtFechaFin.Text = HiddenFechaFin.Value;
                    
                    CargarInforme();
                }
            }
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    /// <summary>
    /// Método que carga los datos obtenidos de la consulta con el filtro introducido
    /// </summary>
    private void CargarInforme()
    {
        rpvPeritosSociedades.Visible = true;

        decimal tipoEntidad = 0;
        decimal estadoDeclaracion = 0;
        decimal tipoFecha = 0;
        if (rbPerito.Checked)
            tipoEntidad = 0;
        else if (rbSociedad.Checked)
            tipoEntidad = 1;
        string busqueda = "";
        if (rbPagados.Checked)
        {
            estadoDeclaracion = 0;
            busqueda = string.Format("Búsqueda por: Estado declaraciones pagadas");
        }
        else if (rbEnviados.Checked)
        {
            estadoDeclaracion = 1;
            busqueda = string.Format("Búsqueda por: Estado declaraciones enviadas");
        }

        if (rbFechaAvaluo.Checked)
        {
            tipoFecha = 0;
        }
        else if (rbFechaPres.Checked)
        {
            tipoFecha = 1;
        }
        else if (rbFechaPago.Checked)
        {
            tipoFecha = 2;
        }

        //Declaramos el DataTable
        ServiceDeclaracionIsai.DseInfSociedadPeritos.FEXNOT_INFOSOCIEDADPERITOSDataTable infSociedadPeritosDT;

        //Limpiamos el origen de datos.
        rpvPeritosSociedades.LocalReport.DataSources.Clear();

        //Obtenemos los notarios asociados a los datos introducidos.
        infSociedadPeritosDT = ClienteDeclaracionIsai.ObtenerInfSociedadPeritos(DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()), estadoDeclaracion, tipoEntidad, tipoFecha);

        //Declaramos un nuevo reportDatasource de tipo InfoNotarios para introducirlo en el reportviewer
        var socPer = infSociedadPeritosDT.Where(a => a.NUMEROAVALUOS != 0 && a.NUMEROCOLONIAS != 0);
        ReportDataSource reportDataSourceSociedadPeritos = new ReportDataSource("FEXNOT_INFOSOCIEDADPERITOS", socPer);

        //Añadimos el reportDataSource al report
        rpvPeritosSociedades.LocalReport.DataSources.Add(reportDataSourceSociedadPeritos);


        busqueda += string.Format(" entre las fechas: {0}-{1}", txtFechaIni.Text.ToString(), txtFechaFin.Text.ToString());
        ReportParameter busquedaParameter = new ReportParameter("busqueda_parameter", busqueda);
        rpvPeritosSociedades.LocalReport.SetParameters(busquedaParameter);
        ReportParameter fechaIniParameter = new ReportParameter("fechaIni_parameter", txtFechaIni.Text.ToString());
        rpvPeritosSociedades.LocalReport.SetParameters(fechaIniParameter);
        ReportParameter fechaFinParameter = new ReportParameter("fechaFin_parameter", txtFechaFin.Text.ToString());
        rpvPeritosSociedades.LocalReport.SetParameters(fechaFinParameter);
        ReportParameter estadoParameter = new ReportParameter("estado_parameter", estadoDeclaracion.ToString());
        rpvPeritosSociedades.LocalReport.SetParameters(estadoParameter);
        ReportParameter entidadParameter = new ReportParameter("entidad_parameter", tipoEntidad.ToString());
        rpvPeritosSociedades.LocalReport.SetParameters(entidadParameter);
        ReportParameter tipoFechaParameter = new ReportParameter("tipo_parameter", tipoFecha.ToString());
        rpvPeritosSociedades.LocalReport.SetParameters(tipoFechaParameter);

        string url = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeSociedadPeritosDetalle.aspx?codTipoPersona=" + tipoEntidad + "&CodEstadoDeclaracion=" + estadoDeclaracion + "&fechaIni=" + txtFechaIni.Text + "&fechaFin=" + txtFechaFin.Text + "&tipoFecha=" + tipoFecha));

        ReportParameter urlParameter = new ReportParameter("url_parameter", url);
        rpvPeritosSociedades.LocalReport.SetParameters(urlParameter);

        rpvPeritosSociedades.DataBind();
    }

    /// <summary>
    /// Maneja el evento click del control btnBuscar
    /// Realiza las búsqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!this.IsValid)
                return;

            CargarInforme();
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }


    /// <summary>
    /// Maneja el evento CheckedChanged del control rbPagados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbPagados_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbPagados.Checked)
                divFechaPago.Visible = true;
            else divFechaPago.Visible = false;
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    /// <summary>
    /// Maneja el evento checkedChanged del control rbEnviados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbEnviados_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbPagados.Checked)
                divFechaPago.Visible = true;
            else divFechaPago.Visible = false;
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    #region Excepciones

    protected void Page_Prerender(object sender, EventArgs e)
    {
        try
        {
            //Se establece el botón de cancelar para los modalpopupextenders
            mpeErrorTareas.CancelControlID = errorTareas.ClientIdCancelacion;
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion

}
