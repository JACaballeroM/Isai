using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using ServiceRCON;
using SIGAPred.Common.Extensions;


/// <summary>
/// Clase encargada de gestionar el formulario de participante
/// </summary>
public partial class PersonaDeclaracion : PageBaseISAI
{
    private const string KEY_VIEW_ROLES = "catRolesActosJurArray";
    /// <summary>
    /// Variable con la Row del participante seleccionado
    /// </summary>
    private DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participantesDR;


    /// <summary>
    /// Obtiene y carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if(!IsPostBack)
            {
                // Se comprueba que exista una pagina previa.
                if ((PageBaseISAI)PreviousPage == null)
                {
                    //Se ocultan el panel botones
                    botones.Visible = false;
                    MostrarMensajeInfoExcepcion("Se ha producido un error, Si el problema persiste contacte con el administrador del sistema");
                }
                else
                {
                    DireccionControl.Clear();
                    HiddenIdDireccion.Value = Utilidades.GetParametroUrl(Constantes.PAR_IDDIRECCION);
                    if (!string.IsNullOrEmpty(Utilidades.GetParametroUrl(Constantes.PAR_OPERACION)))
                    {
                        Operacion = System.Convert.ToString(Utilidades.GetParametroUrl(Constantes.PAR_OPERACION));
                    }
                    if (!string.IsNullOrEmpty(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO)))
                    {
                        string stringFiltro = System.Convert.ToString(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO));
                        FBusqueda.RellenarObjetoFiltro(stringFiltro);
                    }
                    SortDirectionP = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR);
                    SortExpression = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP);
                    SortDirectionP2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR2);
                    SortExpression2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP2);
                    ObtenerDatosPorPost();
                    persona.TipoOperacion = OperacionParticipantes;
                    CargarComboRol();
                    if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
                    {
                        IdPersonaDeclarante = ((PageBaseISAI)PreviousPage).IdPersonaDeclarante;
                        CodTipoPersona = ((PageBaseISAI)PreviousPage).CodTipoPersona;
                        btnBuscar.Visible = false;
                        //btnActualizar.Visible = false;
                        DireccionControl.readOnly = true;
                        CargarPagina();
                    }
                    else if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0)
                    {
                        IdPersonaDeclarante = ((PageBaseISAI)PreviousPage).IdPersonaDeclarante;
                        idDeclaranteAnt = ((PageBaseISAI)PreviousPage).IdPersonaDeclarante;
                        CodTipoPersona = ((PageBaseISAI)PreviousPage).CodTipoPersona;
                        CargarPagina();
                    }
                    DireccionControl.idDireccion = participantesDR == null ? -1 : participantesDR.IDDIRECCION;
                    DireccionControl.LoadControl();
                    CargarDomicilio();
                    string url = System.Web.VirtualPathUtility.ToAbsolute(Constantes.URL_SUBISAI_BUSCARPERSONA);
                    //btnBuscar.OnClientClick = string.Format("javascript:abrirUrlBuscadorPersonas('{0}','{1}')",
                    //        url,
                    //        txtDatosPersona.ClientID);
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

    #region cargar los datos por POST
  
    /// <summary>
    /// Método que carga los parámetros pasados por POST a las propiedades de la página Base de ISAI
    /// </summary>
    private void ObtenerDatosPorPost()
    {
        PaginaOrigen = ((PageBaseISAI)PreviousPage).PaginaOrigen;
        Operacion = ((PageBaseISAI)PreviousPage).Operacion;
        OperacionParticipantes = ((PageBaseISAI)PreviousPage).OperacionParticipantes;
        CondonacionJornada = ((PageBaseISAI)PreviousPage).CondonacionJornada;
        CodActoJuridicoParticipante = ((PageBaseISAI)PreviousPage).CodActoJuridicoParticipante;
        CodTipoPersona = string.Empty;
        IdPersonaDeclarante = ((PageBaseISAI)PreviousPage).IdPersonaDeclarante;

        FBusqueda = ((PageBaseISAI)PreviousPage).FBusqueda;
        XmlDocumental = ((PageBaseISAI)PreviousPage).XmlDocumental;
        TipoDocumental = ((PageBaseISAI)PreviousPage).TipoDocumental;
        ListaIdsDocumental = ((PageBaseISAI)PreviousPage).ListaIdsDocumental;
        Descrip = ((PageBaseISAI)PreviousPage).Descrip;
        DescripBF = ((PageBaseISAI)PreviousPage).DescripBF;
        XmlDocumentalBF = ((PageBaseISAI)PreviousPage).XmlDocumentalBF;
        TipoDocumentalBF = ((PageBaseISAI)PreviousPage).TipoDocumentalBF;
        ListaIdsDocumentalBF = ((PageBaseISAI)PreviousPage).ListaIdsDocumentalBF;

        DseDeclaracionIsaiMant = ((PageBaseISAI)PreviousPage).DseDeclaracionIsaiMant;
        DseDeclaracionIsaiPadreMant = ((PageBaseISAI)PreviousPage).DseDeclaracionIsaiPadreMant;
        DseAvaluo = ((PageBaseISAI)PreviousPage).DseAvaluo;
        SortExpression = ((PageBaseISAI)PreviousPage).SortExpression;
        SortDirectionP = ((PageBaseISAI)PreviousPage).SortDirectionP;
        SortExpression2 = ((PageBaseISAI)PreviousPage).SortExpression2;
        SortDirectionP2 = ((PageBaseISAI)PreviousPage).SortDirectionP2;

    }
    #endregion

    #region Métodos de la página

    /// <summary>
    /// Método que carga el domicilio y configura los botones implicados en la dirección
    /// </summary>
    private void CargarDomicilio()
    {
        //try
        //{
        //    if (DseDeclaracionIsaiMant.FEXNOT_DOMICILIO.Any())
        //    {
        //        for (int i = DseDeclaracionIsaiMant.FEXNOT_DOMICILIO.Count - 1; i >= 0; i--)
        //        {
        //            DseDeclaracionIsaiMant.FEXNOT_DOMICILIO[i].Delete();
        //        }
        //    }
        //    //deshabilito el boton de editar domicilio cuando la operacion de pagina es VER
        //    if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
        //    {
        //        btnActualizar.Enabled = false;
        //        btnActualizar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
        //    }
        //    else
        //    {
        //        btnActualizar.Enabled = true;
        //        btnActualizar.ImageUrl = Constantes.IMG_MODIFICA;
        //    }
        //    string url = System.Web.VirtualPathUtility.ToAbsolute(Constantes.URL_SUBISAI_ESPECIFICARDIRECCION);
        //    //Aqui se mira la operacion de los participantes pero hay que saber tb la operacion de la declaracion.
        //    //SI: VER --> xmlDir tiene que estar vacio e IDdireccion tiene el id.
        //    //MOD --> ""
        //    //INS --> xmldir estara vacio si es añadir, tendra valor si es mod o ver.
        //    if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0 || Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0)
        //    {
        //        //Si es ver no hace falta asignarle nada al botón por que está deshabilitado
        //        if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_INS) == 0)
        //        {
        //            //Función de javascript para abrir la especificación de dirección Propietario
        //            //btnActualizar.Attributes["onClick"] = string.Format("javascript:AbrirUrlDireccion('{0}','{1}','{2}','{3}','{4}'); return false; ", url, txtEspDireccion.ClientID, null, null, null);
        //        }
        //        else if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0 || OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
        //        {
        //            //Función de javascript para abrir la especificación de dirección Propietario
        //            if (!participantesDR.IsXMLDIRNull())
        //            {
        //                string xmlNuevoEncode = Utilidades.base64Encode(participantesDR.XMLDIR);
        //                txtEspDireccion.Text = xmlNuevoEncode;
        //                //btnActualizar.Attributes.Add("onClick", string.Format("javascript:AbrirUrlDireccion('{0}','{1}','{2}','{3}','{4}'); return false; ", url, txtEspDireccion.ClientID, txtEspDireccion.Text, null, null));
        //            }
        //            else
        //            {
        //                //btnActualizar.Attributes.Add("onClick", string.Format("javascript:AbrirUrlDireccion('{0}','{1}','{2}','{3}','{4}'); return false; ", url, txtEspDireccion.ClientID, null, null, participantesDR.IDDIRECCION));
        //            }
        //            CargarDatosDireccion();
        //        }
        //    }
        //    else if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_INS) == 0)
        //    {
        //        if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_INS) == 0)
        //        {
        //            //Función de javascript para abrir la especificación de dirección Propietario
        //            //btnActualizar.Attributes["onClick"] = string.Format("javascript:AbrirUrlDireccion('{0}','{1}','{2}','{3}','{4}'); return false; ", url, txtEspDireccion.ClientID, null, null, null);
        //        }
        //        else if ((OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0) ||
        //            (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0))
        //        {
        //            string xmlEncode = Utilidades.base64Encode(participantesDR.XMLDIR);
        //            txtEspDireccion.Text = xmlEncode;
        //            //btnActualizar.Attributes.Add("onClick", string.Format("javascript:AbrirUrlDireccion('{0}','{1}','{2}','{3}','{4}'); return false; ", url, txtEspDireccion.ClientID, txtEspDireccion.Text, null, null));
        //            CargarDatosDireccion();
        //        }
        //    }
        //}
        //catch (FaultException<DeclaracionIsaiException> cex)
        //{
        //    string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
        //    MostrarMensajeInfoExcepcion(msj);
        //}
        //catch (FaultException<DeclaracionIsaiInfoException> ciex)
        //{
        //    string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
        //    MostrarMensajeInfoExcepcion(msj);
        //}
        //catch (Exception ex)
        //{
        //    ExceptionPolicyWrapper.HandleException(ex);
        //    string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
        //    MostrarMensajeInfoExcepcion(msj);
        //}
    }


    /// <summary>
    /// Método que carga el dataSet de Dirección desde el XML.
    /// Desde el dataset carga los datos en el formulario
    /// </summary>
    private void CargarDatosDireccion()
    {

        //DseInfoDirecciones.DireccionRow drDirec = null;
        //if (hidXmlDireccion.Value != "")
        //{
        //    string xml = hidXmlDireccion.Value;
        //    DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
        //    tablaDireccion.ReadXml(new StringReader(xml));
        //    DseInfoDirecciones dseDireccionRcon = new DseInfoDirecciones();
        //    dseDireccionRcon.Direccion.Merge(tablaDireccion);
        //    drDirec = dseDireccionRcon.Direccion[0];
        //}
        //else
        if (!participantesDR.IsXMLDIRNull())
        {
            //DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
            //tablaDireccion.ReadXml(new StringReader(participantesDR.XMLDIR));
            //DseInfoDirecciones dseDireccionRcon = new DseInfoDirecciones();
            //dseDireccionRcon.Direccion.Merge(tablaDireccion);
            //drDirec = dseDireccionRcon.Direccion[0];
            DireccionControl.xml = participantesDR.XMLDIR;
        }
        //else if (DseRconDomicilio.Direccion.Any())
        //{
        //    CargarDatosDomicilio2();
        //    drDirec = null;
        //}
        //else
        //    drDirec = null;

        //if (drDirec != null)
        //{

        //    lblDelegacionDato.Text = drDirec.DELEGACION;
        //    lblColoniaDato.Text = drDirec.COLONIA;
        //    lblViaDato.Text = drDirec.VIA;
        //    lblNumeroExteriorDato.Text = drDirec.NUMEROEXTERIOR;
        //    ServiceRCON.DseCatalogos.CatTiposLocalidadDataTable dtCatLoc = ApplicationCache.CatalogosRCO.CatTiposLocalidad;
        //    foreach (ServiceRCON.DseCatalogos.CatTiposLocalidadRow drLoc in dtCatLoc)
        //    {
        //        if (!drDirec.IsCODTIPOSLOCALIDADNull())
        //        {
        //            if (drLoc.CODTIPOSLOCALIDAD.Equals(drDirec.CODTIPOSLOCALIDAD))
        //            {
        //                lblTipoLocalidadDato.Text = drLoc.DESCRIPCION.ToString();
        //                break;
        //            }
        //        }
        //    }
        //    if (!drDirec.IsTIPOVIANull())
        //        lblTipoViaDato.Text = drDirec.TIPOVIA;

        //    if (!drDirec.IsTIPOASENTAMIENTONull())
        //        lblTipoAsentamientoDato.Text = drDirec.TIPOASENTAMIENTO;
        //    else
        //        lblTipoAsentamientoDato.Text = string.Empty;

        //    if (!drDirec.IsCODIGOPOSTALNull())
        //        lblCodigoPostalDato.Text = drDirec.CODIGOPOSTAL;
        //    else
        //        lblCodigoPostalDato.Text = string.Empty;

        //    if (!drDirec.IsENTRECALLE1Null())
        //        lblEntreCalle1Dato.Text = drDirec.ENTRECALLE1;
        //    else
        //        lblEntreCalle1Dato.Text = string.Empty;

        //    if (!drDirec.IsENTRECALLE2Null())
        //        lblEntreCalle2Dato.Text = drDirec.ENTRECALLE2;
        //    else
        //        lblEntreCalle2Dato.Text = string.Empty;

        //    if (!drDirec.IsANDADORNull())
        //        lblAndadorDato.Text = drDirec.ANDADOR;
        //    else
        //        lblAndadorDato.Text = string.Empty;

        //    if (!drDirec.IsEDIFICIONull())
        //        lblEdificioDato.Text = drDirec.EDIFICIO;
        //    else
        //        lblEdificioDato.Text = string.Empty;

        //    if (!drDirec.IsSECCIONNull())
        //        lblSeccionDato.Text = drDirec.SECCION;
        //    else
        //        lblSeccionDato.Text = string.Empty;

        //    if (!drDirec.IsENTRADANull())
        //        lblEntradaDato.Text = drDirec.ENTRADA;
        //    else
        //        lblEntradaDato.Text = string.Empty;

        //    if (!drDirec.IsNUMEROINTERIORNull())
        //        lblNumeroInteriorDato.Text = drDirec.NUMEROINTERIOR;
        //    else
        //        lblNumeroInteriorDato.Text = string.Empty;

        //    if (!drDirec.IsTELEFONONull())
        //        lblTelefonoDato.Text = drDirec.TELEFONO;
        //    else
        //        lblTelefonoDato.Text = string.Empty;

        //    if (!drDirec.IsINDICACIONESADICIONALESNull())
        //        lblIndicacionesAdicionalesDato.Text = drDirec.INDICACIONESADICIONALES;
        //    else
        //        lblIndicacionesAdicionalesDato.Text = string.Empty;

        //}
    }

    /// <summary>
    /// Método que carga y configura la página con todos los datos de participantes
    /// </summary>
    private void CargarPagina()
    {
        try
        {
            participantesDR = (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow)DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Select(
                Constantes.PAR_IDDECLARACION + "=" + DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION + " AND " + Constantes.PAR_IDPERSONA + "=" + IdPersonaDeclarante).First();

            if (participantesDR != null)
            {
                DseRconDomicilio = ClienteRcon.GetDireccionById(participantesDR.IDDIRECCION);
                
                if (!participantesDR.IsNOMBRENull())
                {
                    persona.Nombre = participantesDR.NOMBRE;
                }

                if (!participantesDR.IsAPELLIDOPATERNONull())
                {
                    persona.ApellidoPaterno = participantesDR.APELLIDOPATERNO;
                }

                if (!participantesDR.IsAPELLIDOMATERNONull())
                {
                    persona.ApellidoMaterno = participantesDR.APELLIDOMATERNO;
                }

                if (!participantesDR.IsACTIVPRINCIPNull())
                {
                    persona.ActivPrincip = participantesDR.ACTIVPRINCIP;
                }

                if (!participantesDR.IsRFCNull())
                {
                    persona.Rfc = participantesDR.RFC;
                }

                if (!participantesDR.IsCURPNull())
                {
                    persona.Curp = participantesDR.CURP;
                }

                if (!participantesDR.IsCLAVEIFENull())
                {
                    persona.ClaveIfe = participantesDR.CLAVEIFE;
                }

                if (participantesDR.CODTIPOPERSONA.CompareTo(Constantes.PAR_VAL_PERSONA_MORAL) == 0)
                {
                    persona.PersonaMoral = true;
                }
                else
                {
                    persona.PersonaFisica = true;
                }

                if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
                {
                    ddllblParticipacionDato.Enabled = false;
                    lblPorcentajeDato.Text = participantesDR.PORCTRANSMISION.ToPercent();
                    txtPorcentajeDato.Visible = false;
                    lblPorcentajeDato.Visible = true;
                    btnImgVolverPersonas.Visible = false;
                    btnGuardar.Visible = false;
                    btnVolverPersonas.Visible = true;
                    ddllblParticipacionDato.Visible = false;
                    if (!participantesDR.IsROLNull())
                    {
                        lblParticipacionDato.Visible = true;
                        lblParticipacionDato.Text = participantesDR.ROL.ToString();
                    }
              


                }
                else
                {
                   
                    DseCatalogo.FEXNOT_CATROLESRow[] catRolesDR = (DseCatalogo.FEXNOT_CATROLESRow[])ApplicationCache.DseCatalogoISAI.FEXNOT_CATROLES.Select();

                    if (!participantesDR.IsCODROLNull())
                    {
                        this.ddllblParticipacionDato.Items.FindByValue(participantesDR.CODROL.ToString()).Selected = true;
                    }
                    txtPorcentajeDato.Text = (participantesDR.PORCTRANSMISION * 100).ToString();
                    lblPorcentajeDato.Visible = false;
                    txtPorcentajeDato.Visible = true;
                    btnImgVolverPersonas.Visible = true;
                    btnGuardar.Visible = true;
                    btnVolverPersonas.Visible = false;
                }
                CargarDatosDireccion();
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
    ///  Carga en la página de búsqueda de personas los datos pasados por parámetros en sus respectivas casillas 
    /// </summary>
    /// <param name="idPersona">id de la persona declarante</param>
    /// <param name="nombre">Nombre</param>
    /// <param name="apellidoP">Apellido Paterno</param>
    /// <param name="apellidoM">Apellido Materno</param>
    /// <param name="rfc">RFC</param>
    /// <param name="curp">CURP</param>
    /// <param name="ife">IFE</param>
    /// <param name="actividad">Actividad</param>
    /// <param name="codTipoPersona">Código del tipo de persona</param>
    protected void CargarPaginaBusquedaPersonas(string idPersona, string nombre, string apellidoP, string apellidoM, string rfc, string curp, string ife, string actividad, string codTipoPersona)
    {
        try
        {
            IdPersonaDeclarante = Convert.ToInt32(idPersona);
            if (nombre != "&nbsp;")
            {
                persona.Nombre = nombre;
            }
            if (apellidoP != "&nbsp;")
            {
                persona.ApellidoPaterno = apellidoP;
            }
            if (apellidoM != "&nbsp;")
            {
                persona.ApellidoMaterno = apellidoM;
            }
            if (actividad != "&nbsp;")
            {
                persona.ActivPrincip = actividad;
            }
            if (rfc != "&nbsp;")
            {
                persona.Rfc = rfc;
            }
            if (curp != "&nbsp;")
            {
                persona.Curp = curp;
            }
            if (ife != "&nbsp;")
            {
                persona.ClaveIfe = ife;
            }
            if (codTipoPersona.CompareTo(Constantes.PAR_VAL_PERSONA_MORAL) == 0)
            {
                persona.PersonaMoral = true;
            }
            else
            {
                persona.PersonaFisica = true;
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
    /// Método que recoge los datos de la persona introducidos en la pantalla de personas y los guarda en el dataset
    /// </summary>
    private void GuardarPaginaInsercion()
    {
        try
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participanteDR = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.NewFEXNOT_PARTICIPANTESRow(); ;
            if (persona.PersonaFisica != false)
            {
                participanteDR.CODTIPOPERSONA = Constantes.PAR_VAL_PERSONA_FISICA;
                participanteDR.TIPOPERSONA = EnumUtility.GetDescription(TipoPersona.Fisica);
            }
            else
            {
                participanteDR.CODTIPOPERSONA = Constantes.PAR_VAL_PERSONA_MORAL;
                participanteDR.TIPOPERSONA = EnumUtility.GetDescription(TipoPersona.Moral);
            }
            participanteDR.IDDECLARACION = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION;
            participanteDR.PORCTRANSMISION = txtPorcentajeDato.Text.To<Decimal>() / 100;
            if (ddllblParticipacionDato.SelectedIndex > 0)
            {
                participanteDR.CODROL = ddllblParticipacionDato.SelectedItem.Value.To<Decimal>();
                participanteDR.ROL = ddllblParticipacionDato.SelectedItem.Text;
            }
            //participanteDR.CODACTOJUR = CodActoJuridicoParticipante;
            participanteDR.CODSITUACIONESPERSONA = "N";
            participanteDR.NOMBRE = persona.Nombre;
            participanteDR.APELLIDOPATERNO = persona.ApellidoPaterno;
            participanteDR.APELLIDOMATERNO = persona.ApellidoMaterno;
            if (persona.PersonaFisica != false)
            {
                participanteDR.NOMBREAPELLIDOS = string.Format("{0} {1}, {2}",
                    participanteDR.APELLIDOPATERNO,
                    participanteDR.APELLIDOMATERNO,
                    participanteDR.NOMBRE);
            }
            else
            {
                participanteDR.NOMBREAPELLIDOS = participanteDR.NOMBRE;
            }
            if (string.IsNullOrEmpty(persona.Rfc))
            {
                participanteDR.SetRFCNull();
            }
            else
            {
                participanteDR.RFC = persona.Rfc;
            }
            if (string.IsNullOrEmpty(persona.Curp))
            {
                participanteDR.SetCURPNull();
            }
            else
            {
                participanteDR.CURP = persona.Curp;
            }
            if (string.IsNullOrEmpty(persona.ClaveIfe))
            {
                participanteDR.SetCLAVEIFENull();
            }
            else
            {
                participanteDR.CLAVEIFE = persona.ClaveIfe;
            }
            participanteDR.ACTIVPRINCIP = persona.ActivPrincip;
          
            if (participanteDR.IsIDDIRECCIONNull())
            {
                participanteDR.IDDIRECCION = -1;
            }
            participanteDR.XMLDIR = DireccionControl.xml;
            DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.AddFEXNOT_PARTICIPANTESRow(participanteDR);
            DireccionControl.Clear();
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
    /// Método que guarda los cambios realizados en el participante
    /// </summary>
    private void GuardarPaginaModificacion()
    {
        try
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participanteDR = (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow)DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Select(
            Constantes.PAR_IDDECLARACION + "=" + DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION + " AND " + Constantes.PAR_IDPERSONA + "=" + idDeclaranteAnt).First();
            if (persona.PersonaFisica != false)
            {
                participanteDR.CODTIPOPERSONA = Constantes.PAR_VAL_PERSONA_FISICA;
                participanteDR.TIPOPERSONA = EnumUtility.GetDescription(TipoPersona.Fisica);
            }
            else
            {
                participanteDR.CODTIPOPERSONA = Constantes.PAR_VAL_PERSONA_MORAL;
                participanteDR.TIPOPERSONA = EnumUtility.GetDescription(TipoPersona.Moral);
            }

            participanteDR.PORCTRANSMISION = txtPorcentajeDato.Text.To<Decimal>() / 100;
            if (ddllblParticipacionDato.SelectedIndex > 0)
            {
                participanteDR.CODROL = ddllblParticipacionDato.SelectedItem.Value.To<Decimal>();
                participanteDR.ROL = ddllblParticipacionDato.SelectedItem.Text;
            }
            //participanteDR.CODACTOJUR = CodActoJuridicoParticipante;
            participanteDR.CODSITUACIONESPERSONA = "N";
            participanteDR.NOMBRE = persona.Nombre;
            participanteDR.APELLIDOPATERNO = persona.ApellidoPaterno;
            participanteDR.APELLIDOMATERNO = persona.ApellidoMaterno;
            if (persona.PersonaFisica != false)
            {
                participanteDR.NOMBREAPELLIDOS = string.Format("{0} {1}, {2}",
                participanteDR.APELLIDOPATERNO,
                participanteDR.APELLIDOMATERNO,
                participanteDR.NOMBRE);
            }
            else
            {
                participanteDR.NOMBREAPELLIDOS = participanteDR.NOMBRE;
            }

            if (string.IsNullOrEmpty(persona.Rfc))
            {
                participanteDR.SetRFCNull();
            }
            else
            {
                participanteDR.RFC = persona.Rfc;
            }

            if (string.IsNullOrEmpty(persona.Curp))
            {
                participanteDR.SetCURPNull();
            }
            else
            {
                participanteDR.CURP = persona.Curp;
            }

            if (string.IsNullOrEmpty(persona.ClaveIfe))
            {
                participanteDR.SetCLAVEIFENull();
            }
            else
            {
                participanteDR.CLAVEIFE = persona.ClaveIfe;
            }

            participanteDR.ACTIVPRINCIP = persona.ActivPrincip;

 
            if (participanteDR.IsIDDIRECCIONNull())
            {
                participanteDR.IDDIRECCION = -1;
            }
            if (!String.IsNullOrEmpty(DireccionControl.xml))
                participanteDR.XMLDIR = DireccionControl.xml;
            DireccionControl.Clear();
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

    #region RCON

    /// <summary>
    /// Método que carga en los controles de la página los datos de la dirección desde el dataset de RCON
    /// </summary>
    private void CargarDatosDomicilio2()
    {
        
            //if (DseRconDomicilio.Direccion.Any())
            //{
            //    lblDelegacionDato.Text = DseRconDomicilio.Direccion[0].DELEGACION;
            //    lblColoniaDato.Text = DseRconDomicilio.Direccion[0].COLONIA;
            //    lblViaDato.Text = DseRconDomicilio.Direccion[0].VIA;
            //    lblNumeroExteriorDato.Text = DseRconDomicilio.Direccion[0].NUMEROEXTERIOR;

            //    ServiceRCON.DseCatalogos.CatTiposLocalidadDataTable dtCatLoc = ApplicationCache.CatalogosRCO.CatTiposLocalidad;
            //    foreach (ServiceRCON.DseCatalogos.CatTiposLocalidadRow drLoc in dtCatLoc)
            //    {
            //        if (!DseRconDomicilio.Direccion[0].IsCODTIPOSLOCALIDADNull())
            //        {
            //            if (drLoc.CODTIPOSLOCALIDAD.Equals(DseRconDomicilio.Direccion[0].CODTIPOSLOCALIDAD))
            //            {
            //                lblTipoLocalidadDato.Text = drLoc.DESCRIPCION.ToString();
            //                break;
            //            }
            //        }
            //    }

            //    if (!DseRconDomicilio.Direccion[0].IsTIPOVIANull())
            //        lblTipoViaDato.Text = DseRconDomicilio.Direccion[0].TIPOVIA;
            //    else
            //        lblTipoViaDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsTIPOASENTAMIENTONull())
            //        lblTipoAsentamientoDato.Text = DseRconDomicilio.Direccion[0].TIPOASENTAMIENTO;
            //    else
            //        lblTipoAsentamientoDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsCODIGOPOSTALNull())
            //        lblCodigoPostalDato.Text = DseRconDomicilio.Direccion[0].CODIGOPOSTAL;
            //    else
            //        lblCodigoPostalDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsENTRECALLE1Null())
            //        lblEntreCalle1Dato.Text = DseRconDomicilio.Direccion[0].ENTRECALLE1;
            //    else
            //        lblEntreCalle1Dato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsENTRECALLE2Null())
            //        lblEntreCalle2Dato.Text = DseRconDomicilio.Direccion[0].ENTRECALLE2;
            //    else
            //        lblEntreCalle2Dato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsANDADORNull())
            //        lblAndadorDato.Text = DseRconDomicilio.Direccion[0].ANDADOR;
            //    else
            //        lblAndadorDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsEDIFICIONull())
            //        lblEdificioDato.Text = DseRconDomicilio.Direccion[0].EDIFICIO;
            //    else
            //        lblEdificioDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsSECCIONNull())
            //        lblSeccionDato.Text = DseRconDomicilio.Direccion[0].SECCION;
            //    else
            //        lblSeccionDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsENTRADANull())
            //        lblEntradaDato.Text = DseRconDomicilio.Direccion[0].ENTRADA;
            //    else
            //        lblEntradaDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsNUMEROINTERIORNull())
            //        lblNumeroInteriorDato.Text = DseRconDomicilio.Direccion[0].NUMEROINTERIOR;
            //    else
            //        lblNumeroInteriorDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsTELEFONONull())
            //        lblTelefonoDato.Text = DseRconDomicilio.Direccion[0].TELEFONO;
            //    else
            //        lblTelefonoDato.Text = string.Empty;

            //    if (!DseRconDomicilio.Direccion[0].IsINDICACIONESADICIONALESNull())
            //        lblIndicacionesAdicionalesDato.Text = DseRconDomicilio.Direccion[0].INDICACIONESADICIONALES;
            //    else
            //        lblIndicacionesAdicionalesDato.Text = string.Empty;
            //}
 
    }



    #endregion

    #region Cargar Combos de la Pagina

   /// <summary>
   /// Método que carga el combo de roles
   /// </summary>
    private void CargarComboRol()
    {
        try
        {
            DseCatalogo.FEXNOT_CATROLESRow[] catRolesDR = (DseCatalogo.FEXNOT_CATROLESRow[])ApplicationCache.DseCatalogoISAI.FEXNOT_CATROLES.Select();
            DseCatalogo.FEXNOT_CATROLESDataTable catRolesDT = new DseCatalogo.FEXNOT_CATROLESDataTable();

            for (int i = 0; i < catRolesDR.Length; i++)
            {
                DseCatalogo.FEXNOT_CATROLESRow rowR = catRolesDT.NewFEXNOT_CATROLESRow();

                rowR.CODROL = catRolesDR[i].CODROL;
                rowR.DESCRIPCION = catRolesDR[i].DESCRIPCION;         
                rowR.ACTIVO = catRolesDR[i].ACTIVO;

                catRolesDT.AddFEXNOT_CATROLESRow(rowR);
            }

            if (catRolesDT.Any())
            {
                //crear la tabla hash antes de insertar el nuevo elemento
                //Hashtable hashRoles = new Hashtable();

                //foreach (DseCatalogo.FEXNOT_CATROLESRow row in catRolesDT)
                //    hashRoles.Add(row.CODROLAJ, row.EXIGIDO);

                this.ddllblParticipacionDato.DataSource = catRolesDT;
                this.ddllblParticipacionDato.DataTextField = catRolesDT.DESCRIPCIONColumn.ToString();
                this.ddllblParticipacionDato.DataValueField = catRolesDT.CODROLColumn.ToString();
                this.ddllblParticipacionDato.DataBind();

                //insertar elemento de seleccion
                this.ddllblParticipacionDato.Items.Insert(0, new ListItem("Seleccione un Rol...", ""));

                //ViewState.Add(KEY_VIEW_ROLES, hashRoles);
            
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

    #region Botones de la Pagina


    /// <summary>
    /// Método que maneja el evento Click del control btnGuardar 
    /// Comprueba que los participantes esten correctamente configurados y el domicilio
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            //Validar si la misma persona participa en la misma solicitud
            bool statePersona = false;
            //Comprobamos si ya tenemos esta persona
            statePersona = ComprobarPersona();
            if (statePersona)
            {
                ModalInfoGuardar.TextoInformacion = "Ya existe el participante en la Declaración.";
                extenderPnlInfoGuardarModal.Show();
            }
            else
            {
                //Es una persona nueva
                ValidarDomicilio();
            }
        }
        catch (System.Threading.ThreadAbortException)
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
    /// Función que comprueba que los datos del participante estan
    /// correctamente introducidos y compara con los introducidos hasta el momento para ver si existe uno igual
    /// </summary>
    /// <returns>Existe la persona o no</returns>
    private bool ComprobarPersona()
    {
             bool statePersona = false;
            //obtengo los valores insertados en el control del propietario 
            string nombrep, apellidoPaternop, apellidoMaternop, rfcp, curpp, claveIfep, activPrincipp;
            string nombre, apellidoPaterno, apellidoMaterno, rfc, curp, claveIfe, activPrincip;
          
            if (string.IsNullOrEmpty(persona.Nombre))
                nombrep = string.Empty;
            else
                nombrep = persona.Nombre;
            if (string.IsNullOrEmpty(persona.ApellidoPaterno))
                apellidoPaternop = string.Empty;
            else
                apellidoPaternop = persona.ApellidoPaterno;
            if (string.IsNullOrEmpty(persona.ApellidoMaterno))
                apellidoMaternop = string.Empty;
            else
                apellidoMaternop = persona.ApellidoMaterno;
            if (string.IsNullOrEmpty(persona.Rfc))
                rfcp = string.Empty;
            else
                rfcp = persona.Rfc;
            if (string.IsNullOrEmpty(persona.Curp))
                curpp = string.Empty;
            else
                curpp = persona.Curp;
            if (string.IsNullOrEmpty(persona.ClaveIfe))
                claveIfep = string.Empty;
            else
                claveIfep = persona.ClaveIfe;
            if (string.IsNullOrEmpty(persona.ActivPrincip))
                activPrincipp = string.Empty;
            else
                activPrincipp = persona.ActivPrincip;

            foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow personaRow in DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES)
            {
                if (personaRow.RowState != DataRowState.Deleted)
                {
                    activPrincip = personaRow.IsACTIVPRINCIPNull() ? string.Empty : personaRow.ACTIVPRINCIP;
                    apellidoMaterno = personaRow.IsAPELLIDOMATERNONull() ? string.Empty : personaRow.APELLIDOMATERNO;
                    claveIfe = personaRow.IsCLAVEIFENull() ? string.Empty : personaRow.CLAVEIFE;
                    curp = personaRow.IsCURPNull() ? string.Empty : personaRow.CURP;
                    rfc = personaRow.IsRFCNull() ? string.Empty : personaRow.RFC;

                    apellidoPaterno = string.Empty;

                    if (personaRow.CODTIPOPERSONA == Constantes.PAR_VAL_PERSONA_FISICA)
                    {
                        apellidoPaterno = personaRow.IsAPELLIDOPATERNONull() ? string.Empty : personaRow.APELLIDOPATERNO;
                        nombre = personaRow.IsNOMBRENull() ? string.Empty : personaRow.NOMBRE;
                    }
                    else
                    {
                        nombre = personaRow.IsAPELLIDOPATERNONull() ? string.Empty : personaRow.APELLIDOPATERNO;
                    }

                    if ((activPrincip == activPrincipp) &&
                        (apellidoMaterno == apellidoMaternop) &&
                        (apellidoPaterno == apellidoPaternop) &&
                        (claveIfe == claveIfep) &&
                        (curp == curpp) &&
                        (rfc == rfcp) &&
                        (nombre == nombrep) && IdPersonaDeclarante != personaRow.IDPERSONA.ToInt())
                    {
                        statePersona = true;
                        break;
                    }
                }
            }

            return statePersona;
    }


    /// <summary>
    /// Método que valida si se ha introducido una drección para el participante 
    /// En tal caso guarda los datos introducidos y redirecciona a la página de declaración
    /// </summary>
    private void ValidarDomicilio()
    {
        try
        {
            //Validar si se ha introducido un domicilio para el participante
            DireccionControl.EspecificarDireccion();

            if (DireccionControl.direccionCorrecta)
            {
                if (persona.PersonaMoral)
                { 
                    if(string.IsNullOrEmpty(persona.Nombre.Replace(" ","")) || string.IsNullOrEmpty(persona.Rfc.Replace(" ","")))
                    {
                        ModalInfoGuardar.TextoInformacion = "No se puede dar de alta un participante Moral si no tiene Nombre o RFC.";
                        ModalInfoGuardar.TipoMensaje = true;
                        extenderPnlInfoGuardarModal.Show();
                    }
                    else
                    {
                        Save();
                    }
                }
                else
                {
                    Save();
                }
              
            }
            else
            {
                //TODO: MENSAJE INFORMATIVO ( SIN ESPECIFICAR EL VALOR DE LA CUENTA CATASTRAL )
                ModalInfoGuardar.TextoInformacion = "No se puede dar de alta un participante, <BR> si no tiene una dirección de notificación.";
                ModalInfoGuardar.TipoMensaje = true;
                extenderPnlInfoGuardarModal.Show();
            }
        }
        catch (System.Threading.ThreadAbortException)
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

    protected void Save()
    {
        switch (OperacionParticipantes)
        {
            case Constantes.PAR_VAL_OPERACION_INS:
                GuardarPaginaInsercion();
                break;
            case Constantes.PAR_VAL_OPERACION_MOD:
                GuardarPaginaModificacion();
                break;
        }
        Server.Transfer(Constantes.URL_SUBISAI_DECLARACION);
    }

    /// <summary>
    /// Método que maneja el evento Click del control btnImgVolverPersonas
    /// Redirecciona a la página de la declaración
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImgVolverPersonas_Click(object sender, EventArgs e)
    {
        try
        {
            DireccionControl.Clear();
            Server.Transfer(Constantes.URL_SUBISAI_DECLARACION);
        }
        catch (System.Threading.ThreadAbortException)
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
    /// Método que maneja el evento Click del control btnImgVolverPersonas
    /// Redirecciona a la página de la declaración
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVolverPersonas_Click(object sender, EventArgs e)
    {
        try
        {
            DireccionControl.Clear();
            Server.Transfer(Constantes.URL_SUBISAI_DECLARACION);
        }
        catch (System.Threading.ThreadAbortException)
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
    /// Maneja  el evento click del control ModalInfoGuardar_ok
    /// Solicita la confirmación para guardar los datos introducidos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ModalInfoGuardar_Ok_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (e.Cancel)
                extenderPnlInfoGuardarModal.Hide();
            else
                extenderPnlInfoGuardarModal.Show();
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

    #region Eventos TextBox

    /// <summary>
    /// Maneja el evento valueChanged del control txtEspDireccion
    /// Recoge los datos devueltos por RCON tras cerrar la ventana de insertar la dirección
    /// Configura el btnActualizar para que al pulsar salgan los datos introducidos
    /// Carga los datos obtenidos de la pantalla de inserción en el panel de muestra
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void txtEspDireccion_ValueChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //Cogemos el parámetro devuelto por la ventana hija del textbox
    //        //string xml = Utilidades.base64Decode(txtEspDireccion.Text);
    //        if (!string.IsNullOrEmpty(xml))
    //        {
    //            //Recuperamos la tabla
    //            DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
    //            //tablaDireccion.ReadXml(new StringReader(xml));
    //            if (tablaDireccion.Any())
    //            {
    //                //hidXmlDireccion.Value = xml;
    //                //Función de javascript para abrir la especificación de dirección
    //                string url = System.Web.VirtualPathUtility.ToAbsolute(Constantes.URL_SUBISAI_ESPECIFICARDIRECCION);
    //                //iddireccion del dataset de licencia propietario.
    //                //btnActualizar.Attributes["onClick"] = string.Format("javascript:AbrirUrlDireccion('{0}','{1}','{2}','{3}','{4}'); return false; ", url, txtEspDireccion.ClientID, txtEspDireccion.Text, null, null);
    //                CargarDatosDireccion();
    //            }
    //        }
    //    }
    //    catch (FaultException<DeclaracionIsaiException> cex)
    //    {
    //        string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }
    //    catch (FaultException<DeclaracionIsaiInfoException> ciex)
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
    /// Maneja el evento TextChanged del control txtDatosPersona
    /// Recoge los datos devueltos por RCON tras cerrar la ventana de búsqueda de persona
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtDatosPersona_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string xml = Utilidades.base64Decode(txtDatosPersona.Text);

            if (!string.IsNullOrEmpty(xml))
            {
                //Recuperamos la tabla
                DseContribuyentes.ContribuyentesDataTable dtContribuyentes = new DseContribuyentes.ContribuyentesDataTable();
                dtContribuyentes.ReadXml(new StringReader(xml));
                if (dtContribuyentes.Any())
                {
                    if (dtContribuyentes[0].CODTIPOPERSONA == Constantes.PAR_VAL_PERSONA_FISICA)
                    {
                        CargarPaginaBusquedaPersonas(dtContribuyentes[0].IsIDPERSONANull() ? string.Empty : dtContribuyentes[0].IDPERSONA.ToString(),
                                                    dtContribuyentes[0].IsNOMBRENull() ? string.Empty : dtContribuyentes[0].NOMBRE,
                                                    dtContribuyentes[0].IsAPELLIDOPATERNONull() ? string.Empty : dtContribuyentes[0].APELLIDOPATERNO,
                                                    dtContribuyentes[0].IsAPELLIDOMATERNONull() ? string.Empty : dtContribuyentes[0].APELLIDOMATERNO,
                                                    dtContribuyentes[0].IsRFCNull() ? string.Empty : dtContribuyentes[0].RFC,
                                                    dtContribuyentes[0].IsCURPNull() ? string.Empty : dtContribuyentes[0].CURP,
                                                    dtContribuyentes[0].IsCLAVEIFENull() ? string.Empty : dtContribuyentes[0].CLAVEIFE,
                                                    dtContribuyentes[0].IsACTIVPRINCIPNull() ? string.Empty : dtContribuyentes[0].ACTIVPRINCIP,
                                                    dtContribuyentes[0].IsCODTIPOPERSONANull() ? string.Empty : dtContribuyentes[0].CODTIPOPERSONA);
                    }
                    else
                    {
                        CargarPaginaBusquedaPersonas(dtContribuyentes[0].IsIDPERSONANull() ? string.Empty : dtContribuyentes[0].IDPERSONA.ToString(),
                                                    dtContribuyentes[0].IsAPELLIDOPATERNONull() ? string.Empty : dtContribuyentes[0].APELLIDOPATERNO,
                                                    string.Empty,
                                                    string.Empty,
                                                    dtContribuyentes[0].IsRFCNull() ? string.Empty : dtContribuyentes[0].RFC,
                                                    dtContribuyentes[0].IsCURPNull() ? string.Empty : dtContribuyentes[0].CURP,
                                                    dtContribuyentes[0].IsCLAVEIFENull() ? string.Empty : dtContribuyentes[0].CLAVEIFE,
                                                    dtContribuyentes[0].IsACTIVPRINCIPNull() ? string.Empty : dtContribuyentes[0].ACTIVPRINCIP,
                                                    dtContribuyentes[0].IsCODTIPOPERSONANull() ? string.Empty : dtContribuyentes[0].CODTIPOPERSONA);
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

    #endregion

}
