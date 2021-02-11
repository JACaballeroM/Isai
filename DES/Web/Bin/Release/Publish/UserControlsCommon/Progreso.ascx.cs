using System;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;

/// <summary>
/// Control de usuario de progreso
/// </summary>
public partial class UserControlsCommon_Progreso : System.Web.UI.UserControl
{

    /// <summary>
    /// Mensaje por defecto que mostrara el UpdateProgress
    /// </summary>
    private string mensaje = "Cargando datos...";


    /// <summary>
    /// Propiedad del UpdateProgress para indicar la propiedad DisplayAfter
    /// </summary>
    public int DisplayAfter
    {
        get
        {
            return uprCargando.DisplayAfter;
        }
        set
        {
            uprCargando.DisplayAfter = value;
        }
    }

    /// <summary>
    /// Propiedad del UpdateProgress para indicar la propiedad DynamicLayout
    /// </summary>
    public bool DynamicLayout
    {
        get
        {
            return uprCargando.DynamicLayout;
        }
        set
        {
            uprCargando.DynamicLayout = value;
        }
    }

    /// <summary>
    /// Propiedad del UpdateProgress para asociar a un updatePanel
    /// </summary>
    public string AssociatedUpdatePanelID
    {
        get
        {
            return uprCargando.AssociatedUpdatePanelID;
        }
        set
        {
            uprCargando.AssociatedUpdatePanelID = value;
        }
    }

    /// <summary>
    /// Mensaje que se muestra en el progreso, por defecto 'Cargando datos...'
    /// </summary>
    public string Mensaje
    {
        get
        {
            return mensaje;

        }
        set
        {
            mensaje = value;
        }
    }
    /// <summary>
    /// ClientID del UpdateProggress
    /// </summary>
    public string UpdateProgressClientID
    {
        get
        {
            return uprCargando.ClientID;
        }
    }


    /// <summary>
    /// Sobreescribimos el método pre-render
    /// </summary>
    /// <param name="e">Información del evento para enviar al manejador del evento registrado.</param>
    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            base.OnPreRender(e);
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
    /// Sobreescribimos el metodo render para actualizar la label del mensaje con el texto indicado en la propiedad
    /// </summary>
    /// <param name="writer"></param>
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            ((Label)uprCargando.Controls[0].Controls[1]).Text = mensaje;
            base.Render(writer);
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
