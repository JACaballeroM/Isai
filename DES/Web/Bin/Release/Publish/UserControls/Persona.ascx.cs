using System;
using System.ServiceModel;
using ServiceDeclaracionIsai;


/// <summary>
/// Clase del user control que gestiona la información de las personas
/// </summary>
public partial class ISAI_UserControls_Persona : System.Web.UI.UserControl
{
  
    /// <summary>
    /// Propiedad que obtiene y almacena el tipo de Operación
    /// </summary>
    public string TipoOperacion
    {
        get;
        set;
    }


    /// <summary>
    /// Propiedad que obtiene y almacena el nombre de la persona
    /// </summary>
    public string Nombre
    {
        get 
        { 
            return txtNombreDato.Text; 
        }
        set 
        { 
            txtNombreDato.Text = value;
            if (value.Length > 75)
            {
                lblNombreDato.Text = value.Substring(0, 72) + "...";
            }
            else
            {
                lblNombreDato.Text = value;
            }
        
        }
    }


    /// <summary>
    /// Propiedad que obtiene y almacena el apellido paterno de la persona
    /// </summary>
    public string ApellidoPaterno
    {
        get
        { 
            return txtApellidoPaternoDato.Text; 
        }
        set 
        { 
            txtApellidoPaternoDato.Text = value;
            lblApellidoPaternoDato.Text = value;
        }
    }


    /// <summary>
    /// Propiedad que obtiene y almacena el apellido materno de la persona
    /// </summary>
    public string ApellidoMaterno
    {
        get 
        { 
            return txtApellidoMaternoDato.Text; 
        }
        set
        { 
            txtApellidoMaternoDato.Text = value;
            lblApellidoMaternoDato.Text = value;
        }
    }




    /// <summary>
    /// Propiedad que obtiene y almacena el RFC de la persona
    /// </summary>
    public string Rfc
    {
        get 
        { 
            return txtRFCDato.Text.Trim(); 
        }
        set 
        { 
            txtRFCDato.Text = value;
            lblRFCDato.Text = value;
        }
    }


    /// <summary>
    /// Propiedad que obtiene y almacena el Curp de la persona
    /// </summary>
    public string Curp
    {
        get 
        {
            return txtCURPDato.Text.Trim(); 
        }
        set
        {
            txtCURPDato.Text = value;
            lblCURPDato.Text = value;
        }
    }


    /// <summary>
    /// Propiedad que obtiene y almacena la clave ife de la persona
    /// </summary>
    public string ClaveIfe
    {
        get 
        {
            return txtIFEDato.Text.Trim(); 
        }
        set 
        { 
            txtIFEDato.Text = value;
            lblIFEDato.Text = value;
        }
    }


    /// <summary>
    /// Propiedad que obtiene y almacena si el tipo de persona es fisica
    /// </summary>
    public bool PersonaFisica
    {
        get 
        { 
            return rbTipoPersonaFisica.Checked; 
        }
        set
        {
            rbTipoPersonaFisica.Checked = value;
            rbTipoPersonaMoral.Checked = !value;
            MostrarOcultarCamposTipoPersona();
        }
    }


    /// <summary>
    /// Propiedad que obtiene y almacena la actividad principal de la persona
    /// </summary>
    public string ActivPrincip
    {
        get { return txtActividadDato.Text; }
        set
        {
            txtActividadDato.Text = value;

            lblActividadDato.Text = value;

        }
    }

