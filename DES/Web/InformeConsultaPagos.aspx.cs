using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;


/// <summary>
/// Clase encargada de generar el informe de consulta de pagos
/// </summary>
public partial class InformeConsultaPagos : PageBaseISAI
{
    #region Propiedades


    /// <summary>
    /// Enumerador de los tipos de filtro
    /// 0. Conformados
    /// 1. Vencidos
    /// </summary>
    protected enum TipoFiltroBusqueda
    {
        Conformados = 0,
        Vencidos = 1
    }
    #endregion

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
                    UpdatePanelBusqueda.Visible = false;
                    rpvPagosCon.Visible = false;
                }
                //Seleccionamos un rango de fechas por defecto de 1 mes
                txtFechaIni.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaFin.Text = DateTime.Now.ToShortDateString();
                //Ocultamos el report hasta que se agrege un DataSource
                rpvPagosCon.Visible = false;
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
    /// Evento que mostrará el Informe de Consulta especifica filtrado según los datos introducidos.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!this.IsValid)
                return;
            rpvPagosCon.Visible = true;
            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfPagos.FEXNOT_INFPAGOSDataTable infConsultaPagosDT;
            //Limpiamos el origen de datos.
            rpvPagosCon.LocalReport.DataSources.Clear();
            //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
            infConsultaPagosDT = null;
            if (rbConformados.Checked == true)
                if (!string.IsNullOrEmpty(txtNumDeclaracion.Text))
                    infConsultaPagosDT = ClienteDeclaracionIsai.ObtenerInfConsultaPagos(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text, txtNumDeclaracion.Text, txtLineaCaptura.Text, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()), "C");
                else
                    infConsultaPagosDT = ClienteDeclaracionIsai.ObtenerInfConsultaPagos(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text, null, txtLineaCaptura.Text, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()), "C");
            else if (rbVencidos.Checked == true)
                if (!string.IsNullOrEmpty(txtNumDeclaracion.Text))
                    infConsultaPagosDT = ClienteDeclaracionIsai.ObtenerInfConsultaPagos(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text, txtNumDeclaracion.Text, txtLineaCaptura.Text, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()), "V");
                else
                    infConsultaPagosDT = ClienteDeclaracionIsai.ObtenerInfConsultaPagos(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text, null, txtLineaCaptura.Text, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()), "V");
            else if (rbSiscor.Checked == true)
                if (!string.IsNullOrEmpty(txtNumDeclaracion.Text))
                    infConsultaPagosDT = ClienteDeclaracionIsai.ObtenerInfConsultaPagos(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text, txtNumDeclaracion.Text, txtLineaCaptura.Text, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()), "S");
                else
                    infConsultaPagosDT = ClienteDeclaracionIsai.ObtenerInfConsultaPagos(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text, null, txtLineaCaptura.Text, DateTime.Parse(txtFechaIni.Text.ToString()), DateTime.Parse(txtFechaFin.Text.ToString()), "S");
            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceConsultaPagos = new ReportDataSource("DseInfPagos_FEXNOT_INFPAGOS", (DataTable)infConsultaPagosDT);
            //Creamos los parametros de la pagina de detalle de los totales
            rpvPagosCon.LocalReport.EnableHyperlinks = true;
            string tipo;
            if (rbConformados.Checked == true)
                tipo = "C";
            else if (rbVencidos.Checked == true)
                tipo = "V";
            else
                tipo = "S";
            string busqueda = String.Format("Búsqueda por: {0} ", Environment.NewLine);
            if (!string.IsNullOrEmpty(txtNumDeclaracion.Text))
            {
                busqueda += String.Format("Número declaración: {0} {1}", txtNumDeclaracion.Text, Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(txtRegion.Text))
            {
                busqueda += String.Format("Cuenta catastral: {0}-{1}-{2}-{3} {4}", txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text, Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(txtLineaCaptura.Text))
            {
                busqueda += String.Format("Línea de captura: {0} {1}", txtLineaCaptura.Text, Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(txtFechaIni.Text))
            {
                busqueda += String.Format("Fechas: {0} - {1}", txtFechaIni.Text, txtFechaFin.Text);
            }
            ReportParameter urlParameterTipo = new ReportParameter("parameter_tipo", tipo);
            rpvPagosCon.LocalReport.SetParameters(new ReportParameter[1] { urlParameterTipo });
            ReportParameter busquedaParameter = new ReportParameter("busqueda_parameter", busqueda);
            rpvPagosCon.LocalReport.SetParameters(new ReportParameter[1] { busquedaParameter });
   
            //Añadimos el reportDataSource al reportViewer
            rpvPagosCon.LocalReport.DataSources.Add(reportDataSourceConsultaPagos);
            rpvPagosCon.DataBind();
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
