using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;

/// <summary>
/// Clase que gestiona el user control ModalBuscarPeritos
/// </summary>
public partial class UserControls_ModalBuscarPeritos : System.Web.UI.UserControl
{

    #region Delegado y Evento

    /// <summary>
    /// Delegado handler del evento ConfirmClick. Evento cancelable.
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de la clase CancelEventArgs</param>
    public delegate void ConfirmClickHandler(object sender, CancelEventArgs e);

    /// <summary>
    /// Declaración de un nuevo evento para el user control con su ClickHandler (ConfirmClickHandler)
    /// </summary>
    public event ConfirmClickHandler ConfirmClick;

    /// <summary>
    /// Método que llama al evento (ConfirmClick)
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de la clase CancelEventArgs</param>
    private void LaunchConfirmClickHandler(object sender, CancelEventArgs e)
    {
        if (ConfirmClick != null)
            ConfirmClick(sender, e);
    }

    #endregion

    #region Propiedades del control

    /// <summary>
    /// Propiedad que obtiene un booleano
    /// </summary>
    public bool Seleccionado
    {
        get { return (bool)this.ViewState["Seleccionado"]; }
    }

    /// <summary>
    /// Propiedad que obtiene el número de registro
    /// </summary>
    public string NumeroRegistro
    {
        get
        {
            if (ddlOpcionBusqueda.SelectedValue.Equals(Constantes.DDLBUSQUEDA_PERITOS))
            {
                if (gridViewPersonas.SelectedIndex > -1)
                    return gridViewPersonas.DataKeys[gridViewPersonas.SelectedIndex].Values[0].ToString();
            }
            if (ddlOpcionBusqueda.SelectedValue.Equals(Constantes.DDLBUSQUEDA_SOCIEDADES))
            {
                if (gridViewSociedades.SelectedIndex > -1)
                    return gridViewSociedades.DataKeys[gridViewSociedades.SelectedIndex].Values[0].ToString();
            }
            return "";
        }
    }


    private int tipoRegistro = -1;
    /// <summary>
    /// Propiedad que obtiene el valor correspondiente: (1) perito, (0) sociedad
    /// </summary>
    public int TipoRegistro 
    {
        get
        {
            return tipoRegistro;
        }
    }


    #endregion

    #region Métodos

    protected void Page_Load(object sender, EventArgs e)
    {

    }

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

#endregion

    #region Manejadores

    /// <summary>
    /// Maneja el evento de Click del control btnBuscarPersonaModalFiltrar. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnBuscarPersonaModalFiltrar_Click(object sender, EventArgs e)
    {
        try
        {
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));

            if (ddlOpcionBusqueda.SelectedValue.Equals(Constantes.DDLBUSQUEDA_PERITOS))
            {
                MostrarPanelPerito();
                gridViewPersonas.DataSourceID = odsPorBusquedaPerito.ID;

                gridViewPersonas.DataBind();
                gridViewPersonas.Visible = true;
            }

            if (ddlOpcionBusqueda.SelectedValue.Equals(Constantes.DDLBUSQUEDA_SOCIEDADES))
            {
                MostrarPanelSociedad();
                gridViewSociedades.DataSourceID = odsPorBusquedaSociedad.ID;
                gridViewSociedades.DataBind();
                gridViewSociedades.Visible = true;

            }

            pnlBucarPersonasModalFiltros.Update();
            UpdatePanel1.Update();
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
    /// Maneja el evento de SelectedIndexChanged del control gridViewPersonas. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnAceptar.Enabled = true;
            this.ViewState["Seleccionado"] = true;
            updatePanelBotones.Update();
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
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
    /// Maneja el evento de SelectedIndexChanged del control gridViewSociedades. 
    /// </summary>
    /// <param name="sender">Oriegen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewSociedades_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnAceptar.Enabled = true;
            this.ViewState["Seleccionado"] = true;
            updatePanelBotones.Update();
            ///Lanza el evento ConfirmClick
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
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
    /// Maneja el evento de PageIndexChanging del control gridViewPersonas. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewPersonas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
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
    /// Maneja el evento de PageIndexChanging del control gridViewSociedades. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewSociedades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
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
    /// Maneja el evento de Sorting del control gridViewPersonas. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewSortEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewPersonas_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
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
    /// Maneja el evento de Sorting del control gridViewSociedades. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewSortEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewSociedades_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
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
    /// Maneja el evento de Click del control btnBuscarPersonaModalCancelar. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnBuscarPersonaModalCancelar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.ViewState["Seleccionado"] = false;
            LaunchConfirmClickHandler(sender, new CancelEventArgs(true));
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
    /// Maneja el evento de Click del control btnAceptar. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlOpcionBusqueda.SelectedValue.Equals(Constantes.DDLBUSQUEDA_PERITOS))
            {
                if (gridViewPersonas.SelectedIndex > -1)
                    this.ViewState["Seleccionado"] = true;
            }
            if (ddlOpcionBusqueda.SelectedValue.Equals(Constantes.DDLBUSQUEDA_SOCIEDADES))
            {
                if (gridViewSociedades.SelectedIndex > -1)
                    this.ViewState["Seleccionado"] = true;
            }
            LaunchConfirmClickHandler(sender, new CancelEventArgs(true));
            LimpiarDatos();
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
    /// Maneja el evento de Click del control btnCancelar. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            this.ViewState["Seleccionado"] = false;
            LaunchConfirmClickHandler(sender, new CancelEventArgs(true));
            LimpiarDatos();
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
    /// Maneja el evento de SelectedIndexChanged del control ddlOpcionBusqueda. 
    /// </summary>
    /// <param name="sender">Origen de datos</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void ddlOpcionBusqueda_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LimpiarDatosTexto();
            LimpiarDataSources();
            if (ddlOpcionBusqueda.SelectedValue.Equals(Constantes.DDLBUSQUEDA_PERITOS))
            {
                MostrarPanelPerito();
            }
            else
            {
                MostrarPanelSociedad();
            }
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
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

