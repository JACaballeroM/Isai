using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos;
using SIGAPred.Common.DataAccess.OracleDataAccess;
using System.Collections.Generic;


namespace SIGAPred.FuentesExternas.Isai.Services.Negocio.Interfaces
{
    /// <summary>
    /// Proporciona la interfaz para acceder el servicio web de ISAI
    /// </summary>
    [ServiceContract]
    public interface IDeclaracionIsai
    {
        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        string Test(string cad);



        #region Métodos eliminar
        /// <summary>
        /// Elimina de la base de datos la declaración con el indentificador especificado
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración que se va a eliminar</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void EliminarDeclaracion(int idDeclaracion);


        /// <summary>
        /// Método para eliminar una condonación de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración de la cual se va a eliminar la condonación</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void EliminarCondonacion(int idDeclaracion);

        /// <summary>
        ///  Elimina una declaración (Únicamente si esta en estado borrador)
        /// </summary>
        /// <param name="declaracion">Data Table Declaración con la declaración a eliminar en la primera posición</param>
        [OperationContract(Name = "EliminarDeclaracionObject")]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void EliminarDeclaracion(DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracion);

        /// <summary>
        /// Eliminar una participante de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="idPersona">Identificador de la persona</param>
        /// <param name="codTipoPersona">Tipo de persona</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void EliminarParticipanteDeclaracion(int idDeclaracion, int idPersona, string codTipoPersona);

        #endregion

        #region Métodos insertar

        /// <summary>
        /// Inserta un participante en la declaración
        /// </summary>
        /// <param name="participante">DataTable con una única fila de declaración</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable InsertarParticipanteDeclaracion(DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participante);


        /// <summary>
        /// Registra la declaración en la Base de Datos
        /// </summary>
        /// <param name="dseDeclaracionIsai">DataSet declaración ISAI</param>
        /// <param name="xmlDoc">Cadena de texto con el xml de documento</param>
        /// <param name="tipoDocumentoDigital">Cadena de texto con el tipo de documento</param>
        /// <param name="listaFicheros">Cadena de texto con la lista de ficheros</param>
        /// <param name="xmlDocBF">Cadena de texto con el xml del documento de beneficios fiscales</param>
        /// <param name="tipoDocumentoDigitalBF">Cadena de texto con el tipo de documentos de beneficios fiscales</param>
        /// <param name="listaFicherosBF">Cadena de texto con la lista de ficheros</param>
        /// <param name="ImpuestoAnteriorPagado">Impuesto pagado</param>
        /// <param name="valorReferido">Valor referido</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        string RegistrarDeclaracion(ref DseDeclaracionIsai dseDeclaracionIsai, string xmlDoc, string tipoDocumentoDigital, string listaFicheros, string xmlDocBF, string tipoDocumentoDigitalBF, string listaFicherosBF, decimal? ImpuestoAnteriorPagado, decimal? valorReferido, DateTime? fechaValorReferido);

        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        string RegistrarDeclaracion2(ref DseDeclaracionIsai dseDeclaracionIsai, string xmlDoc, string tipoDocumentoDigital, string listaFicheros, string xmlDocBF, string tipoDocumentoDigitalBF, string listaFicherosBF, decimal? ImpuestoAnteriorPagado, decimal? valorReferido, DateTime? fechaValorReferido, decimal[] valoresIsr, decimal? usuario);


        /// <summary>
        /// Registramos una declaracion con el campo EnPapel a 'S' del cual mas adelante podra ser 
        /// modificada para ser guarda en el sistema.
        /// </summary>
        /// <param name="declaracionDT"></param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void RegistrarDeclaracionEnPapelNormalAnticipada(ref DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT);


        /// <summary>
        /// Registramos una declaracion con el campo EnPapel a 'S' del cual mas adelante podra ser 
        /// modificada para ser guarda en el sistema.
        /// </summary>
        /// <param name="dseDeclaracionIsai"></param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void RegistrarDeclaracionEnPapelComplementaria(ref DseDeclaracionIsai dseDeclaracionIsai);

        #endregion

        #region Métodos obtener
        /// <summary>
        /// Devolvemos el porcentaje de reduccion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        decimal ObtenerPorcentajeReduccion(decimal idReduccion);

        /// <summary>
        /// Devolvemos el articulo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        string ObtenerArticulo(decimal idReduccion);

        /// <summary>
        /// Devolvemos el dataset de Catalogos Cargado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo ObtenerCatalogos();

        /// <summary>
        /// Devolvemos el dataTable del catálogo de bancos cargado
        /// </summary>
        /// <returns>DataTable del catálogo de bancos</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_CATBANCODataTable ObtenerCatBanco();


