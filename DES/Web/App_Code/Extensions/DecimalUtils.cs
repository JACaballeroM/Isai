using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGAPred.Common.Extensions;

/// <summary>
/// Descripción breve de DecimalUtils
/// </summary>
/// 
namespace SIGAPred.Common.Extensions
{
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
            catch 
            {

                return numero==0? string.Format("00.00"):numero.ToString();
            }
        }
    }

    public class ImpuestoException:Exception
    {
        public ImpuestoException()
            : base()
        { 
        
        }

        public ImpuestoException(string message)
            : base(message)
        {

        }
    }
}