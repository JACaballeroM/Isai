/// <summary>
/// Clase PagerBase que se encarga de gestionar los datos para la paginación de los gridviews
/// </summary>
public class PaggerBase : PageBaseISAI
{

    /// <summary>
    /// Número total de filas
    /// </summary>
    protected int totalPages = -1;

    /// <summary>
    /// Número de página 
    /// </summary>
    protected int indice = -1;

    /// <summary>
    /// Número de filas por página
    /// </summary>
    protected int pageSize = -1;

    /// <summary>
    /// Expresión de ordenación
    /// </summary>
    protected string SortExpression = "";
  
    /// <summary>
    /// Función que guarda información sobre la paginación
    /// </summary>
    /// <param name="indice">Número de página</param>
    /// <param name="pageSize">Número de filas por página</param>
    /// <param name="SortExpression">Expresión de ordenación</param>
    protected void SavePaggerSettings(int indice, int pageSize, string SortExpression)
    {
        if (indice > 0)
            this.indice = indice / pageSize + 1;
        else
            this.indice = 1;

        this.pageSize = pageSize;
        this.SortExpression = SortExpression;
    }


    /// <summary>
    /// Función que cuenta el número de filas
    /// </summary>
    /// <returns>Entero con el número de filas totales</returns>
    protected int GetRowCount()
    {
        return this.totalPages;
    }


}

