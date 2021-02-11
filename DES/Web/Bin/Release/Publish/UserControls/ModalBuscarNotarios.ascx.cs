using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ServiceModel;
using ServiceAvaluos;
using System.ComponentModel;

/// <summary>
/// Modal para la búsqueda de notarios.
/// </summary>
public partial class UserControls_ModalBuscarNotarios : System.Web.UI.UserControl
{
    #region Propiedad WCF Notarios

    /// <summary>
    /// Cliente para el servicio de avalúos.
    /// </summary>
    private ServiceAvaluos.AvaluosClient clienteNotario = null; 

    /// <summary>
    /// Obtiene el cliente notario.
    /// </summary>
    /// <value>
    /// El cliente notario.
    /// </value>
    protected ServiceAvaluos.AvaluosClient ClienteNotario
    {
        get
        {
            if (clienteNotario == null)
            {
                clienteNotario = new ServiceAvaluos.AvaluosClient(); //ServiceNotarios.NotariosClient();
            }

            return clienteNotario;
        }
    }
 
    #endregion

    #region Delegado y Evento

    /// <summary>
    /// Delegado para confirmación.
    /// </summary>
    /// <param name="sender">Origen.</param>
    /// <param name="e">Evento.</param>
    public delegate void ConfirmClickHandler(object sender, CancelEventArgs e);
    /// <summary>
    /// Evento para confirmación.
    /// </summary>
    public event ConfirmClickHandler ConfirmClick;

    #endregion

    #region Propiedades del control

    /// <summary>
    /// Propiedad que contiene si algún notario ha sido seleccionado.
    /// </summary>
    /// <value>
    /// true si está seleccionado, false si no.
    /// </value>
    public bool Seleccionado
    {
        get { return (bool)this.ViewState[Constantes.PAR_VIEWSTATE_SELECCIONADO]; }
    }

    /// <summary>
    /// Propiedad que contiene el indentidicador de persona del notario.
    /// </summary>
    /// <value>
    /// Identidicador de la persona notario.
    /// </value>
    public int IdentificadorPersonaNotario
    {
        get
        {
            return Convert.ToInt32(gridViewPersonas.DataKeys[gridViewPersonas.SelectedIndex].Values[0].ToString());
        }
    }

    /// <summary>
    /// Propiedad que contiene el número de indentidicador del notario.
    /// </summary>
    /// <value>
    /// Número de indentidicador del notario.
    /// </value>
    public int NumeroIdentificadorNotario
    {
        get
        {
            return Convert.ToInt32(gridViewPersonas.DataKeys[gridViewPersonas.SelectedIndex].Values["NUMERO"].ToString());
        }
    }


    /// <summary>
    /// Propiedad que contiene nombre del notario.
    /// </summary>
    /// <value>
    /// Número de indentidicador del notario.
    /// </value>
    public string NombreNotario
    {
        get
        {
            return gridViewPersonas.DataKeys[gridViewPersonas.SelectedIndex].Values["NOMBRE"].ToString() + " " + gridViewPersonas.DataKeys[gridViewPersonas.SelectedIndex].Values["APELLIDOPATERNO"].ToString() + " " + gridViewPersonas.DataKeys[gridViewPersonas.SelectedIndex].Values["APELLIDOMATERNO"].ToString();
        }
    }

    #endregion
    
    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnAceptar.Enabled = gridViewPersonas.SelectedIndex >= 0;
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    /// <summary>
    /// Manejador de eventos. Llamado por btnBuscarPersonaModalFiltrar para eventos click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void btnBuscarPersonaModalFiltrar_Click(object sender, EventArgs e)
    {
        try
        {
            //LaunchConfirmClickHandler(sender, new CancelEventArgs(false));

            txtIdNotario.Text = txtIdNotario.Text.Trim();

            gridViewPersonas.DataSourceID = odsPorBusquedaNotario.ID;
            gridViewPersonas.PageIndex = 0;
            gridViewPersonas.DataBind();
            gridViewPersonas.Visible = true;
        }
        catch (FaultException<ServiceAvaluos.AvaluosException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
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
    /// Ejecuta el manejador del evento  ConfirmClick
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    private void LaunchConfirmClickHandler(object sender, CancelEventArgs e)
    {
        if (ConfirmClick != null)
            ConfirmClick(sender, e);
    }

    /// <summary>
    /// Manejador de eventos. Llamado por gridViewPersonas para eventos cambio de indice.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void gridViewPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnAceptar.Enabled = true;
            updatePanelBotones.Update();
            //LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
        }
        catch (FaultException<ServiceAvaluos.AvaluosException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
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
    /// Manejador de eventos. Llamado por gridViewPersonas para eventos de cambio de página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void gridViewPersonas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
        }
        catch (FaultException<ServiceAvaluos.AvaluosException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
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
    /// Manejador de eventos. Llamado por gridViewPersonas para eventos ordenación.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void gridViewPersonas_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
        }
        catch (FaultException<ServiceAvaluos.AvaluosException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
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
    /// Manejador de eventos. Llamado por btnAceptar para eventos click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            this.ViewState[Constantes.PAR_VIEWSTATE_SELECCIONADO] = true;
            LaunchConfirmClickHandler(sender, new CancelEventArgs(false));
        }
        catch (FaultException<ServiceAvaluos.AvaluosException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
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
            LimpiarDatos();
            this.ViewState[Constantes.PAR_VIEWSTATE_SELECCIONADO] = false;
            LaunchConfirmClickHandler(sender, new CancelEventArgs(true));
           
        }
        catch (FaultException<ServiceAvaluos.AvaluosException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
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
    /// Manejador de eventos. Llamado por confirmar para eventos confirm click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    //protected void confirmar_ConfirmClick(object sender, CancelEventArgs e)
    //{
    //    try
    //    {
    //        if (!e.Cancel)
    //        {
               
    //            btnAceptar_ModalPopupExtender.Show();

    //                btnAceptar_Click(null, e);
     
    //        }
    //        else
    //        {
     
    //                LaunchConfirmClickHandler(sender, new CancelEventArgs(false));

    //        }
    //        LimpiarDatos();
    //    }
    //    catch (FaultException<ServiceAvaluos.AvaluosException> cex)
    //    {
    //        string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }
    //    catch (FaultException<ServiceAvaluos.AvaluosInfoException> ciex)
    //    {
    //        string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionPolicyWrapper.HandleException(ex);
    //        string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }

    //}

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


    /// <summary>
    /// Método que limpia los criterios de búsqueda.
    /// </summary>
    public void LimpiarDatos()
    {
        //TextBox
        txtIdNotario.Text = string.Empty;
        txtNotarioApellidoMaterno.Text = string.Empty;
        txtNotarioApellidoPaterno.Text = string.Empty;
        txtNotarioCURP.Text = string.Empty;
        txtNotarioIFE.Text = string.Empty;
        txtNotarioNombre.Text = string.Empty;
        txtNotarioRFC.Text = string.Empty;

        //GridView
        btnAceptar.Enabled = false;

        pnlBucarPersonasModalFiltros.Update();

        gridViewPersonas.DataSource = null;
        gridViewPersonas.SelectedIndex = -1;
        gridViewPersonas.Visible = false;
        UpdatePanel1.Update();
    }
}
