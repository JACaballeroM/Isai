using System;
using System.ComponentModel;
using System.ServiceModel;
using ServiceDeclaracionIsai;

/// <summary>
/// Clase que gestiona el control de usuario ModalInfoCorreo
/// </summary>
public partial class UserControls_ModalInfoCorreo : System.Web.UI.UserControl
{

    #region Evento y delegado

    /// <summary>
    /// Delegado handler del evento ConfirmClick. Evento cancelable.
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
    /// Establece y obtiene el Texto de confirmación que aparece en el diálogo
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
    /// Establece y obtiene el Texto del título que aparece en el diálogo
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

    #endregion

    #region Eventos Página

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Maneja el evento Click del botón Aceptar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAceptar_Click(object sender, EventArgs e)
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

    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion
}
