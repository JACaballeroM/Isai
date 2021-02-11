using System;
using System.ComponentModel;
using System.ServiceModel;
using ServiceDeclaracionIsai;

/// <summary>
/// Clase que gestiona el user control ModalDireccion
/// </summary>
public partial class UserControls_ModalDireccion : System.Web.UI.UserControl
{
    #region Propiedades

    /// <summary>
    /// Establece y obtiene el valor del textbox txtAndador
    /// </summary>
    public string TextAndador
    {
        get
        {
            if (string.IsNullOrEmpty(txtAndador.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtAndador.Text;
            }
        }
        set
        {
            txtAndador.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtAsentamiento
    /// </summary>
    public string TextoAsentamiento
    {
        get
        {
            if (string.IsNullOrEmpty(txtColoniaAsentamiento.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtColoniaAsentamiento.Text;
            }
        }
        set
        {
            txtColoniaAsentamiento.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtCalle1
    /// </summary>
    public string TextoCalle1
    {
        get
        {
            if (string.IsNullOrEmpty(txtEntreCalle1.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtEntreCalle1.Text;
            }
        }
        set
        {
            txtEntreCalle1.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtCalle2
    /// </summary>
    public string TextoCalle2
    {
        get
        {
            if (string.IsNullOrEmpty(txtEntreCalle2.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtEntreCalle2.Text;
            }
        }
        set
        {
            txtEntreCalle2.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtCP
    /// </summary>
    public string TextoCp
    {
        get
        {
            if (string.IsNullOrEmpty(txtCP.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtCP.Text;
            }
        }
        set
        {
            txtCP.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el texto del textbox txtDelegacion
    /// </summary>
    public string TextoDelegacion
    {
        get
        {
            if (ddlDelegacion.DataTextField.ToString() == Constantes.VALIDARDELEGACION) 
            {
                return string.Empty;
            }
            else
            {
               return txtDelegacion.Text;
            }
        }
        set
        {
            ddlDelegacion.Items.FindByText(value).Selected = true;    
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtEdificio
    /// </summary>
    public string TextoEdificio
    {
        get
        {
            if (string.IsNullOrEmpty(txtEdificio.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtEdificio.Text;
            }
        }
        set
        {
            txtEdificio.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtEntrada
    /// </summary>
    public string TextoEntrada
    {
        get
        {
            if (string.IsNullOrEmpty(txtEntrada.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtEntrada.Text;
            }
        }
        set
        {
            txtEntrada.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtIndicaciones
    /// </summary>
    public string TextoIndicaciones
    {
        get
        {
            if (string.IsNullOrEmpty(txtIndAdi.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtIndAdi.Text;
            }
        }
        set
        {
            txtIndAdi.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtLocalidad
    /// </summary>
    public string TextoLocalidad
    {
        get
        {
            if (ddlLocalidad.DataTextField.ToString() == Constantes.VALIDARLOCALIDAD) 
            {
                return string.Empty;
            }
            else
            {
                return txtLocalidad.Text;
            }
        }
        set
        {
            ddlLocalidad.DataTextField = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtNumExterior
    /// </summary>
    public string TextoNumExterior
    {
        get
        {
            if (string.IsNullOrEmpty(txtNumExt.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtNumExt.Text;
            }
        }
        set
        {
            txtNumExt.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtNumInterior
    /// </summary>
    public string TextoNumInterior
    {
        get
        {
            if (string.IsNullOrEmpty(txtNumInt.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtNumInt.Text;
            }
        }
        set
        {
            txtNumInt.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtSeccion
    /// </summary>
    public string TextoSeccion
    {
        get
        {
            if (string.IsNullOrEmpty(txtSeccion.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtSeccion.Text;
            }
        }
        set
        {
            txtSeccion.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtTelefono
    /// </summary>
    public string TextoTelefono
    {
        get
        {
            if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtTelefono.Text;
            }
        }
        set
        {
            txtTelefono.Text = value;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtTipoAsentamiento
    public string TextoTipoAsentamiento
    {
        get
        {
            if (ddlTipoAsentamiento.DataTextField.ToString() == Constantes.VALIDARTIPOASENTAMIENTO)
            {
                return string.Empty;
            }
            else
            {
                return txtTipoAsentamiento.Text;
            }
        }
        set
        {
            ddlTipoAsentamiento.Items.FindByText(value).Selected = true;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtTipoVia
    public string TextoTipoVia
    {
        get
        {
            if (ddlTipoVia.DataTextField.ToString() == Constantes.VALIDARTIPOVIA)
            {
                return string.Empty;
            }
            else
            {
               return txtTipoVia.Text;
            }
        }
        set
        {
            ddlTipoVia.Items.FindByText(value).Selected = true;
        }
    }

    /// <summary>
    /// Establece y obtiene el valor del textbox txtVia
    public string TextoVia
    {
        get
        {
            if (string.IsNullOrEmpty(txtVia.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtVia.Text;
            }
        }
        set
        {
            txtVia.Text = value;
        }
    }

    
    /// <summary>
    /// Almacena el valor que tiene la colonia buscada
    /// </summary>
    public decimal ValorColonia
    {
        get
        {
            if (ViewState[Constantes.VALORCOLONIA] == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToDecimal(ViewState[Constantes.VALORCOLONIA]);
            }
        }
        set { ViewState[Constantes.VALORCOLONIA] = value; }
    }

    /// <summary>
    /// Almacena y obtiene el valor que se selecciona en el dropdown de delegaciones
    /// </summary>
    public string ValorDelegacion
    {

        get
        {
            if (string.IsNullOrEmpty(txtValorDelegacion.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtValorDelegacion.Text;
            }
        }
        set
        {
            txtValorDelegacion.Text = value;
        }
    }

    /// <summary>
    /// Almacena y obtiene el valor que se seleccina en el dropdown de localidad
    /// </summary>
    public string ValorLocalidad
    {
        get
        {
            if (string.IsNullOrEmpty(txtValorLocalidad.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtValorLocalidad.Text;
            }
        }
        set
        {
            txtValorLocalidad.Text = value;
        }
    }

    /// <summary>
    /// Almacena y obtiene el valor que se selecciona en el dropdown de tipos de asentamiento
    /// </summary>
    public string ValorTipoAsentamiento
    {
        get
        {
            if (string.IsNullOrEmpty(txtValorTipoAsentamiento.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtValorTipoAsentamiento.Text;
            }
        }
        set
        {
            txtValorTipoAsentamiento.Text = value;
        }
    }

    /// <summary>
    /// Almacena y obtiene el valor que se seleccina en el dropdown de tipos de via
    /// </summary>
    public string ValorTipoVia
    {
        get
        {
            if (string.IsNullOrEmpty(txtValorTipoVia.Text))
            {
                return string.Empty;
            }
            else
            {
                return txtValorTipoVia.Text;
            }
        }
        set
        {
            txtValorTipoVia.Text = value;
        }
    }


    #endregion

    #region Delegado y Evento

    /// <summary>
    /// Delegado handler del evento EventClick. Evento cancelable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EventClickHandler(object sender, CancelEventArgs e);

    /// <summary>
    /// Declaración de un nuevo evento para el user control con su ClickHandler (EventClickHandler)
    /// </summary>
    public event EventClickHandler EventClick;

    /// <summary>
    ///  Método que llama al evento (EventClick)
    /// </summary>
    /// <param name="cancel">Instancia de la clase CancelEventArgs</param>
    private void LaunchEventClick(Boolean cancel)
    {
        if (EventClick != null)
            EventClick(this, new CancelEventArgs(cancel));
    }
    
    #endregion

    /// <summary>
    /// Carga los controles con sus datos correspondientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (ddlDelegacion.Items.Count > 0)
            {
                txtDelegacion.Text = ddlDelegacion.SelectedItem.ToString();
                ValorDelegacion = ddlDelegacion.SelectedValue.ToString();
            }
            ddlDelegacion.DataSource = ApplicationCache.Delegacion;
            ddlDelegacion.DataTextField = ApplicationCache.Delegacion.NOMBREColumn.ToString();
            ddlDelegacion.DataValueField = ApplicationCache.Delegacion.IDDELEGACIONColumn.ToString();
            ddlDelegacion.DataBind();

            if (ddlTipoVia.Items.Count > 0)
            {
                txtTipoVia.Text = ddlTipoVia.SelectedItem.ToString();
                ValorTipoVia = ddlTipoVia.SelectedValue.ToString();
            }
            ddlTipoVia.DataSource = ApplicationCache.CatalogosRCO.CatTiposVia;
            ddlTipoVia.DataTextField = ApplicationCache.CatalogosRCO.CatTiposVia.TIPOVIAColumn.ToString();
            ddlTipoVia.DataValueField = ApplicationCache.CatalogosRCO.CatTiposVia.CODTIPOSVIAColumn.ToString();
            ddlTipoVia.DataBind();

            if (ddlTipoAsentamiento.Items.Count > 0)
            {
                txtTipoAsentamiento.Text = ddlTipoAsentamiento.SelectedItem.ToString();
                ValorTipoAsentamiento = ddlTipoAsentamiento.SelectedValue.ToString();
            }
            ddlTipoAsentamiento.DataSource = ApplicationCache.CatalogosRCO.CatTiposAsentamiento;
            ddlTipoAsentamiento.DataTextField = ApplicationCache.CatalogosRCO.CatTiposAsentamiento.TIPOASENTAMIENTOColumn.ToString();
            ddlTipoAsentamiento.DataValueField = ApplicationCache.CatalogosRCO.CatTiposAsentamiento.CODTIPOSASENTAMIENTOColumn.ToString();
            ddlTipoAsentamiento.DataBind();

            if (ddlLocalidad.Items.Count > 0)
            {
                txtLocalidad.Text = ddlLocalidad.SelectedItem.ToString();
                ValorLocalidad = ddlLocalidad.SelectedValue.ToString();
            }
            ddlLocalidad.DataSource = ApplicationCache.CatalogosRCO.CatTiposLocalidad;
            ddlLocalidad.DataTextField = ApplicationCache.CatalogosRCO.CatTiposLocalidad.DESCRIPCIONColumn.ToString();
            ddlLocalidad.DataValueField = ApplicationCache.CatalogosRCO.CatTiposLocalidad.CODTIPOSLOCALIDADColumn.ToString();
            ddlLocalidad.DataBind();
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

  

    #region Eventos de los botones

    /// <summary>
    /// Maneja el evento de Click del control btnVolver. 
    /// </summary>
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnVolver_Click(object sender, EventArgs e)
    {
        try
        {
           
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
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtColoniaAsentamiento.Text) && !string.IsNullOrEmpty(txtVia.Text) && !string.IsNullOrEmpty(txtNumExt.Text))
            {
                LaunchEventClick(true);
            }
            else
            {
                btn_ModalPopupExtenderError.Show();
                LaunchEventClick(false);
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
    /// Maneja el evento de OnConfirmClick del control ModalErrorObligatorios. 
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.ComponentModel.EventArgs"/> que contiene los datos del evento</param>
    protected void ModalError_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            btn_ModalPopupExtenderError.Hide();
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
    /// <param name="mensaje">Texto a mostrar</param>
    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion
    
}


