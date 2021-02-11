using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Web.UI;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;


/// <summary>
/// Clase que gestiona el control de usuario de exenciones
/// </summary>
public partial class UserControls_Exencion : System.Web.UI.UserControl
{
    
    /// <summary>
    /// Propiedad que almacena y obtiene el Id de la exención seleccionada en el buscador
    /// </summary>
    public int IdExencion
    {
        get
        {
            return this.ViewState["IdExencion"].ToInt();
        }
        set
        {
            try
            {
                this.ViewState["IdExencion"] = value;
                using (ServiceDeclaracionIsai.DeclaracionIsaiClient proxy = new ServiceDeclaracionIsai.DeclaracionIsaiClient())
                {
                    DseCatalogo.FEXNOT_EXENCIONESDataTable exencionDT = proxy.ObtenerExencionesPorId(value);
                    CargarControl(exencionDT[0].DESCRIPCION, exencionDT[0].ARTICULO.ToString());
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
            return ModalBuscarExencinesid.Anio;
        }
        set
        {
            ModalBuscarExencinesid.Anio = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene la visibilidad del botón de buscar exenciones
    /// </summary>
    public bool Editable
    {
        get
        {
            return btnBuscarExenciones.Visible;
        }

        set
        {
            btnBuscarExenciones.Visible = value;
        }
    }



    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">.</param>
    /// <param name="e">.</param>
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    /// <summary>
    /// Lanza la modal que permite realizar la búsqueda de exenciones
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscarExenciones_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(ViewState["IdExencion"].ToString()))
            {
                ModalBuscarExencinesid.IdExencion= ViewState["IdExencion"].ToInt();
                ModalBuscarExencinesid.Buscar();
            }
            else
            {
                ModalBuscarExencinesid.IdExencion = (int?)null;
            }
            extenderModalPopupBtnBuscarExenciones.Show();
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


    /// <summary>
    /// Maneja el evento Click del control modalBuscadorExenciones
    /// Obtiene el identificador de la exención seleccionada y carga su información en la modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void modalBuscadorExenciones_EventClick(object sender, CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                extenderModalPopupBtnBuscarExenciones.Hide();
                if (ModalBuscarExencinesid.IdExencion.HasValue)
                {
                    this.ViewState["IdExencion"] = ModalBuscarExencinesid.IdExencion;
                    CargarControl(ModalBuscarExencinesid.Descripcion, ModalBuscarExencinesid.Articulo);
                }
            }
            else
            {
                extenderModalPopupBtnBuscarExenciones.Show();
            }
            updatePanelExencion.Update();
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
            this.ViewState["IdExencion"] = Constantes.UI_DDL_VALUE_NO_SELECT;
            lblArticulo.Text = Constantes.TEXT_SIN_EXENCION;
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
}
