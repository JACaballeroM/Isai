using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;


/// <summary>
/// Clase encargada de generar el informe de consulta específica
/// </summary>
public partial class InformeConsultaEspecifica : PageBaseISAI
{
    /// <summary>
    /// Enumerador para el tipo de Filtro
    /// 0. Cuenta catastral
    /// 1. Persona
    /// 2. Ubicación
    /// </summary>
    protected enum TipoFiltroBusqueda
    {
        CuentaCatastral = 0,
        Persona = 1,
        Ubicacion = 2
    }

    /// <summary>
    /// Load de la página InformeConsultaEspecifica.
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
                    rpvConsultaEspecifica.Visible = false;
                }
               
                //Ocultamos el report hasta que se agrege un DataSource
                rpvConsultaEspecifica.Visible = false;

                HiddenRegion.Value = Utilidades.GetParametroUrl(Constantes.PAR_VAL_REGION);
                HiddenManzana.Value = Utilidades.GetParametroUrl(Constantes.PAR_VAL_MANZANA);
                HiddenLote.Value = Utilidades.GetParametroUrl(Constantes.PAR_VAL_LOTE);
                HiddenUnidad.Value = Utilidades.GetParametroUrl(Constantes.PAR_VAL_UNIDAD);
                HiddenCalle.Value = Utilidades.GetParametroUrl(Constantes.PAR_VAL_CALLE);
                HiddenInt.Value = Utilidades.GetParametroUrl(Constantes.PAR_VAL_INT);
                HiddenSujeto.Value = Utilidades.GetParametroUrl(Constantes.PAR_VAL_SUJETO);


                if (!string.IsNullOrEmpty(HiddenRegion.Value))
                {
                    rbCuenta.Checked = true;
                    txtRegion.Enabled = true;
                    txtManzana.Enabled = true;
                    txtLote.Enabled = true;
                    txtUnidadPrivativa.Enabled = true;
                    rbPersonas.Checked = false;
                    txtSujeto.Enabled = false;
                    rbUbicacion.Checked = false;
                    txtCalle.Enabled = false;
                    txtNumInt.Enabled = false;
                    txtRegion.Text = HiddenRegion.Value;
                    txtManzana.Text = HiddenManzana.Value;
                    txtLote.Text = HiddenLote.Value;
                    rfvRegion.Enabled = true;
                    rfvManzana.Enabled = true;
                    rfvRegion.Enabled = true;
                    rfvUnidadPrivativa.Enabled = true;
                    txtUnidadPrivativa.Text = HiddenUnidad.Value;
                    CargarInforme();

                }
                else if (!string.IsNullOrEmpty(HiddenSujeto.Value))
                {
                    rbCuenta.Checked = false;
                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    rbPersonas.Checked = true;
                    txtSujeto.Enabled = true;
                    rbUbicacion.Checked = false;
                    txtCalle.Enabled = false;
                    txtNumInt.Enabled = false;
                    txtSujeto.Text = HiddenSujeto.Value;
                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    CargarInforme();

                }
                else if (!string.IsNullOrEmpty(HiddenCalle.Value) || !string.IsNullOrEmpty(HiddenInt.Value))
                {
                    rbCuenta.Checked = false;
                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    rbPersonas.Checked = false;
                    txtSujeto.Enabled = false;
                    rbUbicacion.Checked = true;
                    txtCalle.Enabled = true;
                    txtNumInt.Enabled = true;
                    txtCalle.Text = HiddenCalle.Value;
                    txtNumInt.Text = HiddenInt.Value;
                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
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
    /// Maneja el evento Click del control btnBuscar
    /// Evento que mostrará el Informe de Consulta específica filtrado según los datos introducidos.
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
    /// Método que carga la información que se va a mostrar en el informe.
    /// </summary>
    private void CargarInforme()
    {
        rpvConsultaEspecifica.Visible = true;

        //Declaramos el DataTable
        ServiceDeclaracionIsai.DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable infConsultaEspecificaDT;

        //Limpiamos el origen de datos.
        rpvConsultaEspecifica.LocalReport.DataSources.Clear();

        //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
        infConsultaEspecificaDT = null;
        string visibilidad = "0";
        string busqueda = "";
        if (rbCuenta.Checked == true)
        {
            infConsultaEspecificaDT = ClienteDeclaracionIsai.ObtenerInfConsultaEspecificaCuentaCatastral(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text);
            busqueda = String.Format("Búsqueda por Cuenta Catastral: {0}-{1}-{2}-{3}", txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text);
        }
        else if (rbPersonas.Checked == true)
        {
            infConsultaEspecificaDT = ClienteDeclaracionIsai.ObtenerInfConsultaEspecificaSujeto(txtSujeto.Text);
            busqueda = String.Format("Búsqueda por Participante: {0}", txtSujeto.Text);
        }
        else if (rbUbicacion.Checked == true)
        {
            infConsultaEspecificaDT = ClienteDeclaracionIsai.ObtenerInfConsultaEspecificaUbicacion(txtCalle.Text, txtNumInt.Text);
            busqueda = String.Format("Búsqueda por Ubicación:   Calle y Nº Ext.:{0},  Nº Int.:{1}", txtCalle.Text, txtNumInt.Text);
            if (infConsultaEspecificaDT.Count > 0)
            {
                foreach (var row in infConsultaEspecificaDT)
                {
                    var declaracion = ClienteDeclaracionIsai.ObtenerDeclaracionesPorIdDeclaracion((int)row.IDDECLARACION);
                    if (declaracion.Count > 0)
                        row.NOMBRE_NOTARIO = declaracion[0].CODESTADODECLARACION == 0 ? "Borrador" :
                            declaracion[0].CODESTADODECLARACION == 1 ? "Pendiente" :
                            declaracion[0].CODESTADODECLARACION == 2 ? "Presentada" :
                            declaracion[0].CODESTADODECLARACION == 3 ? "Pendiente de Doc." :
                            declaracion[0].CODESTADODECLARACION == 4 ? "Aceptada" : "Inconsistente";
                            
                }
            }
            visibilidad = "1";


        }


        
        //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
        ReportDataSource reportDataSourceConsultaEspecifica = new ReportDataSource("FEXNOT_INFOCONSESPECIFICA", (DataTable)infConsultaEspecificaDT);

        //Obtenemos la Url del report ConsultaEspecificaDetalle pasandole el idDeclaracion seleccionado en el report.
        string url = string.Format(@"http://{0}{1}", Request.Url.Authority, Page.ResolveUrl("InformeConsultaEspecificaDetalle.aspx?Region=" + txtRegion.Text + "&Manzana=" + txtManzana.Text + "&Lote=" + txtLote.Text + "&Unidad=" + txtUnidadPrivativa.Text + "&Sujeto=" + txtSujeto.Text + "&Int=" + txtNumInt.Text + "&Calle=" + txtCalle.Text + "&idDeclaracion="));
        ReportParameter urlParameter = new ReportParameter("url_parameter",url);
        ReportParameter visibilidadPar = new ReportParameter("ParameterVisibilidad", visibilidad);

        ReportParameter busquedaParameter = new ReportParameter("busqueda_parameter", busqueda);

        rpvConsultaEspecifica.LocalReport.SetParameters(new ReportParameter[3] { urlParameter, busquedaParameter, visibilidadPar });
        
        //Añadimos el reportDataSource al reportViewer
        rpvConsultaEspecifica.LocalReport.DataSources.Add(reportDataSourceConsultaEspecifica);
        rpvConsultaEspecifica.DataBind();

    }


    /// <summary>
    /// Maneja el evento ChekedChanged del control rbBusquedaGroup
    /// Evento que lanza las Restricciones de los filtros según el radiobuton seleccionado.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbBusquedaGroup_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (sender == rbCuenta)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.CuentaCatastral);
            }
            else if (sender == rbPersonas)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.Persona);
            }
            else if (sender == rbUbicacion)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.Ubicacion);
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
    /// Método para Habilitar y Deshabilitar los diferentes texbox dependiendo de los Filtros.
    /// </summary>
    /// <param name="busquedaFiltro">busquedaFiltro</param>
    protected void RestriccionesFiltro(TipoFiltroBusqueda busquedaFiltro)
    {
        try
        {
            switch (busquedaFiltro)
            {
                case TipoFiltroBusqueda.CuentaCatastral:

                    //CuentaCatastral
                    txtRegion.Enabled = true;
                    txtManzana.Enabled = true;
                    txtLote.Enabled = true;
                    txtUnidadPrivativa.Enabled = true;
                    rfvRegion.Enabled = true;
                    rfvManzana.Enabled = true;
                    rfvLote.Enabled = true;
                    rfvUnidadPrivativa.Enabled = true;
                    txtCalle.Enabled = false;
                    txtNumInt.Enabled = false;
                    txtCalle.Text = string.Empty;
                    txtNumInt.Text = string.Empty;
                    rfvtxtSujeto.Enabled = false;
                    cvSujeto.Enabled = false;
                    //Sujeto
                    txtSujeto.Enabled = false;
                    txtSujeto.Text = string.Empty;

                    break;
                case TipoFiltroBusqueda.Persona:

                    //Ubicación
                    txtCalle.Enabled = false;
                    txtNumInt.Enabled = false;
                    txtCalle.Text = string.Empty;
                    txtNumInt.Text = string.Empty;
                    
                    //CuentaCatastral
                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    txtRegion.Text = string.Empty;
                    txtManzana.Text = string.Empty;
                    txtLote.Text = string.Empty;
                    txtUnidadPrivativa.Text = string.Empty;
                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    //Sujeto
                    txtSujeto.Enabled = true;
                    rfvtxtSujeto.Enabled = true;
                    cvSujeto.Enabled = true;
                    break;

                case TipoFiltroBusqueda.Ubicacion:

                    //Ubicación
                    txtCalle.Enabled = true;
                    txtNumInt.Enabled = true;
                    rfvtxtSujeto.Enabled = false;
                    cvSujeto.Enabled = false;

                    //CuentaCatastral
                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    txtRegion.Text = string.Empty;
                    txtManzana.Text = string.Empty;
                    txtLote.Text = string.Empty;
                    txtUnidadPrivativa.Text = string.Empty;
                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    //Sujeto
                    txtSujeto.Enabled = false;
                    txtSujeto.Text = string.Empty;

                    break;
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
