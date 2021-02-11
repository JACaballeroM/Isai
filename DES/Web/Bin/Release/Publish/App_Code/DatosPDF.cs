using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using ServiceDeclaracionIsai;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using SIGAPred.Common.DigitoVerificador;
using System.Text;
using System.Data;
using System.Configuration;
using SIGAPred.Common.Extensions;

/// <summary>
/// Contiene los metodos y propiedades necesarios para crear el pdf del fmt
/// </summary>
namespace Sigapred.DatosPDF
{
    public class DatosPDF
    {

        /// <summary>
        /// Obtiene o establece la región de la cuenta predial
        /// </summary>
        public string region
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece la manzana de la cuenta predial
        /// </summary>
        public string manzana
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el lote de la cuenta predial
        /// </summary>
        public string lote
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece la unidad privativa de la cuenta predial
        /// </summary>
        public string unidadPrivativa
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el adquirente de la cuenta predial
        /// </summary>
        public string adquirente
        {

            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el encargo condonado
        /// </summary>
        public string recargoCondonado { get; set; }

        /// <summary>
        /// Metodo para Obtener el nombre del adquirente
        /// </summary>
        /// <param name="participantes">DataTable que contiene los datos de los participantes de la declaración</param>
        /// <returns>EL nombre completo del adquirente</returns>
        public string GetAdquirente(DataTable participantes)
        {
           
                //DataTable orders = dseDeclaracion.Tables["FEXNOT_PARTICIPANTES"];

            var adui = (from m in participantes.AsEnumerable()
                            where m.Field<string>("ROL") == "Adquirente" 
                            select m).First();

                //adui["NOMBREAPELLIDOS"].ToString();
                return string.Format("{0} {1} {2}",
                    string.IsNullOrEmpty(adui["NOMBRE"].ToString().ToUpper()) ? string.Empty : adui["NOMBRE"].ToString().ToUpper()
                    , string.IsNullOrEmpty(adui["APELLIDOMATERNO"].ToString().ToUpper()) ? string.Empty : adui["APELLIDOMATERNO"].ToString().ToUpper()
                    , string.IsNullOrEmpty(adui["APELLIDOPATERNO"].ToString().ToUpper()) ? string.Empty : adui["APELLIDOPATERNO"].ToString().ToUpper()
                    );
            
        }

        /// <summary>
        /// Obtiene o establece el tipo de pago
        /// </summary>
        public TipoPago TipoPago
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el rfc del notario
        /// </summary>
        public string rfc { get; set; }

        /// <summary>
        /// Obtiene o establece la línea de captura
        /// </summary>
        public string lineaCaptura
        {
            get;
            set;
        }

        public string cadenaveri
        {
            get;
            set;
        }


        /// <summary>
        /// Propiedad que almacena y obtiene el identificador de la jornada Notarial
        /// </summary>
        public decimal? CondonacionJornada
        {
            get;
            set;
        }

        /// <summary>
        /// Propiedad que obtiene el cliente del servicio de ISAI
        /// </summary>
        protected DeclaracionIsaiClient ClienteDeclaracionIsai
        {
            get
            {
                if (clienteDeclaracionIsai == null)
                {
                    clienteDeclaracionIsai = new DeclaracionIsaiClient();
                }
                return clienteDeclaracionIsai;
            }
        }
        private DeclaracionIsaiClient clienteDeclaracionIsai = null;


        /// <summary>
        /// Obtiene o establece el digito verificador de la cuenta predial
        /// </summary>
        private string digitoVerificador
        {
            get
            {
                if (string.IsNullOrEmpty(_digitoVerificador))
                    _digitoVerificador = DigitoVerificadorUtils.ObtenerDigitoVerificador(
                        region,manzana,lote,unidadPrivativa);
                return _digitoVerificador;

            }
        }

        /// <summary>
        /// Obtiene o establece el digito verificador de forma privada
        /// </summary>
        private string _digitoVerificador
        { get; set; }


        /// <summary>
        /// Obtiene la cuenta predial completa con digito verificador
        /// </summary>
        public string cuenta
        {
            get
            {
                return string.Format("{0}{1}{2}{3}{4}",
                    region, 
                    manzana, 
                   lote,
                    unidadPrivativa,
                    digitoVerificador);
            }
        }


        /// <summary>
        /// Obtiene la fecha de vigencia en forma de cadena de caracteres
        /// </summary>
        public string vigencia
        {
            get
            {
                return fechaVigencia.ToString("dd-MM-yyyy");
            }
        }

        /// <summary>
        /// Obtiene o establece la fecha de vigencia
        /// </summary>
        public DateTime fechaVigencia
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene el path en donde se encuentra guardado cada formato
        /// </summary>
        public string path
        {
            get
            {
                string _path = string.Empty;
                switch (TipoPago)
                {
                    case TipoPago.Vigente:
                        _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVigente;
                        break;
                    case TipoPago.Reduccion:
                        _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVigenteReduccion;
                        break;
                    case TipoPago.Vencido:
                        _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVencido;
                        break;
                    case TipoPago.VencidoReduccion:
                        _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVencidoReduccion;
                        break;
                    case global::TipoPago.isr:
                        _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVigente;
                        break;                        
                }
                return _path;
            }
        }

        /// <summary>
        /// Obtiene el nombre del formato vigente que se encuentra en el web config
        /// </summary>
        private string archivoVigente
        {
            get
            {
                string cad = ConfigurationManager.AppSettings["fmtVigente"].ToString();
                return string.IsNullOrEmpty(cad) ? string.Empty : cad;
            }
        }

        /// <summary>
        /// Obtiene el nombre del formato vencido que se encuentra en el web config
        /// </summary>
        private string archivoVencido
        {
            get
            {
                string cad = ConfigurationManager.AppSettings["fmtVencido"].ToString();
                return string.IsNullOrEmpty(cad) ? string.Empty : cad;
            }
        }

        /// <summary>
        /// Obtiene el nombre del formato vigente con beneficios que se encuentra en el web config
        /// </summary>
        private string archivoVigenteReduccion
        {
            get
            {
                string cad = ConfigurationManager.AppSettings["fmtVigenteReduccion"].ToString();
                return string.IsNullOrEmpty(cad) ? string.Empty : cad;
            }
        }

        /// <summary>
        /// Obtiene el nombre del formato vencido con beneficios que se encuentra en el web config
        /// </summary>
        private string archivoVencidoReduccion
        {
            get
            {
                string cad = ConfigurationManager.AppSettings["fmtVencidoReduccion"].ToString();
                return string.IsNullOrEmpty(cad) ? string.Empty : cad;
            }
        }

        /// <summary>
        /// Obtiene o establece el importe de exención
        /// </summary>
        public decimal? importeExencion
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el idReduccion
        /// </summary>
        public decimal? idReduccion
        {
            get;
            set;
        }
        
        /// <summary>
        /// Obtiene o establece el porcentaje de subsidio
        /// </summary>
        public decimal? porcentajeSubsidio
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el porcentaje de disminución
        /// </summary>
        public decimal? porcentajeDisminucion
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el importe de condonación
        /// </summary>
        public decimal? importeReduccion
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene los beneficios
        /// </summary>
        public decimal beneficios
        {
            get
            {
                decimal _beneficios = 0M;
                //Sólo puede tener uno de los beneficios
                if (importeExencion!=null && importeExencion !=0)
                {
                    _beneficios = (decimal)importeExencion;
                }
                //else if (idReduccion != null)
                //{
                //    _beneficios = ClienteDeclaracionIsai.ObtenerPorcentajeReduccion((decimal)idReduccion);
                //    _beneficios *=  (impuesto/(1-_beneficios)); //(impuesto / (1 + (decimal)_beneficios)) * (decimal)_beneficios;
                //}
                //else if (porcentajeSubsidio != null)
                //{
                //    _beneficios = (impuesto.ToRound2() / (1 + (decimal)porcentajeSubsidio)) * (decimal)porcentajeSubsidio;//como el impuesto total ya viene con los beneficios se aplica la formula para sacar el importe de los beneficios del impuesto ya cálculado, es decir, aplica el cálculo inverso
                //}
                //else if (porcentajeDisminucion != null)
                //{
                //    _beneficios = (impuesto.ToRound2() / (1 + (decimal)porcentajeDisminucion)) * (decimal)porcentajeDisminucion; //como el impuesto total ya viene con los beneficios se aplica la formula para sacar el importe de los beneficios del impuesto ya cálculado, es decir, aplica el cálculo inverso
                //}
                else if (importeReduccion !=null && importeReduccion !=0)
                    _beneficios = (decimal) importeReduccion;

                return _beneficios;
            }
        }

        /// <summary>
        /// Obtiene o establece el importe del recargo
        /// </summary>
        public decimal recargo
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el importe de la actualización
        /// </summary>
        public decimal actualizacion
        {
            get;
            set;
        }

        public decimal ImpCondo { get; set; }

        /// <summary>
        /// Obtiene o establece el importe (impuesto antes de recargos y actualizaciones)
        /// </summary>
        public decimal importe
        {
            get
            {
                decimal _importe = 0M;
                switch (TipoPago)
                {
                    case TipoPago.Vigente:
                        _importe = impuesto;
                        break;
                    case TipoPago.Reduccion:
                        _importe = (impuesto + beneficios);
                        break;
                    case TipoPago.Vencido:

                        _importe = impuesto - recargo - actualizacion + ImpCondo;
                        break;
                    case TipoPago.VencidoReduccion:
                        _importe = impuesto + beneficios - recargo - actualizacion + ImpCondo;
                        break;
                    case global::TipoPago.isr:
                         _importe = impuesto;
                        break;
                }
                return _importe;
            }
        }

        #region propiedades simples

        public decimal totalactojuridico { get; set; }
        public decimal valoradquisicion { get; set; }
        public decimal valorcatastral { get; set; }
        public decimal par_valoravaluo { get; set; }
        public string regla { get; set; }
        public string habitacional { get; set; }
        public decimal? codactojuridico { get; set; }
        public decimal? exencion { get; set; }
        public decimal? reduccion { get; set; }
        public decimal? subsidio { get; set; }
        public decimal? disminucion { get; set; }
        public decimal? condonacion { get; set; }
        public decimal? condonacionJornada { get; set; }
        public decimal impuesto { get; set; }
        public decimal? resultadoRecargo { get; set; }
        public decimal? resultadoActualizacion { get; set; }
        public decimal? resultadoImporteActualizacion { get; set; }
        public decimal? resultadoImporteRecargo { get; set; }
        public decimal? resultadoBaseGravable { get; set; }
        public decimal? resultadoReduccion1995 { get; set; }
        public decimal? resultadoTasa1995 { get; set; }
        public decimal? resultadoImpuesto { get; set; }
        public decimal? resultadoReduccionArt309 { get; set; }
        public decimal? resultadoExencionImporte { get; set; }
        public decimal? resultadoImporteCondonacion { get; set; }

        #endregion

        /// <summary>
        /// Obtiene o establece el codigo del tipo de declaración
        /// </summary>
        public string codTipoDeclaracion
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece la fecha de causación
        /// </summary>
        public DateTime fechaCausacion
        {
            get;
            set;

        }

        /// <summary>
        /// Obtiene o establece el valor en caso de ser tasa cero
        /// </summary>
        public string estasaCero
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el tipo de declaración
        /// </summary>
        public decimal tipodeclaracion
        {
            get;
            set;
        }

        public decimal? valorReferido { get; set; }
        public DateTime? fechaValorReferido { get; set; }
        public DateTime? fechaEscritura { get; set; }

        /// <summary>
        /// Metodo que cálcula el impuesto
        /// </summary>
        public void GetImpuesto()
        {
            decimal? parresultadoRecargo = null;
            decimal? parresultadoActualizacion = null;
            decimal? parresultadoImporteActualizacion = null;
            decimal? parresultadoImporteRecargo = null;
            decimal? parresultadoBaseGravable = null;
            decimal? parresultadoReduccion1995 = null;
            decimal? parresultadoTasa1995 = null;
            decimal? parresultadoImpuesto = null;
            decimal? parresultadoReduccionArt309 = null;
            decimal? parresultadoExencionImporte = null;
            decimal? parresultadoImporteCondonacion = null;
            decimal? _impuesto = null;
            decimal? parResultadoImporteCondonacionxFecha = null;
            //decimal decimalParse;
            DateTime fecha = new DateTime();
            try
            {
                if (codTipoDeclaracion != Constantes.PAR_VAL_TIPODECLARACION_ANTI)
                {
                        fecha = fechaCausacion;
                    
                }
                else
                {
                    fecha = DateTime.Now;
                }


                //valor referido <> null,valor del avalúo se debe coger el valor referido
                _impuesto = ClienteDeclaracionIsai.CalcularImpuestoDeclaracion(
                  tipodeclaracion,
                  totalactojuridico,
                  valoradquisicion,
                  valorcatastral,
                  par_valoravaluo,
                  habitacional,
                  estasaCero,
                  exencion,
                  idReduccion,
                  condonacionJornada,
                  subsidio,
                  disminucion,
                  condonacion,
                  fecha,
                  Convert.ToDecimal(regla),
                  valorReferido, fechaValorReferido, codactojuridico, fechaEscritura,
                  out parresultadoRecargo,
                   out parresultadoImporteRecargo,
                    out parresultadoActualizacion,
                    out parresultadoImporteActualizacion,
                    out parresultadoBaseGravable,
                    out parresultadoReduccion1995,
                    out parresultadoTasa1995,
                    out parresultadoImpuesto,
                    out parresultadoReduccionArt309,
                    out parresultadoExencionImporte,
                    out parresultadoImporteCondonacion,
                    out parResultadoImporteCondonacionxFecha);

                resultadoRecargo = parresultadoRecargo;
                resultadoImporteRecargo = parresultadoImporteRecargo;
                resultadoActualizacion = parresultadoActualizacion;
                resultadoImporteActualizacion = parresultadoImporteActualizacion;
                resultadoBaseGravable = parresultadoBaseGravable;
                resultadoReduccion1995 = parresultadoReduccion1995;
                resultadoTasa1995 = parresultadoTasa1995;
                resultadoImpuesto = parresultadoImpuesto;
                resultadoReduccionArt309 = parresultadoReduccionArt309;
                resultadoExencionImporte = parresultadoExencionImporte;
                resultadoImporteCondonacion = parresultadoImporteCondonacion;

            }
            catch (Exception e)
            {

            }
        }


        private string PosicionesImporte(decimal importeTotal)
        {
            int Longitud = 9 - importeTotal.ToString().Length;
            string Posiciones = "";
            for (int Cont = 0; Cont < Longitud; Cont++)
                Posiciones += "0";
            return Posiciones;
        }
    
    }
}