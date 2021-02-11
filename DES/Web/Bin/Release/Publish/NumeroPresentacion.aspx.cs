using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceAvaluos;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;

/// <summary>
/// Clase encargada de gestionar el formulario para la generación del número de presentación de una declaración
/// </summary>
public partial class NumeroPresentacion : PageBaseISAI
{
    /// <summary>
    /// Obtiene y carga los controles del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                CargarComboTipoDeclaracion();
                gridViewAvaluos.DataBind();
                UpdatePanelGrid.Update();
                txtUnidadPrivativa.Attributes.Add("onblur", "javascript:if (this.value.length!=0){rellenar(this, this.value, 3);}");
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

    #region Métodos de la página

    /// <summary>
    /// Método que registra una declaración en estado borrador de tipo normal o anticipada y con el número de presentación obtenido
    /// </summary>
    /// <param name="numPresentacion"></param>
    private void RegistrarDatosNormalAnticipada()
    {
        try
        {
            //guardo los datos de la tabla declaracion con la minima informacion necesaria para ser registrada en el sistema
            DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = new DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable();
            DseDeclaracionIsai.FEXNOT_DECLARACIONRow declaracionDR = declaracionDT.NewFEXNOT_DECLARACIONRow();
            declaracionDR.REGION = Region;
            declaracionDR.MANZANA = Manzana;
            declaracionDR.LOTE = Lote;
            declaracionDR.UNIDADPRIVATIVA = UnidadPrivativa;
            declaracionDR.ESHABITACIONAL = Constantes.PAR_VAL_TRUE;
            declaracionDR.ENPAPEL = Constantes.PAR_VAL_TRUE;
            declaracionDR.IDUSUARIO = Convert.ToDecimal(Usuarios.IdUsuario());
            declaracionDR.GENERADA = Constantes.PAR_VAL_FALSE;
            declaracionDR.CODESTADODECLARACION = Convert.ToDecimal(Constantes.PAR_VAL_ESTADODECLARACION_BORR);
            declaracionDR.IDAVALUO = Convert.ToDecimal(IdAvaluo);
            declaracionDR.CODTIPODECLARACION = Convert.ToDecimal(ddlTipoDeclaracion.SelectedValue);
            declaracionDR.CODESTADOPAGO = EstadosPago.SinPago.ToDecimal();
            declaracionDR.CODPROCEDENCIA = 0;
            declaracionDR.CODACTOJUR = 1;
            declaracionDR.IDPERSONA = Convert.ToDecimal(Usuarios.IdPersona());
            declaracionDR.ESVCATOBTENIDOFISCAL = Constantes.PAR_VAL_FALSE;
            declaracionDT.AddFEXNOT_DECLARACIONRow(declaracionDR);
            //registro la nueva declaracion ENPAPEL en el sistema en estado borrador y de tipo NORMAL o ANTICIPADA segun la seleccion
            ClienteDeclaracionIsai.RegistrarDeclaracionEnPapelNormalAnticipada(ref declaracionDT);
            DseDeclaracionIsaiMant.Merge(declaracionDT);
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
    /// Método que registra una declaración en estado borrador de tipo complementaria con el número de presentación obtenido
    /// </summary>
    /// <param name="tableDeclaracion"></param>
    protected void RegistrarDatosComplementaria( DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable tableDeclaracion)
    {
        try
        {
            base.CargarDeclaracionPadrePorIdDeclaracionPadre(tableDeclaracion[0].IDDECLARACION);
          
            DseDeclaracionIsaiMantAllToStateAdded();

            //Guardo el IDUSUARIO TOKEN
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDUSUARIO = Convert.ToDecimal(Usuarios.IdUsuario());
            //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDPERSONA = Convert.ToDecimal(Usuarios.IdPersona());
            //Guardo el valor de N en el Campo de EnPapel
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ENPAPEL = Constantes.PAR_VAL_TRUE;
            //Guardo la declaracion en estado borrador
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = Convert.ToDecimal(Constantes.PAR_VAL_ESTADODECLARACION_BORR);
            //Guardo como no generada Borrador a Pendiente
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].GENERADA = Constantes.PAR_VAL_FALSE;
            //Guardo la declaracion de tipo complementaria
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION = Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_COMPLE);
            //Guardo el numero de presentacion
            //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACION = numPresentacion;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACIONINICIAL = txtNumeroDeclaracion.Text;

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
            {
                ///el importe a pagar por defecto es el calculado en la declaracion normal
                decimal ImpuestoPorPagar = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO.ToDecimal();
                //si se realizo un pago, se le resta el importe pagado
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                {
                    decimal res = ImpuestoPorPagar - DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO;
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = res;
                    if (res > 0)
                        res = System.Math.Round(res.ToDecimal() + (0.49).ToDecimal()).ToDecimal();
                    else
                        res = 0;
                    
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO = res;
     
                }
            }
            DseDeclaracionIsaiMantDatosTipoComplementariaEstadoBorrador();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFUTNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAVIGENCIALINEACAPTURANull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHALINEACAPTURANull();
            DseDeclaracionIsai dse = DseDeclaracionIsaiMant;
            //El impuesto pagado anterior solo sirve cuando se trata de una declaracion complementaria
            ClienteDeclaracionIsai.RegistrarDeclaracionEnPapelComplementaria(ref dse);
            DseDeclaracionIsaiMant = dse;
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
    /// Método que deja la tabla del dataSet en estado borrador y con los datos de una declaracion de tipo complementaria, de esta manera se puede
    /// crear una nueva declaracion de tipo complementaria
    /// </summary>
    private void DseDeclaracionIsaiMantDatosTipoComplementariaEstadoBorrador()
    {
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAPRESENTACIONNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAPAGONull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetLINEACAPTURANull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO = EstadosPago.SinPago.ToDecimal();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetBANCONull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetSUCURSALNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHALINEACAPTURANull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetNUMPRESENTACIONNull();
    }


    /// <summary>
    /// Método que deja todas las row de todas la tablas del dataset en estado ADDED
    /// </summary>
    private void DseDeclaracionIsaiMantAllToStateAdded()
    {
            //aceptamos todos los cambios
            DseDeclaracionIsaiMant.AcceptChanges();
            //establecemos la rows en estado added
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetAdded();
            foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowP in DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Rows)
            {
                rowP.SetAdded();
            }
            foreach (DseDeclaracionIsai.FEXNOT_CONDONACIONRow rowC in DseDeclaracionIsaiMant.FEXNOT_CONDONACION.Rows)
            {
                rowC.SetAdded();
            }
    }


   
    #endregion
    #region Eventos del DropDownList

    /// <summary>
    /// Método que carga el combo con los tres tipos de declaracion (NORMAL, ANTICIPADA, COMPLEMENTARIA)
    /// </summary>
    private void CargarComboTipoDeclaracion()
    {
        try
        {
            DseCatalogo.FEXNOT_CATTIPOSDECLARACIONDataTable tableTipoDeclaracion = new DseCatalogo.FEXNOT_CATTIPOSDECLARACIONDataTable();

            foreach (DseCatalogo.FEXNOT_CATTIPOSDECLARACIONRow row in ApplicationCache.DseCatalogoISAI.FEXNOT_CATTIPOSDECLARACION.Rows)
            {
                DseCatalogo.FEXNOT_CATTIPOSDECLARACIONRow rowNew = tableTipoDeclaracion.NewFEXNOT_CATTIPOSDECLARACIONRow();

                if ((row.CODTIPODECLARACION == 0 || row.CODTIPODECLARACION == 1 || row.CODTIPODECLARACION == 2) && (row.ACTIVO.CompareTo(Constantes.PAR_VAL_TRUE) == 0))
                {
                    rowNew.CODTIPODECLARACION = row.CODTIPODECLARACION;
                    rowNew.TIPOSDECLARACION = row.TIPOSDECLARACION;
                    rowNew.ACTIVO = row.ACTIVO;
                    tableTipoDeclaracion.AddFEXNOT_CATTIPOSDECLARACIONRow(rowNew);
                }
            }
            if (tableTipoDeclaracion.Any())
            {
                this.ddlTipoDeclaracion.DataSource = tableTipoDeclaracion;
                this.ddlTipoDeclaracion.DataTextField = tableTipoDeclaracion.TIPOSDECLARACIONColumn.ToString();
                this.ddlTipoDeclaracion.DataValueField = tableTipoDeclaracion.CODTIPODECLARACIONColumn.ToString();
                this.ddlTipoDeclaracion.DataBind();
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
    /// Maneja el evento SelectedIndexChanged del control ddlTipoDeclaración
    /// Según la selección del combo cargo unos controles u otros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTipoDeclaracion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if ((ddlTipoDeclaracion.SelectedValue.CompareTo(Constantes.PAR_VAL_TIPODECLARACION_NOR) == 0) ||
               (ddlTipoDeclaracion.SelectedValue.CompareTo(Constantes.PAR_VAL_TIPODECLARACION_ANTI) == 0))
            {
                txtRegion.Enabled = true;
                txtManzana.Enabled = true;
                txtLote.Enabled = true;
                txtUnidadPrivativa.Enabled = true;
                txtNumeroDeclaracion.Enabled = false;
                txtNumeroDeclaracion.Text = string.Empty;
                txtNumRegSoci.Text = string.Empty;
                rfvRegion.Enabled = true;
                txtNumRegSoci.Enabled = true;
                btnPeritos.Enabled = true;
                btnPeritos.ImageUrl = Constantes.IMG_USER;
                rfvManzana.Enabled = true;
                rfvNumeroDeclaracion.Enabled = false;
                revNumPres.Enabled = false;
                btnBuscar.ImageUrl = Constantes.IMG_CONSULTA;
                btnBuscar.Enabled = true;
                btnGenerar.Enabled = false;
                UpdatePanelNumPresentacion.Update();
            }
            else if (ddlTipoDeclaracion.SelectedValue.CompareTo(Constantes.PAR_VAL_TIPODECLARACION_COMPLE) == 0)
            {
                txtRegion.Enabled = false;
                txtManzana.Enabled = false;
                txtLote.Enabled = false;
                txtUnidadPrivativa.Enabled = false;
                txtNumeroDeclaracion.Enabled = true;
                txtNumeroDeclaracion.Text = string.Empty;
                txtRegion.Text = string.Empty;
                txtManzana.Text = string.Empty;
                txtLote.Text = string.Empty;
                txtUnidadPrivativa.Text = string.Empty;
                txtCuenta.Text = string.Empty;
                txtNumRegSoci.Text = string.Empty;
                rfvRegion.Enabled = false;
                rfvManzana.Enabled = false;
                txtNumRegSoci.Enabled = false;
                btnPeritos.Enabled = false;
                btnPeritos.ImageUrl = Constantes.IMG_USER_DISABLED;
                rfvNumeroDeclaracion.Enabled = true;
                revNumPres.Enabled = true;
                btnBuscar.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
                btnBuscar.Enabled = false;
                btnGenerar.Enabled = true;

                UpdatePanelNumPresentacion.Update();
               
                gridViewAvaluos.DataBind();
                gridViewAvaluos.SelectedIndex = -1;
               
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
    #region Eventos del gridView

    /// <summary>
    /// Maneja el evento SelectedIndexChanged del control gridViewAvaluos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewAvaluos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            IdAvaluo = gridViewAvaluos.SelectedDataKey["IDAVALUO"].ToString();
            Region = gridViewAvaluos.SelectedDataKey["REGION"].ToString();
            Manzana = gridViewAvaluos.SelectedDataKey["MANZANA"].ToString();
            Lote = gridViewAvaluos.SelectedDataKey["LOTE"].ToString();
            UnidadPrivativa = gridViewAvaluos.SelectedDataKey["UNIDADPRIVATIVA"].ToString();
            btnGenerar.Enabled = true;
            UpdatePanelNumPresentacion.Update();
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
    /// Método que recarga el gridview de Avaluos dependiendo del los parámetros de búsqueda introducidos
    /// </summary>
    private void CargarGridView()
    {
        HiddenIdPersonaToken.Value = Usuarios.IdPersona();
        gridViewAvaluos.DataSourceID = odsPorCuentaCatastral.ID;
        gridViewAvaluos.PageIndex = 0;
        gridViewAvaluos.DataBind();
        gridViewAvaluos.SelectedIndex = -1;
        btnGenerar.Enabled = false;
        UpdatePanelGrid.Update();
    }

    /// <summary>
    /// Maneja el evento Sorting de gridViewAvaluos
    /// al ordenadr el grid se desselecciona si hubiera algún registro seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewAvaluos_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridViewAvaluos.SelectedIndex = -1;
    }

    /// <summary>
    /// Maneja el evento RowDataBound del gridViewAvaluos
    /// Cuenta el número de registros totales y muestra una etiqueta con la información
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewAvaluos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                lblCount.Text = string.Empty;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == 0)
            {
                DataRowView t = (DataRowView)e.Row.DataItem;
                lblCount.Text = String.Format("Se ha encontrado un total de {0} avalúo(s)", t["ROWS_TOTAL"]);
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
    #region Eventos de los botones

    /// <summary>
    /// Maneja el evento click del control btnGenerar
    /// Obtiene el número de presentación y con el crea una nueva declaración
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        try
        {
            string numPresentacion = string.Empty;
            string mensajeInfo = string.Empty;
            //compruebo si el tipo de declaración seleccionada es de tipo complementaria 
            if (ddlTipoDeclaracion.SelectedValue.CompareTo(Constantes.PAR_VAL_TIPODECLARACION_COMPLE) == 0)
            {
                //valido si existe una declaracion con ese numero de presentacion
                int a=0;
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable tableDeclaracion = ClienteDeclaracionIsai.ObtenerDeclaracionesPorNumPresentacionCodEstadoDeclaracion(
                                                                                                        Usuarios.IdPersona().To<Int32>(),
                                                                                                        txtNumeroDeclaracion.Text,
                                                                                                        -1,10,1, ref a,"iddeclaracion");
                //si existe creo una declaracion complementaria a dicha declaracion presentada
                if (tableDeclaracion.Any())
                {
                    mensajeInfo += "<CENTER> Nº de Presentación: <CENTER><H1><B>";
                    //obtengo el numero de presentacion de la nueva declaracion complementaria que se presentara ENPAPEL
                    //numPresentacion += "I-" + System.DateTime.Now.Year + "-" + ClienteDeclaracionIsai.ObtenerNumeroPresentacion().ToString();
 
                    //registro dicha declaracion complementaria en el sistema
                    RegistrarDatosComplementaria(tableDeclaracion);

                    mensajeInfo += DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACION;

                    //muestro el numero de presentacion en un modalPopup
                    MensajeInformativoModal(mensajeInfo);
                }
                else
                {
                    //mensaje informativo diciendo que no existe dicha declaracion con ese numero de presentacion
                    mensajeInfo = "<CENTER>No existe dicha declaración <BR><CENTER> con ese número de declaración";
                    MensajeInformativoModal(mensajeInfo);
                }
            }
            else
            {
                //si no se ha seleccionado un avaluo no se puede obtener el nuevo numero de presentacion de la declaracion
                if (string.IsNullOrEmpty(IdAvaluo))
                {
                    //mensaje informativo 
                    mensajeInfo = "Es obligatorio seleccionar un avalúo.";
                    MensajeInformativoModal(mensajeInfo);
                }
                else
                {
                    mensajeInfo += "<CENTER> Nº de Presentación:<CENTER><H1><B>"; 
                    //obtengo el numero de presentacion de la nueva declaracion anticipada o normal que se presentara ENPAPEL
                    //numPresentacion += "I-" + System.DateTime.Now.Year + "-" + ClienteDeclaracionIsai.ObtenerNumeroPresentacion().ToString();
               
                    RegistrarDatosNormalAnticipada();

                    mensajeInfo += DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACION;
                    mensajeInfo += "<CENTER>";
                    //muestro el numero de presentacion en un modalPopup
                    MensajeInformativoModal(mensajeInfo);
                }
            }
            DseDeclaracionIsaiMant = null;
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
    /// Maneja el evento click del control btnBuscar
    /// Carga el gridview con los criterios de búsqueda seleccionados.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtCuenta.Text = string.Empty;
            txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;
            CargarGridView();
        }
        catch (FaultException<AvaluosException> dex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + dex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
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
    /// Maneja el evento click del control btnPeritos
    /// Muestra la pantalla de búsqueda de peritos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPeritos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnPeritos_ModalPopupExtender.Show();
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
    #region Eventos de los ModalPopup

    /// <summary>
    /// Maneja el evento click del control ModalInfoGenerar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoGenerar_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                extenderPnlInfoGenerarModal.Hide();
            }
            else
            {
                extenderPnlInfoGenerarModal.Show();
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
    /// Mñetodo que muestra una pantalla modal con un mensaje
    /// </summary>
    /// <param name="mensajeInfo">Texto a mostrar en la pantalla</param>
    private void MensajeInformativoModal(string mensajeInfo)
    {
        try
        {
            ModalInfoGenerar.TextoInformacion = mensajeInfo;
            ModalInfoGenerar.TipoMensaje = true;
            extenderPnlInfoGenerarModal.Show();
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
    /// Mostramos la exception del token 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoToken_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
                extenderPnlInfoTokenModal.Hide();
            else
                extenderPnlInfoTokenModal.Show();
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
    /// Maneja el evento ConfirmClic del user control buscarPerito
    /// Carga los datos del perito seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void buscarPerito_ConfirmClick(object sender, CancelEventArgs e)
    {
        try
        {
            if (!e.Cancel)
            {
                btnPeritos_ModalPopupExtender.Show();
            }
            else
            {
                btnPeritos_ModalPopupExtender.Hide();
                if (ModalBuscarPeritos1.Seleccionado)
                {
                    if (!string.IsNullOrEmpty(ModalBuscarPeritos1.NumeroRegistro))
                        txtNumRegSoci.Text = ModalBuscarPeritos1.NumeroRegistro.ToString();
                }
                CargarGridView();
                UpdatePanelPeritos.Update();
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
