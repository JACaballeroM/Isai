using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyExtentions;
using System.IO;
using iTextSharp.text.pdf;
using System.Configuration;
/// <summary>
/// Descripción breve de PdfIsr
/// </summary>
public class PdfIsr
{

    //Propiedades
    string _rfcNotario;
    public string rfcNotario
    {
        get { return this._rfcNotario; } 
        
        set {
            this._rfcNotario = value;
            this.stamper.AcroFields.SetField("txtRfcNotario", this._rfcNotario);
        } 
    }

    string _curpNotario;
    public string curpNotario
    {
        get { return this._curpNotario; }
        set {
            this._curpNotario = value;
            stamper.AcroFields.SetField("txtCurpNotario", this._curpNotario);
        }
    }

    string _numNotario;
    public string numNotario { get { return this._numNotario; }
        set {
            this._numNotario = value;
            stamper.AcroFields.SetField("txtNumNotario", this._numNotario);
            
        }
    }

    string _dia;
    public string dia { get { return this._dia; }

        set {
            this._dia = value;
            stamper.AcroFields.SetField("txtDia", this._dia);
        }
    }

    string _mes;
    public string mes { get{return this._mes;}
        set{
            this._mes  = value;
            stamper.AcroFields.SetField("txtMes", this._mes);
        }
    }

    string _ano;
    public string ano {
        get{
            return this._ano;
        } 
        set{
            this._ano=value;
            stamper.AcroFields.SetField("txtAno", this._ano);
        } 
    }

    string _nombreNotario;
    public string nombreNotario { get { return this._nombreNotario; }
        set {
            this._nombreNotario = value;
            stamper.AcroFields.SetField("txtNombreNotario", this._nombreNotario);
        }
    }

    string _calleNotario;
    public string calleNotario { get { return this._calleNotario; } 
        set {
            this._calleNotario = value;
            stamper.AcroFields.SetField("txtCalleNotario", this._calleNotario);
        } 
    }

    string _delegacionNotario;
    public string delegacionNotario { get { return this._delegacionNotario; }
        set {
            this._delegacionNotario = value;
            stamper.AcroFields.SetField("txtDelegacionNotario", this._delegacionNotario);
        }
    }

    string _numExtNotario;
    public string numExtNotario { get { return this._numExtNotario; }
        set {
            this._numExtNotario = value;
            stamper.AcroFields.SetField("txtNumExtNotario", this._numExtNotario);
        }
    }

    string _coloniaNotario;
    public string coloniaNotario { get { return this._coloniaNotario; }
        set {
            this._coloniaNotario = value;
            stamper.AcroFields.SetField("txtColoniaNotario", this._coloniaNotario);
        }
    }

    string _numIntNotario;
    public string numIntNotario { get { return this._numIntNotario; }
        set {
            this._numIntNotario = value;
            stamper.AcroFields.SetField("txtNumIntNotario", this._numIntNotario);
        }
    }

    string _cpNotario;
    public string cpNotario { get { return this._cpNotario; }
        set {
            this._cpNotario = value;
            stamper.AcroFields.SetField("txtCPNotario", this._cpNotario);
        }
    }

    string _acto;
    public string acto { get { return this._acto; }
        set {
            this._acto = value;
            stamper.AcroFields.SetField("txtActo", string.Format("{0}.",this._acto.Substring(0,16)));
        }
    }

    string _numEscritura;
    public string numEscritura { get { return this._numEscritura; }
        set {
            this._numEscritura = value;
            stamper.AcroFields.SetField("txtNumEscritura", this._numEscritura);
        }
    }

    string _fechaSentencia;
    public string fechaSentencia { get { return this._fechaSentencia; }
        set {
            this._fechaSentencia = value;
            stamper.AcroFields.SetField("txtFechaSentencia", this._fechaSentencia);
        }
    }

    string _fechaConstitucion;
    public string fechaConstitucion { get { return this._fechaConstitucion; }
        set {
            this._fechaConstitucion = value;
            stamper.AcroFields.SetField("txtFechaConstitucion", this._fechaConstitucion);
        }
    }

    string _declaracion;
    public string declaracion { get { return this._declaracion; }
        set {
            this._declaracion = value;
            stamper.AcroFields.SetField("txtDeclaracion", this._declaracion);
        }
    }

    string _diaComp;
    public string diaComp { get { return _diaComp; }
        set {
            this._diaComp = value;
            stamper.AcroFields.SetField("txtDiaComp", this._diaComp);
        }
    }

    string _mesComp;
    public string mesComp { get { return this._mesComp; }
        set {
            this._mesComp = value;
            stamper.AcroFields.SetField("txtMesComp", this._mesComp);
        }
    }

