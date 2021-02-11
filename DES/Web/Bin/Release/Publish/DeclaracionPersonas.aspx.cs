using System;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Extensions;


/// <summary>
/// Clase encargada de gestionar el formulario de los participantes que forman parte en la declaración
/// </summary>
public partial class DeclaracionPersonas : PageBaseISAI
{

    /// <summary>
    /// Obtiene y carga los datos de los participantes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Operacion = Utilidades.GetParametroUrl(Constantes.PAR_OPERACION);
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
                //Compruebo si ya existen participantes en el dataSet
                if (!DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Any())
                {
                    base.CargarParticipantesPorIdDeclaracion(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION);
                }
                CargarPagina();
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

    #region Cargar los datos por POST
    
    /// <summary>
    /// Método que carga los parámetros pasados por POST a las propiedades de la pagina Base de ISAI
    /// </summary>
    private void ObtenerDatosPorPost()
    {
        try
        {
            PaginaOrigen = ((PageBaseISAI)PreviousPage).PaginaOrigen;
            Operacion = ((PageBaseISAI)PreviousPage).Operacion;
            OperacionParticipantes = ((PageBaseISAI)PreviousPage).OperacionParticipantes;
            CondonacionJornada = ((PageBaseISAI)PreviousPage).CondonacionJornada;
            CodActoJuridicoParticipante = ((PageBaseISAI)PreviousPage).CodActoJuridicoParticipante;
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
    
    #region Cargar pagina

    /// <summary>
    /// Método que configura y carga los datos en el grid
    /// </summary>
    private void CargarPagina()
    {
        try
        {
            if (OperacionParticipantes.CompareTo(Constantes.PAR_VAL_OPERACION_VER) == 0)
            {
                btnAdd.Enabled = false;
                btnAdd.ImageUrl = Constantes.IMG_ANADIR_DISABLED;
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
    /// Método que carga el grid view de las personas ya introducidas y dependendiendo de estas configura los botones
    /// </summary>
    private void CargarGridViewPersonas()
    {
        try
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDTCopy = (DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable)DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.Copy();

            foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowPersonas in participantesDTCopy.Rows)
            {
                if (rowPersonas.RowState != DataRowState.Deleted)
                {
                    if (!rowPersonas.IsNOMBREAPELLIDOSNull())
                    {
                        if (rowPersonas.NOMBREAPELLIDOS.Trim().Length > 20)
                        {
                            rowPersonas.NOMBREAPELLIDOS = string.Concat(rowPersonas.NOMBREAPELLIDOS.Trim().Substring(0, 20), " ...");
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

            gridViewPersonas.DataSource = participantesDTCopy;
            gridViewPersonas.DataBind();
            if (gridViewPersonas.Rows.Count > 0)
            {
                gridViewPersonas.SelectedIndex = 0;
                SeleccionarPersona(0);
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
            UpdatePanel1.Update();
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
    #region Métodos Link

    /// <summary>
    /// Maneja el evento click del control btnAdd
    /// Especifica que la operación es de inserción
    /// y redirige a la página de persona
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            OperacionParticipantes = Constantes.PAR_VAL_OPERACION_INS;
            Server.Transfer(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
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
    /// Maneja el evento click del control btnEditar
    /// Especifica que la operación es de edición
    /// y redirige a la página de persona
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEditar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            OperacionParticipantes = Constantes.PAR_VAL_OPERACION_MOD;
            Server.Transfer(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
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
    /// Maneja el evento click del control btnEliminar
    /// Si se confirma que se quiere eliminar se borra el participante seleccionado en el grid
    /// y se recarga
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEliminarModal_Click(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (!e.Cancel)
            {
                DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.FindByIDPARTICIPANTESIDPERSONA(Convert.ToDecimal(IdParticipantes), Convert.ToDecimal(IdPersonaDeclarante)).Delete();
                CargarGridViewPersonas();
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
    /// Maneja el evento click del control btnVer
    /// Especifica que la operación es de consulta
    /// y redirige a la página de persona
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVer_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            OperacionParticipantes = Constantes.PAR_VAL_OPERACION_VER;
            Server.Transfer(Constantes.URL_SUBISAI_DECLARACIONPERSONA);
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
    /// Maneja el evento click del control lnkVolverDeclaracion
    /// Redirige a la página de declaración
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkVolverDeclaracion_Click(object sender, EventArgs e)
    {
        try
        {
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
    #endregion

    #region Métodos internos del GridView

    /// <summary>
    /// Maneja el evento SelectedIndexChanged del control gridViewPersonas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Método que configura los controles teniendo en que cuenta que el participante seleccionado es el indicado
    /// </summary>
    /// <param name="selectedIndex">Indice del participante seleccionado</param>
    private void SeleccionarPersona(int selectedIndex)
    {
        IdPersonaDeclarante = Convert.ToInt32(gridViewPersonas.DataKeys[selectedIndex].Values[0].ToString());
        IdParticipantes = Convert.ToInt32(gridViewPersonas.DataKeys[selectedIndex].Values[2].ToString());
        CodTipoPersona = gridViewPersonas.DataKeys[selectedIndex].Values[1].ToString();

        if (Operacion.CompareTo(Constantes.PAR_VAL_OPERACION_VER) != 0)
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
    }

    
    protected void gridViewPersonas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridViewPersonas.PageIndex = e.NewPageIndex;
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

    /// <summary>
    /// Maneja el evento Sorting del control gridViewPersonas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewPersonas_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            DataView participantesDV = new DataView(DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES);
            participantesDV.Sort = string.Format("{0} {1}", e.SortExpression.ToString(), GetSortDirection());
            gridViewPersonas.DataSource = participantesDV;
            gridViewPersonas.DataBind();
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
    /// Función que devuelve la dirección con la que corresponde ordenar el grid
    /// </summary>
    /// <returns></returns>
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
