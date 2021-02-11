using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;



/// <summary>
/// Clase que gestiona el user control ModalBuscarReducciones
/// </summary>
public partial class UserControls_ModalBuscarReducciones : System.Web.UI.UserControl
{
    #region Delegado y Evento

    /// <summary>
    /// Delegado handler del evento EventClick. Evento cancelable.
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de la clase CancelEventArgs</param>
    public delegate void EventClickHandler(object sender, CancelEventArgs e);

    /// <summary>
    /// Declaración de un nuevo evento para el user control con su ClickHandler (EventClickHandler)
    /// </summary>
    public event EventClickHandler EventClick;

    /// <summary>
    /// Método que llama al evento (EventClick)
    /// </summary>
    /// <param name="cancel"></param>
    private void LaunchEventClick(Boolean cancel)
    {
        if (EventClick != null)
            EventClick(this, new CancelEventArgs(cancel));
    }

    #endregion

    #region Propiedades reduccion

    /// <summary>
    /// Propiedad que obtiene o establece el ID de la reducción
    /// </summary>
    public int? IdReduccion
    {
        get
        {
            if (ViewState["IdReduccion"] == null)
                return null;
            else
                return (ViewState["IdReduccion"]).ToInt();
        }
        set
        {
            ViewState["IdReduccion"] = value;
        }
    }

    /// <summary>
    /// Propiedad que obtiene o establece la Descripción de la reducción
    /// </summary>
    public string Descripcion
    {
        get
        {
            if (ViewState["Descripcion"] == null)
                return null;
            else
                return (ViewState["Descripcion"]).ToString();
        }
        set
        {
            ViewState["Descripcion"] = value;
        }
    }


    /// <summary>
    /// Propiedad que obtiene o establece el Artículo
    /// </summary>
    public string Articulo
    {
        get
        {
            if (ViewState["Articulo"] == null)
                return null;
            else
                return (ViewState["Articulo"]).ToString();
        }
        set
        {
            ViewState["Articulo"] = value;
        }
    }

    /// <summary>
    /// Propiedad que obtiene o establece el año
    /// </summary>
    public int Anio
    {
        get
        {
            return HiddenAnio.Value.ToInt();
        }
        set
        {
            HiddenAnio.Value = value.ToString();
        }
    }

    #endregion

    #region Métodos y funciones


    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Método que limpia los datos de la pantalla
    /// </summary>
    private void LimpiarReducciones()
    {
        txtBuscarReduccionesModalArticulo.Text = string.Empty;
        txtBuscarReduccionesModalDescripción.Text = string.Empty;

        btnAceptar.Enabled = false;
        gridViewReducciones.Visible = false;

        updatePanelBuscadorReducciones.Update();
    }

    /// <summary>
    /// Método que inicializa el gridview con las reducciones correspondientes
    /// </summary>
    public void Buscar()
    {
        try
        {
            gridViewReducciones.DataSourceID = odsReduccionesPorArticuloDescripcion.ID;
            gridViewReducciones.PageIndex = 0;
            gridViewReducciones.SelectedIndex = -1;
            gridViewReducciones.DataBind();
            gridViewReducciones.Visible = true;

            updatePanelGridReducciones.Update();
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

    #region Gridview

    /// <summary>
    /// Maneja el evento de SelectedIndexChanged del control gridViewReducciones. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewReducciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnAceptar.Enabled = true;
            IdReduccion = gridViewReducciones.SelectedDataKey["IDREDUCCION"].ToInt();
            Descripcion = gridViewReducciones.SelectedDataKey["DESCRIPCION"].ToString().Trim();
            Articulo = gridViewReducciones.SelectedDataKey["ARTICULO"].ToString().Trim();
            updatePanelBotones.Update();
            LaunchEventClick(false);
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
    /// Maneja el evento de PageIndexChanging del control gridViewReducciones. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewReducciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridViewReducciones.SelectedIndex = -1;
            LaunchEventClick(false);
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
    /// Maneja el evento de Sorting del control gridViewReducciones. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewSortEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewReducciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            LaunchEventClick(false);
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
    /// Maneja el evento de RowDataBound del control gridViewReducciones. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewRowEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewReducciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            if (IdReduccion.HasValue && gridViewReducciones.DataKeys[e.Row.RowIndex]["IDREDUCCION"].ToString() == IdReduccion.Value.ToString())
            {
                gridViewReducciones.SelectedIndex = e.Row.RowIndex;
                e.Row.RowState = DataControlRowState.Selected;
            }
    }
    #endregion
      
    #region Botones

    /// <summary>
    /// Maneja el evento Click del control btnBuscarReduccionesModalFiltrar. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnBuscarReduccionesModalFiltrar_Click(object sender, EventArgs e)
    {
        try
        {
            LaunchEventClick(false);
            txtBuscarReduccionesModalArticulo.Text = txtBuscarReduccionesModalArticulo.Text.Trim();
            gridViewReducciones.DataSourceID = odsReduccionesPorArticuloDescripcion.ID;
            gridViewReducciones.PageIndex = 0;
            gridViewReducciones.SelectedIndex = -1;
            gridViewReducciones.DataBind();
            gridViewReducciones.Visible = true;

            updatePanelGridReducciones.Update();

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
    /// Maneja el evento Click del control btnAceptar. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            LaunchEventClick(true);
            LimpiarReducciones();
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
    /// Maneja el evento Click del control btnCancelar. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            IdReduccion = null;
            Descripcion = null;
            Articulo = null;
            LaunchEventClick(true);
            LimpiarReducciones();
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
    /// Maneja el evento Click del control btnCerrarBuscadorReducciones. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnCerrarBuscadorReducciones_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnCancelar_Click(sender, new EventArgs());
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
    /// Muestra un mensaje de Error
    /// </summary>
    /// <param name="mensaje">Muestra el texto a mostrar</param>
    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion
        
}
