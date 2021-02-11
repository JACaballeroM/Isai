using System;
using ServiceDeclaracionIsai;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web;
/// <summary>
/// Clase encargada de gestionar los métodos para la consulta y configuración del gridview del control de reducciones
/// </summary>
public class DseReduccionesPagger : PaggerBase
{
    
    /// <summary>
    /// Función que obtiene el número de registros totales de reducciones
    /// </summary>
    /// <param name="anio">Año para la búsqueda</param>
    /// <param name="articulo">Artículo de búsqueda</param>
    /// <param name="descripcion">Descripción de la búsqueda</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Número total de registros obtenidos para la consulta</returns>
    public int NumTotalFilasBuscarReduccionesPorArticuloDescripcion(int anio, string articulo, string descripcion, int pageSize, int indice, string SortExpression)
    {
        return GetRowCount();
    }


    /// <summary>
    /// Función que obtiene registros de reducciones
    /// </summary>
    /// <param name="anio">Año para la búsqueda</param>
    /// <param name="articulo">Artículo para la búsqueda</param>
    /// <param name="descripcion">Descripción de la búsqueda</param>
    /// <param name="pageSize">Tamaño de registros para una página del grid</param>
    /// <param name="indice">Número de página del grid a mostrar</param>
    /// <param name="SortExpression">Expresión de orden para el grid</param>
    /// <returns>Registros en DseCatalogo.FEXNOT_REDUCCIONESDataTable</returns>
    public DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerBusquedaReduccionesPorArticuloDescripcion(int anio, string articulo, string descripcion, int pageSize, int indice, string SortExpression)
    {
        SavePaggerSettings(indice, pageSize, SortExpression);
        decimal? articulos;
        if (string.IsNullOrEmpty(articulo))
        {
            articulos = null;
        }
        else
        {
            articulos = Convert.ToInt32(articulo);
        }

        if (string.IsNullOrEmpty(descripcion))
        {
            descripcion = string.Empty;
        }
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "ARTICULO ASC";
        }

        DseCatalogo.FEXNOT_REDUCCIONESDataTable dt;
        dt = ClienteDeclaracionIsai.ObtenerBusquedaPorReduccion(anio, articulos, descripcion, base.pageSize, base.indice, ref base.totalPages, SortExpression);
        dt.Columns.Add("BENEFICIOS", typeof(String));
        if (dt != null && dt.Count > 0)
        {
            foreach (DseCatalogo.FEXNOT_REDUCCIONESRow row in dt)
            {
                row["BENEFICIOS"]= string.Format("{0:0%}", row.DESCUENTO);
            }

        }
        return dt;
    }
}
