using System;
using System.ComponentModel;        
using System.Reflection;


/// <summary>
/// Clase que ofrece funciones útiles para tratar con las enumeraciones
/// </summary>
public class EnumUtility
{

    /// <summary>
    /// Función que permite acceder a la descripción de la enumeración
    /// </summary>
    /// <param name="valor">Valor</param>
    /// <returns>Descripción</returns>
    public static string GetDescription(Enum valor)
    {
        FieldInfo fi = valor.GetType().GetField(valor.ToString());
        DescriptionAttribute[] atts = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return (atts.Length > 0) ? atts[0].Description : valor.ToString();
    }
}

/// <summary>
///Enumeración que describe los tipos de persona
///Física
///Moral
/// </summary>
public enum TipoPersona
{
    [Description("Física")]
    Fisica,
    [Description("Moral")]
    Moral
}


/// <summary>
/// Enumeración que describe los posibles mensajes que se obtienen al solicitar una línea de captura
/// {Exito, Error, Linea de captura vigente, Parámetro Invalido, Impuesto Cero, Estado declaración invalido}
/// </summary>
public enum LineaCapturaResultMessage
{
    [Description("La operación se realizó exitosamente.")]
    Exito,
    [Description("Se produjo un error al realizar la operación.")]
    Error,
    [Description("La declaración tiene una línea de captura vigente.")]
    LineaCapturaVigente,
    [Description("No se encontró la información correspondiente a la declaración.")]
    ParametroInvalido,
    [Description("No puede solicitarse una Línea de Captura cuando no hay impuesto por pagar.")]
    ImpuestoEsCero,
    [Description("El estado de la declaración no es válido.")]
    EstadoDeclaracionInvalido
}


/// <summary>
/// Enumeración que describe los tipos de declaración
/// Normal
/// Anticipada
/// Complementaria
/// Jornada notarial
/// </summary>
public enum TipoDeclaracion
{
    Normal,
    Anticipada,
    Complementaria,
    JornadaNotarial
}



/// <summary>
 /// Conjunto de posibles tipos de filtro de búsqueda
/// 0. Fecha
/// 1. Cuenta Catastral
/// 2. Número de Presentación
/// 3. Sujeto
/// 4. Número de Avalúo
/// 5. Identificador de Avalúo
/// </summary>
public enum TipoFiltroBusqueda
{

    Fecha = 0,
    CuentaCatastral = 1,
    NumeroPresentacion = 2,
    Sujeto = 3,
    NumeroAvaluo = 4,
    IdAvaluo = 5
}


/// <summary>
/// Enumeración con los estados de las declaraciones
/// 0. Borrador
/// 1. Pendiente
/// 2. Presentada
/// 4. Aceptada
/// 5. Inconsistente
/// 3. Pendiente de documentación
/// </summary>
public enum EstadosDeclaraciones
{
    Borrador = 0,
    Pendiente = 1,
    Presentada = 2,
    Aceptada = 4,
    Inconsistente = 5,
    PendienteDocumentacion = 3
}


/// <summary>
/// Enumeración con los estados de pago de la declaración
/// 0. SinPago
/// 1. Pagado
/// 2. RecibidoSISCOR
/// </summary>
public enum EstadosPago
{
    SinPago = 0,
    Pagado = 1,
    RecibidoSISCOR = 2,
}


/// <summary>
/// Enumerado para el tipo de pago
///  //Tipo 1 == VIGENTE
///  //Tipo 2 == CON REDUCCION
///  //Tipo 3 == VENCIDO
///  //Tipo 4 == VENCIDO CON REDUCCION
public enum TipoPago
{
    Vigente =1,
    Reduccion=2,
    Vencido=3,
    VencidoReduccion = 4,
    isr = 5
}