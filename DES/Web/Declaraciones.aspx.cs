using System;
using System.Linq;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;
using SIGAPred.Common.Seguridad;

/// <summary>
/// Clase encargada de gestionar el formulario de las declaraciones
/// </summary>
public partial class Declaraciones : PageBaseISAI
{
    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">.</param>
    /// <param name="e">.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
           
                HiddenIdPersonaToken.Value = Usuarios.IdPersona();
                HiddenIdAvaluo.Value = Utilidades.GetParametroUrl(Constantes.PAR_IDAVALUO);
                CargarPagina();
                CargarDatosAvaluo();
                CargarSort();

                string filtro = Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO);

                if (!string.IsNullOrEmpty(filtro)) 
                {
                    FBusqueda.RellenarObjetoFiltro(filtro);
                }

                SortDirectionP = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR);
                SortExpression = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP);
                ShowBtnAdd();

            }
        }
        catch(UserFailedException err)
        {
            AppLog.Log(err);
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : err : " + err.Message + "\n\r");
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : err : " + err.StackTrace + "\n\r");
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + err.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            AppLog.Log(cex);
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : cex : " + cex.Message + "\n\r");
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : cex : " + cex.StackTrace + "\n\r");

            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            AppLog.Log(ciex);
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ciex : " + ciex.Message + "\n\r");
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ciex : " + ciex.StackTrace + "\n\r");

            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            AppLog.Log(ex);
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ex : " + ex.Message + "\n\r");
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ex : " + ex.StackTrace + "\n\r");

            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }



    #region Metodos de Popup


    /// <summary>
    /// Maneja el evento click del control btnModalConfirmarNueva
    /// Redirige a la pantalla de declaración 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModalConfirmarNueva_Click(object sender, EventArgs e)
    {
        try
        {
            if (RBAnticipada.Checked)
            {
                RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
                RedirectUtil.AddParameter(Constantes.PAR_TIPODECLARACION, Constantes.PAR_VAL_TIPODECLARACION_ANTI);
            }
            else
            {
                RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
                RedirectUtil.AddParameter(Constantes.PAR_TIPODECLARACION, Constantes.PAR_VAL_TIPODECLARACION_NOR);
            }
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_INS);
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, HiddenIdAvaluo.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP2, SortExpression2);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR2, SortDirectionP2);
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
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
    /// Maneja el evento ConfirmClick del control ModalConfirmacionEliminar
    /// Confirma que se quiere eliminar la declaración seleccionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalConfirmarcionEliminar_OnConfirmClick(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (!e.Cancel)
            {
                ClienteDeclaracionIsai.EliminarDeclaracion(HiddenIdDeclaracion.Value.ToInt());
                CargarPagina();
                UpdatePanelGrid.Update();
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

    #endregion

    #region Métodos de la página

    /// <summary>
    /// Método que obtiene los datos del avalúo en función del id de avalúo alamcenado en HiddenIdAvaluo
    /// </summary>
    private void CargarDatosAvaluo()
    {
        try
        {
            int idAvaluo;
            //Cargaremos los valores del Avaluo llamando al servicio WCF Avaluos a través del WCF de DeclaracionIsai
            if (!string.IsNullOrEmpty(HiddenIdAvaluo.Value) && Int32.TryParse(HiddenIdAvaluo.Value, out idAvaluo))
            {
                ServiceAvaluos.DseAvaluoConsulta dse = ClienteAvaluo.ObtenerAvaluo(idAvaluo);
                ServiceAvaluos.DseAvaluoMantInf direccion = ClienteAvaluo.GetAvaluoAntecedentes(idAvaluo);
                
                if (direccion.FEXAVA_AVALUO.Any())
                {
                    lblCalleDato.Text = !string.IsNullOrEmpty(direccion.FEXAVA_AVALUO[0].CALLE) ? direccion.FEXAVA_AVALUO[0].CALLE : "-";
                    lblNoExtDato.Text = !string.IsNullOrEmpty(direccion.FEXAVA_AVALUO[0].EXT) ? direccion.FEXAVA_AVALUO[0].EXT : "-";
                    lblNoIntDato.Text = !string.IsNullOrEmpty(direccion.FEXAVA_AVALUO[0].INT) ? direccion.FEXAVA_AVALUO[0].INT : "-";
                    lblColoniaDato.Text = !string.IsNullOrEmpty(direccion.FEXAVA_AVALUO[0].COLONIA) ? direccion.FEXAVA_AVALUO[0].COLONIA : "-";
                    lblDelegacionDato.Text = !string.IsNullOrEmpty(direccion.FEXAVA_AVALUO[0].DELEGACION) ? direccion.FEXAVA_AVALUO[0].DELEGACION : "-";
                    lblCPDato.Text = !string.IsNullOrEmpty(direccion.FEXAVA_AVALUO[0].CP) ? direccion.FEXAVA_AVALUO[0].CP : "-";
                }
                if (dse.FEXAVA_AVALUO_V.Any())
                {
                    if (!dse.FEXAVA_AVALUO_V[0].IsVIGENTENull())
                        lblVigenteDato.Text = dse.FEXAVA_AVALUO_V[0].VIGENTE.ToUpper() == Constantes.PAR_VAL_TRUE ? "Sí" : "No";
                    else
                    {
                        lblVigenteDato.Text = string.Empty;
                    }
                    if (!dse.FEXAVA_AVALUO_V[0].IsFECHA_DOCDIGITALNull())
                    {
                        lblFechaDato.Text = dse.FEXAVA_AVALUO_V[0].FECHA_DOCDIGITAL.ToShortDateString();
                    }
                    else
                    {
                        lblFechaDato.Text = string.Empty;
                    }
                    if (!dse.FEXAVA_AVALUO_V[0].IsCUENTACATASTRALNull())
                    {
                        lblCuentaDato.Text = dse.FEXAVA_AVALUO_V[0].CUENTACATASTRAL;
                    }
                    else
                    {
                        lblCuentaDato.Text = string.Empty;
                    }
                    if (!dse.FEXAVA_AVALUO_V[0].IsESTADONull())
                    {
                        lblEstadoDato.Text = dse.FEXAVA_AVALUO_V[0].ESTADO;
                    }
                    else
                    {
                        lblEstadoDato.Text = string.Empty;
                    }
                    lblAvaluoDato.Text = dse.FEXAVA_AVALUO_V[0].NUMEROUNICO.ToString();

                }
            }
        }
        catch (Exception e)
        {
        }
    }


    /// <summary>
    /// Método que configura los controles y carga los contenidos de la página
    /// </summary>
    private void CargarPagina()
    {
        CargarGridView();
       // if (gridViewDeclaraciones.Rows.Count == 0 || gridViewDeclaraciones.SelectedIndex == -1)
        if (gridViewDeclaraciones.Rows.Count == 0 )
        {
            //avaluo sin declaraciones
            btnModificar.Enabled = false;
            btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
            btnEliminar.Enabled = false;
            btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
            btnVer.Enabled = false;
            btnVer.ImageUrl = Constantes.IMG_ZOOM_DISABLED;
            btnGenerarJustificantes.Enabled = false;
            btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC_DISABLED;
            btnComplementaria.Enabled = false;
            btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD_DISABLED;
            btnGenerarAcuse.Enabled = false;
            btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
        }
        else
        {
            if (!IsPostBack || (IsPostBack && gridViewDeclaraciones.SelectedIndex == gridViewDeclaraciones.Rows.Count))
            {
                gridViewDeclaraciones.SelectedIndex = 0;
            }
            if(gridViewDeclaraciones.SelectedIndex>=0)
                HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();

            Restricciones();
        }

    }


    /// <summary>
    /// Método que configura los controles dependiendo de estado de la declaración seleccionada en el gridview y de los permisos del usuario
    /// </summary>
    private void Restricciones()
    {
        //Se el usuario es de tipo SERVREVISORISAI el botón añadir no debe estar habilitado y si es de tipo SERVEXTPARAISAI sí debe habilitarse.
        //estado Borrador
        if (gridViewDeclaraciones.SelectedIndex > -1)
        {
            if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Borrador.ToInt())
            {
                btnModificar.Enabled = true;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA;
                btnEliminar.Enabled = true;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
                btnGenerarJustificantes.Enabled = true;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnComplementaria.Enabled = false;
                btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD_DISABLED;
                btnGenerarAcuse.Enabled = false;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
            }
            //estado Pendiente de enviar
            else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Pendiente.ToInt())
            {
                btnModificar.Enabled = false;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.Enabled = false;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
              
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnComplementaria.Enabled = false;
                btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD_DISABLED;
                btnGenerarAcuse.Enabled = false;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
                         
                btnGenerarJustificantes.Enabled = true;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
               
            }
            else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.PendienteDocumentacion.ToInt() ||
                     gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Inconsistente.ToInt())
            {
                btnModificar.Enabled = false;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.Enabled = false;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
                btnGenerarJustificantes.Enabled = true;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnComplementaria.Enabled = true;
                btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD;
                btnGenerarAcuse.Enabled = true;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC;
            }
            else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Aceptada.ToInt() ||
                     gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == EstadosDeclaraciones.Presentada.ToInt())
            {
                btnModificar.Enabled = false;
                btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.Enabled = false;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.Enabled = true;
                btnVer.ImageUrl = Constantes.IMG_ZOOM;
                btnGenerarJustificantes.Enabled = true;
                btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                btnComplementaria.Enabled = true;
                btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD;
                btnGenerarAcuse.Enabled = true;
                btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC;
            }
        }
        //GULE- se deshabilita para pruebas con el Superisai que tambien tiene este perfil y por tanto no activa los botones
        // JABS, Correccion paara cuando son varios perfiles
        if (btnNuevo.Visible)
        {
            if (Condiciones.Web(Constantes.FUN_EXTPARAISAI))
            {
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
            }

            else if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
            {
                btnNuevo.Enabled = false;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
            }
        }
       

    }

    protected void ShowBtnAdd()
    {
        //int idAvaluo = 0;
        //if (!string.IsNullOrEmpty(HiddenIdAvaluo.Value) && Int32.TryParse(HiddenIdAvaluo.Value, out idAvaluo))
        //{
        //    ServiceAvaluos.DseAvaluoConsulta dse = ClienteAvaluo.ObtenerAvaluo(idAvaluo);


        //    if (dse != null && dse.FEXAVA_AVALUO_V != null && dse.FEXAVA_AVALUO_V.Any())
        //    {
        //        if (!dse.FEXAVA_AVALUO_V[0].IsFECHA_PRESENTACIONNull())
        //        {
                    bool vigente = btnNuevo.Visible = lblVigenteDato.Text.ToUpper().Contains("S");
                    //btnNuevo.ImageUrl = vigente ? Constantes.IMG_ANADIR : Constantes.IMG_ANADIR_DISABLED;
        //        }
        //    }
        //}
    }

    private bool esVigente(DateTime fecha)
    {
        return DateTime.Now < fecha.AddMonths(6);
    }

    /// <summary>
    /// Método que obtiene de los parámetros de la URL la dirección y expresión de ordenación 
    /// para luego ordenar el grid view.
    /// </summary>
    private void CargarSort()
    {
        SortDirection direction;
        SortExpression2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP2);
        SortDirectionP2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR2);

        switch (SortDirectionP2)
        {
            case "Ascending":
                direction = SortDirection.Ascending;
                break;
            case "Descending":
                direction = SortDirection.Descending;
                break;
            default:
                direction = SortDirection.Ascending;
                break;
        }

        if (string.IsNullOrEmpty(SortExpression2))
        {
            SortExpression2 = Constantes.GRIDCOL_NUMPRESENTACIONDEC;
        }

        gridViewDeclaraciones.Sort(SortExpression2, direction);

    }


    /// <summary>
    /// Maneja el evento click del control btnGenerarJustificante
    /// Muestra el justificante de la declaración seleccionada en el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGenerarJustificantes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_JUSTIFICANTE;
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, HiddenIdAvaluo.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP2, SortExpression2);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR2, SortDirectionP2);
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
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
    /// Maneja el evento click del control btnGenerarAcuse
    /// Muestra el acuse de la declaración seleccionada en el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGenerarAcuse_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_ACUSE;
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, HiddenIdAvaluo.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP2, SortExpression2);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR2, SortDirectionP2);
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
            //Se el usuario es de tipo SERVREVISORISAI el botón añadir no debe estar habilitado y si es de tipo SERVEXTPARAISAI sí debe habilitarse.
            //estado Borrador
            if (gridViewDeclaraciones.SelectedIndex > -1)
            {
                if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == 0)
                {
                    btnModificar.Enabled = true;
                    btnModificar.ImageUrl = Constantes.IMG_MODIFICA;
                    btnEliminar.Enabled = true;
                    btnEliminar.ImageUrl = Constantes.IMG_ELIMINA;
                    btnVer.Enabled = true;
                    btnVer.ImageUrl = Constantes.IMG_ZOOM;
                    btnGenerarJustificantes.Enabled = true;
                    btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                    btnNuevo.Enabled = true;
                    btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                    btnComplementaria.Enabled = false;
                    btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD_DISABLED;
                    btnGenerarAcuse.Enabled = false;
                    btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
                }
                //estado Pendiente de enviar
                else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == 1)
                {
                    btnModificar.Enabled = false;
                    btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                    btnEliminar.Enabled = false;
                    btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                    btnVer.Enabled = true;
                    btnVer.ImageUrl = Constantes.IMG_ZOOM;
                    btnGenerarJustificantes.Enabled = true;
                    btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                    btnNuevo.Enabled = true;
                    btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                    btnComplementaria.Enabled = false;
                    btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD_DISABLED;
                    btnGenerarAcuse.Enabled = false;
                    btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
                }
                //estado Rechazada  (Pdte documentacion e inconsistente)
                else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == 3 ||
                         gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == 5)
                {
                    btnModificar.Enabled = false;
                    btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                    btnEliminar.Enabled = false;
                    btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                    btnVer.Enabled = true;
                    btnVer.ImageUrl = Constantes.IMG_ZOOM;
                    btnGenerarJustificantes.Enabled = true;
                    btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                    btnNuevo.Enabled = true;
                    btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                    btnComplementaria.Enabled = true;
                    btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD;
                    btnGenerarAcuse.Enabled = true;
                    btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC;
                }
                //estado Aceptada (4) o Presentada(2)
                else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == 4 ||
                         gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == 2)
                {
                    btnModificar.Enabled = false;
                    btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                    btnEliminar.Enabled = false;
                    btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                    btnVer.Enabled = true;
                    btnVer.ImageUrl = Constantes.IMG_ZOOM;
                    btnGenerarJustificantes.Enabled = true;
                    btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                    btnNuevo.Enabled = true;
                    btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                    btnComplementaria.Enabled = true;
                    btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD;
                    btnGenerarAcuse.Enabled = true;
                    btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC;
                }
                else if (gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[1].ToInt() == 1)
                {
                    btnModificar.Enabled = true;
                    btnModificar.ImageUrl = Constantes.IMG_MODIFICA;
                    btnEliminar.Enabled = false;
                    btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                    btnVer.Enabled = true;
                    btnVer.ImageUrl = Constantes.IMG_ZOOM;
                    btnGenerarJustificantes.Enabled = true;
                    btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC;
                    btnNuevo.Enabled = true;
                    btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
                    btnComplementaria.Enabled = true;
                    btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD;
                    btnGenerarAcuse.Enabled = false;
                    btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
                }
            }
            //GULE- Se deshabilita para pruebas
            // JABS, Correccion paara cuando son varios perfiles
            if (Condiciones.Web(Constantes.FUN_EXTPARAISAI))
            {
                btnNuevo.Enabled = true;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR;
            }

            else if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
            {
                btnNuevo.Enabled = false;
                btnNuevo.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
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
    /// Maneja el evento click del control buttonVolver
    /// Redirige a la bandeja de entrada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonVolver_Click(object sender, EventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_BANDEJAENTRADA;
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
            SortDirection direction;
            SortExpression2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP2);
            SortDirectionP2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR2);

            switch (SortDirectionP2)
            {
                case "Ascending":
                    direction = SortDirection.Ascending;
                    break;
                case "Descending":
                    direction = SortDirection.Descending;
                    break;
                default:
                    direction = SortDirection.Ascending;
                    break;
            }

            if (string.IsNullOrEmpty(SortExpression2))
            {
                SortExpression2 = Constantes.GRIDCOL_NUMPRESENTACIONDEC;
            }

            gridViewDeclaraciones.Sort(SortExpression2, direction);
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

    #region Metodos link


    /// <summary>
    /// Maneja el evento click del control btnModificar
    /// Redirige a la pantalla de declaración para editarla.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_MOD);
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP2, SortExpression2);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR2, SortDirectionP2);
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
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
    /// Maneja el evento click del control btnVer
    /// Redirige a la pantalla de declaración para consulta.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVer_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_VER);
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP2, SortExpression2);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR2, SortDirectionP2);
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
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
    /// Maneja el evento click del control btnComplementaria
    /// Redirige a la pantalla de declaración para crear una declaración complementaria de la seleccionada en el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnComplementaria_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
            RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_INS);
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, HiddenIdAvaluo.Value);
            RedirectUtil.AddParameter(Constantes.PAR_TIPODECLARACION, Constantes.PAR_VAL_TIPODECLARACION_COMPLE);
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACIONPADRE, HiddenIdDeclaracion.Value);
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP2, SortExpression2);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR2, SortDirectionP2);
            RedirectUtil.Encrypted = true;
            RedirectUtil.Go();
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

    #region Métodos del GridView


    /// <summary>
    /// Método que carga el grid view dependiendo de los permisos del usuario
    /// </summary>
    protected void CargarGridView()
    {
        try
        {
            if (Condiciones.Web(Constantes.FUN_EXTPARAISAI))
            {
                gridViewDeclaraciones.DataSourceID = odsAvaluo.ID;
            }

            else if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
            {
                gridViewDeclaraciones.DataSourceID = odsAvaluoSF.ID;
            }

            if (gridViewDeclaraciones.Rows.Count > 0)
            {
                gridViewDeclaraciones.SelectedIndex = 0;
                Restricciones();
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
    /// Maneja el evento RowDataBound del control gridViewDeclaraciones
    /// Se produce cuando una fila de datos se enlaza a los datos de un control GridView
    /// Chekea o no los check de los que dispone el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewDeclaraciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CheckSiscor = (CheckBox)e.Row.FindControl("checkboxSISCOR");
                CheckSiscor.Checked = e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_DECLARACIONRow>().CODESTADOPAGO == EstadosPago.RecibidoSISCOR.ToDecimal()  ? true : false;
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
    /// Maneja el evento SelectedIndexChanged del control gridViewDeclaraciones 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewDeclaraciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
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
    /// Maneja el evento PageIndexChanging del gridViewDeclaraciones
    /// Al cambiar de página del grid view se vuelve a cargar con los registros solicitados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewDeclaraciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridViewDeclaraciones.PageIndex = e.NewPageIndex;
            CargarGridView();
            gridViewDeclaraciones.SelectedIndex = 0;
            HiddenIdDeclaracion.Value = gridViewDeclaraciones.DataKeys[gridViewDeclaraciones.SelectedIndex].Values[0].ToString();
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
    /// Maneja el evento Sorting del control gridviewdeclaraciones
    /// Al reordenar se establecen los botones a la situación inicial
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewDeclaraciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortExpression2 = e.SortExpression;
            SortDirectionP2 = e.SortDirection.ToString();
            gridViewDeclaraciones.SelectedIndex = -1;
            btnModificar.Enabled = false;
            btnModificar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
            btnEliminar.Enabled = false;
            btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
            btnVer.Enabled = false;
            btnVer.ImageUrl = Constantes.IMG_ZOOM_DISABLED;
            btnGenerarJustificantes.Enabled = false;
            btnGenerarJustificantes.ImageUrl = Constantes.IMG_DOC_DISABLED;
            btnComplementaria.Enabled = false;
            btnComplementaria.ImageUrl = Constantes.IMG_CLIPBOARD_DISABLED;
            btnGenerarAcuse.Enabled = false;
            btnGenerarAcuse.ImageUrl = Constantes.IMG_DOC_DISABLED;
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
    protected void gridViewDeclaraciones_PreRender(object sender, EventArgs e)
    {
        Restricciones();
    }

    #endregion

    #region Excepciones

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
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : Page_Prerender ex : " + ex.Message + "\n\r");
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : Page_Prerender ex : " + ex.StackTrace + "\n\r");

            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    /// <summary>
    /// Mostrar mensaje información excepcion.
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
