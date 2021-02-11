using System;
using System.ComponentModel;
using System.ServiceModel;
using ServiceDeclaracionIsai;

/// <summary>
/// Clase que gestiona el user control ModalErrorObligatorios
/// </summary>
public partial class UserControls_ModalErrorObligatorios : System.Web.UI.UserControl
{
    #region Eventos

    /// <summary>
    ///  Delegado handler del evento ConfirmClick. Evento cancelable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        {
            ConfirmClick(sender, e);
        }
    }


    #endregion

    #region Propiedades

    /// <summary>
    /// Establece y obtiene el Tipo de Mensaje
    /// </summary>
    public bool TipoMensaje
    {
        get 
        { 
            return Convert.ToBoolean(ViewState["tipoMensaje"]); 
        }
        set
        { 
            ViewState["tipoMensaje"] = value; 
        }
    }

    /// <summary>
    /// Retorna el clientid del botón de cancelar, para evitar el refresco en el cliente
    /// </summary>
    public string ClientIdCancelacion
    {
        get
        {
            return lnkCancelar.ClientID;
        }
    }

    /// <summary>
    /// Almacena y obtiene el texto de confirmación que aparece en el diálogo
    /// </summary>
    public string TextoInformacion
    {
        get
        {
            return lblTextoInformacion.Text;
        }

        set
        {
            lblTextoInformacion.Text = value;
        }
    }

    /// <summary>
    /// Almacena y obtiene el texto del título que aparece en el diálogo
    /// </summary>
    public string TextoTitulo
    {
        get
        {
            return lblTextoTitulo.Text;
        }

        set
        {
            lblTextoTitulo.Text = value;
        }
    }

    /// <summary>
    /// Almacena y obtiene el texto del link de cancelar que aparece en el diálogo
    /// </summary>
    public string TextoLinkCancelar
    {
        get
        {
            return lnkCancelar.Text;
        }
        set
        {
            lnkCancelar.Text = value;
        }
    }

    /// <summary>
    /// Activa o desactiva el título del diálogo
    /// </summary>
    public bool VisibleTitulo
    {
        set
        {
            trCabecera.Visible = value;
        }
        get
        {
            return trCabecera.Visible;
        }
    }

    #endregion

    #region Eventos Página

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Eventos Linkbutton

    /// <summary>
    /// Maneja el evento Click del control lnkCancelar
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void lnkCancelar_Click(object sender, EventArgs e)
    {
        try
        {
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
