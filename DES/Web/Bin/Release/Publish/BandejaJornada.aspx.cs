using System;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;

/// <summary>
/// Clase encargada de gestionar la bandeja de entrada de la jornada notarial
/// </summary>
public partial class BandejaJornada : PageBaseISAI
{

    /// <summary>
    /// Almacena el tipo de filtro seleccionado
    /// </summary>
    protected TipoFiltroBusqueda _busquedaActual;

    #region Métodos

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (PreviousPage != null)
                {
                    string name = PreviousPage.GetType().Name;
                    switch (name)
                    {
                        case "home_aspx":
                            AsignarFechasPorDefectoJornadaNot();
                            AsignarValoresBusquedaBJ();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    CargarSort();
                    if (!string.IsNullOrEmpty(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO))) // si el filtro tiene valores recogerlos
                    {
                        CargarUltimaBusqueda();
                    }
                    else
                    {
                        AsignarFechasPorDefectoJornadaNot();
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
            string msj = Constantes.MSJ_ERROR_OPERACION +Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
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
    /// Obtiene los valores guardados en el filtro,asigna los criteriors de búsqueda de la última búsqueda y muestra los resultados de la última búsqueda
    /// <summary>
    /// </summary>
    private void CargarUltimaBusqueda()
    {
        FBusqueda.RellenarObjetoFiltro(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO));
        CargarCamposFiltro();
     
        if (FBusqueda.EsFecha())
            CargarPagina(TipoFiltroBusqueda.Fecha);
        else if (FBusqueda.EsCuenta())
            CargarPagina(TipoFiltroBusqueda.CuentaCatastral);
        else if (FBusqueda.EsSujeto())
            CargarPagina(TipoFiltroBusqueda.Sujeto);
        else
        {
            if (String.IsNullOrEmpty(txtFechaIni.Text)||String.IsNullOrEmpty(txtFechaFin.Text))
            {
                AsignarFechasPorDefectoJornadaNot();
                AsignarValoresBusquedaBJ();
                CargarPagina(TipoFiltroBusqueda.Fecha);
            }
            else
                gridViewDeclaraciones.DataBind();
        }

    }


    /// <summary>
    /// Asigna a los txtbox las fechas por defecto
    /// </summary>
    /// <param name="txtFechaInicio"></param>
    /// <param name="txtFechaFin"></param>
    private void AsignarFechasPorDefectoJornadaNot()
    {
        try
        {
            txtFechaIni.Text = DateTime.Now.FirstDayOfYear().ToShortDateString();
            txtFechaFin.Text = DateTime.Now.LastDayOfYear().ToShortDateString();
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
    /// Método que ordena el gridview de declaraciones con los datos obtenidos por URL
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
    /// Carga los campos del filtro con los datos previos al ir a consultar la declaración
    /// </summary>
    private void CargarCamposFiltro()
    {
        try
        {
            txtRegion.Text = FBusqueda.Region;
            txtLote.Text = FBusqueda.Lote;
            txtManzana.Text = FBusqueda.Manazana;
            txtUnidadPrivativa.Text = FBusqueda.UnidadPrivativa;
            txtFechaIni.Text = FBusqueda.FechaIni;
            txtFechaFin.Text = FBusqueda.FechaFin;
            txtSujeto.Text = FBusqueda.Sujeto;
            txtSujeto0.Text = FBusqueda.Sujeto0;

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
                rbSujeto.Checked = true; ;
                RestriccionesFiltro(TipoFiltroBusqueda.Sujeto);
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
    /// Carga la página con todos los datos tal y como estaba antes de ir a la declaración
    /// </summary>
    /// <param name="tipoFiltroBusqueda"></param>
    private void CargarPagina(TipoFiltroBusqueda tipoFiltroBusqueda)
    {
        try
        {
            HiddenIdPersonaToken.Value = Usuarios.IdPersona();
            AsignarValoresBusquedaBJ();
            _busquedaActual = tipoFiltroBusqueda;
            FBusqueda.Fecha = tipoFiltroBusqueda.ToString();
            CargarDataTable(tipoFiltroBusqueda);
            gridViewDeclaraciones.PageIndex = 0;
            gridViewDeclaraciones.DataBind();
            gridViewDeclaraciones.SelectedIndex = -1;
            RestriccionesBotones();
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
    /// Asigna al gridview el origen de datos correspondiente al criterio de busqueda seleccionado
    /// </summary>
    /// <param name="tipoFiltroBusquea"></param>
    private void CargarDataTable(TipoFiltroBusqueda tipoFiltroBusquea)
    {
        try
        {
            switch (tipoFiltroBusquea)
            {
                case TipoFiltroBusqueda.Fecha:
                    gridViewDeclaraciones.DataSourceID = odsPorFecha.ID;
                    break;
                case TipoFiltroBusqueda.CuentaCatastral:
                    gridViewDeclaraciones.DataSourceID = odsPorCC.ID;
                    break;
                case TipoFiltroBusqueda.Sujeto:
                    gridViewDeclaraciones.DataSourceID = odsPorNom.ID;
                    break;
                default:
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
    /// ALmacena los valores utilizados en los filtros
    /// </summary>
    private void AsignarValoresBusquedaBJ()
    {
        //Filtro Fecha
        HiddenFechaFin.Value = txtFechaFin.Text;
        HiddenFechaIni.Value = txtFechaIni.Text;

        //Filtro cuenta catastral
        txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;
        HiddenCuentaCat.Value = txtCuenta.Text;

        //Filtro Sujeto 
        HiddenSujeto.Value = txtSujeto.Text;
        HiddenSujeto0.Value = txtSujeto0.Text;

    }

    /// <summary>
    /// Actualiza los valores de las propiedades de la clase FBusqueda
    /// </summary>
    private void ActualizarFiltroBusquedaBJ()
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

        //Filtro sujeto
        FBusqueda.Sujeto = string.Empty;
        FBusqueda.Sujeto0 = string.Empty;
        FBusqueda.NumPres = string.Empty;

        FBusqueda.Sujeto = txtSujeto.Text;
        FBusqueda.Sujeto0 = txtSujeto0.Text;
    }

    /// <summary>
    /// Dependiendo del estado de la declaración seleccionada en el gridview se habilitan o no los botones 
    /// </summary>
    private void RestriccionesBotones()
    {
        try
        {
            //No hay declaraciones
            if (gridViewDeclaraciones.Rows.Count <= 0 || gridViewDeclaraciones.SelectedIndex == Convert.ToInt32(Constantes.UI_DDL_VALUE_NO_SELECT))
            {
                btnModificar.Enabled = false;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.Enabled = false;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.Enabled = false;
                btnVer.ImageUrl = Constantes.IMG_ZOOM_DISABLED;
                btnGenerarJustificantes.Enabled = false;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC_DISABLED;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnGenerarAcuse.Enabled = false;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
            }
            //Estado borrador
            else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Borrador.ToInt())
            {
                btnModificar.Enabled = true;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA;
                btnEliminar.Enabled = true;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
                btnGenerarJustificantes.Enabled = false;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC_DISABLED;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnGenerarAcuse.Enabled = false;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
            }
            //estado Pendiente de enviar
            else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Pendiente.ToInt())
            {
                btnModificar.Enabled = false;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.Enabled = false;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
                btnGenerarJustificantes.Enabled = false;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC_DISABLED;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnGenerarAcuse.Enabled = true;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC;
            }
            //estado Rechazada
            else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.PendienteDocumentacion.ToInt() || gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Inconsistente.ToInt())
            {
                btnModificar.Enabled = false;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.Enabled = false;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
                btnGenerarJustificantes.Enabled = true;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC_DISABLED;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnGenerarAcuse.Enabled = false;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
            }
            //estado Aceptada (4) o Presentada(2)
            else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Aceptada.ToInt() ||
                     gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Presentada.ToInt())
            {
                btnModificar.Enabled = false;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.Enabled = false;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
                btnGenerarJustificantes.Enabled = true;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnGenerarAcuse.Enabled = true;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC;
            }
            UpdatePanelBotones.Update();
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
    /// Actualiza las restricciones de los controles dependiendo del origen del evento
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
                AsignarFechasPorDefectoJornadaNot();
            }
            else if (sender == rbSujeto)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.Sujeto);
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
    /// Dependiendo del estado del criterio de búsqueda en el gridview se configuran los controles del filtro
    /// </summary>
    /// <param name="filtroBusqueda"></param>
    protected void RestriccionesFiltro(TipoFiltroBusqueda filtroBusqueda)
    {
        try
        {
            switch (filtroBusqueda)
            {
                case TipoFiltroBusqueda.CuentaCatastral:
                    txtRegion.Enabled = true;
                    txtManzana.Enabled = true;
                    txtLote.Enabled = true;
                    txtUnidadPrivativa.Enabled = true;
                    txtSujeto.Enabled = false;
                    txtSujeto0.Enabled = false;
                    txtFechaFin.Enabled = false;
                    txtFechaIni.Enabled = false;
                    rfvHiddenSuj.Enabled = false;
                    rfvRegion.Enabled = true;
                    rfvManzana.Enabled = true;
                    rfvLote.Enabled = true;
                    rfvUnidadPrivativa.Enabled = true;
                    rfvFechaInicio.Enabled = false;
                    rfvFechaFin.Enabled = false;
                    cvFechaInicio.Enabled = false;
                    cvFechaFin.Enabled = false;
                    cvRangoFechas.Enabled = false;
                    cvSujeto.Enabled = false;
                    cvSujeto0.Enabled = false;
                    txtSujeto.Text = string.Empty;
                    txtSujeto0.Text = string.Empty;
                    txtFechaIni.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    btnFechaIni.Enabled = false;
                    btnFechaFin.Enabled = false;
                    HiddenSuj.Text = string.Empty;
                    rbFechas.Checked = false;
                    rbFechas.Enabled = true;
                    rbSujeto.Checked = false;
                    rbSujeto.Enabled = true;
                    break;
                case TipoFiltroBusqueda.Fecha:
                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    txtSujeto.Enabled = false;
                    txtSujeto0.Enabled = false;
                    txtFechaFin.Enabled = true;
                    txtFechaIni.Enabled = true;
                    rfvHiddenSuj.Enabled = false;
                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    rfvFechaInicio.Enabled = true;
                    rfvFechaFin.Enabled = true;
                    cvFechaInicio.Enabled = true;
                    cvFechaFin.Enabled = true;
                    cvRangoFechas.Enabled = true;
                    cvSujeto.Enabled = false;
                    cvSujeto0.Enabled = false;
                    txtSujeto.Text = string.Empty;
                    txtSujeto0.Text = string.Empty;
                    txtRegion.Text = string.Empty;
                    txtManzana.Text = string.Empty;
                    txtLote.Text = string.Empty;
                    txtUnidadPrivativa.Text = string.Empty;
                    HiddenSuj.Text = string.Empty;
                    btnFechaIni.Enabled = true;
                    btnFechaFin.Enabled = true;
                    rbCuenta.Checked = false;
                    rbCuenta.Enabled = true;
                    rbSujeto.Checked = false;
                    rbSujeto.Enabled = true;
                    break;
                case TipoFiltroBusqueda.Sujeto:
                    txtRegion.Enabled = false;
                    txtManzana.Enabled = false;
                    txtLote.Enabled = false;
                    txtUnidadPrivativa.Enabled = false;
                    txtSujeto.Enabled = true;
                    txtSujeto0.Enabled = true;
                    txtFechaFin.Enabled = false;
                    txtFechaIni.Enabled = false;
                    rfvHiddenSuj.Enabled = true;
                    rfvRegion.Enabled = false;
                    rfvManzana.Enabled = false;
                    rfvLote.Enabled = false;
                    rfvUnidadPrivativa.Enabled = false;
                    rfvFechaInicio.Enabled = false;
                    rfvFechaFin.Enabled = false;
                    cvFechaInicio.Enabled = false;
                    cvFechaFin.Enabled = false;
                    cvRangoFechas.Enabled = false;
                    cvSujeto.Enabled = true;
                    cvSujeto0.Enabled = true;
                    txtRegion.Text = string.Empty;
                    txtManzana.Text = string.Empty;
                    txtLote.Text = string.Empty;
                    txtUnidadPrivativa.Text = string.Empty;
                    txtFechaIni.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    btnFechaIni.Enabled = false;
                    btnFechaFin.Enabled = false;
                    rbCuenta.Checked = false;
                    rbCuenta.Enabled = true;
                    rbFechas.Checked = false;
                    rbFechas.Enabled = true;
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

    #endregion

    #region Eventos Botones

    /// <summary>
    /// Maneja el evento ConfirmClick del control modalConfirmacionEliminar
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.ComponentModel.CancelEventArgs"/> que contiene los datos del evento</param>
    protected void modalConfirmacionEliminar_ConfirmClick(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (!e.Cancel)
            {
                ClienteDeclaracionIsai.EliminarDeclaracion(HiddenIdDeclaracion.Value.ToInt());
                CargarGridView();
                UpdatePanelGrid.Update();
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
    /// Maneja el evento Click del control btnModificar
    /// Redirecciona a la página de la declaración para modificarla
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnModificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_MOD);
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
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
    /// Manejador del evento Click del control btnVer
    /// Redirecciona a la ventana de declaracion para consultarla
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnVer_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_VER);
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
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
    /// Manejador del evento Click del control btnNuevo
    /// Redirecciona a la ventana de declaracion para añadir una nueva
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnNuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_INS);
            RedirectUtil.AddParameter(Constantes.PAR_TIPODECLARACION, Constantes.PAR_VAL_TIPODECLARACION_JOR);
            if (FBusqueda.Fecha != null)
                RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
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
    /// Maneja el evento Click del control btnBuscar
    /// Realiza la búsqueda de declaraciones con los criterios de búsqueda seleccionados y carga los resultado en el gridview
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            AsignarValoresBusquedaBJ();
            txtCuenta.Text = string.Empty;
            txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;
            CargarGridView();
            UpdatePanelGrid.Update();
            ActualizarFiltroBusquedaBJ();
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
    /// Maneja el evento Click del control btnEliminarBusqueda
    /// Establece los valores por defecto del panel del filtro
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnEliminarBusqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            AsignarFechasPorDefectoJornadaNot();
            txtFechaIni.Enabled = true;
            txtFechaFin.Enabled = true;
            btnFechaIni.Enabled = true;
            btnFechaFin.Enabled = true;
            rbFechas.Checked = true;
            rbCuenta.Checked = false;
            rbSujeto.Checked = false;
            txtRegion.Text = string.Empty;
            txtRegion.Enabled = false;
            txtLote.Text = string.Empty;
            txtLote.Enabled = false;
            txtManzana.Text = string.Empty;
            txtManzana.Enabled = false;
            rfvRegion.Enabled = false;
            rfvLote.Enabled = false;
            rfvManzana.Enabled = false;
            txtCuenta.Text = string.Empty;
            txtSujeto.Text = string.Empty;
            txtSujeto.Enabled = false;
            txtSujeto0.Text = string.Empty;
            txtSujeto0.Enabled = false;
            txtUnidadPrivativa.Text = string.Empty;
            txtUnidadPrivativa.Enabled = false;
            rfvUnidadPrivativa.Enabled = false;
            RestriccionesFiltro(TipoFiltroBusqueda.Fecha);
            HiddenFechaFin.Value = string.Empty;
            HiddenFechaIni.Value = string.Empty;
            gridViewDeclaraciones.Sort(String.Empty, SortDirection.Ascending);
            CargarDataTable(TipoFiltroBusqueda.Fecha);
            gridViewDeclaraciones.PageIndex = 0;
            gridViewDeclaraciones.DataBind();
            gridViewDeclaraciones.SelectedIndex = -1;
            RestriccionesBotones();
            UpdatePanelBusqueda.Update();
            UpdatePanelGrid.Update();
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
    /// Maneja el evento Click del control btnGenerarJustificante
    /// Redirecciona al justificante de la declaración seleccionada
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnGenerarJustificantes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_JUSTIFICANTE;
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, string.Empty);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
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
    /// Maneja el evento Click del control btnGenerarAcuse
    /// Redirecciona al acuse de la declaración seleccionada
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnGenerarAcuse_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_ACUSE;
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, string.Empty);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
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
    #endregion

    #region Eventos del GridView

    /// <summary>
    /// Carga el gridview de declaraciones con los criterios de búsqueda introducidos
    /// </summary>
    protected void CargarGridView()
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
            else if (rbSujeto.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.Sujeto);
                hidBusquedaActual.Value = TipoFiltroBusqueda.Sujeto.ToInt().ToString();
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
    /// Actualiza los botones dependiendo de la declaración seleccionada
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewDeclaraciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
            RestriccionesBotones();
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
    /// Maneja el evento PageIndexChanging del control gridViewDeclaraciones
    /// Carga las declaraciones correspondientes a la nueva página del grid
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
    /// Maneja el evento RowDataBound del control gridViewDeclaraciones
    /// Ajusta los datos que se muestran en los campos del grid
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewDeclaraciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CheckBox CheckSiscor = (CheckBox)e.Row.FindControl("checkboxSISCOR");
                CheckSiscor.Checked = e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_DECLARACIONRow>().CODESTADOPAGO == EstadosPago.RecibidoSISCOR.ToDecimal() ? true : false;
                
                if (e.Row.Cells[2].Text.Trim().Length > 20)
                    e.Row.Cells[2].Text = string.Concat(e.Row.Cells[2].Text.Trim().Substring(0, 20), " ...");

            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                lblCount.Text = string.Empty;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == 0)
            {
                DataRowView t = (DataRowView)e.Row.DataItem;
                lblCount.Text = String.Format("Se ha encontrado un total de {0} declaración(es)", t["ROWS_TOTAL"]);
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
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewDeclaraciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortExpression = e.SortExpression;
            SortDirectionP = e.SortDirection.ToString();
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

    #region Excepciones


    /// <summary>
    /// Pre-renderizado de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
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
