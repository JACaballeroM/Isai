using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace SIGAPred.FuentesExternas.Isai.Services.Negocio
{    
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionPolicyWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void HandleException(Exception ex)
        {
            string exceptionPolicy = System.Configuration.ConfigurationManager.AppSettings["SIGAPred.FuentesExternas.Isai.ExceptionPolicy"];
            ExceptionPolicy.HandleException(ex, exceptionPolicy);
        }
    }
}
