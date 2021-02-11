using System;
using ServiceDeclaracionIsai;

/// <summary>
/// Clase encargada de gestionar los métodos para la consulta y configuración del gridview del control de exenciones
/// </summary>
public class DseExencionesPagger : PaggerBase
{
    /// <summary>
    /// Función que obtiene el número de registros totales de exenciones
    /// </summary>
    /// <param name="anio">Año para la búsqueda</param>
    /// <param name="articulo">Artículo de búsqueda</param>
    /// <param name="descripcion">Descripción de la búsqueda</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalFilasBuscarExencionesPorArticuloDescripcion(int anio, decimal articulo, string descripcion, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }


    /// <summary>
    /// Función que obtiene registros de exenciones
    /// </summary>
    /// <param name="anio">Año para la búsqueda</param>
    /// <param name="articulo">Artículo para la búsqueda</param>
    /// <param name="descripcion">Descripción de la búsqueda</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseCatalogo.FEXNOT_EXENCIONESDataTable</returns>
    public DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerBusquedaExencionesPorArticuloDescripcion(int anio, string articulo, string descripcion, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);
        decimal? articulos;
        
        if (string.IsNullOrEmpty(articulo))
        {
            articulos = null;
        }
        else
        {
            articulos = (decimal?)Decimal.Parse(articulo);
        }

        if (string.IsNullOrEmpty(descripcion))
        {
            descripcion = string.Empty;
        }
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "ARTICULO DESC";
        }

        DseCatalogo.FEXNOT_EXENCIONESDataTable dt;
        dt = ClienteDeclaracionIsai.ObtenerBusquedaPorExenciones(anio, articulos, descripcion, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        return dt;
    }
}
