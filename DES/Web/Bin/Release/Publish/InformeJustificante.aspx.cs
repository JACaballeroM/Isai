using System;
using System.Data;
using System.Linq;
using System.ServiceModel;
using Microsoft.Reporting.WebForms;
using ServiceCatastral;
using ServiceDeclaracionIsai;

/// <summary>
/// Clase encargada de generar el informe del justificante de la declaración
/// </summary>
public partial class InformeJustificante : PageBaseISAI
{
    /// <summary>
    /// Load de la página InformeConsultaEspecifica.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO)))
                {
                    string stringFiltro = System.Convert.ToString(Utilidades.GetParametroUrl(Constantes.REQUEST_FILTRO));
                    FBusqueda.RellenarObjetoFiltro(stringFiltro);
                }
                SortDirectionP = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR);
                SortExpression = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP);
                SortDirectionP2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTDIR2);
                SortExpression2 = Utilidades.GetParametroUrl(Constantes.REQUEST_SORTEXP2);
                //Ocultamos el report hasta que se agrege un DataSource
                rpvJustificanteDeclaracion.Visible = false;
                HiddenIdDeclaracion.Value = Utilidades.GetParametroUrl(Constantes.PAR_IDDECLARACION);
                HiddenIdAvaluo.Value = Utilidades.GetParametroUrl(Constantes.PAR_IDAVALUO);
                if (string.IsNullOrEmpty(HiddenIdAvaluo.Value))
                {
                    lnkVolverDeclaraciones.Text = "Volver a Bandeja Jornada";
                }
                CargarJustificante();
                CargarParticipantes();

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
    /// Método que muestra el Informe del justificante de la declaracion por idDeclaracion.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CargarJustificante()
    {
        try
        {

            int idAvaluo;
            ServiceAvaluos.DseAvaluoMantInf direccion = null;
            //Cargaremos los valores del Avaluo llamando al servicio WCF Avaluos a través del WCF de DeclaracionIsai
            if (!string.IsNullOrEmpty(HiddenIdAvaluo.Value) && Int32.TryParse(HiddenIdAvaluo.Value, out idAvaluo))
            {
                direccion = ClienteAvaluo.GetAvaluoAntecedentes(idAvaluo);
            }
   
            rpvJustificanteDeclaracion.Visible = true;
            string artReduccion = "k) Reducción";
            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfJustificante.FEXNOT_INFJUSTIFICANTEDataTable infJustificanteDT;

            //Limpiamos el origen de datos.
            rpvJustificanteDeclaracion.LocalReport.DataSources.Clear();

            //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
            infJustificanteDT = null;

            //infJustificanteDT = ClienteDeclaracionIsai.ObtenerInfJustificante(Convert.ToDecimal(HiddenIdDeclaracion.Value));
            infJustificanteDT = ClienteDeclaracionIsai.ObtenerInfJustificanteGen(Convert.ToDecimal(HiddenIdDeclaracion.Value));

            string par_actualizacion = string.Empty;
            decimal actualizacion = infJustificanteDT[0].IsPORCENTAJEACTUALIZACIONNull()?0: infJustificanteDT[0].PORCENTAJEACTUALIZACION;
            if (actualizacion == 0)
            {
                par_actualizacion = "0.0000";
            }
            else if (actualizacion == 1)
            {
                par_actualizacion = "1.0000";
            }
            else if(actualizacion<1)
            {
                par_actualizacion = (actualizacion + 1).ToString().Replace(",",".");
                string[] temp = par_actualizacion.Split('.');
                if (temp.Count() > 1)
                {
                    if (temp[1].Length != 4)
                    {
                        if (temp[1].Length < 4)
                        {
                            temp[1].PadLeft(4, '0');
                        }
                        else
                        {
                            temp[1] = temp[1].Substring(0, 4);
                        }
                        par_actualizacion = string.Format("{0}.{1}", temp[0], temp[1]);
                    }
                }
                else
                {
                    par_actualizacion = string.Format("{0}.0000", temp[0]);
                }
            }

            ReportParameter parAcatualizacion = new ReportParameter("par_actualizacion", par_actualizacion);
            var d = ClienteDeclaracionIsai.ObtenerDeclaracionesPorIdDeclaracion(Convert.ToInt32(HiddenIdDeclaracion.Value));
            DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable  dtDec =  ClienteDeclaracionIsai.ObtenerDeclaracionesPorIdDeclaracion(Convert.ToInt32(HiddenIdDeclaracion.Value));
            if (dtDec.Any())
            {
                if (!string.IsNullOrEmpty(dtDec[0].FECHACAUSACION.ToString()))
                {
                    infJustificanteDT[0].FECHAPREVENTIVA = dtDec[0].FECHACAUSACION;
                }
                if (!dtDec[0].IsIDREDUCCIONNull())
                    artReduccion = string.Format("{0} art. {1}", artReduccion, ClienteDeclaracionIsai.ObtenerArticulo(dtDec[0].IDREDUCCION));
                infJustificanteDT[0].OBSERVACIONES = ! dtDec[0].IsOBSERVACIONESNull()? dtDec[0].OBSERVACIONES: string.Empty;
            }
            if (infJustificanteDT[0].CODTIPODECLARACION == 3)
            {
                decimal? porcentajeCondonacionJornada;
                CondonacionJornada = IsaiManager.ObtenerCondonacion(dtDec[0].VALORCATASTRAL,
                !dtDec[0].IsFECHACAUSACIONNull() ?
                    dtDec[0].FECHACAUSACION : DateTime.Now, out porcentajeCondonacionJornada);
                if (CondonacionJornada.HasValue)
                {
                    infJustificanteDT[0].PORCENTAJECONDONACION = (float)porcentajeCondonacionJornada.Value;
                }
            }
            if (infJustificanteDT[0].IsPORCENTAJEREDUCCIONNull())
                infJustificanteDT[0].PORCENTAJEREDUCCION = 0;
            DseInmueble inmueble = ClienteCatastral.GetInmuebleByClave(dtDec[0].REGION,
                                                   dtDec[0].MANZANA,
                                                   dtDec[0].LOTE, dtDec[0].UNIDADPRIVATIVA);
            if (direccion!=null)
            {
                if (!direccion.FEXAVA_AVALUO[0].IsCALLENull())
                {
                    infJustificanteDT[0].CALLENUMEXTERIORINMUEBLE = string.Format("{0} {1}", direccion.FEXAVA_AVALUO[0].CALLE, direccion. FEXAVA_AVALUO[0].EXT);
                }
                else
                {
                    infJustificanteDT[0].CALLENUMEXTERIORINMUEBLE = string.Format("{0} {1}", string.Empty, direccion.FEXAVA_AVALUO[0].EXT);
                }

                try
                {
                    infJustificanteDT[0].MANZANAINMUEBLE = direccion.FEXAVA_AVALUO[0].MANZANA_CC;
                }
                catch
                {
                    infJustificanteDT[0].MANZANAINMUEBLE = "-";
                }
                try
                {
                    infJustificanteDT[0].LOTEIMUEBLE = direccion.FEXAVA_AVALUO[0].LOTE;
                }
                catch
                {
                    infJustificanteDT[0].LOTEIMUEBLE = "-";
                }
                try
                {
                    infJustificanteDT[0].NUMINTERIORINMUEBLE =  direccion.FEXAVA_AVALUO[0].INT ;
                }
                catch
                {
                    infJustificanteDT[0].NUMINTERIORINMUEBLE = "-";
                
                }
                
                if (!direccion.FEXAVA_AVALUO[0].IsCOLONIANull())
                {
                    infJustificanteDT[0].COLONIAINMUEBLE = direccion.FEXAVA_AVALUO[0].COLONIA;
                }
                else
                {
                    infJustificanteDT[0].COLONIAINMUEBLE = "-";
                }

                infJustificanteDT[0].DELEGACIONINMUEBLE = !direccion.FEXAVA_AVALUO[0].IsDELEGACIONNull()?direccion.FEXAVA_AVALUO[0].DELEGACION:"-";

                if (!direccion.FEXAVA_AVALUO[0].IsCPNull())
                {
                    infJustificanteDT[0].CODIGOPOSTALINMUEBLE = direccion.FEXAVA_AVALUO[0].CP;
                }
                else
                {
                    infJustificanteDT[0].CODIGOPOSTALINMUEBLE = "-";
                }
            }

            else
            {
                DseInmueblePredio inmueblePredDS = ClienteCatastral.GetInmueblePredioByClave(dtDec[0].REGION,
                                                   dtDec[0].MANZANA,
                                                   dtDec[0].LOTE);


                if (inmueblePredDS.Predios.Any())
                {
                    if (!inmueblePredDS.Predios[0].IsCalleDescNull())
                    {
                        infJustificanteDT[0].CALLENUMEXTERIORINMUEBLE = inmueblePredDS.Predios[0].CalleDesc;
                    }
                    else
                    {
                        infJustificanteDT[0].CALLENUMEXTERIORINMUEBLE = string.Empty;
                    }

                    infJustificanteDT[0].MANZANAINMUEBLE = dtDec[0].MANZANA;
                    infJustificanteDT[0].LOTEIMUEBLE = dtDec[0].LOTE;
                    infJustificanteDT[0].NUMINTERIORINMUEBLE = inmueblePredDS.Predios[0].NUMERO;
                    if (!inmueblePredDS.Predios[0].IsColoniaDescNull())
                    {
                        infJustificanteDT[0].COLONIAINMUEBLE = inmueblePredDS.Predios[0].ColoniaDesc;
                    }
                    else
                    {
                        infJustificanteDT[0].COLONIAINMUEBLE = string.Empty;
                    }
                    infJustificanteDT[0].DELEGACIONINMUEBLE = inmueblePredDS.Predios[0].DELEGACION;

                    if (!inmueblePredDS.Predios[0].IsCODIGOPOSTALNull())
                    {
                        infJustificanteDT[0].CODIGOPOSTALINMUEBLE = inmueblePredDS.Predios[0].CODIGOPOSTAL;
                    }
                    else
                    {
                        infJustificanteDT[0].CODIGOPOSTALINMUEBLE = string.Empty;
                    }
                }

            }
            if (!d.FirstOrDefault().IsCONDONACIONXFECHANull())
            {
                infJustificanteDT.FirstOrDefault().PORCENTAJECONDONACION = 1;
                infJustificanteDT.FirstOrDefault().IMPORTECONDONACION = d.FirstOrDefault().CONDONACIONXFECHA;
            }
            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceJustificante = new ReportDataSource("DseInfJustificante_FEXNOT_INFJUSTIFICANTE", (DataTable)infJustificanteDT);

            //Obtenemos la Url del report ConsultaEspecificaDetalle pasandole el idDeclaracion seleccionado en el report.

            ReportParameter parArticulo = new ReportParameter("articuloReduccion", artReduccion);

            try
            {
                if (!dtDec[0].CODESTADOPAGO.ToString().Equals(Convert.ToInt32( EstadosPago.RecibidoSISCOR).ToString()))
                {
                    dtDec[0].CODESTADODECLARACION = 1;
                }
                else if (dtDec[0].FECHAPAGO <= dtDec[0].FECHAVIGENCIALINEACAPTURA && EstadoDeclaracionValido(dtDec[0].CODESTADODECLARACION))
                {
                    dtDec[0].CODESTADODECLARACION = 2;
                }
                else
                    dtDec[0].CODESTADODECLARACION = 1;
            }
            catch
            {
                dtDec[0].CODESTADODECLARACION = 1;
            }

            
            ReportParameter parBor = new ReportParameter("parBorrador", dtDec[0].CODESTADODECLARACION.ToString());
            rpvJustificanteDeclaracion.LocalReport.SetParameters(new ReportParameter[3] { parBor, parArticulo, parAcatualizacion });
            //rpvJustificanteDeclaracion.LocalReport.SetParameters(new ReportParameter[1] { parArticulo });

              //Añadimos el reportDataSource al reportViewer
            rpvJustificanteDeclaracion.LocalReport.DataSources.Add(reportDataSourceJustificante);
            //rpvJustificanteDeclaracion.DataBind();
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

    private bool EstadoDeclaracionValido(decimal codEstado)
    {
        EstadosDeclaraciones estado = (EstadosDeclaraciones)codEstado;
        return estado == EstadosDeclaraciones.Presentada ||
               estado == EstadosDeclaraciones.Pendiente ||
               estado == EstadosDeclaraciones.Aceptada ||
               estado == EstadosDeclaraciones.Inconsistente;
    }

    protected void CargarParticipantes()
    {
        try
        {
            

            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfJustificante.FEXNOT_INF_ACUSEPAR_PDataTable infAcusePArDT;
            

            //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio button seleccionado
            infAcusePArDT = null;
            infAcusePArDT = ClienteDeclaracionIsai.ObtenerInfAcusePar(Convert.ToDecimal(HiddenIdDeclaracion.Value));

           
            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceAcusePar = new ReportDataSource("FEXNOT_INF_ACUSEPAR_P", (DataTable)infAcusePArDT);

            //Añadimos el reportDataSource al reportViewer
            rpvJustificanteDeclaracion.LocalReport.DataSources.Add(reportDataSourceAcusePar);


   
         
             rpvJustificanteDeclaracion.DataBind();
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
    /// Nos redirige a la pagina referida (PAGINA ORIGEN)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkVolverDeclaraciones_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(HiddenIdAvaluo.Value))
            {
                RedirectUtil.BaseURL = Constantes.URL_SUBISAI_BANDEJAJORNADA;
            }
            else
            {
                RedirectUtil.BaseURL = Constantes.URL_SUBISAI_DECLARACIONES;
                RedirectUtil.AddParameter(Constantes.PAR_IDAVALUO, HiddenIdAvaluo.Value);
            }
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