        /// <summary>
        /// Obtiene información de los avalúos que cumplan los requisitos indicados
        /// </summary>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <param name="pagadoEnviado">Pagado/Enviado</param>
        /// <param name="peritoSociedad">Perito/Sociedad</param>
        /// <param name="tipoFecha">Tipo de fecha</param>
        /// <param name="registro">Registro</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfAvaluos.FEXNOT_INF_SOCPERAVALUOS_PDataTable ObtenerInfSociedadPeritosAvaluos(DateTime fechaIni, DateTime fechaFin, decimal pagadoEnviado, decimal peritoSociedad, decimal tipoFecha, string registro);

        /// <summary>
        /// Obtiene las declaraciones por cuenta catastral desde la secretaría de finanzas
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="txtregion">Región de la cuenta catastral</param>
        /// <param name="txtmanzana">Manzana de la cuenta catastral</param>
        /// <param name="txtlote">Lote de la cuenta catastral</param>
        /// <param name="txtunidadprivativa">Unidad privativa de la cuenta catastral</param>
        /// <param name="codEstadoDeclaracion">Código del estado de la declaración</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastralCodEstadoDeclaracionSF(int idnotario, string txtregion, string txtmanzana, string txtlote, string txtunidadprivativa, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression);



        /// <summary>
        /// Obtiene una búsqueda de reducciones por año , artículo y descripción
        /// </summary>
        /// <param name="anio">Año</param>
        /// <param name="articulo">Articulo</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las reducciones obtenidas</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerBusquedaPorReduccion(int anio, decimal? articulo, string descripcion, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene una búsqueda de exenciones por año , artículo y descripción
        /// </summary>
        /// <param name="anio">Año</param>
        /// <param name="articulo">Articulo</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DseCatalogo.FEXNOT_EXENCIONESDataTable</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerBusquedaPorExenciones(int anio, decimal? articulo, string descripcion, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene el catálogo de estados de las declaraciones
        /// </summary>
        /// <returns>DataTable con los estados de la declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_CATESTADDECLARACIONDataTable ObtenerCatEstadoDeclaraciones();


        /// <summary>
        /// Obtiene las declaraciones pertenecientes a un avalúo especificado
        /// </summary>
        /// <param name="idAvaluo">Identificador del avalúo</param>
        /// <param name="idPersona">Identificador del notario</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesDeAvaluo(int idAvaluo, int idPersona, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene las declaraciones pertenecientes a un avalúo especificado consultado desde la secretaría de finanzas
        /// </summary>
        /// <param name="idAvaluo">Identificador del avalúo</param>
        /// <param name="idPersona">Identificador de persona</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesDeAvaluoSF(int idAvaluo, int idPersona, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene un DataTable con la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con la declaración</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorIdDeclaracion(int idDeclaracion);


        /// <summary>
        /// Obtiene el dataSet Completo mediante la busqueda por idDeclaracion
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>Dataset con la declaración</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai ObtenerDseDeclaracionIsaiPorIdDeclaracion(decimal idDeclaracion);


        /// <summary>
        /// Regresa la lista de registros que estan en un estado de pago especificado
        /// </summary>
        /// <param name="estadoPago">Estado del pago de la declaración</param>
        /// <returns>Dataset de la declaración</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorEstadoDePago(Negocio.Interfaces.EstadoPago estadoPago, int startrow, int endrow);

        /// <summary>
        /// Obtiene un dataTable con el domicilio
        /// </summary>
        /// <param name="idDireccion">Identificador de la dirección</param>
        /// <returns>DataTable con el domicilio</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DOMICILIODataTable ObtenerDomicilioParticipante(decimal idDireccion);

        /// <summary>
        /// Obtiene las declaraciones de un tipo de declaración
        /// </summary>
        /// <param name="codTipoDeclaracion">Código del tipo de declaración</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorTipo(int codTipoDeclaracion);


