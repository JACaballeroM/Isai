using System;
using System.Data;
using System.ServiceModel;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;

/// <summary>
/// Clase encargada de generar el informe de detalle de la consulta específica
/// </summary>
public partial class InformeConsultaEspecificaDetalle : PageBaseISAI
{
  
    /// <summary>
    /// Enumerador para el tipo de Filtro
    /// 0. Cuenta catastral
    /// 1. Persona
    /// 2. Ubicacion
    /// </summary>
    protected enum TipoFiltroBusqueda
    {
        CuentaCatastral = 0,
        Persona = 1,
        Ubicacion = 2
    }

    /// <summary>
    /// Load de la página InformeConsultaEspecificaDetalle.
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
                    rpvConsultaEspecifica.Visible = false;
                }
                //Obtenemos el idDeclaracion
                string idDeclaracion = Utilidades.GetParametroUrl(Constantes.PAR_IDDECLARACION);

                //Declaramos el DataTable
                ServiceDeclaracionIsai.DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable infConsultaEspecificaDT;

                //Limpiamos el origen de datos.
                rpvConsultaEspecifica.LocalReport.DataSources.Clear();

                //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
                infConsultaEspecificaDT = null;

                infConsultaEspecificaDT = ClienteDeclaracionIsai.ObtenerInfConsultaEspecificaByIdDeclaracion(decimal.Parse(idDeclaracion));

                //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
                ReportDataSource reportDataSourceConsultaEspecifica = new ReportDataSource("FEXNOT_INFOCONSESPECIFICA", (DataTable)infConsultaEspecificaDT);

                //Añadimos el reportDataSource al reportViewer
                rpvConsultaEspecifica.LocalReport.DataSources.Add(reportDataSourceConsultaEspecifica);
                CargarParticipantes(decimal.Parse(idDeclaracion));
                rpvConsultaEspecifica.DataBind();

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
    /// Maneja el evento click del control btnVolver
    /// Nos redirige a la página referida (PAGINA ORIGEN)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVolver_Click(object sender, EventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_CONSULTAESPECIFICA;
            RedirectUtil.AddParameter(Constantes.PAR_VAL_REGION, Utilidades.GetParametroUrl(Constantes.PAR_VAL_REGION));
            RedirectUtil.AddParameter(Constantes.PAR_VAL_MANZANA, Utilidades.GetParametroUrl(Constantes.PAR_VAL_MANZANA));
            RedirectUtil.AddParameter(Constantes.PAR_VAL_LOTE, Utilidades.GetParametroUrl(Constantes.PAR_VAL_LOTE));
            RedirectUtil.AddParameter(Constantes.PAR_VAL_UNIDAD, Utilidades.GetParametroUrl(Constantes.PAR_VAL_UNIDAD));
            RedirectUtil.AddParameter(Constantes.PAR_VAL_CALLE, Utilidades.GetParametroUrl(Constantes.PAR_VAL_CALLE));
            RedirectUtil.AddParameter(Constantes.PAR_VAL_SUJETO, Server.UrlDecode(Utilidades.GetParametroUrl(Constantes.PAR_VAL_SUJETO)));
            RedirectUtil.AddParameter(Constantes.PAR_VAL_INT, Utilidades.GetParametroUrl(Constantes.PAR_VAL_INT));
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
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

    protected void CargarParticipantes(decimal idDeclaracion)
    {
        try
        {
            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfJustificante.FEXNOT_INF_ACUSEPAR_PDataTable infAcusePArDT;


            //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio button seleccionado
            infAcusePArDT = null;
            infAcusePArDT = ClienteDeclaracionIsai.ObtenerInfAcusePar(idDeclaracion);


            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceAcusePar = new ReportDataSource("FEXNOT_INF_ACUSEPAR_P", (DataTable)infAcusePArDT);

            //Añadimos el reportDataSource al reportViewer
            rpvConsultaEspecifica.LocalReport.DataSources.Add(reportDataSourceAcusePar);
            
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ex.Message;
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
