using System;
using System.Data;
using System.ServiceModel;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;

/// <summary>
/// Clase encargada de generar el informe sobre peritos y sociedades detallado
/// </summary>
public partial class InformeSociedadPeritosDetalle : PageBaseISAI
{

    /// <summary>
    /// Load de la página InformeSociedadPeritosDetalle
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
                    rpvPeritosSociedades.Visible = false;
                }
                
                //Obtenemos el valor de los parametros
                ReportParameter tipoPersona = new ReportParameter("entidad_parameter", Utilidades.GetParametroUrl(Constantes.PAR_CODTIPOPERSONA));

                if (Utilidades.GetParametroUrl(Constantes.PAR_CODTIPOPERSONA) == "0")
                {
                    lblTipoEntidadDato.Text = "Perito";
                }
                else lblTipoEntidadDato.Text = "Sociedad";
                if (Utilidades.GetParametroUrl(Constantes.PAR_CODESTADODECLARACION) == "0")
                {
                    lblEstadoDato.Text = "Pagada";
                }
                else lblEstadoDato.Text = "Enviada";
                lblFechaIniDato.Text = Utilidades.GetParametroUrl("fechaIni");
                lblFechaFinDato.Text = Utilidades.GetParametroUrl(Constantes.PAR_FECHAFIN);
                lblClaveDato.Text = Utilidades.GetParametroUrl(Constantes.PAR_REGISTRO);
                if (Utilidades.GetParametroUrl(Constantes.PAR_TIPOFECHA) == "0")
                {
                    lblFechas.Text = "Fecha avalúo: ";
                }
                else if (Utilidades.GetParametroUrl(Constantes.PAR_TIPOFECHA) == "1")
                {
                    lblFechas.Text = "Fecha presentación: ";
                }
                else
                {
                    lblFechas.Text = "Fecha pago: ";
                }
                //Declaramos el DataTable
                ServiceDeclaracionIsai.DseInfAvaluos.FEXNOT_INF_SOCPERAVALUOS_PDataTable infAvaluosDT;

                //Limpiamos el origen de datos.
                rpvPeritosSociedades.LocalReport.DataSources.Clear();

                //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
                infAvaluosDT = null;
            
                infAvaluosDT = ClienteDeclaracionIsai.ObtenerInfSociedadPeritosAvaluos(DateTime.Parse(lblFechaIniDato.Text.ToString()),
                                                                                       DateTime.Parse(lblFechaFinDato.Text.ToString()),
                                                                                       Convert.ToDecimal(Utilidades.GetParametroUrl(Constantes.PAR_CODESTADODECLARACION)),
                                                                                       Convert.ToDecimal(Utilidades.GetParametroUrl(Constantes.PAR_CODTIPOPERSONA)),
                                                                                       Convert.ToDecimal(Utilidades.GetParametroUrl(Constantes.PAR_TIPOFECHA)),
                                                                                       lblClaveDato.Text);

                //Declaramos nuevos reportDatasource de tipo infDetalleLineasCapturaDT para introducirlo en el reportviewer
                ReportDataSource reportDataSourceDetalleAvaluos = new ReportDataSource("DseInfAvaluos_FEXNOT_INF_SOCPERAVALUOS_P", (DataTable)infAvaluosDT);

                //Añadimos el reportDataSource al reportViewer
                rpvPeritosSociedades.LocalReport.DataSources.Add(reportDataSourceDetalleAvaluos);
                rpvPeritosSociedades.LocalReport.SetParameters(new ReportParameter[1] { tipoPersona });
                rpvPeritosSociedades.DataBind();
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
    /// Nos redirige a la pagina referida (PAGINA ORIGEN)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVolver_Click(object sender, EventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_SOCIEDAD;
            RedirectUtil.AddParameter(Constantes.PAR_FECHAINI, lblFechaIniDato.Text);
            RedirectUtil.AddParameter(Constantes.PAR_FECHAFIN, lblFechaFinDato.Text);
            RedirectUtil.AddParameter(Constantes.PAR_REGISTRO, lblClaveDato.Text);
            RedirectUtil.AddParameter(Constantes.PAR_CODESTADODECLARACION, Utilidades.GetParametroUrl(Constantes.PAR_CODESTADODECLARACION));
            RedirectUtil.AddParameter(Constantes.PAR_CODTIPOPERSONA, Utilidades.GetParametroUrl(Constantes.PAR_CODTIPOPERSONA));
            RedirectUtil.AddParameter(Constantes.PAR_TIPOFECHA, Utilidades.GetParametroUrl(Constantes.PAR_TIPOFECHA));
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
