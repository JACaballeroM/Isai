using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Web.UI;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;

/// <summary>
/// Clase que gestiona la control de usuario de reducciones
/// </summary>
public partial class UserControls_Reduccion : System.Web.UI.UserControl
{
   
    /// <summary>
    /// Propiedad que almacena y obtiene el identificador de la reducción seleccionada en el buscador
    /// </summary>
    public int IdReducccion
    {
        get
        {
            return this.ViewState["IdReducccion"].ToInt();
        }
        set
        {
            try
            {
                this.ViewState["IdReducccion"] = value;
                using (ServiceDeclaracionIsai.DeclaracionIsaiClient proxy = new ServiceDeclaracionIsai.DeclaracionIsaiClient())
                {
                    DseCatalogo.FEXNOT_REDUCCIONESDataTable reduccionDT = proxy.ObtenerReduccionesPorId(value);
                    CargarControl(reduccionDT[0].DESCRIPCION, reduccionDT[0].ARTICULO.ToString());
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
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el año indicado para mostrar las exenciones correspondientes
    /// </summary>
    public int Anio
    {
        get
        {
            return modalBuscarReduccionesid.Anio;
        }
        set
        {
            modalBuscarReduccionesid.Anio = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene la visibilidad del botón de buscar reducciones
    /// </summary>
    public bool Editable
    {
        get
        {
            return btnBuscarReducciones.Visible;
        }

        set
        {
            btnBuscarReducciones.Visible = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    /// <summary>
    /// Maneja el evento click del control btnBuscarReducciones
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscarReducciones_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(ViewState["IdReducccion"].ToString()))
            {
                modalBuscarReduccionesid.IdReduccion = ViewState["IdReducccion"].ToInt();
                modalBuscarReduccionesid.Buscar();
            }
            else
            {
                modalBuscarReduccionesid.IdReduccion = (int?)null;
            }
            extenderModalPopupBtnBuscarReducciones.Show();
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
    /// Maneja el evento Click del control modalBuscadorReducciones
    /// Obtiene el identificador de la reducción seleccionada y carga su información en la modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void modalBuscadorReducciones_EventClick(object sender, CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                extenderModalPopupBtnBuscarReducciones.Hide();
                if (modalBuscarReduccionesid.IdReduccion.HasValue)
                {
                    this.ViewState["IdReducccion"] = modalBuscarReduccionesid.IdReduccion.Value;
                    CargarControl(modalBuscarReduccionesid.Descripcion, modalBuscarReduccionesid.Articulo);
                }
            }
            else
            {
                extenderModalPopupBtnBuscarReducciones.Show();
            }
            updatePanelReduccion.Update();
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
    /// Método que carga los controles: lblArticulo, lblDescripcion y trDescripcion
    /// </summary>
    /// <param name="descripcion"></param>
    /// <param name="articulo"></param>
    private void CargarControl(string descripcion, string articulo)
    {
        lblArticulo.Text = articulo;
        lblDescripcion.InnerText = descripcion;
        trDescripcion.Visible = true;
    }



    /// <summary>
    /// Método que inicializa los controles:lblArticulo, lblDescripcion y trDescripcion
    /// </summary>
    public void ResetearControl()
    {
        try
        {
            this.ViewState["IdReducccion"] = Constantes.UI_DDL_VALUE_NO_SELECT;
            lblArticulo.Text = Constantes.TEXT_SIN_REDUCCION;
            lblDescripcion.InnerText = string.Empty;
            trDescripcion.Visible = false;
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
    /// Pre-renderizado de la página.
    /// </summary>
    /// <param name="sender">.</param>
    /// <param name="e">.</param>
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
    /// Método que muestra la pantalla modal de error
    /// </summary>
    /// <param name="mensaje">Texto que aparecera en la pantalla</param>
    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }


}
