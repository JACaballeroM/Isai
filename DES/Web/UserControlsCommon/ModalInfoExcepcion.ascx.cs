using System;
using System.ComponentModel;
using SIGAPred.Excepciones;

public partial class UserControlsCommon_ModalInfoExcepcion : System.Web.UI.UserControl
{
    #region Propiedades

    /// <summary>
    /// Retorna el clientid del botón de cancelar, para evitar el refresco en el cliente
    /// </summary>
    public string ClientIdCancelacion
    {
        get
        {
            return btnErrorAceptar.ClientID;
        }
    }

    /// <summary>
    /// Establece o retorna el texto indicado en el label de error basico
    /// </summary>
    public string TextoBasicoMostrar
    {
        get { return lblErrorBasico.Text; }
        set { lblErrorBasico.Text = value; }
    }

    /// <summary>
    /// Establece o retorna el texto indicado en el label de error avanzado
    /// </summary>
    public string TextoAvanzadoMostrar
    {
        get { return lblErrorAvanzado.Text; }
        set { lblErrorAvanzado.Text = value; }
    }

    /// <summary>
    /// Establece o retorna el titulo del control
    /// </summary>
    public string TextoTitulo
    {
        get { return lblTextoTitulo.Text; }
        set { lblTextoTitulo.Text = value; }
    }

    /// <summary>
    /// Establece o retorna el texto del boton Aceptar
    /// </summary>
    public string TextoLinkAceptar
    {
        get { return btnErrorAceptar.Text; }
        set { btnErrorAceptar.Text = value; }
    }

    /// <summary>
    /// Establece o retorna el texto del boton Mostrar/Ocultar
    /// </summary>
    public string TextoLinkMostrarOcultar
    {
        get { return btnErrorMostrar.Text; }
        set { btnErrorMostrar.Text = value; }
    }

    #endregion

    #region Eventos Página

    /// <summary>
    /// En este evento registramos el javascript
    /// </summary>
    /// <param name="sender"></param>
    /// <para m name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnErrorMostrar.OnClientClick = String.Format("javascript:ocultarPanelError(this,{0});return false;", panError.ClientID);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            throw;
        }
    }

    /// <summary>
    /// En este evento se establece que el panel este oculto por defecto, ademas se establece el texto del boton a Mostrar Detalles
    /// </summary>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            panError.Style["display"] = "none";
            btnErrorMostrar.Text = "Mostrar Detalles";
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            throw;
        }
    }
    #endregion
}
