using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;


/// <summary>
/// Clase encargada del control de Exenciones
/// </summary>
public partial class UserControls_ModalBuscarExenciones : System.Web.UI.UserControl
{

    #region Delegado y Evento

    ///Declaración y generación de un evento que utiliza EventHandler como tipo de delegado subyacente.
    /// Delegado que define a los manejadores que podrán 
    /// Se ha declarado un tipo delegado, al que hemos llamado EventClickHandler
    /// que representa a una función que no devuelve nada y que recibe como argumento un objeto y un CancelEventArgs. 
    /// Como es un tipo no es necesario definirlo dentro de una clase. 
    /// Como resultado vamos a poder declarar variables del tipo EventClickHandler.

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
    ///  Método que llama al evento (EventClick)
    ///  Proporciona datos para un evento cancelable, CancelEventArgs
    ///  Propiedad Cancel de CancelEventArgs, obtiene o establece un valor que indica si se debe cancelar el evento. 
    /// </summary>
    /// <param name="cancel">Indica el valor para la propiedad cancel CancelEventArgs</param>
    private void LaunchEventClick(Boolean cancel)
    {
         if (EventClick != null)
             //Inicializa una nueva instancia de la clase CancelEventArgs, estableciendo la propiedad Cancel en el valor dado. 
            EventClick(this, new CancelEventArgs(cancel));
    }

    #endregion


    #region Propiedades exencion

   /// <summary>
   ///Propiedad que obtiene o establece el ID de la exención
   /// </summary>
    public int? IdExencion
    {
        get
        {
            if (ViewState["IdExencion"] == null)
                return null;
            else
                return (ViewState["IdExencion"]).ToInt();
        }
        set
        {
            ViewState["IdExencion"] = value;
        }
    }


    /// <summary>
    /// Propiedad que obtiene o establece la Descripción de la exención
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
    /// Propiedad que obtiene o establece la Fraccion
    /// </summary>
    public string Fraccion
    {
        get
        {
            if (ViewState["Fraccion"] == null)
                return null;
            else
                return (ViewState["Fraccion"]).ToString();
        }
        set
        {
            ViewState["Fraccion"] = value;
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



    protected void Page_Load(object sender, EventArgs e)
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
    /// Método que limpia los datos de la pantalla
    /// </summary>
    private void LimpiarDatos()
    {
        txtArticulo.Text = string.Empty;
        txtDescripcion.Text = string.Empty;

        btnAceptar.Enabled = false;

        gridViewExenciones.Visible = false;

        //Actualizar paneles
        updatePanelBuscadorExenciones.Update();
      
    }

    /// <summary>
    /// Método que inicializa el gridview con las exenciones correspondiente
    /// </summary>
    public void Buscar()
    {
        try
        {
            gridViewExenciones.DataSourceID = odsExencionesPorArticuloDescripcion.ID;
            gridViewExenciones.PageIndex = 0;
            gridViewExenciones.SelectedIndex = -1;
            gridViewExenciones.DataBind();
            gridViewExenciones.Visible = true;

            updatePanelGridExenciones.Update();
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

    #region gridview

    /// <summary>
    /// Maneja el evento de SelectedIndexChanged del control gridViewExenciones. 
    /// </summary>
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewExenciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnAceptar.Enabled = true;
            IdExencion = gridViewExenciones.SelectedDataKey["IDEXENCION"].ToInt();
            Descripcion = gridViewExenciones.SelectedDataKey["DESCRIPCION"].ToString().Trim();
            Articulo = gridViewExenciones.SelectedDataKey["ARTICULO"].ToString().Trim();
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
    /// Maneja el evento de PageIndexChanging del control gridViewExenciones. 
    /// </summary>
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewExenciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridViewExenciones.SelectedIndex = -1;
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
    /// Maneja el evento de Sorting del control gridViewExenciones. 
    /// </summary>
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewSortEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewExenciones_Sorting(object sender, GridViewSortEventArgs e)
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
    /// Maneja el evento de RowDataBound del control gridViewExenciones. 
    /// </summary>
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.GridViewRowEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewExenciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            if (IdExencion.HasValue && gridViewExenciones.DataKeys[e.Row.RowIndex]["IDEXENCION"].ToString() == IdExencion.Value.ToString())
            {
                gridViewExenciones.SelectedIndex = e.Row.RowIndex;
                e.Row.RowState = DataControlRowState.Selected;
            }
    }
    #endregion

    #region Botones

    /// <summary>
    /// Maneja el evento de Click del control btnBuscarExencionesModalFiltrar. 
    /// </summary>
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnBuscarExencionesModalFiltrar_Click(object sender, EventArgs e)
    {
        try
        {
            LaunchEventClick(false);
            txtArticulo.Text = txtArticulo.Text.Trim();
            gridViewExenciones.DataSourceID = odsExencionesPorArticuloDescripcion.ID;
            gridViewExenciones.PageIndex = 0;
            gridViewExenciones.SelectedIndex = -1;
            gridViewExenciones.DataBind();
            gridViewExenciones.Visible = true;

            updatePanelGridExenciones.Update();

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
            //this.ViewState["Seleccionado"] = true;
            LaunchEventClick(true);
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
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            IdExencion = null;
            Descripcion = null;
            Articulo = null;
            LaunchEventClick(true);
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
    /// Maneja el evento de Click del control btnCerrarBuscadorExenciones. 
    /// </summary>
    /// <param name="sender">El origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnCerrarBuscadorExenciones_Click(object sender, ImageClickEventArgs e)
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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
