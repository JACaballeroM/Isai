using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;

/// <summary>
/// Clase encargada de generar el informe de envios totales
/// </summary>
public partial class InformeEnviosTotales : PageBaseISAI
{

    /// <summary>
    /// 
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
                    rpvEnviosTotales.Visible = false;
                }
                //Ocultamos el report hasta que se agrege un DataSource
                rpvEnviosTotales.Visible = false;
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
    /// Maneja el evento click del control btnBuscar
    /// Obtiene los datos para restringir la búsqueda introducidos y consulta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!this.IsValid)
                return;
            rpvEnviosTotales.Visible = true;
            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfEnviosTotales dseInfEnviosTotales;
            //Limpiamos el origen de datos.
            rpvEnviosTotales.LocalReport.DataSources.Clear();
            //Obtenemos los EnviosTotales asociados a los datos introducidos.
            dseInfEnviosTotales = ClienteDeclaracionIsai.ObtenerInfEnviosTotales(decimal.Parse(txtAño.Text.ToString()));
            string busqueda = "";
            busqueda = String.Format("{0}", txtAño.Text);
            ReportParameter busquedaParameter = new ReportParameter("busqueda_parameter", busqueda);
            rpvEnviosTotales.LocalReport.SetParameters(new ReportParameter[1] { busquedaParameter });
            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceAvaluos = new ReportDataSource("FEXNOT_INFOAVALUOSTOTALES", (DataTable)dseInfEnviosTotales.FEXNOT_INFOAVALUOSTOTALES);
            ReportDataSource reportDataSourceDeclaracion = new ReportDataSource("FEXNOT_INFODECLARACTOTALES", (DataTable)dseInfEnviosTotales.FEXNOT_INFODECLARACTOTALES);
            //Añadimos el reportDataSource al reportViewer
            rpvEnviosTotales.LocalReport.DataSources.Add(reportDataSourceAvaluos);
            rpvEnviosTotales.LocalReport.DataSources.Add(reportDataSourceDeclaracion);
            rpvEnviosTotales.DataBind();
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
