using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;


/// <summary>
/// Clase encargada de generar el informe de líneas de captura
/// </summary>
public partial class InformeLineasCaptura : PageBaseISAI
{
    /// <summary>
    /// Load de la página InformeLineasFactura
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
                    rpvLineasCaptura.Visible = false;
                }
                //Seleccionamos un rango de fechas por defecto de 1 mes
                txtFechaIni.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaFin.Text = DateTime.Now.ToShortDateString();
                //Eliminar las dos filas anteriores
                //Obtenemos el valor de los parametros
                HiddenNumNotario.Value = Utilidades.GetParametroUrl(Constantes.PAR_NUMNOTARIO);
                HiddenFechaIni.Value = Utilidades.GetParametroUrl(Constantes.PAR_FECHAINI);
                HiddenFechaFin.Value = Utilidades.GetParametroUrl(Constantes.PAR_FECHAFIN);
                if (string.IsNullOrEmpty(HiddenFechaFin.Value))
                {
                    //Seleccionamos un rango de fechas por defecto de 1 mes
                    txtFechaIni.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                    txtFechaFin.Text = DateTime.Now.AddDays(+1).ToShortDateString();
                    //Ocultamos el report hasta que se agrege un DataSource
                    rpvLineasCaptura.Visible = false;
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
    /// Método que carga los datos a mostrar en el informe
    /// </summary>
    private void CargarInforme()
    {
        rpvLineasCaptura.Visible = true;

        //Declaramos el DataTable
        ServiceDeclaracionIsai.DseInfoLineasCaptura.FEXNOT_INFOLINEASCAPTURADataTable infoLineasCapturaDT;
        rpvLineasCaptura.LocalReport.EnableHyperlinks = true;
        //Limpiamos el origen de datos.
        rpvLineasCaptura.LocalReport.DataSources.Clear();

        string busqueda = "";
        if (string.IsNullOrEmpty(txtNumeroNotario.Text.ToString()))
        {
            //Obtenemos las lineas de captura filtrando por fecha pero sin filtrar por NOTARIO
            infoLineasCapturaDT = ClienteDeclaracionIsai.ObtenerInfLineasCaptura(null, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()));
            busqueda = String.Format("Búsqueda en el intervalo de fechas: {0}-{1}", txtFechaIni.Text.ToString(), txtFechaFin.Text.ToString());
        }
        else
        {
            //Obtenemos las lineas de captura asociados filtrando por fecha y notario
            infoLineasCapturaDT = ClienteDeclaracionIsai.ObtenerInfLineasCaptura(decimal.Parse(txtNumeroNotario.Text.ToString()), DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()));
            busqueda = String.Format("Búsqueda por Nº notario: {0}{1}Búsqueda en el intervalo de fechas: {2}-{3}", txtNumeroNotario.Text.ToString(), System.Environment.NewLine, txtFechaIni.Text.ToString(), txtFechaFin.Text.ToString());
        }
        ReportParameter busquedaParameter = new ReportParameter("busqueda_parameter", busqueda);
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { busquedaParameter });
        
        //si los campos calculados son nulos se le asigna el valor cero
        foreach (ServiceDeclaracionIsai.DseInfoLineasCaptura.FEXNOT_INFOLINEASCAPTURARow row in infoLineasCapturaDT)
        {
            if (row.IsTOTAL_DECLARACIONESNull())
                row.TOTAL_DECLARACIONES = 0;
            if (row.IsTOTAL_DECLARACIONES_MENORESNull())
                row.TOTAL_DECLARACIONES_MENORES = 0;
            if (row.IsTOTAL_DECLARACIONES_MAYORESNull())
                row.TOTAL_DECLARACIONES_MAYORES = 0;
        }

        //Declaramos un nuevo reportDatasource de tipo InfoLineasCaptura para introducirlo en el reportviewer
        ReportDataSource reportDataSourceLineasFactura = new ReportDataSource("FEXNOT_INFOLINEASCAPTURA", (DataTable)infoLineasCapturaDT);
                
        //Creamos los parametros de la pagina de detalle de los totales
       
        string fechaInicial = "&fechaIni=";
        string fechaFinal = "&fechaFin=";

        ReportParameter urlParameterFechaInicial = new ReportParameter("url_parameter_fechIni", fechaInicial);
        ReportParameter urlParameterDatoFechaInicial = new ReportParameter("url_parameter_datofechIni", HiddenFechaIni.Value);
        ReportParameter urlParameterFechaFinal = new ReportParameter("url_parameter_fechFin", fechaFinal);
        ReportParameter urlParameterDatoFechaFinal = new ReportParameter("url_parameter_datofechFin", HiddenFechaFin.Value);
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { urlParameterFechaInicial });
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { urlParameterDatoFechaInicial });
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { urlParameterFechaFinal });
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { urlParameterDatoFechaFinal });

        string urlLCMenor = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeLineaCapturaMenor.aspx?searchnot=" + txtNumeroNotario.Text + "&numNotario="));
        ReportParameter urlParameterMenor = new ReportParameter("url_parameter_menor", urlLCMenor);
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { urlParameterMenor });

        string urlLCMayor = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeLineaCapturaMayor.aspx?searchnot=" + txtNumeroNotario.Text + "&numNotario="));
        ReportParameter urlParameterMayor = new ReportParameter("url_parameter_mayor", urlLCMayor);
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { urlParameterMayor });

        string urlLCTotales = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeLineaCapturaTotales.aspx?searchnot=" + txtNumeroNotario.Text + "&numNotario="));
        ReportParameter urlParameterTotales = new ReportParameter("url_parameter_totales", urlLCTotales);
        rpvLineasCaptura.LocalReport.SetParameters(new ReportParameter[1] { urlParameterTotales });

        //Añadimos el reportDataSource al report
        rpvLineasCaptura.LocalReport.DataSources.Add(reportDataSourceLineasFactura);
        rpvLineasCaptura.DataBind();
    }

    /// <summary>
    /// Maneja el evento click del control btnBuscar
    /// Evento que filtrará por los valores introducidos y mostrará un report de infoLineaCaptura con información.
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
