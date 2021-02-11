using System;
using ServiceRCON;
using ServicePeritosSociedades;

/// <summary>
/// Clase encargada de gestionar los métodos para la consulta y configuración del gridview del control de búsqueda de peritos
/// </summary>
public class DsePeritosPagger:PaggerBase
{
   /// <summary>
   /// Función que obtiene los registros de sociedadValuacion por datos personales o por datos identificativos
   /// </summary>
   /// <param name="registro">Registro</param>
   /// <param name="nombre">Nombre de la sociedad</param>
   /// <param name="rfc">Rfc</param>
    /// <returns>Registros en DsePeritosSociedades.SociedadValuacionDataTable</returns>
    public DsePeritosSociedades.SociedadValuacionDataTable ObtenerBusquedaSociedades(string registro, string nombre, string rfc)
    {
        if (string.IsNullOrEmpty(registro))
        {
            registro = string.Empty;
        }
        if (string.IsNullOrEmpty(nombre))
        {
            nombre = string.Empty;
        }
        if (string.IsNullOrEmpty(rfc))
        {
            rfc = string.Empty;
        }
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "REGISTRO DESC";
        }
        DsePeritosSociedades peritosSocDS = new DsePeritosSociedades();
        if (string.IsNullOrEmpty(registro))
        {
            peritosSocDS = ClientePeritosSoci.GetSocValuacionByDatosPersonales(nombre, ServicePeritosSociedades.TipoFiltroLike.Contiene);
        }
        else
        {
            if (rfc.Equals(string.Empty))
            {
                peritosSocDS = ClientePeritosSoci.GetSocValuacionByDatosIdentificativos(null, registro);
            }
            else
            {
                peritosSocDS = ClientePeritosSoci.GetSocValuacionByDatosIdentificativos(rfc, registro);
            }

        }

        return peritosSocDS.SociedadValuacion;
    }


    /// <summary>
    /// Función que obtiene los registros de peritos por datos personales o por datos identificativos
    /// </summary>
    /// <param name="registro">Registro</param>
    /// <param name="nombre">Nombre del perito</param>
    /// <param name="apellidoPaterno">Apellido Paterno</param>
    /// <param name="apellidoMaterno">Apellido Materno</param>
    /// <param name="rfc">Rfc</param>
    /// <param name="curp">Curp</param>
    /// <param name="claveife">Clave IFE</param>
    /// <returns>Registros en DsePeritosSociedades.PeritoDataTable</returns>
    public DsePeritosSociedades.PeritoDataTable ObtenerBusquedaPeritos(string registro,string nombre,string apellidoPaterno,string apellidoMaterno,string rfc,string curp,string claveife)
    {
        if (string.IsNullOrEmpty(registro))
        {
            registro = string.Empty;
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
        if (string.IsNullOrEmpty(claveife))
        {
            claveife = string.Empty;
        }
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "REGISTRO DESC";
        }

        DsePeritosSociedades peritosDS = new DsePeritosSociedades();

        if (string.IsNullOrEmpty(registro))
        {
            peritosDS = ClientePeritosSoci.GetPeritosByDatosPersonales(nombre, ServicePeritosSociedades.TipoFiltroLike.Contiene, apellidoPaterno, ServicePeritosSociedades.TipoFiltroLike.Contiene, apellidoMaterno, ServicePeritosSociedades.TipoFiltroLike.Contiene);
        }
        else
        {
            peritosDS = ClientePeritosSoci.GetPeritosByDatosIdentificativos(rfc, curp, claveife, null, null, registro);
        }
        AnadirColunmaNombreCompleto(ref peritosDS);
        foreach (DsePeritosSociedades.PeritoRow rowPersonas in peritosDS.Perito.Rows)
        {

            if (rowPersonas.APELLIDOMATERNO == null)
            {
                rowPersonas.APELLIDOMATERNO = string.Empty;
            }
            if (rowPersonas.APELLIDOPATERNO == null)
            {
                rowPersonas.APELLIDOPATERNO = string.Empty;
            }
            if (rowPersonas.NOMBRE == null)
            {
                rowPersonas.NOMBRE = string.Empty;
            }
   
            rowPersonas[Constantes.NOMBRE_COMPLETO_COLUMN] = ComponerNombreCompleto(rowPersonas.NOMBRE, rowPersonas.APELLIDOPATERNO, rowPersonas.APELLIDOMATERNO);
        }
        return peritosDS.Perito;
    }

    /// <summary>
    /// Método que añade una columna para el nombre completo del perito a la Tabla Perito del datasetPeritosSociedades
    /// </summary>
    /// <param name="dsPeritos">DataSet al que hay que añadirle la columna</param>
    private void AnadirColunmaNombreCompleto(ref DsePeritosSociedades dsPeritos)
    {
        Type typeString = typeof(string);
        dsPeritos.Perito.Columns.Add(Constantes.NOMBRE_COMPLETO_COLUMN, typeString);
        dsPeritos.Perito.Columns[Constantes.NOMBRE_COMPLETO_COLUMN].MaxLength = dsPeritos.Perito.APELLIDOMATERNOColumn.MaxLength + dsPeritos.Perito.APELLIDOPATERNOColumn.MaxLength + dsPeritos.Perito.NOMBREColumn.MaxLength + 3;    
    }

    /// <summary>
    /// Función que devuelve el nombre completo
    /// "app2 app1, nombre"
    /// </summary>
    /// <param name="nombre">Nombre</param>
    /// <param name="app1">Apellido Paterno</param>
    /// <param name="app2">Apellido Materno</param>
    /// <returns>String con la composición</returns>
    private string ComponerNombreCompleto(string nombre, string app1, string app2)
    {
        string nombreCompleto = string.Empty;
        nombreCompleto = app2 + " " + app1 + ", " + nombre;
        return nombreCompleto;
    }

    
}
