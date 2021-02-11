using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ServiceCatastral;
using ServiceDeclaracionIsai;
using ServiceRCON;

/// <summary>
/// Clase para gestionar la cache de la aplicación WEB
/// </summary>
public static class ApplicationCache
{
    
    /// <summary>
    /// Propiedad para controlar el acceso a DataSet de catálogos de ISAI 
    /// </summary>
    public static DseCatalogo DseCatalogoISAI
    {
        get
        {
            if (HttpContext.Current.Cache[Constantes.CACHE_ISAI_CATALOGOS] == null)
            {
                using (DeclaracionIsaiClient clienteIsai = new DeclaracionIsaiClient())
                {
                    DseCatalogo dsCatalogosISAI = clienteIsai.ObtenerCatalogos();
                    dsCatalogosISAI.SchemaSerializationMode = SchemaSerializationMode.ExcludeSchema;
                    HttpContext.Current.Cache.Insert(Constantes.CACHE_ISAI_CATALOGOS, dsCatalogosISAI);
                }
            }
            return (DseCatalogo)HttpContext.Current.Cache[Constantes.CACHE_ISAI_CATALOGOS];
        }
        set
        {
            HttpContext.Current.Cache[Constantes.CACHE_ISAI_CATALOGOS] = value;
        }
    }


    /// <summary>
    /// Propiedad para controlar el acceso a DataSet de catálogos de RCON
    /// </summary>
    public static ServiceRCON.DseCatalogos CatalogosRCO
    {
        get
        {
            if (HttpContext.Current.Cache[Constantes.CACHE_RCON_CATALOGOS] == null)
            {
                using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
                {
                    ServiceRCON.DseCatalogos catalogo = clienteRCON.GetCatalogos();
                    HttpContext.Current.Cache.Insert(Constantes.CACHE_RCON_CATALOGOS, catalogo);
                }
            }
            return (ServiceRCON.DseCatalogos)HttpContext.Current.Cache[Constantes.CACHE_RCON_CATALOGOS];
        }
    }


    /// <summary>
    /// Propiedad para controlar el acceso al catálogo de salarios mínimos
    /// </summary>
    public static List<SalarioMinimo> SalariosMinimos
    {
        get
        {
            try
            {
                if (HttpContext.Current.Cache[Constantes.CACHE_SalariosMinimos] == null)
                {
                    DseCatalogo.FEXNOT_CATSALARIOMINIMODataTable salariosDT = null;
                    using (DeclaracionIsaiClient clienteISAI = new DeclaracionIsaiClient())
                    {
                        salariosDT = clienteISAI.ObtenerCatalogoSalarioMinimos();
                    }
                    if (salariosDT != null && salariosDT.Any())
                    {
                        List<SalarioMinimo> salarios = (List<SalarioMinimo>)
                                                        (from sm
                                                         in salariosDT
                                                         select
                                                         new SalarioMinimo
                                                         {
                                                             Vigencia = sm.VIGENCIA,
                                                             Monto = sm.ZONAA
                                                         }
                                                        ).ToList();
                        HttpContext.Current.Cache.Insert(Constantes.CACHE_SalariosMinimos, salarios);
                        return salarios;
                    }
                    return new List<SalarioMinimo>();
                }
                return (List<SalarioMinimo>)HttpContext.Current.Cache[Constantes.CACHE_SalariosMinimos];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Propiedad para controlar el acceso a DataTable de delegaciones de CATASTRAL
    /// </summary>
    public static DseDelegacionColonia.DelegacionDataTable Delegacion
    {
        get
        {
            ConsultaCatastralServiceClient cliente = null;
            try
            {
                if (HttpContext.Current.Cache[Constantes.DELEGACIONES] == null)
                {
                    // instanciamos el cliente de las Delegaciones
                    cliente = new ConsultaCatastralServiceClient();

                    // obtenemos las delegaciones
                    DseDelegacionColonia.DelegacionDataTable tabla = cliente.GetDelegaciones();
                    DseDelegacionColonia.DelegacionDataTable tabla2 = new DseDelegacionColonia.DelegacionDataTable();
                    foreach (DseDelegacionColonia.DelegacionRow row in tabla)
                    {
                        if (row.IDDELEGACION < 37)
                        {
                            //row2.IDDELEGACION = row.IDDELEGACION;
                            //row2.NOMBRE = row.NOMBRE;
                            tabla2.ImportRow(row);
                        }
                        
                    }
                    // insertamos las delegaciones en la caché
                    HttpContext.Current.Cache.Insert(Constantes.DELEGACIONES, tabla2);
                }
                return (DseDelegacionColonia.DelegacionDataTable)HttpContext.Current.Cache[Constantes.DELEGACIONES];
            }
            finally
            {
                // en el caso de que se haya instanciado el cliente, lo cerramos
                if (cliente != null)
                {
                    cliente.Close();
                }
            }
        }
    }

    public static DseEstados EstadosCache
    {
        get
        {
            if (HttpContext.Current.Cache["Estados"] == null)
            {
                //Instanciamos el cliente de RCON
                RegistroContribuyentesClient cliente = new RegistroContribuyentesClient();

                //Obtenemos los estados
                DseEstados estados = cliente.GetEstados();

                cliente.Close();

                //Insertamos los estados en la caché
                HttpContext.Current.Cache.Insert("Estados", estados);
            }
            return (DseEstados)HttpContext.Current.Cache["Estados"];
        }
    }

    

}