        /// <summary>
        /// Obtiene las declaraciones de tipo jornada notarial por fecha de presentación
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="fechaPresentacionInicial">Fecha inicial</param>
        /// <param name="fechaPresentacionFinal">Fecha final</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorFechaPresentacionTipoJornadaNotarial(int idnotario, DateTime fechaPresentacionInicial, DateTime fechaPresentacionFinal, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene las declaraciones de tipo jornada notarial por cuenta catastral
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="cuentaCatastral">Cuenta catastral. Array con los códigos que forma la cuenta</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastralTipoJornadaNotarial(int idnotario, string[] cuentaCatastral, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene las declaraciones jornada notarial por nombre y apellido paterno
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="nombre">Nombre para filtrar </param>
        /// <param name="apellidoPaterno">Apellido paterno para filtrar</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNombApellPaternoTipoJornadaNotarial(int idnotario, string nombre, string apellidoPaterno, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene las declaraciones por fecha de presentación
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="fechaPresentacionInicial">Fecha inicial para filtrar</param>
        /// <param name="fechaPresentacionFinal">Fecha final para filtrar</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorFechaPresentacion(int idnotario, DateTime fechaPresentacionInicial, DateTime fechaPresentacionFinal);


        /// <summary>
        /// Obtiene las declaraciones por la cuenta catastral
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="cuentaCatastral">Cuenta catastral. Array con los códigos de la cuenta</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastral(int idnotario, string[] cuentaCatastral);


        /// <summary>
        /// Obtiene las declaraciones por nombre y apellido paterno
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="nombre">Nombre para filtrar</param>
        /// <param name="apellidoPaterno">Apellido paterno para filtrar</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNombApellPaterno(int idnotario, string nombre, string apellidoPaterno);


        /// <summary>
        /// Obtiene las declaraciones del notario por fecha y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="fechaPresentacionInicial">Fecha Inicial</param>
        /// <param name="fechaPresentacionFinal">Fecha final para filtrar</param>
        /// <param name="codEstadoDeclaracion">Código del estado para filtrar</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorFechaPresentacionCodEstadoDeclaracion(DateTime fechaPresentacionInicial, DateTime fechaPresentacionFinal, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression);

        /// <summary>
        /// Obtiene las declaraciones del notario por cuenta catastral y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="cuentaCatastral">Cuenta para filtrar. Array con  los códigos de la cuenta</param>
        /// <param name="codEstadoDeclaracion">Código de estado de la declaración</param>
        ///  <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastralCodEstadoDeclaracion(int idnotario, string[] cuentaCatastral, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene las declaraciones del notario por nombre, apellido paternal y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="nombre">Nombre para filtar</param>
        /// <param name="apellidoPaterno">Apellido para filtrar</param>
        /// <param name="codEstadoDeclaracion">Código del estado de la declaración</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNombApellPaternoCodEstadoDeclaracion(int idnotario, int idPerNot, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene las declaraciones del notario por numero de presentación y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="numPresentacion">Numero para filtar</param>
        /// <param name="codEstadoDeclaracion">Código del estado de la declaración</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNumPresentacionCodEstadoDeclaracion(int idnotario, string numPresentacion, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression);


        /// <summary>
        /// Obtiene las condonaciones del año en función del valor catastral para las jornadas notariales
        /// </summary>
        /// <param name="valorCatastral">Valor catastral</param>
        /// <returns>DataTable con las jornadas notariales</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_JORNADANOTARIALDataTable ObtenerJornadaNotarialByAnio(decimal valorCatastral);

        /// <summary>
        /// Consulta el valor catastral en función de la cuenta catastral contra FISCAL
        /// </summary>
        /// <param name="cuentaCatastral">Cuenta Catastral</param>
        /// <returns>Valor Catastral</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        Decimal ObtenerValorCatastral(string[] cuentaCatastral);


        /// <summary>
        /// Busca personas sobre FEXNOT y REGISTRO DE CONTRIBUYENTES
        /// </summary>
        /// <param name="nombre">Nombre de las personas a buscar</param>
        /// <param name="apellidoPaterno">Apellido Paterno de las personas a buscar</param>
        /// <param name="apellidoMaterno">Apellido Materno de las personas a buscar</param>
        /// <param name="rfc">rfc de las personas a buscar</param>
        /// <param name="curp">curpo de las personas a buscar</param>
        /// <param name="ife">ife de las personas a buscar</param>
        /// <returns>DataTable con las personas</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DsePersonas.FEXNOT_PERSONASDataTable ObtenerPersonas(string nombre, string apellidoPaterno, string apellidoMaterno, string rfc, string curp, string ife);

        /// <summary>
        /// Busca personas sobre FEXNOT y REGISTRO DE CONTRIBUYENTES
        /// </summary>
        /// <param name="idPersona">Identificador de las persona a buscar</param>
        /// <returns>DataTable con las persona</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DsePersonas.FEXNOT_PERSONASDataTable ObtenerPersonasPorId(decimal idPersona);




        /// <summary>
        /// Obtiene el DataTable con el catálogo de procedencias
        /// </summary>
        /// <returns>DataTable con el catálogo de procendecias</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_CATPROCEDENCIADataTable ObtenerCatProcedencia();


        /// <summary>
        /// Obtiene el catálogo de exenciones
        /// </summary>
        /// <returns>DataTable con todas las exenciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerExenciones();

        /// <summary>
        /// Obtiene el catálogo de exenciones por ejercicio anual
        /// </summary>
        /// <param name="anio">Año del ejercicio a consultar</param>
        /// <returns>DataTable con las exenciones del ejercicio especificado</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerExencionesPorEjercicioAnual(decimal anio);


        /// <summary>
        /// Obtiene el catálogo de exenciones por el identificador de la exención
        /// </summary>
        /// <param name="idExencion">Identificador de la exención</param>
        /// <returns>DataTable con la exención especificada</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerExencionesPorId(decimal idExencion);


        /// <summary>
        /// Obtiene el catálogo de reducciones
        /// </summary>
        /// <returns>DataTable con todas las reducciones</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerReducciones();


        /// <summary>
        /// Obtiene el catálogo de reducciones por ejercicio anual
        /// </summary>
        /// <param name="anio">Año del ejercicio a consultar</param>
        /// <returns>DataTable con las reducciones del ejercicio especificado</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerReduccionesPorEjercicioAnual(decimal anio);


        /// <summary>
        /// Obtiene el catálogo de exenciones por el identificador de la reducción
        /// </summary>
        /// <param name="idReduccion">Identificador de la reducción</param>
        /// <returns>DataTable con la reducción especificada</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerReduccionesPorId(decimal idReduccion);


        /// <summary>
        /// Obtiene los participantes de una declaración especificada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los participantes de la declaración solicitada</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable ObtenerParticipantesPorIdDeclaracion(int idDeclaracion);

        /// <summary>
        /// Obtiene un participante de una declaración
        /// </summary>
        /// <param name="idPersona">Identificador de la persona</param>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con el participante</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable ObtenerParticipantesPorIdPersona_IdDeclaracion(int idPersona, int idDeclaracion);


        /// <summary>
        /// Obtiene línea de captura para una declaración
        /// </summary>
        /// <param name="DeclaracionIsai">DataSet con la declaración</param>
        /// <param name="esRevisor">Indica si el presentador de la declaración es notario o revisor</param>
        /// <param name="nombreRevisor">Nombre del revisor</param>
        /// <returns>DataSet con la declaración y la línea de captura</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai ObtenerLineaCaptura(DseDeclaracionIsai DeclaracionIsai, bool esRevisor, string nombreRevisor, string fechaCausacion);


        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <param name="fechaCausacion">Fecha Causacion</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfNotarios.FEXNOT_INFONOTARIOSDataTable ObtenerInfNotarios(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas de detalle del numero total de declaraciones.
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfDetalleNotarios.FEXNOT_INFODETALLENOTARIOSDataTable ObtenerInfNotariosTotales(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas de detalle del número total de declaraciones.
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfDetalleNotarios.FEXNOT_INFODETALLENOTARIOSDataTable ObtenerInfNotariosMayor15(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas de detalle del número total de declaraciones.
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfDetalleNotarios.FEXNOT_INFODETALLENOTARIOSDataTable ObtenerInfNotariosMayor180(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Obtiene la información de los avaluos y declaraciones totales para un año
        /// </summary>
        /// <param name="anio">Año</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfEnviosTotales ObtenerInfEnviosTotales(decimal anio);

        /// <summary>
        /// Obtiene la información basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfoLineasCaptura.FEXNOT_INFOLINEASCAPTURADataTable ObtenerInfLineasCaptura(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Obtiene la información de detalle basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas mayor 6. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfoDetalleLineasCaptura.FEXNOT_INFODETALLELINEASCAPTURADataTable ObtenerInfDetalleLineasCapturaMayor(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Obtiene la información de detalle basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas menor 6. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfoDetalleLineasCaptura.FEXNOT_INFODETALLELINEASCAPTURADataTable ObtenerInfDetalleLineasCapturaMenor(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Obtiene la información de detalle basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas totales. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfoDetalleLineasCaptura.FEXNOT_INFODETALLELINEASCAPTURADataTable ObtenerInfDetalleLineasCapturaTotales(decimal? idNotario, DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Obtiene la información de Sociedades y Peritos (pagadas y eviadas) en un rango de fechas.
        /// </summary>
        /// <param name="fechaIni">Fecha Inicio</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="pagadoEnviado">Pagado/Enviado</param>
        /// <param name="peritoSociedad">Perito/Sociedad</param>
        /// <param name="tipoFecha">Tipo de fecha</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfSociedadPeritos.FEXNOT_INFOSOCIEDADPERITOSDataTable ObtenerInfSociedadPeritos(DateTime fechaIni, DateTime fechaFin, decimal pagadoEnviado, decimal peritoSociedad, decimal tipoFecha);


        /// <summary>
        /// Obitene la información de los valores unitarios de mercado y los valores unitarios de renta.
        /// </summary>
        /// <param name="fechaIni">Fecha Inicio</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="tipoComparable">Tipo Comparable</param>
        /// <param name="registroSociedad">Registro Sociedad</param>
        /// <param name="registroPerito">Registro Perito</param>
        /// <param name="nombreColonia">Nombre Colonia</param>
        /// <param name="idColonia">IdColonia</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfValores.FEXNOT_INFOVALORESDataTable ObtenerInfValores(DateTime fechaIni, DateTime fechaFin, string tipoComparable, string registroSociedad, string registroPerito, string nombreColonia, decimal? idColonia);


        /// <summary>
        /// Obtiene la información de los pagos conformes.
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="manzana">Manzana</param>
        /// <param name="lote">Lote</param>
        /// <param name="uprivativa">Unidad Privativa</param>
        /// <param name="numdeclaracion">Número Declaración</param>
        /// <param name="lineacaptura">Línea de Captura</param>
        /// <param name="fechaIni">Fecha Inicio</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="tipo">Tipo</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfPagos.FEXNOT_INFPAGOSDataTable ObtenerInfConsultaPagos(string region, string manzana, string lote, string uprivativa, string numdeclaracion, string lineacaptura, DateTime fechaIni, DateTime fechaFin, string tipo);

        /// <summary>
        /// Obitene la información de los avaluos filtrado por Clave Catastral
        /// </summary>
        /// <param name="region">Región</param>
        /// <param name="manzana">Manzana</param>
        /// <param name="lote">Lote</param>
        /// <param name="unidad">Unidad</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaCuentaCatastral(string region, string manzana, string lote, string unidad);


        /// <summary>
        /// Obtiene la información de los avaluos según filtremos por Sujeto.
        /// </summary>
        /// <param name="sujeto">sujeto</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaSujeto(string sujeto);


        /// <summary>
        /// Obtiene la información de los avaluos filtrado por Ubicación.
        /// </summary>
        /// <param name="numexterior">Número exterior</param>
        /// <param name="numinterior">Número interior</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaUbicacion(string numexterior, string numinterior);


        /// <summary>
        /// Obtiene la información de los avaluos filtrados por idDeclaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaByIdDeclaracion(decimal? idDeclaracion);


        /// <summary>
        /// Obtiene la información para realizar el acuse de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfJustificante.FEXNOT_INFJUSTIFICANTEDataTable ObtenerInfJustificante(decimal idDeclaracion);

        /// <summary>
        /// Obtiene la información para realizar el acuse de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfJustificante.FEXNOT_INFJUSTIFICANTEDataTable ObtenerInfJustificanteGen(decimal idDeclaracion);

        /// <summary>
        /// Obtiene la información  para realizar el acuse de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfJustificante.FEXNOT_INFACUSEDataTable ObtenerInfAcuse(decimal idDeclaracion);

        /// <summary>
        /// Obtiene la información general para realizar el acuse de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfJustificante.FEXNOT_INF_ACUSEGEN_PDataTable ObtenerInfAcuseGen(decimal idDeclaracion);

        /// <summary>
        /// Obtiene la información de los participantes para realizar el acuse de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseInfJustificante.FEXNOT_INF_ACUSEPAR_PDataTable ObtenerInfAcusePar(decimal idDeclaracion);

        //[OperationContract]
        //[FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        //[FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        //DseInfEnviosTotales.FEXNOT_INFODECLARACTOTALESDataTable ObtenerInfDeclaracionesTotales(decimal anio);

        /// <summary>
        /// Obtiene el catálogo de los salarios mínimos
        /// </summary>
        /// <returns>DataTable con el catálogo de salarios mínimos</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseCatalogo.FEXNOT_CATSALARIOMINIMODataTable ObtenerCatalogoSalarioMinimos();


        #endregion

        #region Métodos modificar

        /// <summary>
        /// Modifica un participante de la declaración
        /// </summary>
        /// <param name="participante">DataTable con el participante</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void ModificarParticipanteDeclaracion(DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participante);


        /// <summary>
        /// Modifica el estado de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="codEstadoDeclaracion">Código del estado que se va a modificar en la declaración</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void ModificarEstadoDeclaracion(decimal idDeclaracion, decimal codEstadoDeclaracion);

        #endregion

        #region Otros métodos

        /// <summary>
        /// Realizar el pago o marcar como pagada la declaración
        /// </summary>
        /// <param name="declaracionDT">DataTable con una única fila con la declaración que se va a pagar</param>
        /// <param name="fechaPago">Fecha del pago</param>
        /// <param name="banco">Banco en el que se ha pagado la declaración</param>
        /// <param name="sucursal">Sucursal en el que se ha pagado la declaración</param>
        /// <param name="formaPago">Forma de pago</param>
        [OperationContract(Name = "PagarDeclaracionObject")]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void PagarDeclaracion(DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT, DateTime fechaPago, string banco, string sucursal, string formaPago);

        /// <summary>
        /// Realizar el pago o marcar como pagada la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="fechaPago">Fecha del pago</param>
        /// <param name="banco">Banco en el que se realiza el pago</param>
        /// <param name="sucursal">Sucursal en el que se realiza el pago</param>
        /// <param name="formaPago">Forma de pago</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void PagarDeclaracion(int idDeclaracion,
            DateTime fechaPago,
            string banco,
            string sucursal,
            string formaPago);


        /// <summary>
        /// Calcula el impuesto de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Idenficador de la declaración que se quiere calcular</param>
        /// <param name="fechaDoc">Fecha del documento</param>
        /// <param name="resultadoRecargo">Resultado Recargo</param>
        /// <param name="resultadoImporteRecargo">Resultado Importe Recargo</param>
        /// <param name="resultadoActualizacion">Resultado Actualización</param>
        /// <param name="resultadoImporteActualizacion">Resultado Importe Actualización</param>
        /// <param name="resultadoBaseGravable">Resultado Base Gravable</param>
        /// <param name="resultadoReduccion1995">Resultado Reduccion 1995</param>
        /// <param name="resultadoTasa1995">Resultado Tasa 1995</param>
        /// <param name="resultadoImpuesto">Resultado Impuesto</param>
        /// <param name="resultadoReduccionArt309">Resultado Reducción Artículo 309</param>
        /// <param name="resultadoExencionImporte">Resultado Exención Importe</param>
        /// <param name="resultadoImporteCondonacion">Resultado importe Condonación</param>
        /// <param name="resultadoImporteCondonacionxFecha">Resultado de la condonacion</param>
        /// <returns>Impuesto</returns>
        [OperationContract(Name = "CalcularImpuestoDeclaracionId")]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        decimal CalcularImpuestoDeclaracion(int idDeclaracion,
            DateTime fechaDoc, decimal? valorRef, DateTime? fechaValorRef,
            out Decimal? resultadoRecargo, out Decimal? resultadoImporteRecargo,
            out Decimal? resultadoActualizacion, out Decimal? resultadoImporteActualizacion, out Decimal? resultadoBaseGravable,
            out Decimal? resultadoReduccion1995, out Decimal? resultadoTasa1995, out Decimal? resultadoImpuesto, out Decimal? resultadoReduccionArt309,
            out Decimal? resultadoExencionImporte, out Decimal? resultadoImporteCondonacion, out Decimal? resultadoImporteCondonacionxFecha);


        /// <summary>
        ///  Calcula el impuesto de la declaración
        /// </summary>
        /// <param name="declaracionDT">DataTable con una única fila con la declaración que se quiere realizar el calculo</param>
        /// <param name="fecha_doc">Fecha del documento</param>
        /// <param name="valorRef">Valor referido</param>
        /// <param name="resultadoRecargo">Resultado Recargo</param>
        /// <param name="resultadoImporteRecargo">Resultado Importe Recargo</param>
        /// <param name="resultadoActualizacion">Resultado Actualización</param>
        /// <param name="resultadoImporteActualizacion">Resultado Importe Actualización</param>
        /// <param name="resultadoBaseGravable">Resultado Base Gravable</param>
        /// <param name="resultadoReduccion1995">Resultado Reduccion 1995</param>
        /// <param name="resultadoTasa1995">Resultado Tasa 1995</param>
        /// <param name="resultadoImpuesto">Resultado Impuesto</param>
        /// <param name="resultadoReduccionArt309">Resultado Reducción Artículo 309</param>
        /// <param name="resultadoExencionImporte">Resultado Exención Importe</param>
        /// <param name="resultadoImporteCondonacion">Resultado importe Condonación</param>
        /// <param name="resultadoImporteCondonacionxFecha">Resultado de la condonacion</param>
        /// <returns>Impuesto</returns>
        [OperationContract(Name = "CalcularImpuestoDeclaracionObject")]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        decimal CalcularImpuestoDeclaracion(DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT, DateTime fecha_doc, decimal? valorRef, DateTime? fechaValorReferido, out Decimal? resultadoRecargo,
            out Decimal? resultadoImporteRecargo,
            out Decimal? resultadoActualizacion, out Decimal? resultadoImporteActualizacion, out Decimal? resultadoBaseGravable,
            out Decimal? resultadoReduccion1995, out Decimal? resultadoTasa1995, out Decimal? resultadoImpuesto, out Decimal? resultadoReduccionArt309,
            out Decimal? resultadoExencionImporte, out Decimal? resultadoImporteCondonacion, out Decimal? resultadoImporteCondonacionxFecha);


        /// <summary>
        /// Calcula el impuesto de la declaración
        /// </summary>
        /// <param name="par_codtipodeclaracion">Código del tipo de declaración</param>
        /// <param name="par_participacion">Participación en el acto</param>
        /// <param name="par_valoradquisicion">Valor adquisición</param>
        /// <param name="par_valorcatastral">Valor catastral</param>
        /// <param name="par_valoravaluo">Valor avalúo</param>
        /// <param name="par_eshabitacional">Es habitacional</param>
        /// <param name="par_estasacero">Es tasa 0</param>
        /// <param name="par_idexencion">Identificador de la exención a aplicar</param>
        /// <param name="par_idreduccion">Identificador de la reducción a asignar</param>
        /// <param name="par_codjornotarial">Código de la jornada notarial</param>
        /// <param name="par_porcentajesubsio">Procentaje del subsidio a aplicar</param>
        /// <param name="par_porcentajedisminucion">Porcentaje de la disminución a aplicar</param>
        /// <param name="par_porcentajecondonacion">Porcentaje de la condonación a aplicar</param>
        /// <param name="par_fechadocumento">Fecha del documento</param>
        /// <param name="par_regla">Regla para el cálculo</param>
        /// <param name="resultadoRecargo">Resultado Recargo</param>
        /// <param name="resultadoImporteRecargo">Resultado Importe Recargo</param>
        /// <param name="resultadoActualizacion">Resultado Actualización</param>
        /// <param name="resultadoImporteActualizacion">Resultado Importe Actualización</param>
        /// <param name="resultadoBaseGravable">Resultado Base Gravable</param>
        /// <param name="resultadoReduccion1995">Resultado Reduccion 1995</param>
        /// <param name="resultadoTasa1995">Resultado Tasa 1995</param>
        /// <param name="resultadoImpuesto">Resultado Impuesto</param>
        /// <param name="resultadoReduccionArt309">Resultado Reducción Artículo 309</param>
        /// <param name="resultadoExencionImporte">Resultado Exención Importe</param>
        /// <param name="resultadoImporteCondonacion">Resultado importe Condonación</param>
        /// <param name="resultadoImporteCondonacionxFecha">Resultado de la condonacion</param>
        /// <returns>Impuesto</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        decimal CalcularImpuestoDeclaracion(decimal? par_codtipodeclaracion, decimal? par_participacion,
            decimal? par_valoradquisicion, decimal? par_valorcatastral, decimal? par_valoravaluo, string par_eshabitacional,
            string par_estasacero,
            decimal? par_idexencion, decimal? par_idreduccion, decimal? par_codjornotarial,
            decimal? par_porcentajesubsio, decimal? par_porcentajedisminucion, decimal? par_porcentajecondonacion,
            DateTime? par_fechadocumento, decimal? par_regla, decimal? par_valorReferido, DateTime? par_fechaValorReferido, decimal? par_codActosJuridico, DateTime? par_fechaEscritura,
            out Decimal? resultadoRecargo, out Decimal? resultadoImporteRecargo,
            out Decimal? resultadoActualizacion, out Decimal? resultadoImporteActualizacion, out Decimal? resultadoBaseGravable,
            out Decimal? resultadoReduccion1995, out Decimal? resultadoTasa1995, out Decimal? resultadoImpuesto, out Decimal? resultadoReduccionArt309,
            out Decimal? resultadoExencionImporte, out Decimal? resultadoImporteCondonacion, out Decimal? resultadoImporteCondonacionxFecha);


        /// <summary>
        /// Cambia la declaracion al estado aceptada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void AceptarDeclaracion(decimal idDeclaracion);

        /// <summary>
        /// Cambia la declaracion al estado Presentada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void PresentarDeclaracion(decimal idDeclaracion);

        /// <summary>
        /// Cambia la declaración al estado rechazada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="codMotivoRechazo">Código del motivo de rechazo</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void RechazarDeclaracion(decimal idDeclaracion, decimal? codMotivoRechazo);


        /// <summary>
        /// Cambia la declaracion al estado rechazada pdt de documentación
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void RechazarDeclaracionPdteDocumentacion(decimal idDeclaracion);

        /// <summary>
        /// Actualiza el estado de pago de las declaraciones
        /// </summary>
        /// <param name="LineasCaptura">Líneas de pago a actualizar</param>
        /// <param name="codigoEstadoPago">Nuevo estado de pago</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void ActualizarPagoDeclaracion(List<string> LineasCaptura, int codigoEstadoPago);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadValidacion"></param>
        /// <param name="fechaPago"></param>
        /// <param name="importeTotal"></param>
        /// <param name="numOperBancaria"></param>
        /// <param name="referencia"></param>
        /// <param name="secuenciaTransmision"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        bool ValidarRespuestaOnline(string cadValidacion, string fechaPago, Decimal importeTotal, Int32 numOperBancaria, string referencia, Int32 secuenciaTransmision);

        /// <summary>
        /// Llama al servicio de documental para insertar un fichero de forma transaccional
        /// </summary>
        /// <param name="idDocumentoDigital">String del xml del documento</param>
        /// <param name="listaFicheros">String con la lista de ficheros</param>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        void EspecificarFicheroTransaccional(decimal idDocumentoDigital, string listaFicheros);

        /// <summary>
        /// Llama a la fecha valida tomando 15 dias habiles
        /// </summary>
        /// <param name="Fechainicio">Fecha de inicio</param>



        #endregion

        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        string Verificarfechavencida(DateTime Fechainicio);

        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DateTime VerificarDiaFestivo(string Fechainicio);

        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        DseDeclaracionIsai ObtenerLCIsai(DseDeclaracionIsai declaracionIsai, bool esRevisor, string nombreRevisor, string fechaCausacion);

        /// <summary>
        /// Obtiene línea de captura para una declaración
        /// </summary>
        /// <param name="declaracionIsai">DataSet con la declaración</param>
        /// <returns>La linea de captura devuelta por el servicio</returns>
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        string ObtenerLCIsr(DseDeclaracionIsai declaracionIsai);

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
        [OperationContract]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiException))]
        [FaultContract(typeof(Exceptions.DeclaracionIsaiInfoException))]
        List<decimal> GetDatosIsr(decimal idDeclaracion);




    }

