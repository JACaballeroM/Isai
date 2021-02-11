using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceRCON;

public partial class UserControls_DireccionControl : System.Web.UI.UserControl
{
    #region propiedades
    public decimal idDelegacion { get; set; }
    public decimal idColonia { get; set; }
    public string tipoVia { get; set; }
    public string tipoAsentamiento { get; set; }
    public string tipoLocalidad { get; set; }
    public decimal idDireccion
    {
        set
        {
            if (HttpContext.Current.Cache["iddireccion"] == null)
            {
                HttpContext.Current.Cache.Insert("iddireccion", value);
            }
            else
            {
                HttpContext.Current.Cache["iddireccion"] = value;
            }
        }
        get
        {
            try
            {
                if (HttpContext.Current.Cache["iddireccion"] == null)
                {
                    HttpContext.Current.Cache.Insert("iddireccion", -1);
                }
                return (decimal)HttpContext.Current.Cache["iddireccion"];
            }
            catch
            {
                return -1;
            }
        }
    }
    public string xml
    {
        set
        {
            if (HttpContext.Current.Cache["xml"] == null)
            {
                HttpContext.Current.Cache.Insert("xml", value);
            }
            else
            {
                HttpContext.Current.Cache["xml"] = value;
            }
        }
        get
        {
            if (HttpContext.Current.Cache["xml"] == null)
            {
                HttpContext.Current.Cache.Insert("xml", string.Empty);
            }
            return (string)HttpContext.Current.Cache["xml"];
        }
    }
    public decimal idEstado
    {
        //get
        //{
        //    return 0;//se va a regresar el valor en el ddl
        //}
        //set 
        //{
        //    CargarDdlEstado(value);
        //} 
        get;
        set;
    }

    public bool direccionCorrecta
    {
        set
        {

            if (ViewState["dircorrecta"] == null)
            {
                ViewState.Add("dircorrecta", value);
            }
            else
            {
                ViewState["dircorrecta"] = value;
            }
        }
        get
        {
            if (ViewState["dircorrecta"] == null)
            {
                ViewState.Add("dircorrecta", true);
            }
            return (bool)ViewState["dircorrecta"];
        }
    }

    public bool readOnly
    {
        set
        {
            if (HttpContext.Current.Cache["readOnly"] == null)
            {
                HttpContext.Current.Cache.Insert("readOnly", value);
            }
            else
            {
                HttpContext.Current.Cache["readOnly"] = value;
            }
        }
        get
        {
            if (HttpContext.Current.Cache["readOnly"] == null)
            {
                HttpContext.Current.Cache.Insert("readOnly", false);
            }
            return (bool)HttpContext.Current.Cache["readOnly"];
        }
    }

    public bool alta
    {
        set
        {
            if (HttpContext.Current.Cache["alta"] == null)
            {
                HttpContext.Current.Cache.Insert("alta", value);
            }
            else
            {
                HttpContext.Current.Cache["alta"] = value;
            }
        }
        get
        {
            if (HttpContext.Current.Cache["alta"] == null)
            {
                HttpContext.Current.Cache.Insert("alta", false);
            }
            return (bool)HttpContext.Current.Cache["alta"];
        }
    }

    public string textColonia { get { return txtColonia.Text; } set { setText(txtColonia, value.ToString()); } }

    public string NumExt
    { get { return txtNumExt.Text.ToUpper(); } set { setText(txtNumExt, value.ToString()); } }
    
    public string CP 
    { get { return txtCP.Text.ToUpper(); } set { setText(txtCP, value.ToString()); } }

    public string andador 
    { get { return txtAndador.Text.ToUpper(); } set { setText(txtAndador, value.ToString()); } }
    
    public string seccion 
    { get { return txtSeccion.Text.ToUpper(); } set { setText(txtSeccion, value.ToString()); } }
    
    public string edificio
    { get { return txtEdificio.Text.ToUpper(); } set { setText(txtEdificio, value.ToString()); } }
    
    public string entrada
    { get { return txtEntrada.Text.ToUpper(); } set { setText(txtEntrada, value.ToString()); } }
    
    public string numInt
    { get { return txtNumInt.Text.ToUpper(); } set { setText(txtNumInt, value.ToString()); } }
    
    public string indAdicionales
    { get { return txtIndicaciones.Text.ToUpper(); } set { setText(txtIndicaciones, value.ToString()); } }
    
    public string entreCalle1
    { get { return txtCalle1.Text.ToUpper(); } set { setText(txtCalle1, value.ToString()); } }
   
    public string entreCalle2
    { get { return txtCalle2.Text.ToUpper(); } set { setText(txtCalle2, value.ToString()); } }
    
    public string telefono
    { get { return txtTelefono.Text.ToUpper(); } set { setText(txtTelefono, value.ToString()); } }

    public string via
    { get { return txtVia.Text.ToUpper(); } set { setText(txtVia, value.ToString()); } }

    #endregion

