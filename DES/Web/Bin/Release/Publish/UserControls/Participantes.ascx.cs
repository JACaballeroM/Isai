using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;

/// <summary>
/// Clase que gestiona el user control Participantes
/// </summary>
public partial class UserControls_Participantes : System.Web.UI.UserControl
{

    #region Propiedades

    public ImageButton btn { get { return this.btnAdd; } set { this.btnAdd = value; } }

    /// <summary>
    /// Propiedad que almacena y obtiene el idParticipante
    /// </summary>
    public int IdParticipantes
    {
        get
        {
            return Convert.ToInt32(this.ViewState["IdParticipantes"]);
        }
        set
        {
            this.ViewState["IdParticipantes"] = value;
        }
    }

    /// <summary>
    /// Propiedad que almacena y obtiene un booleano con información de si existe la cuenta
    /// </summary>
    public bool ExisteCuenta
    {
        get
        {
            return Convert.ToBoolean(this.ViewState["ExisteCuenta"]);
        }
        set
        {
            this.ViewState["ExisteCuenta"] = value.ToString();
        }
    }

    /// <summary>
    /// Propuedad que almacena y obtiene el orden de ordenación del grid view
    /// </summary>
    private string GridViewSortDirection
    {
        get
        {
            return ViewState["SortDirection"] as string ?? "ASC";
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }


    private decimal sumaPorcent
    {
        get
        {
            return Convert.ToDecimal(this.ViewState["sumaPorcent"]);
        }
        set
        {
            this.ViewState["sumaPorcent"] = value;
        }
    }

   

  
    #endregion

    #region Eventos

    /// <summary>
    /// Evento propio que se lanza cuando se pulsa el checkbox del gridView
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de la clase SujetoPasivoEventArgs</param>
    public delegate void SujetoPasivoSeleccionadoHandler(object sender, SujetoPasivoEventArgs e);

    public event SujetoPasivoSeleccionadoHandler SujetoPasivoSeleccionado;

    /// <summary>
    /// Lanza el evento SujetoPasivoSeleccionado
    /// </summary> 
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de la clase SujetoPasivoEventArgs</param>
    private void LaunchSujetoPasivoSeleccionadoHandler(object sender, SujetoPasivoEventArgs e)
    {
        if (SujetoPasivoSeleccionado != null)
        {
            SujetoPasivoSeleccionado(sender, e);
        }
    }

    /// <summary>
    /// Evento propio que se lanza cuando se carga el control
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de la clase EventArgs</param>
    public delegate void CargaControlHandler(object sender, EventArgs e);

    /// <summary>
    /// Declaración de un nuevo evento para el user control con su ClickHandler (CargaControlHandler)
    /// </summary>
    public event CargaControlHandler CargaControl;

    /// <summary>
    /// Lanza el evento CargaControl
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de la clase EventArgs</param>
    private void LaunchCargaControlHandler(object sender, EventArgs e)
    {
        if (CargaControl != null)
        {
            CargaControl(sender, e);
        }
    }

    #endregion

    #region Cargar Declaraciones

    /// <summary>
    /// Método que carga los participantes de la declaración
    /// </summary>
    protected void CargarDSEDeclaracionConParticipantes()
    {
        if (((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Any())
        {
            using (ServiceDeclaracionIsai.DeclaracionIsaiClient proxy = new ServiceDeclaracionIsai.DeclaracionIsaiClient())
            {
                DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDT = proxy.ObtenerParticipantesPorIdDeclaracion(Convert.ToInt32(((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION));

                foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participantesDR in participantesDT.Rows)
                {
                    DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowP = ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.NewFEXNOT_PARTICIPANTESRow();

                    rowP.PORCTRANSMISION = participantesDR.PORCTRANSMISION;
                    rowP.CODTIPOPERSONA = participantesDR.CODTIPOPERSONA;
                    rowP.TIPOPERSONA = participantesDR.TIPOPERSONA;
                    rowP.IDPARTICIPANTES = participantesDR.IDPARTICIPANTES;
                    rowP.IDPERSONA = participantesDR.IDPERSONA;
                    rowP.IDDECLARACION = participantesDR.IDDECLARACION;
                    if (!participantesDR.IsCODROLNull())
                        rowP.CODROL = participantesDR.CODROL;
                    else
                        rowP.SetCODROLNull();
                    if (!participantesDR.IsROLNull())
                        rowP.ROL = participantesDR.ROL;
                    else
                        rowP.SetROLNull();
                    //if (!participantesDR.IsCODACTOJURNull())
                    //    rowP.CODACTOJUR = participantesDR.CODACTOJUR;
                    //else
                    //    rowP.SetCODACTOJURNull();
                    //if (!participantesDR.IsESSUJETOPASIVONull())
                    //    rowP.ESSUJETOPASIVO = participantesDR.ESSUJETOPASIVO;
                    //else
                    //    rowP.SetESSUJETOPASIVONull();
                    if (!participantesDR.IsCODSITUACIONESPERSONANull())
                        rowP.CODSITUACIONESPERSONA = participantesDR.CODSITUACIONESPERSONA;
                    else
                        rowP.SetCODSITUACIONESPERSONANull();
                    if (!participantesDR.IsNOMBRENull())
                        rowP.NOMBRE = participantesDR.NOMBRE;
                    else
                        rowP.SetNOMBRENull();
                    if (!participantesDR.IsAPELLIDOPATERNONull())
                        rowP.APELLIDOPATERNO = participantesDR.APELLIDOPATERNO;
                    else
                        rowP.SetAPELLIDOPATERNONull();
                    if (!participantesDR.IsAPELLIDOMATERNONull())
                        rowP.APELLIDOMATERNO = participantesDR.APELLIDOMATERNO;
                    else
                        rowP.SetAPELLIDOMATERNONull();
                    if (!participantesDR.IsNOMBREAPELLIDOSNull())
                        rowP.NOMBREAPELLIDOS = participantesDR.NOMBREAPELLIDOS;
                    else
                        rowP.SetNOMBREAPELLIDOSNull();
                    if (!participantesDR.IsRFCNull())
                        rowP.RFC = participantesDR.RFC;
                    else
                        rowP.SetRFCNull();
                    if (!participantesDR.IsCURPNull())
                        rowP.CURP = participantesDR.CURP;
                    else
                        rowP.SetCURPNull();
                    if (!participantesDR.IsCLAVEIFENull())
                        rowP.CLAVEIFE = participantesDR.CLAVEIFE;
                    else
                        rowP.SetCLAVEIFENull();
                    if (!participantesDR.IsACTIVPRINCIPNull())
                        rowP.ACTIVPRINCIP = participantesDR.ACTIVPRINCIP;
                    else
                        rowP.SetACTIVPRINCIPNull();

                    ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.AddFEXNOT_PARTICIPANTESRow(rowP);
                }
            }
        }


    }

    private void CargarParticipantesPorIdDeclaracion(decimal idDeclaracion)
    {
        //TODO: Cambiara para devolver el dataset cargado completo
         using (ServiceDeclaracionIsai.DeclaracionIsaiClient proxy = new ServiceDeclaracionIsai.DeclaracionIsaiClient())
                    {
        DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDT = proxy.ObtenerParticipantesPorIdDeclaracion(Convert.ToInt32(idDeclaracion));

        foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participantesDR in participantesDT.Rows)
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowP = ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.NewFEXNOT_PARTICIPANTESRow();

            rowP.PORCTRANSMISION = participantesDR.PORCTRANSMISION;
            rowP.CODTIPOPERSONA = participantesDR.CODTIPOPERSONA;
            rowP.TIPOPERSONA = participantesDR.TIPOPERSONA;
            rowP.IDPARTICIPANTES = participantesDR.IDPARTICIPANTES;

            rowP.IDPERSONA = participantesDR.IDPERSONA;

            //if (!participantesDR.IsIDDECLARACIONNull())
            rowP.IDDECLARACION = participantesDR.IDDECLARACION;
            //else
            //    rowP.SetIDDECLARACIONNull();
            if (!participantesDR.IsCODROLNull())
                rowP.CODROL = participantesDR.CODROL;
            else
                rowP.SetCODROLNull();
            if (!participantesDR.IsROLNull())
                rowP.ROL = participantesDR.ROL;
            else
                rowP.SetROLNull();
            //if (!participantesDR.IsCODACTOJURNull())
            //    rowP.CODACTOJUR = participantesDR.CODACTOJUR;
            //else
            //    rowP.SetCODACTOJURNull();

            if (!participantesDR.IsCODSITUACIONESPERSONANull())
                rowP.CODSITUACIONESPERSONA = participantesDR.CODSITUACIONESPERSONA;
            else
                rowP.SetCODSITUACIONESPERSONANull();
            if (!participantesDR.IsNOMBRENull())
                rowP.NOMBRE = participantesDR.NOMBRE;
            else
                rowP.SetNOMBRENull();
            if (!participantesDR.IsAPELLIDOPATERNONull())
                rowP.APELLIDOPATERNO = participantesDR.APELLIDOPATERNO;
            else
                rowP.SetAPELLIDOPATERNONull();
            if (!participantesDR.IsAPELLIDOMATERNONull())
                rowP.APELLIDOMATERNO = participantesDR.APELLIDOMATERNO;
            else
                rowP.SetAPELLIDOMATERNONull();
            if (!participantesDR.IsNOMBREAPELLIDOSNull())
                rowP.NOMBREAPELLIDOS = participantesDR.NOMBREAPELLIDOS;
            else
                rowP.SetNOMBREAPELLIDOSNull();
            if (!participantesDR.IsRFCNull())
                rowP.RFC = participantesDR.RFC;
            else
                rowP.SetRFCNull();
            if (!participantesDR.IsCURPNull())
                rowP.CURP = participantesDR.CURP;
            else
                rowP.SetCURPNull();
            if (!participantesDR.IsCLAVEIFENull())
                rowP.CLAVEIFE = participantesDR.CLAVEIFE;
            else
                rowP.SetCLAVEIFENull();
            if (!participantesDR.IsACTIVPRINCIPNull())
                rowP.ACTIVPRINCIP = participantesDR.ACTIVPRINCIP;
            else
                rowP.SetACTIVPRINCIPNull();

            ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.AddFEXNOT_PARTICIPANTESRow(rowP);
        }
         }
    }

    #endregion

    /// <summary>
    /// Obtiene los datos y carga la página
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ((PageBaseISAI)(this.Page)).Operacion = Utilidades.GetParametroUrl(Constantes.PAR_OPERACION);
                if (!string.IsNullOrEmpty(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO)))
                {
                    string stringFiltro = System.Convert.ToString(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO));
                    ((PageBaseISAI)(this.Page)).FBusqueda.RellenarObjetoFiltro(stringFiltro);
                }
                ((PageBaseISAI)(this.Page)).SortDirectionP = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR);
                ((PageBaseISAI)(this.Page)).SortExpression = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP);
                ((PageBaseISAI)(this.Page)).SortDirectionP2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR2);
                ((PageBaseISAI)(this.Page)).SortExpression2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP2);
                ObtenerDatosPorPost();

                if (!((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Any())
                {
                    CargarDSEDeclaracionConParticipantes();
                }
                CargarPagina();

                LaunchCargaControlHandler(new Object(), new EventArgs());
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
    /// Método que actualiza el estado de los botones
    /// </summary>
    /// <param name="deshabilitarBotones">Booleano para habilitar o deshabilitar los botones</param>
    public void ActualizarEstadoBotones(bool deshabilitarBotones)
    {

        bool esInserccion = ((PageBaseISAI)(this.Page)).Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_INS) == 0;
        bool esModificacion = ((PageBaseISAI)(this.Page)).Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_MOD) == 0;

        if (esInserccion || esModificacion)
        {
            DeshabilitarTodosBotones();
            if (!deshabilitarBotones)
            {
                //Desactivar todos los botones
                btnAdd.Enabled = true;
                btnAdd.ImageUrl = Constantes.IMG_ANADIR;
            }
        }

        gridViewPersonas.SelectedIndex = -1; //Ningún participante seleccionado
        uppGridParticipantes.Update();



    }

    /// <summary>
    /// Método que deshabilita todos los botones
    /// </summary>
    private void DeshabilitarTodosBotones()
    {
        btnEditar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
        btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
        btnAdd.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
        btnVer.ImageUrl = Constantes.IMG_ZOOM_DISABLED;

        btnEditar.Enabled = false;
        btnEliminar.Enabled = false;
        btnAdd.Enabled = false;
        btnVer.Enabled = false;
    }

    /// <summary>
    /// Método que habilita todos los botones
    /// </summary>
    private void HabilitarTodosBotones()
    {
        btnEditar.ImageUrl = Constantes.IMG_MODIFICA;
        btnEliminar.ImageUrl = Constantes.IMG_ELIMINA;
        btnAdd.ImageUrl = Constantes.IMG_ANADIR;
        btnVer.ImageUrl = Constantes.IMG_ZOOM;

        btnEditar.Enabled = true;
        btnEliminar.Enabled = true;
        btnAdd.Enabled = true;
        btnVer.Enabled = true;

    }

    #region cargar los datos por POST


    /// <summary>
    /// Carganos los parametros pasados por POST a las propiedades de la pagina Base de ISAI
    /// </summary>
    private void ObtenerDatosPorPost()
    {
        try
        {
            if ((PageBaseISAI)(this.Page).PreviousPage != null)
            {
                ((PageBaseISAI)(this.Page)).PaginaOrigen = ((PageBaseISAI)(this.Page).PreviousPage).PaginaOrigen;
                ((PageBaseISAI)(this.Page)).Operacion = ((PageBaseISAI)(this.Page).PreviousPage).Operacion;
                ((PageBaseISAI)(this.Page)).OperacionParticipantes = ((PageBaseISAI)(this.Page).PreviousPage).OperacionParticipantes;
                ((PageBaseISAI)(this.Page)).CondonacionJornada = ((PageBaseISAI)(this.Page).PreviousPage).CondonacionJornada;
                ((PageBaseISAI)(this.Page)).CodActoJuridicoParticipante = ((PageBaseISAI)(this.Page).PreviousPage).CodActoJuridicoParticipante;
                ((PageBaseISAI)(this.Page)).FBusqueda = ((PageBaseISAI)(this.Page).PreviousPage).FBusqueda;
                ((PageBaseISAI)(this.Page)).XmlDocumental = ((PageBaseISAI)(this.Page).PreviousPage).XmlDocumental;
                ((PageBaseISAI)(this.Page)).TipoDocumental = ((PageBaseISAI)(this.Page).PreviousPage).TipoDocumental;
                ((PageBaseISAI)(this.Page)).ListaIdsDocumental = ((PageBaseISAI)(this.Page).PreviousPage).ListaIdsDocumental;
                ((PageBaseISAI)(this.Page)).Descrip = ((PageBaseISAI)(this.Page).PreviousPage).Descrip;
                ((PageBaseISAI)(this.Page)).DescripBF = ((PageBaseISAI)(this.Page).PreviousPage).DescripBF;
                ((PageBaseISAI)(this.Page)).XmlDocumentalBF = ((PageBaseISAI)(this.Page).PreviousPage).XmlDocumentalBF;
                ((PageBaseISAI)(this.Page)).TipoDocumentalBF = ((PageBaseISAI)(this.Page).PreviousPage).TipoDocumentalBF;
                ((PageBaseISAI)(this.Page)).ListaIdsDocumentalBF = ((PageBaseISAI)(this.Page).PreviousPage).ListaIdsDocumentalBF;

                ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant = ((PageBaseISAI)(this.Page).PreviousPage).DseDeclaracionIsaiMant;
                ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiPadreMant = ((PageBaseISAI)(this.Page).PreviousPage).DseDeclaracionIsaiPadreMant;
                ((PageBaseISAI)(this.Page)).DseAvaluo = ((PageBaseISAI)(this.Page).PreviousPage).DseAvaluo;
                ((PageBaseISAI)(this.Page)).SortExpression = ((PageBaseISAI)(this.Page).PreviousPage).SortExpression;
                ((PageBaseISAI)(this.Page)).SortDirectionP = ((PageBaseISAI)(this.Page).PreviousPage).SortDirectionP;
                ((PageBaseISAI)(this.Page)).SortExpression2 = ((PageBaseISAI)(this.Page).PreviousPage).SortExpression2;
                ((PageBaseISAI)(this.Page)).SortDirectionP2 = ((PageBaseISAI)(this.Page).PreviousPage).SortDirectionP2;
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

    #region clase Persona
    //private class Persona
    //{
    //    private string rfc;

    //    public string Rfc
    //    {
    //        get { return rfc; }
    //        set { rfc = value; }
    //    }
    //    private string curp;

    //    public string Curp
    //    {
    //        get { return curp; }
    //        set { curp = value; }
    //    }
    //    private string ife;

    //    public string Ife
    //    {
    //        get { return ife; }
    //        set { ife = value; }
    //    }
    //    private string nombre;

    //    public string Nombre
    //    {
    //        get { return nombre; }
    //        set { nombre = value; }
    //    }
    //    private string tipo;

    //    public string Tipo
    //    {
    //        get { return tipo; }
    //        set { tipo = value; }
    //    }
    //    private string rol;

    //    public string Rol
    //    {
    //        get { return rol; }
    //        set { rol = value; }
    //    }
    //    private int participacion;

    //    public int Participacion
    //    {
    //        get { return participacion; }
    //        set { participacion = value; }
    //    }
    //    private bool pasivo;

    //    public bool Pasivo
    //    {
    //        get { return pasivo; }
    //        set { pasivo = value; }
    //    }
    //}
    #endregion

    #region Cargar página

    /// <summary>
    /// Método que configura y carga la página de participantes
    /// </summary>
    protected void CargarPagina()
    {
        try
        {
            if (((PageBaseISAI)(this.Page)).Operacion != null)
            {
                if (((PageBaseISAI)(this.Page)).Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
                {
                    DeshabilitarTodosBotones();
                    btnVer.Enabled = true;
                    btnVer.ImageUrl = Constantes.IMG_ZOOM;
                    btnAdd.Visible = false;
                    btnEditar.Visible = false;
                    btnEliminar.Visible = false;
                }
            }
            CargarGridViewPersonas();
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
    /// Método que carga el grid view con las personas implicadas en la declaración
    /// </summary>
    public void CargarGridViewPersonas()
    {
        try
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDTCopy = (DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable)((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Copy();

            AcortarTextos(ref participantesDTCopy);

            gridViewPersonas.DataSource = participantesDTCopy;
            gridViewPersonas.DataBind();
            if (gridViewPersonas.SelectedIndex >= 0)
            {
                SeleccionarPersona(gridViewPersonas.SelectedIndex);
            }
            else
            {
                gridViewPersonas.SelectedIndex = -1;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnVer.Enabled = false;
                btnEditar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
                btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
                btnVer.ImageUrl = Constantes.IMG_ZOOM_DISABLED;
            }
            uppGridParticipantes.Update();
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
    /// Método que acorta los textos de los campos de un datatable
    /// </summary>
    /// <param name="participantesDTCopy">DataTable Participantes</param>
    private void AcortarTextos(ref DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDTCopy)
    {
        foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowPersonas in participantesDTCopy.Rows)
        {
            if (rowPersonas.RowState != DataRowState.Deleted)
            {
                if (!rowPersonas.IsNOMBREAPELLIDOSNull())
                {
                    if (rowPersonas.NOMBREAPELLIDOS.Trim().Length > 18)
                    {
                        rowPersonas.NOMBREAPELLIDOS = string.Concat(rowPersonas.NOMBREAPELLIDOS.Trim().Substring(0, 18), "...");
                    }
                }
                if (!rowPersonas.IsRFCNull())
                {
                    if (rowPersonas.RFC.Trim().Length > 5)
                    {
                        rowPersonas.RFC = string.Concat(rowPersonas.RFC.Trim().Substring(0, 5), " ...");
                    }
                }
                if (!rowPersonas.IsCURPNull())
                {
                    if (rowPersonas.CURP.Trim().Length > 5)
                    {
                        rowPersonas.CURP = string.Concat(rowPersonas.CURP.Trim().Substring(0, 5), " ...");
                    }
                }
                if (!rowPersonas.IsCLAVEIFENull())
                {
                    if (rowPersonas.CLAVEIFE.Trim().Length > 5)
                    {
                        rowPersonas.CLAVEIFE = string.Concat(rowPersonas.CLAVEIFE.Trim().Substring(0, 5), " ...");
                    }
                }
            }
        }
    }

    #endregion
    #region Métodos Link

    /// <summary>
    /// Maneja el evento Click del control btnAdd
    /// </summary>
    /// <param name="sender">Oriegen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {          
            if (ExisteCuenta)
            {
                ((PageBaseISAI)(this.Page)).OperacionParticipantes = Constantes.PAR_VAL_OPERACION_INS;
                //Response.Redirect(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
                Server.Transfer(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
            }
            else
            {
                MostrarMensajeNoExisteCuentaCat();
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
    protected void btnEditar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
             if (ExisteCuenta)
            {
                ((PageBaseISAI)(this.Page)).OperacionParticipantes = Constantes.PAR_VAL_OPERACION_MOD;
                Server.Transfer(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
            }
            else
                MostrarMensajeNoExisteCuentaCat();


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



    protected void btnEliminarModal_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (!e.Cancel)
            {
                //REALIZADO: ELIMINO UN PARTICIPANTE DE LA DECLARACION
                if (!((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(Convert.ToDecimal(IdParticipantes), Convert.ToDecimal(((PageBaseISAI)(this.Page)).IdPersonaDeclarante)).IsROLNull())
                {
                    if (((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(Convert.ToDecimal(IdParticipantes), Convert.ToDecimal(((PageBaseISAI)(this.Page)).IdPersonaDeclarante)).ROL == Constantes.PAR_VAL_ENAJENANTE)
                        sumaPorcent -= ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(Convert.ToDecimal(IdParticipantes), Convert.ToDecimal(((PageBaseISAI)(this.Page)).IdPersonaDeclarante)).PORCTRANSMISION;
                }
                ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(Convert.ToDecimal(IdParticipantes), Convert.ToDecimal(((PageBaseISAI)(this.Page)).IdPersonaDeclarante)).Delete();
              
                gridViewPersonas.SelectedIndex = -1;
                CargarGridViewPersonas();
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
   

    ///// <summary>
    ///// Maneja el evento Click del control btnEliminarModal
    ///// </summary>
    ///// <param name="sender">Origen del evento</param>
    ///// <param name="e">Instancia de <see cref="System.ComponentModel.CancelEventArgs"/> que contiene los datos del evento</param>
    //protected void btnEliminarModal_Click(object sender, System.ComponentModel.CancelEventArgs e)
    //{
    //    try
    //    {
    //        if (!e.Cancel)
    //        {
    //            ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(Convert.ToDecimal(IdParticipantes), Convert.ToDecimal(((PageBaseISAI)(this.Page)).IdPersonaDeclarante)).Delete();
    //            gridViewPersonas.SelectedIndex = Convert.ToInt32(Constantes.UI_DDL_VALUE_NO_SELECT);
    //            CargarGridViewPersonas();
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
    /// Maneja el evento Click del control btnVer
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.ImageClickEventArgs"/> que contiene los datos del evento</param>
    protected void btnVer_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
           
            if (ExisteCuenta)
            {
                ((PageBaseISAI)(this.Page)).OperacionParticipantes = Constantes.PAR_VAL_OPERACION_VER;
                Server.Transfer(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
            }
            else
            {
                MostrarMensajeNoExisteCuentaCat();
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

    private void MostrarMensaje(string msj)
    {
        MostrarMensajeNoExisteCuentaCat();
    }

    #endregion

    #region Métodos internos del GridView

    /// <summary>
    /// Maneja el evento SelectedIndexChanged del control gridViewPersona
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SeleccionarPersona(gridViewPersonas.SelectedIndex);
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
    /// Método que selecciona una persona del grid y configura todos los controles
    /// </summary>
    /// <param name="selectedIndex"></param>
    private void SeleccionarPersona(int selectedIndex)
    {
        ((PageBaseISAI)(this.Page)).IdPersonaDeclarante = Convert.ToInt32(gridViewPersonas.DataKeys[selectedIndex].Values[0].ToString());
        IdParticipantes = Convert.ToInt32(gridViewPersonas.DataKeys[selectedIndex].Values[2].ToString());
        ((PageBaseISAI)(this.Page)).CodTipoPersona = gridViewPersonas.DataKeys[selectedIndex].Values[1].ToString();

        if (((PageBaseISAI)(this.Page)).Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) != 0)
        {
            btnEditar.ImageUrl = Constantes.IMG_MODIFICA;
            btnEliminar.ImageUrl = Constantes.IMG_ELIMINA;
            btnAdd.ImageUrl = Constantes.IMG_ANADIR;
            btnEditar.Enabled = true;
            btnEliminar.Enabled = true;
            btnAdd.Enabled = true;
        }
        btnVer.ImageUrl = Constantes.IMG_ZOOM;
        btnVer.Enabled = true;
        uppGridParticipantes.Update();
    }

    /// <summary>
    /// Maneja el evento RowDataBound del control gridViewPersonas
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> que contiene los datos del evento</param>

    protected void gridviewPersonas_DataBound(object sender, EventArgs e)
    {
        try
        {

            SujetoPasivoEventArgs argumento = new SujetoPasivoEventArgs(null, sumaPorcent);
            LaunchSujetoPasivoSeleccionadoHandler(new Object(), argumento);
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


    protected void gridViewPersonas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRol = (Label)e.Row.FindControl("lblRol");
                DropDownList ddlRol = (DropDownList)e.Row.FindControl("DropDownRol");
                e.Row.Cells[6].Text.Replace("%", string.Empty).Trim();
                LoadRoles(ddlRol, e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow>());
                //Depende del rol
                if (((PageBaseISAI)(this.Page)).Operacion == Constantes.PAR_VAL_OPERACION_VER)
                {
                    ddlRol.Visible = false;
                    ddlRol.Enabled = false;
                    lblRol.Visible = true;
                }
                if (!e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow>().IsROLNull())
                {
                    if (e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow>().ROL == Constantes.PAR_VAL_ENAJENANTE)
                        sumaPorcent += e.Row.ConvertGridViewRow<DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow>().PORCTRANSMISION;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                sumaPorcent = 0;
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
    /// Maneja el evento PageIndexChanging del control gridViewPersonas
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewPersonas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridViewPersonas.PageIndex = e.NewPageIndex;
            CargarGridViewPersonas();
            uppGridParticipantes.Update();
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
    /// Maneja el evento Sorting del control gridViewPersonas
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewPersonas_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDTCopy = (DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable)((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Copy();
            AcortarTextos(ref participantesDTCopy);
            DataView participantesDV = new DataView(participantesDTCopy);
            participantesDV.Sort = string.Format("{0} {1}", e.SortExpression.ToString(), GetSortDirection());

            gridViewPersonas.DataSource = participantesDV;
            gridViewPersonas.DataBind();
            gridViewPersonas.SelectedIndex = Convert.ToInt32(Constantes.UI_DDL_VALUE_NO_SELECT);
            btnEditar.Enabled = false;
            btnEditar.ImageUrl = Constantes.IMG_MODIFICA_DISABLED;
            btnEliminar.Enabled = false;
            btnEliminar.ImageUrl = Constantes.IMG_ELIMINA_DISABLED;
            btnVer.Enabled = false;
            btnVer.ImageUrl = Constantes.IMG_CONSULTA_DISABLED;
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
    /// Obtiene la dirección de ordenación para el grid.
    /// </summary>
    /// <returns>
    /// La dirección
    /// </returns>
    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
    }
    #endregion

    /// <summary>
    /// Método que muestra la pantalla modal que informa que la cuenta catastral introducida no es válida
    /// </summary>
    public void MostrarMensajeNoExisteCuentaCat()
    {
        if (!ExisteCuenta)
        {
            ModalInfo.TextoInformacion = Constantes.MSJ_ERROR_NOEXISTE_CUENTACAT_PARTICIPANTES;
            ModalInfo.TipoMensaje = true;
            ccInfo.Show();
        }

    }

    /// <summary>
    /// Método que carga roles en el dropdown de acuerdo a los parámetros
    /// </summary>
    /// <param name="ddlRol">DropDown que se va a cargar con los roles</param>
    /// <param name="rowParticipante"> DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow</param>
    protected void LoadRoles(DropDownList ddlRol, DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowParticipante)
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
                //    hashRoles.Add(row.CODROL);
                ddlRol.DataSource = catRolesDT;
                ddlRol.DataTextField = catRolesDT.DESCRIPCIONColumn.ToString();
                ddlRol.DataValueField = catRolesDT.CODROLColumn.ToString();
                ddlRol.DataBind();
                //insertar elemento de seleccion
                //mulloa NOV 25, 2015
                //ddlRol.Items.Insert(0, new ListItem("Seleccione un Rol", Constantes.UI_DDL_VALUE_NO_SELECT));
                //ViewState.Add(Constantes.KEY_VIEW_ROLES, hashRoles);
                if (!rowParticipante.IsCODROLNull())
                {
                    if (ddlRol.Items.FindByValue(rowParticipante.CODROL.ToString()) != null)
                        ddlRol.Items.FindByValue(rowParticipante.CODROL.ToString()).Selected = true;
                }
                else
                {
                    ddlRol.Items.FindByValue(Constantes.UI_DDL_VALUE_NO_SELECT).Selected = true;
                }
            }
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
    /// Maneja el evento SelectedIndexChanged del control ddlRol
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlRol = (DropDownList)sender;
            GridViewRow currentRow = (ddlRol.Parent.Parent as GridViewRow);
            decimal idParticipante = Convert.ToDecimal(gridViewPersonas.DataKeys[currentRow.RowIndex].Values["IDPARTICIPANTES"].ToString());
            decimal idPersona = Convert.ToDecimal(gridViewPersonas.DataKeys[currentRow.RowIndex].Values["IDPERSONA"].ToString());
            if (ddlRol.SelectedValue != Constantes.UI_DDL_VALUE_NO_SELECT)
            {

                if (ddlRol.SelectedItem.Text == Constantes.PAR_VAL_ENAJENANTE)
                    sumaPorcent += Convert.ToDecimal(gridViewPersonas.DataKeys[currentRow.RowIndex].Values["PORCTRANSMISION"].ToString());
                else if (!((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(idParticipante, idPersona).IsROLNull())
                    sumaPorcent -= Convert.ToDecimal(gridViewPersonas.DataKeys[currentRow.RowIndex].Values["PORCTRANSMISION"].ToString());

                ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(idParticipante, idPersona).CODROL = Convert.ToDecimal(ddlRol.SelectedValue);
                ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(idParticipante, idPersona).ROL = ddlRol.SelectedItem.Text;

            }
            else
            {
                if (((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(idParticipante, idPersona).ROL == Constantes.PAR_VAL_ENAJENANTE)
                {
                    sumaPorcent -= Convert.ToDecimal(gridViewPersonas.DataKeys[currentRow.RowIndex].Values["PORCTRANSMISION"].ToString());
                }
                ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(idParticipante, idPersona).SetCODROLNull();
                ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(idParticipante, idPersona).SetROLNull();
            }
            SujetoPasivoEventArgs argumento = new SujetoPasivoEventArgs(null, sumaPorcent);
            LaunchSujetoPasivoSeleccionadoHandler(new Object(), argumento);
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



    //protected void ckbPasivo_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox ckb = (CheckBox)sender;
    //        GridViewRow currentRow = (ckb.Parent.Parent as GridViewRow);
    //        decimal idParticipante = Convert.ToDecimal(gridViewPersonas.DataKeys[currentRow.RowIndex].Values["IDPARTICIPANTES"].ToString());
    //        decimal idPersona = Convert.ToDecimal(gridViewPersonas.DataKeys[currentRow.RowIndex].Values["IDPERSONA"].ToString());
    //        ((PageBaseISAI)(this.Page)).DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(idParticipante, idPersona).ESSUJETOPASIVO = ckb.Checked.ToOracleCharSNFromBoolean();


    //        if (ckb.Checked)
    //        {
    //            //deshabilita chekboxes
    //            foreach (GridViewRow gRow in gridViewPersonas.Rows)
    //            {
    //                CheckBox check = (CheckBox)gRow.FindControl("ckbPasivo");
    //                if (check != null && ckb.ClientID != check.ClientID)
    //                    check.Checked = false;
    //            }
    //            SujetoPasivoEventArgs argumento = new SujetoPasivoEventArgs(idParticipante, idPersona);
    //            LaunchSujetoPasivoSeleccionadoHandler(new Object(), argumento);
    //        }
    //        else
    //        {
    //            SujetoPasivoEventArgs argumento = new SujetoPasivoEventArgs(null, null);
    //            LaunchSujetoPasivoSeleccionadoHandler(new Object(), argumento);
    //        }


    //    }
    //    catch (FaultException<DeclaracionIsaiException> cex)
    //    {
    //        string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionPolicyWrapper.HandleException(ex);
    //        string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ex.Message;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }

    //}


    //protected void ChkPasivoVisible(DropDownList ddllblParticipacionDato, CheckBox chkPasivo)
    //{
    //    try
    //    {
    //        if (ViewState[KEY_VIEW_ROLES] == null)
    //            return;
    //        Hashtable hashRoles = (Hashtable)ViewState[KEY_VIEW_ROLES];
    //        //bool cheked = chkPasivo.Visible;
    //        if (ddllblParticipacionDato.SelectedIndex > 0)
    //            chkPasivo.Enabled = (hashRoles[Convert.ToDecimal(ddllblParticipacionDato.SelectedValue)].ToString().ToBooleanFromOracleCharSN());
    //        else
    //        {
    //            chkPasivo.Enabled = false;
    //        }
    //    }
    //    catch (FaultException<DeclaracionIsaiException> cex)
    //    {
    //        string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }
    //    catch (FaultException<DeclaracionIsaiInfoException> ciex)
    //    {
    //        string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionPolicyWrapper.HandleException(ex);
    //        string msj = "Se presentó un error al realizar la operación: " + Environment.NewLine + Environment.NewLine + ex.Message;
    //        MostrarMensajeInfoExcepcion(msj);
    //    }
    //}

    /// <summary>
    /// Lanza el evento
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //private void LaunchSujetoPasivoSeleccionadoHandler(object sender, SujetoPasivoEventArgs e)
    //{
    //    if (SujetoPasivoSeleccionado != null)
    //    {
    //        SujetoPasivoSeleccionado(sender, e);
    //    }
    //}


    #region Excepciones

    /// <summary>
    /// Pre-renderizado de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de la clase EventArgs.</param>
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
