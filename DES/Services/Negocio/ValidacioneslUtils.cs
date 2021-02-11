using System;
using System.ServiceModel;
using SIGAPred.FuentesExternas.Isai.Services.ServiceCatastral;

namespace SIGAPred.FuentesExternas.Isai.Services.Negocio
{
    /// <summary>
    /// Clase encargada de gestionar las utilidades
    /// </summary>
    public static class ValidacioneslUtils
    {
        /// <summary>
        /// Si existe un valor catastral para la cuenta y fecha que se pasan por parámetro devuelve true.
        /// Si no existe devuelve false
        /// </summary>
        /// <param name="region">Región</param>
        /// <param name="manzana">Manzana</param>
        /// <param name="lote">Lote</param>
        /// <param name="unidadprivativa">UnidadPrivativa</param>
        /// <returns></returns>
        public static bool ComprobarExisteCuentaCatastral(string region, string manzana, string lote, string unidadprivativa)
        {
            try
            {
                using (ConsultaCatastralServiceClient proxyCatastral = new ConsultaCatastralServiceClient())
                {

                    DseInmueble inmuebleDS = proxyCatastral.GetInmuebleConTitularesByClave(region, manzana, lote, unidadprivativa, false);
                    if (inmuebleDS != null && inmuebleDS.Inmueble != null && inmuebleDS.Inmueble.Count > 0)
                        return true;
                    else
                        return false;
                }

            }
            catch (FaultException<ConsultaCatastralInfoException> ex)
            {
                if (ex.Message.ToUpper().Trim().Equals("El predio solicitado no es fiscalmente válido".ToUpper().Trim()))
                {   //Si la cuenta no existe fiscalmente devolver false (la cuenta no existe)
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// A partir de una cuenta catastral y una cuenta obtiene el valor catastral correspondiente
        /// si no existe valor catastral para la fecha y la cuenta devuelve null 
        /// </summary>
        /// <param name="region">Región</param>
        /// <param name="manzana">Manzana</param>
        /// <param name="lote">Lote</param>
        /// <param name="unidadprivativa">Unidad privativa</param>
        /// <param name="fecha">Fecha</param>
        /// <returns>Valor catastral</returns>
        public static decimal? ObtenerValorCatastral(string region, string manzana, string lote, string unidadprivativa, DateTime fecha)
        {
            bool existeCuenta = false;
            // comprobar si existe la cuenta catastral
            using (ConsultaCatastralServiceClient proxyCatastral = new ConsultaCatastralServiceClient())
            {
                existeCuenta = ComprobarExisteCuentaCatastral(region, manzana, lote, unidadprivativa);
                if (existeCuenta)
                {
                    DseInmueble inmuebleDS = proxyCatastral.GetInmuebleConTitularesByClave(region, manzana, lote, unidadprivativa, false);
                    using (ServiceFiscalEmision.EmisionClient proxyFiscal = new ServiceFiscalEmision.EmisionClient())
                    {
                        int periodo = proxyFiscal.ObtenerPeriodoActual(fecha);
                        ServiceFiscalEmision.DseEmision emisionDS = proxyFiscal.ObtenerBoleta(Convert.ToInt32(inmuebleDS.Inmueble[0].IDINMUEBLE), fecha.Year, periodo);

                        if (emisionDS != null && emisionDS.FIS_EMISIONRESULTADO != null && emisionDS.FIS_EMISIONRESULTADO.Count > 0)
                        {
                            if (!emisionDS.FIS_EMISIONRESULTADO[0].IsVALORCATASTRALNull())
                                return emisionDS.FIS_EMISIONRESULTADO[0].VALORCATASTRAL;
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
