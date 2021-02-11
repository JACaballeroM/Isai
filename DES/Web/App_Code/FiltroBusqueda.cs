using System;


/// <summary>
/// Clase encargada de gestionar los filtros de búsqueda que se establecen en las bandejas de entrada
/// </summary>
[Serializable]
public class FiltroBusqueda
{
    #region [ Campos filtro ]

    private string sortExpression;
    /// <summary>
    /// Propiedad que obtiene y almacena la expresión de de ordenación del grid
    /// </summary>
    public string SortExpression
    {
        get
        {
            return sortExpression;
        }
        set
        {
            sortExpression = value;
        }
    }


    private int indice;
    /// <summary>
    /// Propiedad que obtiene y almacena el indice de la página del grid
    /// </summary>
    public int Indice
    {
        get
        {
            return indice;
        }
        set
        {
            indice = value;
        }
    }



    private int pageSize;
    /// <summary>
    /// Propiedad que almacena y obtiene el tamaño de las páginas del grid
    /// </summary>
    public int PageSize
    {
        get
        {
            return pageSize;
        }
        set
        {
            pageSize = value;
        }
    }



    private string unidadPrivativa;
    /// <summary>
    /// Propiedad que almacena y obtiene la unidad catastral
    /// </summary>
    public string UnidadPrivativa
    {
        get
        {
            return unidadPrivativa;
        }
        set
        {
            unidadPrivativa = value;
        }
    }

    private string lote;
    /// <summary>
    /// Propiedad que almacena y obtiene el lote
    /// </summary>
    public string Lote
    {
        get
        {
            return lote;
        }
        set
        {
            lote = value;
        }
    }

    private string manzana;
    /// <summary>
    /// Propiedad que almacena y obtiene la manzana
    /// </summary>
    public string Manazana
    {
        get
        {
            return manzana;
        }
        set
        {
            manzana = value;
        }
    }


    private string region;
    /// <summary>
    /// Propiedad que almacena y obtiene la región
    /// </summary>
    public string Region
    {
        get
        {
            return region;
        }
        set
        {
            region = value;
        }
    }


    private string fechaIni;
    /// <summary>
    /// Propiedad que almacena la fecha de inicio
    /// </summary>
    public string FechaIni
    {
        get
        {
            return fechaIni;
        }
        set
        {
            fechaIni = value;
        }
    }


  
    private string fechaFin;
    /// <summary>
    /// Propiedad que almacena y obtiene la fecha fin
    /// </summary>
    public string FechaFin
    {
        get
        {
            return fechaFin;
        }
        set
        {
            fechaFin = value;
        }
    }



    private string registro;
    /// <summary>
    /// Propiedad que almacena y obtiene el registro
    /// </summary>
    public string Registro
    {
        get
        {
            return registro;
        }
        set
        {
            registro = value;
        }
    }


    private string fecha;
    /// <summary>
    /// Propiedad que almacena y obtiene la fecha
    /// </summary>
    public string Fecha
    {
        get
        {
            return fecha;
        }
        set
        {
            fecha = value;
        }
    }


    private string idSujeto;
    public string IdSujeto
    {
        get
        {
            return idSujeto;
        }
        set
        {
            idSujeto = value;
        }
    }

        private string sujeto;
    public string Sujeto
    {
        get
        {
            return sujeto;
        }
        set
        {
            sujeto = value;
        }
    }


    private string sujeto0;

    /// <summary>
    /// Propiedad que almacena y obtiene el sujeto0
    /// </summary>
    public string Sujeto0
    {
        get
        {
            return sujeto0;
        }
        set
        {
            sujeto0 = value;
        }
    }
    
    
    private string numPres;
    /// <summary>
    /// Propiedad que almacena y obtiene el número de presentación
    /// </summary>
    public string NumPres
    {
        get
        {
            return numPres;
        }
        set
        {
            numPres = value;
        }
    }


    private string codEstado;
    /// <summary>
    /// Propiedad que almacena y obtiene el código de estado
    /// </summary>
    public string CodEstado
    {
        get{
            return codEstado;
        }
        set{
            codEstado= value;
        }
    }


    private string vigencia;
    /// <summary>
    /// Propiedad que almacena y obtiene la vigencia
    /// </summary>
    public string Vigencia
    {
        get
        {
            return vigencia;
        }
        set
        {
            vigencia = value;
        }
    }


