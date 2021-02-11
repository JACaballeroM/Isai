using System;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Web.UI;
using ServiceCatastral;
using ServiceDeclaracionIsai;

/// <summary>
/// Clase que gestiona la pantalla modal para la consulta del valor catastral
/// </summary>
public partial class UserControls_ModaConsultaValorCatl : System.Web.UI.UserControl
{

    #region Delegado y Evento
    public delegate void EventClickHandler(object sender, CancelEventArgs e);

    public event EventClickHandler EventClick;

    private void LaunchEventClick(Boolean cancel)
    {
        if (EventClick != null)
            EventClick(this, new CancelEventArgs(cancel));
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            rfvF1.Enabled = true;
            rfvF2.Enabled = false;
            txtFecha.Enabled = true;
            txtFecha_CalendarExtender.Enabled = true;
            btnFecha.Enabled = true;
            lblFechaActual.Text = System.DateTime.Now.Date.ToShortDateString();
            lblFechaActual.Visible = false;

        }
    }

    #region Métodos privados de la página

    /// <summary>
    /// Resetea los los controles de la pantalla modal
    /// </summary>
    private void resetCampos()
    {
        txtRegion.Text = string.Empty;
        txtLote.Text = string.Empty;
        txtManzana.Text = string.Empty;
        txtUnidad.Text = string.Empty;
        txtFecha.Text = string.Empty;
        txtFecha.Enabled = true;
        btnFecha.Enabled = true;
        txtFecha2.Enabled = false;
        btnFecha2.Enabled = false;
        txtFecha2.Text = string.Empty;
        rbFecha.Checked = true;
        rbNormal.Checked = false;
        rbAnticipada.Checked = false;
        rfvF1.Enabled = true;
        rfvF2.Enabled = false;
        lblFechaActual.Visible = false;
        lblFechaActual.Text = System.DateTime.Now.Date.ToShortDateString();
        txtFecha_CalendarExtender.Enabled = true;
        CalendarExtender1.Enabled = false;
        uppCuenta.Update();
        uppDatos.Update();

    }

    /// <summary>
    /// Si existe un valor catastral para la cuenta y fecha que se pasan por parámetro devuelve true.
    /// Si no existe devuelve false
    /// </summary>
    /// <param name="region"></param>
    /// <param name="manzana"></param>
    /// <param name="lote"></param>
    /// <param name="unidadprivativa"></param>
    /// <param name="fecha"></param>
    /// <param name="ErrorMessage"></param>
    /// <returns></returns>
    private bool ComprobarExisteCuentaCatastral()
    {

        using (ConsultaCatastralServiceClient proxyCatastral = new ConsultaCatastralServiceClient())
        {
            DseInmueble inmuebleDS = proxyCatastral.GetInmuebleConTitularesByClave(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidad.Text, false);
            if (inmuebleDS != null && inmuebleDS.Inmueble != null && inmuebleDS.Inmueble.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

    }


    /// <summary>
    /// Obtiene el valor castastral de los datos introducidos en el formulario
    /// </summary>
    /// <param name="ErrorMessage">En caso de error devuelve el motivo</param>
    /// <returns>Valor catastral</returns>
    private decimal? ObtenerValorCatastral(out string ErrorMessage)
    {
        decimal? idInmueble = null;

        using (ConsultaCatastralServiceClient proxyCatastral = new ConsultaCatastralServiceClient())
        {
            DseInmueble inmuebleDS = proxyCatastral.GetInmuebleConTitularesByClave(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidad.Text, false);
            if (inmuebleDS != null && inmuebleDS.Inmueble != null && inmuebleDS.Inmueble.Count > 0)
            {
                idInmueble = inmuebleDS.Inmueble[0].IDINMUEBLE;
            }
        }
        using (ServiceFiscalEmision.EmisionClient proxyFiscal = new ServiceFiscalEmision.EmisionClient())
        {
            ErrorMessage = "";
            string ErrorMessageRes = "";
            ServiceFiscalEmision.DseEmision emisionDS = null;

            if (rbFecha.Checked)
            {
                emisionDS = proxyFiscal.ObtenerBoleta(Convert.ToInt32(idInmueble), Convert.ToDateTime(txtFecha.Text).Year, proxyFiscal.ObtenerPeriodoActual(Convert.ToDateTime(txtFecha.Text)));
                ErrorMessageRes = Constantes.MSJ_ERROR_NOEXISTE_VALCAT_FECHA;

            }
            else if (rbNormal.Checked)
            {
                emisionDS = proxyFiscal.ObtenerBoleta(Convert.ToInt32(idInmueble), Convert.ToDateTime(txtFecha2.Text).Year, proxyFiscal.ObtenerPeriodoActual(Convert.ToDateTime(txtFecha2.Text)));
                ErrorMessageRes = Constantes.MSJ_ERROR_NOEXISTE_VALCAT_FECHA;

            }
            else if (rbAnticipada.Checked)
            {
                emisionDS = proxyFiscal.ObtenerBoleta(Convert.ToInt32(idInmueble), DateTime.Today.Year, proxyFiscal.ObtenerPeriodoActual(DateTime.Today));
                ErrorMessageRes = Constantes.MSJ_ERROR_NOEXISTE_VALCAT_HOY;
            }


            if (emisionDS != null && emisionDS.FIS_EMISIONRESULTADO != null && emisionDS.FIS_EMISIONRESULTADO.Any())
            {
                if (!emisionDS.FIS_EMISIONRESULTADO[0].IsVALORCATASTRALNull())
                {
                    return emisionDS.FIS_EMISIONRESULTADO[0].VALORCATASTRAL;
                }
                else
                {
                    ErrorMessage = ErrorMessageRes;
                }
            }
            else
            {
                ErrorMessage = ErrorMessageRes;
            }
            return null;
        }

    }

    #endregion

    #region Eventos

    /// <summary>
    /// Evento del aspa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCerrar_Click(object sender, ImageClickEventArgs e)
    {
        LaunchEventClick(false);
    }


    /// <summary>
    /// Evento del botón 'Cerrar'
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        resetCampos();
        LaunchEventClick(false);
    }


    /// <summary>
    /// Mostramos un mensaje informativode proceso erroeneo
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e"></param>
    protected void ModalValorCat_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        extenderPnlValorCat.Hide();
    }


    /// <summary>
    /// Maneja el evento Click del control ModalInfoErrorCuenta
    /// Se redirige a la página origen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoValCat_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            extenderPnlValorCat.Hide();
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
    /// Obtiene el valor catastral de los datos introducidos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            string ErrorMessage = null;
            decimal? valorCatastralResult = null;
            if (ComprobarExisteCuentaCatastral())
            {

                valorCatastralResult = ObtenerValorCatastral(out ErrorMessage);
                if (valorCatastralResult.HasValue)
                {
                    pnlValorCat.Attributes["Style"] = "width: 400px;display: none";
                    modalValorCat.TextoInformacion = String.Format("<H4>VALOR CATASTRAL:  {0:C}", valorCatastralResult);
                }
                else
                {
                    pnlValorCat.Attributes["Style"] = "width: 480px;display: none";
                    modalValorCat.TextoInformacion = ErrorMessage;
                }
                extenderPnlValorCat.Show();
            }
            else
            {
                pnlValorCat.Attributes["Style"] = "width: 280px;display: none";
                modalValorCat.TextoInformacion = "La cuenta predial introducida no existe";
                extenderPnlValorCat.Show();
                uppCuenta.Update();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnValidar_Click(object sender, EventArgs e)
    {
         try
        {
        if (!ComprobarExisteCuentaCatastral())
        {
            pnlValorCat.Attributes["Style"] = "width: 280px;display: none";
            modalValorCat.TextoInformacion = "La cuenta predial introducida no existe";
            extenderPnlValorCat.Show();
        }

        UpdatePanelBotones.Update();
        uppDatos.Update();
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
    /// Configura los controles dependiendo del radio button seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbl_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
        if (rbNormal.Checked)
        {
            txtFecha2.Enabled = true;
            CalendarExtender1.Enabled = true;
            btnFecha2.Enabled = true;
            rfvF1.Enabled = false;
            rfvF2.Enabled = true;
            txtFecha.Enabled = false;
            txtFecha_CalendarExtender.Enabled = false;
            btnFecha.Enabled = false;
            lblFechaActual.Visible = false;
        }
        else if (rbAnticipada.Checked)
        {
            txtFecha2.Enabled = false;
            CalendarExtender1.Enabled = false;
            btnFecha2.Enabled = false;
            rfvF1.Enabled = false;
            rfvF2.Enabled = false;
            txtFecha.Enabled = false;
            txtFecha_CalendarExtender.Enabled = false;
            btnFecha.Enabled = false;
            lblFechaActual.Visible = true;
        }
        else if (rbFecha.Checked)
        {
            txtFecha.Enabled = true;
            txtFecha_CalendarExtender.Enabled = true;
            btnFecha.Enabled = true;
            txtFecha2.Enabled = false;
            CalendarExtender1.Enabled = false;
            btnFecha2.Enabled = false;
            rfvF1.Enabled = true;
            rfvF2.Enabled = false;
            lblFechaActual.Visible = false;

        }
        txtFecha.Text = string.Empty;
        txtFecha2.Text = string.Empty;
        uppDatos.Update();
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
    /// Muestra un mensaje de Error
    /// </summary>
    /// <param name="mensaje">Muestra el texto a mostrar</param>
    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion


}
