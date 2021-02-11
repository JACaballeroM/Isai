using SIGAPred.Common.Web;


/// <summary>
/// Clase base PageBase
/// </summary>
public class PageBase : System.Web.UI.Page
{

    /// <summary>
    /// Objeto de la clase Redirect de Sigapred.Common.Web
    /// </summary>
    private Redirect redirect = new Redirect();

    /// <summary>
    /// Propiedad que implementa el patrón singelton sobre redirect.
    /// </summary>
    protected Redirect RedirectUtil
    {
        get
        {
            if (redirect == null)
            {
                redirect = new Redirect();
                redirect.EndResponse = true;
            }
            return redirect;
        }
    }


    /// <summary>
    /// Propiedad que obtiene y almacena el filtro de búsqueda de las bandejas de entrada
    /// </summary>
    public FiltroBusqueda FBusqueda
    {
        get
        {
            if (this.ViewState["fBusqueda"] == null)
                this.ViewState["fBusqueda"] = new FiltroBusqueda();
            return (FiltroBusqueda)this.ViewState["fBusqueda"];
        }
        set { this.ViewState["fBusqueda"] = value; }
    }

    /// <summary>
    /// Propiedad que obtiene y almacena dirección de la ordenación del grid de tareas
    /// </summary>
    public string SortDirectionP
    {
        get
        { 
            return (ViewState["SORTDIRECTION"] == null ? string.Empty : ViewState["SORTDIRECTION"].ToString());
        }
        set 
        { 
            ViewState["SORTDIRECTION"] = value;
        }
    }



    /// <summary>
    /// Propiedad que obtiene y almacena la expresión de ordenación del grid de tareas
    /// </summary>
    public string SortExpression
    {
        get 
        { 
            return (ViewState["SORTEXPRESION"] == null ? string.Empty : ViewState["SORTEXPRESION"].ToString());
        }
        set 
        { 
            ViewState["SORTEXPRESION"] = value; 
        }
   }


    /// <summary>
    /// Propiedad que obtiene y almacena dirección de la ordenación del grid de tareas
    /// </summary>
    public string SortDirectionP2
    {
        get 
        { 
            return (ViewState["SORTDIRECTION2"] == null ? string.Empty : ViewState["SORTDIRECTION2"].ToString()); 
        }
        set
        { 
            ViewState["SORTDIRECTION2"] = value; 
        }
    }



    /// <summary>
    /// Propiedad que obtien y almacena la expresión de ordenación del grid de tareas
    /// </summary>
    public string SortExpression2
    {
        get 
        { 
            return (ViewState["SORTEXPRESION2"] == null ? string.Empty : ViewState["SORTEXPRESION2"].ToString()); 
        }
        set 
        { 
            ViewState["SORTEXPRESION2"] = value;
        }
    }
     
}
