using System;
using ServiceAvaluos;
using ServiceDeclaracionIsai;
//using SIGAPred.Common.BL;
using ServiceRCON;
using System.Data;

/// <summary>
/// Clase encargada de gestionar los métodos para la consulta y configuración de los gridview
/// de las bandejas de entrada
/// </summary>
public class DseDeclaracionIsaiPagger : PaggerBase
{
    private AvaluosClient clienteAvaluo = null;

    protected AvaluosClient ClienteAvaluo
    {
        get
        {
            if (clienteAvaluo == null)
            {
                clienteAvaluo = new AvaluosClient();
            }
            return clienteAvaluo;
        }
    }

    private DeclaracionIsaiClient clienteIsai = null;

    protected DeclaracionIsaiClient ClienteIsai
    {
        get
        {
            if (clienteIsai == null)
            {
                clienteIsai = new DeclaracionIsaiClient();
            }
            return clienteIsai;
        }
    }
    #region Funciones número total de filas

    /// <summary>
    /// Función que obtiene el número de registros totales de avalúos consultado por un notario para una fecha
    /// </summary>
    /// <param name="fechaInicio">Fechal del avalúo inicial </param>
    /// <param name="fechaFin">Fecha final del avalúo para realizar la consulta</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="numValuo">Número del avalúo</param>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número de registros</returns>
    public int NumTotalFilasFechaNotario(string fechaInicio, string fechaFin, int idNotario, string registro, string vigente, string numValuo, string idAvaluo, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de avalúos consultado por un notario para una cuenta catastral
    /// </summary>
    /// <param name="cuentaCatastral">Cuenta catastral para realizar la búsqueda</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros</returns>
    public int NumTotalFilasCuentaCatastralNotario(string cuentaCatastral, int idNotario, string registro, string vigente, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de avalúos consultado desde la secretaria de finanzas para una fecha
    /// </summary>
    /// <param name="fechaInicio">Fechal del avalúo inicial </param>
    /// <param name="fechaFin">Fecha final del avalúo para realizar la consulta</param>
    /// <param name="idNotario">Identificador de un notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia requerida del avalúo</param>
    /// <param name="numValuo">Número del avalúo</param>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalFilasFechaNotarioSF(string fechaInicio, string fechaFin, int idNotario, string registro, string vigente, string numValuo, string idAvaluo, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de avalúos consultado desde la secretaria de finanzas para una cuenta catastral
    /// </summary>
    /// <param name="cuentaCatastral">Cuenta catastral</param>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalFilasCuentaCatastralNotarioSF(string cuentaCatastral, int idNotario, string registro, string vigente, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones consultado desde la secretaria de finanzas para una fecha y un estado
    /// </summary>
    /// <param name="fechaInicio">Fecha inicial de presentación de declaración</param>
    /// <param name="fechaFin">Fecha final de fecha presentación</param>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="codEstado">Código del estado de la declaración</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalFilasFechaSF(DateTime fechaInicio, DateTime fechaFin, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones consultado desde la secretaría de finanzas para una cuenta catastral y un estado
    /// </summary>
    /// <param name="region">Región de la cuenta</param>
    /// <param name="manzana">Manzana de la cuenta</param>
    /// <param name="lote">Lote de la cuenta</param>
    /// <param name="unidad">Unidad de la cuenta</param>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="codEstado">Código del estado de la declaración</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalObtenerDeclaracionesPorCuentaSF(string region, string manzana, string lote, string unidad, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }


    public int NumTotalObtenerDeclaracionesPorSujetoSF(int idPerNot, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones consultado desde la secretaría de finanzas para un número de presentación y un estado
    /// </summary>
    /// <param name="numPres">Número de presentación de la declaración</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="codEstado">Código de estado de la declaración</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalObtenerDeclaracionesPorNumPresSF(string numPres, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones consultado por un notario para un identificador de avalúo y un estado
    /// </summary>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalObtenerDeclaracionesDeAvaluo(int idAvaluo, int idNotario, int pageSize, int indice, string SortExpression2)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones consultado desde la secretaría de finanzas para un identificador de avalúo
    /// </summary>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalObtenerDeclaracionesDeAvaluoSF(int idAvaluo, int idNotario, int pageSize, int indice, string SortExpression2)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de avalúos consultado por un notario para una cuenta catastral
    /// </summary>
    /// <param name="cuentaCatastral">Cuenta catastral</param>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumtTotalObtenerAvaluosPorCuentaObtnNumNotario(string cuentaCatastral, int idNotario, string registro, string vigente, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones de jornada notarial para un rango de fechas
    /// </summary>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="fechaInicio">Fecha inicio de presentación de la declaraciones</param>
    /// <param name="fechaFin">Fecha fin</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumtTotalObtenerDecPorFechaJornada(int idNotario, string fechaInicio, string fechaFin, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones de jornada notarial para una cuenta catastral
    /// </summary>
    /// <param name="idnotario">Identificador de notario</param>
    /// <param name="cuentaCatastral">Cuenta catastral</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumtTotalObtenerDecPorCCJornada(int idnotario, string cuentaCatastral, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Función que obtiene el número de registros totales de declaraciones de jornada notarial para una sujeto
    /// </summary>
    /// <param name="idnotario">Identificador de notario</param>
    /// <param name="nombre">Nombre del sujeto</param>
    /// <param name="apellidoPaterno">Apellido paterno del sujeto</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumtTotalObtenerDecPorNomJornada(int idnotario, string nombre, string apellidoPaterno, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }
    #endregion

    #region Funciones consultas


    /// <summary>
    /// Función que obtiene los registros de declaraciones de jornada notarial para un rango de fechas
    /// </summary>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="fechaInicio">Fecha inicio de presentación de la declaraciones</param>
    /// <param name="fechaFin">Fecha fin</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDecPorFechaJornada(int idNotario, string fechaInicio, string fechaFin, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);


        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMPRESENTACION ASC";
        }
        DateTime fechaI;
        DateTime fechaF;
        if (!string.IsNullOrEmpty(fechaInicio))
        {
            fechaI = DateTime.Parse(fechaInicio);
        }
        else
            return new DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable();
        if (!string.IsNullOrEmpty(fechaFin))
        {
            fechaF = DateTime.Parse(fechaFin);
        }
        else
            return new DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable();

        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ds = ClienteIsai.ObtenerDeclaracionesPorFechaPresentacionTipoJornadaNotarial(idNotario, fechaI, fechaF, base.pageSize, base.indice, ref base.totalPages, SortExpression);

        return ds;

    }


    /// <summary>
    /// Función que obtiene los registros de declaraciones de jornada notarial para una cuenta catastral
    /// </summary>
    /// <param name="idnotario">Identificador de notario</param>
    /// <param name="cuentaCatastral">Cuenta catastral</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDecPorCCJornada(int idnotario, string cuentaCatastral, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);


        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMPRESENTACION ASC";
        }

        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ds = ClienteIsai.ObtenerDeclaracionesPorCuentaCatastralTipoJornadaNotarial(idnotario, cuentaCatastral.CuentaCatastralToCodes(), base.pageSize, base.indice, ref base.totalPages, SortExpression);

