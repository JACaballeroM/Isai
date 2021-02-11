    /// <summary>
    /// Clase estatica con las constantes de la aplicación:
    ///     - Nombres de parametros de la QueryString
    ///     - Posibles valores de los parametros de la QueryString
    ///     - Nombres de las variables de sesión
    /// </summary>
    public static class Constantes
    {
        //NOMBRE DE LOS PARAMETROS
        public const string PAR_VAL_ADQUIRIENTE = "Adquirente";
        public const string PAR_VAL_ENAJENANTE = "Enajenante";
        /// <summary>
        /// Identifica a la caché de ISAI de catálogos
        /// </summary>
        public const string CACHE_ISAI_CATALOGOS = "DseCatalogo";
        /// <summary>
        /// MENSAJE DE USUARIO NO ENCONTRADO
        /// </summary>
        public const string MSJ_USUARIONOEXISTE_EXECP = "No se ha encontrado usuario en RCON";
        /// <summary>
        /// Identifica a la caché de RCON de catálogos
        /// </summary>
        public const string CACHE_RCON_CATALOGOS = "DseRcon";

        /// <summary>
        /// Identifica la caché de Registro de contribuyentes
        /// </summary>
        public const string CATALOGOSRCO = "CatalogosRCO";

        /// <summary>
        /// Identifica a la caché de delegaciones
        /// </summary>
        public const string DELEGACIONES = "DELEGACIONES";

        /// <summary>
        /// Identifica a la caché de estados
        /// </summary>
        public const string ESTADOS = "Estados";


        //NOMBRE DE LOS PARAMETROS
        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de página de origen
        /// </summary>
        public const string PAR_PAGINAORIGEN = "P0rg";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de operación sobre la declaración
        /// </summary>
        public const string PAR_OPERACION = "op";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de operación sobre la declaración padre
        /// </summary>
        public const string PAR_OPERACION_PADRE = "opp";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de una declaración
        /// </summary>
        public const string PAR_IDDECLARACION = "idDeclaracion";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de una persona
        /// </summary>
        public const string PAR_IDPERSONA = "idPersona";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del código del tipo de una persona
        /// </summary>
        public const string PAR_CODTIPOPERSONA = "codTipoPersona";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de un avalúo
        /// </summary>
        public const string PAR_IDAVALUO = "idAval";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del tipo de una declaración
        /// </summary>
        public const string PAR_TIPODECLARACION = "tipoDec";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del código del acto jurídico de la declaración
        /// </summary>
        public const string PAR_CODACTOJUR = "codActoJur";

        /// <summary>
        ///  Constante que almacena el valor del código de acto jurídico herencia
        /// </summary>
        public const string VALOR_CODACTOJUR_HERENCIA = "3";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de la declaracion padre
        /// </summary>
        public const string PAR_IDDECLARACIONPADRE = "idDecPadre";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del tipo de justificante
        /// </summary>
        public const string PAR_TIPOJUSTIFICANTE = "tipoJustificante";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de un notario
        /// </summary>
        public const string PAR_IDNOTARIO = "idnot";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de dirección
        /// </summary>
        public const string PAR_IDDIRECCION = "IDDIRECCION";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del año
        /// </summary>
        public const string PAR_ANIO = "anio";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del límite inferior
        /// </summary>
        public const string PAR_LIMITE_INFERIOR = "limiteinferior";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del límite superior
        /// </summary>
        public const string PAR_LIMITE_SUPERIOR = "limitesuperior";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del código del tipo de la declaración
        /// </summary>
        public const string PAR_CODTIPODECLARACION = "CodTipoDeclaracion";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del código de estado declaración
        /// </summary>
        public const string PAR_CODESTADODECLARACION = "CodEstadoDeclaracion";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del número de Notario
        /// </summary>
        public const string PAR_NUMNOTARIO = "numNotario";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador del código del tipo de la declaración
        /// </summary>
        public const string PAR_SEARCHNOT = "searchnot";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de la fecha de inicio
        /// </summary>
        public const string PAR_FECHAINI = "fechaIni";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de la fecha de finalización
        /// </summary>
        public const string PAR_FECHAFIN = "fechaFin";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de tipo de fecha
        /// </summary>
        public const string PAR_TIPOFECHA = "tipoFecha";

        /// <summary>
        ///  Constante que almacena el nombre de la variable de la querystring para el identificador de registro
        /// </summary>
        public const string PAR_REGISTRO = "registro";
        public const string PAR_NOMBRENOT = "nomNotario";

        //VALORES DIRECCIÓN
        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de manzana
        /// </summary>
        public const string PAR_VAL_MANZANA = "Manzana";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de región
        /// </summary>
        public const string PAR_VAL_REGION = "Region";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de lote
        /// </summary>
        public const string PAR_VAL_LOTE = "Lote";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de unidad privativa
        /// </summary>
        public const string PAR_VAL_UNIDAD = "Unidad";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de sujeto
        /// </summary>
        public const string PAR_VAL_SUJETO = "Sujeto";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de calle
        /// </summary>
        public const string PAR_VAL_CALLE = "Calle";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de número interior
        /// </summary>
        public const string PAR_VAL_INT = "Int";

        //VALORES FILTRO
        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de filtro
        /// </summary>
        public const string REQUEST_FILTRO = "filtro";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de sort expression
        /// </summary>
        public const string REQUEST_SORTEXP = "sortexp";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de dort direction
        /// </summary>
        public const string REQUEST_SORTDIR = "sortdir";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de sort expression secundaria
        /// </summary>
        public const string REQUEST_SORTEXP2 = "sortexp2";

        /// <summary>
        /// Constante que almacena el nombre de la variable de la querystring para el identificador de sort direction secundaria
        /// </summary>
        public const string REQUEST_SORTDIR2 = "sortdir2";


        //VALORES PARA EL PARAMETRO OPERACION
        /// <summary>
        /// Constante que almacena el valor de inserción para el parámetro operación 
        /// </summary>
        public const string PAR_VAL_OPERACION_INS = "I"; //Nuevo

        /// <summary>
        /// Constante que almacena el valor de modificación para el parámetro operación 
        /// </summary>
        public const string PAR_VAL_OPERACION_MOD = "M"; //Modificar

        /// <summary>
        /// Constante que almacena el valor de eliminación para el parámetro operación 
        /// </summary>
        public const string PAR_VAL_OPERACION_DEL = "D"; //Eliminar

        /// <summary>
        /// Constante que almacena el valor de visualización para el parámetro operación 
        /// </summary>
        public const string PAR_VAL_OPERACION_VER = "V"; //Visualizar


        //VALORES PARA EL PARAMETRO TIPO DE DECLARACION
        /// <summary>
        /// Constante que almacena el valor de tipo normal para el parámetro tipo de declaración 
        /// </summary>
        public const string PAR_VAL_TIPODECLARACION_NOR = "0";     //Normal

        /// <summary>
        /// Constante que almacena el valor de tipo anticipada para el parámetro tipo de declaración 
        /// </summary>
        public const string PAR_VAL_TIPODECLARACION_ANTI = "1";    //Anticipada

        /// <summary>
        /// Constante que almacena el valor de tipo complementaria para el parámetro tipo de declaración 
        /// </summary>
        public const string PAR_VAL_TIPODECLARACION_COMPLE = "2";  //Complementaria

        /// <summary>
        /// Constante que almacena el valor de tipo jornada notarial para el parámetro tipo de declaración 
        /// </summary>
        public const string PAR_VAL_TIPODECLARACION_JOR = "3";     //Jornada Notarial


        //VALORES PARA EL PARAMETRO TIPO DE JUSTIFICANTE
        /// <summary>
        /// Constante que almacena el valor de tipo envío para el parámetro tipo de justificante
        /// </summary>
        public const string PAR_VAL_TIPOJUSTIFICANTE_ENVIO = "0"; //Justificante de envio ISAI

        /// <summary>
        /// Constante que almacena el valor de tipo pago para el parámetro tipo de justificante
        /// </summary>
        public const string PAR_VAL_TIPOJUSTIFICANTE_PAGO = "1"; //Justificante de pago ISAI


        //VALORES PARA EL PARAMETROS DE CODIGO DE TIPO PERSONA
        /// <summary>
        /// Constante que almacena el valor de tipo física para el parámetro de código de tipo persona
        /// </summary>
        public const string PAR_VAL_PERSONA_FISICA = "F";

        /// <summary>
        /// Constante que almacena el valor de tipo moral para el parámetro de código de tipo persona
        /// </summary>
        public const string PAR_VAL_PERSONA_MORAL = "M";


        //VALORES PARA EL PARAMETRO DE TIPO VALOR UNITARIO
        /// <summary>
        /// Constante que almacena el valor de tipo venta para el parámetro de tipo valor unitario
        /// </summary>
        public const string PAR_VAL_VALOR_VENTA = "V";

        /// <summary>
        /// Constante que almacena el valor de tipo renta para el parámetro de tipo valor unitario
        /// </summary>
        public const string PAR_VAL_VALOR_RENTA = "R";

        
        //VALORES PARA EL PARAMETRO BOOLEANO TRUE / FALSE
        /// <summary>
        /// Constante que almacena el valor de true ara el parámetro booleano
        /// </summary>
        public const string PAR_VAL_TRUE = "S";

        /// <summary>
        /// Constante que almacena el valor de false ara el parámetro booleano
        /// </summary>
        public const string PAR_VAL_FALSE = "N";

        //VALORES PARA EL PARAMETRO ESTADO DE DECLARACION
        /// <summary>
        /// Constante que almacena el valor de estado borrador para el parámetro de estado declaración
        /// </summary>
        public const string PAR_VAL_ESTADODECLARACION_BORR = "0";

        /// <summary>
        /// Constante que almacena el valor de estado pendiente de presentar para el parámetro de estado declaración
        /// </summary>
        public const string PAR_VAL_ESTADODECLARACION_PDTE_PRESENTAR = "1";

        /// <summary>
        /// Constante que almacena el valor de estado presentada para el parámetro de estado declaración
        /// </summary>
        public const string PAR_VAL_ESTADODECLARACION_PRESENTADA = "2";

        /// <summary>
        /// Constante que almacena el valor de estado aceptada para el parámetro de estado declaración
        /// </summary>
        public const string PAR_VAL_ESTADODECLARACION_ACEPTADA = "3";

        /// <summary>
        /// Constante que almacena el valor de estado rechazada para el parámetro de estado declaración
        /// </summary>
        public const string PAR_VAL_ESTADODECLARACION_RECHAZADA = "4";

        /// <summary>
        /// Constante que almacena el valor de estado pendiente de documentación para el parámetro de estado declaración
        /// </summary>
        public const string PAR_VAL_ESTADODECLARACION_PDTE_DOCUMENTACION = "5";

        //VALORES PARA LA FORMA DE PAGO DE LA DECLARACION
        /// <summary>
        /// Constante que almacena el valor de forma de pago telemático para la forma de pago de la declaración
        /// </summary>
        public const string PAR_VAL_FORMAPAGO_TELEMATICO = "Telematico";

        /// <summary>
        /// Constante que almacena el valor de forma de pago caja para la forma de pago de la declaración
        /// </summary>
        public const string PAR_VAL_FORMAPAGO_CAJA = "Caja";


        //VALORES PARA EL SERVICIO WEB FUT (ISAI) DE SOLICITAR LINEA DE CAPTURA
        /// <summary>
        /// Constante que almacena el valor de derechos para el servicio web FUT para solicitar línea de captura
        /// </summary>
        public const string PAR_VAL_FUT_DERECHOS = "0";

        /// <summary>
        /// Constante que almacena el valor de operación para el servicio web FUT para solicitar línea de captura
        /// </summary>
        public const string PAR_VAL_FUT_TIPOPERACION = "O";

        /// <summary>
        /// Constante que almacena el valor de clave para el servicio web FUT para solicitar línea de captura
        /// </summary>
        public const string PAR_VAL_FUT_CLAVE = "92";

        /// <summary>
        /// Constante que almacena el valor de usuario para el servicio web FUT para solicitar línea de captura
        /// </summary>
        public const string PAR_VAL_FUT_USUARIO = "Catastro";

        /// <summary>
        /// Constante que almacena el valor de password para el servicio web FUT para solicitar línea de captura
        /// </summary>
        public const string PAR_VAL_FUT_PASSWORD = "c88048bc6ad9097a9f767bbf233b12e2";


        //VALORES VIGENCIA
        /// <summary>
        /// Constante que almacena el valor avalúos vigentes
        /// </summary>
        public const string PAR_VIGENTE = "S";

        /// <summary>
        /// Constante que almacena el valor de avalúos no vigentes
        /// </summary>
        public const string PAR_NO_VIGENTE = "N";

        /// <summary>
        /// Constante que almacena el valor de todos los avalúos (vigentes + no vigentes )
        /// </summary>
        public const string PAR_VIGENCIA_TODOS = "T";
             

        /// <summary>
        /// Constante que almacena el valor del texto del acto jurídico de Herencia
        /// </summary>
        public const string PAR_VAL_HERENCIA = "Herencia";


        //VALORES URLS
        /// <summary>
        /// Constante que almacena el nombre de la página BandejaEntrada.aspx
        /// </summary>
        public const string URL_SUBISAI_BANDEJAENTRADA = "~/BandejaEntrada.aspx";

        public const string URL_SUBISAI_HOME = "~/Home.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página BandejaJornada.aspx
        /// </summary>
        public const string URL_SUBISAI_BANDEJAJORNADA = "~/BandejaJornada.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página BandejaEntradaSF.aspx
        /// </summary>
        public const string URL_SUBISAI_BANDEJAENTRADASF = "~/BandejaEntradaSF.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página CambioClave.aspx
        /// </summary>
        public const string URL_SUBISAI_CAMBIOCLAVE = "~/CambioClave.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página Declaracion.aspx
        /// </summary>
        public const string URL_SUBISAI_DECLARACION = "~/Declaracion.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página Declaraciones.aspx
        /// </summary>
        public const string URL_SUBISAI_DECLARACIONES = "~/Declaraciones.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página DeclaracionPersonas.aspx
        /// </summary>
        public const string URL_SUBISAI_DECLARACIONPERSONAS = "~/DeclaracionPersonas.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página DeclaracionPersona.aspx
        /// </summary>
        public const string URL_SUBISAI_DECLARACIONPERSONA = "~/DeclaracionPersona.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página DescargaJustificante.aspx
        /// </summary>
        public const string URL_SUBISAI_JUSTIFICANTES = "~/DescargaJustificante.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página InformeJustificante.aspx
        /// </summary>
        public const string URL_SUBISAI_INF_JUSTIFICANTE = "~/InformeJustificante.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página InformeAcuse.aspx
        /// </summary>
        public const string URL_SUBISAI_INF_ACUSE = "~/InformeAcuse.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página InformeNotarios.aspx
        /// </summary>
        public const string URL_SUBISAI_INF_NOTARIOS = "~/InformeNotarios.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página InformeLineasCaptura.aspx
        /// </summary>
        public const string URL_SUBISAI_INF_LINEACAPTURA = "~/InformeLineasCaptura.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página InformeConsultaEspecifica.aspx
        /// </summary>
        public const string URL_SUBISAI_INF_CONSULTAESPECIFICA = "~/InformeConsultaEspecifica.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página InformeSociedadPeritos.aspx
        /// </summary>
        public const string URL_SUBISAI_INF_SOCIEDAD = "~/InformeSociedadPeritos.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página IFrameEspecificarDireccion.aspx
        /// </summary>
        public const string URL_SUBISAI_ESPECIFICARDIRECCION = "~/IFrameEspecificarDireccion.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página IFrameBuscadorPersonas.aspx
        /// </summary>
        public const string URL_SUBISAI_BUSCARPERSONA = "~/IFrameBuscadorPersonas.aspx";

        /// <summary>
        /// Constante que almacena el nombre de la página IFrameFUT.aspx
        /// </summary>
        public const string URL_SUBISAI_FUT = "~/IFrameFUT.aspx";


        //MENSAJE DE INFORMACION DE MODIFICACION DE IMPORTE DE DECLARACION
        /// <summary>
        /// Constante que almacena el mensaje de información de importe del impuesto calculado es distinto del declarado
        /// </summary>
        public const string MENSAJEIMPORTEDECLARACION = "El importe del impuesto calculado es distinto al del importe declarado";


        //MENSAJES PARA LA TOMA DE DECISION
        /// <summary>
        /// Constante que almacena el mensaje de declaración aceptada
        /// </summary>
        public const string MENSAJE_DECLARACION_ACEPTADA = "Aceptada";

        /// <summary>
        /// Constante que almacena el mensaje de declaración inconsistente
        /// </summary>
        public const string MENSAJE_DECLARACION_INCONSISTENTE = "Inconsistente";

        /// <summary>
        /// Constante que almacena el mensaje de declaración con documentación pendiente 
        /// </summary>
        public const string MENSAJE_DECLARACION_PENDIENTE = "Pendiente de documentación";


        //VALORES FUNCIONALIDADES
        /// <summary>
        /// Constante que almacena el valor de la funcionalidad permitir toma decisión
        /// </summary>
        public const string FUN_PERMITIRTOMADECISION = "FunPermitirTomaDecision";

        /// <summary>
        /// Constante que almacena el valor de la funcionalidad externa para ISAI
        /// </summary>
        public const string FUN_EXTPARAISAI = "FunExtParaIsai";

        /// <summary>
        /// Constante que almacena el valor de la funcionalidad Informes de ISAI 
        /// </summary>
        public const string FUN_INFORMES = "FunInformesIsai";

        //VALORES IMÁGENES
        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono consulta disponible
        /// </summary>
        public const string IMG_CONSULTA = "~/Images/search.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono editar disponible
        /// </summary>
        public const string IMG_MODIFICA = "~/Images/edit.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de búsqueda disponible
        /// </summary>
        public const string IMG_BUSQUEDA = "~/Images/search.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de cambio de estado disponible
        /// </summary>
        public const string IMG_CAMBIAESTADO = "~/Images/Get_LC.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de papelera disponible
        /// </summary>
        public const string IMG_ELIMINA = "~/Images/trash.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de añadir disponible
        /// </summary>
        public const string IMG_ANADIR = "~/Images/plus.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de documento disponible
        /// </summary>
        public const string IMG_DOC = "~/Images/two-docs.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de lupa(zoom) disponible
        /// </summary>
        public const string IMG_ZOOM = "~/Images/zoom-in.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imagen de icono de copia disponible
        /// </summary>
        public const string IMG_CLIPBOARD = "~/Images/clipboard.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imagen de icono de consulta no disponible
        /// </summary>
        public const string IMG_CONSULTA_DISABLED = "~/Images/search_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de edición no disponible
        /// </summary>
        public const string IMG_MODIFICA_DISABLED = "~/Images/edit_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de búsqueda no disponible
        /// </summary>
        public const string IMG_BUSQUEDA_DISABLED = "~/Images/search_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de cambio de estado no disponible
        /// </summary>
        public const string IMG_CAMBIAESTADO_DISABLED = "~/Images/Get_LC2.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de papelera no disponible
        /// </summary>
        public const string IMG_ELIMINA_DISABLED = "~/Images/trash_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de añadir no disponible
        /// </summary>
        public const string IMG_ANADIR_DISABLED = "~/Images/plus_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de documento no disponible
        /// </summary>
        public const string IMG_DOC_DISABLED = "~/Images/two-docs_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de lupa(zoom) no disponible
        /// </summary>
        public const string IMG_ZOOM_DISABLED = "~/Images/zoom-in_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de copia no disponible
        /// </summary>
        public const string IMG_CLIPBOARD_DISABLED = "~/Images/clipboard_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de usuario
        /// </summary>
        public const string IMG_USER = "~/Images/user.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de usuario no disponible
        /// </summary>
        public const string IMG_USER_DISABLED = "~/Images/user_p.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de carro
        /// </summary>
        public const string IMG_CARRO = "~/Images/cart.gif";

        /// <summary>
        /// Constante que almacena la ruta y nombre de la imágen de icono de carro no disponible
        /// </summary>
        public const string IMG_CARRO_DISABLED = "~/Images/cart_p.gif";


        /// <summary>
        /// Constante que almacena el valor del salario mínimo para el beneficio fiscal tasa cero
        /// </summary>
        public const int TasaCeroLimiteSalariosMinimos = 12073;

        //Estilos
        /// <summary>
        /// Constante que almacena el nombre del estilo de text box obligatorio
        /// </summary>
        public const string STYLE_TEXTBOX_OBLIGATORIO = "TextBoxObligatorio";


        //DDL OPCIÓN BUSQUEDA
        /// <summary>
        /// Constante que almacena el valor de búsqueda perito desde la selección en un drop down list
        /// </summary>
        public const string DDLBUSQUEDA_PERITOS = "perito";

        /// <summary>
        /// Constante que almacena el valor de búsqueda sociedad desde la selección en un drop down list
        /// </summary>
        public const string DDLBUSQUEDA_SOCIEDADES = "sociedad";

        /// <summary>
        /// Constante que almacena el valor del indice cuando no hay ningún elemento seleccionado en un drop down list 
        /// </summary>
        public const string UI_DDL_VALUE_NO_SELECT = "-1";

        //MENSAJES ERROR
        /// <summary>
        /// Constante que almacena el mensaje de error al realizar la operación
        /// </summary>
        public const string MSJ_ERROR_OPERACION = "Se presentó un error al realizar la operación: ";

        /// <summary>
        /// Constante que almacena el mensaje de error cuando no existe valor catastral perteneciente a una cuenta y fechas indicadas
        /// </summary>
        public const string MSJ_ERROR_NOEXISTE_VALCAT_FECHA = "No existe valor catastral perteneciente a esta cuenta catastral para la fecha indicada";

        /// <summary>
        /// Constante que almacena el mensaje de error cuando no existe valor catastral perteneciente a una cuenta y fechas actual
        /// </summary>
        public const string MSJ_ERROR_NOEXISTE_VALCAT_HOY = "No existe valor catastral perteneciente a esta cuenta catastral para la fecha actual";

        /// <summary>
        /// Constante que almacena el mensaje de error cuando una cuenta catastral no existe
        /// </summary>
        public const string MSJ_ERROR_NOEXISTE_CUENTACAT = "No existe la Cuenta Catastral";

        /// <summary>
        /// Constante que almacena el mensaje de error al querer editar/añadir/ver participantes sin introducir una cuenta válida
        /// </summary>
        public const string MSJ_ERROR_NOEXISTE_CUENTACAT_PARTICIPANTES = "No se puede editar, añadir o ver participantes sin introducir cuenta catastral válida";

        /// <summary>
        /// Constante que almacena el mensaje de error cuando el predio que se solicita no es fiscalmente válido
        /// </summary>
        public const string MSJ_PREDIO_FISCALMENTE_NO_VALIDO = "El predio solicitado no es fiscalmente válido";

         /// <summary>
        /// Constante que almacena el mensaje de error si no se ha adjuntado un documento que acredite el beneficio fiscal
        /// </summary>
        public const string MSJ_ERROR_ANADIRDOCJUSTI = "El beneficio fiscal seleccionado requiere que seleccione un documento que lo justifique.";
       
         /// <summary>
        /// Constante que almacena el mensaje que se muestra al actualizarse el valor catastral
        /// </summary>
        public const string MSJ_ACT_VALORCAT = "Se ha actualizado el valor catastral.";

        /// <summary>
        /// Constante que almacena el mensaje de error al solicitar línea de captura para una declaración complementaria por falta de impuesto a pagar
        /// </summary>
        public const string MSJ_ERROR_SOLLINEA = "No puede solicitarse una Línea de Captura para esta declaración complementaria porque no hay impuesto por pagar. Se requiere información adicional sobre como proceder.";

        //NOMBRE COLUMNAS
        /// <summary>
        /// Constante que contiene el nombre de la columna de NombreCompleto
        /// </summary>
        public const string NOMBRE_COMPLETO_COLUMN = "NombreCompleto"; 
        /// <summary>
        /// Constante que contiene el nombre de la columna de NUMPRESENTACION
        /// </summary>
        public const string GRIDCOL_NUMPRESENTACIONDEC = "NUMPRESENTACION";


        //TEXTO BOTONES
        /// <summary>
        /// Constante que almacena el texto para botón de cancelar
        /// </summary>
        public const string BTN_CANCELAR = "Cancelar";

        /// <summary>
        /// Constante que almacena el texto para el botón de Volver
        /// </summary>
        public const string BTN_VOLVER = "Volver";


        /// <summary>
        /// Constante que almacena el valor de la longitud máxima para la línea de descripción
        /// </summary>
        public const int LongitudLineaDescripcion = 80;

        /// <summary>
        /// Almacena el valor que se establece al seleccionar una colonia 
        /// </summary>
        public const string VALORCOLONIA = "ValorColonia";

        /// <summary>
        /// Almacena el nombre del viewState para los roles
        /// </summary>
        public const string KEY_VIEW_ROLES = "catRolesActosJurArray";

        /// <summary>
        /// Almacena el valor que se establece para el view state de Salarios Mínimos
        /// </summary>
        public const string CACHE_SalariosMinimos = "SalariosMinimos";

        /// <summary>
        /// Código con el que se evalúa si un dde de tipo delegación está vacío
        /// </summary>
        public const string VALIDARDELEGACION = "Seleccione una Delegación...";

        /// <summary>
        /// Código con el que se evalúa si un dde de tipo localidad está vacío
        /// </summary>
        public const string VALIDARLOCALIDAD = "Seleccione una Localidad...";

        /// <summary>
        /// Código con el que se evalúa si un dde de tipo tipoAsentamiento está vacío
        /// </summary>
        public const string VALIDARTIPOASENTAMIENTO = "Seleccione un Tipo de Asentamiento...";

        /// <summary>
        /// Código con el que se evalúa si un dde de tipo tipoAsentamiento está vacío
        /// </summary>
        public const string VALIDARTIPOVIA = "Seleccione un Tipo de Vía...";

        /// <summary>
        /// Constante que almacena el texto a mostrar si no se ha seleccionado ninguna reducción
        /// </summary>
        public const string TEXT_SIN_REDUCCION = "Debe seleccionar una reducción con el buscador";

        /// <summary>
        /// Constante que almacena el texto a mostrar si no se ha seleccionado ninguna exención
        /// </summary>
        public const string TEXT_SIN_EXENCION = "Debe seleccionar una exención con el buscador";

        /// <summary>
        /// Constante que almacen el texto a mostrar si no se ha proporcionado la línea de captura
        /// </summary>
        public const string TEXT_SIN_LINEA = "Debe proporcionar la línea de Captura";

        /// <summary>
        /// PARÁMETRO VIEWSTATE SELECCIONADO
        /// </summary>
        public const string PAR_VIEWSTATE_SELECCIONADO = "Seleccionado";

        /// <summary>
        /// Constante que almacen el texto a mostrar si no se ha proporcionado la fecha de captura
        /// </summary>
        public const string TEXT_SIN_FECHA ="Debe proporcionar la fecha de Captura";


        /// <summary>
        /// Constante que almacena el nombre de la página InformeJustificante.aspx
        /// </summary>
        public const string URL_SUBISAI_INF_JUSTIFICANTE_ISR = "~/InformeJustificanteISR.aspx";
    }