    private string idAvaluo;
    /// <summary>
    /// Propiedad que almacena y obtiene el identificador del avalúo
    /// </summary>
    public string Idavaluo
    {
        get
        {
            return idAvaluo;
        }
        set
        {
            idAvaluo = value;
        }
    }


    private string numAvaluo;
    /// <summary>
    /// Propiedad que almacena y obtiene el número del avalúo
    /// </summary>
    public string NumAvaluo
    {
        get
        {
            return numAvaluo;
        }
        set
        {
            numAvaluo = value;
        }
    }

    #endregion

    #region [ Funciones filtro ]

    /// <summary>
    /// Función que indica si el filtro seleccionado fue por fecha
    /// </summary>
    /// <returns></returns>
    public bool EsFecha()
    {
        return fecha == TipoFiltroBusqueda.Fecha.ToString();
    }

    /// <summary>
    /// Función que indica si el filtro seleccionado fue por cuenta
    /// </summary>
    /// <returns></returns>
    public bool EsCuenta()
    {
        return fecha == TipoFiltroBusqueda.CuentaCatastral.ToString();
    }

    /// <summary>
    /// Función que indica si el filtro seleccionado fue por sujeto
    /// </summary>
    /// <returns></returns>
    public bool EsSujeto()
    {
        return fecha == TipoFiltroBusqueda.Sujeto.ToString();
    }

    /// <summary>
    /// Función que indica si el filtro seleccionado fue por identificador de avalúo
    /// </summary>
    /// <returns></returns>
    public bool EsIdavaluo()
    {
        return fecha == TipoFiltroBusqueda.IdAvaluo.ToString();
    }

    /// <summary>
    /// Función que indica si el filtro seleccionado fue por número de avalúo
    /// </summary>
    /// <returns></returns>
    public bool EsNumAvaluo()
    {
        return fecha == TipoFiltroBusqueda.NumeroAvaluo.ToString();
    }
    
    /// <summary>
    /// Función que indica si el filtro seleccionado fue por número de presentación
    /// </summary>
    /// <returns></returns>
    public bool EsNumPres()
    {
        return fecha == TipoFiltroBusqueda.NumeroPresentacion.ToString();
    }


    /// <summary>
    /// Función que obtiene un string con todos los datos encadenados
    /// </summary>
    /// <returns></returns>
    public string ObtenerStringFiltro()
    {
        return fechaIni + ";" + fechaFin + ";" + region + ";" + manzana + ";" + lote + ";" + unidadPrivativa + ";" + registro + ";" + pageSize + ";" + indice + ";" + sortExpression + ";" + fecha + ";"+ idSujeto +";"+ sujeto + ";" + sujeto0 + ";" + numPres + ";" + numAvaluo + ";" + idAvaluo + ";" + vigencia + ";" + codEstado;
    }


    /// <summary>
    /// Método que rellena las propiedades de la clase con la información del parámetro 
    /// </summary>
    /// <param name="stringFiltro">Texto con los datos para las propiedades separadas por puntos y comas(;)</param>
    public void RellenarObjetoFiltro(string stringFiltro)
    {
        string[] camposFiltro = stringFiltro.Split(';');
        fechaIni = camposFiltro[0];
        fechaFin = camposFiltro[1];
        region = camposFiltro[2];
        manzana = camposFiltro[3];
        lote = camposFiltro[4];
        unidadPrivativa = camposFiltro[5];
        registro = camposFiltro[6];
        pageSize = Convert.ToInt32(camposFiltro[7]);
        indice = Convert.ToInt32(camposFiltro[8]);
        sortExpression = camposFiltro[9];
        fecha = camposFiltro[10];
        idSujeto = camposFiltro[11];
        sujeto = camposFiltro[12];
        sujeto0 = camposFiltro[13];
        numPres = camposFiltro[14];
        numAvaluo = camposFiltro[15];
        idAvaluo = camposFiltro[16];
        vigencia = camposFiltro[17];
        //bug RedirectUtil.AddParameter(Constantes.REQUEST_FILTRO, FBusqueda.ObtenerStringFiltro());
        //Add parameter concatena y deberia reemplazar
        if (camposFiltro.Length > 19)
        {
            string[] datosBuenos = camposFiltro[18].Split('?');
            if (datosBuenos != null && datosBuenos.Length > 0 && datosBuenos[0] != null)
            {
                codEstado = datosBuenos[0];
            }
            else
            {
                codEstado = "0";
            }
        }
        else
        {
            codEstado = camposFiltro[18];
        }
    }

#endregion 
}
