using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

/// <summary>
/// Summary description for ExceptionPolicyWrapper
/// </summary>
public class ExceptionPolicyWrapper
{
    public static void HandleException(Exception ex)
    {
        string exceptionPolicy = System.Configuration.ConfigurationManager.AppSettings["SIGAPred.FuentesExternas.Isai.ExceptionPolicy"];
        ExceptionPolicy.HandleException(ex, exceptionPolicy);
    }
}
