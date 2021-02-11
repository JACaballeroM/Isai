﻿using System;
using System.Data;
using System.ServiceModel;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;

/// <summary>
/// Clase encargada de generar el informe sobre los notarios con las declaraciones mayor a 6 meses en avaluo
/// </summary>
public partial class InformeNotariosMayor180 : PageBaseISAI
{

    /// <summary>
    /// Load de la página InformeNotariosMayor180
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
                //Obtenemos el valor de los parametros
                HiddenNotario.Value = Utilidades.GetParametroUrl(Constantes.PAR_SEARCHNOT);
                lblNumNotarioDato.Text = Utilidades.GetParametroUrl(Constantes.PAR_NUMNOTARIO);
                lblFechaIniDato.Text = Utilidades.GetParametroUrl(Constantes.PAR_FECHAINI);
                lblFechaFinDato.Text = Utilidades.GetParametroUrl(Constantes.PAR_FECHAFIN);

                //Declaramos el DataTable
                ServiceDeclaracionIsai.DseInfDetalleNotarios.FEXNOT_INFODETALLENOTARIOSDataTable infDetalleNotariosDT;

                //Limpiamos el origen de datos.
                rpvNotarios.LocalReport.DataSources.Clear();

                //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
                infDetalleNotariosDT = null;

                if (!string.IsNullOrEmpty(lblNumNotarioDato.Text))
                {
                    infDetalleNotariosDT = ClienteDeclaracionIsai.ObtenerInfNotariosMayor180(decimal.Parse(lblNumNotarioDato.Text.ToString()), DateTime.Parse(lblFechaIniDato.Text.ToString()), DateTime.Parse(lblFechaFinDato.Text.ToString()));
                }
                else
                {
                    infDetalleNotariosDT = ClienteDeclaracionIsai.ObtenerInfNotariosMayor180(null, DateTime.Parse(lblFechaIniDato.Text.ToString()), DateTime.Parse(lblFechaFinDato.Text.ToString()));
                }

                //Declaramos nuevos reportDatasource de tipo infDetalleLineasCapturaDT para introducirlo en el reportviewer
                ReportDataSource reportDataSourceDetalleNotarios = new ReportDataSource("FEXNOT_INFODETALLENOTARIOS", (DataTable)infDetalleNotariosDT);
                ReportParameter numero = new ReportParameter("Numero", lblNumNotarioDato.Text);// Utilidades.GetParametroUrl(Constantes.PAR_NOMBRENOTARIO));
                rpvNotarios.LocalReport.SetParameters(numero);
                //Añadimos el reportDataSource al reportViewer
                rpvNotarios.LocalReport.DataSources.Add(reportDataSourceDetalleNotarios);
                rpvNotarios.DataBind();
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
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_NOTARIOS;

            if (HiddenNotario.Value.CompareTo(lblNumNotarioDato.Text) == 0)
            {
                RedirectUtil.AddParameter(Constantes.PAR_NUMNOTARIO, lblNumNotarioDato.Text);
            }
            else
            {
                RedirectUtil.AddParameter(Constantes.PAR_NUMNOTARIO, HiddenNotario.Value);
            }

            RedirectUtil.AddParameter(Constantes.PAR_FECHAINI, lblFechaIniDato.Text);
            RedirectUtil.AddParameter(Constantes.PAR_FECHAFIN, lblFechaFinDato.Text);
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