    /// <summary>
    /// Propiedad que obtiene y almacena si el tipo de persona es moral o no
    /// </summary>
    public bool PersonaMoral
    {
        get 
        { 
            return rbTipoPersonaMoral.Checked; 
        }
        set
        {
            rbTipoPersonaMoral.Checked = value;
            rbTipoPersonaFisica.Checked = !value;
            MostrarOcultarCamposTipoPersona();
        }

    }

   
    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">.</param>
    /// <param name="e">.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                CargarPagina();

            }
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }


    /// <summary>
    /// Método que carga y configura la página dependiendo del tipo de operación
    /// </summary>
    private void CargarPagina()
    {
        try
        {
            switch (TipoOperacion)
            {
                case Constantes.PAR_VAL_OPERACION_INS:
                    rbTipoPersonaFisica.Enabled = true;
                    rbTipoPersonaMoral.Enabled = true;
                    break;
                case Constantes.PAR_VAL_OPERACION_MOD:
                    rbTipoPersonaFisica.Enabled = true;
                    rbTipoPersonaMoral.Enabled = true;
                    LabelsVisible(false);
                    TextBoxVisible(true);
                    break;
                case Constantes.PAR_VAL_OPERACION_VER:
                    rbTipoPersonaFisica.Enabled = false;
                    rbTipoPersonaMoral.Enabled = false;
                    LabelsVisible(true);
                    TextBoxVisible(false);
                    break;
            }
            MostrarOcultarCamposTipoPersona();
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }


    /// <summary>
    /// Método que configura la visibilidad de las etiquetas del control
    /// </summary>
    /// <param name="estado">Visible/Invisible</param>
    private void LabelsVisible(bool estado)
    {
        try
        {
            lblNombreDato.Visible = estado;
            lblApellidoPaternoDato.Visible = estado;
            lblApellidoMaternoDato.Visible = estado;
            lblRFCDato.Visible = estado;
            lblCURPDato.Visible = estado;
            lblIFEDato.Visible = estado;
            lblActividadDato.Visible = estado;
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }


    /// <summary>
    /// Método que configura la visibilidad de los textbox del control
    /// </summary>
    /// <param name="estado">Visible/Invisible</param>
    private void TextBoxVisible(bool estado)
    {
        try 
	    {
            txtNombreDato.Visible = estado;
            txtApellidoPaternoDato.Visible = estado;
            txtApellidoMaternoDato.Visible = estado;
            txtRFCDato.Visible = estado;
            txtCURPDato.Visible = estado;
            txtIFEDato.Visible = estado;
            txtActividadDato.Visible = estado;
    	}
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }


    /// <summary>
    /// Método que Muestra/Oculta controles dependiendo del tipo de persona
    /// </summary>
    private void MostrarOcultarCamposTipoPersona()
    {
        try
        {
            if (rbTipoPersonaFisica.Checked != false)
            {
                lblPersonaTitulo.Text = "Detalle persona Fisica";
                trApellidos.Visible = true;
                tdActividadTitulo.Visible = false;
                tdActividadLblTxt.Visible = false;
                tdIfeTitulo.Visible = true;
                tdIfeLblTxt.Visible = true;
                rfvApPaterno.Enabled = true;
                rbTipoPersonaMoral.Checked = false;
                cvAlMenosUno.Enabled = true;
                //rfvRfc.Enabled = false;
                tdCurpLblTxt.Visible = true;
                tdCurpTitulo.Visible = true;
            }
            else
            {
                lblPersonaTitulo.Text = "Detalle persona Moral";
                trApellidos.Visible = false;
                tdIfeTitulo.Visible = false;
                tdIfeLblTxt.Visible = false;
                rfvApPaterno.Enabled = false;
                rbTipoPersonaFisica.Checked = false;
                tdCurpLblTxt.Visible = false;
                tdCurpTitulo.Visible = false;
                cvAlMenosUno.Enabled = false;
                //rfvRfc.Enabled = true;
                tdActividadTitulo.Visible = false;
                tdActividadLblTxt.Visible = false;
            }
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }
    

    /// <summary>
    /// Maneja el evento checkedChanged del control rbTipoPersonaMoral
    /// Carga la página y borrar los datos de los controles que no son comunes para los dos tipos de persona
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbTipoPersonaMoral_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CargarPagina();
            BorrarTextBoxSalvoDatosComunes();
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }

    }


    /// <summary>
    /// Borra los datos de los controles que no son comunes para los dos tipos de persona
    /// </summary>
    private void BorrarTextBoxSalvoDatosComunes()
    {
        try
        {
            txtApellidoPaternoDato.Text = "";
            txtApellidoMaternoDato.Text = "";
            txtNombreDato.Text = "";
            txtCURPDato.Text = "";
            txtIFEDato.Text = "";
            if (rbTipoPersonaFisica.Checked)
            {
                txtNombreDato.MaxLength = 50;
            }
            else
            {
                txtNombreDato.MaxLength = 100;
            }
        }
        catch (FaultException<DeclaracionIsaiException> cex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + cex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (FaultException<DeclaracionIsaiInfoException> ciex)
        {
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ciex.Detail.Descripcion;
            MostrarMensajeInfoExcepcion(msj);
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }



    #region Excepciones

    /// <summary>
    /// Pre-renderizado de la página.
    /// </summary>
    /// <param name="sender">.</param>
    /// <param name="e">.</param>
    protected void Page_Prerender(object sender, EventArgs e)
    {
        try
        {
            //Se establece el botón de cancelar para los modalpopupextenders
            mpeErrorTareas.CancelControlID = errorTareas.ClientIdCancelacion;
        }
        catch (Exception ex)
        {
            ExceptionPolicyWrapper.HandleException(ex);
            string msj = Constantes.MSJ_ERROR_OPERACION + Environment.NewLine + Environment.NewLine + ex.Message;
            MostrarMensajeInfoExcepcion(msj);
        }
    }

    /// <summary>
    /// Mostrar mensaje información excepcion.
    /// </summary>
    /// <param name="mensaje">El/La mensaje.</param>
    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion


}
