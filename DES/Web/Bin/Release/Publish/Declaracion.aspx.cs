using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Resources;
using ServiceCatastral;
using ServiceDeclaracionIsai;
using ServiceDocumentos;
using ServiceRCON;
using SIGAPred.Common.BL;
using SIGAPred.Common.Extensions;
using SIGAPred.Common.Seguridad;
using System.Web;
using Sigapred.DatosPDF;
using MyExtentions;
using System.Collections.Generic;
using SIGAPred.Common.DigitoVerificador;


/// <summary>
/// Clase encargada de gestionar el formulario de declaración
/// </summary>
public partial class Declaracion : PageBaseISAI
{



    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <exception cref="FaultException<ConsultaCatastralInfoException>">Thrown when a fault
    /// exception< consulta catastral información exception> error condition occurs.</exception>
    /// <param name="sender">.</param>
    /// <param name="e">.</param>
    private string FechaCausacionAnt
    {
        get
        {
            return (ViewState["fechaCausacionAnt"] == null ? string.Empty : ViewState["fechaCausacionAnt"].ToString());
        }
        set
        {
            ViewState["fechaCausacionAnt"] = value;
        }
    }

    private string ActoJurAnt
    {
        get
        {
            return (ViewState["actoJurAnt"] == null ? string.Empty : ViewState["actoJurAnt"].ToString());
        }
        set
        {
            ViewState["actoJurAnt"] = value;
        }
    }

    private decimal _a
    {
        get
        {
            return txta.ToDecimalValue();
        }
        set
        {
            txta.SetValue(value);
        }
    }
    private decimal _b
    {
        get
        {
            return txtb.ToDecimalValue();
        }
        set
        {
            txtb.SetValue(value);
        }
    }
    private decimal _c
    {
        get
        {
            return txtc.ToDecimalValue();
        }
        set
        {
            txtc.SetValue(value);
        }
    }
    private decimal _d
    {
        get
        {
            return txtd.ToDecimalValue();
        }
        set
        {
            txtd.SetValue(value);
        }
    }
    private decimal _e
    {
        get
        {
            return txte.ToDecimalValue();
        }
        set
        {
            txte.SetValue(value);
        }
    }
    private decimal _f
    {
        get
        {
            return txtf.ToDecimalValue();
        }
        set
        {
            txtf.SetValue(value);
        }
    }
    private decimal _g
    {
        get
        {
            return txtg.ToDecimalValue();
        }
        set
        {
            txtg.SetValue(value);
        }
    }
    private decimal _h
    {
        get
        {
            return txth.ToDecimalValue();
        }
        set
        {
            txth.SetValue(value);
        }
    }
    private decimal _i
    {
        get
        {
            return txti.ToDecimalValue();
        }
        set
        {
            txti.SetValue(value);
        }
    }
    private decimal _j
    {
        get
        {
            return txtj.ToDecimalValue();
        }
        set
        {
            txtj.SetValue(value);
        }
    }
    private decimal _A
    {
        get
        {
            return txtA2.ToDecimalValue();
        }
        set
        {
            txtA2.SetValue(value);
        }
    }
    private decimal _B
    {
        get
        {
            return txtB2.ToDecimalValue();
        }
        set
        {
            txtB2.SetValue(value);
        }
    }
    private decimal _C
    {
        get
        {
            return txtC2.ToDecimalValue();
        }
        set
        {
            txtC2.SetValue(value);
        }
    }
    private decimal _D
    {
        get
        {
            return txtD2.ToDecimalValue();
        }
        set
        {
            txtD2.SetValue(value);
        }
    }
    private decimal _E
    {
        get
        {
            return txtE2.ToDecimalValue();
        }
        set
        {
            txtE2.SetValue(value);
        }
    }

    private static System.Web.Caching.Cache CurrentCache = HttpContext.Current.Cache;

    private static void clearCache()
    {
        try
        {
            CurrentCache.Remove("lval");
        }
        catch { }
    }

    private List<decimal> valoresCache
    {
        get
        {
            List<decimal> lista = new List<decimal>();
            try
            {
                if (CurrentCache["lval"] != null)
                {
                    lista = (List<decimal>)CurrentCache["lval"];
                }

            }
            catch
            {
            }
            return lista;
        }

        set
        {

            if (CurrentCache["lval"] == null)
            {
                CurrentCache.Insert("lval", value);
            }
            else
                CurrentCache["lval"] = value;
        }
    }

    private List<decimal> listaValoresIsr
    {
        get
        {
            return new List<decimal>(){
                _a, _b, _c, _d, _e, _f, _g, _h, _i,_j,_A,_B,_C,_D,_E
            };
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                //add por jamg
                //oculto el control el el evento load
                //23-04-2013
                //-----------------------------------------
                if (string.IsNullOrEmpty(txtFechaPago.Text))
                {
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Count > 0)
                    {
                        if (!btnObtener.Enabled || !btnObtener.Visible)
                        {
                            btnObtener.Enabled = true;
                            btnObtener.Visible = true;
                        }
                    }

                    else
                        btnObtener.Visible = false;
                }
                //-----

                ((PageBaseISAI)(this.Page)).OperacionParticipantes = string.Empty;

                string filtro = Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO);

