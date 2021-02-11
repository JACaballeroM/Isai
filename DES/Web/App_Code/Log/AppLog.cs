using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AppLog
/// </summary>
public static class AppLog
{
    private static System.Diagnostics.EventLog log = null;

    public static void LogEventLog(string message)
    {
        try
        {
            if (log == null)
                log = new System.Diagnostics.EventLog();
            log.Source = "Aplicativo Condominios";
            log.WriteEntry(message);
        }
        catch (Exception exeption)
        {
            Log(exeption);
        }
    }

    public static void LogEventLog(Exception exeptionLog)
    {
        try
        {
            if (log == null)
                log = new System.Diagnostics.EventLog();
            log.Source = "Aplicativo Condominios";
            log.WriteEntry(exeptionLog.StackTrace);
        }
        catch (Exception exeption)
        {
            Log(exeption);
        }
    }

    public static void Log(string message)
    {
        ExceptionPolicyWrapper.HandleException(new Exception(message));
    }


    public static void Log(Exception exeption)
    {
        ExceptionPolicyWrapper.HandleException(exeption);
    }

}