        return ds;

    }


    /// <summary>
    /// Función que obtiene los registros de declaraciones de jornada notarial para una sujeto
    /// </summary>
    /// <param name="idnotario">Identificador de notario</param>
    /// <param name="nombre">Nombre del sujeto</param>
    /// <param name="apellidoPaterno">Apellido paterno del sujeto</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDecPorNomJornada(int idnotario, string nombre, string apellidoPaterno, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);


        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMPRESENTACION ASC";
        }

        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ds = ClienteIsai.ObtenerDeclaracionesPorNombApellPaternoTipoJornadaNotarial(idnotario, nombre, apellidoPaterno, base.pageSize, base.indice, ref base.totalPages, SortExpression);

        return ds;

    }


    /// <summary>
    /// Función que obtiene los registros de avalúos consultado por un notario para una cuenta catastral
    /// </summary>
    /// <param name="cuentaCatastral">Cuenta catastral para realizar la búsqueda</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros</returns>
    public DseAvaluoConsulta ObtenerAvaluosPorCuentaObtnNumNotario(string cuentaCatastral, int idNotario, string registro, string vigente, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);

        if (string.IsNullOrEmpty(registro))
        {
            registro = string.Empty;
        }

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMEROAVALUO DESC";
        }
        DseAvaluoConsulta ds = new DseAvaluoConsulta();
        //process account -> remove last '-'
        if (!string.IsNullOrEmpty(cuentaCatastral))
        {
            while (cuentaCatastral.EndsWith("-"))
                cuentaCatastral = cuentaCatastral.Substring(0, cuentaCatastral.Length - 1);


            ds = ClienteAvaluo.ObtenerAvaluosPorCuentaObtnNum(cuentaCatastral, idNotario, registro, vigente, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        }

        return ds;

    }


    /// <summary>
    /// Función que obtiene los registros de avalúos consultados por un notario para una cuenta catastral
    /// </summary>
    /// <param name="cuentaCatastral">Cuenta catastral para realizar la búsqueda</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseAvaluoConsulta</returns>
    public DseAvaluoConsulta ObtenerAvaluosPorCuentaCatastralNotario(string cuentaCatastral, int idNotario, string registro, string vigente, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);

        if (string.IsNullOrEmpty(registro))
        {
            registro = string.Empty;
        }

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMEROAVALUO DESC";
        }

        //process account -> remove last '-'
        if (!string.IsNullOrEmpty(cuentaCatastral))
            while (cuentaCatastral.EndsWith("-"))
                cuentaCatastral = cuentaCatastral.Substring(0, cuentaCatastral.Length - 1);


        DseAvaluoConsulta ds = ClienteAvaluo.ObtenerAvaluosPorCuentaCatastralNotario(cuentaCatastral, idNotario, registro, vigente, base.pageSize, base.indice, ref base.totalPages, SortExpression);

        return ds;

    }

    /// <summary>
    /// Función que los registros de avalúos consultado desde la secretaria de finanzas para una cuenta catastral
    /// </summary>
    /// <param name="cuentaCatastral">Cuenta catastral</param>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseAvaluoConsulta</returns>
    public DseAvaluoConsulta ObtenerAvaluosPorCuentaCatastralNotarioSF(string cuentaCatastral, int idNotario, string registro, string vigente, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);
        if (string.IsNullOrEmpty(registro))
        {
            registro = string.Empty;
        }
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMEROAVALUO DESC";
        }
        DseAvaluoConsulta ds = ClienteAvaluo.ObtenerAvaluosPorCuentaCatastralNotarioSF(cuentaCatastral, idNotario, registro, vigente, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        return ds;
    }

    /// <summary>
    /// Función que obtiene los registros de avalúos consultado por un notario para una fecha
    /// </summary>
    /// <param name="fechaInicio">Fechal del avalúo inicial </param>
    /// <param name="fechaFin">Fecha final del avalúo para realizar la consulta</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia del avalúo</param>
    /// <param name="numValuo">Número del avalúo</param>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseAvaluoConsulta</returns>
    public DseAvaluoConsulta ObtenerAvaluosPorFechaNotario(string fechaInicio, string fechaFin, int idNotario, string registro, string vigente, string numValuo, string idAvaluo, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMEROAVALUO DESC";
        }
        DateTime? fechaI = (DateTime?)null;
        DateTime? fechaF = (DateTime?)null;
        if (!string.IsNullOrEmpty(fechaInicio))
        {
            fechaI = DateTime.Parse(fechaInicio);
        }
        if (!string.IsNullOrEmpty(fechaFin))
        {
            fechaF = DateTime.Parse(fechaFin);
        }
        DseAvaluoConsulta ds = ClienteAvaluo.ObtenerAvaluosPorFechaNotario(fechaI, fechaF, idNotario, registro, vigente, numValuo, idAvaluo, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        return ds;
    }


    /// <summary>
    /// Función que obtiene los registros de avalúos consultados desde la secretaria de finanzas para una fecha
    /// </summary>
    /// <param name="fechaInicio">Fechal del avalúo inicial </param>
    /// <param name="fechaFin">Fecha final del avalúo para realizar la consulta</param>
    /// <param name="idNotario">Identificador de un notario</param>
    /// <param name="registro"></param>
    /// <param name="vigente">Vigencia requerida del avalúo</param>
    /// <param name="numValuo">Número del avalúo</param>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseAvaluoConsulta</returns>
    public DseAvaluoConsulta ObtenerAvaluosPorFechaNotarioSF(string fechaInicio, string fechaFin, int idNotario, string registro, string vigente, string numValuo, string idAvaluo, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);

        if (string.IsNullOrEmpty(registro))
        {
            registro = string.Empty;
        }
        if (string.IsNullOrEmpty(idAvaluo))
        {
            idAvaluo = string.Empty;
        }
        if (string.IsNullOrEmpty(numValuo))
        {
            numValuo = string.Empty;
        }
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMEROAVALUO DESC";
        }
        registro = (registro.Trim() == "" || registro.Trim().ToLower() == "null" ? null : registro);
        idAvaluo = (idAvaluo.Trim() == "" || idAvaluo.Trim().ToLower() == "null" ? null : idAvaluo);
        numValuo = (numValuo.Trim() == "" || numValuo.Trim().ToLower() == "null" ? null : numValuo);
        DateTime? fechaI = (DateTime?)null;
        DateTime? fechaF = (DateTime?)null;
        if (!string.IsNullOrEmpty(fechaInicio))
        {
            fechaI = DateTime.Parse(fechaInicio);
        }
        if (!string.IsNullOrEmpty(fechaFin))
        {
            fechaF = DateTime.Parse(fechaFin);
        }
        DseAvaluoConsulta ds;
        ds = ClienteAvaluo.ObtenerAvaluosPorFechaNotarioSF(fechaI, fechaF, idNotario, registro, vigente, numValuo, idAvaluo, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        return ds;
    }


    /// <summary>
    /// Función que obtiene los registros de declaraciones consultados desde la secretaria de finanzas para una fecha y un estado
    /// </summary>
    /// <param name="fechaInicio">Fecha inicial de presentación de declaración</param>
    /// <param name="fechaFin">Fecha final de fecha presentación</param>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="codEstado">Código del estado de la declaración</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorFechaSF(DateTime fechaInicio, DateTime fechaFin, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "IDDECLARACION ASC";
        }
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dt;
        dt = ClienteIsai.ObtenerDeclaracionesPorFechaPresentacionCodEstadoDeclaracion(fechaInicio, fechaFin, codEstado, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        GuardarNotario(ref dt);
        return dt;
    }


    /// <summary>
    /// Función que obtiene los registros de declaraciones consultados desde la secretaría de finanzas para una cuenta catastral y un estado
    /// </summary>
    /// <param name="region">Región de la cuenta</param>
    /// <param name="manzana">Manzana de la cuenta</param>
    /// <param name="lote">Lote de la cuenta</param>
    /// <param name="unidad">Unidad de la cuenta</param>
    /// <param name="idNotario">Identificador de notario</param>
    /// <param name="codEstado">Código del estado de la declaración</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaSF(string region, string manzana, string lote, string unidad, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "IDDECLARACION ASC";
        }
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dt;
        dt = ClienteIsai.ObtenerDeclaracionesPorCuentaCatastralCodEstadoDeclaracionSF(idNotario, region, manzana, lote, unidad, codEstado, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        GuardarNotario(ref dt);
        return dt;
    }

    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorSujetoSF(int idPerNot, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "IDDECLARACION ASC";
        }

        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dt;
        dt = ClienteIsai.ObtenerDeclaracionesPorNombApellPaternoCodEstadoDeclaracion(idNotario, idPerNot, codEstado, base.pageSize, base.indice, ref base.totalPages, SortExpression);

        if (dt.Count > 0)
            totalPages = Convert.ToInt32(dt[0].ROWS_TOTAL);
        GuardarNotario(ref dt);
        return dt;
    }



    /// <summary>
    /// Función que obtiene los registros de declaraciones consultados desde la secretaría de finanzas para un número de presentación y un estado
    /// </summary>
    /// <param name="numPres">Número de presentación de la declaración</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="codEstado">Código de estado de la declaración</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNumPresSF(string numPres, int idNotario, int codEstado, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "IDDECLARACION ASC";
        }

        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dt;
        dt = ClienteDeclaracionIsai.ObtenerDeclaracionesPorNumPresentacionCodEstadoDeclaracion(idNotario, numPres, codEstado, base.pageSize, base.indice, ref base.totalPages, SortExpression);

        if (dt.Count > 0)
            totalPages = Convert.ToInt32(dt[0].ROWS_TOTAL);
        GuardarNotario(ref dt);
        return dt;
    }



    /// <summary>
    /// Función que obtiene los registros de declaraciones consultados desde la secretaría de finanzas para un identificador de avalúo
    /// </summary>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesDeAvaluoSF(int idAvaluo, int idNotario, int pageSize, int indice, string SortExpression2)
    {
        SavePaggerSettings(indice, pageSize, SortExpression2);
        if (string.IsNullOrEmpty(SortExpression2))
        {
            SortExpression2 = "IDDECLARACION ASC";
        }
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dt;
        dt = ClienteIsai.ObtenerDeclaracionesDeAvaluoSF(idAvaluo, idNotario, base.pageSize, base.indice, ref base.totalPages, SortExpression2);
        GuardarNotario(ref dt);

        return dt;
    }


    /// <summary>
    /// Función que obtiene los registros de declaraciones consultados por un notario para un identificador de avalúo y un estado
    /// </summary>
    /// <param name="idAvaluo">Identificador del avalúo</param>
    /// <param name="idNotario">Identificador del notario</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable</returns>
    public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesDeAvaluo(int idAvaluo, int idNotario, int pageSize, int indice, string SortExpression2)
    {
        
        SavePaggerSettings(indice, pageSize, SortExpression2);
        if (string.IsNullOrEmpty(SortExpression2))
        {
            SortExpression2 = "IDDECLARACION ASC";
        }
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dt;

        dt = ClienteDeclaracionIsai.ObtenerDeclaracionesDeAvaluo(idAvaluo, idNotario, base.pageSize, base.indice, ref base.totalPages, SortExpression2);
        return dt;
    }

    //metodo temporal de ordenacion
    private DseAvaluoConsulta PerformSort(string SortExpression, DseAvaluoConsulta ds)
    {
        //lo siguiente habria q realizarlo en el store procedure
        //perform sort
        string[] sorts = SortExpression.Split(' ');
        string sortExp = sorts[0].Trim();
        string sortDir = (sorts.Length > 1) ? sorts[1].Trim() : "ASC";

        DataView dv = new DataView(ds.FEXAVA_AVALUO_V);
        dv.Sort = string.Format("{0} {1}", sortExp, sortDir);

        DseAvaluoConsulta dsNew = new DseAvaluoConsulta();

        //clone DataSet y crea los renglones "as sorted"
        foreach (DataRowView row in dv)
        {
            DseAvaluoConsulta.FEXAVA_AVALUO_VRow nRow = dsNew.FEXAVA_AVALUO_V.NewFEXAVA_AVALUO_VRow();
            nRow.ItemArray = ((DseAvaluoConsulta.FEXAVA_AVALUO_VRow)row.Row).ItemArray;
            dsNew.FEXAVA_AVALUO_V.AddFEXAVA_AVALUO_VRow(nRow);
        }
        //devuelve nuevo dataset ordenado
        return dsNew;
    }

    /// <summary>
    /// Guardamos el declarante en la declaracion
    /// </summary>
    /// <param name="rowP"></param>
    private void GuardarNotario(ref DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable dtDeclaracionIsai)
    {
        try
        {
            DseInfoContribuyente dseInfo;
            DseInfoNotarios dseNot;
            if (dtDeclaracionIsai.Rows.Count > 0)
            {
                foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow fila in dtDeclaracionIsai)
                {
                    using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
                    {
                        dseInfo = clienteRCON.GetInfoContribuyente(fila.IDPERSONA);
                        dseNot = clienteRCON.GetInfoNotario(fila.IDPERSONA, false);

                    }
                    if (dseInfo.Contribuyente.Rows.Count > 0)
                    {
                        fila.SUJETO = dseInfo.Contribuyente[0].APELLIDOPATERNO +" "+ dseInfo.Contribuyente[0].APELLIDOMATERNO + ", " + dseInfo.Contribuyente[0].NOMBRE;
                    }
                    if (dseNot.Notario.Rows.Count > 0)
                    {
                         //Número de notario
                        fila.IDPERSONADECLARANTE = dseNot.Notario[0].NUMNOTARIO.ToString();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }




    /// <summary>
    /// Número total de filas según notarios
    /// </summary>
    /// <param name="numero">número</param>
    /// <param name="nombre">nombre</param>
    /// <param name="apellidoPaterno">apellido paterno</param>
    /// <param name="apellidoMaterno">apellido materno</param>
    /// <param name="rfc">RFC</param>
    /// <param name="curp">CURP</param>
    /// <param name="ife">IFE</param>
    /// <param name="pageSize">tamaño página</param>
    /// <param name="indice">índice</param>
    /// <param name="SortExpression">criterio de ordenación</param>
    /// <returns>entero que indica el número de filas devueltas</returns>
    public int NumTotalFilasNotarios(decimal? numero, string nombre, string apellidoPaterno, string apellidoMaterno, string rfc, string curp, string ife, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }

    /// <summary>
    /// Tabla con los notarios obtenidos según los criterios de búsqueda
    /// </summary>
    /// <param name="numero">número</param>
    /// <param name="nombre">nombre</param>
    /// <param name="apellidoPaterno">apellido paterno</param>
    /// <param name="apellidoMaterno">apellido materno</param>
    /// <param name="rfc">RFC</param>
    /// <param name="curp">CURP</param>
    /// <param name="ife">IFE</param>
    /// <param name="pageSize">tamaño página</param>
    /// <param name="indice">índice</param>
    /// <param name="SortExpression">criterio de ordenación</param>
    /// <returns>tabla con las filas que cumplen los criterios de búsqueda</returns>
    public ServiceAvaluos.DseNotarios.FEXAVA_NOTARIOSDataTable ObtenerNotariosPorBusqueda(decimal? numero, string nombre, string apellidoPaterno, string apellidoMaterno, string rfc, string curp, string ife, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "NUMERO DESC";
        }
        if (string.IsNullOrEmpty(nombre))
        {
            nombre = string.Empty;
        }
        if (string.IsNullOrEmpty(apellidoPaterno))
        {
            apellidoPaterno = string.Empty;
        }
        if (string.IsNullOrEmpty(apellidoMaterno))
        {
            apellidoMaterno = string.Empty;
        }
        if (string.IsNullOrEmpty(rfc))
        {
            rfc = string.Empty;
        }
        if (string.IsNullOrEmpty(curp))
        {
            curp = string.Empty;
        }
        if (string.IsNullOrEmpty(ife))
        {
            ife = string.Empty;
        }

        ServiceAvaluos.DseNotarios dse = null;
        dse = ClienteAvaluo.BusquedaNotarios(numero, nombre, apellidoPaterno, apellidoMaterno, rfc, curp, ife, base.pageSize, base.indice, ref base.totalPages, SortExpression);

        foreach (ServiceAvaluos.DseNotarios.FEXAVA_NOTARIOSRow rowPersonas in dse.FEXAVA_NOTARIOS.Rows)
        {
            if (!rowPersonas.IsNOMBREAPELLIDOSNull())
            {
                if (rowPersonas.NOMBREAPELLIDOS.Trim().Length > 100)
                {
                    rowPersonas.NOMBREAPELLIDOS = string.Concat(rowPersonas.NOMBREAPELLIDOS.Trim().Substring(0, 100), "...");
                }
            }
            if (!rowPersonas.IsRFCNull())
            {
                if (rowPersonas.RFC.Trim().Length > 8)
                {
                    rowPersonas.RFC = string.Concat(rowPersonas.RFC.Trim().Substring(0, 8), "...");
                }
            }
            if (!rowPersonas.IsCURPNull())
            {
                if (rowPersonas.CURP.Trim().Length > 8)
                {
                    rowPersonas.CURP = string.Concat(rowPersonas.CURP.Trim().Substring(0, 8), "...");
                }
            }
            if (!rowPersonas.IsCLAVEIFENull())
            {
                if (rowPersonas.CLAVEIFE.Trim().Length > 8)
                {
                    rowPersonas.CLAVEIFE = string.Concat(rowPersonas.CLAVEIFE.Trim().Substring(0, 8), "...");
                }
            }
        }

        return dse.FEXAVA_NOTARIOS;
    }
    #endregion
}




