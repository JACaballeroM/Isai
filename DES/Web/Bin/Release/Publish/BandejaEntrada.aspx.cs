using System;
using System.ComponentModel;
using System.Data;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;
using SIGAPred.Common.Seguridad;

/// <summary>
/// Clase encarga de gestionar la bandeja de entrada
/// </summary>
public partial class BandejaEntrada : PageBase
{

    private string Vigencia;
    protected TipoFiltroBusqueda _busquedaActual;

    #region Métodos

    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (PreviousPage != null)
                {
                    string name = PreviousPage.GetType().Name;
                    switch (name)
                    {
                        case "home_aspx":
                            AsignarFechasPorDefecto();
                            HiddenVigente.Value = Constantes.PAR_VIGENTE;
                            AsignarValoresBusquedaBE();
                            break;
                    }
                }
                else
                {
                    CargarSort();

                    string filtro = Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO);

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        FBusqueda.RellenarObjetoFiltro(filtro);
                        CargarCamposFiltro();
                        ObtenerValorVigencia();
                        if (FBusqueda.EsCuenta())
                        {
                            CargarPagina(TipoFiltroBusqueda.CuentaCatastral);
                        }
                        else
                        {
                            CargarPagina(TipoFiltroBusqueda.Fecha);
                        }
                    }
                    else
                    {
                        AsignarFechasPorDefecto();
                    }
                }

                hidBusquedaActual.Value = ((int)_busquedaActual).ToString();
            }
            else
            {
                _busquedaActual = (TipoFiltroBusqueda)(System.Convert.ToInt32(hidBusquedaActual.Value));
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
    /// Método que asigna a los textbox las fechas por defecto    
    /// </summary>
    private void AsignarFechasPorDefecto()
    {
        try
        {
            txtFechaIni.Text = DateTime.Now.AddDays(-29).ToShortDateString();
            txtFechaFin.Text = DateTime.Now.ToShortDateString();
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
    /// Ordena el gridViewAvaluos tal y como estaba antes de ir a la pantalla de declaraciones
    /// </summary>
    private void CargarSort()
    {
        try
        {
            SortDirection direction;
            SortExpression = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP);
            SortDirectionP = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR);
            switch (SortDirectionP)
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
            gridViewAvaluos.Sort(SortExpression, direction);
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
    /// Configura todos los controles del filtro tal y como estaban antes de ir a la pantalla de declaraciones
    /// </summary>
    private void CargarCamposFiltro()
    {
        try
        {
            if (FBusqueda.EsCuenta())
            {
                RestriccionesFiltro(TipoFiltroBusqueda.CuentaCatastral);
            }
            else if (FBusqueda.EsFecha())
            {
                RestriccionesFiltro(TipoFiltroBusqueda.Fecha);
            }
            else if (FBusqueda.EsIdavaluo())
            {
                RestriccionesFiltro(TipoFiltroBusqueda.IdAvaluo);
            }
            else if (FBusqueda.EsNumAvaluo())
            {
                RestriccionesFiltro(TipoFiltroBusqueda.NumeroAvaluo);
            }

            txtRegion.Text = FBusqueda.Region;
            txtLote.Text = FBusqueda.Lote;
            txtManzana.Text = FBusqueda.Manazana;
            txtUnidadPrivativa.Text = FBusqueda.UnidadPrivativa;
            txtFechaIni.Text = FBusqueda.FechaIni;
            txtFechaFin.Text = FBusqueda.FechaFin;
            txtPerito.Text = FBusqueda.Registro;
            rbFechas.Checked = FBusqueda.EsFecha();
            rbCuenta.Checked = FBusqueda.EsCuenta();
            rbIdAvaluo.Checked = FBusqueda.EsIdavaluo();
            rbNumeroAvaluo.Checked = FBusqueda.EsNumAvaluo();
            txtIdAvaluo.Text = FBusqueda.Idavaluo;
            textNumeroAvaluo.Text = FBusqueda.NumAvaluo;

            switch (FBusqueda.Vigencia)
            {
                case Constantes.PAR_VIGENCIA_TODOS:
                    RadioButtonTodos.Checked = true;
                    RadioButtonVigente.Checked = false;
                    RadioButtonNoVigente.Checked = false;
                    break;
                case Constantes.PAR_VIGENTE:
                    RadioButtonTodos.Checked = false;
                    RadioButtonVigente.Checked = true;
                    RadioButtonNoVigente.Checked = false;
                    break;
                case Constantes.PAR_NO_VIGENTE:
                    RadioButtonTodos.Checked = false;
                    RadioButtonVigente.Checked = false;
                    RadioButtonNoVigente.Checked = true;
                    break;
            }

            AsignarValoresBusquedaBE();
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
    /// Cargar pagina.
    /// </summary>
    /// <param name="tipoFiltroBusqueda">Tipo filtro búsqueda.</param>
    private void CargarPagina(TipoFiltroBusqueda tipoFiltroBusqueda)
    {
        try
        {
            HiddenIdPersonaToken.Value = Usuarios.IdPersona();
            AsignarValoresBusquedaBE();
            _busquedaActual = tipoFiltroBusqueda;
            FBusqueda.Fecha = tipoFiltroBusqueda.ToString();
            switch (tipoFiltroBusqueda)
            {
                case TipoFiltroBusqueda.CuentaCatastral:
                    txtCuenta.Text = string.Empty;
                    txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;

                    if (Condiciones.Web(Constantes.FUN_EXTPARAISAI))
                    {
                        gridViewAvaluos.DataSourceID = odsPorCuentaCatastral.ID;
                    }

                    else if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
                    {
                        gridViewAvaluos.DataSourceID = odsPorCuentaCatastralSF.ID;
                    }

                    break;
                default:
                    if (Condiciones.Web(Constantes.FUN_EXTPARAISAI))
                    {
                        gridViewAvaluos.DataSourceID = odsPorFecha.ID;
                    }
                    else if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
                    {
                        gridViewAvaluos.DataSourceID = odsPorFechaSF.ID;
                    }

                    break;
            }
            gridViewAvaluos.PageIndex = 0;
            gridViewAvaluos.DataBind();
            gridViewAvaluos.SelectedIndex = -1;
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
    /// Registrar las funciones javascript a los botones que integran con rango de fechas
    /// </summary>
    private void RegistrarJSButton()
    {
        try
        {
            btnBuscar.OnClientClick = "javascript:rangoFechasMaxAno('" + txtFechaIni.ClientID + "','" + txtFechaFin.ClientID + "')";
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
    ///Oculta, vacia y desactiva todos los campos y validaciones asociad@s a cualquier opción de busqueda
    /// y selecciona estado = TODOS y vigencia = TODOS
    /// </summary>
    private void DesactivarTodo()
    {
        //FECHAS
        DesactivarFechas();

        //Nº AVALÚO/PERITO/IDAVALUO
        DesactivarNumAvaluo();

        //IDAVALUO
        DesactivarIdAvaluo();

        //REGIÓN/MANZANA/LOTE/UNIDAD PRIVATIVA (CUENTA CATASTRAL)
        DesactivarCuentaCatastral();

        //VIGENCIA
        SeleccionarVigenciaVigente();
    }

    /// <summary>
    ///Oculta, vacia y desactiva todos los campos y validaciones asociad@s  a la busqueda por por fecha 
    /// </summary>
    private void DesactivarFechas()
    {
        txtFechaIni.Text = string.Empty;
        txtFechaFin.Text = string.Empty;

        txtFechaFin.Enabled = false;
        txtFechaIni.Enabled = false;

        rfvFechaFin.Enabled = false;
        rfvFechaInicio.Enabled = false;

        btnFechaFin.Enabled = false;
        btnFechaIni.Enabled = false;

        cvFechaInicio.Enabled = false;
        cvFechaFin.Enabled = false;
        cvRangoFechas.Enabled = false;
    }

    /// <summary>
    /// Oculta, vacia y desactiva todos los campos y validaciones asociad@s  a la busqueda por numAvaluo
    /// </summary>
    private void DesactivarNumAvaluo()
    {
        textNumeroAvaluo.Text = string.Empty;
        txtPerito.Text = string.Empty;

        textNumeroAvaluo.Enabled = false;
        txtPerito.Enabled = false;

        textNumeroAvaluo.Text = string.Empty;
        txtPerito.Text = string.Empty;
        btnPeritos.Enabled = false;
        btnPeritos.ImageUrl = Constantes.IMG_USER_DISABLED;

        rfvNumeroAvaluo.Enabled = false;
        revNumeroPerito.Enabled = false;
    }

    /// <summary>
    /// Oculta, vacia y desactiva todos los campos y validaciones asociad@s  a la busqueda por idAvaluo
    /// </summary>
    private void DesactivarIdAvaluo()
    {
        txtIdAvaluo.Enabled = false;
        txtIdAvaluo.Text = string.Empty;
        rfvIdAvaluo.Enabled = false; // añadido 2.2.8

        revIdAvaluo.Enabled = false;
    }

    /// <summary>
    /// Selecciona la opción Todos en el radiobutton vigencia 
    /// </summary>
    private void SeleccionarVigenciaTodos()
    {
        RadioButtonNoVigente.Checked = false;
        RadioButtonVigente.Checked = false;
        RadioButtonTodos.Checked = true;
    }

    /// <summary>
    /// Selecciona la opción Todos en el radiobutton vigencia 
    /// </summary>
    private void SeleccionarVigenciaVigente()
    {
        RadioButtonNoVigente.Checked = false;
        RadioButtonVigente.Checked = true;
        RadioButtonTodos.Checked = false;
    }

    /// <summary>
    /// Oculta, vacia y desactiva todos los campos y validaciones asociad@s a la búsqueda por cuenta catastral
    /// </summary>
    private void DesactivarCuentaCatastral()
    {
        txtRegion.Text = string.Empty;
        txtManzana.Text = string.Empty;
        txtLote.Text = string.Empty;
        txtUnidadPrivativa.Text = string.Empty;

        txtRegion.Enabled = false;
        txtManzana.Enabled = false;
        txtLote.Enabled = false;
        txtUnidadPrivativa.Enabled = false;

        rfvRegion.Enabled = false;
        rfvManzana.Enabled = false;
        rfvLote.Enabled = false;
        rfvUnidadPrivativa.Enabled = false;
    }

    /// <summary>
    /// Método que oculta, vacia, y desactiva los controles del filtro dependiendo del páramentro de entrada
    /// </summary>
    /// <param name="busquedaFiltro">Tipo de filtro</param>
    private void RestriccionesFiltro(TipoFiltroBusqueda busquedaFiltro)
    {
        try
        {
            DesactivarTodo();

            switch (busquedaFiltro)
            {
                case TipoFiltroBusqueda.CuentaCatastral:

                    txtRegion.Enabled = true;
                    txtManzana.Enabled = true;
                    txtLote.Enabled = true;
                    txtUnidadPrivativa.Enabled = true;

                    rfvRegion.Enabled = true;
                    rfvManzana.Enabled = true;
                    rfvLote.Enabled = true;
                    rfvUnidadPrivativa.Enabled = true;

                    txtRegion.CssClass = Constantes.STYLE_TEXTBOX_OBLIGATORIO;
                    txtManzana.CssClass = Constantes.STYLE_TEXTBOX_OBLIGATORIO;
                    txtLote.CssClass = Constantes.STYLE_TEXTBOX_OBLIGATORIO;
                    txtUnidadPrivativa.CssClass = Constantes.STYLE_TEXTBOX_OBLIGATORIO;

                    break;
                case TipoFiltroBusqueda.Fecha:

                    txtFechaFin.Enabled = true;
                    txtFechaIni.Enabled = true;


                    txtRegion.CssClass = "TextBoxNormal";
                    txtManzana.CssClass = "TextBoxNormal";
                    txtLote.CssClass = "TextBoxNormal";
                    txtUnidadPrivativa.CssClass = "TextBoxNormal";

                    rfvFechaInicio.Enabled = true;
                    rfvFechaFin.Enabled = true;
                    cvRangoFechas.Enabled = true;
                    cvFechaInicio.Enabled = true;
                    cvFechaFin.Enabled = true;

                    btnFechaIni.Enabled = true;
                    btnFechaFin.Enabled = true;

                    break;
                case TipoFiltroBusqueda.IdAvaluo:
                    txtIdAvaluo.Enabled = true;
                    rfvIdAvaluo.Enabled = true;
                    revIdAvaluo.Enabled = true;

                    break;
                case TipoFiltroBusqueda.NumeroAvaluo:
                    textNumeroAvaluo.Enabled = true;
                    rfvNumeroAvaluo.Enabled = true;
                    txtPerito.Enabled = true;
                    txtPerito.Visible = true;

                    lblNumPerSoci.Visible = true;
                    this.btnPeritos.Visible = true;
                    this.btnPeritos.Enabled = true;
                    btnPeritos.Enabled = true;
                    btnPeritos.ImageUrl = Constantes.IMG_USER;

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
    /// Asigna al campo HiddenVigente el valor que corresponda según el radiobutton seleccionado (vigente, no vigente o todos)
    /// </summary>
    private void ObtenerValorVigencia()
    {
        if (this.RadioButtonVigente.Checked)
        {
            HiddenVigente.Value = Constantes.PAR_VIGENTE;
        }
        else if (this.RadioButtonNoVigente.Checked)
        {
            HiddenVigente.Value = Constantes.PAR_NO_VIGENTE;
        }
        else if (this.RadioButtonTodos.Checked)
        {
            HiddenVigente.Value = Constantes.PAR_VIGENCIA_TODOS;
        }
    }

    
    /// <summary>
    /// Guarda los valores especificados en el filtro de búsqueda
    /// </summary>
    private void AsignarValoresBusquedaBE()
    {
        //Filtro fecha
        HiddenFechaFin.Value = txtFechaFin.Text;
        HiddenFechaIni.Value = txtFechaIni.Text;

        //Filtro cuenta catastral
        txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;
        HiddenCuentaCat.Value = txtCuenta.Text;

        //Filtro registro
        HiddenRegistro.Value = txtPerito.Text;

        //Num Avalúo
        HiddenNumAvaluo.Value = textNumeroAvaluo.Text;

        //IdAvaluo
        //Eliminar los ceros a la izquierda
        if (txtIdAvaluo.Text.Length > 0)
        {
            string numStr = (txtIdAvaluo.Text).Substring(11, (txtIdAvaluo.Text.Length - 11));
            int num = numStr.ToInt();
            txtIdAvaluo.Text = (txtIdAvaluo.Text).Replace(numStr, num.ToString());
            // Fin eliminar ceros a la izquierda 
        }
        HiddenNumUnicoAv.Value = txtIdAvaluo.Text;

        Vigencia = HiddenVigente.Value;
    }

    /// <summary>
    /// Almacena en la FBusqueda los valores de los filtros utilizados para la búsqueda
    /// </summary>
    private void ActualizarFiltroBusquedaBE()
    {
        //Busqueda por fecha
        FBusqueda.FechaIni = string.Empty;
        FBusqueda.FechaFin = string.Empty;

        //Busqueda por cuentacatastral
        FBusqueda.Region = string.Empty;
        FBusqueda.Lote = string.Empty;
        FBusqueda.Manazana = string.Empty;
        FBusqueda.UnidadPrivativa = string.Empty;

        //Filtro registro
        FBusqueda.Registro = string.Empty;
        FBusqueda.Vigencia = string.Empty;

        FBusqueda.FechaIni = txtFechaIni.Text;
        FBusqueda.FechaFin = txtFechaFin.Text;

        FBusqueda.Lote = txtLote.Text;
        FBusqueda.Manazana = txtManzana.Text;
        FBusqueda.Region = txtRegion.Text;
        FBusqueda.UnidadPrivativa = txtUnidadPrivativa.Text;

        FBusqueda.Registro = txtPerito.Text;
        FBusqueda.Vigencia = HiddenVigente.Value.ToString();

        FBusqueda.Idavaluo = string.Empty;
        FBusqueda.NumAvaluo = string.Empty;
        FBusqueda.Idavaluo = txtIdAvaluo.Text;
        FBusqueda.NumAvaluo = textNumeroAvaluo.Text;
    }

    /// <summary>
    /// Carga el gridview realizando la búsqueda con los parámetros introducidos
    /// </summary>
    private void CargarGridView()
    {
        try
        {
            if (rbCuenta.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.CuentaCatastral);
                hidBusquedaActual.Value = TipoFiltroBusqueda.CuentaCatastral.ToInt().ToString();
            }
            else if (rbFechas.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.Fecha);
                hidBusquedaActual.Value = TipoFiltroBusqueda.Fecha.ToInt().ToString();
            }
            else if (rbIdAvaluo.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.IdAvaluo);
                hidBusquedaActual.Value = TipoFiltroBusqueda.IdAvaluo.ToInt().ToString();
            }
            else if (rbNumeroAvaluo.Checked)
            {
                CargarPagina(TipoFiltroBusqueda.NumeroAvaluo);
                hidBusquedaActual.Value = TipoFiltroBusqueda.NumeroAvaluo.ToInt().ToString();
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

    #region Eventos del RadioButton


    /// <summary>
    /// Maneja el evento CheckedChangeddel control rbBusquedaGroup
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void rbBusquedaGroup_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            if (sender == rbCuenta)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.CuentaCatastral);
            }
            else if (sender == rbFechas)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.Fecha);
                AsignarFechasPorDefecto();
            }
            else if (sender == rbNumeroAvaluo)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.NumeroAvaluo);
            }

            else if (sender == rbIdAvaluo)
            {
                RestriccionesFiltro(TipoFiltroBusqueda.IdAvaluo);
            }


        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION  +Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
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

    #region Botones

   


    /// <summary>
    /// Manejador de eventos. Llamado por btnBuscar para eventos click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            AsignarValoresBusquedaBE();
            txtCuenta.Text = string.Empty;
            txtCuenta.Text = txtRegion.Text + "-" + txtManzana.Text + "-" + txtLote.Text + "-" + txtUnidadPrivativa.Text;
            ObtenerValorVigencia();
            CargarGridView();
            UpdatePanelGrid.Update();
            ActualizarFiltroBusquedaBE();
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION  +Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
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
    /// Manejador de eventos. Llamado por btnPeritos para eventos click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
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

   

    /// <summary>
    /// Manejador de eventos. Llamado por btnEliminarBusqueda para eventos click.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
    protected void btnEliminarBusqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            rbIdAvaluo.Checked = false;
            rbCuenta.Checked = false;
            rbNumeroAvaluo.Checked = false;
            rbFechas.Checked = true;
            RestriccionesFiltro(TipoFiltroBusqueda.Fecha);
            txtFechaIni.Text = DateTime.Now.AddDays(-29).ToShortDateString();
            txtFechaFin.Text = DateTime.Now.ToShortDateString();
            HiddenFechaFin.Value = string.Empty;
            HiddenFechaIni.Value = string.Empty;
            gridViewAvaluos.Sort(String.Empty, SortDirection.Ascending);
            CargarPagina(TipoFiltroBusqueda.Fecha);
            UpdatePanelBusqueda.Update();
            UpdatePanelGrid.Update();
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

    #region Eventos del GridView


   

    /// <summary>
    /// Maneja el evento SelectedIndexChanged del control gridViewAvaluos
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento</param>
    protected void gridViewAvaluos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //JABS Redireccionamos al detalled e las declaraciones. Declaraciones.aspx
            RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACIONES;
            RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, gridViewAvaluos.SelectedDataKey["IDAVALUO"].ToString());
            RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTEXP, SortExpression);
            RedirectUtil.AddParameter(Constantes.REQUEST_SORTDIR, SortDirectionP);
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
    /// Maneja el evento RowDataBound del control gridViewAvaluos
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.web.UI.WebControls.GridViewRowEventArgs"/> que contiene los datos del evento</param>
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

    /// <summary>
    /// Maneja el evento Sorting del control gridViewAvaluos
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.web.UI.WebControls.GridViewSortEventArgs"/> que contiene los datos del evento</param>
    protected void gridViewAvaluos_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortExpression = e.SortExpression;
            SortDirectionP = e.SortDirection.ToString();
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

    #region Eventos de los UserControl

    /// <summary>
    /// Maneja el evento ConfirmClick del control buscarPerito
    /// </summary>
    /// <param name="sender">Origen del evento</param>
    /// <param name="e">Instancia de <see cref="System.ComponentModel.CancelEventArgs"/> que contiene los datos del evento</param></param>
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
                        txtPerito.Text = ModalBuscarPeritos1.NumeroRegistro.ToString();
                }
                UpdatePanelBusqueda.Update();
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
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
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
