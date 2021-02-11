using System;
using System.Data;
using ServiceAltasDocumentos;
using ServiceAvaluos;
using ServiceCatastral;
using ServiceDeclaracionIsai;
using ServiceDocumentos;
using ServiceFinanzas;
using ServiceFiscalEmision;
using ServiceRCON;
using ServicePeritosSociedades;

/// <summary>
/// Clase base de ISAI
/// </summary>
public class PageBaseISAI : PageBase
{
    
    /// <summary>
    /// Propiedad que almacena y obtiene la cadena de texto con el formato de xml de la transaccionalidad con documental
    /// </summary>
    public string XmlDocumental
    {
        get
        {
            return (string)this.ViewState["XmlDocumental"];
        }
        set
        {
            this.ViewState["XmlDocumental"] = value;
        }
    }


    /// <summary>
    ///  Propiedad que almacena y obtiene el tipo de documento digital 
    /// </summary>
    public string TipoDocumental
    {
        get
        {
            return (string)this.ViewState["TipoDocumental"];
        }
        set
        {
            this.ViewState["TipoDocumental"] = value;
        }
    }


    /// <summary> 
    /// Propiedad que almacena y obtiene una lista de iddocumentos digitales
    /// </summary>
    public string ListaIdsDocumental
    {
        get
        {
            return (string)this.ViewState["ListaIdsDocumental"];
        }
        set
        {
            this.ViewState["ListaIdsDocumental"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene cadena de texto con el formato de xml de la transaccionalidad con documental
    /// </summary>
    public string XmlDocumentalBF
    {
        get
        {
            return (string)this.ViewState["XmlDocumentalBF"];
        }
        set
        {
            this.ViewState["XmlDocumentalBF"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el tipo de documento digital 
    /// </summary>
    public string TipoDocumentalBF
    {
        get
        {
            return (string)this.ViewState["TipoDocumentalBF"];
        }
        set
        {
            this.ViewState["TipoDocumentalBF"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene una lista de iddocumentos digitales
    /// </summary>
    public string ListaIdsDocumentalBF
    {
        get
        {
            return (string)this.ViewState["ListaIdsDocumentalBF"];
        }
        set
        {
            this.ViewState["ListaIdsDocumentalBF"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene una lista de iddocumentos digitales
    /// </summary>
    public string FechaCausacionAnt
    {
        get
        {
            return (ViewState["fechaCausacionAnt"] == null ? string.Empty : ViewState["fechaCausacionAnt"].ToString());
        }
        set
        {
            ViewState["fechaCausacionAnt"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el acto jurídico anterior de la declaración
    /// </summary>
    public string ActoJurAnt
    {
        get
        {
            return (ViewState["actoJurAnt"] == null ? string.Empty : ViewState["actoJurAnt"].ToString());
        }
        set
        {
            ViewState["actoJurAnt"] = value;
        }
    }

   
    /// <summary>
    /// Propiedad que almacena y obtiene la url de la página original
    /// </summary>
    public string PaginaOrigen
    {
        get
        {
            return (string)this.ViewState["PaginaOrigen"];
        }
        set
        {
            this.ViewState["PaginaOrigen"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el tipo de operación de la página (I/M/D/V)
    /// </summary>
    public string Operacion
    {
        get
        {
            return (string)this.ViewState["Operacion"];
        }
        set
        {
            this.ViewState["Operacion"] = value;
        }
    }

    
    /// <summary>
    /// Propiedad que almacena y obtiene el tipo de operación de la página (I/M/D/V)
    /// </summary>
    public string OperacionParticipantes
    {
        get
        {
            return (string)this.ViewState["OperacionParticipantes"];
        }
        set
        {
            this.ViewState["OperacionParticipantes"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene la descripción del documento
    /// </summary>
    public string Descrip
    {
        get
        {
            return (string)this.ViewState["Descrip"];
        }
        set
        {
            this.ViewState["Descrip"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene la descripción del documento de beneficios fiscales
    /// </summary>
    public string DescripBF
    {
        get
        {
            return (string)this.ViewState["DescripBF"];
        }
        set
        {
            this.ViewState["DescripBF"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el identificador de la jornada Notarial
    /// </summary>
    public decimal? CondonacionJornada
    {
        get
        {
            if (this.ViewState["CondonacionJornada"] == null)
                return null;
            else
                return Convert.ToDecimal(this.ViewState["CondonacionJornada"]);
        }
        set
        {
            this.ViewState["CondonacionJornada"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el código del acto Jurídico para el participante
    /// </summary>
    public decimal CodActoJuridicoParticipante
    {
        get
        {
            return Convert.ToDecimal(this.ViewState["CodActoJuridicoParticipante"]);
        }
        set
        {
            this.ViewState["CodActoJuridicoParticipante"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el identificador de la persona
    /// </summary>
    public int IdPersonaDeclarante
    {
        get
        {
            return Convert.ToInt32(this.ViewState["IdPersonaDeclarante"]);
        }
        set
        {
            this.ViewState["IdPersonaDeclarante"] = value;
        }
    }

  
    /// <summary>
    /// Propiedad que almacena y obtiene el código tipo de persona 
    /// </summary>
    public string CodTipoPersona
    {
        get
        {
            return this.ViewState["CodTipoPersona"].ToString();
        }
        set
        {
            this.ViewState["CodTipoPersona"] = value;
        }
    }

    /// <summary>
    /// Propiedad que almacena y obtiene el dataset de declaración DSEDECLARACIONISAI
    /// </summary>
    public DseDeclaracionIsai DseDeclaracionIsaiMant
    {
        get
        {
            if (this.ViewState["DECISAI"] == null)
            {
                DseDeclaracionIsai decIsai = new DseDeclaracionIsai();
                decIsai.EnforceConstraints = false;
                decIsai.SchemaSerializationMode = SchemaSerializationMode.ExcludeSchema;
                this.ViewState.Add("DECISAI", decIsai);
            }
            return (DseDeclaracionIsai)this.ViewState["DECISAI"];
        }
        set
        {
            if (value == null)
                this.ViewState.Remove("DECISAI");
            else
                this.ViewState["DECISAI"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el dataset de declaración DSEDECLARACIONISAIPADRE
    /// </summary>
    public DseDeclaracionIsai DseDeclaracionIsaiPadreMant
    {
        get
        {
            if (this.ViewState["DECISAIPADRE"] == null)
            {
                DseDeclaracionIsai decIsaiPadre = new DseDeclaracionIsai();
                decIsaiPadre.EnforceConstraints = false;
                decIsaiPadre.SchemaSerializationMode = SchemaSerializationMode.ExcludeSchema;
                this.ViewState.Add("DECISAIPADRE", decIsaiPadre);
            }
            return (DseDeclaracionIsai)this.ViewState["DECISAIPADRE"];
        }
        set
        {
            this.ViewState["DECISAIPADRE"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el dataset DseAvaluoConsulta
    /// </summary>
    public DseAvaluoConsulta DseAvaluo
    {
        get
        {
            if (this.ViewState["DSEAVALUO"] == null)
            {
                DseAvaluoConsulta dseAvaluo = new DseAvaluoConsulta();
                dseAvaluo.EnforceConstraints = false;
                dseAvaluo.SchemaSerializationMode = SchemaSerializationMode.ExcludeSchema;
                this.ViewState.Add("DSEAVALUO", dseAvaluo);
            }
            return (DseAvaluoConsulta)this.ViewState["DSEAVALUO"];
        }
        set
        {
            this.ViewState["DSEAVALUO"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el dataset DseInfoDirecciones
    /// </summary>
    public DseInfoDirecciones DseRconDomicilio
    {
        get
        {
            return (DseInfoDirecciones)this.ViewState["DseInfoDirecciones"];
        }
        set 
        {
            this.ViewState["DseInfoDirecciones"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el identificador del declarante anterior
    /// </summary>
    public int idDeclaranteAnt
    {
        get
        {
            return (int)ViewState["idDeclaranteAnt"];
        }
        set 
        { 
            this.ViewState["idDeclaranteAnt"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el identificador del participante
    /// </summary>
    public int IdParticipantes
    {
        get
        {
            return Convert.ToInt32(this.ViewState["IdParticipantes"]);
        }
        set
        {
            this.ViewState["IdParticipantes"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene la dirección de ordenación del grid
    /// </summary>
    public string GridViewSortDirection
    {
        get
        {
            return ViewState["SortDirection"] as string ?? "ASC";
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el identificador del avalúo seleccionado
    /// </summary>
    public string IdAvaluo
    {
        get
        {
            return (string)this.ViewState["IdAvaluo"];
        }
        set
        {
            this.ViewState["IdAvaluo"] = value;
        }
    }

    /// <summary>
    /// Propiedad que almacena y obtiene la región de una cuenta catastral
    /// </summary>
    public string Region
    {
        get
        {
            return (string)this.ViewState["Region"];
        }
        set
        {
            this.ViewState["Region"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene la manzana de una cuenta catastral
    /// </summary>
    public string Manzana
    {
        get
        {
            return (string)this.ViewState["Manzana"];
        }
        set
        {
            this.ViewState["Manzana"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene el Lote de una cuenta catastral
    /// </summary>
    public string Lote
    {
        get
        {
            return (string)this.ViewState["Lote"];
        }
        set
        {
            this.ViewState["Lote"] = value;
        }
    }


    /// <summary>
    /// Propiedad que almacena y obtiene la unidad privativa de una cuenta catastral
    /// </summary>
    public string UnidadPrivativa
    {
        get
        {
            return (string)this.ViewState["UnidadPrivativa"];
        }
        set
        {
            this.ViewState["UnidadPrivativa"] = value;
        }
    }



    /// <summary>
    /// Método que carga el dataset de declaracionISAI por completo
    /// </summary>
    /// <param name="idDeclaracion">Id de la declaración a cargar</param>
    protected void CargarDeclaracionesPorIdDeclaracion(decimal idDeclaracion)
    {
        if (DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FindByIDDECLARACION(idDeclaracion) == null)
        {
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION.Merge(ClienteDeclaracionIsai.ObtenerDeclaracionesPorIdDeclaracion(Convert.ToInt32(idDeclaracion)));

        }
    }

    
    /// <summary>
    /// Método que carga el dataset de declaracionISAI Padre por completo
    /// </summary>
    /// <param name="idDeclaracionPadre">id de la declaración padre a cargar</param>
    protected void CargarDeclaracionPadrePorIdDeclaracionPadre(decimal idDeclaracionPadre)
    {
        DseDeclaracionIsaiMant = null;
        if (idDeclaracionPadre > 0)
        {
            DseDeclaracionIsaiMant.Merge(ClienteDeclaracionIsai.ObtenerDseDeclaracionIsaiPorIdDeclaracion(idDeclaracionPadre));
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACION = -1;
            DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDDECLARACIONPADRE = idDeclaracionPadre;
       }
    }


    /// <summary>
    /// Método que carga el dataset de Avaluos 
    /// </summary>
    /// <param name="idAvaluo"></param>
    protected void CargarAvaluo(decimal idAvaluo)
    {
        DseAvaluo = ClienteAvaluo.ObtenerAvaluo(Convert.ToInt32(idAvaluo));
    }


    /// <summary>
    /// Método que carga la tabla de participantes
    /// </summary>
    /// <param name="idDeclaracion">Id de la declaración de los participantes a cargar</param>
    protected void CargarParticipantesPorIdDeclaracion(decimal idDeclaracion)
    {
        DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDT = ClienteDeclaracionIsai.ObtenerParticipantesPorIdDeclaracion(Convert.ToInt32(idDeclaracion));

        foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participantesDR in participantesDT.Rows)
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow rowP = DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.NewFEXNOT_PARTICIPANTESRow();

            rowP.PORCTRANSMISION = participantesDR.PORCTRANSMISION;
            rowP.CODTIPOPERSONA = participantesDR.CODTIPOPERSONA;
            rowP.TIPOPERSONA = participantesDR.TIPOPERSONA;
            rowP.IDPARTICIPANTES = participantesDR.IDPARTICIPANTES;
            rowP.IDPERSONA = participantesDR.IDPERSONA;
            rowP.IDDECLARACION = participantesDR.IDDECLARACION;
             if (!participantesDR.IsCODROLNull())
                rowP.CODROL = participantesDR.CODROL;
            else
                rowP.SetCODROLNull();
            if (!participantesDR.IsROLNull())
                rowP.ROL = participantesDR.ROL;
            else
                rowP.SetROLNull();
            //if (!participantesDR.IsCODACTOJURNull())
            //    rowP.CODACTOJUR = participantesDR.CODACTOJUR;
            //else
            //    rowP.SetCODACTOJURNull();
           
            if (!participantesDR.IsCODSITUACIONESPERSONANull())
                rowP.CODSITUACIONESPERSONA = participantesDR.CODSITUACIONESPERSONA;
            else
                rowP.SetCODSITUACIONESPERSONANull();
            if (!participantesDR.IsNOMBRENull())
                rowP.NOMBRE = participantesDR.NOMBRE;
            else
                rowP.SetNOMBRENull();
            if (!participantesDR.IsAPELLIDOPATERNONull())
                rowP.APELLIDOPATERNO = participantesDR.APELLIDOPATERNO;
            else
                rowP.SetAPELLIDOPATERNONull();
            if (!participantesDR.IsAPELLIDOMATERNONull())
                rowP.APELLIDOMATERNO = participantesDR.APELLIDOMATERNO;
            else
                rowP.SetAPELLIDOMATERNONull();
            if (!participantesDR.IsNOMBREAPELLIDOSNull())
                rowP.NOMBREAPELLIDOS = participantesDR.NOMBREAPELLIDOS;
            else
                rowP.SetNOMBREAPELLIDOSNull();
            if (!participantesDR.IsRFCNull())
                rowP.RFC = participantesDR.RFC;
            else
                rowP.SetRFCNull();
            if (!participantesDR.IsCURPNull())
                rowP.CURP = participantesDR.CURP;
            else
                rowP.SetCURPNull();
            if (!participantesDR.IsCLAVEIFENull())
                rowP.CLAVEIFE = participantesDR.CLAVEIFE;
            else
                rowP.SetCLAVEIFENull();
            if (!participantesDR.IsACTIVPRINCIPNull())
                rowP.ACTIVPRINCIP = participantesDR.ACTIVPRINCIP;
            else
                rowP.SetACTIVPRINCIPNull();

            DseDeclaracionIsaiMant.FEXNOT_PARTICIPANTES.AddFEXNOT_PARTICIPANTESRow(rowP);
        }
    }




    /// <summary>
    /// Para convertir el char = 1 de Oracle usado como Boolean. 1--> true
    /// </summary>
    /// <param name="s"></param>
    /// <returns>1--> True</returns>
    public bool ConvertToBooleanFromOracleCharSN(string s)
    {
        if (s.ToUpper().CompareTo("S").Equals(0)) return true;
        else return false;
    }

    #region Servicio WCF

    private DeclaracionIsaiClient clienteDeclaracionIsai = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de ISAI
    /// </summary>
    protected DeclaracionIsaiClient ClienteDeclaracionIsai
    {
        get
        {
            if (clienteDeclaracionIsai == null)
            {
                clienteDeclaracionIsai = new DeclaracionIsaiClient();
            }
            return clienteDeclaracionIsai;
        }
    }

    private AvaluosClient clienteAvaluo = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de Avalúos
    /// </summary>
    protected AvaluosClient ClienteAvaluo
    {
        get
        {
            if (clienteAvaluo == null)
            {
                clienteAvaluo = new AvaluosClient();
            }
            return clienteAvaluo;
        }
    }

    private ConsultasDocumentosClient clienteDocumentos = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de Documental
    /// </summary>
    protected ConsultasDocumentosClient ClienteDocumentos
    {
        get
        {
            if (clienteDocumentos == null)
            {
                clienteDocumentos = new ConsultasDocumentosClient();
            }
            return clienteDocumentos;
        }
    }


    private AltasDocumentosClient clienteAltaDocumentos = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de Altas de Documental
    /// </summary>
    protected AltasDocumentosClient ClienteAltaDocumentos
    {
        get
        {
            if (clienteAltaDocumentos == null)
            {
                clienteAltaDocumentos = new AltasDocumentosClient();
            }
            return clienteAltaDocumentos;
        }
    }


    private ConsultaCatastralServiceClient clienteCatastral = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de Catastral
    /// </summary>
    protected ConsultaCatastralServiceClient ClienteCatastral
    {
        get
        {
            if (clienteCatastral == null)
            {
                clienteCatastral = new ConsultaCatastralServiceClient();
            }
            return clienteCatastral;
        }
    }

    private EmisionClient clienteFiscalEmision = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de Fiscal Emisión
    /// </summary>
    protected EmisionClient ClienteFiscalEmision
    {
        get
        {
            if (clienteFiscalEmision == null)
            {
                clienteFiscalEmision = new EmisionClient();
            }
            return clienteFiscalEmision;
        }
    }

    private RegistroContribuyentesClient clienteRcon = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de RCON
    /// </summary>
    protected RegistroContribuyentesClient ClienteRcon
    {
        get
        {
            if (clienteRcon == null)
            {
                clienteRcon = new RegistroContribuyentesClient();
            }
            return clienteRcon;
        }
    }



    private PeritosSociedadesClient clientePeritosSoci = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de RCON
    /// </summary>
    protected PeritosSociedadesClient ClientePeritosSoci
    {
        get
        {
            if (clientePeritosSoci == null)
            {
                clientePeritosSoci = new PeritosSociedadesClient();
            }
            return clientePeritosSoci;
        }
    }

    private Services clienteFinanzas = null;
    /// <summary>
    /// Propiedad que obtiene el cliente del servicio de Finanzas
    /// </summary>
    protected Services ClienteFinanzas
    {
        get
        {
            if (clienteFinanzas == null)
            {
                clienteFinanzas = new Services();
            }
            return clienteFinanzas;
        }
    }

 
    /// <summary>
    /// Maneja el evento Dispose de la página
    /// Método que provaca que pasen todos los clientes de los servicios de su estado actual al estado cerrado.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Page_Dispose(object sender, System.EventArgs e)
    {
        if (clienteAvaluo != null) clienteAvaluo.Close();
        if (clienteDeclaracionIsai != null) clienteDeclaracionIsai.Close();
        if (clienteDocumentos != null) clienteDocumentos.Close();
        if (clienteAltaDocumentos != null) clienteAltaDocumentos.Close();
        if (clienteCatastral != null) clienteCatastral.Close();
        if (clienteFiscalEmision != null) clienteFiscalEmision.Close();
        if (clienteRcon != null) clienteRcon.Close();
    }


    #endregion
}
