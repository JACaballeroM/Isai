using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MyExtentions;
namespace SIGAPred.FuentesExternas.Isai.Services.Negocio
{
    /// <summary>
    /// Clase para los datos identificativos del webService
    /// </summary>
    internal class DatosWebService
    {
        /// <summary>
        /// Almacena el usuario del web service
        /// </summary>
        public string usuario { get; set; }

        /// <summary>
        /// Almacena el password para conectarse al sevicio de la LC
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Almacena el usuario que solicita la LC
        /// </summary>
        public string usuarioSolicita { get; set; }

        /// <summary>
        /// Almacena la funcion de cobro
        /// </summary>
        public string funcionDeCobro { get; set; }

        /// <summary>
        /// Almacena la clave del impuesto
        /// </summary>
        public int claveImpuesto { get; set; }

        /// <summary>
        /// Almacena la referencia
        /// </summary>
        public string idPago { get; set; }


       /// <summary>
       /// Constructor del objeto, recibe una cadena (ISR o ISAI) para construirlo con la informacion guardada en base de datos
       /// </summary>
       /// <param name="motivo">isr o isai dependiendo de lo que se quiera consultar</param>
        public DatosWebService(string motivo)
        {
            try
            {
                DataSet ds = new DataSet();
                SecurityCore.TransactionHelper tranHelper = new SecurityCore.TransactionHelper();
                OracleCommand comando = new OracleCommand("FEXNOT.fexnot_catcatalogos_pkg.fexnot_obter_catfuncioncobro");
                using (comando)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add("P_DESCRIPCIONCOBRO", OracleDbType.Varchar2).Value = motivo;
                    comando.Parameters.Add("P_CONSULTA", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    ds = tranHelper.EjecutaConsultaSP(comando);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                this.usuario = dr.ToStringValue("USUARIOOVICALC");
                                this.password = dr.ToStringValue("PASSWORDOVICALC");
                                this.usuarioSolicita = dr.ToStringValue("LINEACAPTURAUSUARIOGEN");
                                this.funcionDeCobro = dr.ToStringValue("FUNCION_COBRO");
                                this.claveImpuesto = dr.toIntValue( "CLAVE_IMPUESTO");
                                this.idPago = dr.ToStringValue("REFERENCIA");
                            }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