                if (!string.IsNullOrEmpty(filtro))
                {
                    FBusqueda.RellenarObjetoFiltro(filtro);
                }
                SortDirectionP = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR);
                SortExpression = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP);
                SortExpression2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP2);
                SortDirectionP2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR2);

                //Almacenamos la url referida para realizar una redirección al terminar alguna acción
                if (Request.UrlReferrer != null &&
                    (Request.UrlReferrer.AbsolutePath.Contains(Constantes.URL_SUBISAI_DECLARACIONES.Substring(1)) ||
                     Request.UrlReferrer.AbsolutePath.Contains(Constantes.URL_SUBISAI_BANDEJAJORNADA.Substring(1)) ||
                     Request.UrlReferrer.AbsolutePath.Contains(Constantes.URL_SUBISAI_BANDEJAENTRADASF.Substring(1))))
                {
                    PaginaOrigen = Request.UrlReferrer.LocalPath;
                }

                if (PreviousPage == null)
                {
                    //obtenemos los datos pasados por GET ( solo se entra la primera vez que se carga la pagina )
                    ObtenerDatosPorGet();
                    //Validamos que si no es jornada notarial la cuenta catastral tenga inmueble
                    //Descomentar para obtener la linea en cualquier momento de la declaracion
                    //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetLINEACAPTURANull();
                    //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO = 0;
                    //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = 1;
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) != 0)
                    {
                        base.CargarAvaluo(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO);

                        DseInmueble inmuebleDS = new DseInmueble();
                        try
                        {
                            inmuebleDS = ClienteCatastral.GetInmuebleConTitularesByClave(DseAvaluo.FEXAVA_AVALUO_V[0].REGION,
                            DseAvaluo.FEXAVA_AVALUO_V[0].MANZANA,
                            DseAvaluo.FEXAVA_AVALUO_V[0].LOTE,
                            DseAvaluo.FEXAVA_AVALUO_V[0].UNIDADPRIVATIVA, false);
                        }
                        catch (FaultException<ConsultaCatastralInfoException> ex)
                        {
                            if (ex.Message.ToUpper().Trim().Equals("El predio solicitado no es fiscalmente válido".ToUpper().Trim()))
                            {
                                AccionesInmuebleNoExiste();
                            }
                            else
                                throw; // si la excepción es de otro tipo lanzarla
                        }
                        if (!inmuebleDS.Inmueble.Any())
                        {   //Si no existe cuenta catastral borrar el valor catastral y dehabilitar el textbox ya que no se tiene que poder introducir valor
                            AccionesInmuebleNoExiste();
                        }
                        else
                            AccionesCuentaValida();
                    }
                    else
                        AccionesCuentaValida();
                }
                else
                {
                    //obtenemos los datos pasados por POST
                    ObtenerDatosPorPost();
                    //temp Fix hasta rediseño Pag Declaracion
                    if (XmlDocumental != null && XmlDocumental.Length != 0)
                    {
                        ObtenerFechaEscrituraDocumento();
                        ConfigurarBotonesDoc();
                        txtIdDoc.Text = XmlDocumental.Length.ToString();
                    }
                    if (XmlDocumentalBF != null && XmlDocumentalBF.Length != 0)
                    {
                        ConfigurarBotonesDocBF();
                        txtIdDocDig.Text = XmlDocumentalBF.Length.ToString();
                    }
                    else
                    {
                        btnDocBeneficios.Enabled = true;
                        btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR;
                    }

                    AccionesCuentaValida();
                }
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                {
                    FechaCausacionAnt = txtFechaCausacion.Text;
                }
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
                {
                    ActoJurAnt = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR.ToString();
                }
            }
            else
            {
                if (ddlCatBanco.Items.Count == 0)//si no está rellenado el combo rellenar 
                {
                    CargarComboCatBanco();
                }

                guardarDatos();
                ConfiguraEstiloImpuestoDeclarado();

            }
            ucParticipantes.btn.Click += new ImageClickEventHandler(btnAdd_Click2);
            ActualizarBotonVolverActualizar();
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
    /// Si se accede a la declaración desde el botón ver, el botón 
    /// tendrá el texto volver,
    /// si se accede para editar o añadir una declaración el botón tendrá el valor cancelar
    /// </summary>
    private void ActualizarBotonVolverActualizar()
    {
        if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
        {
            lnkVolverDeclaraciones.Text = Constantes.BTN_VOLVER;
        }
        else lnkVolverDeclaraciones.Text = Constantes.BTN_CANCELAR;

    }

    #region cargar los datos por GET / POST

    /// <summary>
    /// Cargamos los parametros pasados por GET a las propiedades de la pagina Base de ISAI
    /// </summary>
    private void ObtenerDatosPorGet()
    {
        try
        {
            //Obtenemos la pagina origen de nuevo a la hora de presentar una declaracion
            //por el motivo de recargar la pagina al utilizar un Redirect.Go() por que al 
            //usar un Server.Transfer() tenemos problemas al ser la misma pagina Declaracion.aspx
            if (string.IsNullOrEmpty(PaginaOrigen))
            {
                PaginaOrigen = Utilidades.GetParametroUrl(Constantes.PAR_PAGINAORIGEN);
            }
            //Obtenemos parametros de la QueryString
            Operacion = Utilidades.GetParametroUrl(Constantes.PAR_OPERACION);

            if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_INS) == 0)
            {
                DseDeclaracionIsai.FEXNOT_DECLARACIONRow declaracionRow = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.NewFEXNOT_DECLARACIONRow();
                declaracionRow.CODTIPODECLARACION = Convert.ToInt32(Utilidades.GetParametroUrl(Constantes.PAR_TIPODECLARACION));
                declaracionRow.CODESTADODECLARACION = Convert.ToInt32(Constantes.PAR_VAL_ESTADODECLARACION_BORR);
                DseCatalogo.FEXNOT_CATTIPOSDECLARACIONRow rowCTD = ApplicationCache.DseCatalogoISAI.FEXNOT_CATTIPOSDECLARACION.Where(d => d.CODTIPODECLARACION == Convert.ToDecimal(Utilidades.GetParametroUrl(Constantes.PAR_TIPODECLARACION))).First();
                declaracionRow.TIPO = rowCTD.TIPOSDECLARACION;

                string idAvaluo = Utilidades.GetParametroUrl(Constantes.PAR_IDAVALUO);
                if (string.IsNullOrEmpty(idAvaluo))
                    declaracionRow.SetIDAVALUONull();
                else
                    declaracionRow.IDAVALUO = Convert.ToDecimal(Utilidades.GetParametroUrl(Constantes.PAR_IDAVALUO));

                if (Convert.ToInt32(Utilidades.GetParametroUrl(Constantes.PAR_TIPODECLARACION)).ToString() == Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
                    declaracionRow.IDDECLARACIONPADRE = Convert.ToDecimal(Utilidades.GetParametroUrl(Constantes.PAR_IDDECLARACIONPADRE));

                declaracionRow.ENPAPEL = Constantes.PAR_VAL_FALSE;
                declaracionRow.ESVCATOBTENIDOFISCAL = Constantes.PAR_VAL_TRUE;
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION.AddFEXNOT_DECLARACIONRow(declaracionRow);
            }
            else if ((Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0) || (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0))
            {
                //llamamos al servicio para cargar los datos de la declaracion por medio del iddeclaracion pasado por la URL
                DseDeclaracionIsaiMant = ClienteDeclaracionIsai.ObtenerDseDeclaracionIsaiPorIdDeclaracion(Convert.ToDecimal(Utilidades.GetParametroUrl(Constantes.PAR_IDDECLARACION)));

                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
                {
                    DseDeclaracionIsaiPadreMant = ClienteDeclaracionIsai.ObtenerDseDeclaracionIsaiPorIdDeclaracion(Convert.ToDecimal(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACIONPADRE));
                }
            }

            if (!string.IsNullOrEmpty(Utilidades.GetParametroUrl(Constantes.PAR_TIPODECLARACION)) &&
                Convert.ToInt32(Utilidades.GetParametroUrl(Constantes.PAR_TIPODECLARACION)).ToString() == Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
            {
                DseDeclaracionIsaiPadreMant = ClienteDeclaracionIsai.ObtenerDseDeclaracionIsaiPorIdDeclaracion(Convert.ToDecimal(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACIONPADRE));
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
    /// Carganos los parametros pasados por POST a las propiedades de la pagina Base de ISAI
    /// </summary>
    private void ObtenerDatosPorPost()
    {
        try
        {
            //obtenemos los datos pasados de una pagina a otra por POST
            PaginaOrigen = ((PageBaseISAI)PreviousPage).PaginaOrigen;
            Operacion = ((PageBaseISAI)PreviousPage).Operacion;
            OperacionParticipantes = ((PageBaseISAI)PreviousPage).OperacionParticipantes;
            FBusqueda = ((PageBaseISAI)PreviousPage).FBusqueda;
            XmlDocumental = ((PageBaseISAI)PreviousPage).XmlDocumental;
            TipoDocumental = ((PageBaseISAI)PreviousPage).TipoDocumental;
            ListaIdsDocumental = ((PageBaseISAI)PreviousPage).ListaIdsDocumental;
            Descrip = ((PageBaseISAI)PreviousPage).Descrip;
            DescripBF = ((PageBaseISAI)PreviousPage).DescripBF;
            XmlDocumentalBF = ((PageBaseISAI)PreviousPage).XmlDocumentalBF;
            TipoDocumentalBF = ((PageBaseISAI)PreviousPage).TipoDocumentalBF;
            ListaIdsDocumentalBF = ((PageBaseISAI)PreviousPage).ListaIdsDocumentalBF;
            CondonacionJornada = ((PageBaseISAI)PreviousPage).CondonacionJornada;
            CodActoJuridicoParticipante = ((PageBaseISAI)PreviousPage).CodActoJuridicoParticipante;
            DseDeclaracionIsaiMant = ((PageBaseISAI)PreviousPage).DseDeclaracionIsaiMant;
            DseDeclaracionIsaiPadreMant = ((PageBaseISAI)PreviousPage).DseDeclaracionIsaiPadreMant;
            DseAvaluo = ((PageBaseISAI)PreviousPage).DseAvaluo;
            SortExpression = ((PageBaseISAI)PreviousPage).SortExpression;
            SortDirectionP = ((PageBaseISAI)PreviousPage).SortDirectionP;
            SortExpression2 = ((PageBaseISAI)PreviousPage).SortExpression2;
            SortDirectionP2 = ((PageBaseISAI)PreviousPage).SortDirectionP2;

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

    #region Combos
    /// <summary>
    /// Cargo los combos de actos jurídicos con unos datos o con otros según el tipo de operación
    /// </summary>
    private void CargarCombosOperacion()
    {
        try
        {
            switch (Operacion)
            {
                case Constantes.PAR_VAL_OPERACION_INS:
                    CargarComboActoJuridicoOperacionInsertar();
                    CargarComboProcedenciaOperacionInsertar();
                    break;
                case Constantes.PAR_VAL_OPERACION_MOD:
                    CargarComboActoJuridicoOperacionEdicion();
                    CargarComboProcedenciaOperacionEdicion();
                    break;
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
    /// Método que carga el combo de los bancos con el cátalogo
    /// </summary>
    private void CargarComboCatBanco()
    {
        try
        {
            DseCatalogo.FEXNOT_CATBANCODataTable datasource = ApplicationCache.DseCatalogoISAI.FEXNOT_CATBANCO;
            this.ddlCatBanco.DataSource = datasource.Select("", "codbanco");
            this.ddlCatBanco.DataSource = from d in datasource
                                          select new
                                          {
                                              a = d.CODBANCO + " - " + d.DESCRIPCION,
                                              descripcion = d.DESCRIPCION,
                                              codbanco = d.CODBANCO,
                                              url = d.URL
                                          };
            this.ddlCatBanco.DataTextField = "a";
            this.ddlCatBanco.DataValueField = "url";
            this.ddlCatBanco.DataBind();

            //insertar elemento de seleccion
            this.ddlCatBanco.Items.Insert(0, new ListItem("Seleccione un banco...", Constantes.UI_DDL_VALUE_NO_SELECT));
            this.ddlCatBanco.SelectedValue = Constantes.UI_DDL_VALUE_NO_SELECT;
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
    /// Método que actualiza los parámetros de la URL
    /// </summary>
    private void ActualizarParametrosUrlBusqueda()
    {
        clearCache();
        if (FBusqueda.Fecha != null)
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
        RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
        RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
        RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP2, SortExpression2);
        RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR2, SortDirectionP2);
        RedirectUtil.Encrypted = true;
        RedirectUtil.Go();
    }

    /// <summary>
    /// Método que obtiene el catálogo de actos jurídicos y rellena un combo con los valores
    /// </summary>
    private void CargarComboActoJuridicoOperacionInsertar()
    {
        try
        {
            this.ddlActoJuridicoDato.DataSource = ApplicationCache.DseCatalogoISAI.FEXNOT_CATACTOSJURIDICOS.Select("", "descripcion asc");
            this.ddlActoJuridicoDato.DataTextField = ApplicationCache.DseCatalogoISAI.FEXNOT_CATACTOSJURIDICOS.DESCRIPCIONColumn.ToString();
            this.ddlActoJuridicoDato.DataValueField = ApplicationCache.DseCatalogoISAI.FEXNOT_CATACTOSJURIDICOS.CODACTOJURColumn.ToString();

            this.ddlActoJuridicoDato.DataBind();

            //insertar elemento de seleccion
            this.ddlActoJuridicoDato.Items.Insert(0, new ListItem("Seleccione un Acto Jurídico...", Constantes.UI_DDL_VALUE_NO_SELECT));
            this.ddlActoJuridicoDato.SelectedIndex = 0;
            ConfigurarTasaCero();
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
    /// Método que prepara el combo de acto jurídico para una operacion de edicion
    /// </summary>
    private void CargarComboActoJuridicoOperacionEdicion()
    {
        try
        {
            DseCatalogo.FEXNOT_CATACTOSJURIDICOSDataTable actosJuridicosDT = new DseCatalogo.FEXNOT_CATACTOSJURIDICOSDataTable();
            actosJuridicosDT.Merge(ApplicationCache.DseCatalogoISAI.FEXNOT_CATACTOSJURIDICOS);
            this.ddlActoJuridicoDato.DataSource = actosJuridicosDT.Select("", "descripcion asc");
            this.ddlActoJuridicoDato.DataTextField = actosJuridicosDT.DESCRIPCIONColumn.ToString();
            this.ddlActoJuridicoDato.DataValueField = actosJuridicosDT.CODACTOJURColumn.ToString();
            this.ddlActoJuridicoDato.DataBind();
            this.ddlActoJuridicoDato.Items.Insert(0, new ListItem("Seleccione un Acto Jurídico...", ""));
            AsignarComboActoJuridico();
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
    /// Método que preselecciona un valor en el combo de acto jurídico
    /// </summary>
    private void AsignarComboActoJuridico()
    {
        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
        {
            this.ddlActoJuridicoDato.SelectedValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR.ToString();
        }
        EsHerencia();
        //ActualizarBotonesParticipantes();
    }


    /// <summary>
    /// Método que obtiene el catalogo de procedencia y rellena un combo con los valores
    /// </summary>
    private void CargarComboProcedencia()
    {
        try
        {
            this.ddlPaisDato.DataSource = ApplicationCache.DseCatalogoISAI.FEXNOT_CATPROCEDENCIA;

            this.ddlPaisDato.DataTextField = ApplicationCache.DseCatalogoISAI.FEXNOT_CATPROCEDENCIA.DESCRIPCIONColumn.ToString();
            this.ddlPaisDato.DataValueField = ApplicationCache.DseCatalogoISAI.FEXNOT_CATPROCEDENCIA.CODPROCEDENCIAColumn.ToString();

            this.ddlPaisDato.DataBind();

            //insertar elemento de seleccion
            this.ddlPaisDato.Items.Insert(0, new ListItem("Seleccione una procedencia...", Constantes.UI_DDL_VALUE_NO_SELECT));
            this.ddlPaisDato.SelectedIndex = 1;



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
    /// Método que prepara el combo de procedencia para una operacion de insercion
    /// </summary>
    private void CargarComboProcedenciaOperacionInsertar()
    {
        try
        {
            CargarComboProcedencia();
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
    /// Método que prepara el combo de procedencia para una operacion de edicion
    /// </summary>
    private void CargarComboProcedenciaOperacionEdicion()
    {
        try
        {
            CargarComboProcedencia();
            AsignarComboProcedencia();
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
    /// Método que preselecciona un valor en el combo de procedencia
    /// </summary>
    private void AsignarComboProcedencia()
    {
        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODPROCEDENCIANull())
            this.ddlPaisDato.SelectedValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODPROCEDENCIA.ToString();
    }


    /// <summary>
    /// Maneja el evento OnSelectedIndexChanged del control ddlCatBanco
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCatBanco_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ComprobarBanco();
        extenderModalPopupPago.Show();
    }
    #endregion

    #region Configurar los controles de la página

    /// <summary>
    /// Método que registra las funciones javascript a los botones que integran con documental
    /// </summary>
    private void RegistrarJSButton()
    {
        try
        {
            string idNotario = Usuarios.IdPersona();
            if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
                idNotario = null;

            //calculo de impuestos utility
            txtImpuestoNotarioDato.Attributes.Add("onkeypress", "return compararImpuestos(event)");
            string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlDocumental"]);
            if ((Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0) || (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0))
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCUMENTODIGITALNull())
                {
                    btnVerDoc.Enabled = true;
                    btnVerDoc.ImageUrl = Constantes.IMG_CONSULTA;

                    txtIdDoc.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL.ToString();
                    //visualizar el documento digital
                    btnVerDoc.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                        url,
                        null,
                        null,
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL,
                        'S',
                        null,
                        null,
                        null,
                        "N", null, null, null, idNotario, "DeclaracionISAI");


                    lblDescripcionDoc.Text = DescripcionDocumento(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL);
                    Descrip = lblDescripcionDoc.Text;
                    btnEditarDocumento.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                       url,
                       null,
                       null,
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL,
                       'S',
                       txtDocDigital.ClientID,
                       txtTipoDocumento.ClientID,
                       txtListaIdsFicheros.ClientID,
                       "S", null, null, null, idNotario, "DeclaracionISAI");
                }
                else
                {
                    btnVerDoc.Enabled = false;
                    btnVerDoc.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_ESTADODECLARACION_BORR))
                    {
                        btnDocumentos0.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                        url,
                        "1",
                        "EP,DP,RJ,DA",
                        null,
                        'S',
                        txtDocDigital.ClientID,
                        txtTipoDocumento.ClientID,
                        txtListaIdsFicheros.ClientID,
                        "S", null, null, "S", idNotario, "DeclaracionISAI");
                    }
                }

                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCBENFISCALESNull())
                {
                    btnVerDocBeneficios.Enabled = true;
                    btnVerDocBeneficios.ImageUrl = Constantes.IMG_CONSULTA;

                    txtIdDocDig.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES.ToString();
                    //visualizar el documento digital
                    btnVerDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                        url,
                        "99",
                        null,
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES,
                        'S',
                        null,
                        null,
                        null,
                        "N", null, null, null, idNotario, "DeclaracionISAI");
                    dseDocumentosDigitales.DOC_DOCUMENTODIGITALDataTable dtDoc = new dseDocumentosDigitales.DOC_DOCUMENTODIGITALDataTable();
                    dtDoc = ClienteDocumentos.GetOtrosById(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES);
                    if (dtDoc.Any())
                    {
                        if (!dtDoc.First().IsDESCRIPCIONNull())
                        {
                            lblDescripcionDocBF.Text = (dtDoc.First().DESCRIPCION.ToString().Length > Constantes.LongitudLineaDescripcion) ? dtDoc.First().DESCRIPCION.ToString().Substring(0, Constantes.LongitudLineaDescripcion - 20) + "..." : dtDoc.First().DESCRIPCION.ToString() + " - Fecha:  " + Convert.ToDateTime(dtDoc.First().FECHA).ToShortDateString();
                        }
                        else
                        {
                            lblDescripcionDocBF.Text = "Beneficio fiscal - Fecha:  " + Convert.ToDateTime(dtDoc.First().FECHA).ToShortDateString();

                        }
                    }
                    DescripBF = lblDescripcionDocBF.Text;
                    btnEditarDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12},'{13}')",
                       url,
                       null,
                       null,
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES,
                       'S',
                       txtDocDigitalBF.ClientID,
                       txtTipoDocumentoBF.ClientID,
                       txtListaIdsFicherosBF.ClientID,
                       "S", null, null, null, idNotario, "DeclaracionISAI");
                }
                else
                {
                    btnVerDocBeneficios.Enabled = false;
                    btnVerDocBeneficios.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
                    //No hay documento y es borrador
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_ESTADODECLARACION_BORR))
                    {
                        btnDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                             url,
                             "99",
                             null,
                         null,
                         'S',
                         txtDocDigitalBF.ClientID,
                         txtTipoDocumentoBF.ClientID,
                         txtListaIdsFicherosBF.ClientID,
                         "S", null, null, "S", idNotario, "DeclaracionISAI");
                    }
                }

                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION != Convert.ToDecimal(Constantes.PAR_VAL_ESTADODECLARACION_BORR))
                {
                    btnDocumentos0.Enabled = false;
                    btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocBeneficios.Enabled = false;
                    btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                }
                else
                {
                    if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0)
                    {

                        btnDocumentos0.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                            url,
                            "1",
                            "EP,DP,RJ,DA",
                            null,
                            'S',
                            txtDocDigital.ClientID,
                            txtTipoDocumento.ClientID,
                            txtListaIdsFicheros.ClientID,
                            "S", null, null, "S", idNotario, "DeclaracionISAI");


                        btnDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                           url,
                           "99",
                            null,
                       null,
                       'S',
                       txtDocDigitalBF.ClientID,
                       txtTipoDocumentoBF.ClientID,
                       txtListaIdsFicherosBF.ClientID,
                       "S", null, null, "S", idNotario, "DeclaracionISAI");

                    }
                    else
                    {
                        btnDocumentos0.Enabled = false;
                        btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                        btnDocBeneficios.Enabled = false;
                        btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    }
                }
            }
            //INSERTAR
            else
            {
                //visualizar el documento digital
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCUMENTODIGITALNull())
                {
                    btnVerDoc.Enabled = true;
                    btnVerDoc.ImageUrl = Constantes.IMG_CONSULTA;
                    btnVerDoc.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                        url,
                        null,
                        null,
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL,
                        "S",
                        null,
                        null,
                        null,
                        "N", null, null, null, idNotario, "DeclaracionISAI");

                    lblDescripcionDoc.Text = DescripcionDocumento(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL);
                    Descrip = lblDescripcionDoc.Text;

                    txtIdDoc.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL.ToString();
                    btnEditarDocumento.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                       url,
                       null,
                       null,
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL,
                       "S",
                       txtDocDigital.ClientID,
                       txtTipoDocumento.ClientID,
                       txtListaIdsFicheros.ClientID,
                       "S", null, null, null, idNotario, "DeclaracionISAI");
                }
                //Puede que en la declaracion padre exista un documento y en insertar tiene que mostrarse
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCBENFISCALESNull())
                {
                    btnVerDocBeneficios.Enabled = true;
                    btnVerDocBeneficios.ImageUrl = Constantes.IMG_CONSULTA;
                    txtIdDoc.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES.ToString();
                    //visualizar el documento digital
                    btnVerDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                        url,
                        "99",
                        null,
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES,
                        "S",
                        null,
                        null,
                        null,
                        "N", null, null, null, idNotario, "DeclaracionISAI");

                    lblDescripcionDocBF.Text = ClienteDocumentos.GetOtrosById(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES)[0].DESCRIPCION.ToString();
                    DescripBF = lblDescripcionDocBF.Text;

                    btnEditarDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                       url,
                       null,
                       null,
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCBENFISCALES,
                       'S',
                       txtDocDigitalBF.ClientID,
                       txtTipoDocumentoBF.ClientID,
                       txtListaIdsFicherosBF.ClientID,
                       "S", null, null, null, idNotario, "DeclaracionISAI");
                }


                //insertar un documento digital
                // 1 = 
                //EP = escritura publica
                //DP = documento privado
                //RJ = registro juridico
                //DA = documento administrativo
                //btnDocumentos0.Enabled = true;
                //btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR;

                btnDocumentos0.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                    url,
                    "1",
         "EP,DP,RJ,DA",
         null,
         "S",
         txtDocDigital.ClientID,
         txtTipoDocumento.ClientID,
         txtListaIdsFicheros.ClientID,
         "S", null, null, "S", idNotario, "DeclaracionISAI");

                btnDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12},'{13}')",
               url,
               "99",
               "DA",
           null,
           "S",
           txtDocDigitalBF.ClientID,
           txtTipoDocumentoBF.ClientID,
           txtListaIdsFicherosBF.ClientID,
           "S", null, null, "S", idNotario, "DeclaracionISAI");
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

    #region Configurar el formulario según la operación

    /// <summary>
    /// Método que configura el formulario según el tipo de operación
    /// </summary>
    private void ConfigurarFormularioOperacion()
    {
        try
        {
            //Por la operación
            switch (Operacion)
            {
                case Constantes.PAR_VAL_OPERACION_VER:
                    ConfigurarFormularioOperacionVisualizacion();
                    CargarDeclaracion();
                    break;
                case Constantes.PAR_VAL_OPERACION_INS:
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
                        ConfigurarFormularioOperacionInsertar();
                    else
                        ConfigurarFormularioOperacionEdicion();
                    break;
                case Constantes.PAR_VAL_OPERACION_MOD:
                    ConfigurarFormularioOperacionEdicion();
                    CargarDeclaracion();
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION > 0)
                        FormularioActoJuridicoVisualizacion();
                    break;
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
    /// Método que configura el formulario para la operación modificar
    /// </summary>
    private void ConfigurarFormularioOperacionEdicion()
    {
        //Botones de los documentos
        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCUMENTODIGITALNull())
        {
            //Hay documento
            btnDocumentos0.Enabled = false;
            btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
            btnEditarDocumento.Enabled = true;
            btnEditarDocumento.ImageUrl = Constantes.IMG_MODIFICA;
            btnBorrarDocumento.Enabled = true;
            btnBorrarDocumento.ImageUrl = Constantes.IMG_ELIMINA;
        }
        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCBENFISCALESNull() || (XmlDocumentalBF != null && XmlDocumentalBF.Length != 0))
        {
            btnDocBeneficios.Enabled = false;
            btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
            btnEditarDocBeneficios.Enabled = true;
            btnEditarDocBeneficios.ImageUrl = Constantes.IMG_MODIFICA;
            btnEliminarDocBeneficios.Enabled = true;
            btnEliminarDocBeneficios.ImageUrl = Constantes.IMG_ELIMINA;
        }
        else
        {
            btnDocBeneficios.Enabled = true;
            btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR;
        }

        //Fecha preventiva, solo existe en el caso de declaración anticipada
        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_ANTI)
        {
            divFechaPreventiva.Visible = true;
            lblFechaPreveDato.Visible = false;
            txtFechaPrev.Visible = true;
            btnFechaPrev.Visible = true;
        }

        ucParticipantes.ActualizarEstadoBotones(false);
    }

    /// <summary>
    /// Método que configura el formulario para la operación insertar
    /// </summary>
    private void ConfigurarFormularioOperacionInsertar()
    {
        //Oculta el boton BImprirInformeISR
        divPago.Visible = false;
        this.divBeneficiosFiscales.Visible = false;
        this.BeneficiosiscalesLeyend.Visible = true;
        ucParticipantes.ActualizarEstadoBotones(false);
    }

    /// <summary>
    /// Método que configura el formulario para la operación visualizar declaración
    /// </summary>
    /// </summary>
    private void ConfigurarFormularioOperacionVisualizacion()
    {

        //Enable al control CBCausaISR 
        CBCausaISR.Enabled = false;
        //Detalles Declaracion
        txtRegion.Visible = false;
        txtManzana.Visible = false;
        txtLote.Visible = false;
        txtUnidadPrivativa.Visible = false;
        lblCuentaDato.Visible = true;
        btnValidarCuentaCatastral.Visible = false;
        btnBorrarCuenta.Visible = false;
        HabilitarDeshabilitarValorCatastral(false); //se muestra el lblDato con el valor que corresponda en vez del textbox donde el usuario introduce un valor

        //Acto Juridico
        FormularioActoJuridicoVisualizacion();
        cvValorAdquisicionDato.Enabled = false;
        lblTotalActoJurDato.Visible = true;
        //Declaracion
        lblValorAdquisicionDato.Visible = true;
        txtValorAdquisicionDato.Visible = false;
        LblLugarDato.Visible = true;
        txtLugar.Visible = false;

        #region [ Beneficios Fiscales ]

        //botones
        BotonesBeneficios.Visible = false;
        imgBeneficiosOff.Visible = false;
        imgBeneficiosOn.Visible = false;

        //Exencion
        tdExencion1.Visible = false;
        tdExencion2.Visible = false;

        //Reduccion
        tdReduccion1.Visible = false;
        tdReduccion2.Visible = false;

        //Subsidio        
        tdSubsidio1.Visible = false;
        tdSubsidio2.Visible = false;
        tdSubsidio3.Visible = false;

        //Disminucion
        tdDisminucion1.Visible = false;
        tdDisminucion2.Visible = false;


        lblPorcentaje2.Visible = false;
        //Condonacion
        trCondonacion1.Visible = false;
        trCondonacion2.Visible = false;
        trCondonacion3.Visible = false;
        txtPorcentajeCondonacion.Visible = false;
        LblPorcentajeCondonacion.Visible = true;
        lblPorcentaje3.Visible = false;
        txtMotivoCondonacion.Visible = false;
        LblMotivoCondonacionDato.Visible = true;
        txtFechaCondonacion.Visible = false;
        LblFechaCondonacionDato.Visible = true;

        //tasa cero
        trTasaCero.Visible = false;
        rbTasaCero.Checked = false;
        //documento justificativo

        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCBENFISCALESNull())
        {
            trBeneficiosJustificacion.Visible = true;
            trBeneficiosJustificacion2.Visible = true;
            this.trDesBeneficios.Visible = true;
        }
        else
        {
            trBeneficiosJustificacion.Visible = false;
            trBeneficiosJustificacion2.Visible = false;
            this.trDesBeneficios.Visible = false;
        }

        #endregion

        //Impuesto
        btnCalcular.Visible = false;
        ////lblImpuestoNotarioDato.visible = true;
        ////lblImpuestoNotarioDato.visible = false;
        ////txtImpuestoNotarioDato.visible = false;

        //pago
        txtFechaLineaCaptura.Visible = false;
        btnFechaLineaCaptura.Visible = false;
        txtLineaDato.Visible = false;


        //if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION != Constantes.PAR_VAL_ESTADODECLARACION_PDTE_PRESENTAR.ToInt())
        //{
        //    btnObtenerLinea.Visible = false;
        //    btnPagoTelematico.Visible = false;
        //}
        mostrarBotonPago(Convert.ToInt32(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION.ToString()));
        //Botones
        btnGuardar.Visible = false;
        btnDocumentos0.Visible = false;
        btnDocumentos0.Enabled = false;
        btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
        btnEditarDocumento.Visible = false;
        btnBorrarDocumento.Visible = false;

        btnDocBeneficios.Visible = false;
        btnDocBeneficios.Enabled = false;
        btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
        btnEditarDocBeneficios.Visible = false;
        btnEliminarDocBeneficios.Visible = false;

        lblValorAdquisicionDolar.Visible = false;
        //lblImpuestoNotarioDolar.Visible = false;

        //Fecha causacion
        txtFechaCausacion.Visible = false;
        btnFechaCausa.Visible = false;
        lblFechaCausacionDato.Visible = true;

        //Fecha preventiva, solo existe en el caso de declaración anticipada
        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_ANTI)
        {
            divFechaPreventiva.Visible = true;
            lblFechaPreveDato.Visible = true;
            txtFechaPrev.Visible = false;
            btnFechaPrev.Visible = false;
        }
        IsrReadOnly((int)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION);
    }

    /// <summary>
    /// Método que Configura la parte del formulario de Actos Juridicos en visualizacion
    /// </summary>
    private void FormularioActoJuridicoVisualizacion()
    {
        if ((DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_ESTADODECLARACION_BORR) == 0)
            && ((Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0)))
        {
            LblPaisDato.Visible = false;
            ddlPaisDato.Visible = true;
            LblActoJuridicoDato.Visible = false;
            ddlActoJuridicoDato.Visible = true;
        }
        else
        {
            LblPaisDato.Visible = true;
            ddlPaisDato.Visible = false;
            LblActoJuridicoDato.Visible = true;
            ddlActoJuridicoDato.Visible = false;
        }
    }

    /// <summary>
    /// Método que carga la declaración 
    /// </summary>
    private void CargarDeclaracion()
    {
        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION > 0)
        {
            base.CargarDeclaracionesPorIdDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
        }

    }


    /// <summary>
    /// Método que recarga el dataset de la declaración por el id almacenado
    /// </summary>
    /// <param name="idDeclaracion">id de la declaración a cargar</param>
    private void RecargarDeclaracion(decimal idDeclaracion)
    {
        DseDeclaracionIsaiMant = null;
        DseDeclaracionIsaiMant = ClienteDeclaracionIsai.ObtenerDseDeclaracionIsaiPorIdDeclaracion(idDeclaracion);
    }


    #endregion

    #region Cargar Datos Formulario

    /// <summary>
    /// Método que carga los controles del formulario con los datos necesarios.
    /// </summary>
    private void CargarDatosFormulario()
    {
        try
        {
            switch (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString())
            {
                case Constantes.PAR_VAL_TIPODECLARACION_JOR:
                    if (Operacion == Constantes.PAR_VAL_OPERACION_INS)
                        CargarDatosFormularioOperacionInsertarJornada();
                    else if ((Operacion == Constantes.PAR_VAL_OPERACION_MOD) || (Operacion == Constantes.PAR_VAL_OPERACION_VER))
                        CargarDatosDeclaracion();
                    break;
                case Constantes.PAR_VAL_TIPODECLARACION_COMPLE:
                    rfvActoJuridico.Enabled = false;
                    if (Operacion == Constantes.PAR_VAL_OPERACION_INS)
                    {
                        CargarDatosFormularioTipoNormalAnticipadaComplementaria();
                        CargarDatosFormularioOperacionInsertarComplementaria();
                        //Complementaria
                        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCBENFISCALESNull())
                        {
                            btnDocBeneficios.Enabled = false;
                            btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                            btnEditarDocBeneficios.Enabled = true;
                            btnEditarDocBeneficios.ImageUrl = Constantes.IMG_MODIFICA;
                            btnEliminarDocBeneficios.Enabled = true;
                            btnEliminarDocBeneficios.ImageUrl = Constantes.IMG_ELIMINA;
                        }
                        else
                        {
                            btnDocBeneficios.Enabled = true;
                            btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR;
                        }

                    }
                    else if (Operacion == Constantes.PAR_VAL_OPERACION_MOD || Operacion == Constantes.PAR_VAL_OPERACION_VER)
                    {
                        CargarDatosDeclaracion();
                    }
                    break;
                case Constantes.PAR_VAL_TIPODECLARACION_ANTI:
                    if (Operacion == Constantes.PAR_VAL_OPERACION_INS)
                    {
                        CargarDatosFormularioTipoNormalAnticipadaComplementaria();
                        CargarDatosFormularioOperacionInsertarNormalAnticipada();
                    }
                    else if (Operacion == Constantes.PAR_VAL_OPERACION_MOD ||
                       Operacion == Constantes.PAR_VAL_OPERACION_VER)
                    {
                        CargarDatosDeclaracion();
                    }
                    break;
                case Constantes.PAR_VAL_TIPODECLARACION_NOR:
                    if (Operacion == Constantes.PAR_VAL_OPERACION_INS)
                    {
                        CargarDatosFormularioTipoNormalAnticipadaComplementaria();
                        CargarDatosFormularioOperacionInsertarNormalAnticipada();
                    }
                    else if (Operacion == Constantes.PAR_VAL_OPERACION_MOD ||
                        Operacion == Constantes.PAR_VAL_OPERACION_VER)
                    {
                        CargarDatosDeclaracion();
                    }
                    break;
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
    /// Método que carga los datos para crear la declaración Jornada notarial.
    /// </summary>
    private void CargarDatosFormularioOperacionInsertarJornada()
    {
        try
        {
            //Tipo Jornada
            lblTipoDato.Text = Resources.Resource.LIT_DEC_TIP_JORNADA_NOTARIAL;

            //Estado borrador
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = Constantes.PAR_VAL_ESTADODECLARACION_BORR.ToInt();
            lblEstadoDato.Text = Resources.Resource.LIT_DEC_EST_BORRADOR;

            //Idpersona
            if (!Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDPERSONA = Convert.ToDecimal(Usuarios.IdPersona());
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
    /// Método que carga los datos de los controles para el tipo Normal, anticipada o complementaria.
    /// </summary>
    private void CargarDatosFormularioTipoNormalAnticipadaComplementaria()
    {
        //Cargamos los combos de exencion y reducción
        try
        {
            //habilitamos o deshabilitamos los radiobutton de reduccion y exencion y dejamos subsidio por defecto activo
            if ((!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull() && DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_ANTI) || (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_ANTI))
            {
                rbReduccion.Enabled = true;
                rbExencion.Enabled = true;
            }
            else
            {
                rbReduccion.Enabled = false;
                rbExencion.Enabled = false;
                txtPorcentajeSubsidio.Enabled = false;
                ValoresPorDefectoCombos();
            }
            CargarDDLExeccion();
            CargarDDLReduccion();
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
    /// Método que carga los datos de la declaración padre para copiar en la declaración complementaria en la operación insertar.
    /// </summary>
    private void CargarDatosFormularioOperacionInsertarComplementaria()
    {
        try
        {
            //REALIZADO: Obtenemos el dataset cargado con el iddeclaracionPadre
            base.CargarDeclaracionPadrePorIdDeclaracionPadre(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACIONPADRE);
            DseDeclaracionIsaiMantDatosTipoComplementariaEstadoBorrador();
            DseDeclaracionIsaiMantAllToStateAdded();
            //cargamos los datos del avalúo
            CargarDatosAvaluo();

            //Acto juridico
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsACTOJURIDICONull())
                LblActoJuridicoDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ACTOJURIDICO.ToString();
            AsignarComboActoJuridico();

            //procedencia
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODPROCEDENCIANull())
                LblPaisDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PROCEDENCIA.ToString();
            AsignarComboProcedencia();

            //Numero presentación anterior -> Numero presentación del padre
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsNUMPRESENTACIONINICIALNull())
                lblNumPresentacionAntDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACIONINICIAL.ToString();


            //Tipo complementaria
            lblTipoDato.Text = Resources.Resource.LIT_DEC_TIP_COMPLEMENTARIA;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION = Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_COMPLE);

            //Estado borrador
            lblEstadoDato.Text = Resources.Resource.LIT_DEC_EST_BORRADOR;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = Constantes.PAR_VAL_ESTADODECLARACION_BORR.ToInt();

            //Detalle valor adquisición y deudas
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORADQUISICIONNull())
                txtValorAdquisicionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORADQUISICION.ToString();

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
            {
                txtValorCatastral.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL.ToString();
                lblValorCatastralDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL.ToString();

            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsREGLANull())
            {
                rbReglasVigentes.Checked = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA == 0 ? true : false;
                rbReglasFallecimiento.Checked = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA == 1 ? true : false;
            }
            else
            {
                rbReglasVigentes.Checked = true;
                rbReglasFallecimiento.Checked = false;
            }
            //Fecha escritura del documento digital
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
                lblFechaEscrituraDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA.ToShortDateString();

            CargarDatosFormularioTipoNormalAnticipadaComplementaria();

            //Beneficios Fiscales con los datos de la declaracion padre
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDEXENCIONNull())
            {
                rbSubsidio.Checked = false;
                rbExencion.Checked = true;
                rbExencion.Enabled = true;
                rbReduccion.Enabled = true;
                rbSubsidio.Enabled = true;
                BeneficioFiscal_CheckedChanged(this.rbExencion, null);
                ucExencion.IdExencion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDEXENCION.ToInt();
                ActivaSeccionBeneficiosFiscales(true, true);
            }
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
            {
                rbSubsidio.Checked = false;
                rbReduccion.Checked = true;
                rbReduccion.Enabled = true;
                BeneficioFiscal_CheckedChanged(this.rbReduccion, null);
                ucReduccion.IdReducccion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDREDUCCION.ToInt();
                ActivaSeccionBeneficiosFiscales(true, true);
            }
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull())
            {
                rbSubsidio.Checked = true;
                rbSubsidio.Enabled = true;
                BeneficioFiscal_CheckedChanged(rbSubsidio, null);
                txtPorcentajeSubsidio.Text = Convert.ToString(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO * 100);
                ActivaSeccionBeneficiosFiscales(true, true);
            }
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull())
            {
                rbSubsidio.Checked = false;
                rbDisminucion.Checked = true;
                BeneficioFiscal_CheckedChanged(rbDisminucion, null);
                txtPorcentajeDisminucíon.Text = (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION * 100).ToString();
                ActivaSeccionBeneficiosFiscales(true, true);
            }
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
            {
                rbSubsidio.Checked = false;
                rbCondonacion.Checked = true;
                BeneficioFiscal_CheckedChanged(rbCondonacion, null);
                txtPorcentajeCondonacion.Text = (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION * 100).ToString();
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
                    txtFechaCondonacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACONDONACION.ToString("dd/MM/yyyy");
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
                    txtMotivoCondonacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MOTIVOCONDONACION.ToString();
                ActivaSeccionBeneficiosFiscales(true, true);
            }
            else
            {
                ActivaSeccionBeneficiosFiscales(false, false);
            }

            //Detalle del sujeto de la declaración del padre

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCJERECARGOPAGOEXTEMPNull())
                lblPorcRegargoExTempDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCJERECARGOPAGOEXTEMP.ToPercent();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsACTUALIZACIONIMPORTENull())
                lblActualizacionImporteDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE.ToCurrency();

            //Calculo Automatico del Impuesto - Mantenemos el branch
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
            {
                ///el importe a pagar por defecto es el calculado en la declaracion normal
                decimal ImpuestoPorPagar = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO.ToDecimal();
                //si se realizo un pago, se le resta el importe pagado
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                    ImpuestoPorPagar = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO - DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO;
                //se visualiza el impuesto calculado
                lblImpuestoCalculadoDato.Text = System.Math.Round(ImpuestoPorPagar + (0.49).ToDecimal()).ToString();
                //se propone el impuesto calculado como pago
                RellenarImpuestoDeclarado(ImpuestoPorPagar, txtImpuestoNotarioDato);
            }


            //valor catastral
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
            {
                AsignarValorCatastral(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL);
                if (IsaiManager.HabilitarTasaCero(null, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL))
                {
                    rbTasaCero.Enabled = true;
                    trBeneficiosJustificacion.Visible = true;
                    trBeneficiosJustificacion2.Visible = true;
                    this.trDesBeneficios.Visible = true;
                }
                else
                {
                    rbTasaCero.Checked = false;
                    rbTasaCero.Enabled = false;
                }

                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESVCATOBTENIDOFISCAL == Constantes.PAR_VAL_FALSE)
                {
                    HabilitarDeshabilitarValorCatastral(true);
                }
                else
                {
                    HabilitarDeshabilitarValorCatastral(false);
                }
            }


            //impuesto declarado anterior
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                RellenarImpuestoDeclarado(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO, lblImpuestoPagadoAnterior);

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
            {
                txtFechaCausacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION.ToShortDateString();
                lblFechaCausacionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION.ToShortDateString();
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
    /// Método que carga los datos del avalúo sobre el que se va a crear la declaración.
    /// </summary>
    private void CargarDatosFormularioOperacionInsertarNormalAnticipada()
    {
        try
        {
            CargarDatosAvaluo();
            //Tipo de declaración
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_ANTI) == 0)
                lblTipoDato.Text = Resources.Resource.LIT_DEC_TIP_ANTICIPADA;
            else if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_NOR) == 0)
                lblTipoDato.Text = Resources.Resource.LIT_DEC_TIP_NORMAL;

            //Estado borrador
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = Constantes.PAR_VAL_ESTADODECLARACION_BORR.ToInt();
            lblEstadoDato.Text = Resources.Resource.LIT_DEC_EST_BORRADOR;

            //IdPersona
            if (!Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDPERSONA = Convert.ToDecimal(Usuarios.IdPersona());
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

    #region Cargar datos del formulario de tipo NORMAL / ANTICIPADA / COMPLEMENTARIA
    /// <summary>
    /// Establece los valores por defecto en los combos.
    /// </summary>
    protected void ValoresPorDefectoCombos()
    {
        try
        {
            //borrar los datos de los campos de forma que no se guardan
            txtPorcentajeDisminucíon.Text = string.Empty;
            txtPorcentajeSubsidio.Text = string.Empty;
            txtPorcentajeCondonacion.Text = string.Empty;
            txtFechaCondonacion.Text = string.Empty;
            txtMotivoCondonacion.Text = string.Empty;
            //dejar los combos con la seleccion por default
            ucReduccion.ResetearControl();
            ucExencion.ResetearControl();
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
    /// Carga de datos el combo reducciones.
    /// </summary>
    protected void CargarDDLReduccion()
    {
        try
        {
            ucReduccion.Anio = DateTime.Now.Year;
            if (rbReduccion.Checked)
                ucReduccion.Editable = true;
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
    /// Carga de datos el combo exencciones.
    /// </summary>
    protected void CargarDDLExeccion()
    {
        try
        {
            ucExencion.Anio = DateTime.Now.Year;
            if (rbExencion.Checked)
                ucExencion.Editable = true;
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
    #region Cargar los datos de Avalúo

    /// <summary>
    /// Método que obtiene los datos del avalúo en función del id de avalúo almacenado en HiddenIdAvaluo
    /// </summary>
    private void CargarDatosAvaluo()
    {
        //Cargaremos los valores del Avaluo llamando al servicio WCF Avaluos a través del WCF de DeclaracionIsai
        decimal idAvaluo = idAvaluo = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO;
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
        base.CargarAvaluo(idAvaluo);
        if (DseAvaluo.FEXAVA_AVALUO_V.Any())
        {
            //carga del numero de avaluo
            lblIdAvaluoDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].NUMEROUNICO.ToString();

            if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORCOMERCIALNull())
            {
                lblValorComercialDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].VALORCOMERCIAL.ToCurrency();// ((Decimal)300000).ToCurrency();
            }
            else if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORCATASTRALNull())
            {
                lblValorComercialDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].VALORCATASTRAL.ToCurrency();
            }

            string[] cuentaCatastral = new string[4];
            cuentaCatastral[0] = DseAvaluo.FEXAVA_AVALUO_V[0].REGION.Trim().PadLeft(3, '0');
            cuentaCatastral[1] = DseAvaluo.FEXAVA_AVALUO_V[0].MANZANA.Trim().PadLeft(3, '0');
            cuentaCatastral[2] = DseAvaluo.FEXAVA_AVALUO_V[0].LOTE.Trim().PadLeft(2, '0');
            cuentaCatastral[3] = DseAvaluo.FEXAVA_AVALUO_V[0].UNIDADPRIVATIVA.Trim().PadLeft(3, '0');
            if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVIGENTENull())
                lblVigenteDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].VIGENTE == Constantes.PAR_VAL_TRUE ? "Si" : "No";
            else
                lblVigenteDato.Text = string.Empty;

            if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHA_DOCDIGITALNull())
                lblFechaDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].FECHA_DOCDIGITAL.ToShortDateString();// DateTime.Now.ToShortDateString();
            else
                lblFechaDato.Text = string.Empty;

            if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull())
            {
                lblFechaValRefDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO.ToShortDateString();
                if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull())
                    lblValorRefDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO.ToCurrency();
            }
            else
            {

                divValorRef.Visible = false;
                if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORCOMERCIALNull())
                    lblValorComercialDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].VALORCOMERCIAL.ToCurrency();

            }

            //Detalles del inmueble
            lblRegionDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].REGION.FormaRegion(); //"001";
            lblLoteDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].LOTE.FormatLote();//"01";
            lblManzanaDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].MANZANA.FormatManzana();//"001";
            lblUnidadPrivativaDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].UNIDADPRIVATIVA.FormatUnidadPrivativa();// "001";
            if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsCUENTACATASTRALNull())
            {
                lblCuentaDato.Text = DseAvaluo.FEXAVA_AVALUO_V[0].CUENTACATASTRAL;// CatastralUtils.CodesToCuentaCatastral("001", "001", "01", "001", false);
            }
            else
            {
                lblCuentaDato.Text = string.Empty;
            }


        }
    }

    #endregion
    #region Cargar los datos de la declaración

    /// <summary>
    /// Método que carga los datos de la declaracion
    /// </summary>
    private void CargarDatosDeclaracion()
    {
        try
        {
            CargarDatosDeclaracionComunes();

            switch (Operacion)
            {
                case Constantes.PAR_VAL_OPERACION_VER:
                    CargarDatosDeclaracionVisualizacion();
                    break;
                case Constantes.PAR_VAL_OPERACION_MOD:
                    CargarDatosDeclaracionEdicion();
                    break;
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
    /// Método que carga datos comunes de tipo de operación Visualizar y Modificar
    /// </summary>
    private void CargarDatosDeclaracionComunes()
    {
        try
        {
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION != Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_JOR) &&
                !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDAVALUONull())
            {
                CargarDatosAvaluo();
            }
            else if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_JOR))
            {

                HabilitarControlesCuentaCatastral(true);

                txtRegion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION;
                txtManzana.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA;
                txtLote.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE;
                txtUnidadPrivativa.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;

                lblCuentaDato.Text = CatastralUtils.CodesToCuentaCatastral(
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION,
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA,
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE,
                       DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA,
                       false);
                ///Si estamos modificando se obtienen los datos del servicio de avaluos
                ///Si estamos visualizando se obtienen de la declaración.
                if (Operacion == Constantes.PAR_VAL_OPERACION_MOD)
                {
                    lblCuentaDato.Visible = false;
                }
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
                    AsignarValorCatastral(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL);
                else
                {
                    lblValorCatastralDato.Text = string.Empty;
                }
                txtCuentaCatastral.Text = string.Empty;

            }

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsTIPONull())
                lblTipoDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].TIPO;
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsESTADONull())
                lblEstadoDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTADO;

            //detalle declaracion
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsNUMPRESENTACIONNull())
                lblNumPresentacionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACION.ToString();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsNUMPRESENTACIONINICIALNull())
                lblNumPresentacionAntDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACIONINICIAL.ToString();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAPRESENTACIONNull())
                lblFechaPresentacionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAPRESENTACION.ToShortDateString();

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCJERECARGOPAGOEXTEMPNull())
                lblPorcRegargoExTempDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCJERECARGOPAGOEXTEMP.ToPercent();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsACTUALIZACIONIMPORTENull())
                lblActualizacionImporteDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE.ToCurrency(); //review
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
                lblImpuestoCalculadoDato.Text =
                    //System.Math.Round(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO + (0.49).ToDecimal()).ToString("$ #,###,##0");
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO.ToRoundTwo().ToString("$ #,###,##0");
            //impuesto declarado
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCONDONACIONXFECHANull())
            {
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CONDONACIONXFECHA != 0)
                {
                    lblCondonacionxFecha.Visible = lblCondonacionxFechaDato.Visible = true;
                    lblCondonacionxFechaDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CONDONACIONXFECHA.TruncateVal(2).ToString("C");
                }
            }
            //if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
            //    RellenarImpuestoDeclarado(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO, lblImpuestoNotarioDato);

            //GULE - Requerimiento para mostrar las etiquetas de reducción y recargo

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTERECARGOPAGOEXTEMPNull())
                lblRecargoImporteDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP.TruncateVal(2).ToString("C");// ToRound2().ToString("$ #,###,##0");

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
            {
                DatosPDF pdf = new DatosPDF();
                pdf.impuesto = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO;
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDEXENCIONNull())
                    pdf.exencion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDEXENCION;
                else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsREDUCCIONART309Null())
                    pdf.importeReduccion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REDUCCIONART309;
                else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull())
                    pdf.subsidio = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO;
                else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull())
                    pdf.disminucion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION;
                else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
                    pdf.condonacion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
                else
                    pdf.condonacionJornada = string.IsNullOrEmpty(pdf.CondonacionJornada.ToString()) ? null : (decimal?)pdf.CondonacionJornada;
                lblBeneficioImporteDato.Text = pdf.beneficios > 0 ? pdf.beneficios.ToRound2().ToString("$ #,###,##0") : "$00.00";
            }

            //GULE - FIN

            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsLINEACAPTURANull())
            {
                //codigo gregado pro jamg
                //CAmbia estados del boton formato 
                //// 18-04-2013
                btnObtener.Enabled = false;
                btnObtener.Visible = false;
                //PanelObtenerComprobante.Enabled = false;
                //PanelObtenerComprobante.Visible = false;

                btnObtenerLinea.Enabled = true;
                lnkVolverDeclaraciones.Focus();
                //btnObtenerLinea.ImageUrl = Constantes.IMG_CAMBIAESTADO;
                lblObtenerLinea.Visible = true;
                lblRealizarPago.Visible = false;
            }
            else if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO == EstadosPago.SinPago.ToDecimal())
            {
                lblInsertarFecha.Visible = true;
                txtFechaPago.Visible = false;
                lblFechaPagoDato.Text =
                txtFechaPago.Text = DateTime.Now.ToShortDateString();
                lblFechaPagoDato.Visible = true;
                btnFechaPago.Visible = false;//true;
                //aqui
                //btnPagoTelematico.Enabled = true;
                //btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO;
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ENPAPEL == "N")
                    lblRealizarPago.Visible = true;
                lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_NOPAGADO;
                lblInsertarFecha.Visible = false;
                if (!btnObtener.Visible)
                {
                    //GeneraFormatoPDF();
                    btnObtener.Enabled = true;
                    btnObtener.Visible = true;
                }
                pnlObtener.Update();

            }
            else if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO == EstadosPago.Pagado.ToDecimal())
            {
                //aqui
                //btnPagoTelematico.Enabled = false;
                //btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO_DISABLED;
                lblRealizarPago.Visible = false;
                lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_PENDIENTE;
            }
            else
            {
                //--------------------------
                // JAMG
                // 18-04-2013
                btnObtener.Enabled = true;
                btnObtener.Visible = true;
                //PanelObtenerComprobante.Enabled = true;
                //PanelObtenerComprobante.Visible = true;


                // --------------------------
                //btnPagoTelematico.Enabled = false;
                //btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO_DISABLED;
                lblRealizarPago.Visible = false;
                lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_RECIBIDO;
            }
            mostrarBotonPago(Convert.ToInt32(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION.ToString()));
            //FUT
            //Cambio para el error 1925
            //if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFUTNull())
            //{
            //    btnVerFUT.Enabled = true;
            //    btnVerFUT.ImageUrl = Constantes.IMG_ZOOM;
            //    string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlFUT"]);
            //    btnVerFUT.OnClientClick = string.Format("javascript:AbrirUrlFut('{0}','{1}')", url, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FUT);
            //}
            //else
            //{
            //    btnVerFUT.Enabled = false;
            //    btnVerFUT.ImageUrl = Constantes.IMG_ZOOM_DISABLED;
            //}

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsBANCONull())
                lblBancoDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].BANCO;
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsSUCURSALNull())
                lblSucursalDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SUCURSAL;
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsLINEACAPTURANull())
            {
                lblLineaDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().LINEACAPTURA;
            }


            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHALINEACAPTURANull())
                lblFechaLineaDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAPAGONull())
            {
                lblFechaPagoDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAPAGO.ToShortDateString();
                lblFechaPagoDato.Visible = true;
            }

            //Codigo original
            lblDescripcionDoc.Text = Descrip;
            //Codigo modificado por jamg
            //23-04-2013
            //lblDescripcionDoc.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA.ToShortDateString(); 

            //-----------termina

            lblDescripcionDocBF.Text = DescripBF;

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                txtFechaCausacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION.ToShortDateString();


            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAPREVENTIVANull())
                txtFechaPrev.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAPREVENTIVA.ToShortDateString();

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
    /// Método que carga los datos que se pueden editar
    /// </summary>
    private void CargarDatosDeclaracionEdicion()
    {
        try
        {
            CagarDatosActosJuridicosEdicion();

            //detalle declaracion              
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORADQUISICIONNull())
                txtValorAdquisicionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORADQUISICION.ToString();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsLUGARJORNADANOTARIALNull())
                txtLugar.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LUGARJORNADANOTARIAL;

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCACTOTOTALNull())
            {
                lblTotalActoJurDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL.ToPercent();
            }

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsREGLANull())
            {
                rbReglasVigentes.Checked = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA == 0 ? true : false;
                rbReglasFallecimiento.Checked = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA == 1 ? true : false;
            }
            else
            {
                rbReglasVigentes.Checked = true;
                rbReglasFallecimiento.Checked = false;
            }

            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESVCATOBTENIDOFISCAL == Constantes.PAR_VAL_FALSE)
            {
                //Comprobar si la cuenta existe y si no existe deshabilitarlo y el valor es vacio
                if (!ExisteCuenta() && !ExisteValorCatastral())
                    HabilitarDeshabilitarValorCatastral(false);
                else
                    HabilitarDeshabilitarValorCatastral(true);
            }
            else
            {
                HabilitarDeshabilitarValorCatastral(false);
            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
            {
                AsignarValorCatastral(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL);
            }
            chkbHabitacional.Enabled = true;
            chkbHabitacional.Checked = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESHABITACIONAL.ToBooleanFromOracleChar1();

            //Beneficios Fiscales
            ActivaSeccionBeneficiosFiscales(false, false);

            rbExencion.Checked = false;
            rbCondonacion.Checked = false;
            rbSubsidio.Checked = false;
            rbReduccion.Checked = false;
            rbDisminucion.Checked = false;

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
                lblFechaEscrituraDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA.ToShortDateString();

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                lblFechaCausacionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION.ToShortDateString();
            CargarDatosFormularioTipoNormalAnticipadaComplementaria();


            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDEXENCIONNull())
            {
                ActivaSeccionBeneficiosFiscales(true, false);
                rbExencion.Checked = true;
                BeneficioFiscal_CheckedChanged(this.rbExencion, null);
                ucExencion.IdExencion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDEXENCION.ToInt();
            }
            else
            {
                ucExencion.Editable = false;

                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
                {
                    ActivaSeccionBeneficiosFiscales(true, false);
                    rbReduccion.Checked = true;
                    BeneficioFiscal_CheckedChanged(this.rbReduccion, null);
                    ucReduccion.IdReducccion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDREDUCCION.ToInt();
                }
                else
                {
                    ucReduccion.Editable = false;

                    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull())
                    {
                        ActivaSeccionBeneficiosFiscales(true, true);
                        rbSubsidio.Checked = true;
                        BeneficioFiscal_CheckedChanged(rbSubsidio, null);
                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-MX", false);
                        txtPorcentajeSubsidio.Text = Convert.ToString(Convert.ToDouble(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO) * 100.00);

                    }
                    else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull())
                    {
                        ActivaSeccionBeneficiosFiscales(true, true);
                        rbDisminucion.Checked = true;
                        BeneficioFiscal_CheckedChanged(rbDisminucion, null);
                        txtPorcentajeDisminucíon.Text = Convert.ToString(Convert.ToDouble(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION) * 100.00);

                    }
                    else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
                    {
                        ActivaSeccionBeneficiosFiscales(true, true);
                        rbCondonacion.Checked = true;
                        BeneficioFiscal_CheckedChanged(rbCondonacion, null);
                        txtPorcentajeCondonacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() ? string.Empty : (Convert.ToString(Convert.ToDouble(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION) * 100.00));
                        txtFechaCondonacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull() ? string.Empty : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACONDONACION.ToString("dd/MM/yyyy");
                        txtMotivoCondonacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull() ? string.Empty : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MOTIVOCONDONACION.ToString();
                    }
                    else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsESTASACERONull())
                    {
                        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTASACERO.Equals(Constantes.PAR_VAL_TRUE))
                        {
                            ActivaSeccionBeneficiosFiscales(true, false);
                            trTasaCero.Visible = true;
                            rbTasaCero.Checked = true;
                            rbTasaCero.Enabled = true;
                            BeneficioFiscal_CheckedChanged(rbTasaCero, null);
                        }
                    }
                }
            }

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDJORNADANOTARIALNull())
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
                    ConfigurarBeneficiosFiscalesJornadaNotarial(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL);
            }

            //detalle impuesto
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                //Impuesto Pagado o Por Pagar
                txtImpuestoNotarioDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO.ToString();

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                txtFechaCausacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION.ToShortDateString();

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAPREVENTIVANull())
                txtFechaPrev.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAPREVENTIVA.ToShortDateString();

            if ((!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsOBSERVACIONESNull()))
            {
                txtAreaObservaciones.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].OBSERVACIONES;

            }

            // Valida si contiene datos de calculos del ISR
            if ((!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCAUSAISRNull()) && (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CAUSAISR > 0))
            {
                CBCausaISR.Checked = true;
                DivISR.Visible = true;
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IsIMPORTELOCALISRNull())
                    _a = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IMPORTELOCALISR;
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IsIMPORTEFDERALISRNull())
                    _b = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IMPORTEFDERALISR;
                CargarValoresISR(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IDDECLARACION);
                IsrReadOnly((int)DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().CODESTADODECLARACION);
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
    /// Método que carga los datos en modo Visualización
    /// </summary>
    private void CargarDatosDeclaracionVisualizacion()
    {
        try
        {


            ///Acto Juridico
            CagarDatosActosJuridicosVisulizacion();
            ///Detalle declaracion
            chkbHabitacional.Enabled = false;
            chkbHabitacional.Checked = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESHABITACIONAL.ToBooleanFromOracleChar1();
            ///declaración.(Se añadira un campo a la vista de la declaracion)
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                lblFechaCausacionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION.ToShortDateString();

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAPREVENTIVANull())
                lblFechaPreveDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAPREVENTIVA.ToShortDateString();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
            {
                lblFechaEscrituraDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA.ToShortDateString();
            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCACTOTOTALNull())
            {
                lblTotalActoJurDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL.ToPercent();
            }


            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORADQUISICIONNull())
                lblValorAdquisicionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORADQUISICION.ToCurrency();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsLUGARJORNADANOTARIALNull())
                LblLugarDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LUGARJORNADANOTARIAL;
            //Beneficios Fiscales
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDJORNADANOTARIALNull())
            {
                //apagar beneficios fiscales leyend
                this.BeneficiosiscalesLeyend.Visible = false;

                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
                    ConfigurarBeneficiosFiscalesJornadaNotarial(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL);
            }
            else if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION != Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_JOR))
            {
                //se apaga para no tener doble leyenda
                this.divBeneficiosFiscales.Visible = false;
                trSinBeneficio.Visible = false;
            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDEXENCIONNull())
            {
                //apagar beneficios fiscales leyend
                this.BeneficiosiscalesLeyend.Visible = false;
                this.divBeneficiosFiscales.Visible = true;
                tdExencion1.Visible = true;
                tdExencion2.Visible = true;
                ucExencion.Editable = false;
                rbExencion.Checked = true;
                rbExencion.Visible = false;
                lblExencion.Visible = true;
                ucExencion.IdExencion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDEXENCION.ToInt();
            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
            {
                //apagar beneficios fiscales leyend
                this.BeneficiosiscalesLeyend.Visible = false;
                this.divBeneficiosFiscales.Visible = true;
                tdReduccion1.Visible = true;
                tdReduccion2.Visible = true;
                ucReduccion.Editable = false;
                rbReduccion.Checked = true;
                rbReduccion.Visible = false;
                lblReduccion.Visible = true;
                ucReduccion.IdReducccion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDREDUCCION.ToInt();

            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsESTASACERONull())
            {
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTASACERO == Constantes.PAR_VAL_TRUE)
                {
                    //apagar beneficios fiscales leyend
                    this.BeneficiosiscalesLeyend.Visible = false;
                    this.divBeneficiosFiscales.Visible = true;
                    trTasaCero.Visible = true;
                    rbTasaCero.Enabled = true;
                    rbTasaCero.Checked = true;
                }
            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull())
            {
                //apagar beneficios fiscales leyend
                this.BeneficiosiscalesLeyend.Visible = false;
                this.divBeneficiosFiscales.Visible = true;
                tdSubsidio1.Visible = true;
                tdSubsidio2.Visible = true;
                tdSubsidio3.Visible = true;
                txtPorcentajeSubsidio.Visible = false;
                LblPorcentajeSubsidio.Visible = true;
                lblPorcentaje1.Visible = false;
                rbSubsidio.Checked = true;
                rbSubsidio.Visible = false;
                lblSubsidio.Visible = true;

                LblPorcentajeSubsidio.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO.ToPercent();

            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull())
            {
                //apagar beneficios fiscales leyend
                this.BeneficiosiscalesLeyend.Visible = false;
                this.divBeneficiosFiscales.Visible = true;
                tdDisminucion1.Visible = true;
                tdDisminucion2.Visible = true;
                txtPorcentajeDisminucíon.Visible = false;
                LblPorcentajeDisminucíon.Visible = true;
                lblPorcentaje2.Visible = false;
                rbDisminucion.Checked = true;
                rbDisminucion.Visible = false;
                lblDisminuscion.Visible = true;
                LblPorcentajeDisminucíon.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION.ToPercent();

            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
            {
                //apagar beneficios fiscales leyend
                this.divBeneficiosFiscales.Visible = true;
                this.BeneficiosiscalesLeyend.Visible = false;
                trCondonacionJornada.Visible = true;
                trCondonacion1.Visible = true;
                trCondonacion2.Visible = true;
                trCondonacion3.Visible = true;
                txtPorcentajeCondonacion.Visible = false;
                LblPorcentajeCondonacion.Visible = true;
                lblPorcentaje3.Visible = false;
                txtMotivoCondonacion.Visible = false;
                LblMotivoCondonacionDato.Visible = true;
                txtFechaCondonacion.Visible = false;
                LblFechaCondonacionDato.Visible = true;
                rbCondonacion.Checked = true;
                rbCondonacion.Visible = false;
                lblcondonacion.Visible = true;
                LblPorcentajeCondonacion.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() ? string.Empty : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION.ToPercent();
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
                {
                    LblFechaCondonacionDato.Text = "Fecha no indicada.";
                    LblFechaCondonacion.Visible = false;
                }
                else
                    LblFechaCondonacionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACONDONACION.ToString("dd/MM/yyyy");

                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
                {
                    LblMotivoCondonacionDato.Text = "Motivo no indicado.";
                    LblMotivoCondonacion.Visible = false;
                }
                else
                    LblMotivoCondonacionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MOTIVOCONDONACION.ToString();
            }
            ///Detalle impuesto
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
            {
                //Impuesto Pagado o Por Pagar
                txtImpuestoNotarioDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO.ToString();
                if (LblActoJuridicoDato.Text == Constantes.PAR_VAL_HERENCIA)
                {
                    string regla;
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsREGLANull())
                    {
                        regla = " vigentes";
                    }
                    else
                    {
                        regla = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA == 1 ? " de  la fecha del fallecimiento" : " vigentes";
                    }
                    lblNotaReglaDato.Text = "* Cálculo realizado aplicando las reglas" + regla;
                    lblNotaReglaDatoDos.Visible = true;
                    DivLblRegla.Visible = true;
                }
            }

            //Valor Catastral
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
            {
                AsignarValorCatastral(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL);
            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORADQUISICIONNull())
            {
                lblValorAdquisicionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORADQUISICION.ToCurrency();
                txtValorAdquisicionDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORADQUISICION.ToCurrency();

            }

            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsOBSERVACIONESNull())
            {
                txtAreaObservaciones.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].OBSERVACIONES;
            }
            txtAreaObservaciones.ReadOnly = true;


            // Valida si contiene datos de calculos del ISR
            if ((!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCAUSAISRNull()) && (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CAUSAISR > 0))
            {
                CBCausaISR.Checked = true;
                DivISR.Visible = true;
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IsIMPORTELOCALISRNull())
                    _a = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IMPORTELOCALISR;
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IsIMPORTEFDERALISRNull())
                    _b = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IMPORTEFDERALISR;
                CargarValoresISR(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IDDECLARACION);
                IsrReadOnly((int)DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().CODESTADODECLARACION);
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

    #region Cargar los datos de los actos juridicos
    /// <summary>
    /// Cargar los datos de la parte de Actos Juridicos en Edicion
    /// </summary>
    protected void CagarDatosActosJuridicosEdicion()
    {
        try
        {
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
            {
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo("2") == 0)
                {
                    LblActoJuridicoDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ACTOJURIDICO.ToString();
                    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODPROCEDENCIANull())
                        LblPaisDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PROCEDENCIA.ToString();
                }
                else
                {
                    ddlActoJuridicoDato.SelectedValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR.ToInt().ToString();
                    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODPROCEDENCIANull())
                        ddlPaisDato.SelectedValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODPROCEDENCIA.ToInt().ToString();
                }
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
    /// Cargar los datos de la parte de Actos Juridicos en Visualización
    /// </summary>
    protected void CagarDatosActosJuridicosVisulizacion()
    {
        try
        {
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
            {
                LblActoJuridicoDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ACTOJURIDICO.ToString();
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODPROCEDENCIANull())
                    LblPaisDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PROCEDENCIA.ToString();
            }
            else
            {
                trProcedencia.Visible = false;
                trActoJuridico.Visible = true;
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

    #endregion

    #endregion

    #region Configurar formulario según el tipo

    /// <summary>
    /// Método que configura el formulario según el tipo de declaración
    /// </summary>
    private void ConfigurarFormularioTipo()
    {
        try
        {
            //Por el tipo
            switch (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString())
            {
                case Constantes.PAR_VAL_TIPODECLARACION_JOR:
                    ConfigurarFormularioTipoJornada();
                    break;
                case Constantes.PAR_VAL_TIPODECLARACION_COMPLE:
                    ConfigurarFormularioTipoComplemantaria();
                    break;
                case Constantes.PAR_VAL_TIPODECLARACION_NOR:
                    ConfigurarFormularioTipoNormal();
                    break;
                case Constantes.PAR_VAL_TIPODECLARACION_ANTI:
                    ConfigurarFormularioTipoAnticipada();
                    break;
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
    /// Método que configura el formulario para el tipo Jornada Notarial
    /// </summary>
    private void ConfigurarFormularioTipoJornada()
    {
        //Tipo Jornada Notarial
        DivAvaluo.Visible = false;
        trValorDeuda.Visible = false;
        lblNumPresentacionAnt.Visible = false;
        trRegion.Visible = false;
        trLote.Visible = false;
        //condonacion no visible
        trCondonacionJornada.Visible = false;
        trExencion.Visible = false;
        trReduccion.Visible = false;
        trDisminucion.Visible = false;
        trSubsidio.Visible = false;
        trCondonacion2.Visible = false;
        trCondonacion3.Visible = false;
        trCondonacion1.Visible = false;
        tdlblRegargo.Visible = false;
        tdlblRegargoDato.Visible = false;
        trLugar.Visible = true;

        if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
        {
            lblCuentaDato.Visible = true;
            txtRegion.Visible = false;
            txtManzana.Visible = false;
            txtLote.Visible = false;
            txtUnidadPrivativa.Visible = false;
            btnValidarCuentaCatastral.Visible = false;
            btnBorrarCuenta.Visible = false;
            HabilitarDeshabilitarValorCatastral(false);
        }
        else
        {
            lblCuentaDato.Visible = false;
            txtRegion.Visible = true;
            txtManzana.Visible = true;
            txtLote.Visible = true;
            txtUnidadPrivativa.Visible = true;
            btnBorrarCuenta.Visible = true;
            btnBorrarCuenta.Enabled = false;

        }

        cvValorCatastral.Enabled = false;
        rbExencion.Checked = false;
        rbDisminucion.Checked = false;
        rbReduccion.Checked = false;
        rbSubsidio.Checked = false;
        rbCondonacion.Checked = false;
        BeneficioFiscal_CheckedChanged(rbCondonacion, null);

        //beneficio fiscal no obligatorio
        trBeneficiosJustificacion.Visible = false;
        trBeneficiosJustificacion2.Visible = false;
        this.trDesBeneficios.Visible = false;

        //tasa cero no visible de momento
        trTasaCero.Visible = false;
        rbTasaCero.Checked = false;
        //leyenda sin beneficios activa
        trSinBeneficio.Visible = true;

        //validacion de beneficios desactivada
        this.valBeneficios.Enabled = false;

        imgBeneficiosOff.Visible = false;
        imgBeneficiosOn.Visible = false;
        divBeneficiosFiscales.Visible = true;
        BeneficiosiscalesLeyend.Visible = false;

        rfvRegion.Enabled = true;
        rfvManzana.Enabled = true;
        rfvLote.Enabled = true;
        rfvUnidadPrivativa.Enabled = true;

        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull())
            ConfigurarBeneficiosFiscalesJornadaNotarial(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL);
        else
            ConfigurarBeneficiosFiscalesJornadaNotarial(0);

    }


    /// <summary>
    /// Método que configura el formulario para el tipo Complementaria
    /// </summary>
    private void ConfigurarFormularioTipoComplemantaria()
    {
        //Tipo Complementaria
        lblNumPresentacionAnt.Visible = true;
        lblNumPresentacionAntDato.Visible = true;

        //Se supone que carga los datos de la padré y no puede estar sin participantes
        ucParticipantes.ActualizarEstadoBotones(false);

        //documento
        btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
        btnDocumentos0.Enabled = false;

        btnEditarDocumento.Enabled = true;
        btnEditarDocumento.ImageUrl = Constantes.IMG_MODIFICA;

        btnBorrarDocumento.Enabled = true;
        btnBorrarDocumento.ImageUrl = Constantes.IMG_ELIMINA;

        trCondonacionJornada.Visible = false;

    }


    /// <summary>
    /// Método que configura el formulario para los tipos de declaración Normales 
    /// </summary>
    private void ConfigurarFormularioTipoNormal()
    {
        //Tipo Normal 
        lblNumPresentacionAnt.Visible = false;
        lblNumPresentacionAntDato.Visible = false;
        trCondonacionJornada.Visible = false;
    }


    /// <summary>
    /// Método que configura el formulario para los tipos de declaración Anticipada
    /// </summary>
    private void ConfigurarFormularioTipoAnticipada()
    {
        //Tipo  Anticipada
        lblNumPresentacionAnt.Visible = false;
        lblNumPresentacionAntDato.Visible = false;
        trCondonacionJornada.Visible = false;
        divFechaPreventiva.Visible = true;

        txtFechaCausacion.AutoPostBack = false;
        //Obtener valor catastral
        if (PreviousPage == null && Operacion != Constantes.PAR_VAL_OPERACION_VER && Operacion != Constantes.PAR_VAL_OPERACION_MOD)
            ValidarCuentaCatastral();
    }

    #endregion

    #region Configurar el formulario según el estado

    /// <summary>
    /// Configura el formulario según el estado
    /// </summary>
    protected void ConfigurarFormularioEstado()
    {
        try
        {
            //el div de decision estara oculto por default
            divDecision.Visible = false;
            ConfiguraEstiloImpuestoDeclarado();
            mostrarBotonPago(Convert.ToInt32(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION.ToString()));
            switch ((EstadosDeclaraciones)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION)
            {
                case EstadosDeclaraciones.Borrador:
                    divPago.Visible = false;
                    break;
                case EstadosDeclaraciones.Presentada:
                    btnCalcular.Visible = false;
                    //btnPagoTelematico.Visible = false;
                    lblRealizarPago.Visible = false;
                    btnObtenerLinea.Visible = false;
                    lblObtenerLinea.Visible = false;
                    rvFechaPago.MaximumValue = DateTime.Today.ToShortDateString();
                    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHALINEACAPTURANull())
                        rvFechaPago.MinimumValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();
                    OcultarPanelPago();
                    btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocumentos0.Enabled = false;
                    btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocBeneficios.Enabled = false;
                    if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
                    {
                        PermitirTomaDecision();
                        bntAceptadaDeclaracion.Visible = true;
                    }
                    else
                    {
                        MostrarDecision("No se ha tomado ninguna decisión.");
                    }
                    break;
                case EstadosDeclaraciones.Pendiente:
                    btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocumentos0.Enabled = false;
                    btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocBeneficios.Enabled = false;
                    rvFechaPago.MaximumValue = DateTime.Today.ToShortDateString();
                    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHALINEACAPTURANull())
                        rvFechaPago.MinimumValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();
                    break;
                case EstadosDeclaraciones.Aceptada:
                    btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocumentos0.Enabled = false;
                    btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocBeneficios.Enabled = false;
                    MostrarDecision(Constantes.MENSAJE_DECLARACION_ACEPTADA);
                    OcultarPanelPago();
                    break;
                case EstadosDeclaraciones.PendienteDocumentacion:
                    btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocumentos0.Enabled = false;
                    btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocBeneficios.Enabled = false;
                    MostrarDecision(Constantes.MENSAJE_DECLARACION_PENDIENTE);
                    OcultarPanelPago();
                    break;
                case EstadosDeclaraciones.Inconsistente:
                    btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocumentos0.Enabled = false;
                    btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
                    btnDocBeneficios.Enabled = false;
                    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODMOTIVORECHAZONull())
                        MostrarDecision(Constantes.MENSAJE_DECLARACION_INCONSISTENTE + ": " + DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MOTIVORECHAZO);
                    OcultarPanelPago();
                    break;
                default:
                    btnCalcular.Visible = false;
                    divPago.Visible = true;
                    rvFechaPago.MaximumValue = DateTime.Today.ToShortDateString();
                    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHALINEACAPTURANull())
                        rvFechaPago.MinimumValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();

                    break;
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
    /// Configura el formulario según el estado
    /// </summary>
    protected void ConfigurarFormularioEnPapel()
    {
        try
        {
            if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_INS) != 0)
            {
                //estado de la declaracion si esta en papel o no
                if ((DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTADO.CompareTo(Constantes.PAR_VAL_ESTADODECLARACION_PRESENTADA) != 0) ||
                    (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTADO.CompareTo(Constantes.PAR_VAL_ESTADODECLARACION_ACEPTADA) != 0) ||
                    (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTADO.CompareTo(Constantes.PAR_VAL_ESTADODECLARACION_PDTE_DOCUMENTACION) != 0) ||
                    (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTADO.CompareTo(Constantes.PAR_VAL_ESTADODECLARACION_RECHAZADA) != 0))
                {

                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ENPAPEL.CompareTo(Constantes.PAR_VAL_TRUE) == 0)
                    {
                        btnObtenerLinea.Enabled = false;
                        //add por jamg
                        //22-04-2013
                        btnObtenerLinea.Visible = false;
                        //btnObtenerLinea.ImageUrl = Constantes.IMG_CAMBIAESTADO_DISABLED;
                        lblObtenerLinea.Visible = false;

                        //al ser linea de captura, deberia estar deshabilitado?
                        rfvLineaCaptura.Enabled = true;
                        rfvFechaLineaDato.Enabled = true;
                        cvFechaLineaCaptura.Enabled = true;

                        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHALINEACAPTURANull() && !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsLINEACAPTURANull())
                        {
                            txtLineaDato.Visible = false;
                            txtFechaLineaCaptura.Visible = false;
                            btnFechaLineaCaptura.Visible = false;
                        }
                        else
                        {
                            txtLineaDato.Visible = true;
                            txtFechaLineaCaptura.Visible = true;
                            btnFechaLineaCaptura.Visible = true;
                        }


                        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO == EstadosPago.SinPago.ToDecimal())
                        {
                            //btnPagoTelematico.Enabled = true;
                            //btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO;
                            //btnPagoTelematico.CausesValidation = true;
                            //btnPagoTelematico.ValidationGroup = "LineaCaptura";

                            txtFechaPago.Visible = true;
                            btnFechaPago.Visible = false;//true;

                            // lblInsertarFecha.Visible = true;


                            lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_NOPAGADO;
                        }
                        else if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO == EstadosPago.Pagado.ToDecimal())
                        {
                            //btnPagoTelematico.Enabled = false;
                            //btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO_DISABLED;
                            lblRealizarPago.Visible = false;
                            lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_PENDIENTE;
                        }
                        else
                        {
                            //btnPagoTelematico.Enabled = false;
                            //btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO_DISABLED;
                            lblRealizarPago.Visible = false;
                            lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_RECIBIDO;
                        }
                        mostrarBotonPago(Convert.ToInt32(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION.ToString()));
                        //FUT
                        //cambio error 1925
                        //if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFUTNull() &&
                        //    (DateTime.Now <= DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAVIGENCIALINEACAPTURA))
                        //{
                        //    btnVerFUT.Enabled = true;
                        //    btnVerFUT.ImageUrl = Constantes.IMG_ZOOM;
                        //    string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlFUT"]);
                        //    btnVerFUT.OnClientClick = string.Format("javascript:AbrirUrlFut('{0}','{1}')", url, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FUT);
                        //}
                        //else
                        //{
                        //    btnVerFUT.Enabled = false;
                        //    btnVerFUT.ImageUrl = Constantes.IMG_ZOOM_DISABLED;
                        //}


                        UpdatePanelDatosPresentacion.Update();
                    }
                    //add por jamg
                    //oculta el boton de 
                    if (!btnObtenerLinea.Enabled) //"~/Images/Get_LC2.gif")
                    {

                        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Count > 0 || !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsLINEACAPTURANull())
                        {
                            btnObtenerLinea.Visible = false;
                            //if ((!btnObtener.Enabled || !btnObtener.Visible) && int.Parse(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO.ToString()) != 2)
                            if (!string.IsNullOrEmpty(txtFechaPago.Text))
                                btnObtener.Visible = true;
                            else
                                btnObtener.Visible = false;


                            //JAMG
                            //26- 04- 2013
                            //if ((!PanelObtenerComprobante.Enabled || !PanelObtenerComprobante.Visible) && int.Parse(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO.ToString()) != 2)
                            //    PanelObtenerComprobante.Visible = true;
                            //else
                            //    PanelObtenerComprobante.Visible = false;
                        }

                        btnObtenerLinea.Visible = false;
                        UpdatePanelDatosPresentacion.Update();
                    }
                    //UpdatePanelDatosPresentacion.Update();
                    //--termina jamg
                }


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

    #region Métodos del Checked

    /// <summary>
    /// Maneja el evento CheckedChanged de los radio buttons de beneficios fiscales
    /// Dependiendo de cual se seleccione se habilitan unos controles u otros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BeneficioFiscal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) != 0)
            {
                this.valBeneficios.ErrorMessage = "Seleccione un beneficio fiscal.";
                this.txtValBeneficio.Text = "";
                this.trBeneficiosJustificacion.Visible = true;
                this.trBeneficiosJustificacion2.Visible = true;
                this.trDesBeneficios.Visible = true;
                BeneficiosiscalesLeyend.Visible = false;
                switch (((RadioButton)sender).ID)
                {
                    case "rbExencion":
                        ValoresPorDefectoCombos();
                        ucReduccion.Editable = false;
                        ucExencion.Editable = true;
                        txtPorcentajeDisminucíon.Enabled = false;
                        txtPorcentajeSubsidio.Enabled = false;
                        txtPorcentajeCondonacion.Enabled = false;
                        txtFechaCondonacion.Enabled = false;
                        txtMotivoCondonacion.Enabled = false;
                        trSinBeneficio.Visible = false;
                        txtValBeneficio.Text = "Continuar";
                        this.imgBeneficiosOff.Enabled = true;
                        this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
                        rfvDocDig.Enabled = true;
                        break;
                    case "rbSubsidio":
                        ValoresPorDefectoCombos();
                        ucReduccion.Editable = false;
                        ucExencion.Editable = false;
                        txtPorcentajeDisminucíon.Enabled = false;
                        txtPorcentajeSubsidio.Enabled = true;
                        txtPorcentajeCondonacion.Enabled = false;
                        txtFechaCondonacion.Enabled = false;
                        txtMotivoCondonacion.Enabled = false;
                        trSinBeneficio.Visible = false;
                        this.imgBeneficiosOff.Enabled = true;
                        this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
                        rfvDocDig.Enabled = true;
                        this.valBeneficios.ErrorMessage = Constantes.MSJ_ERROR_ANADIRDOCJUSTI;
                        break;
                    case "rbReduccion":
                        ValoresPorDefectoCombos();
                        ucReduccion.Editable = true;
                        ucExencion.Editable = false;
                        txtPorcentajeDisminucíon.Enabled = false;
                        txtPorcentajeSubsidio.Enabled = false;
                        txtPorcentajeCondonacion.Enabled = false;
                        txtFechaCondonacion.Enabled = false;
                        txtMotivoCondonacion.Enabled = false;
                        trSinBeneficio.Visible = false;
                        txtValBeneficio.Text = "Continuar";
                        this.imgBeneficiosOff.Enabled = true;
                        this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
                        rfvDocDig.Enabled = false;
                        break;
                    case "rbDisminucion":
                        ValoresPorDefectoCombos();
                        ucReduccion.Editable = false;
                        ucExencion.Editable = false;
                        txtPorcentajeDisminucíon.Enabled = true;
                        txtPorcentajeSubsidio.Enabled = false;
                        txtPorcentajeCondonacion.Enabled = false;
                        txtFechaCondonacion.Enabled = false;
                        trSinBeneficio.Visible = false;
                        txtMotivoCondonacion.Enabled = false;
                        this.valBeneficios.ErrorMessage = Constantes.MSJ_ERROR_ANADIRDOCJUSTI;
                        this.imgBeneficiosOff.Enabled = true;
                        this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
                        rfvDocDig.Enabled = true;
                        break;
                    case "rbCondonacion":
                        ValoresPorDefectoCombos();
                        ucReduccion.Editable = false;
                        ucExencion.Editable = false;
                        txtPorcentajeDisminucíon.Enabled = false;
                        txtPorcentajeSubsidio.Enabled = false;
                        txtPorcentajeCondonacion.Enabled = true;
                        txtFechaCondonacion.Enabled = true;
                        txtMotivoCondonacion.Enabled = true;
                        this.imgBeneficiosOff.Enabled = true;
                        this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
                        trSinBeneficio.Visible = false;
                        rfvDocDig.Enabled = false;
                        break;
                    case "rbTasaCero":
                        ValoresPorDefectoCombos();
                        ucReduccion.Editable = false;
                        ucExencion.Editable = false;
                        txtPorcentajeDisminucíon.Enabled = false;
                        txtPorcentajeSubsidio.Enabled = false;
                        txtPorcentajeCondonacion.Enabled = false;
                        txtFechaCondonacion.Enabled = false;
                        txtMotivoCondonacion.Enabled = false;
                        trSinBeneficio.Visible = false;
                        txtValBeneficio.Text = "Continuar";
                        this.imgBeneficiosOff.Enabled = true;
                        this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
                        rfvDocDig.Enabled = true;
                        break;
                    default:
                        break;
                }
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

    #region Eventos de los TextBox

    /// <summary>
    /// Maneja el evento TextChanged del control txtValorCatastral
    /// Este evento se lanza cuando se ha obtenido el valor catastral
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtValorCatastral_TextChanged(object sender, EventArgs e)
    {
        try
        {
            this.Validate("DeclaracionNormal");
            if (this.IsValid)
            {
                //guardar el valor de catastral
                if (ExisteValorCatastral())
                {
                    string expresion = "[^0-9.]";
                    if (Regex.IsMatch(ObtenerValorCatastral().ToString(), expresion))
                    {
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORCATASTRALNull();
                    }
                    else
                    {
                        hidValorCatastral.Value = ObtenerValorCatastral();
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL = Convert.ToDecimal(ObtenerValorCatastral());
                        //configurar tasa cero
                        ConfigurarTasaCero();
                        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_JOR))
                        {
                            ConfigurarBeneficiosFiscalesJornadaNotarial(Convert.ToDecimal(ObtenerValorCatastral()));
                            divBeneficiosFiscales.Visible = true;
                        }
                        if (divBeneficiosFiscales.Visible == false)
                        {
                            imgBeneficiosOn.Enabled = true;
                            imgBeneficiosOn.ImageUrl = Constantes.IMG_ANADIR;
                        }
                    }
                }
                else
                {
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORCATASTRALNull();
                    ConfigurarTasaCero();
                }
            }
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
            {
                string script2 = string.Format("cambiarFoco('{0}');", btnCalcular.ClientID);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Enfocar2", script2, true);

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
    /// Este evento se lanza cuando se cierra la ventana de 
    /// EspecificarInformacionDocumentos.aspx y el valor del
    /// Textbox ha cambiado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtDescripcion_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlDocumental"]);

            XmlDocumental = AnadirIdUsuario(Utilidades.base64Decode(txtDocDigital.Text));
            TipoDocumental = txtTipoDocumento.Text;
            string TipoDocumentalCorto = null;
            switch (TipoDocumental)
            {
                case "EscrituraPublica":
                    TipoDocumentalCorto = "EP";
                    break;
                case "ResolucionJudicial":
                    TipoDocumentalCorto = "RJ";
                    break;
                case "DocumentoPrivado":
                    TipoDocumentalCorto = "DP";
                    break;
                case "DocumentoAdministrativo":
                    TipoDocumentalCorto = "DA";
                    break;
            }

            ListaIdsDocumental = txtListaIdsFicheros.Text;

            //obtenemos la fecha escritura del documento digital que acabamos de insertar
            ObtenerFechaEscrituraDocumento();
            //Cargo el tipo de formulario
            CargarDDLExeccion();
            CargarDDLReduccion();
            btnDocumentos0.Enabled = false;
            btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
            btnEditarDocumento.Enabled = true;
            btnEditarDocumento.ImageUrl = Constantes.IMG_MODIFICA;
            //Registrar onclick de editar
            btnEditarDocumento.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                                  url,
                                  "1",
                                  TipoDocumentalCorto,
                                  null,
                                  'S',
                                  txtDocDigital.ClientID,
                                  txtTipoDocumento.ClientID,
                                  txtListaIdsFicheros.ClientID,
                                  "S", txtDocDigital.Text, ListaIdsDocumental, null, (Usuarios.IdPersona() == "-1") ? null : Usuarios.IdPersona(), "DeclaracionISAI");

            btnVerDoc.Enabled = false;
            btnVerDoc.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
            btnBorrarDocumento.Enabled = true;
            btnBorrarDocumento.ImageUrl = Constantes.IMG_ELIMINA;
            cargarDescripcionDoc(XmlDocumental, null);
            txtIdDoc.Text = TipoDocumentalCorto;
            //Limpiar el contenido para que se lance el evento la si se modifica el documento pero no la descripción
            txtDocDigital.Text = string.Empty;

            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Any())
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IsIDDOCUMENTODIGITALNull())
                {
                    ClienteDeclaracionIsai.EspecificarFicheroTransaccional(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IDDOCUMENTODIGITAL, ListaIdsDocumental);

                    btnVerDoc.Enabled = true;
                    btnVerDoc.ImageUrl = Constantes.IMG_CONSULTA;
                    btnVerDoc.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                        url,
                        "1",
                        null,
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL,
                        'S',
                        null,
                        null,
                        null,
                        "N", null, null, null, (Usuarios.IdPersona() == "-1") ? null : Usuarios.IdPersona(), "DeclaracionISAI");

                }
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
    /// Método que añade a un xml el elemento "IDUSUARIO" con el valor del id Usuario
    /// </summary>
    /// <param name="xml">XML al que añadir el elemento</param>
    /// <returns>XML resultante</returns>
    private string AnadirIdUsuario(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        XmlNode root = doc.DocumentElement.LastChild;
        XmlElement elem = doc.CreateElement("IDUSUARIO");

        elem.InnerText = Usuarios.IdUsuario();
        root.AppendChild(elem);

        // Now create StringWriter object to get data from xml document.
        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        doc.WriteTo(xw);
        return sw.ToString();
    }



    /// <summary>
    /// Este evento se lanza cuando se cierra la ventana de 
    /// EspecificarInformacionDocumentos.aspx y el valor del
    /// Textbox ha cambiado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtDescripcionBF_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlDocumental"]);

            XmlDocumentalBF = AnadirIdUsuario(Utilidades.base64Decode(txtDocDigitalBF.Text));
            TipoDocumentalBF = txtTipoDocumentoBF.Text;
            ListaIdsDocumentalBF = txtListaIdsFicherosBF.Text;
            btnDocBeneficios.Enabled = false;
            btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
            btnEditarDocBeneficios.Enabled = true;
            btnEditarDocBeneficios.ImageUrl = Constantes.IMG_MODIFICA;

            btnEditarDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                                  url,
                                  "99",
                                  null,
                                  null,
                                  Constantes.PAR_VAL_TRUE,
                                  txtDocDigitalBF.ClientID,
                                  txtTipoDocumentoBF.ClientID,
                                  txtListaIdsFicherosBF.ClientID,
                                  Constantes.PAR_VAL_TRUE, txtDocDigitalBF.Text, ListaIdsDocumentalBF, null, (Usuarios.IdPersona() == "-1") ? null : Usuarios.IdPersona(), "DeclaracionISAI");

            btnVerDocBeneficios.Enabled = false;
            btnVerDocBeneficios.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
            btnEliminarDocBeneficios.Enabled = true;
            btnEliminarDocBeneficios.ImageUrl = Constantes.IMG_ELIMINA;


            if (txtDocDigitalBF.Text.Trim().Length > 0)
                txtValBeneficio.Text = "Continuar";

            cargarDescripcionDoc(XmlDocumentalBF, "BF");
            txtIdDocDig.Text = TipoDocumentalBF;
            //Limpiar el contenido para que se lance el evento la si se modifica el documento pero no la descripción
            txtDocDigitalBF.Text = string.Empty;

            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Any())
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IsIDDOCBENFISCALESNull())
                {
                    ClienteDeclaracionIsai.EspecificarFicheroTransaccional(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IDDOCBENFISCALES, ListaIdsDocumentalBF);

                    btnVerDocBeneficios.Enabled = true;
                    btnVerDocBeneficios.ImageUrl = Constantes.IMG_CONSULTA;

                    btnVerDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                           url,
                           "99",
                           null,
                           DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IDDOCBENFISCALES,
                           'S',
                           null,
                           null,
                           null,
                           "N", null, null, null, (Usuarios.IdPersona() == "-1") ? null : Usuarios.IdPersona(), "DeclaracionISAI");
                }
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
    /// Método que extrae de un xml el valor del elemento DESCRIPCION y se lo establece al label correspondiente
    /// </summary>
    /// <param name="xmlDocu">xml de donde extraer la información</param>
    /// <param name="label">Parámetro para saber a que documento corresponde la descripción</param>
    private void cargarDescripcionDoc(string xmlDocu, string label)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xmlDocu);
        XmlNodeList docu = xDoc.GetElementsByTagName("DESCRIPCION");


        if (label == "BF")
        {
            if (!string.IsNullOrEmpty(docu[0].InnerText))
            {
                lblDescripcionDocBF.Text = (docu[0].InnerText.Length > Constantes.LongitudLineaDescripcion) ? docu[0].InnerText.Substring(0, Constantes.LongitudLineaDescripcion - 20) + "..." : docu[0].InnerText + " - Fecha:  " + Convert.ToDateTime(xDoc.GetElementsByTagName("FECHA")[0].InnerText).ToShortDateString();
                DescripBF = (docu[0].InnerText.Length > Constantes.LongitudLineaDescripcion) ? docu[0].InnerText.Substring(0, Constantes.LongitudLineaDescripcion - 20) + "..." : docu[0].InnerText + " - Fecha:  " + Convert.ToDateTime(xDoc.GetElementsByTagName("FECHA")[0].InnerText).ToShortDateString();
            }
            else
            {
                lblDescripcionDocBF.Text = "Beneficio fiscal - Fecha:  " + Convert.ToDateTime(xDoc.GetElementsByTagName("FECHA")[0].InnerText).ToShortDateString();
                DescripBF = "Beneficio fiscal - Fecha: " + Convert.ToDateTime(xDoc.GetElementsByTagName("FECHA")[0].InnerText).ToShortDateString();
            }

        }
        else
        {
            lblDescripcionDoc.Text = TipoDocumental.ToString() + " - Fecha: " + Convert.ToDateTime(xDoc.GetElementsByTagName("FECHA")[0].InnerText).ToShortDateString();
            Descrip = TipoDocumental.ToString() + " - Fecha: " + Convert.ToDateTime(xDoc.GetElementsByTagName("FECHA")[0].InnerText).ToShortDateString();

        }

    }

    /// <summary>
    /// Función que decodfica en base64
    /// </summary>
    /// <param name="data">Cadena con los datos que queremos descifrar</param>
    /// <returns>Cadena que contendra el xml</returns>
    public static string bbase64Decode(string data)
    {
        try
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        catch (Exception e)
        {
            ExceptionPolicyWrapper.HandleException(e);
            throw new Exception("Error en base64Decode" + e.Message);
        }
    }


    /// <summary>
    /// Obtenemos la fecha escritura por el iddocumento digital llamando al servicio de documental
    /// </summary>
    private void ObtenerFechaEscrituraDocumento()
    {
        try
        {
            switch (TipoDocumental)
            {
                case "EscrituraPublica":
                    //Recuperar tabla
                    dseDocumentosDigitales.DOC_DOCUMENTONOTARIALDataTable docNotarioDT = new dseDocumentosDigitales.DOC_DOCUMENTONOTARIALDataTable();
                    docNotarioDT.ReadXml(new StringReader(XmlDocumental));
                    if (docNotarioDT.Any())
                        lblFechaEscrituraDato.Text = docNotarioDT[0].FECHA.ToShortDateString();
                    break;
                case "ResolucionJudicial":
                    dseDocumentosDigitales.DOC_RESOLUCIONJUDICIALDataTable docResolucionJudicialDT = new dseDocumentosDigitales.DOC_RESOLUCIONJUDICIALDataTable();
                    docResolucionJudicialDT.ReadXml(new StringReader(XmlDocumental));
                    if (docResolucionJudicialDT.Any())
                        lblFechaEscrituraDato.Text = docResolucionJudicialDT[0].FECHA.ToShortDateString();
                    break;
                case "DocumentoPrivado":
                    dseDocumentosDigitales.DOC_DOCUMENTOJURIDICODataTable docJuridicoDT = new dseDocumentosDigitales.DOC_DOCUMENTOJURIDICODataTable();
                    docJuridicoDT.ReadXml(new StringReader(XmlDocumental)); ;
                    if (docJuridicoDT.Any())
                        lblFechaEscrituraDato.Text = docJuridicoDT[0].FECHA.ToShortDateString();
                    break;
                case "DocumentoAdministrativo":
                    dseDocumentosDigitales.DOC_DOCUMENTOADMINISTRATIVODataTable docAdministrativoDT = new dseDocumentosDigitales.DOC_DOCUMENTOADMINISTRATIVODataTable();
                    docAdministrativoDT.ReadXml(new StringReader(XmlDocumental));
                    if (docAdministrativoDT.Any())
                        lblFechaEscrituraDato.Text = docAdministrativoDT[0].FECHA.ToShortDateString();
                    break;
            }
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA = Convert.ToDateTime(lblFechaEscrituraDato.Text);

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
    /// Maneja el evento CargarControl del control de usuario Participantes
    /// Guuarda los datos ya introducidos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void participantes(object sender, EventArgs e)
    {
        try
        {
            //No se puede acceder a la pantalla de personas si no se ha introducido una cuenta catastral (La cuenta catastral es un campo obligatorio)
            //ucParticipantes.ExisteCuenta = IsaiManager.ComprobarExisteCuentaCatastral(lblRegionDato.Text.Trim(), lblManzanaDato.Text.Trim(), lblLoteDato.Text.Trim(), lblUnidadPrivativaDato.Text.Trim());

            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TipoDeclaracion.JornadaNotarial.ToDecimal())
            {
                ucParticipantes.ExisteCuenta = IsaiManager.ComprobarExisteCuentaCatastral(txtRegion.Text.Trim(), txtManzana.Text.Trim(), txtLote.Text.Trim(), txtUnidadPrivativa.Text.Trim());
            }
            else
            {
                ucParticipantes.ExisteCuenta = true;
            }

            if (ViewState["existeInmueble"] == null)
            {
                if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) != 0)
                {
                    //En el caso de estar dando de alta una declaración, antes de insertar participantes hay que persistir la declaracion
                    CrearDeclaracionDTFormulario(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION == Constantes.PAR_VAL_ESTADODECLARACION_BORR.ToInt());
                    //REALIZADO: TODAVIA NO GUARDO LA DECLARACION EN LA BASE DE DATOS
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_COMPLE) == 0)
                    {
                        if (!string.IsNullOrEmpty(lblRegionDato.Text))
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION = lblRegionDato.Text.Trim();
                        else DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION = string.Empty;

                        if (!string.IsNullOrEmpty(lblManzanaDato.Text))
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA = lblManzanaDato.Text.Trim();
                        else DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA = string.Empty;

                        if (!string.IsNullOrEmpty(lblLoteDato.Text))
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE = lblLoteDato.Text.Trim();
                        else DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE = string.Empty;

                        if (!string.IsNullOrEmpty(lblUnidadPrivativaDato.Text))
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA = lblUnidadPrivativaDato.Text.Trim();
                        else DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA = string.Empty;
                    }
                    Operacion = Constantes.PAR_VAL_OPERACION_MOD;
                    OperacionParticipantes = Constantes.PAR_VAL_OPERACION_MOD;
                }

                if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0 ||
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_COMPLE) == 0 ||
                    (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION > 0))
                {
                    if (!string.IsNullOrEmpty(ddlActoJuridicoDato.SelectedValue.ToString()))
                    {
                        CodActoJuridicoParticipante = ddlActoJuridicoDato.SelectedValue.ToInt();
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR = CodActoJuridicoParticipante;
                    }
                    if (!string.IsNullOrEmpty(ddlPaisDato.SelectedValue.ToString()))
                    {
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODPROCEDENCIA = ddlPaisDato.SelectedValue.ToInt();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(ddlActoJuridicoDato.SelectedValue.ToString()))
                    {
                        CodActoJuridicoParticipante = ddlActoJuridicoDato.SelectedValue.ToInt();
                    }
                }
            }

        }
        catch (FaultException<DeclaracionIsaiInfoException> diex)
        {
            MostrarInfoGuardar(diex.Detail.Descripcion, true);
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
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
    /// Cálculo del impuesto, pero primero validamos que todos los datos son correctos según el tipo
    /// de declaración que estemos realizando y después actualizamos el cálculo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCalcular_Click(object sender, ImageClickEventArgs e)
    {
        DateTime fecha;
        try
        {
            string mensaje = string.Empty;

            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) != 0)
            {



                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_ANTI)
                {
                    if (!DateTime.TryParse(txtFechaCausacion.Text, out fecha))
                        mensaje += "<LI>Fecha causación.";
                    else if (DateTime.Compare(fecha, Convert.ToDateTime(ConfigurationManager.AppSettings["FechaMinDecla"])) < 0)
                        mensaje += "<LI>La aplicación solamente permite hacer declaraciones de ISAI con fecha igual o posterior a 01/01/1972";
                }

                if (!ExisteValorCatastral())
                    mensaje += "<LI>Valor catastral.";
                if (string.IsNullOrEmpty(mensaje))
                {
                    ActualizarDatosCalculo(true);
                }
                else
                {
                    ModalInfoCalculo.TextoInformacion = "Faltan los siguientes valores:" + mensaje;
                    ModalInfoCalculo.TipoMensaje = true;
                    extenderPnlInfoCalculoImpuestoModal.Show();
                }
            }
            else
            {


                if (!ExisteValorCatastral())
                    mensaje += "<LI>Valor catastral.";
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_ANTI)
                {
                    if (!DateTime.TryParse(txtFechaCausacion.Text, out fecha))
                        mensaje += "<LI>Fecha causación.";
                    else if (DateTime.Compare(fecha, new DateTime(1972, 01, 01)) < 0)
                        mensaje += "<LI>La aplicación solamente permite hacer declaraciones de ISAI con fecha igual o posterior a 01/01/1972";
                }

                if (string.IsNullOrEmpty(mensaje))
                {
                    ActualizarDatosCalculo(true);
                }
                else
                {
                    ModalInfoCalculo.TextoInformacion = "Faltan los siguientes valores:" + mensaje;
                    ModalInfoCalculo.TipoMensaje = true;
                    extenderPnlInfoCalculoImpuestoModal.Show();
                }
            }

            ConfiguraEstiloImpuestoDeclarado();
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
    /// Validamos el número de cuenta catastral después habilitamos los botones de Participante y 
    /// alta de una declaración (borrador o pendiente presentar).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnValidarCuentaCatastral_Click(object sender, EventArgs e)
    {
        try
        {
            ValidarCuentaCatastral();
            ucParticipantes.ExisteCuenta = ExisteCuenta();
            btnBorrarCuenta.Enabled = true;
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
    /// LLamamos al servicio de ISAI y cambiamos el estado de la declaración a ACEPTADA
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bntAceptadaDeclaracion_Click(object sender, EventArgs e)
    {
        try
        {
            if (rbDecision.SelectedIndex == 0)
                ClienteDeclaracionIsai.AceptarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
            else if (rbDecision.SelectedIndex == 1)
                ClienteDeclaracionIsai.RechazarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION, (decimal?)Convert.ToDecimal(ddlMotivoRechazo.SelectedValue));
            else if (rbDecision.SelectedIndex == 2)
                ClienteDeclaracionIsai.RechazarDeclaracionPdteDocumentacion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);

            try
            {
                bool error_ = true;
                string error = "";
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDPERSONANull())
                {
                    DseInfoNotarios dsNotarios = ClienteRcon.GetInfoNotario(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDPERSONA, false);
                    if (dsNotarios.Notario.Any())
                    {
                        string nombre = "";
                        string numDec = "";
                        string tipoDecision = "";
                        string email = "";
                        if (!dsNotarios.Notario[0].IsNOMBRENull())
                            nombre = dsNotarios.Notario[0].NOMBRE + " ";
                        if (!dsNotarios.Notario[0].IsAPELLIDOPATERNONull())
                            nombre += dsNotarios.Notario[0].APELLIDOPATERNO + " ";
                        if (!dsNotarios.Notario[0].IsAPELLIDOMATERNONull())
                            nombre += dsNotarios.Notario[0].APELLIDOMATERNO;
                        if (!dsNotarios.Notario[0].IsEMAILNull())
                            email = dsNotarios.Notario[0].EMAIL;

                        if (rbDecision.SelectedIndex == 1)
                            tipoDecision = ddlMotivoRechazo.SelectedItem.Text;
                        else
                            tipoDecision = rbDecision.SelectedItem.Text;

                        numDec = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACION.ToString();

                        if (email != "")
                        {
                            ListDictionary lstReemplazos = new ListDictionary();
                            lstReemplazos.Add("<%NombreCompleto%>", nombre);
                            lstReemplazos.Add("<%NumDec%>", numDec);
                            lstReemplazos.Add("<%TipoDecision%>", tipoDecision);
                            string rutaTemplate = ConfigurationManager.AppSettings["rutaTemplateEmail"] + "/Template.htm";

                            SIGAPred.Common.Email.EmailUtils.DatosEmail datos = new SIGAPred.Common.Email.EmailUtils.DatosEmail();
                            datos.From = ConfigurationManager.AppSettings["fromEmail"];
                            datos.To = email;
                            datos.CC = "";
                            datos.Subject = "Decisión declaración " + numDec;

                            if (SIGAPred.Common.Email.EmailUtils.SendEmail(rutaTemplate, lstReemplazos, datos, out error))
                            {
                                error_ = false;
                                ModalInfoCorreo.TextoInformacion = "Se ha enviado un correo al notario notificando la decisión tomada sobre la declaración.";
                                informar_ModalPopupExtender.Show();
                            }

                            datos.Subject = String.Format("MAIL INFO: {0}", datos.Subject);
                            string[] mails = ConfigurationManager.AppSettings["MailsNotificacionesFinanzas"].ToString().Split(',');
                            foreach (string mail in mails)
                            {
                                datos.To = mail;
                                if (SIGAPred.Common.Email.EmailUtils.SendEmail(rutaTemplate, lstReemplazos, datos, out error))
                                {
                                    error_ = false;
                                }
                            }
                        }
                        else
                        {
                            error_ = false;
                            ModalInfoCorreo.TextoInformacion = "No se ha enviado correo al notario por no disponer de la dirección de correo electrónico.";
                            informar_ModalPopupExtender.Show();
                        }
                    }
                }
                else
                {
                    error_ = false;
                    RedirectUtil.BaseURL = PaginaOrigen;

                    RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO.ToString());
                    ActualizarParametrosUrlBusqueda();
                }
                if (error_)
                {
                    if (error != "")
                        ModalInfoCorreo.TextoInformacion = "No se ha enviado un correo al notario.\nError: " + error;
                    else
                        ModalInfoCorreo.TextoInformacion = "Error en el envio de correo al notario.";
                    informar_ModalPopupExtender.Show();
                }
            }
            catch (Exception ex)
            {
                ModalInfoCorreo.TextoInformacion = "No se ha enviado un correo al notario.\nError: " + ex.Message;
                informar_ModalPopupExtender.Show();
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
    /// Maneja el evento ConfirmClick de la pantalla modal del envio de correo
    /// Redirecciona a la página a la bandeja
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void informar_ConfirmClick(object sender, CancelEventArgs e)
    {
        try
        {
            informar_ModalPopupExtender.Hide();
            RedirectUtil.BaseURL = PaginaOrigen;
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDAVALUONull())
                RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO.ToString());
            ActualizarParametrosUrlBusqueda();
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
    /// Maneja el evento Click de btnBorrarCuenta
    /// Elimina y pone por defecto todos los datos y controles de la cuenta catastral
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBorrarCuenta_Click(object sender, EventArgs e)
    {
        txtRegion.Text = string.Empty;
        txtManzana.Text = string.Empty;
        txtLote.Text = string.Empty;
        txtUnidadPrivativa.Text = string.Empty;
        BorrarValorCatastral();
        HabilitarDeshabilitarValorCatastral(false);
        btnBorrarCuenta.Enabled = false;
    }

    private void EnviarCorreo(DseDeclaracionIsai DseDeclaracion)
    {
        #region declaracion
        bool error_ = false;
        string error = "";
        string lineacaptura = "";
        string cuenta = "";
        string fechacausacion = "";
        string fechaescritura = "";
        string fechalineacaptura = "";
        string idavaluo = "";
        string iddeclaracion = "";
        string idpersona = "";
        #endregion


        if (!DseDeclaracion.FEXNOT_DECLARACION[0].IsLINEACAPTURANull())
            lineacaptura = DseDeclaracion.FEXNOT_DECLARACION[0].LINEACAPTURA.ToString();
        if (!DseDeclaracion.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
            fechacausacion = DseDeclaracion.FEXNOT_DECLARACION[0].FECHACAUSACION.ToString();
        if (!DseDeclaracion.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
            fechaescritura = DseDeclaracion.FEXNOT_DECLARACION[0].FECHAESCRITURA.ToString();
        if (!DseDeclaracion.FEXNOT_DECLARACION[0].IsFECHALINEACAPTURANull())
            fechalineacaptura = DseDeclaracion.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToString();
        if (!DseDeclaracion.FEXNOT_DECLARACION[0].IsIDAVALUONull())
            idavaluo = DseDeclaracion.FEXNOT_DECLARACION[0].IDAVALUO.ToString();
        if (!DseDeclaracion.FEXNOT_DECLARACION[0].IsIDPERSONANull())
            idpersona = DseDeclaracion.FEXNOT_DECLARACION[0].IDPERSONA.ToString();
        cuenta = string.Format("Region: {0}, Manzana: {1}, Lote: {2}, UnidadPrivativa: {3}",
            DseDeclaracion.FEXNOT_DECLARACION[0].REGION,
            DseDeclaracion.FEXNOT_DECLARACION[0].MANZANA,
            DseDeclaracion.FEXNOT_DECLARACION[0].LOTE,
            DseDeclaracion.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA);
        iddeclaracion = DseDeclaracion.FEXNOT_DECLARACION[0].IDDECLARACION.ToString();

        ListDictionary lstReemplazos = new ListDictionary();
        lstReemplazos.Add("<%LineaCaptura%>", lineacaptura);
        lstReemplazos.Add("<%Cuenta%>", cuenta);
        lstReemplazos.Add("<%FechaCausacion%>", fechacausacion);
        lstReemplazos.Add("<%FechaEscritura%>", fechaescritura);
        lstReemplazos.Add("<%FechaLineaCaptura%>", fechalineacaptura);
        lstReemplazos.Add("<%IdAvaluo%>", idavaluo);
        lstReemplazos.Add("<%iddeclaracion%>", iddeclaracion);
        lstReemplazos.Add("<%idpersona%>", idpersona);
        string rutaTemplate = ConfigurationManager.AppSettings["rutaTemplateEmail"] + "/TemplateLinea.htm";

        SIGAPred.Common.Email.EmailUtils.DatosEmail datos = new SIGAPred.Common.Email.EmailUtils.DatosEmail();
        datos.From = ConfigurationManager.AppSettings["fromEmail"];
        datos.CC = "";
        datos.Subject = string.Format("MAILINFO ISAI: GENERACION LINEA CAPTURA {0}", lineacaptura);

        string[] mails = ConfigurationManager.AppSettings["MailsNotificacionesFinanzas"].ToString().Split(',');
        foreach (string mail in mails)
        {
            datos.To = mail;
            if (SIGAPred.Common.Email.EmailUtils.SendEmail(rutaTemplate, lstReemplazos, datos, out error))
            {
                error_ = false;
            }
        }

    }



    /// <summary>
    /// Obtenemos la línea de captura para poder realizar el pago de la declaración 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnObtenerLinea_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //servicio web para la generacion de la linea de captura para el impuesto sobre adquisicion de inmuebles(ISAI).
            string fechaCausacion = string.Empty;
            string ErrorMessage = null;
            DseDeclaracionIsai declaracion = DseDeclaracionIsaiMant;

            fechaCausacion = string.Format("{0}-{1}-{2}", Convert.ToDateTime(txtFechaCausacion.Text).Year,
                Convert.ToDateTime(txtFechaCausacion.Text).Month,
                Convert.ToDateTime(txtFechaCausacion.Text).Day);
            LineaCapturaResultMessage result = IsaiManager.ObtenerLineaCaptura(ref declaracion, fechaCausacion, out ErrorMessage);
            DseDeclaracionIsaiMant = declaracion;

            //mensaje de errordel servicio FUT
            if (result == LineaCapturaResultMessage.Exito)
            {
                btnObtenerLinea.Enabled = true;

                lblLineaDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA;
                lblFechaLineaDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();

                //JAMG
                //VALIDO SI YA TRAE BIEN LA LINEA DE CAPTURA
                //QUITO EL BOTN DE GENERAR LINEA DE CAPTURA
                //22-04-2013
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA.Count() >= 1 || DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA != null)
                {
                    btnObtenerLinea.Visible = false;
                    if (!btnObtener.Enabled || !btnObtener.Visible)
                        btnObtener.Visible = true;
                }
                //-----------------------------------

                //recargamos la declaracion
                RecargarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
                CargarDatosFormulario();
                btnObtenerLinea.Enabled = true;
                //btnObtenerLinea.ImageUrl = Constantes.IMG_CAMBIAESTADO_DISABLED;
                lblObtenerLinea.Visible = false;

                rvFechaPago.MaximumValue = DateTime.Today.ToShortDateString();
                rvFechaPago.MinimumValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();

                //FUT 
                //cambio para el error 1925
                //string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlFUT"]);
                //btnVerFUT.Enabled = true;
                //btnVerFUT.ImageUrl = Constantes.IMG_ZOOM;
                //btnVerFUT.OnClientClick = string.Format("javascript:AbrirUrlFut('{0}','{1}')", url, declaracion.FEXNOT_DECLARACION[0].FUT);
                //UpdatePanelDatosPresentacion.Update();
            }
            else
            {
                btnObtenerLinea.Enabled = false;

                if (result == LineaCapturaResultMessage.ImpuestoEsCero && DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(TipoDeclaracion.Complementaria))
                    ModalInfoFUT.TextoInformacion = Constantes.MSJ_ERROR_SOLLINEA;
                else
                {
                    if (String.IsNullOrEmpty(ErrorMessage))
                        ModalInfoFUT.TextoInformacion = EnumUtility.GetDescription(result);
                    else
                        ModalInfoFUT.TextoInformacion = ErrorMessage;
                }
                ModalInfoFUT.TipoMensaje = true;
                extenderPnlInfoFUTModal.Show();
            }
        }
        catch (FaultException<DeclaracionIsaiException> diex)
        {
            ModalInfoFUT.TextoInformacion = diex.InnerException != null ? diex.InnerException.Message : diex.Message;
            ModalInfoFUT.TipoMensaje = true;
            extenderPnlInfoFUTModal.Show();
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            ModalInfoFUT.TextoInformacion = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ModalInfoFUT.TipoMensaje = true;
            extenderPnlInfoFUTModal.Show();
        }
    }



    /// <summary>
    /// Método que encripta en MD5
    /// </summary>
    /// <param name="str">Cadena a encriptar</param>
    /// <returns></returns>
    public string GetMD5(string str)
    {
        //Creamos los sistemas de codificación
        MD5 md5 = MD5CryptoServiceProvider.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();

        //Creamos las cadenas para ir almacenando la codificación
        StringBuilder sb = new StringBuilder();
        byte[] stream = null;

        //Creamos el Hash en un array de bytes
        stream = md5.ComputeHash(encoding.GetBytes(str));

        //Añadimos, cada byte, al StringBuilder con el formato adecuado
        for (int i = 0; i < stream.Length; i++)
            sb.AppendFormat("{0:x2}", stream[i]);

        return sb.ToString();
    }


    /// <summary>
    /// Método que carga y prepara los datos para el pago telemático
    /// </summary>
    private void enviarDatosBanco(string url)
    {
        try
        {
            registrarPago();

            string urlBanco = url;
            string c_referencia = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA;
            string t_importe = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO.ToString("0.00");
            //string val_2 = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NOMBRESUJETO + " " + (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsAPELLIDOPATERNOSUJETONull() ? "" : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].APELLIDOPATERNOSUJETO);
            string s_transm = "00000000000000000425";

            string cuenta = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION + DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA + DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE + DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;
            string val_7 = "concepto|predial|cuenta|" + cuenta + "|bimestre|" + DateTime.Now.Year.ToString() + System.Math.Round(DateTime.Now.Month.ToDecimal() / 2).ToString();

            string nombre = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SUJETO.Split(',')[1] + ' ' + DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SUJETO.Split(',')[0];
            //TODO:Que participante?
            DseInfoDirecciones.DireccionDataTable dtInfoDir = ClienteRcon.GetDireccionById(((ServiceDeclaracionIsai.DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow)(DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.First())).IDDIRECCION.ToDecimal()).Direccion;
            string calle = dtInfoDir[0].VIA;
            string numero = dtInfoDir[0].NUMEROEXTERIOR;
            string colonia = dtInfoDir[0].COLONIA;
            string val_8 = "Nombre|" + nombre + "|calle|" + calle + "|numero|" + numero + "|colonia|" + colonia;

            string val_11 = ConfigurationManager.AppSettings["EmailPagoEnLinea"].ToString();
            string val_12 = ConfigurationManager.AppSettings["TelefonoPagoEnLinea"].ToString();
            string t_servicio = "934";
            string importe = t_importe.Replace(".", "").PadLeft(15, '0');
            string union = s_transm + c_referencia + importe + t_servicio;

            string val_13 = GetMD5(union);

            id_s_transm.Value = "00000000000000000425";
            id_c_referencia.Value = c_referencia;
            id_t_importe.Value = t_importe;
            //id_val_2.Value = val_2;
            id_val_1.Value = "0";
            id_val_3.Value = "1";
            id_val_4.Value = "1";
            id_val_5.Value = "1";
            id_val_6.Value = "0";
            id_val_7.Value = val_7;
            id_val_8.Value = val_8;
            id_t_pago.Value = "01";
            id_servicio.Value = "934";
            id_val_11.Value = val_11;
            id_val_12.Value = val_12;
            id_val_13.Value = val_13;


            string script = string.Format("validar_datos();");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "envio", script, true);


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
    /// Antes de enviar los datos a la URL del banco, se debe insertar un registro en la tabla PAgo asociado a la declaración en curso
    /// </summary>
    private void registrarPago()
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
    /// Realizamos el pago de la declaración (CAJA o TELEMÁTICO)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPagar_click(object sender, EventArgs e)
    {
        try
        {
            //Fecha de pago incorrecta
            this.Validate("pago");
            if (!this.IsValid)
            {
                extenderPnlInfoFechaPago.Show();

            }
            else
            {
                if (rbCaja.Checked)
                    PagoCaja();
                else if (RBTelematico.Checked)
                    PagoCaja();
            }
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string ErrorMessage = null;
            System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException>;
            if (faultEx != null)
            {
                if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                    ErrorMessage = faultEx.Detail.Descripcion;
                else
                    ErrorMessage = "Se presento un problema al realizar la operación.";
            }
            else
            {
                if (ex.InnerException != null)
                    ErrorMessage = ex.InnerException.Message;
                else
                    ErrorMessage = ex.Message;
            }
            MostrarInfoGuardar(ErrorMessage, true);
        }
    }

    /// <summary>
    /// Método que realiza el pago por caja
    /// </summary>
    private void PagoCaja()
    {
        ClienteDeclaracionIsai.PagarDeclaracion(Convert.ToInt32(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION), DateTime.Now, null, null, Constantes.PAR_VAL_FORMAPAGO_CAJA);
        ClienteDeclaracionIsai.PresentarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
        //recargamos la declaracion
        RecargarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
        CargarDatosFormulario();
        txtFechaLineaCaptura.Visible = false;
        txtLineaDato.Visible = false;
        btnFechaLineaCaptura.Visible = false;

        txtFechaPago.Visible = false;
        lblFechaPagoDato.Visible = true;
        lblFechaPagoDato.Text = DateTime.Now.ToShortDateString();
        btnFechaPago.Visible = false;

        UpdatePanelDatosPresentacion.Update();
        extenderPnlInfoPres.Show();


    }


    /// <summary>
    /// Nos redirige a la página referida (PÁGINA ORIGEN)
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e"></param>
    protected void lnkVolverDeclaraciones_Click(object sender, EventArgs e)
    {
        try
        {
            clearCache();
            RedirectUtil.BaseURL = PaginaOrigen;
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Any() && !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDAVALUONull())
                RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO.ToString());
            ActualizarParametrosUrlBusqueda();
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
    /// Mostramos el Modal popup con los dos estados permitidos (BORRADOR o PENDIENTE PRESENTAR)
    /// con los que guardaremos la declaración en la base de datos
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e"></param>
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) == 0)//Si es de tipo Jornada Notarial validar que existe la cuenta catastral
                this.Validate("ValidarCuenta");
            if (this.IsValid)
            {
                //checkea los roles de los participantes
                bool faltaRol = false;
                DateTime fechacon;

                foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participante in DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Rows)
                {
                    if (participante.RowState != DataRowState.Deleted)
                    {
                        if (participante.IsCODROLNull())
                        {
                            faltaRol = true;
                            break;
                        }
                    }
                }



                //valida que los participantes tengan roles asignados
                if (faltaRol)
                {
                    this.modalBeneficios.TextoInformacion = "Al menos uno de los participantes no tiene un rol asignado. Por favor verifique antes de continuar.";
                    extenderPnlValidarBeneficios.Show();
                }
                //valida que se haya seleccionado una exencion o una reduccion segun el caso
                else if ((rbExencion.Checked && ucExencion.IdExencion < 1))
                {
                    this.modalBeneficios.TextoInformacion = "Por favor seleccione una causa de exención antes de continuar.";
                    extenderPnlValidarBeneficios.Show();
                }
                else if (rbReduccion.Checked && ucReduccion.IdReducccion < 1)
                {
                    this.modalBeneficios.TextoInformacion = "Por favor seleccione una causa de reducción antes de continuar.";
                    extenderPnlValidarBeneficios.Show();
                }
                else if (rbSubsidio.Checked && string.IsNullOrEmpty(txtPorcentajeSubsidio.Text))
                {
                    this.modalBeneficios.TextoInformacion = "Por favor aporte un Porcentaje de subsidio.";
                    extenderPnlValidarBeneficios.Show();
                }
                else if (this.rbDisminucion.Checked && string.IsNullOrEmpty(this.txtPorcentajeDisminucíon.Text))
                {
                    this.modalBeneficios.TextoInformacion = "Por favor aporte un Porcentaje de Disminución.";
                    extenderPnlValidarBeneficios.Show();
                }
                else if (this.rbCondonacion.Checked && (string.IsNullOrEmpty(this.txtPorcentajeCondonacion.Text) || string.IsNullOrEmpty(txtMotivoCondonacion.Text) || !DateTime.TryParse(txtFechaCondonacion.Text, out fechacon)))
                {

                    this.modalBeneficios.TextoInformacion = "Por favor debe aportar un Porcentaje de Condonación, la fecha y el motivo.";
                    extenderPnlValidarBeneficios.Show();
                }

                else
                {
                    bool importeNotificado = false;
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION != Constantes.PAR_VAL_ESTADODECLARACION_BORR.ToInt())
                    {
                        ActualizarDatosCalculo(false);
                        importeNotificado = (lblImpuestoCalculadoDato.Text.ToDecimalFromStringFormatted() != txtImpuestoNotarioDato.Text.ToDecimalFromStringFormatted());
                    }
                    if (importeNotificado)
                    {
                        ModalInfoValidarImporte.TextoInformacion = Constantes.MENSAJEIMPORTEDECLARACION;
                        ModalInfoValidarImporte.TipoMensaje = true;
                        extenderPnlValidarImporte.Show();
                    }
                    else
                    {
                        this.Validate("DeclaracionNormal");
                        if (this.IsValid)
                            btnGuardar_ModalPopupExtender.Show();
                    }
                }

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
    /// Mostramos un mensaje informativo de restricción al usuario, para el cual pueda realizar el cálculo
    /// con los datos necesarios si es que falta algún dato para el cálculo
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e"></param>
    protected void ModalValidarImporte_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            this.Validate("DeclaracionNormal");
            if (this.IsValid)
                btnGuardar_ModalPopupExtender.Show();
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
    /// Maneja el evento Click del control ModalInfoErrorCuenta
    /// Se redirige a la página origen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoErrorCuenta_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            RedirectUtil.BaseURL = PaginaOrigen;
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDAVALUONull())
                RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO.ToString());
            ActualizarParametrosUrlBusqueda();
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
    /// Maneja el evento Click del control btnPagoTelematico
    /// Verifica que se ha obtenido línea de captura y la fecha de la misma para marcar como pagada la declaración.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPagoTelematico_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.Validate("pago");
            if (this.IsValid)
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsNull(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.ENPAPELColumn))
                {
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ENPAPEL.CompareTo(Constantes.PAR_VAL_TRUE) == 0)
                    {
                        DateTime fechaCaptura;
                        if (String.IsNullOrEmpty(txtLineaDato.Text))
                        {
                            MostrarInfoGuardar(Constantes.TEXT_SIN_LINEA, true);

                        }
                        else if (!DateTime.TryParse(txtFechaLineaCaptura.Text, out fechaCaptura))
                        {
                            MostrarInfoGuardar(Constantes.TEXT_SIN_FECHA, true);
                        }
                        else
                        {
                            string mensajeAviso = string.Empty;
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA = txtLineaDato.Text;
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA = Convert.ToDateTime(txtFechaLineaCaptura.Text);
                            DseDeclaracionIsai dse = DseDeclaracionIsaiMant;

                            //Hay que recuperar el impuesto pagado anterior si se trata de una declaracion complementaria
                            //de lo contrario el impuesto calculado no se calculara correctamente

                            decimal? impuestoPagadoAnterior = null;
                            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDECLARACIONPADRENull())
                            {
                                //recuperar impuesto pagado en la declaracion padre
                                DseDeclaracionIsaiPadreMant = ClienteDeclaracionIsai.ObtenerDseDeclaracionIsaiPorIdDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACIONPADRE);
                                if (DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION.Any() && DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0] != null && !DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                                    impuestoPagadoAnterior = DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO;
                            }

                            mensajeAviso = ClienteDeclaracionIsai.RegistrarDeclaracion2(ref dse, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, impuestoPagadoAnterior, DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull() ? (decimal?)null : DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO, DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull() ? null : (DateTime?)DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO, listaValoresIsr.ToArray());
                            DseDeclaracionIsaiMant = dse;
                            btnObtener.Visible = false;
                            pnlObtener.Update();
                            lblObtenerLinea.Visible = false;
                            extenderModalPopupPago.Show();
                        }
                    }
                    else
                    {
                        //string mensajeAviso = string.Empty;
                        //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA = lblLineaDato.Text;
                        //DseDeclaracionIsai dse = DseDeclaracionIsaiMant;

                        ////Hay que recuperar el impuesto pagado anterior si se trata de una declaracion complementaria
                        ////de lo contrario el impuesto calculado no se calculara correctamente

                        //decimal? impuestoPagadoAnterior = null;
                        //if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDECLARACIONPADRENull())
                        //{
                        //    //recuperar impuesto pagado en la declaracion padre  
                        //    DseDeclaracionIsaiPadreMant = ClienteDeclaracionIsai.ObtenerDseDeclaracionIsaiPorIdDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACIONPADRE);
                        //    if (DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION.Any() && DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0] != null && !DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                        //        impuestoPagadoAnterior = DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO;
                        //}
                        //if (dse.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_JOR && dse.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
                        //    mensajeAviso = ClienteDeclaracionIsai.RegistrarDeclaracion(ref dse, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, impuestoPagadoAnterior, DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull() ? (decimal?)null : DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO);
                        //else if (dse.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
                        //    mensajeAviso = ClienteDeclaracionIsai.RegistrarDeclaracion(ref dse, XmlDocumental, TipoDocumental, ListaIdsDocumental, XmlDocumentalBF, TipoDocumentalBF, ListaIdsDocumentalBF, RecuperaImpuestoPagado(lblImpuestoPagadoAnterior, true), DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull() ? (decimal?)null : DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO);
                        //else mensajeAviso = ClienteDeclaracionIsai.RegistrarDeclaracion(ref dse, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, impuestoPagadoAnterior, (decimal?)null);
                        //DseDeclaracionIsaiMant = dse;

                        lblObtenerLinea.Visible = false;
                        extenderModalPopupPago.Show();
                    }
                }
                else
                {
                    extenderModalPopupPago.Show();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string ErrorMessage = null;
            System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException>;
            if (faultEx != null)
            {
                if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                    ErrorMessage = faultEx.Detail.Descripcion;
                else
                    ErrorMessage = "Se presento un problema al realizar la operación.";
            }
            else
            {
                if (ex.InnerException != null)
                    ErrorMessage = ex.InnerException.Message;
                else
                    ErrorMessage = ex.Message;
            }
            MostrarInfoGuardar(ErrorMessage, true);
        }
    }


    //En el aspx se utiliza una caja de texto junto con un control RequiredField para simplificar la validación 
    //El botón Tomar decisión siempre valida.. si no se ha tomado una decisión o si la decisión es rechazar sin haber especificado motivo
    //la validacion falla pues la caja de texto esta vacía 
    //Si se selecciona una opcion diferente a rechazar, o bien ésta y un motivo, la caja se inicializa con un valor que puede ser cualquiera,
    //así la validacion no falla
    /// <summary>
    /// Actualiza el valor de la caja de texto que se utiliza para simplificar la validación de la toma de decisión
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void decisionChanged(object sender, EventArgs e)
    {
        try
        {
            //si la decision sera rechazar la declaracion
            if (rbDecision.SelectedIndex == 1)
            {
                ddlMotivoRechazo.Visible = true;
                //limpiar al caja de texto para provocar falla en la validacion
                txtMotivo.Text = "";
            }
            else
            {
                ddlMotivoRechazo.Visible = false;
                //inicializar la caja de texto para que la validacion no falle
                txtMotivo.Text = "Continuar";
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
    /// Inicializa la caja de texto usada en la validación de la toma de decisión cuando se selecciona un motivo de rechazo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LogMotivo(object sender, EventArgs e)
    {
        try
        {
            txtMotivo.Text = (ddlMotivoRechazo.SelectedIndex > 0) ? "Continuar" : "";
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
    /// Configura los controles relacionados con la activación del panel de beneficios fiscales
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ConfigurarBeneficiosFiscales(object sender, EventArgs e)
    {
        try
        {
            ResetBeneficiosDocumentos();
            trSinBeneficio.Visible = false;
            this.divBeneficiosFiscales.Visible = true;
            this.BeneficiosiscalesLeyend.Visible = true;
            this.trBeneficiosJustificacion.Visible = false;
            this.trBeneficiosJustificacion2.Visible = false;
            this.trDesBeneficios.Visible = false;
            txtValBeneficio.Text = "";
            //verifica si aplica tasa cero
            ConfigurarTasaCero();


            this.imgBeneficiosOn.Enabled = false;
            this.imgBeneficiosOff.Enabled = true;
            this.imgBeneficiosOn.ImageUrl = (this.imgBeneficiosOn.Enabled) ? Constantes.IMG_ANADIR : Constantes.IMG_ANADIR_DISABLED;
            this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;

            rbReduccion.Enabled = true;
            rbExencion.Enabled = true;
            ucExencion.Editable = false;
            ucReduccion.Editable = false;
            CargarDDLExeccion();
            CargarDDLReduccion();

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
    /// Maneja el evento Click de imgBeneficiosOff
    /// Configura los controles fiscales dejandolos por defecto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ConfigurarBeneficiosFiscalesBorrar(object sender, EventArgs e)
    {
        try
        {
            ResetBeneficiosDocumentos();
            rfvDocDig.Enabled = false;

            this.divBeneficiosFiscales.Visible = false;
            this.BeneficiosiscalesLeyend.Visible = true;
            this.trBeneficiosJustificacion.Visible = false;
            this.trBeneficiosJustificacion2.Visible = false;
            this.trDesBeneficios.Visible = false;
            ucReduccion.ResetearControl();
            ucExencion.ResetearControl();
            //Sin Beneficios fiscales
            txtValBeneficio.Text = "Continuar";
            rbExencion.Checked = false;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDEXENCIONNull();
            rbCondonacion.Checked = false;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetPORCENTAJECONDONACIONNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHACONDONACIONNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetMOTIVOCONDONACIONNull();
            rbSubsidio.Checked = false;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetPORCENTAJESUBSIDIONull();
            rbReduccion.Checked = false;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDREDUCCIONNull();
            rbDisminucion.Checked = false;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetPORCENTAJEDISMINUCIONNull();
            rbTasaCero.Checked = false;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetESTASACERONull();
            ucReduccion.Editable = false;
            ucExencion.Editable = false;
            txtPorcentajeDisminucíon.Enabled = false;
            txtPorcentajeDisminucíon.Text = string.Empty;
            txtPorcentajeSubsidio.Enabled = false;
            txtPorcentajeSubsidio.Text = string.Empty;
            txtPorcentajeCondonacion.Enabled = false;
            txtPorcentajeCondonacion.Text = string.Empty;
            txtFechaCondonacion.Enabled = false;
            txtFechaCondonacion.Text = string.Empty;
            txtMotivoCondonacion.Enabled = false;
            txtMotivoCondonacion.Text = string.Empty;
            trSinBeneficio.Visible = true;
            txtDocDigitalBF.Text = string.Empty;
            this.valBeneficios.ErrorMessage = "Seleccione un beneficio fiscal.";
            this.imgBeneficiosOn.Enabled = true;
            this.imgBeneficiosOff.Enabled = false;
            this.imgBeneficiosOn.ImageUrl = (this.imgBeneficiosOn.Enabled) ? Constantes.IMG_ANADIR : Constantes.IMG_ANADIR_DISABLED;
            this.imgBeneficiosOff.ImageUrl = (this.imgBeneficiosOff.Enabled) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
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
    /// Maneja el evento Click del control btnBorrarDocumentos
    /// Muestra la pantalla de confirmación
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBorrarDocumentos_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopEliminarDoc.Show();
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
    /// Maneja el evento click del control btnBorrarDocBeneficios
    /// Configura todos los controles del panel de beneficios para dejarlos por defecto
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e"></param>
    protected void btnBorrarDocBeneficios_Click(object sender, EventArgs e)
    {
        try
        {
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDDOCBENFISCALESNull();
            btnDocBeneficios.Enabled = true;
            btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR;
            btnEditarDocBeneficios.Enabled = false;
            btnEditarDocBeneficios.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
            btnEliminarDocBeneficios.Enabled = false;
            btnEliminarDocBeneficios.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
            btnVerDocBeneficios.Enabled = false;
            btnVerDocBeneficios.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
            lblDescripcionDocBF.Text = "No hay documentos registrados";
            DescripBF = lblDescripcionDocBF.Text;
            txtDocDigitalBF.Text = null;
            txtIdDocDig.Text = null;
            if (XmlDocumentalBF != null && XmlDocumentalBF.Length != 0)
                XmlDocumentalBF = string.Empty;
            if (TipoDocumentalBF != null && XmlDocumentalBF.Length != 0)
                TipoDocumentalBF = string.Empty;
            if (ListaIdsDocumentalBF != null && XmlDocumentalBF.Length != 0)
                ListaIdsDocumentalBF = string.Empty;

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
    /// Maneja el evento CheckedChanged del control RBTelematico
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RBTelematico_OnCheckedChanged(object sender, EventArgs e)
    {
        if (RBTelematico.Checked)
        {
            ddlCatBanco.Visible = true;
            ComprobarBanco();
        }
        else
        {
            ddlCatBanco.Visible = false;
            btnPagar.Enabled = true;
        }
        extenderModalPopupPago.Show();
    }

    #endregion

    #region Eventos Modales

    /// <summary>
    /// Mostramos un mensaje informativo al usuario después de guardar la declaración
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoGuardar_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (!ModalInfoGuardar.TipoMensaje)
            {
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) == 0)
                {
                    RedirectUtil.BaseURL = Constantes.URL_SUBISAI_BANDEJAJORNADA;
                }
                else
                {
                    RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACIONES;
                    RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO.ToString());
                }
                ActualizarParametrosUrlBusqueda();
            }
            clearCache();
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
    /// Maneja el evento Click del control ModalInfoValidarCuenta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoValidarCuenta_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
                extenderPnlInfoValidarCuentaCatastralModal.Hide();
            else
                extenderPnlInfoValidarCuentaCatastralModal.Show();
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
    /// Maneja el evento Click del control ModalInfoCuentaCatastral
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoCuentaCatastral_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                extenderPnlInfoCuentaCatastralModal.Hide();
                lnkVolverDeclaraciones_Click(null, null);
            }
            else
                extenderPnlInfoCuentaCatastralModal.Show();
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
    /// Maneja el evento Click del control ModalInfoFUT
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoFUT_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
                extenderPnlInfoFUTModal.Hide();
            else
                extenderPnlInfoFUTModal.Show();
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

    protected void ActualizarPorcTotal()
    {
        decimal porc = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Where(c => c.RowState != DataRowState.Deleted).Where(c => c.ROL.Equals(Constantes.PAR_VAL_ENAJENANTE)).Where(c => !c.IsTIPOPERSONANull()).Sum(c => c.PORCTRANSMISION).ToDecimal();
        lblTotalActoJurDato.Text = porc.ToPercent();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL = porc;
    }
    /// <summary>
    /// Evento del botón del modal popup para guardar la declaración (BORRADOR o PENDIENTE PRESENTAR)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPnlGuardarModalOK_Click(object sender, EventArgs e)
    {

        //Importante:
        //En la clase de Negocio se realiza una validacion despues (¿?) de haber impactado DB
        //Esa validacion puede lanzar una excepcion que asu vez se cacha en este metodo
        //Si se cacha la excepcion hay que, por lo menos: regresar el DATASET de la declaracion al estado original
        //es posible que esto tambien ocurra con otros campos

        decimal EstadoOriginalDeclaracion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION;

        try
        {
            bool valido = false;
            if (rbModalGuardarPendiente.Checked)
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCACTOTOTALNull())
                {
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL == 0)
                    {
                        this.modalBeneficios.TextoInformacion = "El porcentaje total debe ser mayor a 0.";
                        extenderPnlValidarBeneficios.Show();
                        valido = false;
                    }
                    else
                    {
                        var lista = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Where(c => c.RowState != DataRowState.Deleted).Where(c => c.PORCTRANSMISION ==0).ToList();
                        if (lista.Any())
                        {
                            this.modalBeneficios.TextoInformacion = "El porcentaje de los participantes debe ser mayor a 0";
                            extenderPnlValidarBeneficios.Show();
                            valido = false;
                        }
                        else
                        {
                            valido = true;
                        }
                    }
                }
                else
                {
                    this.modalBeneficios.TextoInformacion = "El porcentaje total debe ser mayor a 0.";
                    extenderPnlValidarBeneficios.Show();
                    valido = false;
                }
                if (valido)
                {
                        if (ddlActoJuridicoDato.SelectedItem.Text.ToUpper().Contains("HERENCIA") || ddlActoJuridicoDato.SelectedItem.Text.ToUpper().Contains("JUDICIAL"))
                        {
                            valido = true;
                        }
                        else
                        {
                            try
                            {
                                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION > DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA)
                                {
                                    this.modalBeneficios.TextoInformacion = "La fecha causación no debe ser mayor a la fecha de escritura para este acto juridico.";
                                    extenderPnlValidarBeneficios.Show();
                                    valido = false;
                                }
                                else
                                {
                                    valido = true;
                                }
                            }
                            catch
                            {
                                this.modalBeneficios.TextoInformacion = "Falta fecha causación y/o de escritura";
                                extenderPnlValidarBeneficios.Show();
                            }
                        }
                }
            }
            else
            {
                valido = true;
            }
            // Valida que la persona sea fisica y que tenga capturado el RFC
            if (valido)
            {
                if (ValidarucParticipantesISR())
                {

                    decimal tipoDeclaracion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION;
                    if (rbModalGuardarPendiente.Checked &&
                        (tipoDeclaracion == Constantes.PAR_VAL_TIPODECLARACION_NOR.ToDecimal() || tipoDeclaracion == Constantes.PAR_VAL_TIPODECLARACION_COMPLE.ToDecimal() || tipoDeclaracion == Constantes.PAR_VAL_TIPODECLARACION_JOR.ToDecimal()))
                    {
                        this.Validate("validacionDocEsc");

                    }
                    if (this.IsValid)
                    {
                        if (rbModalGuardarPendiente.Checked)
                        {
                            this.Validate("validacionDocBenFis");
                        }
                        if (this.IsValid)
                        {
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDUSUARIO = Convert.ToDecimal(Usuarios.IdUsuario());
                            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_COMPLE))
                            {
                                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION = DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].REGION;
                                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA = DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].MANZANA;
                                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE = DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].LOTE;
                                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA = DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;
                            }
                            //Guardo el valor de N en el Campo de EnPapel
                            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsNull(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.ENPAPELColumn))
                                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ENPAPEL = Constantes.PAR_VAL_FALSE;

                            //Guardo como no generada Borrador a Pendiente
                            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].GENERADA = Constantes.PAR_VAL_FALSE;

                            guardarDatos();




                            //Calcular impuesto
                            //if (rbModalGuardarPendiente.Checked)
                            //{
                            //    decimal? impuestoCalculadoNuevoValor = null;
                            //    decimal? impuestoCalculadoAnteriorValor = null;
                            //    decimal? resultadoRecargo = null;
                            //    decimal? resultadoActualizacion = null;
                            //    decimal? resultadoImporteActualizacion = null;
                            //    decimal? resultadoImporteRecargo = null;
                            //    decimal? resultadoBaseGravable = null;
                            //    decimal? resultadoReduccion1995 = null;
                            //    decimal? resultadoTasa1995 = null;
                            //    decimal? resultadoImpuesto = null;
                            //    decimal? resultadoReduccionArt309 = null;
                            //    decimal? resultadoExencionImporte = null;
                            //    decimal? resultadoImporteCondonacion = null;
                            //    decimal? resultadoImporteCondonacionxFecha = null;

                            //    decimal? par_codtipodeclaracion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION;
                            //    decimal? par_participacion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCACTOTOTALNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL;
                            //    decimal? par_valoradquisicion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORADQUISICIONNull() ? 0 : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORADQUISICION;
                            //    decimal? par_valorcatastral = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull() ? 0 : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL;
                            //    decimal? par_valoravaluo = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORAVALUONull() ? 0 : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORAVALUO;
                            //    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_JOR)
                            //        par_valoravaluo = DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull() ? par_valoravaluo : DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO;
                            //    if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull() && DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO.Year < 1993)
                            //        par_valoravaluo = par_valoravaluo / 1000;
                            //    string par_eshabitacional = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESHABITACIONAL;
                            //    string par_estasacero = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsESTASACERONull() ? null : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTASACERO;
                            //    decimal? par_idexencion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDEXENCIONNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDEXENCION;
                            //    decimal? par_idreduccion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDREDUCCION;
                            //    decimal? par_codjornotarial = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDJORNADANOTARIALNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDJORNADANOTARIAL;
                            //    decimal? par_porcentajesubsio = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO;
                            //    decimal? par_porcentajedisminucion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION;
                            //    decimal? par_porcentajecondonacion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
                            //    string par_regla = rbReglasVigentes.Checked.ToOracleChar0FromBoolean();
                            //    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_ANTI
                            //        && !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull() && !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull()
                            //        && DateTime.Compare(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION, new DateTime(1972, 01, 01)) >= 0)
                            //    {
                            //        //Codigo original comentado por GULE
                            //        //impuestoCalculadoNuevoValor = System.Math.Round(ClienteDeclaracionIsai.CalcularImpuestoDeclaracion(par_codtipodeclaracion,
                            //        //    par_participacion,
                            //        //    par_valoradquisicion,
                            //        //    par_valorcatastral,
                            //        //    par_valoravaluo,
                            //        //    par_eshabitacional,
                            //        //    par_estasacero,
                            //        //    par_idexencion,
                            //        //    par_idreduccion,
                            //        //    par_codjornotarial,
                            //        //    par_porcentajesubsio,
                            //        //    par_porcentajedisminucion,
                            //        //    par_porcentajecondonacion,
                            //        //    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION,
                            //        //    Convert.ToDecimal(par_regla),
                            //        //    out  resultadoRecargo, out  resultadoImporteRecargo,
                            //        //    out  resultadoActualizacion,
                            //        //    out  resultadoImporteActualizacion,
                            //        //    out resultadoBaseGravable,
                            //        //    out resultadoReduccion1995,
                            //        //    out resultadoTasa1995,
                            //        //    out resultadoImpuesto,
                            //        //    out resultadoReduccionArt309,
                            //        //    out resultadoExencionImporte,
                            //        //    out resultadoImporteCondonacion) + (0.49).ToDecimal());

                            //        //Código nuevo implementado por GULE
                            //        impuestoCalculadoNuevoValor = ClienteDeclaracionIsai.CalcularImpuestoDeclaracion(par_codtipodeclaracion,
                            //           par_participacion,
                            //           par_valoradquisicion,
                            //           par_valorcatastral,
                            //           par_valoravaluo,
                            //           par_eshabitacional,
                            //           par_estasacero,
                            //           par_idexencion,
                            //           par_idreduccion,
                            //           par_codjornotarial,
                            //           par_porcentajesubsio,
                            //           par_porcentajedisminucion,
                            //           par_porcentajecondonacion,
                            //           DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION,
                            //           Convert.ToDecimal(par_regla),
                            //           out  resultadoRecargo, out  resultadoImporteRecargo,
                            //           out  resultadoActualizacion,
                            //           out  resultadoImporteActualizacion,
                            //           out resultadoBaseGravable,
                            //           out resultadoReduccion1995,
                            //           out resultadoTasa1995,
                            //           out resultadoImpuesto,
                            //           out resultadoReduccionArt309,
                            //           out resultadoExencionImporte,
                            //           out resultadoImporteCondonacion, 
                            //           out resultadoImporteCondonacionxFecha).ToDecimal().ToRoundTwo();

                            //    }
                            //    if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
                            //        impuestoCalculadoAnteriorValor = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO;
                            //    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_COMPLE)
                            //    {
                            //        impuestoCalculadoNuevoValor = ((impuestoCalculadoNuevoValor.Value) - Convert.ToDecimal(DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO)).ToString("0.00").ToDecimal();
                            //    }
                            //    //if (impuestoCalculadoAnteriorValor.HasValue && impuestoCalculadoNuevoValor.HasValue && (impuestoCalculadoAnteriorValor.Value.ToString("0.00") != impuestoCalculadoNuevoValor.Value.ToString("0.00")))
                            //    //{
                            //    //    ModalConfirmarcionCalc.TextoConfirmacion = "Se ha recalculado el importe calculado cuyo nuevo importe es: $" + impuestoCalculadoNuevoValor.Value.ToString("0.00") + ".  <br> La declaración se registrará con el importe declarado: ";
                            //    //    ModalConfirmarcionCalc.ImporteDeclarado = impuestoCalculadoNuevoValor.Value;
                            //    //    ModalPopupExtenderCalc.Show();
                            //    //}
                            //    //else
                            //        FinalizarRegistroDeclaracion();

                            //}
                            //else
                            if (rbModalGuardarPendiente.Checked)
                            {
                                if (ddlActoJuridicoDato.SelectedIndex > 0)
                                {
                                    FinalizarRegistroDeclaracion();
                                }
                                else
                                {
                                    ModalInfoDoc.TextoInformacion = "Debe seleccionar un acto juridico.";
                                    extInfoDoc.Show();
                                }
                            }
                            else
                            {
                                FinalizarRegistroDeclaracion();
                            }
                        }

                        else
                        {
                            ModalInfoDoc.TextoInformacion = "Se requiere adjuntar un documento jurídico que acredite el beneficio fiscal especificado.";
                            extInfoDoc.Show();
                        }

                    }
                    else
                    {
                        ModalInfoDoc.TextoInformacion = "Se requiere adjuntar un documento jurídico a la declaración.";
                        extInfoDoc.Show();
                    }

                }
                else
                {
                    ModalInfoDoc.TextoInformacion = "Causa ISR:Todos los enajenantes deben tener RFC.";
                    extInfoDoc.Show();
                }
            }
        }
        catch (FaultException<DeclaracionIsaiInfoException> diex)
        {
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].RowState == DataRowState.Modified)
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = Convert.ToDecimal(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0][DseDeclaracionIsaiMant.FEXNOT_DECLARACION.CODESTADODECLARACIONColumn.ColumnName, DataRowVersion.Original].ToString());
            if (ConfigurationManager.AppSettings["DEBUG"].ToInt() != 1)
                ExceptionPolicyWrapper.HandleException(diex);

            MostrarInfoGuardar(diex.Detail.Descripcion, true);
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = EstadoOriginalDeclaracion;
        }
        catch (FaultException<DeclaracionIsaiException> dex)
        {
            if (ConfigurationManager.AppSettings["DEBUG"].ToInt() != 1)
                ExceptionPolicyWrapper.HandleException(dex);

            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = EstadoOriginalDeclaracion;
            MostrarInfoGuardar(dex.Detail.Descripcion, true);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].RowState == DataRowState.Modified)
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = Convert.ToDecimal(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0][DseDeclaracionIsaiMant.FEXNOT_DECLARACION.CODESTADODECLARACIONColumn.ColumnName, DataRowVersion.Original].ToString());

            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = EstadoOriginalDeclaracion;

            string ErrorMessage = null;
            System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException>;
            if (faultEx != null)
            {
                if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                    ErrorMessage = faultEx.Detail.Descripcion;
                else
                    ErrorMessage = "Se presento un problema al realizar la operación.";
            }
            else
            {
                if (ex.InnerException != null)
                    ErrorMessage = ex.InnerException.Message;
                else
                    ErrorMessage = ex.Message;
            }

            MostrarInfoGuardar(ErrorMessage, true);
        }
    }

    private void guardarDatos()
    {

        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION == 0)
        {


            decimal decimalParse;
            DateTime datetimeParse;
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TipoDeclaracion.JornadaNotarial.ToDecimal())
            {
                ucParticipantes.ExisteCuenta = IsaiManager.ComprobarExisteCuentaCatastral(txtRegion.Text.Trim(), txtManzana.Text.Trim(), txtLote.Text.Trim(), txtUnidadPrivativa.Text.Trim());
            }
            else
            {
                ucParticipantes.ExisteCuenta = true;
            }
            //lugar de jornada notarial
            if (!string.IsNullOrEmpty(txtLugar.Text))
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LUGARJORNADANOTARIAL = txtLugar.Text;
            }
            else
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetLUGARJORNADANOTARIALNull();
            }

            //guardo los datos del calculo
            if (!string.IsNullOrEmpty(lblActualizacionImporteDato.Text))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE = lblActualizacionImporteDato.Text.ToDecimalFromStringFormatted();
            if (!string.IsNullOrEmpty(lblImpuestoCalculadoDato.Text))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = lblImpuestoCalculadoDato.Text.ToDecimalFromStringFormatted();
            if (!string.IsNullOrEmpty(lblPorcRegargoExTempDato.Text))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCJERECARGOPAGOEXTEMP = lblPorcRegargoExTempDato.Text.ToDecimalFromStringFormatted();

            //Guardo el Importe Notificado que es el ImpuestoPagado
            if (!string.IsNullOrEmpty(txtImpuestoNotarioDato.Text))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO = txtImpuestoNotarioDato.Text.ToDecimalFromStringFormatted();
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIMPUESTOPAGADONull();

            #region [ BENEFICIOS FISCALES]
            //reset campos relacionados con beneficios fiscales
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDEXENCIONNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDREDUCCIONNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetPORCENTAJEDISMINUCIONNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetPORCENTAJECONDONACIONNull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetPORCENTAJESUBSIDIONull();
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetESTASACERONull();

            if (rbExencion.Checked)
            {
                if (ucExencion.IdExencion != Constantes.UI_DDL_VALUE_NO_SELECT.ToInt())
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDEXENCION = ucExencion.IdExencion.To<Decimal>();
            }
            else if (rbReduccion.Checked)
            {
                if (ucReduccion.IdReducccion != Constantes.UI_DDL_VALUE_NO_SELECT.ToInt())
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDREDUCCION = ucReduccion.IdReducccion.To<Decimal>();
            }
            else if (rbSubsidio.Checked)
            {
                if (decimal.TryParse(txtPorcentajeSubsidio.Text, out decimalParse))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO = (decimalParse / 100);

            }
            else if (rbDisminucion.Checked)
            {
                if (decimal.TryParse(txtPorcentajeDisminucíon.Text, out decimalParse))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION = (decimalParse / 100);
            }
            else if (rbCondonacion.Checked)
            {
                if (decimal.TryParse(txtPorcentajeCondonacion.Text, out decimalParse))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION = (decimalParse / 100);
                if (!string.IsNullOrEmpty(txtMotivoCondonacion.Text))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MOTIVOCONDONACION = txtMotivoCondonacion.Text.Trim();
                if (DateTime.TryParse(txtFechaCondonacion.Text, out datetimeParse))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACONDONACION = datetimeParse;
            }
            else if (rbTasaCero.Checked)
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESTASACERO = Constantes.PAR_VAL_TRUE;
            }
            #endregion


            //JornadaNotarial
            if (String.IsNullOrEmpty(CondonacionJornada.ToString()))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDJORNADANOTARIALNull();
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDJORNADANOTARIAL = (decimal)CondonacionJornada;

            //guardar acto juridico
            if (!string.IsNullOrEmpty(ddlActoJuridicoDato.SelectedValue))
                if (ddlActoJuridicoDato.SelectedValue != Constantes.UI_DDL_VALUE_NO_SELECT)
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR = ddlActoJuridicoDato.SelectedValue.ToInt();
                else
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetCODACTOJURNull();
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetCODACTOJURNull();
            if (!string.IsNullOrEmpty(ddlPaisDato.SelectedValue))
                if (ddlPaisDato.SelectedValue != Constantes.UI_DDL_VALUE_NO_SELECT)
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODPROCEDENCIA = ddlPaisDato.SelectedValue.ToInt();
                else
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetCODPROCEDENCIANull();
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetCODPROCEDENCIANull();

            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA = Convert.ToDecimal(rbReglasVigentes.Checked.ToOracleChar0FromBoolean());

            //valor Catastral
            decimal valorCatastral;
            if (ExisteValorCatastral())
            {
                if (Decimal.TryParse(ObtenerValorCatastral().Trim(), out valorCatastral))
                    //valor catastral puede ser Cero, asi q basta con q sea un numero valido
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL = valorCatastral;
                else DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORCATASTRALNull();
            }
            else DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORCATASTRALNull();
            decimal valorAdquisicion;
            if (!string.IsNullOrEmpty(txtValorAdquisicionDato.Text.ToString()))
            {
                if (Decimal.TryParse(txtValorAdquisicionDato.Text.Trim(), out valorAdquisicion))
                    //valor catastral puede ser Cero, asi q basta con q sea un numero valido
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORADQUISICION = valorAdquisicion;
            }
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORADQUISICIONNull();

            //Fecha causación
            if (DateTime.TryParse(txtFechaCausacion.Text, out datetimeParse))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION = datetimeParse;
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHACAUSACIONNull();
            //Fecha preventiva, declaracion anticipada
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_ANTI)
            {
                if (DateTime.TryParse(txtFechaPrev.Text, out datetimeParse))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAPREVENTIVA = datetimeParse;
                else DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAPREVENTIVANull();
            }

            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESHABITACIONAL = chkbHabitacional.Checked.ToOracleChar1FromBoolean();
            if (!string.IsNullOrEmpty(txtAreaObservaciones.Text))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].OBSERVACIONES = txtAreaObservaciones.Text;
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetOBSERVACIONESNull();




            //Almacena los datos de isr
            if (CBCausaISR.Checked)
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CAUSAISR = 1;
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CAUSAISR = 0;


        }
    }

    /// <summary>
    /// Método que realiza los pasos finales necesarios para el registro de la declaración
    /// </summary>
    private void FinalizarRegistroDeclaracion()
    {
        try
        {
            //declaro la variable del mensaje de aviso       
            string mensajeAviso = string.Empty;
            string xml = Utilidades.base64Decode(txtDocDigital.Text);
            decimal estadoOriginal = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION;
            //estado de la declaracion
            if (rbModalGuardarBorrador.Checked)
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = EstadosDeclaraciones.Borrador.ToDecimal();
            else if (rbModalGuardarPendiente.Checked)
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = EstadosDeclaraciones.Pendiente.ToDecimal();
            }

            // JABS, Correccion para que se vean en la Bandeja de entrada las declaraciones.
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDPERSONA = Convert.ToDecimal(Usuarios.IdPersona());

            //Almacena los datos de isr
            if (CBCausaISR.Checked)
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CAUSAISR = 1;
            else
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CAUSAISR = 0;






            DseDeclaracionIsai dse = DseDeclaracionIsaiMant;
            decimal? valorReferido = null;
            DateTime? fechaValorReferido = null;
            if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull())
            {
                valorReferido = DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO;
            }
            if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull())
                fechaValorReferido = DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO;
            // JABS, Aqui se guarda la declaracion en la DB
            if (dse.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TipoDeclaracion.JornadaNotarial.ToDecimal() && dse.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TipoDeclaracion.Complementaria.ToDecimal())
                mensajeAviso = ClienteDeclaracionIsai.RegistrarDeclaracion2(
                    ref dse,
                    XmlDocumental,
                    TipoDocumental,
                    ListaIdsDocumental,
                    XmlDocumentalBF,
                    TipoDocumentalBF,
                    ListaIdsDocumentalBF,
                    RecuperaImpuestoPagado(lblImpuestoPagadoAnterior, true),
                    valorReferido, fechaValorReferido, listaValoresIsr.ToArray());


            else if (dse.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TipoDeclaracion.Complementaria.ToDecimal())
                mensajeAviso = ClienteDeclaracionIsai.RegistrarDeclaracion2(
                    ref dse,
                    XmlDocumental,
                    TipoDocumental,
                    ListaIdsDocumental,
                    XmlDocumentalBF,
                    TipoDocumentalBF,
                    ListaIdsDocumentalBF,
                    RecuperaImpuestoPagado(lblImpuestoPagadoAnterior, true),
                    valorReferido, fechaValorReferido, listaValoresIsr.ToArray());
            else
                mensajeAviso = ClienteDeclaracionIsai.RegistrarDeclaracion2(
                    ref dse,
                    XmlDocumental,
                    TipoDocumental,
                    ListaIdsDocumental,
                    XmlDocumentalBF,
                    TipoDocumentalBF,
                    ListaIdsDocumentalBF,
                    RecuperaImpuestoPagado(lblImpuestoPagadoAnterior, true), valorReferido, fechaValorReferido, listaValoresIsr.ToArray());

            DseDeclaracionIsaiMant = dse;

            if (!string.IsNullOrEmpty(mensajeAviso))
            {
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION != EstadosDeclaraciones.Borrador.ToDecimal())
                {
                    ActualizarDatosCalculo(false);
                }
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = estadoOriginal;
                MostrarMensajeInformacion("Faltan los siguientes valores:" + mensajeAviso);
            }
            else
            {
                if (!rbModalGuardarPendiente.Checked)
                {
                    RedirectUtil.BaseURL = PaginaOrigen;
                }
                else
                {
                    if (!dse.FEXNOT_DECLARACION[0].IsIDDOCBENFISCALESNull())
                        ClienteAltaDocumentos.EstablecerDocumentoDefinitivo(dse.FEXNOT_DECLARACION[0].IDDOCBENFISCALES.ToDecimal());
                    if (!dse.FEXNOT_DECLARACION[0].IsIDDOCUMENTODIGITALNull())
                        ClienteAltaDocumentos.EstablecerDocumentoDefinitivo(dse.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL.ToDecimal());
                    //Si el impuesto declarado es 0.00€ la declaración no pasa por el estado pendiente de presentar, directamente pasa a presentada.
                    //No se va a mostrar el div de pago.
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO == 0)
                    {
                        ClienteDeclaracionIsai.PagarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION.ToInt(), DateTime.Now, null, null, "Cero");
                    }
                    clearCache();
                    RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACION;
                    RedirectUtil.AddParameter(Constantes.PAR_OPERACION, Constantes.PAR_VAL_OPERACION_VER);
                    RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION.ToString());
                    RedirectUtil.AddParameter(Constantes.PAR_PAGINAORIGEN, PaginaOrigen);
                }

                string parIdAvaluo = string.Empty;
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDAVALUONull())
                    parIdAvaluo = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO.ToString();
                RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, parIdAvaluo);
                ActualizarParametrosUrlBusqueda();

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
    /// Mostramos la exception del token 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoToken_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                extenderPnlInfoTokenModal.Hide();
            }
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
    /// Mostramos la exception del token 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoFechaPago_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                extenderPnlInfoFechaPago.Hide();
            }
            else
                extenderPnlInfoFechaPago.Show();
            //txtFechaPago.Text = string.Empty;
            uppInfoFechaPago.Update();
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
    /// Ocultamos el mensaje de declaración pagada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoPres_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                extenderModalPopupPago.Hide();
            }
            else
            {
                ClienteDeclaracionIsai.PresentarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
                extenderModalPopupPago.Show();
            }
            uppInfoPres.Update();
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
    /// Maneja el evento OnConfirmClick de la modal de confirmación con id ModalConfirmarcionCalc
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProcesarRespuestaCalc(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
                ModalPopupExtenderCalc.Hide();
            else
            {
                RellenarImpuestoDeclarado(ModalConfirmarcionCalc.ImporteDeclarado, txtImpuestoNotarioDato);
                ConfiguraEstiloImpuestoDeclarado();
                if (!string.IsNullOrEmpty(txtImpuestoNotarioDato.Text))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO = txtImpuestoNotarioDato.Text.ToDecimalFromStringFormatted();
                else
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIMPUESTOPAGADONull();
                FinalizarRegistroDeclaracion();
                ModalPopupExtenderCalc.Hide();
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

    #region Métodos privados de la página

    /// <summary>
    /// Método que realiza las acciones necesarias si el inmueble introducido no existe
    /// Borra el valor catastral
    /// Deshabilita los controles relacionados con el valor catastral
    /// Muestra el mensaje de error
    /// </summary>
    private void AccionesInmuebleNoExiste()
    {
        ViewState["existeInmueble"] = false;
        BorrarValorCatastral();
        HabilitarDeshabilitarValorCatastral(false);
        modalErrorCuenta.Show();
    }


    /// <summary>
    /// Acciones a realizar trás comprobar que la cuenta introducida es valida
    /// </summary>
    private void AccionesCuentaValida()
    {
        try
        {
            CargarCombosOperacion();

            ConfigurarFormularioOperacion();

            CargarDatosFormulario();

            //Configuramos y cargamos los datos de la declaracin
            ConfigurarFormularioTipo();

            ConfigurarFormularioEstado();

            ConfigurarFormularioEnPapel();

            //Cargo el dataSet si es la primera vez que se carga la pagina y si la declaracion no es de tipo Complementaria
            if ((PreviousPage == null) &&
                (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TipoDeclaracion.Complementaria.ToDecimal()))
            {
                //cargar el dataset con los valores principales cargados en la pagina
                CrearDeclaracionDTFormulario(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadosDeclaraciones.Borrador.ToDecimal());
            }
            //bug tasa cero
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TipoDeclaracion.JornadaNotarial.ToDecimal() && Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) != 0)
                ConfigurarTasaCero();

            //bug ValorCatastral
            // if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull() || DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TipoDeclaracion.Anticipada.ToDecimal())
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TipoDeclaracion.Anticipada.ToDecimal())
            {
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadosDeclaraciones.Borrador.ToDecimal())
                    HabilitarDeshabilitarValorCatastral(true);
            }

            //registrar las funciones javascript a los botones que integran con documental
            RegistrarJSButton();
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
    /// Método que oculta el panel que permite realizar el pago
    /// </summary>
    private void OcultarPanelPago()
    {
        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO != 0)
        {
            divPago.Visible = true;
        }
        else
        {
            rvFechaPago.Enabled = false;
            divPago.Visible = false;
        }

    }

    /// <summary>
    /// Función que obtiene la fecha de un documento y devuelve un frase dependediento del tipo de documento
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Frase</returns>
    private string DescripcionDocumento(decimal id)
    {
        switch (ClienteDocumentos.GetTipoDocumentoDigital(id).ToString())
        {
            case "EscrituraPublica":
                return "Escritura pública - Fecha: " + Convert.ToDateTime(ClienteDocumentos.GetEscrituraPublicaById(id)[0].FECHA).ToShortDateString();
            case "ResolucionJudicial":
                return "Resolución judicial - Fecha: " + Convert.ToDateTime(ClienteDocumentos.GetResolucionJudicialById(id)[0].FECHA).ToShortDateString();
            case "DocumentoPrivado":
                return "Documento privado - Fecha: " + Convert.ToDateTime(ClienteDocumentos.GetDocumentoPrivadoById(id)[0].FECHA).ToShortDateString();
            case "DocumentoAdministrativo":
                return "Documento administrativo - Fecha: " + Convert.ToDateTime(ClienteDocumentos.GetDocumentoAdministrativoById(id)[0].FECHA).ToShortDateString();
            default:
                return null;
        }

    }

    /// <summary>
    /// Dejamos la tabla del dataSet en estado borrador y con los datos de una declaracion de tipo complementaria, de esta manera podremos
    /// crear una nueva declaración de tipo complementaria
    /// </summary>
    private void DseDeclaracionIsaiMantDatosTipoComplementariaEstadoBorrador()
    {

        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACIONINICIAL = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].NUMPRESENTACION;
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetNUMPRESENTACIONNull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAPRESENTACIONNull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAPAGONull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetLINEACAPTURANull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO = EstadosPago.SinPago.ToDecimal();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetBANCONull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetSUCURSALNull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHALINEACAPTURANull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFUTNull();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAVIGENCIALINEACAPTURANull();

    }



    /// <summary>
    /// Dejamos todas las row de todas la tablas del dataset en estado ADDED
    /// </summary>
    private void DseDeclaracionIsaiMantAllToStateAdded()
    {

        DseDeclaracionIsaiMant.AcceptChanges();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetAdded();
        foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowP in DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Rows)
        {
            if (rowP.RowState != DataRowState.Deleted)
                rowP.SetAdded();
        }
        foreach (DseDeclaracionIsai.FEXNOT_CONDONACIONRow rowC in DseDeclaracionIsaiMant.FEXNOT_CONDONACION.Rows)
        {
            rowC.SetAdded();
        }

    }




    /// <summary>
    /// Método que comprueba si se ha seleccionado un banco del drop down list para habilitar o no el botón de pagar
    /// </summary>
    private void ComprobarBanco()
    {
        if (ddlCatBanco.SelectedValue == Constantes.UI_DDL_VALUE_NO_SELECT)
        {
            btnPagar.Enabled = false;
        }
        else
        {
            btnPagar.Enabled = true;
        }
    }






    /// <summary>
    /// Crea un DataTable con una nueva row de declaración con los datos de los controles del formualario.
    /// </summary>
    /// <returns>DataTable de declaración</returns>
    protected void CrearDeclaracionDTFormulario(bool borrador)
    {
        decimal decimalParse;
        DateTime datetimeParse;
        DseDeclaracionIsai.FEXNOT_DECLARACIONRow formularioDeclaracionRow;
        try
        {
            formularioDeclaracionRow = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0];
            //Detalle del avaluo      
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDAVALUONull())
                formularioDeclaracionRow.IDAVALUO = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO;
            if (!string.IsNullOrEmpty(lblValorComercialDato.Text))
                formularioDeclaracionRow.VALORAVALUO = lblValorComercialDato.Text.ToDecimalFromStringFormatted();

            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) == 0)
            {
                if (!string.IsNullOrEmpty(txtRegion.Text.Trim()))
                    lblRegionDato.Text = txtRegion.Text.Trim();
                if (!string.IsNullOrEmpty(txtManzana.Text.Trim()))
                    lblManzanaDato.Text = txtManzana.Text.Trim();
                if (!string.IsNullOrEmpty(txtLote.Text.Trim()))
                    lblLoteDato.Text = txtLote.Text.Trim();
                if (!string.IsNullOrEmpty(txtUnidadPrivativa.Text.Trim()))
                    lblUnidadPrivativaDato.Text = txtUnidadPrivativa.Text.Trim();
            }

            //Detalle del inmueble
            if (!string.IsNullOrEmpty(lblRegionDato.Text))
                formularioDeclaracionRow.REGION = lblRegionDato.Text.Trim();
            if (!string.IsNullOrEmpty(lblManzanaDato.Text))
                formularioDeclaracionRow.MANZANA = lblManzanaDato.Text.Trim();
            if (!string.IsNullOrEmpty(lblLoteDato.Text))
                formularioDeclaracionRow.LOTE = lblLoteDato.Text.Trim();
            if (!string.IsNullOrEmpty(lblUnidadPrivativaDato.Text))
                formularioDeclaracionRow.UNIDADPRIVATIVA = lblUnidadPrivativaDato.Text.Trim();
            if (ExisteValorCatastral())
            {
                decimal cat = 0;
                if (decimal.TryParse(ObtenerValorCatastral(), out cat))
                    formularioDeclaracionRow.VALORCATASTRAL = cat;
                else
                    formularioDeclaracionRow.SetVALORCATASTRALNull();
            }
            else
            {
                formularioDeclaracionRow.SetVALORCATASTRALNull();
            }
            if (!string.IsNullOrEmpty(lblValorAdquisicionDato.Text))
            {
                decimal cata = 0;
                if (decimal.TryParse(lblValorAdquisicionDato.Text, out cata))
                    formularioDeclaracionRow.VALORADQUISICION = cata;
                else
                    formularioDeclaracionRow.SetVALORADQUISICIONNull();
            }
            else
            {
                formularioDeclaracionRow.SetVALORADQUISICIONNull();
            }
            if (!string.IsNullOrEmpty(txtValorAdquisicionDato.Text))
                formularioDeclaracionRow.VALORADQUISICION = txtValorAdquisicionDato.Text.ToDecimalFromStringFormatted();

            if (ExisteValorCatastral())
            {
                string valorCat = ObtenerValorCatastral();
                formularioDeclaracionRow.VALORCATASTRAL = valorCat.ToDecimalFromStringFormatted();
            }

            //Declaracion.
            if (!string.IsNullOrEmpty(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION.ToString()))
                formularioDeclaracionRow.IDDECLARACION = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION;
            if (!string.IsNullOrEmpty(lblNumPresentacionDato.Text) && lblNumPresentacionDato.Text.StartsWith("I-"))
            {
                formularioDeclaracionRow.NUMPRESENTACION = lblNumPresentacionDato.Text;
            }
            if (!string.IsNullOrEmpty(lblNumPresentacionAntDato.Text) && lblNumPresentacionAntDato.Text.StartsWith("I-"))
                formularioDeclaracionRow.NUMPRESENTACIONINICIAL = lblNumPresentacionAntDato.Text;

            if (lblEstadoDato.Text.CompareTo(Resource.LIT_DEC_EST_PRESENTADA) == 0)
            {
                formularioDeclaracionRow.CODESTADODECLARACION = Constantes.PAR_VAL_ESTADODECLARACION_PRESENTADA.To<Decimal>();
            }
            else
            {
                if (borrador)
                    formularioDeclaracionRow.CODESTADODECLARACION = Constantes.PAR_VAL_ESTADODECLARACION_BORR.To<Decimal>();
                else
                    formularioDeclaracionRow.CODESTADODECLARACION = Constantes.PAR_VAL_ESTADODECLARACION_PDTE_PRESENTAR.To<Decimal>();
            }

            //catalogo de la cache (ESTADO DECLARACION)
            DseCatalogo.FEXNOT_CATESTADDECLARACIONRow rowED = (DseCatalogo.FEXNOT_CATESTADDECLARACIONRow)ApplicationCache.DseCatalogoISAI.FEXNOT_CATESTADDECLARACION.Select(Constantes.PAR_CODESTADODECLARACION + "=" + formularioDeclaracionRow.CODESTADODECLARACION.ToString()).First();
            formularioDeclaracionRow.ESTADO = rowED.DESCRIPCION;
            formularioDeclaracionRow.CODTIPODECLARACION = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION;
            formularioDeclaracionRow.ESHABITACIONAL = chkbHabitacional.Checked.ToOracleCharSNFromBoolean();
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsNull(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.IDDECLARACIONPADREColumn))
                formularioDeclaracionRow.IDDECLARACIONPADRE = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACIONPADRE;
            //Impuesto
            if (!string.IsNullOrEmpty(lblPorcRegargoExTempDato.Text))
                formularioDeclaracionRow.PORCJERECARGOPAGOEXTEMP = lblPorcRegargoExTempDato.Text.ToDecimalFromStringFormatted();
            if (!string.IsNullOrEmpty(lblActualizacionImporteDato.Text)) //review
                formularioDeclaracionRow.ACTUALIZACIONIMPORTE = lblActualizacionImporteDato.Text.ToDecimalFromStringFormatted();
            if (!string.IsNullOrEmpty(lblImpuestoCalculadoDato.Text))
                formularioDeclaracionRow.IMPORTEIMPUESTO = lblImpuestoCalculadoDato.Text.ToDecimalFromStringFormatted();
            //Pago
            if (!string.IsNullOrEmpty(lblBancoDato.Text))
                formularioDeclaracionRow.BANCO = lblBancoDato.Text.Trim();
            if (!string.IsNullOrEmpty(lblSucursalDato.Text))
                formularioDeclaracionRow.SUCURSAL = lblSucursalDato.Text.Trim();
            if (DateTime.TryParse(lblFechaLineaDato.Text, out datetimeParse))
            {
                formularioDeclaracionRow.FECHALINEACAPTURA = datetimeParse;
                formularioDeclaracionRow.LINEACAPTURA = lblLineaDato.Text.Trim();
            }
            formularioDeclaracionRow.CODESTADOPAGO = EstadosPago.SinPago.ToDecimal();
            if (DateTime.TryParse(lblFechaPagoDato.Text, out datetimeParse))
            {
                formularioDeclaracionRow.CODESTADOPAGO = EstadosPago.Pagado.ToDecimal();
                formularioDeclaracionRow.FECHAPAGO = datetimeParse;
            }

            //NOTA:Actualizar el idpersona se hace en CargarDatosFormulario()-->(antes)formularioDeclaracionRow.IDPERSONA = Convert.ToDecimal(Usuarios.IdPersona());
            //Acto Juridico
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_COMPLE) == 0 ||
                (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION > 0))
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
                    formularioDeclaracionRow.CODACTOJUR = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR;
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlActoJuridicoDato.SelectedValue))
                {
                    formularioDeclaracionRow.CODACTOJUR = ddlActoJuridicoDato.SelectedValue.To<Decimal>();
                    formularioDeclaracionRow.ACTOJURIDICO = ddlActoJuridicoDato.Items.FindByValue(ddlActoJuridicoDato.SelectedValue).Text;
                }
            }
            //Procedencia
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_COMPLE) == 0 ||
                (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION > 0))
            {
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODPROCEDENCIANull())
                    formularioDeclaracionRow.CODPROCEDENCIA = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODPROCEDENCIA;
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlPaisDato.SelectedValue))
                {
                    formularioDeclaracionRow.CODPROCEDENCIA = ddlPaisDato.SelectedValue.To<Decimal>();
                    formularioDeclaracionRow.PROCEDENCIA = ddlPaisDato.Items.FindByValue(ddlPaisDato.SelectedValue).Text;
                }
            }

            //Beneficios Fiscales
            if (rbExencion.Checked)
            {
                if (ucExencion.IdExencion != Constantes.UI_DDL_VALUE_NO_SELECT.ToInt())
                    formularioDeclaracionRow.IDEXENCION = ucExencion.IdExencion.To<Decimal>();
            }
            else formularioDeclaracionRow.SetIDEXENCIONNull();
            if (rbReduccion.Checked)
            {
                if (ucReduccion.IdReducccion != Constantes.UI_DDL_VALUE_NO_SELECT.ToInt())
                    formularioDeclaracionRow.IDREDUCCION = ucReduccion.IdReducccion.To<Decimal>();
            }
            else formularioDeclaracionRow.SetIDREDUCCIONNull();
            if (rbSubsidio.Checked)
            {
                if (decimal.TryParse(txtPorcentajeSubsidio.Text, out decimalParse))
                    formularioDeclaracionRow.PORCENTAJESUBSIDIO = (decimalParse / 100);
            }
            else formularioDeclaracionRow.SetPORCENTAJESUBSIDIONull();
            if (rbDisminucion.Checked)
            {
                if (decimal.TryParse(txtPorcentajeDisminucíon.Text, out decimalParse))
                    formularioDeclaracionRow.PORCENTAJEDISMINUCION = (decimalParse / 100);
            }
            else formularioDeclaracionRow.SetPORCENTAJEDISMINUCIONNull();

            if (rbCondonacion.Checked)
            {
                if (decimal.TryParse(txtPorcentajeCondonacion.Text, out decimalParse))
                    formularioDeclaracionRow.PORCENTAJECONDONACION = (decimalParse / 100);
                if (!string.IsNullOrEmpty(txtMotivoCondonacion.Text))
                    formularioDeclaracionRow.MOTIVOCONDONACION = txtMotivoCondonacion.Text.Trim();
                if (DateTime.TryParse(txtFechaCondonacion.Text, out datetimeParse))
                    formularioDeclaracionRow.FECHACONDONACION = datetimeParse;
            }
            else
            {
                formularioDeclaracionRow.SetPORCENTAJECONDONACIONNull();
                formularioDeclaracionRow.SetMOTIVOCONDONACIONNull();
                formularioDeclaracionRow.SetFECHACONDONACIONNull();
            }
            if (rbTasaCero.Checked)
            {
                formularioDeclaracionRow.ESTASACERO = Constantes.PAR_VAL_TRUE;
            }
            else formularioDeclaracionRow.ESTASACERO = Constantes.PAR_VAL_FALSE;

            if (String.IsNullOrEmpty(CondonacionJornada.ToString()))
                formularioDeclaracionRow.SetIDJORNADANOTARIALNull();
            else
                formularioDeclaracionRow.IDJORNADANOTARIAL = (decimal)CondonacionJornada;
            //Impuesto
            if (!string.IsNullOrEmpty(txtImpuestoNotarioDato.Text))
                formularioDeclaracionRow.IMPUESTOPAGADO = txtImpuestoNotarioDato.Text.ToDecimalFromStringFormatted();
            else
                formularioDeclaracionRow.SetIMPUESTOPAGADONull();

            if (!string.IsNullOrEmpty(txtLugar.Text))
                formularioDeclaracionRow.LUGARJORNADANOTARIAL = txtLugar.Text.Trim();
            else
                formularioDeclaracionRow.SetLUGARJORNADANOTARIALNull();

            //Participacion
            if ((!String.IsNullOrEmpty(lblTotalActoJurDato.Text)))
            {
                formularioDeclaracionRow.PORCACTOTOTAL = lblTotalActoJurDato.Text.ToDecimalFromStringFormatted();
            }

            //Fecha causacion

            if (DateTime.TryParse(txtFechaCausacion.Text, out datetimeParse))
                formularioDeclaracionRow.FECHACAUSACION = Convert.ToDateTime(txtFechaCausacion.Text);

            //Fecha preventiva, declaracion anticipada
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_ANTI)
            {
                if (DateTime.TryParse(txtFechaPrev.Text, out datetimeParse))
                    formularioDeclaracionRow.FECHAPREVENTIVA = Convert.ToDateTime(txtFechaPrev.Text);
            }

        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    protected DateTime GetDate(int regla)
    {
        //        reglas vigentes 0
        //        reglas fallecimiento 1
        DateTime fecha = DateTime.Now; DateTime fechaParseada;
        if (regla == 1)//Si reglas de fallecimiento va la fecha causacion
        {
            if (DateTime.TryParse(txtFechaCausacion.Text, out fechaParseada))
            {

                fecha = fechaParseada;
            }
            else if (DateTime.TryParse(lblFechaCausacionDato.Text, out fechaParseada))
            {

                fecha = fechaParseada;
            }
            else
                fecha = DateTime.Now;
        }
        else if (regla == 0)//Si actuales va la fecha de escritura
        {
            if (DateTime.TryParse(lblFechaEscrituraDato.Text, out fechaParseada))
            {

                fecha = fechaParseada;
            }
            else
            {
                throw new Exception("No se ha definido la fecha de escritura");
            }
        }
        return fecha;

    }

    /// <summary>
    /// Actualiza los datos calculados sobre el impuesto
    /// </summary>
    private void ActualizarDatosCalculo(bool cambio)
    {
        bool haveReduction = false;
        bool HaveExention = false;
        bool haveOtherBenef = false;
        decimal? codactojuridico = null;
        decimal? exencion = null;
        decimal? reduccion = null;
        decimal? subsidio = null;
        decimal? disminucion = null;
        decimal? condonacion = null;
        decimal? condonacionJornada = null;
        decimal impuesto;
        decimal? resultadoRecargo = null;
        decimal? resultadoActualizacion = null;
        decimal? resultadoImporteActualizacion = null;
        decimal? resultadoImporteRecargo = null;
        decimal? resultadoBaseGravable = null;
        decimal? resultadoReduccion1995 = null;
        decimal? resultadoTasa1995 = null;
        decimal? resultadoImpuesto = null;
        decimal? resultadoReduccionArt309 = null;
        decimal? resultadoExencionImporte = null;
        decimal? resultadoImporteCondonacion = null;
        decimal? resultadoImporteCondonacionxFecha = null;
        decimal? valorReferido = null;
        DateTime? fechaValorReferido = null;
        DateTime? fechaEscritura = null;
        //decimal decimalParse;
        string EsTasaCero = null;

        DateTime fecha = new DateTime();
        DateTime fechaParseada;
        try
        {
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_ANTI)
            {
                //if (DateTime.TryParse(txtFechaCausacion.Text, out fechaParseada))
                //{

                //    fecha = fechaParseada;
                //}
                //else if (DateTime.TryParse(lblFechaCausacionDato.Text, out fechaParseada))
                //{

                //    fecha = fechaParseada;
                //}
                //else
                //    fecha = DateTime.Now;
                if (rbReglasFallecimiento.Visible)//Si está visible es porque se seleccionó herencia
                {
                    if (rbReglasFallecimiento.Checked)
                    {
                        fecha = GetDate(1);//Si aplican las reglas de fallecimiento se envia 1 para usar la fecha causacion
                    }
                    else
                    {
                        fecha = GetDate(0);//Si aplican las vigentes se envia 0 para usar la fecha de escritura
                    }
                }
                else
                {
                    fecha = GetDate(1);//Se envia 1 porque se sigue tomando la fecha causacion
                }

            }
            else
            {
                fecha = DateTime.Now;
            }
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
            {
                if (!string.IsNullOrEmpty(ddlActoJuridicoDato.SelectedValue))
                    codactojuridico = ddlActoJuridicoDato.SelectedValue.To<Decimal>();
                else
                    throw new ImpuestoException("No se cuenta con un acto juridico");
            }
            else
            {
                codactojuridico = (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR;
            }
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
                fechaEscritura = (DateTime?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA;
            else
            {
                if (!string.IsNullOrEmpty(lblFechaEscrituraDato.Text))
                {
                    try
                    {
                        fechaEscritura = (DateTime?)Convert.ToDateTime(lblFechaEscrituraDato.Text);
                    }
                    catch
                    {
                        throw new ImpuestoException("No se cuenta con una fecha de escritura");
                    }
                }
                else
                    throw new ImpuestoException("No se cuenta con una fecha de escritura");
            }

            //Solo puede existir 1 ó 0 beneficios fiscales. Los demas tienen que ser nulos.
            if (rbExencion.Checked)
            {
                exencion = (ucExencion.IdExencion == Constantes.UI_DDL_VALUE_NO_SELECT.ToInt()) ? null : (decimal?)ucExencion.IdExencion.To<decimal>();
                HaveExention = true;
            }
            else if (rbTasaCero.Checked)
                EsTasaCero = Constantes.PAR_VAL_TRUE;
            else if (rbReduccion.Checked)
            {
                reduccion = (ucReduccion.IdReducccion == Constantes.UI_DDL_VALUE_NO_SELECT.ToInt()) ? null : (decimal?)ucReduccion.IdReducccion.To<decimal>();
                haveReduction = true;
            }
            else if (rbSubsidio.Checked)
            {
                subsidio = (txtPorcentajeSubsidio.Text.ToDecimalFromStringFormatted() / 100);
                haveOtherBenef = true;
            }
            else if (rbDisminucion.Checked)
            {
                disminucion = (txtPorcentajeDisminucíon.Text.ToDecimalFromStringFormatted() / 100);
                haveOtherBenef = true;
            }
            else if (rbCondonacion.Checked)
            {
                condonacion = (txtPorcentajeCondonacion.Text.ToDecimalFromStringFormatted() / 100);
                haveOtherBenef = true;
            }
            else
                condonacionJornada = string.IsNullOrEmpty(CondonacionJornada.ToString()) ? null : (decimal?)CondonacionJornada;



            if ((!string.IsNullOrEmpty(lblTotalActoJurDato.Text)))
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL = lblTotalActoJurDato.Text.ToDecimalFromStringFormatted();


            //valor referido <> null,valor del avalúo se debe coger el valor referido

            decimal par_valoravaluo = lblValorComercialDato.Text.ToDecimalFromStringFormatted();
            if (DseAvaluo != null && DseAvaluo.FEXAVA_AVALUO_V != null && DseAvaluo.FEXAVA_AVALUO_V.Any()
                && DseAvaluo.FEXAVA_AVALUO_V[0] != null)
            {
                if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull())
                    valorReferido = (decimal?)DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO;
                if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull())
                    fechaValorReferido = (DateTime?)DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO;
                //if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull() && DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO.Year < 1993)
                //    par_valoravaluo = DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO / 1000;
                //else
                //    par_valoravaluo = DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO;

            }



            string regla = rbReglasVigentes.Checked.ToOracleChar0FromBoolean();
            //string s = string.Format("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9} - {10} - {11} - {12} - {13} - {14} - {15} - {16} - {17} - {18}", DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION,
            //  lblTotalActoJurDato.Text.ToDecimalFromStringFormatted(),
            //  txtValorAdquisicionDato.Text.ToDecimalFromStringFormatted(),
            //  ObtenerValorCatastral().ToDecimalFromStringFormatted(),
            //  par_valoravaluo,
            //  chkbHabitacional.Checked.ToOracleCharSNFromBoolean(),
            //  EsTasaCero,
            //  exencion,
            //  reduccion,
            //  condonacionJornada,
            //  subsidio,
            //  disminucion,
            //  condonacion,
            //  fecha,
            //  Convert.ToDecimal(regla),
            //  valorReferido, fechaValorReferido, codactojuridico, fechaEscritura);
            //throw new ImpuestoException(s);

            impuesto = ClienteDeclaracionIsai.CalcularImpuestoDeclaracion(
              DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION,
              lblTotalActoJurDato.Text.ToDecimalFromStringFormatted(),
              txtValorAdquisicionDato.Text.ToDecimalFromStringFormatted(),
              ObtenerValorCatastral().ToDecimalFromStringFormatted(),
              par_valoravaluo,
              chkbHabitacional.Checked.ToOracleCharSNFromBoolean(),
              EsTasaCero,
              exencion,
              reduccion,
              condonacionJornada,
              subsidio,
              disminucion,
              condonacion,
              fecha,
              Convert.ToDecimal(regla),
              valorReferido, fechaValorReferido, codactojuridico, fechaEscritura,
              out resultadoRecargo,
               out resultadoImporteRecargo,
                out resultadoActualizacion,
                out resultadoImporteActualizacion,
                out resultadoBaseGravable,
                out resultadoReduccion1995,
                out resultadoTasa1995,
                out resultadoImpuesto,
                out resultadoReduccionArt309,
                out resultadoExencionImporte,
                out resultadoImporteCondonacion,
                out resultadoImporteCondonacionxFecha);


            //Mirar si es complementaria 
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_COMPLE))
            {
                //Como esta pagada el importeimpuesto e importepagado  no tien por qúe tener el mismo valor.
                decimal importeComple = -Convert.ToDecimal(DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO);
                if (!string.IsNullOrEmpty(impuesto.ToString()))
                {
                    importeComple = (Convert.ToDecimal(impuesto) - Convert.ToDecimal(DseDeclaracionIsaiPadreMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO));
                    lblImpuestoCalculadoDato.Text = System.Math.Round(importeComple + (0.49).ToDecimal()).ToString("$ #,###,##0");
                    if (cambio)
                        RellenarImpuestoDeclarado(importeComple, txtImpuestoNotarioDato);
                }
                //Si el resultado es cero o menor, no se da opción de FUT. 
                if (txtImpuestoNotarioDato.Text.ToDecimal() <= 0)
                {
                    rbCaja.Enabled = false;
                    txtModalPagoBanco.Enabled = false;
                    txtModalPagoSucursal.Enabled = false;
                }
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTECONDONACION = resultadoImporteCondonacion.ToDecimal();
            }
            else
            {
                if (!string.IsNullOrEmpty(impuesto.ToString()))
                {
                    lblImpuestoCalculadoDato.Text = impuesto.ToRoundTwo().ToString("$ #,###,##0"); //System.Math.Round(impuesto + (0.49).ToDecimal()).ToString("$ #,###,##0");
                    if (cambio)
                        RellenarImpuestoDeclarado(impuesto, txtImpuestoNotarioDato);
                }
            }
            if (!string.IsNullOrEmpty(resultadoImporteActualizacion.ToString()))
                lblActualizacionImporteDato.Text = resultadoImporteActualizacion.Value.ToCurrency();
            else
                lblActualizacionImporteDato.Text = "0";
            if (!string.IsNullOrEmpty(resultadoRecargo.ToString()))
                lblPorcRegargoExTempDato.Text = resultadoRecargo.Value.ToPercent();
            else
                lblPorcRegargoExTempDato.Text = "0.00 %";

            string resActualizacion;
            if (!string.IsNullOrEmpty(resultadoActualizacion.ToString()))
                resActualizacion = resultadoActualizacion.ToString();
            else
                resActualizacion = "0";
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEINPC = Convert.ToDecimal(resActualizacion);

            if (resultadoImporteCondonacionxFecha.HasValue && resultadoImporteCondonacionxFecha != 0)
            {
                lblCondonacionxFecha.Visible = lblCondonacionxFechaDato.Visible = true;
                lblCondonacionxFechaDato.Text = Convert.ToDecimal(resultadoImporteCondonacionxFecha.Value.ToString()).TruncateVal(2).ToString("C");
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CONDONACIONXFECHA = resultadoImporteCondonacionxFecha.Value;
            }
            else
            {
                lblCondonacionxFecha.Visible = lblCondonacionxFechaDato.Visible = false;
                lblCondonacionxFechaDato.Text = "$0.00";
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetCONDONACIONXFECHANull();
            }


            //Gule-Código para presentar desglose
            if (HaveExention)
            {
                if (resultadoExencionImporte != null && resultadoExencionImporte != 0)
                    lblBeneficioImporteDato.Text = decimal.Parse(resultadoExencionImporte.ToString()).ToRoundTwo().ToString("$ #,###,##0");
                else
                    lblBeneficioImporteDato.Text = "$00.00";
            }
            else if (haveReduction)
            {
                if (resultadoReduccionArt309 != null && resultadoReduccionArt309 != 0)
                    lblBeneficioImporteDato.Text = decimal.Parse(resultadoReduccionArt309.ToString()).ToRoundTwo().ToString("$ #,###,##0");
                else
                    lblBeneficioImporteDato.Text = "$00.00";
            }
            else if (haveOtherBenef)
            {
                if (resultadoImporteCondonacion != null && resultadoImporteCondonacion != 0)
                    lblBeneficioImporteDato.Text = decimal.Parse(resultadoImporteCondonacion.ToString()).ToRoundTwo().ToString("$ #,###,##0");
                else
                    lblBeneficioImporteDato.Text = "$00.00";
            }
            else
                lblBeneficioImporteDato.Text = "$00.00";
            if (resultadoImporteRecargo != null && resultadoImporteRecargo != 0)
            {
                lblRecargoImporteDato.Text = decimal.Parse(resultadoImporteRecargo.ToString()).TruncateVal(2).ToString("C"); //ToRoundTwo().ToString("$ #,###,##0");
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP = (decimal)resultadoImporteRecargo;
            }
            else
                lblRecargoImporteDato.Text = "$00.00";
            //Fin - gule
            updatePanelImpuesto.Update();
        }
        catch (ImpuestoException e)
        {
            MostrarInfoGuardar(e.Message, true);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string ErrorMessage = null;
            System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException>;
            if (faultEx != null)
            {
                if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                    ErrorMessage = faultEx.Detail.Descripcion;
                else
                    ErrorMessage = Constantes.MSJ_ERROR_OPERACION;
            }
            else
            {
                if (ex.InnerException != null)
                    ErrorMessage = ex.InnerException.Message;
                else
                    ErrorMessage = ex.Message;
            }
            MostrarInfoGuardar(ErrorMessage, true);
        }
    }


    /// <summary>
    /// Función que devuelve true si todos los campos de la cuenta catastral tienen valor y false en caso contrario
    /// </summary>
    /// <returns></returns>
    private bool HayCuentaCatastral()
    {
        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Any())
        {
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) == 0)//Solo se controla en Declaraciones de tipo Jornada Notarial, en el resto siempre existe
            {
                if (string.IsNullOrEmpty(txtRegion.Text) || string.IsNullOrEmpty(txtManzana.Text) || string.IsNullOrEmpty(txtLote.Text) || string.IsNullOrEmpty(txtUnidadPrivativa.Text))
                    return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Método que valida la cuenta catastral
    /// Obtiene el valor catastral
    /// </summary>
    private void ValidarCuentaCatastral()
    {
        try
        {
            string region = "";
            string manzana = "";
            string lote = "";
            string unidadPrivativa = "";
            if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) == 0)
            {
                region = txtRegion.Text.Trim();
                manzana = txtManzana.Text.Trim();
                lote = txtLote.Text.Trim();
                unidadPrivativa = txtUnidadPrivativa.Text.Trim();
            }
            else if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_ANTI) == 0)
            {
                region = lblRegionDato.Text;
                manzana = lblManzanaDato.Text;
                lote = lblLoteDato.Text;
                unidadPrivativa = lblUnidadPrivativaDato.Text;
            }
            else
            {
                region = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION;
                manzana = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA;
                lote = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE;
                unidadPrivativa = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;
            }

            txtCuentaCatastral.Text = region + "-" + manzana + "-" + lote + "-" + unidadPrivativa;

            if (!string.IsNullOrEmpty(region) || !string.IsNullOrEmpty(lblCuentaDato.Text))
            {
                //Detalle del inmueble
                if (!string.IsNullOrEmpty(txtRegion.Text))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION = region;
                if (!string.IsNullOrEmpty(txtManzana.Text))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA = manzana;
                if (!string.IsNullOrEmpty(txtLote.Text))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE = lote;
                if (!string.IsNullOrEmpty(txtUnidadPrivativa.Text))
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA = unidadPrivativa;

                string ErrorMessage = null;
                decimal? valorCatastralResult = null;
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                {

                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() == Constantes.PAR_VAL_TIPODECLARACION_ANTI)
                        valorCatastralResult = IsaiManager.ObtenerValorCatastral(DseDeclaracionIsaiMant, DseAvaluo, DateTime.Now, out ErrorMessage);
                    else
                        valorCatastralResult = IsaiManager.ObtenerValorCatastral(DseDeclaracionIsaiMant, DseAvaluo, Convert.ToDateTime(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION.ToShortDateString()), out ErrorMessage);
                }
                else
                {
                    HabilitarDeshabilitarValorCatastral(false);
                }

                if (valorCatastralResult.HasValue)
                {
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].VALORCATASTRAL = valorCatastralResult.Value;

                    HabilitarDeshabilitarValorCatastral(false);
                    AsignarValorCatastral(valorCatastralResult.Value);
                    //Si el tipo de declaracion es de jornada notarial y tenemos ya calculado el valor catastral entonces
                    //permitimos poder ir a participantes y guardara una declaracion de tipo jornada notarial
                    if ((Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_INS) == 0) &&
                        (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_JOR)) &&
                        (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull()))
                    {
                        btnGuardar.Enabled = true;
                    }
                    //beneficios fiscales Jornada Notarial
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_JOR))
                    {
                        ConfigurarBeneficiosFiscalesJornadaNotarial(valorCatastralResult.Value);
                        this.divBeneficiosFiscales.Visible = true;
                    }
                    ConfigurarTasaCero();
                    if (trTasaCero.Visible)
                        BeneficiosiscalesLeyend.Visible = false;
                    UpdatePanelValorCatastral.Update();
                    UpdatePanelCondonacionJornada.Update();
                    MostrarInfoValidarCuenta(Constantes.MSJ_ACT_VALORCAT, true);
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESVCATOBTENIDOFISCAL = Constantes.PAR_VAL_TRUE;
                    extenderPnlInfoValidarCuentaCatastralModal.Show();
                }
                else
                {
                    decimal valorCatActual = 0;
                    bool existeCuenta = IsaiManager.ComprobarExisteCuentaCatastral(region, manzana, lote, unidadPrivativa);
                    if (!existeCuenta)
                    {
                        BorrarValorCatastral();
                        HabilitarDeshabilitarValorCatastral(false);
                        MostrarMensajeInformacion(Constantes.MSJ_ERROR_NOEXISTE_CUENTACAT);

                    }
                    valorCatastralResult = null;
                    //Si hay un valor catastral ya introducido y no se ha encontrado un valor para la fecha y cuenta indicadas, mantener el valor anterior 
                    if (ExisteValorCatastral())
                    {
                        valorCatActual = Convert.ToDecimal(ObtenerValorCatastral());
                    }
                    else
                    {
                        if (lblValorCatastralDato.Visible)
                        {
                            txtValorCatastral.Text = String.Empty;
                            hidValorCatastral.Value = ObtenerValorCatastral();
                        }
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORCATASTRALNull();
                    }
                    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_TIPODECLARACION_JOR))
                        ConfigurarBeneficiosFiscalesJornadaNotarial(valorCatActual);

                    ConfigurarTasaCero();

                    HabilitarDeshabilitarValorCatastral(existeCuenta);
                    UpdatePanelValorCatastral.Update();
                    UpdatePanelCondonacionJornada.Update();

                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESVCATOBTENIDOFISCAL = Constantes.PAR_VAL_FALSE;
                    //TODO: MENSAJE INFORMATIVO ( SIN ESPECIFICAR EL VALOR DE LA CUENTA CATASTRAL )
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        if (ErrorMessage.Equals(Constantes.MSJ_ERROR_NOEXISTE_CUENTACAT))
                        {
                            BorrarValorCatastral();
                            HabilitarDeshabilitarValorCatastral(false);
                            ucParticipantes.ExisteCuenta = false;
                            //Si la cuenta introducida no existe deshabilitar la opción de introducir el valor catastral
                        }
                        else if (ErrorMessage.Equals(Constantes.MSJ_ERROR_NOEXISTE_VALCAT_FECHA))
                        {//Si el error es que no existe cuenta ya se ha mostrado antes
                            MostrarInfoValidarCuenta(ErrorMessage, true);
                            ucParticipantes.ExisteCuenta = true;
                        }
                        else
                            MostrarInfoValidarCuenta(ErrorMessage, true);
                    }
                    else
                    {
                        ucParticipantes.ExisteCuenta = true;
                    }
                }
            }
            hidValorCatastral.Value = ObtenerValorCatastral();

        }
        catch (Exception ex)
        {
            ConfigurarBeneficiosFiscalesJornadaNotarial(0);
            ExceptionPolicyWrapper.HandleException(ex);
            MostrarInfoValidarCuenta(ex.ToString(), true);
        }
    }

    #region valorcatastral

    /// <summary>
    /// el valor catastral puede estar en txtValorCatastral o lblValorCatastralDato en función de si
    /// se ha calculado por el sistema (lblValorCatastralDato) o se ha introducido por el usuario (txtValorCatastral) por no existir valor almacenado en el sistema
    /// </summary>
    /// <returns></returns>
    private string ObtenerValorCatastral()
    {

        if (txtValorCatastral.Visible && !String.IsNullOrEmpty(txtValorCatastral.Text))
        {
            return txtValorCatastral.Text;
        }
        else if (lblValorCatastralDato.Visible && !String.IsNullOrEmpty(lblValorCatastralDato.Text))
        {
            //Para no obtener el simbolo del euro cogemos todo menos el primer simbolo
            return lblValorCatastralDato.Text.Trim().Substring(1, lblValorCatastralDato.Text.Length - 1);
        }
        else
            return string.Empty;
    }

    /// <summary>
    /// True si existe valor, false e.o.c
    /// el valor catastral puede estar en txtValorCatastral o lblValorCatastralDato en función de si
    /// se ha calculado por el sistema (lblValorCatastralDato) o se ha introducido por el usuario (txtValorCatastral) por no existir valor almacenado en el sistema
    /// </summary>
    /// <returns></returns>
    private bool ExisteValorCatastral()
    {
        if (txtValorCatastral.Visible)
        {
            if (!string.IsNullOrEmpty(txtValorCatastral.Text))
                return true;
            else
                return false;
        }
        else if (lblValorCatastralDato.Visible)
        {
            {
                if (!string.IsNullOrEmpty(lblValorCatastralDato.Text))
                    return true;
                else
                    return false;
            }

        }
        else
            return false;

    }

    /// <summary>
    /// Método que asigna el valor pasado a los controles del valor catastral
    /// </summary>
    /// <param name="valor"></param>
    private void AsignarValorCatastral(decimal valor)
    {
        if (txtValorCatastral.Visible)
            txtValorCatastral.Text = valor.ToString();
        if (lblValorCatastralDato.Visible)
        {
            lblValorCatastralDato.Text = valor.ToCurrency();
        }
        hidValorCatastral.Value = ObtenerValorCatastral();
    }

    /// <summary>
    /// Se debe habilitar txtValorCatastral cuando haya una cuenta valida y  no haya un valor para la cuenta y la fecha introducidas (Para que el usuario introduzca el valor a mano)
    /// lblValorCatastralDato cuando haya un valor para la cuenta y la fecha introducidas
    /// </summary>
    /// <param name="habilitar"></param>
    private void HabilitarDeshabilitarValorCatastral(bool habilitar)
    {
        //Si se esta modificando o inserta y se oculta el label o el textbox que se estaba mostrando borrar los valores
        if (Operacion != Constantes.PAR_VAL_OPERACION_VER)
        {
            if (txtValorCatastral.Visible && !habilitar)
            {
                txtValorCatastral.Text = string.Empty;
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORCATASTRALNull();
            }
            if (lblValorCatastralDato.Visible && habilitar)
            {
                lblValorCatastralDato.Text = string.Empty;
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetVALORCATASTRALNull();
            }
        }

        txtValorCatastral.Enabled = habilitar;
        txtValorCatastral.Visible = habilitar;
        cvValorCatastral.Enabled = habilitar;
        lblValorCatastralDato.Visible = !habilitar;
    }

    /// <summary>
    /// Método que borra el valor catastral
    /// </summary>
    private void BorrarValorCatastral()
    {
        txtValorCatastral.Text = string.Empty;
        lblValorCatastralDato.Text = string.Empty;
    }

    #endregion

    /// <summary>
    /// Método que configura los beneficios fiscales de jornada notarial dependiendo del valor catastral
    /// </summary>
    /// <param name="valorCatastral"></param>
    private void ConfigurarBeneficiosFiscalesJornadaNotarial(decimal valorCatastral)
    {
        trCondonacionJornada.Visible = false;
        trTasaCero.Visible = false;
        rbTasaCero.Checked = false;
        trSinBeneficio.Visible = true;
        BeneficiosiscalesLeyend.Visible = false;

        if (valorCatastral > 0)
        {
            btnGuardar.Enabled = true;
            //aplica tasa cero?
            if (IsaiManager.HabilitarTasaCero(null, valorCatastral))
            {
                trTasaCero.Visible = true;
                rbTasaCero.Checked = true;
                rbTasaCero.Enabled = true;
                trSinBeneficio.Visible = false;
                BeneficiosiscalesLeyend.Visible = false;
            }
            else
            {
                //aplica condonacion
                decimal? porcentajeCondonacionJornada;
                CondonacionJornada = IsaiManager.ObtenerCondonacion(valorCatastral, DateTime.Now, out porcentajeCondonacionJornada);
                if (CondonacionJornada.HasValue)
                {
                    trCondonacionJornada.Visible = true;
                    lblCondonacionJornadaDato.Text = porcentajeCondonacionJornada.Value.ToPercent();
                    trSinBeneficio.Visible = false;
                    BeneficiosiscalesLeyend.Visible = false;
                }
            }
        }
    }


    /// <summary>
    /// Método que rellena el combo de motivos de rechazo
    /// </summary>
    private void CargarComboMotivosRechazo()
    {
        this.ddlMotivoRechazo.DataSource = ApplicationCache.DseCatalogoISAI.FEXNOT_CATMOTIVORECHAZO;
        this.ddlMotivoRechazo.DataTextField = ApplicationCache.DseCatalogoISAI.FEXNOT_CATMOTIVORECHAZO.DESCRIPCIONColumn.ToString();
        this.ddlMotivoRechazo.DataValueField = ApplicationCache.DseCatalogoISAI.FEXNOT_CATMOTIVORECHAZO.CODMOTIVORECHAZOColumn.ToString();
        this.ddlMotivoRechazo.DataBind();
        //insertar elemento de seleccion
        this.ddlMotivoRechazo.Items.Insert(0, new ListItem("Seleccione un Motivo...", "0"));
    }


    /// <summary>
    /// Muestra el panel de toma de decisión y carga el combo de motivos de rechazo
    /// </summary>
    private void PermitirTomaDecision()
    {
        divDecision.Visible = true;
        rbDecision.Visible = true;
        //el catalogo de motivos solo se carga si hace falta
        CargarComboMotivosRechazo();
    }


    /// <summary>
    /// Muestra la decisión tomada para la declaración
    /// </summary>
    /// <param name="decision">Texto opcional para el Motivo de Rechazo</param>
    private void MostrarDecision(string decision)
    {
        rowDecision.Attributes.Add("style", "display:none");
        divDecision.Visible = true;
        lblDecision.Visible = true;
        lblDecision.Text = decision;

    }


    /// <summary>
    /// Elimina la info referente al documento que acredita un beneficio fiscal
    /// </summary>
    private void ResetBeneficiosDocumentos()
    {
        txtTipoDocumentoBF.Text = string.Empty;
        txtListaIdsFicherosBF.Text = string.Empty;
        txtDocDigitalBF.Text = string.Empty;
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDDOCBENFISCALESNull();
        //Actualizar los botones
        btnDocBeneficios.Enabled = true;
        btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR;
        btnEditarDocBeneficios.Enabled = false;
        btnEditarDocBeneficios.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
        btnVerDocBeneficios.Enabled = false;
        btnVerDocBeneficios.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
        btnEliminarDocBeneficios.Enabled = false;
        btnEliminarDocBeneficios.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
        btnBorrarDocBeneficios_Click(null, null);

    }


    /// <summary>
    /// Método que activa o desactiva el panel de beneficios fiscales
    /// </summary>
    /// <param name="estado">Activa/Desactiva</param>
    /// <param name="RequiresJustification">Activa o desactiva el apartado de documentos</param>
    private void ActivaSeccionBeneficiosFiscales(bool estado, bool RequiresJustification)
    {
        this.divBeneficiosFiscales.Visible = estado;
        this.BeneficiosiscalesLeyend.Visible = !this.divBeneficiosFiscales.Visible;
        this.imgBeneficiosOn.Enabled = !estado;
        this.imgBeneficiosOff.Enabled = estado;
        this.imgBeneficiosOn.ImageUrl = (estado) ? Constantes.IMG_ANADIR_DISABLED : Constantes.IMG_ANADIR;
        this.imgBeneficiosOff.ImageUrl = (estado) ? Constantes.IMG_ELIMINA : Constantes.IMG_ELIMINA_DISABLED;
        this.trBeneficiosJustificacion.Visible = estado ? false : RequiresJustification;
        this.trBeneficiosJustificacion2.Visible = estado ? false : RequiresJustification;
        trDesBeneficios.Visible = trBeneficiosJustificacion.Visible;
        //Aunque no sea necesario un justificante puede tenerlo y hay que mostrarlo
        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDDOCBENFISCALESNull())
        {
            this.trBeneficiosJustificacion.Visible = true;
            this.trBeneficiosJustificacion2.Visible = true;
            this.trDesBeneficios.Visible = true;
        }
    }


    /// <summary>
    /// Maneja el evento SelectedIndexChanged del contol ddlActoJuridicoDato
    /// Se eliminan los participantes existentes y se actualizan los botones
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e"></param>
    protected void CambioActoJuridico(object sender, EventArgs e)
    {
        try
        {
            if (ddlActoJuridicoDato.SelectedIndex > 0)
            {
                EsHerencia();
                //rbReglasVigentes.Checked = true;
                //rbReglasFallecimiento.Checked = false;
                //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA = 0;
                CodActoJuridicoParticipante = Convert.ToDecimal(ddlActoJuridicoDato.SelectedValue);
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR = CodActoJuridicoParticipante;
            }
            else
            {
                if (DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Select(string.Empty, string.Empty, DataViewRowState.CurrentRows).Length > 0)
                {   //avisar q se van a eliminar los participantes
                    //ModalPopupExtenderEliminarPar.Show();
                }
                else
                {
                    //solo deshabilitar boton
                    //ucParticipantes.ActualizarEstadoBotones(true);
                }
            }
            ConfigurarTasaCero();
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
    /// Maneja el evento ConfirmClick del control ModalConfirmarcion con id ModalConfirmarcionEliminar
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e"></param>
    protected void ProcesarRespuestaRolesParticipantes(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                ddlActoJuridicoDato.SelectedValue = ActoJurAnt;
            }
            else
            {
                //reset roles en row participantes
                foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow row in DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                    {
                        row.SetROLNull();
                        row.SetCODROLNull();

                    }
                }
                lblTotalActoJurDato.Text = string.Empty;
                updatePanelImpuesto.Update();
                ActualizarBotonesParticipantes();
                ucParticipantes.CargarGridViewPersonas();
                ActoJurAnt = ddlActoJuridicoDato.SelectedValue;
                ModalPopupExtenderEliminar.Hide();
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
    ///  Maneja el evento ConfirmClick del control ModalConfirmarcion con id ModalConfirmarcionEliminarPar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProcesarRespuestaParticipantes(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                //regresa al ultimo indice seleccionado
                int idx = 0;
                foreach (ListItem item in this.ddlActoJuridicoDato.Items)
                {
                    if (item.Value == DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR.ToString())
                    {
                        ddlActoJuridicoDato.SelectedIndex = idx;
                        break;
                    }
                    idx++;
                }
            }
            else
            {
                //Borrar participantes
                for (int i = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Rows.Count - 1; i >= 0; i--)
                {
                    DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Rows[i].Delete();
                }
                lblTotalActoJurDato.Text = string.Empty;
                updatePanelImpuesto.Update();
                ActualizarBotonesParticipantes();
                ModalPopupExtenderEliminarPar.Hide();
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
    /// Actualiza los botones de los participantes dependiendo de si hay acto jurídico seleccionado
    /// </summary>
    private void ActualizarBotonesParticipantes()
    {
        if (ddlActoJuridicoDato.SelectedValue == string.Empty)
            ucParticipantes.ActualizarEstadoBotones(true); //No hay un acto jurídico seleccionado
        else
            ucParticipantes.ActualizarEstadoBotones(false);
    }


    /// <summary>
    /// Maneja el evento ConfirmClick del control ModalConfirmarcion con id ModalConfirmarcionBorrarDoc
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProcesarRespuestaBorrarDoc(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (!e.Cancel)
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetIDDOCUMENTODIGITALNull();
                btnDocumentos0.Enabled = true;
                btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR;
                btnBorrarDocumento.Enabled = false;
                btnBorrarDocumento.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnEditarDocumento.Enabled = false;
                btnEditarDocumento.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnVerDoc.Enabled = false;
                btnVerDoc.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
                //Eliminar todos los datos relacionados con la escritura introducida
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHAESCRITURANull();
                lblFechaEscrituraDato.Text = "Sin especificar";
                lblDescripcionDoc.Text = "No hay documentos registrados";
                Descrip = lblDescripcionDoc.Text;
                DescripBF = lblDescripcionDoc.Text;
                if (XmlDocumental != null && XmlDocumental.Length != 0)
                    XmlDocumental = string.Empty;
                if (TipoDocumental != null && TipoDocumental.Length != 0)
                    TipoDocumental = string.Empty;
                if (ListaIdsDocumental != null && ListaIdsDocumental.Length != 0)
                    ListaIdsDocumental = string.Empty;
                //Borrar los onClic
                btnEditarDocumento.OnClientClick = string.Empty;
                btnVerDoc.OnClientClick = string.Empty;


                txtIdDoc.Text = string.Empty;
                ModalPopEliminarDoc.Hide();
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

    #region Actualizar fecha


    /// <summary>
    /// Función que devuelve si existe la cuenta catastral introducida o no
    /// </summary>
    /// <returns></returns>
    private bool ExisteCuenta()
    {
        bool existeCuenta = false;
        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) != 0)//si no es de tipo jornada notarial la cuenta viene del avalúo y siempre existe
        {
            existeCuenta = true;
        }
        else
        {
            if (HayCuentaCatastral())
                existeCuenta = IsaiManager.ComprobarExisteCuentaCatastral(txtRegion.Text, txtManzana.Text, txtLote.Text, txtUnidadPrivativa.Text);
        }
        return existeCuenta;

    }


    /// <summary>
    /// Método que realiza las acciones necesarias trás eliminar la fecha
    /// Valida la cuenta y el valor catastral
    /// </summary>
    private void AccionesFechaBorrada()
    {
        //Si se ha borrado la fecha y si se estaba mostrando un valor calculado por el sistema (lblValorCatastralDato.Visible y con texto) habilitar el textbox y mantener valor
        if (ExisteCuenta())
        {
            if (lblValorCatastralDato.Visible)
            {
                ///decimal valAnt = ObtenerValorCatastral().ToDecimal();
                HabilitarDeshabilitarValorCatastral(true);
                //AsignarValorCatastral(valAnt);
            }
        }
        else
        {
            MostrarInfoValidarCuenta(Constantes.MSJ_ERROR_NOEXISTE_CUENTACAT, true);
            BorrarValorCatastral();
            HabilitarDeshabilitarValorCatastral(false);
        }
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ESVCATOBTENIDOFISCAL = Constantes.PAR_VAL_FALSE;
    }


    /// <summary>
    /// Método que realiza los pasos siguientes trás actualizar la fecha.
    /// Valida la cuenta catastral
    /// </summary>
    private void ProcesoActualizarFecha()
    {
        try
        {

            ModalPopupExtenderFecha.Hide();

            if (string.IsNullOrEmpty(txtFechaCausacion.Text))//Si se ha dejado vacio
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHACAUSACIONNull();
                trTasaCero.Visible = false;
                rbTasaCero.Checked = false;
                AccionesFechaBorrada();

            }
            else
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION = Convert.ToDateTime(txtFechaCausacion.Text);
                rbReduccion.Enabled = true;
                rbExencion.Enabled = true;
                if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_ANTI)
                {
                    ValidarCuentaCatastral();
                }
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
    /// Maneja el evento ConfirmClick del control ModalConfirmarcion con id ModalActualizarFecha 
    /// Se ejecuta después de pinchar en accepatar tras las pregunta
    /// Esta acción conlleva eliminar el valor catastral, la fecha de escritura y el beneficio fiscal indicado, ¿Desea realizar la operación?
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProcesarRespuestaActualizarFecha(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                txtFechaCausacion.Text = FechaCausacionAnt;
            }
            else
            {
                ProcesoActualizarFecha();
                FechaCausacionAnt = txtFechaCausacion.Text;
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

    /// <summary>
    /// Maneja el evento ConfirmClick del control ModalConfirmarcion con id ModalConfirmarcionEliminarBF
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProcesarRespuestaBorrarValorCatastral(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
            {
                txtValorCatastral.Text = hidValorCatastral.Value;
                lblValorCatastralDato.Text = hidValorCatastral.Value;
            }
            else
            {
                //Eliminar beneficios fiscales
                ConfigurarBeneficiosFiscalesBorrar(null, null);
                ModalPopupExtenderEliminarBF.Hide();
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
    /// Devuelve verdadero si son iguales, falso en caso contrario
    /// </summary>
    private bool ComparaImpuestosCalculadoDeclarado()
    {
        decimal taxCalculado;
        decimal taxDeclarado;
        Decimal.TryParse(lblImpuestoCalculadoDato.Text.Replace("$", string.Empty).Replace(",", string.Empty), out taxCalculado);
        Decimal.TryParse(txtImpuestoNotarioDato.Text.Replace("$", string.Empty).Replace(",", string.Empty), out taxDeclarado);
        return (taxCalculado == taxDeclarado);
    }

    /// <summary>
    /// Configura el estilo dependiendo de si es un dato obligatorio o no
    /// </summary>
    private void ConfiguraEstiloImpuestoDeclarado()
    {
        if (txtImpuestoNotarioDato.Text.Trim().Length > 0 && !ComparaImpuestosCalculadoDeclarado())
            txtImpuestoNotarioDato.ForeColor = System.Drawing.Color.Red;
        else
            txtImpuestoNotarioDato.ForeColor = System.Drawing.Color.Black;
    }

    /// <summary>
    /// Si el impuesto calculado es negativo se rellena a cero
    /// </summary>
    /// <param name="impuesto"></param>
    private void RellenarImpuestoDeclarado(decimal impuesto, WebControl target)
    {
        if (target is TextBox)
        {
            if (impuesto > 0)
                ((TextBox)target).Text = impuesto.ToRoundTwo().ToString(); //System.Math.Round(impuesto + (0.49).ToDecimal()).ToDecimal().ToString();
            else
                ((TextBox)target).Text = "0";
        }
        else if (target is Label)
            ((Label)target).Text = impuesto.ToString("$ #,###,##0");
    }

    /// <summary>
    /// Función que obtiene el valor del impuesto pagado almacenado en una label
    /// </summary>
    /// <param name="source"></param>
    /// <param name="isCurrencyFormatted"></param>
    /// <returns></returns>
    private decimal? RecuperaImpuestoPagado(WebControl source, bool isCurrencyFormatted)
    {
        decimal impuestoPagado;
        decimal? impuestoActualmentePagado = null;
        if (source is Label)
            if (isCurrencyFormatted)
            {
                if (Decimal.TryParse(((Label)source).Text.Replace("$", string.Empty).Replace(",", string.Empty).Trim(), out impuestoPagado))
                    impuestoActualmentePagado = impuestoPagado;
            }
            else
            {
                if (Decimal.TryParse(((Label)source).Text.Trim(), out impuestoPagado))
                    impuestoActualmentePagado = impuestoPagado;
            }
        //otros controles aqui
        return impuestoActualmentePagado;
    }

    /// <summary>
    /// Método que configura los botones del documento
    /// Apariencia y eventos
    /// </summary>
    private void ConfigurarBotonesDoc()
    {
        btnDocumentos0.Enabled = false;
        btnDocumentos0.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
        btnEditarDocumento.Enabled = true;
        btnEditarDocumento.ImageUrl = Constantes.IMG_MODIFICA;
        btnBorrarDocumento.Enabled = true;
        btnBorrarDocumento.ImageUrl = Constantes.IMG_ELIMINA;
        string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlDocumental"]);
        string TipoDocumentalCorto = null;
        switch (TipoDocumental)
        {
            case "EscrituraPublica":
                TipoDocumentalCorto = "EP";
                break;
            case "ResolucionJudicial":
                TipoDocumentalCorto = "RJ";
                break;
            case "DocumentoPrivado":
                TipoDocumentalCorto = "DP";
                break;
            case "DocumentoAdministrativo":
                TipoDocumentalCorto = "DA";
                break;
        }
        btnEditarDocumento.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                                url,
                                "1",
                                TipoDocumentalCorto,
                                null,
                                Constantes.PAR_VAL_TRUE,
                                txtDocDigital.ClientID,
                                txtTipoDocumento.ClientID,
                                txtListaIdsFicheros.ClientID,
                                Constantes.PAR_VAL_TRUE, Utilidades.base64Encode(XmlDocumental), ListaIdsDocumental, null, Usuarios.IdPersona(), "DeclaracionISAI");
    }


    /// <summary>
    /// Método que configura los botones del documento de beneficios fiscales
    /// Apariencia y eventos
    /// </summary>
    private void ConfigurarBotonesDocBF()
    {
        btnDocBeneficios.Enabled = false;
        btnDocBeneficios.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
        btnEditarDocBeneficios.Enabled = true;
        btnEditarDocBeneficios.ImageUrl = Constantes.IMG_MODIFICA;
        btnEliminarDocBeneficios.Enabled = true;
        btnEliminarDocBeneficios.ImageUrl = Constantes.IMG_ELIMINA;
        string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlDocumental"]);

        btnEditarDocBeneficios.OnClientClick = string.Format("javascript:AbrirUrlDestino('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13})",
                               url,
                               "99",
                               null,
                               null,
                               Constantes.PAR_VAL_TRUE,
                               txtDocDigitalBF.ClientID,
                               txtTipoDocumentoBF.ClientID,
                               txtListaIdsFicherosBF.ClientID,
                               Constantes.PAR_VAL_TRUE, Utilidades.base64Encode(XmlDocumentalBF), ListaIdsDocumentalBF, null, Usuarios.IdPersona(), "DeclaracionISAI");
    }



    /// <summary>
    /// Método que configura los controles relacionados con el beneficio fiscal de tasa cero
    /// Valida si existe el valor catastral, obtiene el valor catastral y decide si se debe permitir tasa cero
    /// </summary>
    private void ConfigurarTasaCero()
    {
        decimal? valorCatastralActualizado = null;

        if (ExisteValorCatastral())
        {
            decimal temp;
            if (decimal.TryParse(ObtenerValorCatastral().ToDecimalFromStringFormatted().ToString(), out temp))
                valorCatastralActualizado = temp;
        }

        if (IsaiManager.HabilitarTasaCero(DseDeclaracionIsaiMant, valorCatastralActualizado))
        {
            trTasaCero.Visible = true;
            rbTasaCero.Enabled = true;
        }
        else
        {
            rbTasaCero.Enabled = false;
            rbTasaCero.Checked = false;
            trTasaCero.Visible = false;
        }
    }


    /// <summary>
    /// Maneja el evento textchanged del control txtfechapago
    /// Si es válida habilita el botón de pago telemático 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtFechaPago_TextChanged(object sender, EventArgs e)
    {
        //this.Validate("pago");
        //if (this.IsValid)
        //{
        //    btnPagoTelematico.Enabled = true;
        //    btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO;
        //    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ENPAPEL == "N")
        //        lblRealizarPago.Visible = true;
        //    lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_NOPAGADO;
        //    lblInsertarFecha.Visible = false;
        //    if (!btnObtener.Visible)
        //    {
        //        //GeneraFormatoPDF();
        //        btnObtener.Enabled = true;
        //        btnObtener.Visible = true;
        //    }
        //    pnlObtener.Update();
        //}
        //else
        //{
        //    btnPagoTelematico.Enabled = false;
        //    btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO_DISABLED;
        //    lblRealizarPago.Visible = false;
        //    lblEstadoPagoDato.Text = Resources.Resource.LIT_DEC_EST_PAGO_NOPAGADO;
        //    if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ENPAPEL == "N")
        //        lblInsertarFecha.Visible = true;
        //    extenderPnlInfoFechaPago.Show();
        //    if (btnObtener.Visible)
        //    {
        //        //GeneraFormatoPDF();
        //        btnObtener.Enabled = false;
        //        btnObtener.Visible = false;
        //    }
        //    pnlObtener.Update();
        //}

    }

    /// <summary>
    /// Maneja el evento textchanged del control txtFechaCausacion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtFechaCausacion_TextChanged(object sender, EventArgs e)
    {
        DeclaracionIsaiClient VerificarDiaFestivo = new DeclaracionIsaiClient();
        FechaCausacionAnt = VerificarDiaFestivo.VerificarDiaFestivo(txtFechaCausacion.Text.ToString()).ToShortDateString();
        if (!FechaCausacionAnt.Equals(txtFechaCausacion.Text))
            txtFechaCausacion.Text = FechaCausacionAnt;

        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TipoDeclaracion.Anticipada.ToDecimal())
        {
            this.Validate("txtFecha2");
            if (this.IsValid)
            {
                this.Validate("txtFecha");
                if (this.IsValid)
                {
                    if (!string.IsNullOrEmpty(FechaCausacionAnt))
                    {
                        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TipoDeclaracion.JornadaNotarial.ToDecimal())
                        {
                            if ((!string.IsNullOrEmpty(txtRegion.Text)
                                 && !string.IsNullOrEmpty(txtManzana.Text)
                                 && !string.IsNullOrEmpty(txtLote.Text)
                                 && !string.IsNullOrEmpty(txtUnidadPrivativa.Text)))
                                //Advertir que se va ha actualizar el valor catastral y los beneficios fiscales
                                ModalPopupExtenderFecha.Show();
                        }
                        else
                            ModalPopupExtenderFecha.Show();
                    }
                    else
                    {
                        HabilitarControlesCuentaCatastral(true);
                        ProcesoActualizarFecha();
                        FechaCausacionAnt = txtFechaCausacion.Text;
                    }
                    lblFechaCausacionDato.Text = txtFechaCausacion.Text;
                }
                else //si se ha métido un valor que no es vacío o fecha
                {
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHACAUSACIONNull();
                }
            }
            else //si se ha métido un valor que no es vacío o fecha
            {
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetFECHACAUSACIONNull();
            }
        }
    }




    /// <summary>
    /// Método que habilita/deshabilita los controles de la cuenta catastral
    /// </summary>
    /// <param name="habilitar">True/false</param>
    private void HabilitarControlesCuentaCatastral(bool habilitar)
    {
        btnValidarCuentaCatastral.Enabled = habilitar;
        txtRegion.Enabled = habilitar;
        txtManzana.Enabled = habilitar;
        txtLote.Enabled = habilitar;
        txtUnidadPrivativa.Enabled = habilitar;
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
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }
    #region mensajes
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
    /// <summary>
    /// Mostrar mensaje información.
    /// </summary>
    /// <param name="mensaje">El mensaje.</param>
    private void MostrarMensajeInformacion(string mensaje)
    {
        ModalInfoToken.TextoInformacion = mensaje;
        ModalInfoToken.TipoMensaje = true;
        extenderPnlInfoTokenModal.Show();
        uppInfo.Update();

    }
    /// <summary>
    /// Mostrar información validar cuenta.
    /// </summary>
    /// <param name="msj">El msj.</param>
    /// <param name="tipo">Verdadero para tipo.</param>
    private void MostrarInfoValidarCuenta(string msj, bool tipo)
    {
        ModalInfoValidarCuenta.TextoInformacion = msj;
        ModalInfoValidarCuenta.TipoMensaje = tipo;
        extenderPnlInfoValidarCuentaCatastralModal.Show();
    }
    /// <summary>
    /// Mostrar información guardar.
    /// </summary>
    /// <param name="msj">El msj.</param>
    /// <param name="tipo">Verdadero para tipo.</param>
    private void MostrarInfoGuardar(string msj, bool tipo)
    {
        ModalInfoGuardar.TextoInformacion = msj; ;
        ModalInfoGuardar.TipoMensaje = tipo;
        extenderPnlGuardarModalOK_Extender.Show();
    }
    #endregion
    #endregion





    /// <summary>
    /// Método que compruebra si se ha seleccionado como acto jurídico la herencia,
    /// para habilitar o no el valor de adquisición       
    /// </summary>
    private void EsHerencia()
    {
        try
        {
            if (ddlActoJuridicoDato.SelectedItem.Text == Constantes.PAR_VAL_HERENCIA)
            {
                lblValorAdquisicion.Visible = false;
                lblValorAdquisicionDolar.Visible = false;
                lblValorAdquisicionDato.Text = "0";
                lblValorAdquisicionDato.Visible = false;
                txtValorAdquisicionDato.Text = "0";
                txtValorAdquisicionDato.Visible = false;
                cvValorAdquisicionDato.Enabled = false;
                rbReglasFallecimiento.Visible = true;
                rbReglasVigentes.Visible = true;
                if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORREFERIDONull())
                {
                    rbReglasFallecimiento.Enabled =
                    rbReglasFallecimiento.Checked = true;

                    rbReglasVigentes.Checked =
                    rbReglasVigentes.Enabled = false;
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA = 1;
                }
                else
                {
                    rbReglasFallecimiento.Enabled =
                    rbReglasFallecimiento.Checked = false;

                    rbReglasVigentes.Checked =
                    rbReglasVigentes.Enabled = true;
                    DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA = 0;
                }

            }
            else
            {
                rbReglasFallecimiento.Checked = false;
                rbReglasVigentes.Checked = true;
                DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA = 0;
                //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGLA = 0;
                //}
                //else
                //{
                rbReglasFallecimiento.Enabled =
                rbReglasVigentes.Enabled = true;
                lblValorAdquisicion.Visible = true;
                lblValorAdquisicionDolar.Visible = true;
                txtValorAdquisicionDato.Visible = true;
                cvValorAdquisicionDato.Enabled = true;
                rbReglasFallecimiento.Visible = false;
                rbReglasVigentes.Visible = false;
            }

        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// ***********************************************************************
    /// <Date>29/10/2014</Date>
    /// <Modification>Se cambia para que tome el porcentaje de la suma de porcentajes en la tabla de participantes</Modification>
    /// <Author>Edgar</Author>
    protected void SujetoPasivoChanged(object sender, SujetoPasivoEventArgs e)
    {
        decimal porc = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Where(c => c.RowState != DataRowState.Deleted).Where(c => c.ROL.Equals(Constantes.PAR_VAL_ENAJENANTE)).Where(c => !c.IsTIPOPERSONANull()).Sum(c => c.PORCTRANSMISION).ToDecimal();
        lblTotalActoJurDato.Text = porc.ToPercent();
        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL = porc;
        //e.PorcTotal.ToDecimal().ToPercent();
        //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCACTOTOTAL = e.PorcTotal.ToDecimal();
        updatePanelImpuesto.Update();
    }


    protected void btnAdd_Click2(object sender, ImageClickEventArgs e)
    {
        try
        {
            valoresCache = listaValoresIsr;
            if (ucParticipantes.ExisteCuenta)
            {
                ((PageBaseISAI)(this.Page)).OperacionParticipantes = Constantes.PAR_VAL_OPERACION_INS;
                //Response.Redirect(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
                Server.Transfer(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
            }
            else
            {
                ucParticipantes.MostrarMensajeNoExisteCuentaCat();
            }
        }
        catch (System.Threading.ThreadAbortException)
        {

        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    public static System.Byte[] Sesionjust
    {

        get
        {
            if (HttpContext.Current.Session["justificante"] != null)
            {
                return (Byte[])HttpContext.Current.Session["justificante"] as System.Byte[];
            }
            else
            {
                return null;
            }
        }

        set
        {

            HttpContext.Current.Session["justificante"] = value;
        }

    }

    protected bool TieneBeneficios
    {
        get
        {
            return (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDEXENCIONNull() ||
                   !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull() ||
                   !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull() ||
                   !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull() ||
                   !DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull());
        }
    }

    public bool FechaVencida
    {
        get
        {
            bool r = false;
            string ret = string.Empty;
            DeclaracionIsaiClient cliente = new DeclaracionIsaiClient();
            ret = cliente.Verificarfechavencida(
                        DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION
                    ).ToUpper();
            r = ret.Equals("ADEUDO NO VIGENTE");

            return r;
        }
    }

    protected void btnObtener_Click(object sender, EventArgs e)
    {
        decimal par_valoravaluo = lblValorComercialDato.Text.ToDecimalFromStringFormatted();
        decimal? codactojuridico = null;
        decimal? valorReferido = null;
        DateTime? fechaValorReferido = null;
        if (!string.IsNullOrEmpty(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR.ToString()))
            codactojuridico = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODACTOJUR;
        if (DseAvaluo != null && DseAvaluo.FEXAVA_AVALUO_V != null && DseAvaluo.FEXAVA_AVALUO_V.Any()
            && DseAvaluo.FEXAVA_AVALUO_V[0] != null)
        {
            //if (!DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull() && DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO.Year < 1993)
            //    par_valoravaluo = DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO / 1000;
            //else
            //    par_valoravaluo = DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO;
            valorReferido = DseAvaluo.FEXAVA_AVALUO_V[0].IsVALORCATASTRALNull() ? null : (decimal?)DseAvaluo.FEXAVA_AVALUO_V[0].VALORREFERIDO;
            fechaValorReferido = DseAvaluo.FEXAVA_AVALUO_V[0].IsFECHAVALORREFERIDONull() ? null : (DateTime?)DseAvaluo.FEXAVA_AVALUO_V[0].FECHAVALORREFERIDO;

        }

        GenerarPDF gen = new GenerarPDF(lblTotalActoJurDato.Text.ToDecimalFromStringFormatted(),
            txtValorAdquisicionDato.Text.ToDecimalFromStringFormatted(),
            ObtenerValorCatastral().ToDecimalFromStringFormatted(),
            chkbHabitacional.Checked.ToOracleCharSNFromBoolean(),
            par_valoravaluo,
            rbReglasVigentes.Checked.ToOracleChar0FromBoolean(),
            Convert.ToDecimal(CondonacionJornada),
            codactojuridico);



        byte[] fut;
        fut = null;
        bool EstaVencido = FechaVencida;
        //GULE
        //VENCIDO CON BENEFICIOS
        if (EstaVencido && TieneBeneficios)
            fut = gen.GeneraPDFFMT(TipoPago.VencidoReduccion, DseDeclaracionIsaiMant, valorReferido, fechaValorReferido);
        //VENCIDO
        //if (Convert.ToDateTime(DateTime.Now.ToShortDateString()) > Convert.ToDateTime(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA.AddDays(15).ToShortDateString()))
        else if (EstaVencido && !TieneBeneficios)
            fut = gen.GeneraPDFFMT(TipoPago.Vencido, DseDeclaracionIsaiMant, valorReferido, fechaValorReferido);
        //VIGENTE CON BENEFICIOS
        else if (!EstaVencido && TieneBeneficios)
            fut = gen.GeneraPDFFMT(TipoPago.Reduccion, DseDeclaracionIsaiMant, valorReferido, fechaValorReferido);
        //VIGENTE
        //else if ((Convert.ToDateTime(DateTime.Now.ToShortDateString()) < Convert.ToDateTime(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA.AddDays(15).ToShortDateString())))
        else if (!EstaVencido && !TieneBeneficios)
            fut = gen.GeneraPDFFMT(TipoPago.Vigente, DseDeclaracionIsaiMant, valorReferido, fechaValorReferido);
        //Sesionjust = fut;

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=formatoPago.pdf");
        HttpContext.Current.Response.BinaryWrite(fut);
        HttpContext.Current.Response.End();
    }


    /// <summary>
    /// Obtenemos la línea de captura para poder realizar el pago de la declaración 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnObtenerLinea_Click(object sender, EventArgs e)
    {
        try
        {
            //servicio web para la generacion de la linea de captura para el impuesto sobre adquisicion de inmuebles(ISAI).
            string fechaCausacion = string.Empty;
            string ErrorMessage = null;
            DseDeclaracionIsai declaracion = DseDeclaracionIsaiMant;
            //Descomentar para obtener la linea en cualquier momento de la declaracion
            //declaracion.FEXNOT_DECLARACION[0].SetLINEACAPTURANull();
            //declaracion.FEXNOT_DECLARACION[0].CODESTADOPAGO = 0;
            //declaracion.FEXNOT_DECLARACION[0].CODESTADODECLARACION = 1;
            //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].SetLINEACAPTURANull();
            //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADOPAGO = 0;
            //DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODESTADODECLARACION = 1;
            try
            {
                fechaCausacion = string.Format("{0}-{1}-{2}", Convert.ToDateTime(txtFechaCausacion.Text).Year,
                    Convert.ToDateTime(txtFechaCausacion.Text).Month,
                    Convert.ToDateTime(txtFechaCausacion.Text).Day);
            }
            catch
            {
                try
                {
                    fechaCausacion = string.Format("{0}-{1}-{2}", Convert.ToDateTime(lblFechaCausacionDato.Text).Year,
                      Convert.ToDateTime(lblFechaCausacionDato.Text).Month,
                      Convert.ToDateTime(lblFechaCausacionDato.Text).Day);
                }
                catch
                {
                    throw new Exception("Favor de revisar la fecha causación");
                }
            }
            LineaCapturaResultMessage result = IsaiManager.ObtenerLineaCaptura(ref declaracion, fechaCausacion, out ErrorMessage);
            DseDeclaracionIsaiMant = declaracion;

            //mensaje de errordel servicio FUT
            if (result == LineaCapturaResultMessage.Exito)
            {
                btnObtenerLinea.Enabled = true;

                lblLineaDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA;
                lblFechaLineaDato.Text = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();

                ////JAMG
                ////VALIDO SI YA TRAE BIEN LA LINEA DE CAPTURA
                ////QUITO EL BOTN DE GENERAR LINEA DE CAPTURA
                ////22-04-2013
                if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsLINEACAPTURANull())
                {
                    EnviarCorreo(DseDeclaracionIsaiMant);
                    btnObtenerLinea.Visible = false;
                    //if (!btnObtener.Visible)
                    //{
                    //    //GeneraFormatoPDF();
                    //    btnObtener.Enabled = true;
                    //    btnObtener.Visible = true;
                    //}
                }
                //-----------------------------------

                //recargamos la declaracion
                RecargarDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
                CargarDatosFormulario();
                btnObtenerLinea.Enabled = false;
                //btnObtenerLinea.ImageUrl = Constantes.IMG_CAMBIAESTADO_DISABLED;
                lblObtenerLinea.Visible = false;

                rvFechaPago.MaximumValue = DateTime.Today.ToShortDateString();
                try
                {
                    rvFechaPago.MinimumValue = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHALINEACAPTURA.ToShortDateString();
                }
                catch
                {
                    rvFechaPago.MinimumValue = DateTime.Today.ToShortDateString();
                }

                //FUT
                string url = System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["UrlFUT"]);
                btnVerFUT.Enabled = true;
                btnVerFUT.ImageUrl = Constantes.IMG_ZOOM;
                btnVerFUT.OnClientClick = string.Format("javascript:AbrirUrlFut('{0}','{1}')", url, declaracion.FEXNOT_DECLARACION[0].FUT);
                //JAMG
                //26-04-2013
                //UpdatePanel5.Update(); 
                //-----termina jamg
                UpdatePanelDatosPresentacion.Update();
                //pnlObtener.Update();
            }
            else
            {
                btnObtenerLinea.Enabled = true;

                if (result == LineaCapturaResultMessage.ImpuestoEsCero && DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION == Convert.ToDecimal(TipoDeclaracion.Complementaria))
                    ModalInfoFUT.TextoInformacion = Constantes.MSJ_ERROR_SOLLINEA;
                else
                {
                    if (String.IsNullOrEmpty(ErrorMessage))
                        ModalInfoFUT.TextoInformacion = EnumUtility.GetDescription(result);
                    else
                        ModalInfoFUT.TextoInformacion = ErrorMessage;
                }
                ModalInfoFUT.TipoMensaje = true;
                extenderPnlInfoFUTModal.Show();
            }
        }
        catch (FaultException<DeclaracionIsaiException> diex)
        {
            ModalInfoFUT.TextoInformacion = diex.InnerException != null ? diex.InnerException.Message : diex.Message;
            ModalInfoFUT.TipoMensaje = true;
            extenderPnlInfoFUTModal.Show();
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            ModalInfoFUT.TextoInformacion = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ModalInfoFUT.TipoMensaje = true;
            extenderPnlInfoFUTModal.Show();
        }
    }

    /// <summary>
    ///  Habilita el div para la captura de los campos del ISR
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CBCausaISR_CheckedChanged(object sender, EventArgs e)
    {
        if (CBCausaISR.Checked)
        {
            DivISR.Visible = true;
        }
        else
        {

            DivISR.Visible = false;
        }

    }

    /// <summary>
    /// Valida los participantes para la captura de los datos del ISR
    /// </summary>
    /// <returns></returns>
    private bool ValidarucParticipantesISR()
    {
        Boolean Valida = false;

        if (CBCausaISR.Checked)
        {
            Valida = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Where(p => !p.IsROLNull())    //Seleccionamos todos los que tengan rol para que lo lance una excepcion
                                                                .Where(p => p.ROL.ToUpper().StartsWith("E")) //Seleccionamos los que empiezan con E, es decir, los enajenantes.
                                                                .Where(p => p.IsRFCNull()) //Seleccionamos todos los que no tengan rfc
                                                                .Count() == 0;//Si el count es 0 quiere decir que todos tienen rfc y la validacion es correcta

            //foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participante in DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Rows)
            //{
            //    Valida = false;

            //    if (!participante.IsRFCNull())
            //    {
            //        Valida = true;
            //    }
            //}
        }
        else
        {
            Valida = true;
        }

        return Valida;
    }

    protected void BImprirInformeISR_Click(object sender, EventArgs e)
    {
        try
        {

            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_INF_JUSTIFICANTE_ISR;
            RedirectUtil.AddParameter(Constantes.PAR_IDDECLARACION, Convert.ToString(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION));
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, Convert.ToString(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDAVALUO));
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

    protected void mostrarBotonPago(int estadoDeclaración)
    {
        try
        {
            EstadosDeclaraciones estado = (EstadosDeclaraciones)estadoDeclaración;
            switch (estado)
            {
                case EstadosDeclaraciones.Borrador:
                    btnPagoTelematico.Visible = false;
                    break;
                case EstadosDeclaraciones.Pendiente:
                    btnPagoTelematico.Visible = true;
                    btnPagoTelematico.Enabled = true;
                    btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO; ;
                    break;
                case EstadosDeclaraciones.Presentada:
                    btnPagoTelematico.Visible = true;
                    btnPagoTelematico.Enabled = false;
                    btnPagoTelematico.ImageUrl = Constantes.IMG_CARRO_DISABLED;
                    break;
            }
        }
        catch
        {
            btnPagoTelematico.Visible = false;
        }

    }

    protected void IsrReadOnly(int estadoDeclaracion)
    {
        try
        {
            EstadosDeclaraciones estado = (EstadosDeclaraciones)estadoDeclaracion;
            if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0 || estado != EstadosDeclaraciones.Borrador)
            {
                var controles = DivISR.Controls.Cast<Control>().Where(c => c is TextBox).ToList();
                //controles.ForEach(c => {((TextBox)c).ReadOnly = true); ((TextBox)c).BorderStyle = BorderStyle.None});
                controles.ForEach(c =>
                {
                    ((TextBox)c).ReadOnly = true;
                    ((TextBox)c).BorderStyle = BorderStyle.None;
                    string s = ((TextBox)c).Text;
                    ((TextBox)c).Text = string.IsNullOrEmpty(s) ? "$0.00" : s.ToDecimal().ToString("C");

                });
                CBCausaISR.Enabled = false;

            }
            else
            {
                CBCausaISR.Enabled = true;
            }
            btnObtenerFutIsr.Visible = estado != EstadosDeclaraciones.Borrador;
            updatePanelImpuesto.Update();
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
    protected void CargarValoresISR(decimal idDeclaracion)
    {
        try
        {
            if (valoresCache.Any())
            {
                CargarValoresISR(valoresCache.ToArray());
            }
            else
            {
                decimal[] datos = ClienteDeclaracionIsai.GetDatosIsr(idDeclaracion);

                if (datos.Any())
                {
                    valoresCache.Clear();
                    valoresCache.AddRange(datos);
                    CargarValoresISR(datos);
                }
            }
            updatePanelImpuesto.Update();
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
    protected void CargarValoresISR(decimal[] datos)
    {

        _a = _a == 0 ? datos[0] : _a;
        _b = _b == 0 ? datos[1] : _b;
        _c = datos[2];
        _d = datos[3];
        _e = datos[4];
        _f = datos[5];
        _g = datos[6];
        _h = datos[7];
        _i = datos[8];
        _j = datos[9];
        _A = datos[10];
        _B = datos[11];
        _C = datos[12];
        _D = datos[13];
        _E = datos[14];
    }


    protected void btnObtenerFutIsr_Click(object sender, EventArgs e)
    {
        try
        {
            PdfIsr isr = new PdfIsr(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().CODESTADOPAGO == 1);
            using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
            {

                DseInfoContribuyente dseInfo = clienteRCON.GetInfoContribuyente(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IDPERSONA);
                var infoNotario = clienteRCON.GetInfoNotario(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IDPERSONA, true);
                if (infoNotario.Notario.Any())
                {
                    isr.rfcNotario = infoNotario.Notario.First().IsRFCNull() ? string.Empty : infoNotario.Notario.First().RFC;
                    isr.curpNotario = infoNotario.Notario.First().IsCURPNull() ? string.Empty : infoNotario.Notario.First().CURP;
                    isr.numNotario = infoNotario.Notario.FirstOrDefault().NUMNOTARIO.ToString();
                    isr.nombreNotario = string.Format("{0} {1} {2}", infoNotario.Notario.FirstOrDefault().IsAPELLIDOPATERNONull() ? string.Empty : infoNotario.Notario.FirstOrDefault().APELLIDOPATERNO,
                        infoNotario.Notario.FirstOrDefault().IsAPELLIDOPATERNONull() ? string.Empty : infoNotario.Notario.FirstOrDefault().APELLIDOMATERNO,
                        infoNotario.Notario.FirstOrDefault().NOMBRE);
                    if (infoNotario.Direcciones.Any())
                    {
                        isr.calleNotario = infoNotario.Direcciones.FirstOrDefault().IsIDVIANull() ? string.Empty : infoNotario.Direcciones.FirstOrDefault().VIA;
                        isr.delegacionNotario = infoNotario.Direcciones.FirstOrDefault().DELEGACION;
                        isr.coloniaNotario = infoNotario.Direcciones.FirstOrDefault().COLONIA;
                        isr.cpNotario = infoNotario.Direcciones.FirstOrDefault().IsCODIGOPOSTALNull() ? string.Empty : infoNotario.Direcciones.FirstOrDefault().CODIGOPOSTAL;
                        isr.numIntNotario = infoNotario.Direcciones.FirstOrDefault().IsNUMEROINTERIORNull() ? string.Empty : infoNotario.Direcciones.FirstOrDefault().NUMEROINTERIOR;
                        isr.numExtNotario = infoNotario.Direcciones.FirstOrDefault().NUMEROEXTERIOR;
                    }
                }
            }

            isr.fechaSentencia = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().FECHACAUSACION.ToShortDateString();
            decimal[] d = ClienteDeclaracionIsai.GetDatosIsr(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IDDECLARACION);

            if (d.Any())
            {
                isr.a = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTELOCALISRNull() ? d[0].ToString("C") : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTELOCALISR.ToString("C");
                isr.b = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTEFDERALISRNull() ? d[1].ToString("C") : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEFDERALISR.ToString("C");
                isr.c = d[2].ToString("C");
                isr.d = d[3].ToString("C");
                isr.e = d[4].ToString("C");
                isr.f = d[5].ToString("C");
                isr.g = d[6].ToString("C");
                isr.h = d[7].ToString("C");
                isr.i = d[8].ToString("C");
                isr.j = d[9].ToString("C");
                isr.A = d[10].ToString("C");
                isr.B = d[11].ToString("C");
                isr.C = d[12].ToString("C");
                isr.D = d[13].ToString("C");
                isr.E = d[14].ToString("C");
            }

            var persona = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Where(p => p.ROL.ToUpper().Equals("ENAJENANTE")).OrderBy(r => r.PORCTRANSMISION).FirstOrDefault();
            isr.rfcEnajenante = persona.IsRFCNull() ? string.Empty : persona.RFC;
            isr.curpEnajenante = persona.IsCURPNull() ? string.Empty : persona.CURP;
            isr.nombreEnajenante = string.Format("{0} {1} {2}", persona.IsAPELLIDOPATERNONull() ? string.Empty : persona.APELLIDOPATERNO,
                persona.IsAPELLIDOMATERNONull() ? string.Empty : persona.APELLIDOMATERNO,
                persona.NOMBRE);

            using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
            {

                var direccion = clienteRCON.GetDireccionById(persona.IDDIRECCION).Direccion.FirstOrDefault();
                isr.calleEnajenante = direccion.VIA;
                isr.coloniaEnajenante = direccion.COLONIA;
                isr.estadoEnajenante = ApplicationCache.EstadosCache.EstadoSEPOMEX.Where(es => es.CODESTADO == direccion.CODESTADO).FirstOrDefault().ESTADO;
                isr.numExtEnajenante = direccion.NUMEROEXTERIOR;
                isr.numIntEnajenante = direccion.IsNUMEROINTERIORNull() ? string.Empty : direccion.NUMEROINTERIOR;
                isr.delegacionEnajenante = direccion.DELEGACION;
                isr.cpEnajenante = direccion.IsCODIGOPOSTALNull() ? string.Empty : direccion.CODIGOPOSTAL;
                isr.telefono = direccion.IsTELEFONONull() ? string.Empty : direccion.TELEFONO;
            }
            using (ConsultasDocumentosClient cliente = new ConsultasDocumentosClient())
            {
                try
                {
                    DateTime fechaFirma = new DateTime();
                    var doc = cliente.GetEscrituraPublicaById(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IDDOCUMENTODIGITAL).FirstOrDefault();
                    if (doc != null)
                    {
                        isr.numEscritura = doc.IsNUMPROTOCOLONull() ? string.Empty : doc.NUMPROTOCOLO.ToString();
                        if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IsFECHAESCRITURANull())
                        {
                            fechaFirma = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().FECHAESCRITURA;
                        }
                        else if (!doc.IsFECHANull())
                        {
                            fechaFirma = doc.FECHA;

                        }
                        fechaFirma = fechaFirma > DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().FECHACAUSACION ? fechaFirma : DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().FECHACAUSACION;
                        isr.dia = fechaFirma.Day.ToString();
                        isr.mes = fechaFirma.Month.ToString();
                        isr.ano = fechaFirma.Year.ToString();

                    }
                }
                catch { }
            }
            isr.calleInmueble = lblCalleDato.Text;
            isr.coloniaInmueble = lblColoniaDato.Text;
            isr.municipioInmueble = lblDelegacionDato.Text;
            isr.numIntInmueble = lblNoIntDato.Text;
            isr.numExtInmueble = lblNoExtDato.Text;
            isr.region = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().REGION;
            isr.manzana = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().MANZANA;
            isr.lote = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().LOTE;
            isr.unidad = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().UNIDADPRIVATIVA;
            isr.digitoVerificador = DigitoVerificadorUtils.ObtenerDigitoVerificador(isr.region, isr.manzana, isr.lote, isr.unidad);


            byte[] fut = isr.CreatePDF();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=formatoPago.pdf");
            HttpContext.Current.Response.BinaryWrite(fut);
            HttpContext.Current.Response.End();
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