    /// <summary>
    /// Define el estado de la declaración
    /// </summary>
    [DataContract(Name = "EstadoDeclaracion", Namespace = "http://SIGAPred.FuentesExternas/Isai/Negocio/Interfaces")]
    public enum EstadoDeclaracionEnum
    {
        /// <summary>
        /// 0. Indica que la declaración esta en estado Borrador
        /// </summary>
        [EnumMember]
        Borrador = 0,
        /// <summary>
        /// 1. Indica que la declaración esta en estado Pendiente
        /// </summary>
        [EnumMember]
        Pendiente = 1,
        /// <summary>
        /// 2. Indica que la declaración esta en estado Presentada
        /// </summary>
        [EnumMember]
        Presentada = 2,
        /// <summary>
        /// 3. Indica que la declaración esta en estado Rechazada
        /// </summary>
        [EnumMember]
        Rechazada = 3,
        /// <summary>
        /// 4. Indica que la declaración esta en estado Aceptada
        /// </summary>
        [EnumMember]
        Aceptada = 4,
        /// <summary>
        /// 5. Indica que la declaración esta en estado PendienteDocumentacion
        /// </summary>
        [EnumMember]
        PendienteDocumentacion = 5,
    }

    /// <summary>
    /// Define el estado del pago
    /// </summary>
    [DataContract(Name = "EstadoPago", Namespace = "http://SIGAPred.FuentesExternas/Isai/Negocio/Interfaces")]
    public enum EstadoPago
    {
        /// <summary>
        /// 0. Indica que la declaración esta en estado No Pagado
        /// </summary>
        [EnumMember]
        NoPagado = 0,
        /// <summary>
        /// 1. Indica que la declaración esta en estado Pagado 
        /// </summary>
        [EnumMember]
        PendienteSISCOR = 1,
        /// <summary>
        /// 1. Indica que el pago ha sido recibido por SISCOR
        /// </summary>
        [EnumMember]
        PagoRecibidoSISCOR = 2
    }



}
