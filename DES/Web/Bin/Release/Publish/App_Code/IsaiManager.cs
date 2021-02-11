using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using ServiceAvaluos;
using ServiceCatastral;
using ServiceDeclaracionIsai;
using SIGAPred.Common.Seguridad;
using System.Web;
using Microsoft.IdentityModel.Claims;
using SIGAPred.Seguridad.Utilidades.ClaimTypes;

/// <summary>
/// Clase encargada de ofrecer funciones para las operaciones de ISAI
/// </summary>
public class IsaiManager
{

    /// <summary>
    /// Función estática que obtiene el identificador de la jornada notarial
    /// </summary>
    /// <param name="valorCatastral">Valor catastral para saber el rango en el que se encuentra</param>
    /// <param name="fechaEscritura">Fecha de la escritura para extraer el año</param>
    /// <param name="porcentajeCondonacion">Resultado</param>
    /// <returns>Resultado</returns>
    public static decimal? ObtenerCondonacion(decimal valorCatastral, DateTime fechaEscritura, out decimal? porcentajeCondonacion)
    {
        porcentajeCondonacion = null;

        DseCatalogo.FEXNOT_JORNADANOTARIALRow[] jornadaNotarialDR = ApplicationCache.DseCatalogoISAI.FEXNOT_JORNADANOTARIAL.Where(jn =>
        jn.ANIO == Convert.ToDecimal(fechaEscritura.Year) &&
        jn.LIMITEINFERIOR <= valorCatastral &&
        jn.LIMITESUPERIOR >= valorCatastral).ToArray();

        //toma en cuenta solo el primer renglon
        if (jornadaNotarialDR != null && jornadaNotarialDR.Length > 0 && jornadaNotarialDR[0] != null)
        {
            porcentajeCondonacion = jornadaNotarialDR[0].PORCENTAJEREDUCCION;
            return jornadaNotarialDR[0].IDJORNADANOTARIAL;
        }
        return null;
    }