    #region eventos
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }
    protected void btnEspecificar_Click(object sender, EventArgs e)
    {
        EspecificarDireccion();
    }

    protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            idEstado = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());
            ResetBuscarColonias();
            CargarDelegaciones(idEstado);
        }
        catch (Exception ex)
        { e.ToString(); }
    }

    protected void ddlDelegacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            decimal iddelegacion = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());
            ResetBuscarColonias();
            //CargarColonias(iddelegacion);
        }
        catch (Exception ex) { ex.ToString(); }
    }

    protected void btnBuscar_Click (object sender, EventArgs e)
    {
        BuscarColonias();
    }

    protected void btnEliminarBusqueda_Click (object sender, EventArgs e)
    {
        ResetBuscarColonias();
    }

   
    #endregion

    #region metodos

    public void Clear()
    {
        HttpContext.Current.Cache.Remove("iddireccion");
        HttpContext.Current.Cache.Remove("xml");
        ViewState.Remove("dircorrecta");
        HttpContext.Current.Cache.Remove("readOnly");
        HttpContext.Current.Cache.Remove("alta");

    }

    public void LoadControl()
    {
        CargarPagina(this.idDireccion == -1);
    }

    public void BuscarColonias()
    {
        string colonia = textColonia;
        textColonia = string.Empty;
        DesactivaBusqueda();
        CargarColonias(colonia);
    }

    public void DesactivaBusqueda()
    {
        rfvTxtColonia.Enabled = rfvColonia2.Enabled = false;
        rfvColonia.Enabled = true;
        txtColonia.Visible = false;
        btnBuscar.Visible = false;
        btnEliminarBusqueda.Visible = true;
    }

    public void CargarPagina(bool nuevo)
    {
        alta = nuevo;
        if (nuevo && string.IsNullOrEmpty(this.xml))
        {
            if (string.IsNullOrEmpty(this.xml))
            {
                CargarDdlEstado(-1);
                CargarTipoVia();
                CargarTipoLocalidad();
                cargarTipoAsentamiento();
                ResetBuscarColonias();
                this.direccionCorrecta = false;
            }
            else
            {
                CargarTipoVia();
                CargarTipoLocalidad();
                cargarTipoAsentamiento();
                CargarDireccion();
            }
        }
        else
        {
            CargarDdlEstado(-1);
            CargarTipoVia();
            CargarTipoLocalidad();
            cargarTipoAsentamiento();
            CargarDireccion();
        }
        if (readOnly)
            ReadOnly();
    }

    private void ReadOnly()
    { 
        foreach(Control control in PanelDomicilio.Controls )
        {
            if(control is TextBox )
            {
                DesactivaControl((TextBox)control);
            }
            else if (control is Button)
            {
                DesactivaControl((Button)control);
            }
            else if(control is DropDownList)
            {
                DesactivaControl((DropDownList) control);
            }
            else if (control is ImageButton)
            {
                DesactivaControl((ImageButton) control);
            }
            else if (control is Label)
            {
                DesactivaControl((Label)control);
            }
        }
    }

    private void DesactivaControl(TextBox txt)
    {
        txt.ReadOnly = true;
    }

    private void DesactivaControl(Button button)
    {
        button.Visible = false;
    }

    private void DesactivaControl(ImageButton button)
    {
        button.Visible = false;
    }

    private void DesactivaControl(DropDownList ddl)
    {
        ddl.Visible = false;
    }

    private void DesactivaControl(Label lbl)
    {
        lbl.Visible = true;
    }




    public void CargarDireccion()
    {
        if (string.IsNullOrEmpty(this.xml))
        {
            RegistroContribuyentesClient ClienteRcon = new RegistroContribuyentesClient();
            DseInfoDirecciones DseRconDomicilio = ClienteRcon.GetDireccionById(this.idDireccion);
                      
            CargarDireccion(DseRconDomicilio.Direccion);
        }
        else
        {
            DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
            tablaDireccion.ReadXml(new StringReader(xml));
            DseInfoDirecciones dseDireccionRcon = new DseInfoDirecciones();
            dseDireccionRcon.Direccion.Merge(tablaDireccion);
            CargarDireccion(dseDireccionRcon.Direccion);
        }
       
    }

    public void CargarDireccion(DseInfoDirecciones.DireccionDataTable Direccion)
    {
        if (Direccion.Count > 0)
        {
            decimal idEstado = Direccion[0].CODESTADO;
            via = Direccion[0].VIA;
            CP = Direccion[0].IsCODIGOPOSTALNull() ? string.Empty : Direccion[0].CODIGOPOSTAL;
            NumExt = Direccion[0].NUMEROEXTERIOR;
            numInt = Direccion[0].IsNUMEROINTERIORNull() ? string.Empty : Direccion[0].NUMEROINTERIOR;
            andador = Direccion[0].IsANDADORNull() ? string.Empty : Direccion[0].ANDADOR;
            edificio = Direccion[0].IsEDIFICIONull() ? string.Empty : Direccion[0].EDIFICIO;
            entrada = Direccion[0].IsENTRADANull() ? string.Empty : Direccion[0].ENTRADA;
            seccion = Direccion[0].IsSECCIONNull() ? string.Empty : Direccion[0].SECCION;
            entreCalle1 = Direccion[0].IsENTRECALLE1Null() ? string.Empty : Direccion[0].ENTRECALLE1;
            entreCalle2 = Direccion[0].IsENTRECALLE2Null() ? string.Empty : Direccion[0].ENTRECALLE2;
            telefono = Direccion[0].IsTELEFONONull() ? string.Empty : Direccion[0].TELEFONO;
            indAdicionales = Direccion[0].IsINDICACIONESADICIONALESNull() ? string.Empty : Direccion[0].INDICACIONESADICIONALES;
            CargarDdlEstado(idEstado);
            ddlEstado.SelectedValue = Direccion[0].CODESTADO.ToString();
            if (idEstado != 9)
            {
                if (!Direccion[0].IsCODMUNICIPIONull())
                    ddlDelegacion.SelectedValue = Direccion[0].CODMUNICIPIO.ToString();
                else if (!Direccion[0].IsIDDELEGACIONNull())
                    ddlDelegacion.SelectedValue = Direccion[0].IDDELEGACION.ToString();

            }
            else
            {
                if (!Direccion[0].IsIDDELEGACIONNull())
                    ddlDelegacion.SelectedValue = Direccion[0].IDDELEGACION.ToString();
            }
            ddlTipoAsentamiento.SelectedValue = Direccion[0].CODTIPOSASENTAMIENTO.ToString();
            ddlTipoVia.SelectedValue = Direccion[0].CODTIPOSVIA.ToString();
            ddlTipoLocalidad.SelectedValue = Direccion[0].IsCODTIPOSLOCALIDADNull() ? "-1" : Direccion[0].CODTIPOSLOCALIDAD.ToString();
            if (Direccion[0].IsIDCOLONIANull() || Direccion[0].IDCOLONIA == -27)
            {
                ResetBuscarColonias();
                textColonia = Direccion[0].COLONIA;
            }
            else
            {
                txtColonia.Text = Direccion[0].COLONIA;
                BuscarColonias();
                if (idEstado != 9)
                {
                    if (!Direccion[0].IsCODASENTAMIENTONull())
                        ddlColonia.SelectedValue = Direccion[0].CODASENTAMIENTO.ToString();
                        txtColonia.Text = string.Empty;
                    //Se vefirica si el valor seleccionado no es un texto vacio.
                    if (string.IsNullOrEmpty(ddlColonia.SelectedValue))
                        ResetBuscarColonias();
                        textColonia = Direccion[0].COLONIA;
                }
                else
                {
                    ddlColonia.SelectedValue = Direccion[0].IDCOLONIA.ToString();
                    txtColonia.Text = string.Empty;

                }
                
            }
            this.direccionCorrecta = true;
        }
        if (readOnly)
        {
            lblEstadoDato.Text = ddlEstado.SelectedItem.Text;
            lblDelegacionDato.Text = ddlDelegacion.SelectedItem.Text;
            lblColoniaDato.Text = Direccion[0].COLONIA;
            lblTipoAsentamientoDato.Text = Direccion[0].IsTIPOASENTAMIENTONull() ? string.Empty : Direccion[0].TIPOASENTAMIENTO;
            lblTipoViaDato.Text = Direccion[0].IsTIPOVIANull() ? string.Empty : Direccion[0].TIPOVIA;
            lblTipoLocalidadDato.Text = Direccion[0].IsTIPOLOCALIDADNull() ? "Sin especificar" : Direccion[0].TIPOLOCALIDAD;
            txtColonia.Visible = false;
        }
    }

    public void CargarDdlEstado(decimal idEstado)
    {
        try
        {
            ddlEstado.DataSource = ApplicationCache.EstadosCache;
            ddlEstado.DataValueField = "CODESTADO";
            ddlEstado.DataTextField = "ESTADO";
            ddlEstado.DataMember = "ESTADOSEPOMEX";
            ddlEstado.DataBind();
            ddlEstado.SelectedValue = "9";
            this.idEstado = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());
            CargarDelegaciones(idEstado != -1 ? idEstado : Convert.ToDecimal(ddlEstado.SelectedValue.ToString()));
        }
        catch (Exception e)
        { e.ToString();}
        //Seleccionar el elemento con id estado
    }

    public void ResetBuscarColonias()
    {
        rfvTxtColonia.Enabled = rfvColonia2.Enabled = true;
        rfvColonia.Enabled = false;
        txtColonia.Visible = true;
        btnBuscar.Visible = true;
        btnEliminarBusqueda.Visible = false;
        ddlColonia.Visible = false;
        ddlColonia.Items.Clear();
        ddlColonia.Items.Add(new ListItem("No existen resultados para mostrar.", "-1"));
    }

    public void CargarDelegaciones(decimal idEstado)
    {
        try
        {
                ddlDelegacion.Items.Clear();
               if (idEstado != -1)
                {
                    ddlDelegacion.Enabled = true;
                    try
                    {
                        if (idEstado != 9)
                        {
                            ServiceRCON.RegistroContribuyentesClient clienteRCON = new ServiceRCON.RegistroContribuyentesClient();
                            DseMunicipios dseMunicipios = clienteRCON.GetMunicipiosByEstado(idEstado);
                            clienteRCON.Close();
                            ddlDelegacion.DataSource = dseMunicipios.MunicipioSEPOMEX;
                            ddlDelegacion.DataValueField = "CODMUNICIPIO";
                            ddlDelegacion.DataTextField = "MUNICIPIO";
                            ddlDelegacion.DataBind();
                            ddlDelegacion.Items.Insert(0, new ListItem("Seleccione un municipio", "-1"));
                            ddlDelegacion.SelectedValue = "-1";
                        }
                        else
                        {
                            ddlDelegacion.DataSource = ApplicationCache.Delegacion;
                            ddlDelegacion.DataValueField = "IDDELEGACION";
                            ddlDelegacion.DataTextField = "NOMBRE";
                            ddlDelegacion.DataBind();
                            ddlDelegacion.Items.Insert(0, new ListItem("Seleccione una delegacion", "-1"));
                            ddlDelegacion.SelectedValue = "-1";
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    ddlDelegacion.Enabled = false;
                }
           
        }
        catch (Exception ex) { ex.ToString(); }
    }

    public void CargarColonias(string colonia)
    {
        try
        {
            //if (idDelegacion != -1)
            //{
            ServiceRCON.RegistroContribuyentesClient clienteRCON = new ServiceRCON.RegistroContribuyentesClient();
            DseColoniaAsentamiento dseMunicipios = clienteRCON.GetColAsentByDelegacion(Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString()), colonia);
            dseMunicipios.ColoniaAsentamiento.Merge(clienteRCON.GetColAsentByDelegacion(Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString()), colonia.ToUpper()).ColoniaAsentamiento);
            clienteRCON.Close();
            
            if (dseMunicipios.ColoniaAsentamiento.Count > 0)
            {
                ddlColonia.Items.Clear();
                ddlColonia.DataSource = dseMunicipios.ColoniaAsentamiento;
                ddlColonia.DataValueField = "CODTIPOSASENTAMIENTO";
                ddlColonia.DataTextField = "DESCRIPCION";
                ddlColonia.DataBind();
            }
            ddlColonia.Visible = true;
            //}
            //else
            //{
            //    ddlColonia.Enabled = false;
            //}
        }
        catch (Exception ex)
        { ex.ToString(); }
    }

    public void cargarTipoAsentamiento()
    {
        try
        {
            ddlTipoAsentamiento.DataSource = ApplicationCache.CatalogosRCO.CatTiposAsentamiento.Select("ACTIVO='S'");
            ddlTipoAsentamiento.DataValueField = "CODTIPOSASENTAMIENTO";
            ddlTipoAsentamiento.DataTextField = "TIPOASENTAMIENTO";
            ddlTipoAsentamiento.DataBind();
            ddlTipoAsentamiento.SelectedIndex = 0;
            CargarDefault(ddlTipoAsentamiento);
        }
        catch { ;}
    }

    public void CargarTipoVia()
    {
        try
        {
            ddlTipoVia.DataSource = ApplicationCache.CatalogosRCO.CatTiposVia.Select("ACTIVO='S'");
            ddlTipoVia.DataValueField = "CODTIPOSVIA";
            ddlTipoVia.DataTextField = "TIPOVIA";
            ddlTipoVia.DataBind();
            CargarDefault(ddlTipoVia);
        }
        catch { ;}

    }

    public void CargarTipoLocalidad()
    {
        try
        {
            ddlTipoLocalidad.DataSource = ApplicationCache.CatalogosRCO.CatTiposLocalidad;
            ddlTipoLocalidad.DataValueField = "CODTIPOSLOCALIDAD";
            ddlTipoLocalidad.DataTextField = "DESCRIPCION";
            ddlTipoLocalidad.DataBind();
            CargarDefault(ddlTipoLocalidad);
        }
        catch { ;}
    }

    private void CargarDefault(DropDownList ddl)
    {
        ddl.Items.Insert(0, new ListItem("Seleccione un tipo...", "-1"));
        ddl.SelectedValue = "-1";
    }


    public void EspecificarDireccion()
    {
        try
        {
            if (alta)
            {
                AgregarNueva();
            }
            else
            {
                Modificar();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void AgregarNueva()
    {
            StringBuilder log = new StringBuilder();

        try
        {
            log.AppendLine("AgregarNueva();");
            log.AppendLine("decimal idEstado = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());");
            decimal idEstado = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());
            //Creamos la tabla de dirección y la fila de dirección
            log.AppendLine("DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();");
            DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
            log.AppendLine("DseInfoDirecciones.DireccionRow filaDireccion = tablaDireccion.NewDireccionRow();");

            DseInfoDirecciones.DireccionRow filaDireccion = tablaDireccion.NewDireccionRow();
            log.AppendLine("if (this.idDireccion != -1)");

            //Establecemos los datos
            log.AppendLine(this.idDireccion.ToString());
            if (this.idDireccion != -1)
            {
                log.AppendLine("true: filaDireccion.IDDIRECCION = this.idDireccion;");
                filaDireccion.IDDIRECCION = this.idDireccion;
            }
            log.AppendLine("if (!ddlEstado.SelectedValue.ToString().Equals('9'))");
            if (!ddlEstado.SelectedValue.ToString().Equals("9"))
            {
                log.AppendLine("true: if (!ddlColonia.SelectedValue.ToString().Equals('-1'))");
                if (!ddlColonia.SelectedValue.ToString().Equals("-1"))
                {
                    log.AppendLine("true: filaDireccion.CODASENTAMIENTO = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());");
                    filaDireccion.CODASENTAMIENTO = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());
                }
                log.AppendLine("filaDireccion.CODMUNICIPIO = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());");
                filaDireccion.CODMUNICIPIO = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());
                log.AppendLine("filaDireccion.DELEGACION = ddlDelegacion.SelectedItem.Text;");
                filaDireccion.DELEGACION = ddlDelegacion.SelectedItem.Text;
            }
            else
            {
                log.AppendLine("false: if (!ddlColonia.SelectedValue.ToString().Equals('-1'))");
                if (!ddlColonia.SelectedValue.ToString().Equals("-1"))
                {
                    log.AppendLine("true: filaDireccion.CODASENTAMIENTO = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());");
                    filaDireccion.CODASENTAMIENTO = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());
                }
                log.AppendLine("filaDireccion.IDDELEGACION = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());");
                filaDireccion.IDDELEGACION = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());
                log.AppendLine("filaDireccion.DELEGACION = ddlDelegacion.SelectedItem.Text;");
                filaDireccion.DELEGACION = ddlDelegacion.SelectedItem.Text;
            }



            log.AppendLine("filaDireccion.ANDADOR = andador;");
            filaDireccion.ANDADOR = andador;
            log.AppendLine("filaDireccion.CODIGOPOSTAL = this.CP;");
            filaDireccion.CODIGOPOSTAL = this.CP;

            log.AppendLine("if (!ddlTipoLocalidad.SelectedValue.ToString().Equals('-1'))");
            if (!ddlTipoLocalidad.SelectedValue.ToString().Equals("-1"))
            {
                log.AppendLine("true: filaDireccion.CODTIPOSLOCALIDAD = Convert.ToDecimal(ddlTipoLocalidad.SelectedValue.ToString());");
                filaDireccion.CODTIPOSLOCALIDAD = Convert.ToDecimal(ddlTipoLocalidad.SelectedValue.ToString());
            }
            else
            {
                log.AppendLine("false: filaDireccion.SetCODTIPOSLOCALIDADNull();");
                filaDireccion.SetCODTIPOSLOCALIDADNull();
            }
            log.AppendLine("if (ddlTipoLocalidad.SelectedValue.ToString().Equals('-1'))");
            if (ddlTipoLocalidad.SelectedValue.ToString().Equals("-1"))
            {
                log.AppendLine("true: filaDireccion.SetTIPOLOCALIDADNull();");
                filaDireccion.SetTIPOLOCALIDADNull();
            }
            else
            {
                log.AppendLine("false:  filaDireccion.TIPOLOCALIDAD = ddlTipoLocalidad.SelectedItem.Text;");
                filaDireccion.TIPOLOCALIDAD = ddlTipoLocalidad.SelectedItem.Text;
            }
            log.AppendLine("filaDireccion.EDIFICIO = this.edificio;");
            filaDireccion.EDIFICIO = this.edificio;
            log.AppendLine("filaDireccion.ENTRADA = this.entrada;");
            filaDireccion.ENTRADA = this.entrada;
            log.AppendLine("filaDireccion.ENTRECALLE1 = this.entreCalle1;");
            filaDireccion.ENTRECALLE1 = this.entreCalle1;
            log.AppendLine("filaDireccion.ENTRECALLE2 = this.entreCalle2;");
            filaDireccion.ENTRECALLE2 = this.entreCalle2;

            log.AppendLine("if (txtColonia.Visible)");
            if (txtColonia.Visible)
            {
                log.AppendLine("true: filaDireccion.COLONIA = textColonia.ToUpper();");
                filaDireccion.COLONIA = textColonia.ToUpper();
                log.AppendLine("filaDireccion.SetIDCOLONIANull();");
                filaDireccion.SetIDCOLONIANull();
                log.AppendLine("filaDireccion.SetCODASENTAMIENTONull();");
                filaDireccion.SetCODASENTAMIENTONull();
            }
            else
            {
                log.AppendLine("false:filaDireccion.COLONIA = ddlColonia.SelectedItem.Text;");
                filaDireccion.COLONIA = ddlColonia.SelectedItem.Text;
                log.AppendLine("filaDireccion.SetIDCOLONIANull();");
                filaDireccion.SetIDCOLONIANull();
                log.AppendLine("filaDireccion.SetCODASENTAMIENTONull();");
                filaDireccion.SetCODASENTAMIENTONull();
                //if (!ddlColonia.SelectedValue.ToString().Equals("-1") && idEstado==9)
                //{
                //    filaDireccion.IDCOLONIA = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());
                //}
                //else
                //{
                //    filaDireccion.SetIDCOLONIANull();
                //}
            }
            log.AppendLine("filaDireccion.INDICACIONESADICIONALES = this.indAdicionales;");
            filaDireccion.INDICACIONESADICIONALES = this.indAdicionales;
            log.AppendLine("filaDireccion.NUMEROEXTERIOR = this.NumExt;");
            filaDireccion.NUMEROEXTERIOR = this.NumExt;
            log.AppendLine("filaDireccion.NUMEROINTERIOR = this.numInt;");
            filaDireccion.NUMEROINTERIOR = this.numInt;
            log.AppendLine("filaDireccion.SECCION = this.seccion;");
            filaDireccion.SECCION = this.seccion;
            log.AppendLine("filaDireccion.TELEFONO = this.telefono;");
            filaDireccion.TELEFONO = this.telefono;
            log.AppendLine("filaDireccion.CODESTADO = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());");
            filaDireccion.CODESTADO = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());
            log.AppendLine("filaDireccion.VIA = this.via;");
            filaDireccion.VIA = this.via;
            log.AppendLine("filaDireccion.SetIDVIANull();");
            filaDireccion.SetIDVIANull();
            log.AppendLine("filaDireccion.SetCODCIUDADNull();");
            filaDireccion.SetCODCIUDADNull();
            log.AppendLine("filaDireccion.SetCIUDADNull();");
            filaDireccion.SetCIUDADNull();
            log.AppendLine("filaDireccion.SetIDPERSONANull();");
            filaDireccion.SetIDPERSONANull();
            log.AppendLine("filaDireccion.SetCODTIPOSDIRECCIONNull();");
            filaDireccion.SetCODTIPOSDIRECCIONNull();
            log.AppendLine("filaDireccion.SetTIPODIRECCIONNull();");
            filaDireccion.SetTIPODIRECCIONNull();
            log.AppendLine("filaDireccion.CODTIPOSVIA = Convert.ToDecimal(ddlTipoVia.SelectedValue.ToString());");
            filaDireccion.CODTIPOSVIA = Convert.ToDecimal(ddlTipoVia.SelectedValue.ToString());
            log.AppendLine("filaDireccion.CODTIPOSASENTAMIENTO = Convert.ToDecimal(ddlTipoAsentamiento.SelectedValue.ToString());");
            filaDireccion.CODTIPOSASENTAMIENTO = Convert.ToDecimal(ddlTipoAsentamiento.SelectedValue.ToString());
            log.AppendLine("filaDireccion.TIPOASENTAMIENTO = ddlTipoAsentamiento.SelectedItem.Text;");
            filaDireccion.TIPOASENTAMIENTO = ddlTipoAsentamiento.SelectedItem.Text;
            log.AppendLine("filaDireccion.TIPOVIA = ddlTipoVia.SelectedItem.Text;");
            filaDireccion.TIPOVIA = ddlTipoVia.SelectedItem.Text;

            log.AppendLine("tablaDireccion.AddDireccionRow(filaDireccion);");
            tablaDireccion.AddDireccionRow(filaDireccion);

            //Obtiene el XML de la tabla y lo codifica a base64
            using (StringWriter sw = new StringWriter())
            {
                tablaDireccion.WriteXml(sw, System.Data.XmlWriteMode.WriteSchema, false);
                tablaDireccion.TableName = "RCON_DIRECCION";
                this.xml = sw.ToString();
            }
            this.direccionCorrecta = true;
            domicilio_CollapsiblePanelExtender.Collapsed = true;
            domicilio_CollapsiblePanelExtender.ClientState = "true";
            //Obtiene el xml codificado en base64
            //string valorEspecificado = Utilidades.base64Encode(xmlString);

            //Si todo ha ido bien ejecuta el javascript que devuelve los valores y cierra la ventana

        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            Log(log.ToString());
            throw ex;
        } 
    }

    public void Modificar()
    {
        StringBuilder log = new StringBuilder();
        try
        {
            log.AppendLine("Modificar();");

            //Creamos la tabla de dirección y la fila de dirección
            log.AppendLine("DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();");
            DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
            log.AppendLine("DseInfoDirecciones.DireccionRow filaDireccion = tablaDireccion.NewDireccionRow();");
            DseInfoDirecciones.DireccionRow filaDireccion = tablaDireccion.NewDireccionRow();

            //Establecemos los datos
            log.AppendLine(this.idDireccion.ToString());
            log.AppendLine("if (this.idDireccion != -1)");
            if (this.idDireccion != -1)
            {
                log.AppendLine("true: filaDireccion.IDDIRECCION = this.idDireccion;");
                filaDireccion.IDDIRECCION = this.idDireccion;
            }
            log.AppendLine("filaDireccion.ANDADOR = andador;");
            filaDireccion.ANDADOR = andador;
            log.AppendLine("filaDireccion.CODASENTAMIENTO = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());");
            filaDireccion.CODASENTAMIENTO = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());
            log.AppendLine("filaDireccion.CODIGOPOSTAL = this.CP;");
            filaDireccion.CODIGOPOSTAL = this.CP;

            log.AppendLine("if (!ddlTipoLocalidad.SelectedValue.ToString().Equals('-1'))");
            if (!ddlTipoLocalidad.SelectedValue.ToString().Equals("-1"))
            {
                log.AppendLine("true: filaDireccion.CODTIPOSLOCALIDAD = Convert.ToDecimal(ddlTipoLocalidad.SelectedValue.ToString());");
                filaDireccion.CODTIPOSLOCALIDAD = Convert.ToDecimal(ddlTipoLocalidad.SelectedValue.ToString());
            }
            else
            {
                log.AppendLine("filaDireccion.SetCODTIPOSLOCALIDADNull();");
                filaDireccion.SetCODTIPOSLOCALIDADNull();
            }
            log.AppendLine("if (ddlTipoLocalidad.SelectedValue.ToString().Equals('-1'))");
            if (ddlTipoLocalidad.SelectedValue.ToString().Equals("-1"))
            {
                log.AppendLine("true: filaDireccion.SetTIPOLOCALIDADNull();");
                filaDireccion.SetTIPOLOCALIDADNull();
            }
            else
            {
                log.AppendLine("false: filaDireccion.TIPOLOCALIDAD = ddlTipoLocalidad.SelectedItem.Text;");
                filaDireccion.TIPOLOCALIDAD = ddlTipoLocalidad.SelectedItem.Text;
            }
            log.AppendLine("if (!ddlEstado.SelectedValue.ToString().Equals('9'))");
            if (!ddlEstado.SelectedValue.ToString().Equals("9"))
            {
                log.AppendLine("true: filaDireccion.IDDELEGACION = -1;");
                filaDireccion.IDDELEGACION = -1;
                log.AppendLine("filaDireccion.CODMUNICIPIO = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());");
                filaDireccion.CODMUNICIPIO = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());
            }
            else
            {
                log.AppendLine("false:  filaDireccion.IDDELEGACION = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());");
                filaDireccion.IDDELEGACION = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());
                log.AppendLine("filaDireccion.SetCODMUNICIPIONull();");
                filaDireccion.SetCODMUNICIPIONull();
            }
            log.AppendLine("filaDireccion.DELEGACION = ddlDelegacion.SelectedItem.Text;");
            filaDireccion.DELEGACION = ddlDelegacion.SelectedItem.Text;
            log.AppendLine("filaDireccion.EDIFICIO = this.edificio;");
            filaDireccion.EDIFICIO = this.edificio;
            log.AppendLine("filaDireccion.ENTRADA = this.entrada;");
            filaDireccion.ENTRADA = this.entrada;
            log.AppendLine("filaDireccion.ENTRECALLE1 = this.entreCalle1;");
            filaDireccion.ENTRECALLE1 = this.entreCalle1;
            log.AppendLine("filaDireccion.ENTRECALLE2 = this.entreCalle2;");
            filaDireccion.ENTRECALLE2 = this.entreCalle2;

            log.AppendLine("if (txtColonia.Visible)");
            if (txtColonia.Visible)
            {
                log.AppendLine("true: filaDireccion.COLONIA = textColonia.ToUpper();");
                filaDireccion.COLONIA = textColonia.ToUpper();
                log.AppendLine("filaDireccion.SetIDCOLONIANull();");
                filaDireccion.SetIDCOLONIANull();
                log.AppendLine("filaDireccion.SetCODASENTAMIENTONull();");
                filaDireccion.SetCODASENTAMIENTONull();
            }
            else
            {
                log.AppendLine("false: filaDireccion.COLONIA = ddlColonia.SelectedItem.Text;");
                filaDireccion.COLONIA = ddlColonia.SelectedItem.Text;
                log.AppendLine("filaDireccion.SetIDCOLONIANull();");
                filaDireccion.SetIDCOLONIANull();
                log.AppendLine("filaDireccion.SetCODASENTAMIENTONull();");
                filaDireccion.SetCODASENTAMIENTONull();
                //if (!ddlColonia.SelectedValue.ToString().Equals("-1"))
                //{
                //    filaDireccion.IDCOLONIA = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());
                //}
                //else
                //{
                //    filaDireccion.SetIDCOLONIANull();
                //}
            }
            log.AppendLine("filaDireccion.INDICACIONESADICIONALES = this.indAdicionales;");
            filaDireccion.INDICACIONESADICIONALES = this.indAdicionales;
            log.AppendLine("filaDireccion.NUMEROEXTERIOR = this.NumExt;");
            filaDireccion.NUMEROEXTERIOR = this.NumExt;
            log.AppendLine("filaDireccion.NUMEROINTERIOR = this.numInt;");
            filaDireccion.NUMEROINTERIOR = this.numInt;
            log.AppendLine("filaDireccion.SECCION = this.seccion;");
            filaDireccion.SECCION = this.seccion;
            log.AppendLine("filaDireccion.TELEFONO = this.telefono;");
            filaDireccion.TELEFONO = this.telefono;
            log.AppendLine("filaDireccion.CODESTADO = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());");
            filaDireccion.CODESTADO = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());
            log.AppendLine("filaDireccion.VIA = this.via;");
            filaDireccion.VIA = this.via;
            log.AppendLine("filaDireccion.SetIDVIANull();");
            filaDireccion.SetIDVIANull();
            log.AppendLine("filaDireccion.SetCODCIUDADNull();");
            filaDireccion.SetCODCIUDADNull();
            log.AppendLine("filaDireccion.SetCIUDADNull();");
            filaDireccion.SetCIUDADNull();
            log.AppendLine("filaDireccion.SetIDPERSONANull();");
            filaDireccion.SetIDPERSONANull();
            log.AppendLine("filaDireccion.SetCODTIPOSDIRECCIONNull();");
            filaDireccion.SetCODTIPOSDIRECCIONNull();
            log.AppendLine("filaDireccion.SetTIPODIRECCIONNull();");
            filaDireccion.SetTIPODIRECCIONNull();
            log.AppendLine("filaDireccion.CODTIPOSVIA = Convert.ToDecimal(ddlTipoVia.SelectedValue.ToString());");
            filaDireccion.CODTIPOSVIA = Convert.ToDecimal(ddlTipoVia.SelectedValue.ToString());
            log.AppendLine("filaDireccion.CODTIPOSASENTAMIENTO = Convert.ToDecimal(ddlTipoAsentamiento.SelectedValue.ToString());");
            filaDireccion.CODTIPOSASENTAMIENTO = Convert.ToDecimal(ddlTipoAsentamiento.SelectedValue.ToString());
            log.AppendLine("filaDireccion.TIPOASENTAMIENTO = ddlTipoAsentamiento.SelectedItem.Text;");
            filaDireccion.TIPOASENTAMIENTO = ddlTipoAsentamiento.SelectedItem.Text;
            log.AppendLine("filaDireccion.TIPOVIA = ddlTipoVia.SelectedItem.Text;");
            filaDireccion.TIPOVIA = ddlTipoVia.SelectedItem.Text;

            log.AppendLine("tablaDireccion.AddDireccionRow(filaDireccion);");
            tablaDireccion.AddDireccionRow(filaDireccion);

            //Obtiene el XML de la tabla y lo codifica a base64
            using (StringWriter sw = new StringWriter())
            {
                tablaDireccion.WriteXml(sw, System.Data.XmlWriteMode.WriteSchema, false);
                tablaDireccion.TableName = "RCON_DIRECCION";
                this.xml = sw.ToString();
            }
            this.direccionCorrecta = true;
            domicilio_CollapsiblePanelExtender.Collapsed = true;
            domicilio_CollapsiblePanelExtender.ClientState = "true";
            //Obtiene el xml codificado en base64
            //string valorEspecificado = Utilidades.base64Encode(xmlString);

            //Si todo ha ido bien ejecuta el javascript que devuelve los valores y cierra la ventana

        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            Log(log.ToString());
            throw ex;
        }
    }

    //public void EspecificarDireccion()
    //{
    //    try
    //    {
    //            //Creamos la tabla de dirección y la fila de dirección
    //            DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
    //            DseInfoDirecciones.DireccionRow filaDireccion = tablaDireccion.NewDireccionRow();

    //            //Establecemos los datos
    //            if (this.idDireccion!=-1)
    //            {
    //                filaDireccion.IDDIRECCION = this.idDireccion;
    //            }
    //            filaDireccion.ANDADOR = andador;
    //            filaDireccion.CODASENTAMIENTO = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());
    //            filaDireccion.CODIGOPOSTAL = this.CP;
                
    //            if (!ddlTipoLocalidad.SelectedValue.ToString().Equals("-1"))
    //            {
    //                filaDireccion.CODTIPOSLOCALIDAD = Convert.ToDecimal(ddlTipoLocalidad.SelectedValue.ToString());
    //            }
    //            else
    //            {
    //                filaDireccion.SetCODTIPOSLOCALIDADNull();
    //            }
    //            if (ddlTipoLocalidad.SelectedValue.ToString().Equals("-1"))
    //                filaDireccion.SetTIPOLOCALIDADNull();
    //            else
    //                filaDireccion.TIPOLOCALIDAD = ddlTipoLocalidad.SelectedItem.Text;
    //            filaDireccion.IDDELEGACION = Convert.ToDecimal(ddlDelegacion.SelectedValue.ToString());
    //            filaDireccion.DELEGACION = ddlDelegacion.SelectedItem.Text;
    //            filaDireccion.EDIFICIO = this.edificio;
    //            filaDireccion.ENTRADA = this.entrada;
    //            filaDireccion.ENTRECALLE1 = this.entreCalle1;
    //            filaDireccion.ENTRECALLE2 = this.entreCalle2;

    //            if (txtColonia.Visible)
    //            {
    //                filaDireccion.COLONIA = textColonia.ToUpper();
    //                filaDireccion.SetIDCOLONIANull();
    //                filaDireccion.SetCODASENTAMIENTONull();
    //            }
    //            else
    //            {
    //                filaDireccion.COLONIA = ddlColonia.SelectedItem.Text;
    //                if (!ddlColonia.SelectedValue.ToString().Equals("-1"))
    //                {
    //                    filaDireccion.IDCOLONIA = Convert.ToDecimal(ddlColonia.SelectedValue.ToString());
    //                }
    //                else
    //                {
    //                    filaDireccion.SetIDCOLONIANull();
    //                }
    //            }
    //            filaDireccion.INDICACIONESADICIONALES = this.indAdicionales;
    //            filaDireccion.NUMEROEXTERIOR = this.NumExt;
    //            filaDireccion.NUMEROINTERIOR = this.numInt;
    //            filaDireccion.SECCION = this.seccion;
    //            filaDireccion.TELEFONO = this.telefono;
    //            filaDireccion.CODESTADO = Convert.ToDecimal(ddlEstado.SelectedValue.ToString());
    //            filaDireccion.VIA = this.via;
    //            filaDireccion.SetIDVIANull();
    //            filaDireccion.SetCODCIUDADNull();
    //            filaDireccion.SetCIUDADNull();
    //            filaDireccion.SetIDPERSONANull();
    //            filaDireccion.SetCODTIPOSDIRECCIONNull();
    //            filaDireccion.SetTIPODIRECCIONNull();
    //            filaDireccion.CODTIPOSVIA = Convert.ToDecimal(ddlTipoVia.SelectedValue.ToString());
    //            filaDireccion.CODTIPOSASENTAMIENTO = Convert.ToDecimal(ddlTipoAsentamiento.SelectedValue.ToString());
    //            filaDireccion.TIPOASENTAMIENTO = ddlTipoAsentamiento.SelectedItem.Text;
    //            filaDireccion.TIPOVIA = ddlTipoVia.SelectedItem.Text;

    //            tablaDireccion.AddDireccionRow(filaDireccion);

    //            //Obtiene el XML de la tabla y lo codifica a base64
    //            using (StringWriter sw = new StringWriter())
    //            {
    //                tablaDireccion.WriteXml(sw, System.Data.XmlWriteMode.WriteSchema, false);
    //                tablaDireccion.TableName = "RCON_DIRECCION";
    //                this.xml = sw.ToString();
    //            }
    //            this.direccionCorrecta= true;
    //            domicilio_CollapsiblePanelExtender.Collapsed = true;
    //            domicilio_CollapsiblePanelExtender.ClientState = "true";
    //            //Obtiene el xml codificado en base64
    //            //string valorEspecificado = Utilidades.base64Encode(xmlString);

    //            //Si todo ha ido bien ejecuta el javascript que devuelve los valores y cierra la ventana
           
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionPolicyWrapper.HandleException(ex);
    //    }
    //}


    private void setText(TextBox txt, string cad)
    {
        txt.Text = string.IsNullOrEmpty(cad) ? string.Empty : cad.ToUpper();
    }

    protected void txtEspDireccion_ValueChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    //Cogemos el parámetro devuelto por la ventana hija del textbox
        //    //string xml = Utilidades.base64Decode(txtEspDireccion.Text);
        //    if (!string.IsNullOrEmpty(xml))
        //    {
        //        //Recuperamos la tabla
        //        DseInfoDirecciones.DireccionDataTable tablaDireccion = new DseInfoDirecciones.DireccionDataTable();
        //        //tablaDireccion.ReadXml(new StringReader(xml));
        //        if (tablaDireccion.Any())
        //        {
        //            //hidXmlDireccion.Value = xml;
        //            //Función de javascript para abrir la especificación de dirección
        //            string url = System.Web.VirtualPathUtility.ToAbsolute(Constantes.URL_SUBISAI_ESPECIFICARDIRECCION);
        //            //iddireccion del dataset de licencia propietario.
        //            //btnActualizar.Attributes["onClick"] = string.Format("javascript:AbrirUrlDireccion('{0}','{1}','{2}','{3}','{4}'); return false; ", url, txtEspDireccion.ClientID, txtEspDireccion.Text, null, null);
        //            CargarDatosDireccion();
        //        }
        //    }
        //}
        //catch (FaultException<DeclaracionIsaiException> cex)
        //{
        //    string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
        //    MostrarMensajeInfoExcepcion(msj);
        //}
        //catch (FaultException<DeclaracionIsaiInfoException> ciex)
        //{
        //    string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
        //    MostrarMensajeInfoExcepcion(msj);
        //}
        //catch (Exception ex)
        //{
        //    ExceptionPolicyWrapper.HandleException(ex);
        //    string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
        //    MostrarMensajeInfoExcepcion(msj);
        //}
    }

    public void Log(string message)
    {
        ExceptionPolicyWrapper.HandleException(new Exception(message));
    }  

    #endregion
}