    string _anoComp;
    public string anoComp { get { return this._anoComp; }
        set {
            this._anoComp = value;
            stamper.AcroFields.SetField("txtAnoComp", this._anoComp);
        }
    }

    string _numComp;
    public string numComp { get { return this._numComp; }
        set {
            this._numComp = value;
            stamper.AcroFields.SetField("txtNumComp", this._numComp);
        }
    }

    string _claveEntidad;
    public string claveEntidad { get { return this._claveEntidad; }
        set {
            this._claveEntidad = value;
            stamper.AcroFields.SetField("txtClaveEntidad", this._claveEntidad);
        }
    }

    string _a;
    public string a { get { return this._a; }
        set {
            this._a = value;
            stamper.AcroFields.SetField("txta", this._a);
        }
    }

    string _b;
    public string b { get { return this._b; } 
        set {
            this._b = value;
            stamper.AcroFields.SetField("txtb", this._b);
    } }

    string _c;
    public string c { get { return this._c; }
        set {
            this._c = value;
            stamper.AcroFields.SetField("txtc", this._c);
        }
    }

    string _d;
    public string d { get { return this._d; }
        set {
            this._d = value;
            stamper.AcroFields.SetField("txtd", this._d);
        }
    }

    string _e;
    public string e { get { return this._e; } 
        set {
            this._e = value;
            stamper.AcroFields.SetField("txte", this._e);
    } }

    string _f;
    public string f { get { return this._f; }
        set {
            this._f = value;
            stamper.AcroFields.SetField("txtf", this._f);
        }
    }

    string _g;
    public string g { get { return this._g; }
        set {
            this._g = value;
            stamper.AcroFields.SetField("txtg", this._g);
        }
    }

    string _h;
    public string h { get { return this._h; }
        set {
            this._h = value;
            stamper.AcroFields.SetField("txth", this._h);
        }
    }

    string _i;
    public string i { get { return this._i; }
        set {
            this._i = value;
            stamper.AcroFields.SetField("txti", this._i);
        }
    }

    string _j;
    public string j { get { return this._j; } 
        set { 
            this._j = value; 
            stamper.AcroFields.SetField("txtj", this._j); 
        }
    }

    string _A;
    public string A { get { return this._A; } 
        set { 
            this._A = value;
        stamper.AcroFields.SetField("txtA1", this._A);
        }
    }

    string _B;
    public string B { get { return this._B; }
        set
        {
            this._B = value;
            stamper.AcroFields.SetField("txtB1", this._B);
        }
    }

    string _C;
    public string C { get { return this._C; }
        set {
            this._C = value;
            stamper.AcroFields.SetField("txtC1", this._C);
        }
    }

    string _D;
    public string D { get { return this._D; }
        set {
            this._D = value;
            stamper.AcroFields.SetField("txtD1", this._D);
        }
    }

    string _E;
    public string E { get { return this._E; }
        set {
            this._E = value;
            stamper.AcroFields.SetField("txtE1", this._E);
        }
    }

    string _rfcEnajenante;
    public string rfcEnajenante { get { return this._rfcEnajenante; } 
        set{
            this._rfcEnajenante = value;
            stamper.AcroFields.SetField("txtRFCEnajenante", this._rfcEnajenante);
        }
    }

    string _curpEnajenante;
    public string curpEnajenante { get { return this._curpEnajenante; }
        set {
            this._curpEnajenante = value;
            stamper.AcroFields.SetField("txtCurpEnajenante", this._curpEnajenante);
        }
    }


    string _nombreEnajenante;
    public string nombreEnajenante { get { return this._nombreEnajenante; }
        set {
            this._nombreEnajenante = value;
            stamper.AcroFields.SetField("txtNombreEnajenante", this._nombreEnajenante);
        }
    }

    string _calleEnajenante;
    public string calleEnajenante { get { return this._calleEnajenante; }
        set
        {
            this._calleEnajenante = value;
            stamper.AcroFields.SetField("txtCalleEnajenante", this._calleEnajenante);
        }
    }

    string _coloniaEnajenante;
    public string coloniaEnajenante { get { return this._coloniaEnajenante; }
        set {
            this._coloniaEnajenante = value;
            stamper.AcroFields.SetField("txtColoniaEnajenante", this._coloniaEnajenante);
        }
    }

    string _estadoEnajenante;
    public string estadoEnajenante { get { return this._estadoEnajenante; }
        set {
            this._estadoEnajenante = value;
            stamper.AcroFields.SetField("txtEstadoEnajenante", this._estadoEnajenante);
        }
    }