    /// <summary>
    /// Función que devuelve verdadero si la declaración es elegible para tasa cero
    /// </summary>
    /// <param name="declaracion">Dataset DseDeclaracionIsai</param>
    /// <param name="ValorCatastralActualizado">Valor catastral</param>
    /// <returns></returns>
    public static bool HabilitarTasaCero(DseDeclaracionIsai declaracion, decimal? ValorCatastralActualizado)
    {
        if (declaracion != null)
        {
            if (declaracion.FEXNOT_DECLARACION.Count() > 0)
            {
                if (!declaracion.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
                {
                    if (declaracion.FEXNOT_DECLARACION[0].CODACTOJUR == Convert.ToDecimal(Constantes.VALOR_CODACTOJUR_HERENCIA) && ValorCatastralActualizado != null)
                    {
                        //Obtener el valor actual del salario mínimo
                        DateTime hoy = DateTime.Now;

                        //extrae los salario cuya vigencia haya iniciado antes ( o como mucho, el mismo dia) que la fecha de consulta
                        //porque los salarios con fecha futura todavia no estaran vigentes
                        SalarioMinimo salario = (from sm
                                                in ApplicationCache.SalariosMinimos
                                                 where sm.Vigencia <= hoy
                                                 orderby sm.Vigencia
                                                 select sm).ToList().Last();

                        //el salario minimo que aplique sera el ultimo de la lista, es decir, el mas actual

                        //si valor inmueble menor que el limite, habilitar tasa cero
                        //valor de inmueble, maximo entre valor catastral y valor avaluo

                        decimal valorReferencia = 0;
                        if (declaracion != null && !declaracion.FEXNOT_DECLARACION[0].IsVALORAVALUONull())
                            valorReferencia = (!declaracion.FEXNOT_DECLARACION[0].IsVALORCATASTRALNull() &&
                                               declaracion.FEXNOT_DECLARACION[0].VALORCATASTRAL > declaracion.FEXNOT_DECLARACION[0].VALORAVALUO) ?
                                                declaracion.FEXNOT_DECLARACION[0].VALORCATASTRAL : declaracion.FEXNOT_DECLARACION[0].VALORAVALUO;

                        if (ValorCatastralActualizado.HasValue && (ValorCatastralActualizado.Value > valorReferencia))
                            valorReferencia = ValorCatastralActualizado.Value;

                        return (valorReferencia <= (salario.Monto * Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["LimiteTasaCero"])));
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }
        return false;
    }


    /// <summary>
    /// Función que devuelve un control de tipo enumerador con el resultado del proceso de obtención de línea de captura.
    /// </summary>
    /// <param name="declaracion">Dataset DseDeclaracionIsai</param>
    /// <param name="ErrorMessage">Si se ha sido erronea la operación contendra el mensaje de error</param>
    /// <returns>LineaCapturaResultMessage</returns>
    public static LineaCapturaResultMessage ObtenerLineaCaptura(ref DseDeclaracionIsai declaracion, string fechaCausacion, out string ErrorMessage)
    {
        LineaCapturaResultMessage result = LineaCapturaResultMessage.Exito;
        ErrorMessage = null;
        string nombreRevisor = null;
        if (declaracion.FEXNOT_DECLARACION != null && declaracion.FEXNOT_DECLARACION.Any() && declaracion.FEXNOT_DECLARACION[0] != null)
        {
            if (!declaracion.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull() && declaracion.FEXNOT_DECLARACION[0].IMPUESTOPAGADO > 1)
            {
                //validar estado de la declaracion
                if (declaracion.FEXNOT_DECLARACION[0].CODESTADODECLARACION == Convert.ToDecimal(Constantes.PAR_VAL_ESTADODECLARACION_PDTE_PRESENTAR))
                {
                    //validar linea de captura preexistente
                    //si NO existe linea de captura, o la existente ya NO esta vigente
                    if (declaracion.FEXNOT_DECLARACION[0].IsLINEACAPTURANull()
                        || String.IsNullOrEmpty(declaracion.FEXNOT_DECLARACION[0].LINEACAPTURA)
                        || (!declaracion.FEXNOT_DECLARACION[0].IsFECHAVIGENCIALINEACAPTURANull() && declaracion.FEXNOT_DECLARACION[0].FECHAVIGENCIALINEACAPTURA >= DateTime.Now))
                    {
                        //Nombre del revisor
                        if (Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION))
                        {
                          SIGAPred.Common.Token.TokenTransport token = new SIGAPred.Common.Token.TokenBuilder(HttpContext.Current.User.Identity).Token;
                          nombreRevisor =token.Name + " "+  token.Surname;
                        //  nombreRevisor = (from c in ((IClaimsIdentity)HttpContext.Current.User.Identity).Claims where c.ClaimType == PromocaClaims.Name select c).First().Value +" "+
                         //      (from c in ((IClaimsIdentity)HttpContext.Current.User.Identity).Claims where c.ClaimType == PromocaClaims.Surname select c).First().Value + " " +
                         //      (from c in ((IClaimsIdentity)HttpContext.Current.User.Identity).Claims where c.ClaimType == PromocaClaims.ApellidoMaterno select c).First().Value;


                        }
                        //obten la linea de captura
                        try
                        {
                            using (ServiceDeclaracionIsai.DeclaracionIsaiClient proxy = new ServiceDeclaracionIsai.DeclaracionIsaiClient())
                            {
                                //declaracion = proxy.ObtenerLineaCaptura(declaracion, Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION), nombreRevisor, fechaCausacion);
                                declaracion = proxy.ObtenerLCIsai(declaracion, Condiciones.Web(Constantes.FUN_PERMITIRTOMADECISION), nombreRevisor, fechaCausacion);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<ServiceDeclaracionIsai.DeclaracionIsaiException>;
                            if (faultEx != null)
                            {
                                if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                                    ErrorMessage = faultEx.Detail.Descripcion;
                                //si no hay un mensaje de error detallado, usar la descripcion en la enumeracion
                            }
                            else
                            {
                                if (ex.InnerException != null)
                                    ErrorMessage = ex.InnerException.Message;
                                else
                                    ErrorMessage = ex.Message;
                            }
                            result = LineaCapturaResultMessage.Error;
                        }
                    }
                    else
                        //existe una linea de captura previa que esta vigente
                        result = LineaCapturaResultMessage.LineaCapturaVigente;
                }
                else
                    result = LineaCapturaResultMessage.EstadoDeclaracionInvalido;
            }
            else
                result = LineaCapturaResultMessage.ImpuestoEsCero;
        }
        else
            result = LineaCapturaResultMessage.ParametroInvalido;

        return result;
    }



    /// <summary>
    /// Función que comprueba si existe un valor catastral para la cuenta que se pasa por parámetro.
    /// </summary>
    /// <param name="region">Región de la cuenta catastral</param>
    /// <param name="manzana">Manzana de la cuenta catastral</param>
    /// <param name="lote">Lote de la cuenta catastral</param>
    /// <param name="unidadprivativa">Unidad privativa de la cuenta catastral</param>
    /// <returns>True: Existe valor; False: No existe valor</returns>
    public static bool ComprobarExisteCuentaCatastral(string region, string manzana, string lote, string unidadprivativa)
    {
        try
        {
            using (ConsultaCatastralServiceClient proxyCatastral = new ConsultaCatastralServiceClient())
            {
                DseInmueble inmuebleDS = proxyCatastral.GetInmuebleConTitularesByClave(region, manzana, lote, unidadprivativa, false);
                if (inmuebleDS != null && inmuebleDS.Inmueble != null && inmuebleDS.Inmueble.Any())
                    return true;
                else
                    return false;
            }

        }
        catch (FaultException<ConsultaCatastralInfoException> ex)
        {
            if (ex.Message.ToUpper().Trim().Equals(Constantes.MSJ_PREDIO_FISCALMENTE_NO_VALIDO.ToUpper().Trim()))
            {   //Si la cuenta no existe fiscalmente devolver false (la cuenta no existe)
                return false;
            }
            throw;
        }
    }



    /// <summary>
    /// Función que obtiene el valor catastral
    /// </summary>
    /// <param name="declaracion">DseDeclaracionIsai</param>
    /// <param name="avaluo">DseAvaluoConsulta</param>
    /// <param name="fecha">Fecha de escritura</param>
    /// <param name="ErrorMessage">Error por el cual no se ha podido obtener el valor catastral</param>
    /// <returns>Valor catastral</returns>
    public static decimal? ObtenerValorCatastral(DseDeclaracionIsai declaracion, DseAvaluoConsulta avaluo, DateTime fecha, out string ErrorMessage)
    {
        bool existeCuenta = false;
        bool continuar = true;
        ErrorMessage = Constantes.MSJ_ERROR_OPERACION;
        DateTime fechaValidacionCatastral;

        //si la fecha limite en el Key esta mal, se procedera a intentar obtener el valor catastral
        if (DateTime.TryParseExact(ConfigurationManager.AppSettings["FechaValidacionCatastral"].ToString(), "yyyy-MM-dd",
            new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentCulture.Name),
            System.Globalization.DateTimeStyles.None, out fechaValidacionCatastral))
        {
            //si la fecha de escritura es anterior a la fecha limite de validacion, no se intentara obtener el valor catastral
            if (fecha < fechaValidacionCatastral)
            {
                continuar = false;
            }
        }
        if (continuar)
        {
            string region = null, lote = null, manzana = null, unidadprivativa = null;
            try
            {
                using (ConsultaCatastralServiceClient proxyCatastral = new ConsultaCatastralServiceClient())
                {
                    if (declaracion.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString().CompareTo(Constantes.PAR_VAL_TIPODECLARACION_JOR) == 0)
                    {
                        region = declaracion.FEXNOT_DECLARACION[0].REGION;
                        manzana = declaracion.FEXNOT_DECLARACION[0].MANZANA;
                        lote = declaracion.FEXNOT_DECLARACION[0].LOTE;
                        unidadprivativa = declaracion.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;
                    }
                    else if (avaluo != null && avaluo.FEXAVA_AVALUO_V != null && avaluo.FEXAVA_AVALUO_V.Any() && avaluo.FEXAVA_AVALUO_V[0] != null)
                    {
                        region = avaluo.FEXAVA_AVALUO_V[0].REGION;
                        manzana = avaluo.FEXAVA_AVALUO_V[0].MANZANA;
                        lote = avaluo.FEXAVA_AVALUO_V[0].LOTE;
                        unidadprivativa = avaluo.FEXAVA_AVALUO_V[0].UNIDADPRIVATIVA;
                    }
                    existeCuenta = ComprobarExisteCuentaCatastral(region, manzana, lote, unidadprivativa);
                    if (existeCuenta)
                    {
                        DseInmueble inmuebleDS = proxyCatastral.GetInmuebleConTitularesByClave(region, manzana, lote, unidadprivativa, false);
                        using (ServiceFiscalEmision.EmisionClient proxyFiscal = new ServiceFiscalEmision.EmisionClient())
                        {
                            int periodo = proxyFiscal.ObtenerPeriodoActual(fecha);
                            ServiceFiscalEmision.DseEmision emisionDS = proxyFiscal.ObtenerBoleta(Convert.ToInt32(inmuebleDS.Inmueble[0].IDINMUEBLE), fecha.Year, periodo);
                            if (emisionDS != null && emisionDS.FIS_EMISIONRESULTADO != null && emisionDS.FIS_EMISIONRESULTADO.Any())
                            {
                                if (!emisionDS.FIS_EMISIONRESULTADO[0].IsVALORCATASTRALNull())
                                {
                                    return emisionDS.FIS_EMISIONRESULTADO[0].VALORCATASTRAL;
                                }
                                else
                                {
                                    ErrorMessage = Constantes.MSJ_ERROR_NOEXISTE_VALCAT_FECHA;
                                }
                            }
                            else
                            {
                                ErrorMessage = Constantes.MSJ_ERROR_NOEXISTE_VALCAT_FECHA;
                            }
                        }
                    }
                    else
                    {
                        ErrorMessage = Constantes.MSJ_ERROR_NOEXISTE_CUENTACAT;
                    }
                }
            }
            catch (Exception)
            {
                ErrorMessage = Constantes.MSJ_ERROR_NOEXISTE_VALCAT_FECHA;
            }
        }
        else
        {
            ErrorMessage = Constantes.MSJ_ERROR_NOEXISTE_VALCAT_FECHA;
        }
        return null;
    }
}
