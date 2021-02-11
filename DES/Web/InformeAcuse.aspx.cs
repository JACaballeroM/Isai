using System;
using System.Data;
using System.Linq;
using System.ServiceModel;
using Microsoft.Reporting.WebForms;
using ServiceCatastral;
using ServiceDeclaracionIsai;


/// <summary>
/// Clase encargada de generar el informe de acuse de la declaración.
/// </summary>
public partial class InformeAcuse : PageBaseISAI
{
    /// <summary>
    /// Load de la página InformeAcuse
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
                rpvAcuseDeclaracion.Visible = false;
                HiddenIdDeclaracion.Value = Utilidades.GetParametroUrl(Constantes.PAR_IDDECLARACION);
                HiddenIdAvaluo.Value = Utilidades.GetParametroUrl(Constantes.PAR_IDAVALUO);
                if (string.IsNullOrEmpty(HiddenIdAvaluo.Value))
                {
                    lnkVolverDeclaraciones.Text = "Volver a Bandeja Jornada";
                }
                //CargarAcuse();
                rpvAcuseDeclaracion.Visible = true;
                //Limpiamos el origen de datos.
                rpvAcuseDeclaracion.LocalReport.DataSources.Clear();
                CargarAcuseGen();
                CargarAcusePar();

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

    private void CargarAcusePar()
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
            rpvAcuseDeclaracion.LocalReport.DataSources.Add(reportDataSourceAcusePar);


            rpvAcuseDeclaracion.DataBind();
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

