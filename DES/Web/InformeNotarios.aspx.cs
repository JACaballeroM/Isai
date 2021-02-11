using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;


/// <summary>
/// Clase encargada de generar el informe de sobre los notarios
/// </summary>
public partial class InformeNotarios : PageBaseISAI
{
    /// <summary>
    /// Load de la página InformeNotarios
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
                    panel1.Visible = false;
                    rpvNotarios.Visible = false;
                }
               
                //Obtenemos el valor de los parámetros
                HiddenNumNotario.Value = Utilidades.GetParametroUrl(Constantes.PAR_NUMNOTARIO);
                HiddenFechaIni.Value = Utilidades.GetParametroUrl(Constantes.PAR_FECHAINI);
                HiddenFechaFin.Value = Utilidades.GetParametroUrl(Constantes.PAR_FECHAFIN);

                if (string.IsNullOrEmpty(HiddenFechaFin.Value))
                {
                    //Seleccionamos un rango de fechas por defecto de 1 mes
                    txtFechaIni.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                    txtFechaFin.Text = DateTime.Now.AddDays(+1).ToShortDateString();
                    //Ocultamos el report hasta que se agrege un DataSource
                    rpvNotarios.Visible = false;
                }
                else
                {
                    txtNumeroNotario.Text = HiddenNumNotario.Value;
                    txtFechaIni.Text = HiddenFechaIni.Value;
                    txtFechaFin.Text = HiddenFechaFin.Value;
                    CargarInforme();
                }
                HiddenNumNotario.Value = txtNumeroNotario.Text;
                HiddenFechaIni.Value = txtFechaIni.Text;
                HiddenFechaFin.Value = txtFechaFin.Text;
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
    /// Método que obtiene los datos a mostrar en el informe con los datos introducidos en el filtro
    /// </summary>
    private void CargarInforme()
    {
        rpvNotarios.Visible = true;

        //Declaramos el DataTable
        ServiceDeclaracionIsai.DseInfNotarios.FEXNOT_INFONOTARIOSDataTable infoNotariosDT;

        //Limpiamos el origen de datos.
        rpvNotarios.LocalReport.DataSources.Clear();
        string busqueda = "";

        //Obtenemos los notarios asociados a los datos introducidos.
        if (string.IsNullOrEmpty(txtNumeroNotario.Text.ToString()))
        {
            infoNotariosDT = ClienteDeclaracionIsai.ObtenerInfNotarios(null, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()));
            busqueda = String.Format("Búsqueda en el intervalo de fechas: {0}-{1}", txtFechaIni.Text.ToString(), txtFechaFin.Text.ToString());
        }
        else
        {
            infoNotariosDT = ClienteDeclaracionIsai.ObtenerInfNotarios(decimal.Parse(txtNumeroNotario.Text.ToString()), DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()));
        }
        
        //si los campos calculados son nulos se le asigna el valor cero
        foreach (ServiceDeclaracionIsai.DseInfNotarios.FEXNOT_INFONOTARIOSRow row in infoNotariosDT)
        {
            if (row.IsTOTAL_DECLARACIONESNull())
                row.TOTAL_DECLARACIONES = 0;
            if (row.IsTOTAL_FUERA_AVALUONull())
                row.TOTAL_FUERA_AVALUO = 0;
            if (row.IsTOTAL_FUERA_HABILESNull())
                row.TOTAL_FUERA_HABILES = 0;
             
        }

        busqueda = String.Format("Búsqueda por Nº notario: {0}{1}Búsqueda en el intervalo de fechas: {2}-{3}", txtNumeroNotario.Text.ToString(), System.Environment.NewLine, txtFechaIni.Text.ToString(), txtFechaFin.Text.ToString());

        //Declaramos un nuevo reportDatasource de tipo InfoNotarios para introducirlo en el reportviewer
        ReportDataSource reportDataSourceNotarios = new ReportDataSource("DataSet1", (DataTable)setDataset( infoNotariosDT ) );
        ReportDataSource reportDataSourceNotarios2 = new ReportDataSource("FEXNOT_INFONOTARIOS", (DataTable)infoNotariosDT);

        //Creamos los parametros de la pagina de detalle de los totales
        rpvNotarios.LocalReport.EnableHyperlinks = true;
        
        string fechaInicial = "&fechaIni=";
        string fechaFinal = "&fechaFin=";

        ReportParameter urlParameterFechaInicial = new ReportParameter("url_parameter_fechIni", fechaInicial);
        ReportParameter urlParameterDatoFechaInicial = new ReportParameter("url_parameter_datofechIni", HiddenFechaIni.Value);
        ReportParameter urlParameterFechaFinal = new ReportParameter("url_parameter_fechFin", fechaFinal);
        ReportParameter urlParameterDatoFechaFinal = new ReportParameter("url_parameter_datofechFin", HiddenFechaFin.Value);
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { urlParameterFechaInicial });
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { urlParameterDatoFechaInicial });
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { urlParameterFechaFinal });
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { urlParameterDatoFechaFinal });

        string importeNot = "";
        ReportParameter importeNotificado = new ReportParameter("importeNotificado", importeNot);
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { importeNotificado });

        string url180 = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeNotariosMayor180.aspx?searchnot=" + txtNumeroNotario.Text + "&numNotario="));
        ReportParameter urlParameterMayor180 = new ReportParameter("url_parameter_mayor180", url180);
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { urlParameterMayor180 });

        string url15 = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeNotariosMayor15.aspx?searchnot=" + txtNumeroNotario.Text + "&numNotario="));
        ReportParameter urlParameterMayor15 = new ReportParameter("url_parameter_mayor15", url15);
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { urlParameterMayor15 });

        string urlTotal = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeNotariosTotales.aspx?searchnot=" + txtNumeroNotario.Text + "&numNotario="));
        ReportParameter urlParameterTotales = new ReportParameter("url_parameter_totales", urlTotal);
        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { urlParameterTotales });

        ReportParameter busquedaParameter = new ReportParameter("busqueda_parameter", busqueda);

        rpvNotarios.LocalReport.SetParameters(new ReportParameter[1] { busquedaParameter });
       
        //Añadimos el reportDataSource al report
        rpvNotarios.LocalReport.DataSources.Add(reportDataSourceNotarios);
        rpvNotarios.LocalReport.DataSources.Add(reportDataSourceNotarios2);
        rpvNotarios.DataBind();
     
    }


    protected FEXNOT_INFONOTARIOS.FEXNOT_INFONOTARIOSDataTable setDataset(ServiceDeclaracionIsai.DseInfNotarios.FEXNOT_INFONOTARIOSDataTable infoNotariosDT)
    {
        FEXNOT_INFONOTARIOS.FEXNOT_INFONOTARIOSDataTable newTable = new FEXNOT_INFONOTARIOS.FEXNOT_INFONOTARIOSDataTable();
        foreach (var row in infoNotariosDT)
        {
            FEXNOT_INFONOTARIOS.FEXNOT_INFONOTARIOSRow newRow = newTable.NewFEXNOT_INFONOTARIOSRow(); ;
            newRow.APELLIDOMATERNO = row.APELLIDOMATERNO;
            newRow.APELLIDOPATERNO = row.APELLIDOPATERNO;
            newRow.DIFERENCIA_IMPUESTO = row.DIFERENCIA_IMPUESTO;
            newRow.ESTDECLARA = row.ESTDECLARA;
            newRow.IMPORTE_NOTIFICADO = row.IMPORTE_NOTIFICADO;
            newRow.NOMBRE = row.NOMBRE;
            newRow.NUMNOTARIO = row.NUMNOTARIO.ToString();
            newRow.TOTAL_DECLARACIONES = row.TOTAL_DECLARACIONES;
            newRow.TOTAL_FUERA_AVALUO = row.TOTAL_FUERA_AVALUO;
            newRow.TOTAL_FUERA_HABILES = row.TOTAL_FUERA_HABILES;
            newTable.AddFEXNOT_INFONOTARIOSRow(newRow);
        }
        return newTable;
    }
    /// <summary>
    /// Maneja el evento click del control btnBuscar
    /// Evento que filtrará por los valores introducidos y mostrará un report de infoNotarios con información.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!this.IsValid)
                return;

            HiddenNumNotario.Value = txtNumeroNotario.Text;
            HiddenFechaIni.Value = txtFechaIni.Text;
            HiddenFechaFin.Value = txtFechaFin.Text;
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
