using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecurityCore;
using Oracle.DataAccess.Client;
using System.Data;
using MyExtentions;
namespace SIGAPred.FuentesExternas.Isai.Services.Negocio
{
    internal class Isr
    {
        /// <summary>
        ///[0]a - monto de la operacion
        ///[1]b - deducciones autorizadas
        ///[2]c - Ganancia obtenida
        ///[3]d - Tasa
        ///[4]e - Pago
        ///[5]f - Pago provisional conforme al artiulo 154 de la lisr
        ///[6]g - impuesto a pagar a la entidad federativa local
        ///[7]h - monto pagado
        ///[8]i - cantidad a cargo
        ///[9]j - pago en exceso
        ///[10]A - impuesto sobre la renta
        ///[11]B - parte actualizada del impuesto
        ///[12]C - recargos
        ///[13]D - multa por correcion fiscal
        ///[14]E - cantidad a pagar (A + B + C + D)
        /// </summary>
        /// <param name="idDeclaracion">El id de la declaracion</param>
        /// <param name="valores"></param>
        /// <returns>true en caso de no tener errores, false en caso contrario</returns>
        internal static bool Insert_Update(decimal idDeclaracion, List<decimal> valores)
        {
            bool ret = true;
            string mensaje = string.Empty;
            try
            {
                TransactionHelper THelper = new TransactionHelper();
                using (OracleCommand command = new OracleCommand("fexnot.fexnot_declaracion_pkg.fexnot_update_isrcomplet_p"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_iddeclaracion", OracleDbType.Int32, idDeclaracion, ParameterDirection.Input);
                    command.Parameters.Add("p_monto_operacion", OracleDbType.Int32, valores[0], ParameterDirection.Input);
                    command.Parameters.Add("p_deducciones_autorizadas", OracleDbType.Int32, valores[1], ParameterDirection.Input);
                    command.Parameters.Add("p_ganancia_obtenida", OracleDbType.Int32, valores[2], ParameterDirection.Input);
                    command.Parameters.Add("p_tasa", OracleDbType.Int32, valores[3], ParameterDirection.Input);
                    command.Parameters.Add("p_pago", OracleDbType.Int32, valores[4], ParameterDirection.Input);
                    command.Parameters.Add("p_pago_provisional", OracleDbType.Int32, valores[5], ParameterDirection.Input);
                    command.Parameters.Add("p_impuesto_a_pagar", OracleDbType.Int32, valores[6], ParameterDirection.Input);
                    command.Parameters.Add("p_monto_pagado", OracleDbType.Int32, valores[7], ParameterDirection.Input);
                    command.Parameters.Add("p_cantidad_cargo", OracleDbType.Int32, valores[8], ParameterDirection.Input);
                    command.Parameters.Add("p_pago_exceso", OracleDbType.Int32, valores[9], ParameterDirection.Input);
                    command.Parameters.Add("p_impuesto_sobre_renta", OracleDbType.Int32, valores[10], ParameterDirection.Input);
                    command.Parameters.Add("p_parte_actualziada_imp", OracleDbType.Int32, valores[11], ParameterDirection.Input);
                    command.Parameters.Add("p_recargos", OracleDbType.Int32, valores[12], ParameterDirection.Input);
                    command.Parameters.Add("p_multa", OracleDbType.Int32, valores[13], ParameterDirection.Input);
                    command.Parameters.Add("p_cantidad_pagar_abcd", OracleDbType.Int32, valores[14], ParameterDirection.Input);
                    command.Parameters.Add("p_msgerror", OracleDbType.Varchar2, 4000, null, ParameterDirection.Output);
                    THelper.EjecutaNonQuerySP(command);
                    mensaje = command.Parameters["p_msgerror"].Value.ToString();
                    ret = mensaje.ToUpper().Equals("NULL");
                    if (!ret)
                        throw new Exception(mensaje);
                }
            }
            catch (Exception e)
            {
                ret = false;
                throw e;
            }
            return ret;
        }
        /// <summary>
        ///[0]a - monto de la operacion
        ///[1]b - deducciones autorizadas
        ///[2]c - Ganancia obtenida
        ///[3]d - Tasa
        ///[4]e - Pago
        ///[5]f - Pago provisional conforme al artiulo 154 de la lisr
        ///[6]g - impuesto a pagar a la entidad federativa local
        ///[7]h - monto pagado
        ///[8]i - cantidad a cargo
        ///[9]j - pago en exceso
        ///[10]A - impuesto sobre la renta
        ///[11]B - parte actualizada del impuesto
        ///[12]C - recargos
        ///[13]D - multa por correcion fiscal
        ///[14]E - cantidad a pagar (A + B + C + D)
        /// </summary>
        /// <param name="idDeclaracion"></param>
        /// <returns></returns>
        internal static List<decimal> GetValues(decimal idDeclaracion)
        {
            List<decimal> regreso = new List<decimal>();
            try
            {
                DataSet ds = new DataSet();
                TransactionHelper THelper = new TransactionHelper();
                using (OracleCommand command = new OracleCommand("fexnot.fexnot_declaracion_pkg.FEXNOT_OBTN_ISRCOMPLET_P"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_IDDECLARACION", OracleDbType.Int32, idDeclaracion, ParameterDirection.Input);
                    command.Parameters.Add("C_DECLARACION", OracleDbType.RefCursor, ParameterDirection.Output);
                    ds = THelper.EjecutaConsultaSP(command);
                    if(ds.Tables.Count>0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            regreso.Add(dr.ToDecimalValue("MONTO_OPERACION"));
                            regreso.Add(dr.ToDecimalValue("DEDUCCIONES_AUTORIZADAS"));
                            regreso.Add(dr.ToDecimalValue("GANANCIA_OBTENIDA"));
                            regreso.Add(dr.ToDecimalValue("TASA"));
                            regreso.Add(dr.ToDecimalValue("PAGO"));
                            regreso.Add(dr.ToDecimalValue("PAGO_PROVISIONAL"));
                            regreso.Add(dr.ToDecimalValue("IMPUESTO_A_PAGAR"));
                            regreso.Add(dr.ToDecimalValue("MONTO_PAGADO"));
                            regreso.Add(dr.ToDecimalValue("CANTIDAD_CARGO"));
                            regreso.Add(dr.ToDecimalValue("PAGO_EXCESO"));
                            regreso.Add(dr.ToDecimalValue("IMPUESTO_SOBRE_RENTA"));
                            regreso.Add(dr.ToDecimalValue("PARTE_ACTUALZIADA_IMP"));
                            regreso.Add(dr.ToDecimalValue("RECARGOS"));
                            regreso.Add(dr.ToDecimalValue("MULTA"));
                            regreso.Add(dr.ToDecimalValue("CANTIDAD_PAGAR_ABCD"));
                        }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return regreso;

        }

    }
}