    string _numExtEnajenante;
    public string numExtEnajenante { get { return this._numExtEnajenante; }
        set {
            this._numExtEnajenante = value;
            stamper.AcroFields.SetField("txtNumExtEnajenante", this._numExtEnajenante);
        }
    }

    string _numIntEnajenante;
    public string numIntEnajenante { get { return this._numIntEnajenante; }
        set {
            this._numIntEnajenante = value;
            stamper.AcroFields.SetField("txtNumIntEnajenante", this._numIntEnajenante);
        }
    }

    string _delegacionEnajenante;
    public string delegacionEnajenante { get { return this._delegacionEnajenante; }
        set {
            this._delegacionEnajenante = value;
            stamper.AcroFields.SetField("txtDelegacionEnajenante", this._delegacionEnajenante);
        }
    }

    string _cpEnajenante;
    public string cpEnajenante { get { return this._cpEnajenante; }
        set {
            this._cpEnajenante = value;
            stamper.AcroFields.SetField("txtCPEnajenante", this._cpEnajenante);
        }
    }

    string _telefono;
    public string telefono { get { return this._telefono; }
        set {
            this._telefono = value;
            stamper.AcroFields.SetField("txtTelefono", this._telefono);
        }
    }

    string _calleInmueble;
    public string calleInmueble { get { return this._calleInmueble; }
        set {
            this._calleInmueble = value;
            stamper.AcroFields.SetField("txtCalleInmueble", this._calleInmueble);
        }
    }

    string _coloniaInmueble;
    public string coloniaInmueble { get { return this._coloniaInmueble; }
        set {
            this._coloniaInmueble = value;
            stamper.AcroFields.SetField("txtColoniaInmueble", this._coloniaInmueble);
        }
    }

    string _numExtInmueble;
    public string numExtInmueble { get { return this._numExtEnajenante; }
        set {
            this._numExtEnajenante = value;
            stamper.AcroFields.SetField("txtNumExtInmueble", this._numExtInmueble);
        }
    }

    string _numIntInmueble;
    public string numIntInmueble { get { return this._numIntInmueble; }
        set {
            this._numIntInmueble = value;
            stamper.AcroFields.SetField("txtNumIntInmueble", this._numIntInmueble);
        }
    }

    string _municipioInmueble;
    public string municipioInmueble { get { return this._municipioInmueble; }
        set {
            this._municipioInmueble = value;
            stamper.AcroFields.SetField("txtMunicipioInmueble", this._municipioInmueble);
        }
    }

    string _region;
    public string region { get { return this._region; }
        set {
            this._region = value;
            stamper.AcroFields.SetField("txtRegion", this._region);
        }
    }

    string _manzana;
    public string manzana { get { return this._manzana; }
        set {
            this._manzana = value;
            stamper.AcroFields.SetField("txtManzana", this._manzana);
        }
    }

    string _lote;
    public string lote { get {
        return this._lote;
    }
        set {
            this._lote = value;
            stamper.AcroFields.SetField("txtLote", this._lote);
        }
    }

    string _unidad;
    public string unidad { get { return this._unidad; }
        set {
            this._unidad = value;
            stamper.AcroFields.SetField("txtUnidad", this._unidad);
        }
    }

    string _digitoVerificador;
    public string digitoVerificador { get { return this._digitoVerificador; }
        set {
            this._digitoVerificador = value;
            stamper.AcroFields.SetField("txtDigitoV", this._digitoVerificador);
        }
    }

    private string reporte { get { return ConfigurationManager.AppSettings["ReporteISR"].ToString(); } }

    private string reporteBorrador { get { return ConfigurationManager.AppSettings["ReporteBorradorISR"].ToString(); } }

    public bool pagado { get; set; }

    private string path { get {
        if (pagado)
            return HttpContext.Current.Server.MapPath("~\\DOCS\\") + reporte;
        else
            return HttpContext.Current.Server.MapPath("~\\DOCS\\") + reporteBorrador;
    } }

    private PdfStamper stamper { get; set; }

    private MemoryStream _MemoryStream { get; set; }

    PdfReader reader { get; set; }

    PdfContentByte contenido1;


    public PdfIsr(bool isPagado)
    {
        this.pagado = isPagado;
         _MemoryStream = new MemoryStream();
         reader = new PdfReader(this.path);
         stamper = new PdfStamper(reader, _MemoryStream);

    }

    public byte[] CreatePDF()
    {
        stamper.FormFlattening = true;
        stamper.SetFullCompression();
        stamper.Close();
        return _MemoryStream.ToArray();
    }


}