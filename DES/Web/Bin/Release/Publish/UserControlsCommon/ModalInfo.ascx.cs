using System;
using System.ComponentModel;
using System.ServiceModel;
using ServiceDeclaracionIsai;

public partial class UserControlsCommon_ModalInfo : System.Web.UI.UserControl
{

    #region Eventos

    /// <summary>
    /// Confirm click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento Cancel.</param>
    public delegate void ConfirmClickHandler(object sender, CancelEventArgs e);

    /// <summary>
    /// Cola de evento para todos los elementos suscritos al evento de ConfirmClick.
    /// </summary>
    public event ConfirmClickHandler ConfirmClick;

    #endregion

    #region Propiedades

    /// <summary>
    /// Obtiene o establece un valor que indica si contiene tipo mensaje.
    /// </summary>
    /// <value>
    /// true if tipo mensaje, false if not.
    /// </value>
    public bool TipoMensaje
    {
        get { return Convert.ToBoolean(ViewState["tipoMensaje"]); }
        set { ViewState["tipoMensaje"] = value; }
    }

    /// <summary>
    /// Retorna el clientid del botón de cancelar, para evitar el refresco en el cliente
    /// </summary>
    public string ClientIdCancelacion
    {
        get
        {
            return btnCancelar.ClientID;
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
            return btnOk.Text;
        }
        set
        {
            btnOk.Text = value;
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

    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Eventos Linkbutton

    /// <summary>
    /// Manejador de eventos. Llamado por btnCancelar para eventos click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void btnCancelar_Click(object sender, EventArgs e)
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

    #region Métodos Privados

    /// <summary>
    /// Lanza el evento ConfirmClick
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    private void LaunchConfirmClickHandler(object sender, CancelEventArgs e)
    {
        if (ConfirmClick != null)
        {
            ConfirmClick(sender, e);
        }
    }
    #endregion


    #region Excepciones

    /// <summary>
    /// Pre-renderizado de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
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