    private void CargarAcuseGen()
    {
        try
        {
           

            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfJustificante.FEXNOT_INF_ACUSEGEN_PDataTable infAcuseGenDT;
                      

            //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
            infAcuseGenDT = null;
            infAcuseGenDT = ClienteDeclaracionIsai.ObtenerInfAcuseGen(Convert.ToDecimal(HiddenIdDeclaracion.Value));

            DseInmueble inmueble = ClienteCatastral.GetInmuebleByClave(infAcuseGenDT[0].REGCATASTRAL,
                                                  infAcuseGenDT[0].MANZCATASTRAL,
                                                  infAcuseGenDT[0].LOTECATASTRAL, infAcuseGenDT[0].LOCCATASTRAL);


            int idAvaluo;
            ServiceAvaluos.DseAvaluoMantInf direccion = null;
            //Cargaremos los valores del Avaluo llamando al servicio WCF Avaluos a través del WCF de DeclaracionIsai
            if (!string.IsNullOrEmpty(HiddenIdAvaluo.Value) && Int32.TryParse(HiddenIdAvaluo.Value, out idAvaluo))
            {
                direccion = ClienteAvaluo.GetAvaluoAntecedentes(idAvaluo);
            }


            if (direccion != null)
            {
                if (!direccion.FEXAVA_AVALUO[0].IsCALLENull())
                {
                    infAcuseGenDT[0].CALLENUMEXTERIORINMUEBLE = string.Format("{0} {1}", direccion.FEXAVA_AVALUO[0].CALLE, direccion.FEXAVA_AVALUO[0].EXT);
                }
                else
                {
                    infAcuseGenDT[0].CALLENUMEXTERIORINMUEBLE = string.Format("{0} {1}", string.Empty, direccion.FEXAVA_AVALUO[0].EXT);
                }

                try
                {
                    //infAcuseGenDT[0].MANZANAINMUEBLE = direccion.FEXAVA_AVALUO[0].MANZANA_CC;
                }
                catch
                {
                    infAcuseGenDT[0].MANZANAINMUEBLE = "-";
                }
                try
                {
                    infAcuseGenDT[0].LOTEINMUEBLE = direccion.FEXAVA_AVALUO[0].LOTE;
                }
                catch
                {
                    infAcuseGenDT[0].LOTEINMUEBLE = "-";
                }
                try
                {
                    infAcuseGenDT[0].NUMINTERIORINMUEBLE = direccion.FEXAVA_AVALUO[0].INT;
                }
                catch
                {
                    infAcuseGenDT[0].NUMINTERIORINMUEBLE = "-";

                }

                if (!direccion.FEXAVA_AVALUO[0].IsCOLONIANull())
                {
                    infAcuseGenDT[0].COLONIAINMUEBLE = direccion.FEXAVA_AVALUO[0].COLONIA;
                }
                else
                {
                    infAcuseGenDT[0].COLONIAINMUEBLE = "-";
                }

                infAcuseGenDT[0].DELEGACIONINMUEBLE = !direccion.FEXAVA_AVALUO[0].IsDELEGACIONNull() ? direccion.FEXAVA_AVALUO[0].DELEGACION : "-";

                if (!direccion.FEXAVA_AVALUO[0].IsCPNull())
                {
                    infAcuseGenDT[0].CODIGOPOSTALINMUEBLE = direccion.FEXAVA_AVALUO[0].CP;
                }
                else
                {
                    infAcuseGenDT[0].CODIGOPOSTALINMUEBLE = "-";
                }
            }
            else
            {

                DseInmueblePredio inmueblePredDS = ClienteCatastral.GetInmueblePredioByClave(infAcuseGenDT[0].REGCATASTRAL,
                                                   infAcuseGenDT[0].MANZCATASTRAL,
                                                   infAcuseGenDT[0].LOTECATASTRAL);
                if (inmueblePredDS.Predios.Any())
                {
                    if (!inmueblePredDS.Predios[0].IsCalleDescNull())
                    {
                        infAcuseGenDT[0].CALLENUMEXTERIORINMUEBLE = inmueblePredDS.Predios[0].CalleDesc;
                    }
                    else
                    {
                        infAcuseGenDT[0].CALLENUMEXTERIORINMUEBLE = string.Empty;
                    }

                    infAcuseGenDT[0].MANZANAINMUEBLE = infAcuseGenDT[0].MANZCATASTRAL;
                    infAcuseGenDT[0].LOTEINMUEBLE = infAcuseGenDT[0].LOTECATASTRAL;
                    infAcuseGenDT[0].NUMINTERIORINMUEBLE = inmueblePredDS.Predios[0].NUMERO;
                    if (!inmueblePredDS.Predios[0].IsColoniaDescNull())
                    {
                        infAcuseGenDT[0].COLONIAINMUEBLE = inmueblePredDS.Predios[0].ColoniaDesc;
                    }
                    else
                    {
                        infAcuseGenDT[0].COLONIAINMUEBLE = string.Empty;
                    }
                    infAcuseGenDT[0].DELEGACIONINMUEBLE = inmueblePredDS.Predios[0].DELEGACION;

                    if (!inmueblePredDS.Predios[0].IsCODIGOPOSTALNull())
                    {
                        infAcuseGenDT[0].CODIGOPOSTALINMUEBLE = inmueblePredDS.Predios[0].CODIGOPOSTAL;
                    }
                    else
                    {
                        infAcuseGenDT[0].CODIGOPOSTALINMUEBLE = string.Empty;
                    }
                }

            }

            DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dtDec = ClienteDeclaracionIsai.ObtenerDeclaracionesPorIdDeclaracion(Convert.ToInt32(HiddenIdDeclaracion.Value));
            string estado = string.Empty;
            try
            {
                if (!dtDec[0].CODESTADOPAGO.ToString().Equals(Convert.ToInt32( EstadosPago.RecibidoSISCOR ).ToString()))
                {
                    estado = "1";
                }
                else if (dtDec[0].FECHAPAGO <= dtDec[0].FECHAVIGENCIALINEACAPTURA && EstadoDeclaracionValido(dtDec[0].CODESTADODECLARACION))
                {
                    estado = "2";
                }
                else
                    estado = "1";
            }
            catch
            {
                estado = "1";
            }


            ReportParameter parBor = new ReportParameter("par_borrador", estado);
            rpvAcuseDeclaracion.LocalReport.SetParameters(new ReportParameter[1] { parBor });

            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceAcuse = new ReportDataSource("FEXNOT_INF_ACUSEGEN_P", (DataTable)infAcuseGenDT);

            //Añadimos el reportDataSource al reportViewer
            rpvAcuseDeclaracion.LocalReport.DataSources.Add(reportDataSourceAcuse);
           
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

    private bool EstadoDeclaracionValido(decimal codEstado)
    {
        EstadosDeclaraciones estado = (EstadosDeclaraciones)codEstado;
        return estado == EstadosDeclaraciones.Presentada ||
               estado == EstadosDeclaraciones.Pendiente ||
               estado == EstadosDeclaraciones.Aceptada ||
               estado == EstadosDeclaraciones.Inconsistente;
    }

    /// <summary>
    /// Método que carga los datos y los completa del Informe del justificante de la declaración por idDeclaracion.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CargarAcuse()
    {
        try
        {
            rpvAcuseDeclaracion.Visible = true;

            //Declaramos el DataTable
            ServiceDeclaracionIsai.DseInfJustificante.FEXNOT_INFACUSEDataTable infAcuseDT;

            //Limpiamos el origen de datos.
            rpvAcuseDeclaracion.LocalReport.DataSources.Clear();

            //Obtenemos los EnviosTotales asociados a los datos introducidos dependiendo del radio buton seleccionado
            infAcuseDT = null;
            infAcuseDT = ClienteDeclaracionIsai.ObtenerInfAcuse(Convert.ToDecimal(HiddenIdDeclaracion.Value));
            DseInmueble inmueble = ClienteCatastral.GetInmuebleByClave(infAcuseDT[0].REGCATASTRAL,
                                                                       infAcuseDT[0].MANZCATASTRAL,
                                                                       infAcuseDT[0].LOTECATASTRAL, 
                                                                       infAcuseDT[0].LOCCATASTRAL);
            if (inmueble.Inmueble.Any())
            {
                if (!inmueble.Inmueble[0].IsCalleNumeroNull())
                {
                    infAcuseDT[0].CALLENUMEXTERIORINMUEBLE = inmueble.Inmueble[0].CalleNumero;
                }
                else
                {
                    infAcuseDT[0].CALLENUMEXTERIORINMUEBLE = string.Empty;
                }

                infAcuseDT[0].MANZANAINMUEBLE = infAcuseDT[0].MANZCATASTRAL;
                infAcuseDT[0].LOTEIMUEBLE = infAcuseDT[0].LOTECATASTRAL;
                if (!inmueble.Inmueble[0].IsNUMEROINTERIORNull())
                {
                    infAcuseDT[0].NUMINTERIORINMUEBLE = inmueble.Inmueble[0].NUMEROINTERIOR;
                }
                else
                {
                    infAcuseDT[0].NUMINTERIORINMUEBLE = string.Empty;
                }
                if (!inmueble.Inmueble[0].IsColoniaNull())
                {
                    infAcuseDT[0].COLONIAINMUEBLE = inmueble.Inmueble[0].Colonia;
                }
                else
                {
                    infAcuseDT[0].COLONIAINMUEBLE = string.Empty;
                }

                infAcuseDT[0].DELEGACIONINMUEBLE = inmueble.Inmueble[0].NOMBREDELEGACION.ToString();

                if (!inmueble.Inmueble[0].IsCODIGOPOSTALNull())
                {
                    infAcuseDT[0].CODIGOPOSTALINMUEBLE = inmueble.Inmueble[0].CODIGOPOSTAL;
                }
                else
                {
                    infAcuseDT[0].CODIGOPOSTALINMUEBLE = string.Empty;
                }
            }
            else
            {

                DseInmueblePredio inmueblePredDS = ClienteCatastral.GetInmueblePredioByClave(infAcuseDT[0].REGCATASTRAL,
                                                   infAcuseDT[0].MANZCATASTRAL,
                                                   infAcuseDT[0].LOTECATASTRAL);
                if (inmueblePredDS.Predios.Any())
                {
                    if (!inmueblePredDS.Predios[0].IsCalleDescNull())
                    {
                        infAcuseDT[0].CALLENUMEXTERIORINMUEBLE = inmueblePredDS.Predios[0].CalleDesc;
                    }
                    else
                    {
                        infAcuseDT[0].CALLENUMEXTERIORINMUEBLE = string.Empty;
                    }

                    infAcuseDT[0].MANZANAINMUEBLE = infAcuseDT[0].MANZCATASTRAL;
                    infAcuseDT[0].LOTEIMUEBLE = infAcuseDT[0].LOTECATASTRAL;
                    infAcuseDT[0].NUMINTERIORINMUEBLE = inmueblePredDS.Predios[0].NUMERO;
                    if (!inmueblePredDS.Predios[0].IsColoniaDescNull())
                    {
                        infAcuseDT[0].COLONIAINMUEBLE = inmueblePredDS.Predios[0].ColoniaDesc;
                    }
                    else
                    {
                        infAcuseDT[0].COLONIAINMUEBLE = string.Empty;
                    }
                    infAcuseDT[0].DELEGACIONINMUEBLE = inmueblePredDS.Predios[0].DELEGACION;

                    if (!inmueblePredDS.Predios[0].IsCODIGOPOSTALNull())
                    {
                        infAcuseDT[0].CODIGOPOSTALINMUEBLE = inmueblePredDS.Predios[0].CODIGOPOSTAL;
                    }
                    else
                    {
                        infAcuseDT[0].CODIGOPOSTALINMUEBLE = string.Empty;
                    }
                }

            }
            DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dtDec = ClienteDeclaracionIsai.ObtenerDeclaracionesPorIdDeclaracion(Convert.ToInt32(HiddenIdDeclaracion.Value));
            string estado = string.Empty;
            try
            {
                if (!dtDec[0].CODESTADOPAGO.ToString().Equals(EstadosPago.RecibidoSISCOR.ToString()))
                {
                    estado = "1";
                }
                else if (dtDec[0].FECHAPAGO <= dtDec[0].FECHAVIGENCIALINEACAPTURA)
                {
                    estado = "2";
                }
                else
                    estado = "1";
            }
            catch
            {
                estado = "1";
            }


            ReportParameter parBor = new ReportParameter("par_borrador", estado);
            rpvAcuseDeclaracion.LocalReport.SetParameters(new ReportParameter[1] { parBor});
            //Declaramos nuevos reportDatasource de tipo infoAvaluosTotales e InfoDeclaracionesTotales para introducirlo en el reportviewer
            ReportDataSource reportDataSourceAcuse = new ReportDataSource("FEXNOT_INFACUSE", (DataTable)infAcuseDT);

            //Añadimos el reportDataSource al reportViewer
            rpvAcuseDeclaracion.LocalReport.DataSources.Add(reportDataSourceAcuse);
            rpvAcuseDeclaracion.DataBind();
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
    /// Método que nos redirige a la pagina referida (PAGINA ORIGEN)
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
