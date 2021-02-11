using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGAPred.Common.Extensions;


namespace SIGAPred.Common.Extensions
{
    /// <summary>
    /// Descripción breve de DecimalUtils
    /// </summary>
    public static partial class DecimalUtils
    {
        /// <summary>
        /// Redondea el valor con el criterio si valor >.51 aumenta al siguiente valor, de lo contrario disminuye
        /// </summary>
        /// <param name="impTotal">Representa valor a redondear</param>
        /// <returns>El valor redondeado</returns>
        public static decimal ToRoundTwo(this decimal impTotal)
        {
            // Nos quedamos con los dos primeros dígitos de la parte decimal.
            string auxImpDec = impTotal.ToString("F2");
            Int32 auxParteDecimal = Convert.ToInt32(auxImpDec.Substring(auxImpDec.IndexOf(".") + 1, 2));

            Decimal auxImporte = 0;

            // Si la parte decimal en menor de 50 
            if (auxParteDecimal <= 50)
            {
                auxImporte = Decimal.Truncate(impTotal);
            }
            // Si la parte decimal es mayor o igual que 51
            else
            {
                auxImporte = Decimal.Truncate(impTotal) + 1;
            }
            return auxImporte;
        }

        public static decimal ToRoundTwoDecimals(this decimal impTotal)
        {

            decimal ret = 0M;
            if (impTotal.ToString().Contains("."))
            {
                string[] arr = impTotal.ToString().Split('.');
                if (arr[1].Length >= 4)
                {
                    if (Convert.ToDecimal(arr[1].Substring(2, 2)) <= 50)
                        arr[1] = arr[1].Substring(0, 2);
                    else
                        arr[1] = (Convert.ToDecimal(arr[1].Substring(0, 2)) + 1).ToString();

                    ret = Convert.ToDecimal(string.Format("{0}.{1}", arr[0], arr[1]));
                }
            }
            else
            {
                ret = impTotal;
            }
            // Nos quedamos con los dos primeros dígitos de la parte decimal.
            return ret;
        }

        public static string  ToRoundTwoDecimalsString(this decimal impTotal)
        {

            string ret = "0.00";
            if (impTotal.ToString().Contains("."))
            {
                string[] arr = impTotal.ToString().Split('.');
                arr[1] = arr[1].PadRight(4, '0');
                arr[1] = arr[1].Substring(0, 2);
                ret = string.Format("{0}.{1}", arr[0], arr[1]);
               
            }
            else
            {
                ret = string.Format("{0}.00", impTotal);
            }
            // Nos quedamos con los dos primeros dígitos de la parte decimal.
            return ret;
        }

        public static decimal Truncate2(this decimal value)
        {
            int length = 2;
            string[] param = value.ToString().Split('.');

            if (param.Length > 1)
            {
                if (param[1].Length >= length)
                    return Convert.ToDecimal(param[0] + "." + param[1].Substring(0, length));
                else
                    return Convert.ToDecimal(param[0] + "." + param[1].Substring(0, param[1].Length));
            }
            else
                return value;
        }

        public static string toDecimalString(this decimal numero)
        {
            try
            {
                return (numero % Math.Truncate(numero)) != 0 ? numero.ToString() : string.Format("{0}.00", numero);
            }
            catch (Exception e)
            {

                return numero==0? string.Format("00.00"):numero.ToString();
            }
        }
    }
}