    #region TxtChanged 
    
    /// <summary>
    /// Maneja el evento de TextChanged del control txtPeritoRFC. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void txtPeritoRFC_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtPeritoRFC.Text))
            {
                txtNumeroRegistro.Text = string.Empty;
                TextBox1.Text = txtPeritoRFC.Text;
            }
            else TextBox1.Text = string.Empty;
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
    /// Maneja el evento de TextChanged del control txtPeritoCURP. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void txtPeritoCURP_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtPeritoCURP.Text))
            {
                txtNumeroRegistro.Text = string.Empty;
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
    /// Maneja el evento de TextChanged del control txtPeritoIFE. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void txtPeritoIFE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtPeritoIFE.Text))
            {
                txtNumeroRegistro.Text = string.Empty;
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

    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #region LimpiarDatos

    /// <summary>
    /// Método que devuelve todos los valores del user control al valor inicial
    /// </summary>
    private void LimpiarDatos()
    {
        LimpiarDatosTexto();
        //Visualizar el panel por defecto
        ddlOpcionBusqueda.SelectedValue = Constantes.DDLBUSQUEDA_PERITOS;
        MostrarPanelPerito();
        btnAceptar.Enabled = false;
        LimpiarDataSources();
    }

    /// <summary>
    /// Método que actualiza los gridviews a valores iniciales
    /// </summary>
    private void LimpiarDataSources()
    {
        gridViewPersonas.DataSource = null;
        gridViewPersonas.DataBind();

        gridViewSociedades.DataSource = null;
        gridViewSociedades.DataBind();

        gridViewPersonas.Visible = false;
        gridViewSociedades.Visible = false;

        UpdatePanel1.Update();
    }

    /// <summary>
    /// Método que resetea los textos de todos lo textbox
    /// </summary>
    private void LimpiarDatosTexto()
    {
        txtPeritoNombre.Text = string.Empty;
        txtNumeroRegistro.Text = string.Empty;
        txtPeritoApellidoMaterno.Text = string.Empty;
        txtPeritoApellidoPaterno.Text = string.Empty;
        txtPeritoCURP.Text = string.Empty;
        txtPeritoIFE.Text = string.Empty;
        txtPeritoRFC.Text = string.Empty;
    }

    /// <summary>
    /// Método que hace aparecer los controles referentes a Perito
    /// </summary>
    private void MostrarPanelPerito()
    {
        this.lblPeritoApellidoPaterno.Visible = true;
        this.txtPeritoApellidoPaterno.Visible = true;
        this.lblPeritoApellidoMaterno.Visible = true;
        this.txtPeritoApellidoMaterno.Visible = true;
        tipoRegistro = 1;
        lblBuscarPersonaModalNombre.Text = "Nombre";

    }

    /// <summary>
    /// Método que hace aparecer los controles referentes a Sociedades
    /// </summary>
    private void MostrarPanelSociedad()
    {
        this.lblPeritoApellidoPaterno.Visible = false;
        this.txtPeritoApellidoPaterno.Visible = false;
        this.lblPeritoApellidoMaterno.Visible = false;
        this.txtPeritoApellidoMaterno.Visible = false;
        txtPeritoApellidoPaterno.Text = string.Empty;
        txtPeritoApellidoMaterno.Text = string.Empty;

        tipoRegistro = 0;
        lblBuscarPersonaModalNombre.Text = "Razón Social";
    }

    #endregion
}
