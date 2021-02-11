using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;
using SIGAPred.Common.BL;
using SIGAPred.Common.Web;
using SIGAPred.Common.Token;
using System.ServiceModel;
using ServiceRCON;

/// <summary>
/// Clase encargada de gestionar la bandeja de entrada de la secretaria de finanzas
/// </summary>
public partial class BandejaEntradaSF : PageBaseISAI
{

    #region Propiedades

    /// <summary>
    /// La búsqueda actual.
    /// </summary>
    protected TipoFiltroBusqueda _busquedaActual;


    #endregion

    #region Métodos

    /// <summary>
    /// Carga y configura los controles de la página
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                CargarComboCodEstadosDeclaracion();
                if (PreviousPage != null)
                {
                    string name = PreviousPage.GetType().Name;
                    switch (name)
                    {
                        case "home_aspx":
                            txtFechaIni.Text = DateTime.Now.AddDays(-29).ToShortDateString();
                            txtFechaFin.Text = DateTime.Now.ToShortDateString();
                            FBusqueda.FechaIni = txtFechaIni.Text;
                            FBusqueda.FechaFin = txtFechaFin.Text;
                            CargarPagina(TipoFiltroBusqueda.Fecha);
                            break;

                        default:
                            break;
                    }

                }
                else
                {
                    CargarSort();
                    if (!string.IsNullOrEmpty(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO)))
                    {
                        FBusqueda.RellenarObjetoFiltro(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO));
                        CargarCamposFiltro();
                        if (FBusqueda.EsFecha())
                            CargarPagina(TipoFiltroBusqueda.Fecha);
                        else if (FBusqueda.EsCuenta())
                            CargarPagina(TipoFiltroBusqueda.CuentaCatastral);
                        else if (FBusqueda.EsNumPres())
                            CargarPagina(TipoFiltroBusqueda.NumeroPresentacion);
                        else if (FBusqueda.EsSujeto())
                            CargarPagina(TipoFiltroBusqueda.Sujeto);
                    }
                    else
                    {
                        txtFechaIni.Text = DateTime.Now.AddDays(-29).ToShortDateString();
                        txtFechaFin.Text = DateTime.Now.ToShortDateString();
                        FBusqueda.FechaIni = txtFechaIni.Text;
                        FBusqueda.FechaFin = txtFechaFin.Text;
                        CargarPagina(TipoFiltroBusqueda.Fecha);
                    }
                }
                hidBusquedaActual.Value = ((int)_busquedaActual).ToString();
            }
            else
            {
                _busquedaActual = (TipoFiltroBusqueda)(System.Convert.ToInt32(hidBusquedaActual.Value));
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
    /// Ordena el gridView con la dirección y expresión obtenidos por parámetro URL
    /// </summary>
    private void CargarSort()
    {
        try
        {
            SortDirection direction;

            SortExpression = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP);
            SortDirectionP = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR);
            switch (SortDirectionP)
            {
                case "Ascending":
                    direction = SortDirection.Ascending;
                    break;
                case "Descending":
                    direction = SortDirection.Descending;
                    break;
                default:
                    direction = SortDirection.Ascending;
                    break;
            }


            gridViewDeclaraciones.Sort(SortExpression, direction);
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
    /// Carga los campos del filtro con la información almacenada
    /// </summary>
    private void CargarCamposFiltro()
    {
        try
        {
            rbFechas.Checked = rbCuenta.Checked = rbNotario.Checked = rbNumPresentacion.Checked = false;
            txtRegion.Text = FBusqueda.Region;
            txtLote.Text = FBusqueda.Lote;
            txtManzana.Text = FBusqueda.Manazana;
            txtUnidadPrivativa.Text = FBusqueda.UnidadPrivativa;
            txtFechaIni.Text = FBusqueda.FechaIni;
            txtFechaFin.Text = FBusqueda.FechaFin;
            txtIdPerNot.Text = FBusqueda.IdSujeto;
            txtNumNot.Text = FBusqueda.Sujeto;
            txtNomNot.Text = FBusqueda.Sujeto0;
            txtNumPresentacion.Text = FBusqueda.NumPres;
            if (FBusqueda.EsFecha())
            {
                rbFechas.Checked = true;
                RestriccionesFiltro(TipoFiltroBusqueda.Fecha);
            }
            if (FBusqueda.EsCuenta())
            {
                rbCuenta.Checked = true;
                RestriccionesFiltro(TipoFiltroBusqueda.CuentaCatastral);
            }
            if (FBusqueda.EsSujeto())
            {
                rbNotario.Checked = true; ;
                RestriccionesFiltro(TipoFiltroBusqueda.Sujeto);
            }
            if (FBusqueda.EsNumPres())
            {
                rbNumPresentacion.Checked = true; ;
                RestriccionesFiltro(TipoFiltroBusqueda.NumeroPresentacion);
            }
            if (!string.IsNullOrEmpty(FBusqueda.CodEstado))
            {
                ddlEstadoDeclaracion.SelectedIndex = -1;

                ddlEstadoDeclaracion.Items.FindByValue(FBusqueda.CodEstado).Selected = true;
            }
            AsignarValoresBusquedaBESF();
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
    /// Carga la página 
    /// </summary>
    /// <param name="tipoFiltroBusqueda"></param>
    private void CargarPagina(TipoFiltroBusqueda tipoFiltroBusqueda)
    {
        try
        {
            _busquedaActual = tipoFiltroBusqueda;
            FBusqueda.Fecha = tipoFiltroBusqueda.ToString();
            HiddenIdPersonaToken.Value = Usuarios.IdPersona();

            CargarDataTable(tipoFiltroBusqueda);

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
    /// Se le asigna al gridview el origen de datos correspondiente al filtro seleccionado
    /// </summary>
    /// <param name="tipoFiltroBusquea"></param>
    private void CargarDataTable(TipoFiltroBusqueda tipoFiltroBusquea)
    {
        try
        {
            switch (tipoFiltroBusquea)
            {
                case TipoFiltroBusqueda.Fecha:

                    gridViewDeclaraciones.DataSourceID = odsDeclaracionFechaSF.ID;
                    break;
                case TipoFiltroBusqueda.CuentaCatastral:
                    gridViewDeclaraciones.DataSourceID = odsDeclaracionCuentaSF.ID;
                    break;
                case TipoFiltroBusqueda.Sujeto:
                    gridViewDeclaraciones.DataSourceID = odsDeclaracionSujetoSF.ID;
                    break;
                case TipoFiltroBusqueda.NumeroPresentacion:
                    gridViewDeclaraciones.DataSourceID = odsDeclaracionNumPresSF.ID;
                    break;
                default:

                    break;
            }
            gridViewDeclaraciones.DataBind();
            gridViewDeclaraciones.SelectedIndex = -1;
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

    #endregion

    #region Cargar Combos

    /// <summary>
    /// Carga el dropdown ddlEstadoDeclaracion con el catálogo de estado de declaraciones de ISAI
    /// </summary>
    private void CargarComboCodEstadosDeclaracion()
    {
        try
        {
            DseCatalogo.FEXNOT_CATESTADDECLARACIONDataTable catEstadoDeclaracionDT = ClienteDeclaracionIsai.ObtenerCatEstadoDeclaraciones();
            
            foreach (DseCatalogo.FEXNOT_CATESTADDECLARACIONRow dr in catEstadoDeclaracionDT.Rows)
            {
                if (dr.CODESTADODECLARACION == 0)
                {
                    dr.Delete();
                }
                else if (dr.CODESTADODECLARACION == 1)
                {
                    dr.Delete();
                }
            }


            this.ddlEstadoDeclaracion.DataSource = catEstadoDeclaracionDT;

            this.ddlEstadoDeclaracion.DataTextField = catEstadoDeclaracionDT.DESCRIPCIONColumn.ToString();
            this.ddlEstadoDeclaracion.DataValueField = catEstadoDeclaracionDT.CODESTADODECLARACIONColumn.ToString();

            this.ddlEstadoDeclaracion.DataBind();

            this.ddlEstadoDeclaracion.Items.Insert(0, new ListItem("Todos", Constantes.UI_DDL_VALUE_NO_SELECT));
            this.ddlEstadoDeclaracion.Items.FindByValue("2").Selected = true;
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

    #endregion

    #region Eventos del RadioButton

    /// <summary>
    /// Maneja el evento CheckedChanged del control rbBusquedaGroup
    /// Depende del radio button que este seleccionado realiza una búsqueda u otra
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void rbBusquedaGroup_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (sender == rbCuenta)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.CuentaCatastral);
            }
            else if (sender == rbFechas)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.Fecha);
                AsignarFechasPorDefectoSF(txtFechaIni, txtFechaFin);
            }
            else if (sender == rbNotario)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.Sujeto);
            }
            else if (sender == rbNumPresentacion)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.NumeroPresentacion);
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
    /// Configura los controles del filtro dependiendo del parámetro de entrada
    /// </summary>
    /// <param name="busquedaFiltro">Tipo de búsqueda</param>
    protected void RestriccionesFiltro(TipoFiltroBusqueda busquedaFiltro)
    {
        try
        {
            switch (busquedaFiltro)
            {
                case TipoFiltroBusqueda.CuentaCatastral:


                    txtRegion.Enabled = true;
                    txtManzana.Enabled = true;
                    txtLote.Enabled = true;
                    txtUnidadPrivativa.Enabled = true;
                    txtNumNot.Enabled = false;
                    txtNomNot.Enabled = false;
                    txtFechaFin.Enabled = false;
                    txtFechaIni.Enabled = false;
                    txtNumPresentacion.Enabled = false;
                    //HiddenSuj.Text = string.Empty;
                    rfvRegion.Enabled = true;
                    rfvManzana.Enabled = true;

                    txtUnidadPrivativa.Attributes.Add("onblur", "javascript:if (this.value.length!=0){document.getElementById('" + rfvLote.ClientID + "').enabled=true;rellenar(this, this.value, 3);}else {document.getElementById('" + rfvLote.ClientID + "').enabled=false;}");

                    rfvFechaFin.Enabled = false;
                    rfvFechaInicio.Enabled = false;
                    cvFechaFin.Enabled = false;
                    cvFechaInicio.Enabled = false;
                    cvRangoFechas.Enabled = false;


                    rfvNumPresentacion.Enabled = false;

                    txtNumNot.Text = string.Empty;
                    txtNomNot.Text = string.Empty;
                    txtFechaIni.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    txtNumPresentacion.Text = string.Empty;

                    btnFechaIni.Enabled = false;
                    btnFechaFin.Enabled = false;

                    btnNotario.Enabled = false;
                    btnNotario.ImageUrl = Constantes.IMG_USER_DISABLED;

                    break;
                case TipoFiltroBusqueda.Fecha:


                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    txtNumNot.Enabled = false;
                    txtNomNot.Enabled = false;
                    txtFechaFin.Enabled = true;
                    txtFechaIni.Enabled = true;
                    txtNumPresentacion.Enabled = false;
                    //HiddenSuj.Text = string.Empty;
                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    rfvFechaFin.Enabled = true;
                    rfvFechaInicio.Enabled = true;
                    cvFechaFin.Enabled = true;
                    cvFechaInicio.Enabled = true;
                    cvRangoFechas.Enabled = true;

                    rfvNumPresentacion.Enabled = false;

                    txtNumNot.Text = string.Empty;
                    txtNomNot.Text = string.Empty;
                    txtRegion.Text = string.Empty;
                    txtManzana.Text = string.Empty;
                    txtLote.Text = string.Empty;
                    txtUnidadPrivativa.Text = string.Empty;
                    txtNumPresentacion.Text = string.Empty;

                    btnFechaIni.Enabled = true;
                    btnFechaFin.Enabled = true;

                    btnNotario.Enabled = false;
                    btnNotario.ImageUrl = Constantes.IMG_USER_DISABLED;
                    break;
                case TipoFiltroBusqueda.Sujeto:

                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;

                    txtFechaFin.Enabled = false;
                    txtFechaIni.Enabled = false;
                    txtNumPresentacion.Enabled = false;

                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    rfvFechaFin.Enabled = false;
                    rfvFechaInicio.Enabled = false;
                    cvFechaFin.Enabled = false;
                    cvFechaInicio.Enabled = false;
                    cvRangoFechas.Enabled = false;

                    rfvNumPresentacion.Enabled = false;
                    txtRegion.Text = string.Empty;
                    txtManzana.Text = string.Empty;
                    txtLote.Text = string.Empty;
                    txtUnidadPrivativa.Text = string.Empty;
                    txtFechaIni.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    txtNumPresentacion.Text = string.Empty;

                    btnFechaIni.Enabled = false;
                    btnFechaFin.Enabled = false;


                    txtNumNot.Enabled = true;
                    txtNomNot.Enabled = true;
                    btnNotario.Enabled = true;
                    btnNotario.ImageUrl = Constantes.IMG_USER;

                    break;
                case TipoFiltroBusqueda.NumeroPresentacion:


                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    txtNumNot.Enabled = false;
                    txtNomNot.Enabled = false;
                    txtFechaFin.Enabled = false;
                    txtFechaIni.Enabled = false;
                    txtNumPresentacion.Enabled = true;

                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    rfvFechaFin.Enabled = false;
                    rfvFechaInicio.Enabled = false;
                    cvFechaFin.Enabled = false;
                    cvFechaInicio.Enabled = false;
                    cvRangoFechas.Enabled = false;

                    rfvNumPresentacion.Enabled = true;
                    txtRegion.Text = string.Empty;
                    txtManzana.Text = string.Empty;
                    txtLote.Text = string.Empty;
                    txtUnidadPrivativa.Text = string.Empty;
                    txtFechaIni.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    txtNumNot.Text = string.Empty;
                    txtNomNot.Text = string.Empty;
                    btnFechaIni.Enabled = false;
                    btnFechaFin.Enabled = false;
                    btnNotario.Enabled = false;
                    btnNotario.ImageUrl = Constantes.IMG_USER_DISABLED;

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

    /// <summary>
    /// Asigna a los textbox de fechas las fechas por defecto
    /// </summary>
    /// <param name="txtFechaInicio"></param>
    /// <param name="txtFechaFin"></param>
    public void AsignarFechasPorDefectoSF(TextBox txtFechaInicio, TextBox txtFechaFin)
    {
        try
        {
            txtFechaInicio.Text = DateTime.Now.AddDays(-29).ToShortDateString();
            txtFechaFin.Text = DateTime.Now.ToShortDateString();
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

    #endregion

    #region Botones

    /// <summary>
    /// Maneja el evento Click del control btnBuscar 
    /// Guardo los valores de la búsqueda
    /// Carga el gridview con los resultados 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.EventArgs"/> que contiene los datos del evento</param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            AsignarValoresBusquedaBESF();

            txtCuenta.Text = string.Empty;
            txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;
            CargarGridView();
            UpdatePanelGrid.Update();

            ActualizarFiltroBusquedaBESF();
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
    /// Método que asigna los valores del filtro de la secretaría de finanzas desde los controles a los hidden
    /// </summary>
    private void AsignarValoresBusquedaBESF()
    {
        //Filtro Fecha
        HiddenFechaFin.Value = txtFechaFin.Text;
        HiddenFechaIni.Value = txtFechaIni.Text;


        //Filtro cuenta catastral
        HiddenRegion.Value = txtRegion.Text;
        HiddenManzana.Value = txtManzana.Text;
        HiddenLote.Value = txtLote.Text;
        HiddenUPrivativa.Value = txtUnidadPrivativa.Text;
        txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;
        HiddenCuentaCat.Value = txtCuenta.Text;

        //Filtro estado
        HiddenCodEstado.Value = ddlEstadoDeclaracion.SelectedValue.ToString();

        //Filtro Sujeto 
        HiddenIdSujeto.Value = txtIdPerNot.Text;
        HiddenSujeto.Value = txtNumNot.Text;
        HiddenSujeto0.Value = txtNomNot.Text;
        HiddenNumPres.Value = txtNumPresentacion.Text;
    }

    /// <summary>
    /// Actualiza la clase FiltroBusqueda con los datos seleccionados en el filtro para la búsqueda
    /// </summary>
    private void ActualizarFiltroBusquedaBESF()
    {
        //Busqueda por fecha
        FBusqueda.FechaIni = string.Empty;
        FBusqueda.FechaFin = string.Empty;

        FBusqueda.FechaIni = txtFechaIni.Text;
        FBusqueda.FechaFin = txtFechaFin.Text;

        //Busqueda por cuentacatastral
        FBusqueda.Region = string.Empty;
        FBusqueda.Lote = string.Empty;
        FBusqueda.Manazana = string.Empty;
        FBusqueda.UnidadPrivativa = string.Empty;


        FBusqueda.Lote = txtLote.Text;
        FBusqueda.Manazana = txtManzana.Text;
        FBusqueda.Region = txtRegion.Text;
        FBusqueda.UnidadPrivativa = txtUnidadPrivativa.Text;

        //Filtro codestado
        FBusqueda.CodEstado = string.Empty;

        FBusqueda.CodEstado = ddlEstadoDeclaracion.SelectedValue.ToString();

        //Filtro sujeto
        FBusqueda.IdSujeto = string.Empty;
        FBusqueda.Sujeto = string.Empty;
        FBusqueda.Sujeto0 = string.Empty;
        FBusqueda.NumPres = string.Empty;
        FBusqueda.IdSujeto = txtIdPerNot.Text;
        FBusqueda.Sujeto = txtNumNot.Text;
        FBusqueda.Sujeto0 = txtNomNot.Text;
        FBusqueda.NumPres = txtNumPresentacion.Text;

    }

    /// <summary>
    /// Maneja el evento Click del control btnEliminarBusqueda
    /// Actualiza al estado por defecto de todos los controles del filtro
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnEliminarBusqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            rbFechas.Checked = true;
            txtFechaIni.Text = DateTime.Now.AddDays(-29).ToShortDateString();
            txtFechaFin.Text = DateTime.Now.ToShortDateString();
            btnFechaIni.Enabled = true;
            btnFechaFin.Enabled = true;
            txtFechaFin.Enabled = true;
            txtFechaIni.Enabled = true;
            rfvFechaInicio.Enabled = true;
            rfvFechaFin.Enabled = true;

            rbCuenta.Checked = false;
            txtRegion.Text = "";
            txtRegion.Enabled = false;
            txtLote.Text = "";
            txtLote.Enabled = false;
            txtManzana.Text = "";
            txtManzana.Enabled = false;
            txtUnidadPrivativa.Text = "";
            txtUnidadPrivativa.Enabled = false;
            txtCuenta.Text = "";
            rfvRegion.Enabled = false;
            rfvLote.Enabled = false;
            rfvManzana.Enabled = false;

            rbNotario.Checked = false;
            txtNumNot.Text = "";
            txtNomNot.Text = "";

            rbNumPresentacion.Checked = false;
            txtNumPresentacion.Text = "";
            txtNumPresentacion.Enabled = false;
            rfvNumPresentacion.Enabled = false;
            ddlEstadoDeclaracion.SelectedIndex = 0;

            HiddenFechaFin.Value = string.Empty;

            HiddenFechaIni.Value = string.Empty;

            gridViewDeclaraciones.Sort(String.Empty, SortDirection.Ascending);

            CargarPagina(TipoFiltroBusqueda.Fecha);

            UpdatePanelBusqueda.Update();
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
    /// Manejador de eventos. Llamado por btnNotario para eventos click.
    /// </summary>
    /// <param name="sender">Origen</param>
    /// <param name="e">Evento</param>
    protected void btnNotario_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnNotario_ModalPopupExtender.Show();
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }


    #endregion

    #region Eventos del GridView

    /// <summary>
    /// Carga el gridview con los resultados obtenidos de la búsqueda
    /// </summary>
    private void CargarGridView()
    {
        try
        {

            if (rbCuenta.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.CuentaCatastral);
                hidBusquedaActual.Value = TipoFiltroBusqueda.CuentaCatastral.ToInt().ToString();
            }
            else if (rbFechas.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.Fecha);
                hidBusquedaActual.Value = TipoFiltroBusqueda.Fecha.ToInt().ToString();
            }
            else if (rbNotario.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.Sujeto);
                hidBusquedaActual.Value = TipoFiltroBusqueda.Sujeto.ToInt().ToString();
            }
            else if (rbNumPresentacion.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.NumeroPresentacion);
                hidBusquedaActual.Value = TipoFiltroBusqueda.NumeroPresentacion.ToInt().ToString();
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
    /// Maneja el evento SelectedIndexChanged del control gridViewDeclaraciones
    /// Redirecciona a la declaración seleccionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewDeclaraciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();

            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_VER);
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
   
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
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

    /// <summary>
    /// Manejador del evento PageIndexChanging del control gridViewDeclaraciones
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewDeclaraciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridViewDeclaraciones.PageIndex = e.NewPageIndex;
            CargarGridView();
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
    /// Manejador del evento RowDataBound del control gridViewDeclaraciones
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewDeclaraciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CheckBox CheckSiscor = (CheckBox)e.Row.FindControl("checkboxSISCOR");
                CheckSiscor.Checked = e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_DECLARACIONRow>().CODESTADOPAGO == EstadosPago.RecibidoSISCOR.ToDecimal() ? true : false;
                   if (string.IsNullOrEmpty(e.Row.Cells[3].Text) || e.Row.Cells[3].Text=="&nbsp;")
                    e.Row.Cells[3].Text = "-";

                if (e.Row.RowIndex == 0)
                {
                    DataRowView t = (DataRowView)e.Row.DataItem;
                    lblCount.Text = String.Format("Se ha encontrado un total de {0} declaración(es)", t["ROWS_TOTAL"]);
                }

               
                string ToolTipString = e.Row.Cells[11].Text;
                e.Row.Cells[3].Attributes.Add("title", ToolTipString);

                DseInfoContribuyente dseInfo;
                using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
                {
                    dseInfo = clienteRCON.GetInfoContribuyente(e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_DECLARACIONRow>().IDPERSONA);
                }
                if (dseInfo.Contribuyente.Rows.Count > 0)
                {
                    e.Row.Cells[3].Attributes.Add("title", dseInfo.Contribuyente[0].APELLIDOPATERNO + " " + dseInfo.Contribuyente[0].APELLIDOMATERNO + ", " + dseInfo.Contribuyente[0].NOMBRE);
                }


            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                lblCount.Text = string.Empty;
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
    /// Maneja el evento Sorting del control gridViewDeclaraciones
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewDeclaraciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortExpression = e.SortExpression;
            SortDirectionP = e.SortDirection.ToString();
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION +Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
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

    protected TipoFiltroBusqueda FiltroSeleccionado()
    {
        TipoFiltroBusqueda tipoBusqueda = TipoFiltroBusqueda.Fecha;

        try
        {
            if (rbCuenta.Checked)
            {
                tipoBusqueda = TipoFiltroBusqueda.CuentaCatastral;
            }
            else if (rbFechas.Checked)
            {
                tipoBusqueda = TipoFiltroBusqueda.Fecha;
            }
            else if (rbNotario.Checked)
            {
                tipoBusqueda = TipoFiltroBusqueda.Sujeto;
            }
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
        return tipoBusqueda;
    }
    /// <summary>
    /// Manejador de eventos. Llamado por buscarNotario para eventos confirm click.
    /// </summary>
    /// <param name="sender">Origen</param>
    /// <param name="e">Evento</param>
    protected void buscarNotario_ConfirmClick(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (!e.Cancel)
            {
                txtNumNot.Text = ModalBuscarNotarios.NumeroIdentificadorNotario.ToString();
                txtNomNot.Text = ModalBuscarNotarios.NombreNotario;
                txtIdPerNot.Text = ModalBuscarNotarios.IdentificadorPersonaNotario.ToString();
            }
            else
            {
                //txtNumNot.Text = string.Empty;
                //txtNomNot.Text = string.Empty;
                //txtIdPerNot.Text = string.Empty;

            }
            txtNomNot.Enabled = true;
            txtNumNot.Enabled = true;
            ModalBuscarNotarios.LimpiarDatos();
            btnNotario_ModalPopupExtender.Hide();
            UpdatePanelBusqueda.Update();

        }
        catch (FaultException<ServiceAvaluos.AvaluosException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
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
    /// Manejador de eventos. Llamado por btnNotarioHidden para eventos value cambio.
    /// </summary>
    /// <param name="sender">Origen</param>
    /// <param name="e">Evento</param>
    protected void btnNotarioHidden_ValueChanged(object sender, EventArgs e)
    {

    }

    #endregion

    #region Excepciones

    /// <summary>
    /// Pre-renderizado de la página.
    /// </summary>
    /// <param name="sender">.</param>
    /// <param name="e">.</param>
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

    /// <summary>
    /// Mostrar mensaje información excepcion.
    /// </summary>
    /// <param name="mensaje">El/La mensaje.</param>
    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion

}
