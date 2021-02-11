using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;

/// <summary>
/// Clase que genera el informe de valores unitarios de renta o de mercado del terreno
/// </summary>
public partial class InformeValores : PageBaseISAI
{
    /// <summary>
    /// Load de la página InformeValores
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
                    Panel1.Visible = false;
                    Panel2.Visible = false;
                    rpvValores.Visible = false;
                }
                
                //Seleccionamos un rango de fechas por defecto de 1 mes
                txtFechaIni.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaFin.Text = DateTime.Now.ToShortDateString();

                //Ocultamos el report hasta que se agrege un DataSource
                rpvValores.Visible = false;
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
    /// Realiza la consulta con los parámetros introducidos en el filtro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!this.IsValid)
                return;

            string tipoValorUnitario = string.Empty;

            //Dependiendo del Tipo de valor unitario introducimos R (Renta) o V (Venta)
            if (rbMercado.Checked)
                tipoValorUnitario = Constantes.PAR_VAL_VALOR_VENTA;
            else if (rbRenta.Checked)
                tipoValorUnitario = Constantes.PAR_VAL_VALOR_RENTA;

            //Comprobacion del campo ColoniaCatastral
            decimal? idColonia = null;
            if (!string.IsNullOrEmpty(txtColoniaCatastral.Text))
                idColonia = decimal.Parse(txtColoniaCatastral.Text.ToString());

            rpvValores.Visible = true;

            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfValores.FEXNOT_INFOVALORESDataTable infoValoresDT;

            //Limpiamos el origen de datos.
            rpvValores.LocalReport.DataSources.Clear();

            //Obtenemos los EnviosTotales asociados a los datos introducidos.
            infoValoresDT = ClienteDeclaracionIsai.ObtenerInfValores(DateTime.Parse(txtFechaIni.Text.ToString()),
                                                                    DateTime.Parse(txtFechaFin.Text.ToString()),
                                                                    tipoValorUnitario,
                                                                    txtNumSociedad.Text.ToString(),
                                                                    txtNumPerito.Text.ToString(),
                                                                    txtColoniaNominal.Text.ToString(),
                                                                    idColonia);
            string busqueda = "";
            if (!string.IsNullOrEmpty(txtNumSociedad.Text.ToString()))
            {
                busqueda = String.Format("Búsqueda por Nº sociedad: {0}{1}", txtNumSociedad.Text.ToString(), System.Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(txtNumPerito.Text.ToString()))
            {
                busqueda += String.Format("Búsqueda por Nº perito: {0}{1}", txtNumPerito.Text.ToString(), System.Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(txtColoniaNominal.Text.ToString()))
            {
                busqueda += String.Format("Búsqueda por colonia nominal: {0}{1}", txtColoniaNominal.Text.ToString(), System.Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(txtColoniaCatastral.Text.ToString()))
            {
                busqueda += String.Format("Búsqueda por colonia catastral: {0}{1}", txtColoniaCatastral.Text.ToString(), System.Environment.NewLine);
            }
            if (rbMercado.Checked)
            {
                busqueda += String.Format("Tipo de valor unitario: {0}{1}", rbMercado.Text.ToString(), System.Environment.NewLine);

            }
            else
            {
                busqueda += String.Format("Tipo de valor unitario: {0}{1}", rbRenta.Text.ToString(), System.Environment.NewLine);

            }
            busqueda += String.Format("Búsqueda en el intervalo de fechas: {0}-{1}", txtFechaIni.Text.ToString(), txtFechaFin.Text.ToString());
            ReportParameter busquedaParameter = new ReportParameter("busqueda_parameter", busqueda);
            rpvValores.LocalReport.SetParameters(new ReportParameter[1] { busquedaParameter });
            
            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceValores = new ReportDataSource("DseInfValores_FEXNOT_INFOVALORES", (DataTable)infoValoresDT);

            //Añadimos el reportDataSource al reportViewer
            rpvValores.LocalReport.DataSources.Add(reportDataSourceValores);
            rpvValores.DataBind();
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
