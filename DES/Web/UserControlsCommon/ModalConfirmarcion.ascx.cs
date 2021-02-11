using System;
using System.ComponentModel;
using System.ServiceModel;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;


/// <summary>
/// Control de usuario modal de confirmación
/// </summary>
public partial class UserControlsCommon_ModalConfirmacion : System.Web.UI.UserControl
{
    #region Eventos

    /// <summary>
    /// Manejador que se llama al confirmar
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
    /// Texto de confirmación que aparece en el diálogo.
    /// </summary>
    public string TextoConfirmacion
    {
        get
        {
            return lblTextoConfirmacion.Text;
        }

        set
        {
            lblTextoConfirmacion.Text = value;
        }
    }

    /// <summary>
    /// Texto del título que aparece en el diálogo
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
    /// Texto del link de cancelar que aparece en el diálogo
    /// </summary>
    public string TextoLinkCancelar
    {
        get
        {
            return btnCancelar.Text;
        }
        set
        {
            btnCancelar.Text = value;
        }
    }

    /// <summary>
    /// Texto del link de confirmación que aparece en el diálogo
    /// </summary>
    public string TextoLinkConfirmacion
    {
        get
        {
            return btnConfirmar.Text;
        }
        set
        {
            btnConfirmar.Text = value;
        }
    }

    /// <summary>
    /// Texto del mensaje de progreso
    /// </summary>
    public string TextoProgreso
    {
        get
        {
            return ProgresoConfirmando.Mensaje;
        }
        set
        {
            ProgresoConfirmando.Mensaje = value;
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

    /// <summary>


    /// <summary>
    /// Activa o desactiva el txt de importe declarado
    /// </summary>
    public decimal ImporteDeclarado
    {
        set
        {
            btnConfirmar.CausesValidation = true;
            trImpDeclarado.Visible = true;
            txtImpDeclarado.Text = value.ToString();
        }
        get
        {
            return txtImpDeclarado.Text.ToDecimalFromStringFormatted();
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
    /// Manejador de eventos. Llamado por btnConfirmar para eventos click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void btnConfirmar_Click(object sender, EventArgs e)
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
    /// Executes the confirm click handler operation.
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
    /// Mostrar mensaje información excepción.
    /// </summary>
    /// <param name="mensaje">El mensaje.</param>
    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion

}
