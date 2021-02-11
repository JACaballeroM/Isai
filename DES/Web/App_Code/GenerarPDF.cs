using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using ServiceDeclaracionIsai;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using SIGAPred.Common.DigitoVerificador;   
using System.Text;
using System.Data;
using System.Configuration;
using Sigapred.DatosPDF;
using SIGAPred.Common.Extensions;
using MyExtentions;
using ServiceRCON;
/// <summary>
/// Summary description for GenerarPDF
/// </summary>
public class GenerarPDF : System.Web.UI.Page
{

    ///// <summary>
    ///// Propiedad que almacena y obtiene el dataset de declaración DSEDECLARACIONISAI
    ///// </summary>
    //public DseDeclaracionIsai DseDeclaracionIsaiMant
    //{
    //    get
    //    {
    //        if (this.ViewState["DECISAI"] == null)
    //        {
    //            DseDeclaracionIsai decIsai = new DseDeclaracionIsai();
    //            decIsai.EnforceConstraints = false;
    //            decIsai.SchemaSerializationMode = SchemaSerializationMode.ExcludeSchema;
    //            this.ViewState.Add("DECISAI", decIsai);
    //        }
    //        return (DseDeclaracionIsai)this.ViewState["DECISAI"];
    //    }
    //    set
    //    {
    //        if (value == null)
    //            this.ViewState.Remove("DECISAI");
    //        else
    //            this.ViewState["DECISAI"] = value;
    //    }
    //}

    public GenerarPDF(decimal totalactojuridico_input, decimal valoradquisicion_input, decimal valorcatastral_input, string habitacional_input, decimal par_valoravaluo_input, string regla_input, decimal condonacionJornada_input, decimal? codactojuridico_input)
	{
        totalactojuridico = totalactojuridico_input;
        valoradquisicion = valoradquisicion_input;
        valorcatastral = valorcatastral_input;
        habitacional = habitacional_input;
        par_valoravaluo = par_valoravaluo_input;
        regla = regla_input;
        CondonacionJornada = condonacionJornada_input;
        codactojuridico = codactojuridico_input;
	}


    private string PosicionesImporte(decimal importeTotal)
    {
        int Longitud = 9 - importeTotal.ToString().Length;
        string Posiciones = "";
        for (int Cont = 0; Cont < Longitud; Cont++)
            Posiciones += "0";
        return Posiciones;
    }



    private String DigitoVerificador()
    {
        Random random = new Random();
        int Digito = random.Next(1, 999);
        int Longitud = 3 - Digito.ToString().Length;
        string Posiciones = "";
        for (int Cont = 0; Cont < Longitud; Cont++)
            Posiciones += "0";
        return (Posiciones + Digito.ToString()).ToString();

    }

    private string GetAdquirente(DseDeclaracionIsai DseDeclaracionIsaiMant)
    {
        DataTable orders = DseDeclaracionIsaiMant.Tables["FEXNOT_PARTICIPANTES"];
       
        var adui = (from m in orders.AsEnumerable()
                   where m.Field<string>("ROL") == "Adquirente"
                   select m).First();

        //adui["NOMBREAPELLIDOS"].ToString();
        return adui["NOMBRE"].ToString().ToUpper() + " " + adui["APELLIDOMATERNO"].ToString().ToUpper() + " " + adui["APELLIDOPATERNO"].ToString().ToUpper();
    }


    public string cadenaveri 
    {
        get 
        {
            string _cadenaveri = string.Empty;
            switch (TipoPago)
            { 
                case TipoPago.Vigente:
                    _cadenaveri = dseDeclaracion.FEXNOT_DECLARACION[0].LINEACAPTURA.ToString() + PosicionesImporte(RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO)) + RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO).ToString() + DigitoVerificador();
                    break;
                case TipoPago.Reduccion:
                    _cadenaveri = dseDeclaracion.FEXNOT_DECLARACION[0].LINEACAPTURA.ToString() + PosicionesImporte(RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO)) + RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO).ToString() + DigitoVerificador();
                    break;
                case TipoPago.Vencido:
                    _cadenaveri = dseDeclaracion.FEXNOT_DECLARACION[0].LINEACAPTURA.ToString() + PosicionesImporte(RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO)) + RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO).ToString() + DigitoVerificador();
                    break;
                case TipoPago.VencidoReduccion:
                    _cadenaveri = dseDeclaracion.FEXNOT_DECLARACION[0].LINEACAPTURA.ToString() + PosicionesImporte(RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO)) + RedondearImporte(dseDeclaracion.FEXNOT_DECLARACION[0].IMPUESTO).ToString() + DigitoVerificador();
                    break;
            }
            return _cadenaveri;
        }
    }

    private string vigencia
    {
        get
        {
            return dseDeclaracion.FEXNOT_DECLARACION[0].FECHAVIGENCIALINEACAPTURA.ToString("dd-MM-yyyy");
        }
    }

    

    public byte[] GeneraPDFFMT(TipoPago TipoPago_input,DseDeclaracionIsai DseDeclaracionIsaiMant, decimal? valorReferido, DateTime? fechaValorReferido)
    {
        TipoPago = TipoPago_input;
        //dseDeclaracion = new DseDeclaracionIsai();
        //dseDeclaracion = (DseDeclaracionIsai)DseDeclaracionIsaiMant.Copy();
        
        //Tipo 1 == VIGENTE
        //Tipo 2 == CON REDUCCION
        //Tipo 3 == VENCIDO
        //Tipo 3 == VENCIDO CON REDUCCION


        DatosPDF pdf = new DatosPDF();
     //Tipo 1 == VIGENTE
     //Tipo 2 == CON REDUCCION
     //Tipo 3 == VENCIDO
        if (TipoPago_input != global::TipoPago.isr)
        {
            pdf.TipoPago = TipoPago_input;
            pdf.region = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION;
            pdf.manzana = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA;
            pdf.lote = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE;
            pdf.unidadPrivativa = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;
            pdf.lineaCaptura = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LINEACAPTURA;
            pdf.fechaEscritura = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAESCRITURA;
            pdf.valorReferido = valorReferido;
            pdf.fechaValorReferido = fechaValorReferido;
            //pdf.dseDeclaracion = DseDeclaracionIsaiMant;
            pdf.adquirente = pdf.GetAdquirente(DseDeclaracionIsaiMant.Tables["FEXNOT_PARTICIPANTES"]);
            pdf.fechaVigencia = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHAVIGENCIALINEACAPTURA;
            pdf.importeExencion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIMPORTEEXENCIONNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTEEXENCION;
            pdf.importeReduccion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsREDUCCIONART309Null() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REDUCCIONART309;
            pdf.porcentajeSubsidio = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO;
            pdf.porcentajeDisminucion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull() ? null : (decimal?)DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION;
            pdf.recargo = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP;
            pdf.totalactojuridico = this.totalactojuridico;
            pdf.actualizacion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsACTUALIZACIONIMPORTENull() ? 0M : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE;
            pdf.fechaCausacion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION;
            pdf.valoradquisicion = this.valoradquisicion;
            pdf.valorcatastral = this.valorcatastral;
            pdf.habitacional = this.habitacional;
            pdf.par_valoravaluo = this.par_valoravaluo;
            pdf.impuesto = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTOPAGADO;
            pdf.regla = this.regla;
            pdf.recargoCondonado = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCONDONACIONXFECHANull() ? "0.00" : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CONDONACIONXFECHA.TruncateVal(2).ToString("N");
            pdf.ImpCondo = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsCONDONACIONXFECHANull() ? 0M : DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CONDONACIONXFECHA;
            pdf.CondonacionJornada = CondonacionJornada;
            pdf.codTipoDeclaracion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString();
            pdf.codactojuridico = this.codactojuridico;
            if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDEXENCIONNull())
                pdf.exencion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDEXENCION;
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsESTASACERONull())
                pdf.estasaCero = Constantes.PAR_VAL_TRUE;
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
                pdf.importeReduccion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IDREDUCCION;
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull())
                pdf.subsidio = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO;
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull())
                pdf.disminucion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION;
            else if (!DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
                pdf.condonacion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
            else
                pdf.condonacionJornada = string.IsNullOrEmpty(pdf.CondonacionJornada.ToString()) ? null : (decimal?)pdf.CondonacionJornada;
            pdf.tipodeclaracion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].CODTIPODECLARACION;
            pdf.cadenaveri = DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().FUT.Split('|')[0];

        }
        else
        {
            //string[] DatosLC = new DeclaracionIsaiClient().GetDatosLCISR(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.FirstOrDefault().IDDECLARACION);
            

            //if (DatosLC.Count() > 0)
            //{
            //    try
            //    {
            //        pdf.TipoPago = TipoPago_input;
            //        pdf.region = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].REGION;
            //        pdf.manzana = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].MANZANA;
            //        pdf.lote = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].LOTE;
            //        pdf.unidadPrivativa = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;
            //        pdf.lineaCaptura = DatosLC[4];
            //        //pdf.dseDeclaracion = DseDeclaracionIsaiMant;
            //        pdf.adquirente = pdf.GetAdquirente(DseDeclaracionIsaiMant.Tables["FEXNOT_PARTICIPANTES"]);
            //        pdf.fechaVigencia = DatosLC[7].ToDateTime();
            //        pdf.fechaCausacion = DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].FECHACAUSACION;
            //        pdf.valoradquisicion = this.valoradquisicion;
            //        pdf.valorcatastral = this.valorcatastral;
            //        pdf.habitacional = this.habitacional;
            //        pdf.par_valoravaluo = this.par_valoravaluo;
            //        pdf.impuesto = DatosLC[2].ToDecimal();
            //        pdf.cadenaveri = DatosLC[6].Split('|')[0];
            //        using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
            //        {

            //            DseInfoContribuyente dseInfo = clienteRCON.GetInfoContribuyente(DseDeclaracionIsaiMant.FEXNOT_DECLARACION.First().IDPERSONA);
            //            if (dseInfo.Contribuyente.Any())
            //            {
            //                pdf.rfc = dseInfo.Contribuyente.First().IsRFCNull() ? string.Empty : dseInfo.Contribuyente.First().RFC;
            //            }
            //            else
            //            {
            //                pdf.rfc = string.Empty;
            //            }
                        
            //        }
                    
            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception("No se pudieron obtener los datos de el documento isr");
            //    }
            //}
            //else
            //{
            //    throw new Exception("No se pudieron obtener los datos de el documento isr");
            //}
        }

        return CreatePDF(pdf);

    }


    public byte[] CreatePDF(DatosPDF pdf)
    {
        MemoryStream _MemoryStream = new MemoryStream();
        PdfReader reader = new PdfReader(pdf.path);
        PdfStamper stamper = new PdfStamper(reader, _MemoryStream);
        //PdfContentByte contenido1;
        //decimal importetotal = 0;
        MemoryStream imageStream = new MemoryStream();

        pdf.GetImpuesto();

        //this.GetImpuesto(DseDeclaracionIsaiMant);
        if (TipoPago.Equals(TipoPago.Vigente))
        {
            if (!pdf.path.Equals(string.Empty))
            {
                foreach (string name in stamper.AcroFields.Fields.Keys)
                {
                    switch (name)
                    {
                        case "txtFechaCausacion":
                            stamper.AcroFields.SetField(name, pdf.fechaCausacion.ToString("dd-MM-yyyy"));
                            break;

                        case "txtCuenta":
                            stamper.AcroFields.SetField(name, pdf.cuenta);
                            break;

                        case "txtVigencia":
                            stamper.AcroFields.SetField(name, string.Format("VIGENCIA HASTA: {0}", pdf.vigencia));
                            break;

                        case "txtDetalle":
                            stamper.AcroFields.SetField(name, pdf.cadenaveri);
                            break;

                        case "txtImpuesto":
                            //stamper.AcroFields.SetField(name, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO.ToString("N"));
                            stamper.AcroFields.SetField(name, (pdf.importe.Truncate2()).ToString("N"));
                            break;

                        case "txtImpuesto2":
                            //stamper.AcroFields.SetField(name, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO.ToString("N"));
                            stamper.AcroFields.SetField(name, (pdf.importe.Truncate2()).ToString("N"));
                            break;

                        case "txtTotalPagar":
                            //stamper.AcroFields.SetField(name, RedondearImporte(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO).ToString("N"));
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtTotalPagar2":
                            //stamper.AcroFields.SetField(name, RedondearImporte(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO).ToString("N"));
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtConceptoCobro":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtConceptoCobro2":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtLineaCaptura":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;

                        case "txtLineaCaptura2":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;


                        case "txtQR":

                            string cuenta2 = pdf.lineaCaptura;
                            DateTime fechavalida = DateTime.Now;
                            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                            qrCodeEncoder.QRCodeScale = 2;
                            qrCodeEncoder.QRCodeVersion = 0;
                            System.Drawing.Image imagen;
                            StringBuilder datos = new StringBuilder(2000);
                            string validador = pdf.cuenta + fechavalida.ToString("dd-MM-yyyy HH:mm:ss");

                            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            byte[] data = System.Text.Encoding.ASCII.GetBytes(validador);
                            data = x.ComputeHash(data);
                            string ret = "";
                            for (int i = 0; i < data.Length; i++)
                                ret += data[i].ToString("x2").ToLower();

                            string val = ret;

                            string datas = string.Format("CONCEPTO DE COBRO: IMPUESTO SOBRE ADQUISICION DE INMUEBLES      CUENTA: {0}      FECHA CAUSACION: {7}      EXPEDICION DE LINEA DE CAPTURA: {1}      IMPUESTO: {2}      TOTAL A PAGAR: {3}      VIGENCIA: {4}      LINEA: {5}      VALIDADOR: {6} ",
                                pdf.cuenta,
                                fechavalida.ToString("dd-MM-yyyy HH:mm:ss"),
                                pdf.importe.Truncate2().toDecimalString(),
                                pdf.impuesto.ToRoundTwo().toDecimalString(),
                                pdf.vigencia,
                                pdf.lineaCaptura,
                                val.ToUpper().Substring(0, 19),
                                pdf.fechaCausacion.ToString("dd-MM-yyyy")); 
                            //datas= " EXPEDICION DE LINEA DE CAPTURA:" + fechavalida.ToString("dd-MM-yyyy HH:mm:ss") + "      " + "CUENTA:" + pdf.cuenta + " " + "TOTAL A PAGAR:" + pdf.impuesto.ToRoundTwo().ToString("N") + "                 " + "VIGENCIA:" + pdf.vigencia + "                                " + "LINEA DE CAPTURA:" + pdf.lineaCaptura + "  " + "VALIDADOR:" + val.ToUpper().Substring(0, 19) + "   " + "   " + "   " + "   ";

                            imagen = qrCodeEncoder.Encode(datas, System.Text.Encoding.UTF8);

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagen, iTextSharp.text.Color.BLACK);

                            jpg.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                            jpg.SetAbsolutePosition(215, 625);
                            jpg.ScaleAbsoluteHeight(135);
                            jpg.ScaleAbsoluteWidth(135);//
                            PdfContentByte contenido;

                            contenido = stamper.GetOverContent(1);
                            contenido.AddImage(jpg);

                            break;


                        case "txtCodigo":
                            Barcode128 code128 = new Barcode128();
                            code128.CodeType = Barcode.CODE128;
                            code128.ChecksumText = true;
                            code128.GenerateChecksum = true;
                            code128.Code = pdf.cadenaveri;
                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));

                            iTextSharp.text.Image code128a = iTextSharp.text.Image.GetInstance(bm, iTextSharp.text.Color.BLACK);
                            code128a.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                            code128a.ScaleAbsoluteHeight(60);
                            code128a.ScaleAbsoluteWidth(650);
                            code128a.SetAbsolutePosition(550, 650);

                            PdfContentByte contenidobarras;
                            contenidobarras = stamper.GetOverContent(1);
                            contenidobarras.AddImage(code128a);
                            break;

                        default:
                            break;
                    }
                }
            }

        } //Cierra opcion 1

        //gule    
        //Tipo 2 == CON REDUCCION
        if (TipoPago.Equals(TipoPago.Reduccion))
        {
            if (!pdf.path.Equals(string.Empty))
            {
                foreach (string name in stamper.AcroFields.Fields.Keys)
                {
                    switch (name)
                    {
                        case "txtFechaCausacion":
                            stamper.AcroFields.SetField(name, pdf.fechaCausacion.ToString("dd-MM-yyyy"));
                            break;

                        case "txtCuenta":
                            stamper.AcroFields.SetField(name, pdf.cuenta);
                            break;

                        case "txtVigencia":
                            stamper.AcroFields.SetField(name, string.Format("VIGENCIA HASTA: {0}", pdf.vigencia));
                            break;

                        case "txtReduccion":
                            stamper.AcroFields.SetField(name, Convert.ToDecimal(pdf.beneficios.Truncate2().ToString()).ToString("N"));
                            break;

                        case "txtReduccion2":
                            stamper.AcroFields.SetField(name, Convert.ToDecimal(pdf.beneficios.Truncate2().ToString()).ToString("N"));
                            break;

                        case "txtDetalle":
                            stamper.AcroFields.SetField(name, pdf.cadenaveri);
                            break;

                        case "txtImpuesto":

                            stamper.AcroFields.SetField(name, (pdf.importe.Truncate2()).ToString("N"));
                            break;

                        case "txtImpuesto2":
                            stamper.AcroFields.SetField(name, (pdf.importe.Truncate2()).ToString("N"));
                            break;

                        case "txtTotalPagar":
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtTotalPagar2":
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtConceptoCobro":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtConceptoCobro2":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtLineaCaptura":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;

                        case "txtLineaCaptura2":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;


                        case "txtQR":

                            string cuenta2 = pdf.lineaCaptura;
                            DateTime fechavalida = DateTime.Now;
                            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                            qrCodeEncoder.QRCodeScale = 2;
                            qrCodeEncoder.QRCodeVersion = 0;
                            System.Drawing.Image imagen;
                            StringBuilder datos = new StringBuilder(2000);
                            string validador = pdf.cuenta + fechavalida.ToString("dd-MM-yyyy HH:mm:ss");

                            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            byte[] data = System.Text.Encoding.ASCII.GetBytes(validador);
                            data = x.ComputeHash(data);
                            string ret = "";
                            for (int i = 0; i < data.Length; i++)
                                ret += data[i].ToString("x2").ToLower();

                            string val = ret;

                            string datas = string.Format("CONCEPTO DE COBRO: IMPUESTO SOBRE ADQUISICION DE INMUEBLES      CUENTA: {0}      FECHA CAUSACION: {8}      EXPEDICION DE LINEA DE CAPTURA: {1}      IMPUESTO: {2}      BENEFICIOS: {3}      TOTAL A PAGAR: {4}      VIGENCIA: {5}      LINEA: {6}      VALIDADOR: {7} ",
                                pdf.cuenta,
                                fechavalida.ToString("dd-MM-yyyy HH:mm:ss"),
                                pdf.importe.Truncate2().toDecimalString(),
                                pdf.beneficios.Truncate2().toDecimalString(),
                                pdf.impuesto.ToRoundTwo().toDecimalString(),
                                pdf.vigencia,
                                pdf.lineaCaptura,
                                val.ToUpper().Substring(0, 19),
                                pdf.fechaCausacion.ToString("dd-MM-yyyy")); 
                                //"EXPEDICION DE LINEA DE CAPTURA:" + fechavalida.ToString("dd-MM-yyyy HH:mm:ss") + "      " + "CUENTA:" + pdf.cuenta + " " + "TOTAL A PAGAR:" + pdf.impuesto.ToRoundTwo().ToString("N") + "                 " + "VIGENCIA:" + pdf.vigencia + "                                " + "LINEA DE CAPTURA:" + pdf.lineaCaptura + "  " + "VALIDADOR:" + val.ToUpper().Substring(0, 19) + "   " + "   " + "   " + "   ";

                            imagen = qrCodeEncoder.Encode(datas, System.Text.Encoding.UTF8);

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagen, iTextSharp.text.Color.BLACK);

                            jpg.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                            jpg.SetAbsolutePosition(215, 625);
                            jpg.ScaleAbsoluteHeight(135);
                            jpg.ScaleAbsoluteWidth(135);//
                            PdfContentByte contenido;

                            contenido = stamper.GetOverContent(1);
                            contenido.AddImage(jpg);

                            break;


                        case "txtCodigo":
                            Barcode128 code128 = new Barcode128();
                            code128.CodeType = Barcode.CODE128;
                            code128.ChecksumText = true;
                            code128.GenerateChecksum = true;
                            code128.Code = pdf.cadenaveri;

                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));

                            iTextSharp.text.Image code128a = iTextSharp.text.Image.GetInstance(bm, iTextSharp.text.Color.BLACK);
                            code128a.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                            code128a.ScaleAbsoluteHeight(60);
                            code128a.ScaleAbsoluteWidth(650);
                            code128a.SetAbsolutePosition(550, 650);

                            PdfContentByte contenidobarras;
                            contenidobarras = stamper.GetOverContent(1);
                            contenidobarras.AddImage(code128a);
                            break;
                        default:
                            break;
                    }
                }
            }

        } //Cierra opcion 2


        //Opcion 3 "VENCIDO"
        if (TipoPago.Equals(TipoPago.Vencido))
        {
            if (!pdf.path.Equals(string.Empty))
            {
                foreach (string name in stamper.AcroFields.Fields.Keys)
                {
                    switch (name)
                    {
                        case "txtRecargoCondonado":
                            stamper.AcroFields.SetField(name, pdf.recargoCondonado);
                            break;
                        case "txtFechaCausacion":
                            stamper.AcroFields.SetField(name, pdf.fechaCausacion.ToString("dd-MM-yyyy"));
                            break;

                        case "txtCuenta":
                            stamper.AcroFields.SetField(name, pdf.cuenta);
                            break;

                        case "txtVigencia":
                            stamper.AcroFields.SetField(name, string.Format("VIGENCIA HASTA: {0}", pdf.vigencia));
                            break;
                        case "txtRecargo":
                            stamper.AcroFields.SetField(name, (pdf.recargo.TruncateVal(2)).ToString("N"));
                            break;

                        case "txtRecargo2":
                            stamper.AcroFields.SetField(name, (pdf.recargo.TruncateVal(2)).ToString("N"));
                            break;

                        case "txtDetalle":
                            stamper.AcroFields.SetField(name, pdf.cadenaveri);
                            break;

                        case "txtImp":
                            stamper.AcroFields.SetField(name, ((pdf.importe).Truncate2()).ToString("N"));
                            break;

                        case "txtImp2":
                            stamper.AcroFields.SetField(name, ((pdf.importe).Truncate2()).ToString("N"));
                            break;

                        case "txtImpuesto":
                            stamper.AcroFields.SetField(name, ((pdf.importe + pdf.actualizacion).Truncate2()).ToString("N"));
                            break;

                        case "txtImpuesto2":
                            stamper.AcroFields.SetField(name, ((pdf.importe + pdf.actualizacion).Truncate2()).ToString("N"));
                            break;

                        case "txtTotalPagar":
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtTotalPagar2":
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtConceptoCobro":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtConceptoCobro2":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtLineaCaptura":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;

                        case "txtLineaCaptura2":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;


                        case "txtQR":

                            string cuenta2 = pdf.lineaCaptura;
                            DateTime fechavalida = DateTime.Now;
                            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                            qrCodeEncoder.QRCodeScale = 2;
                            qrCodeEncoder.QRCodeVersion = 0;
                            System.Drawing.Image imagen;
                            StringBuilder datos = new StringBuilder(2000);
                            string validador = pdf.cuenta + fechavalida.ToString("dd-MM-yyyy HH:mm:ss");

                            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            byte[] data = System.Text.Encoding.ASCII.GetBytes(validador);
                            data = x.ComputeHash(data);
                            string ret = "";
                            for (int i = 0; i < data.Length; i++)
                                ret += data[i].ToString("x2").ToLower();

                            string val = ret;

                            string datas = string.Format("CONCEPTO DE COBRO: IMPUESTO SOBRE ADQUISICION DE INMUEBLES      CUENTA: {0}      FECHA CAUSACION: {9}      EXPEDICION DE LINEA DE CAPTURA: {1}      IMPUESTO: {8}      IMPUESTO ACTUALIZADO: {2}      RECARGOS: {3}      TOTAL A PAGAR: {4}      VIGENCIA: {5}      LINEA: {6}      VALIDADOR: {7} ",
                                pdf.cuenta,
                                fechavalida.ToString("dd-MM-yyyy HH:mm:ss"),
                                (pdf.importe + pdf.actualizacion).Truncate2().toDecimalString(),
                                pdf.recargo.Truncate2().toDecimalString(),
                                pdf.impuesto.ToRoundTwo().toDecimalString(),
                                pdf.vigencia,
                                pdf.lineaCaptura,
                                val.ToUpper().Substring(0, 19),
                                pdf.importe.Truncate2().toDecimalString(),
                                pdf.fechaCausacion.ToString("dd-MM-yyyy")); 
                                //"EXPEDICION DE LINEA DE CAPTURA:" + fechavalida.ToString("dd-MM-yyyy HH:mm:ss") + "      " + "CUENTA:" + pdf.cuenta + " " + "TOTAL A PAGAR:" + pdf.impuesto.ToRoundTwo().ToString("N") + "                 " + "VIGENCIA:" + pdf.vigencia + "                                " + "LINEA DE CAPTURA:" + pdf.lineaCaptura + "  " + "VALIDADOR:" + val.ToUpper().Substring(0, 19) + "   " + "   " + "   " + "   ";

                            imagen = qrCodeEncoder.Encode(datas, System.Text.Encoding.UTF8);

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagen, iTextSharp.text.Color.BLACK);

                            jpg.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                            jpg.SetAbsolutePosition(215, 625);
                            jpg.ScaleAbsoluteHeight(135);
                            jpg.ScaleAbsoluteWidth(135);//
                            PdfContentByte contenido;

                            contenido = stamper.GetOverContent(1);
                            contenido.AddImage(jpg);

                            break;


                        case "txtCodigo":


                            Barcode128 code128 = new Barcode128();
                            code128.CodeType = Barcode.CODE128;
                            code128.ChecksumText = true;
                            code128.GenerateChecksum = true;


                            code128.Code = pdf.cadenaveri;

                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));

                            iTextSharp.text.Image code128a = iTextSharp.text.Image.GetInstance(bm, iTextSharp.text.Color.BLACK);
                            code128a.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                            code128a.ScaleAbsoluteHeight(60);
                            code128a.ScaleAbsoluteWidth(650);
                            code128a.SetAbsolutePosition(550, 650);

                            PdfContentByte contenidobarras;
                            contenidobarras = stamper.GetOverContent(1);
                            contenidobarras.AddImage(code128a);


                            break;


                        default:
                            break;
                    }
                }
            }

        }//Cierra opcion 3

        //Opcion 3 "VENCIDO CON REDUCCION"
        if (TipoPago.Equals(TipoPago.VencidoReduccion))
        {
            if (!pdf.path.Equals(string.Empty))
            {
                foreach (string name in stamper.AcroFields.Fields.Keys)
                {
                    switch (name)
                    {
                        case "txtRecargoCondonado":
                            stamper.AcroFields.SetField(name, pdf.recargoCondonado);
                            break;
                        case "txtFechaCausacion":
                            stamper.AcroFields.SetField(name, pdf.fechaCausacion.ToString("dd-MM-yyyy"));
                            break;

                        case "txtCuenta":
                            stamper.AcroFields.SetField(name, pdf.cuenta);
                            break;

                        case "txtVigencia":
                            stamper.AcroFields.SetField(name, string.Format("VIGENCIA HASTA: {0}", pdf.vigencia));
                            break;
                        case "txtReduccion":
                            stamper.AcroFields.SetField(name, Convert.ToDecimal(pdf.beneficios.Truncate2().ToString()).ToString("N"));
                            break;

                        case "txtReduccion2":
                            stamper.AcroFields.SetField(name, Convert.ToDecimal(pdf.beneficios.Truncate2().ToString()).ToString("N"));
                            break;

                        case "txtRecargo":
                            stamper.AcroFields.SetField(name, (pdf.recargo.TruncateVal(2)).ToString("N"));
                            break;

                        case "txtRecargo2":
                            stamper.AcroFields.SetField(name, (pdf.recargo.TruncateVal(2)).ToString("N"));
                            break;

                        case "txtDetalle":
                            stamper.AcroFields.SetField(name, pdf.cadenaveri);
                            break;

                        case "txtImp":
                            stamper.AcroFields.SetField(name, ((pdf.importe).Truncate2()).ToString("N"));
                            break;

                        case "txtImp2":
                            stamper.AcroFields.SetField(name, ((pdf.importe).Truncate2()).ToString("N"));
                            break;

                        case "txtImpuesto":
                            stamper.AcroFields.SetField(name, ((pdf.importe + pdf.actualizacion).Truncate2()).ToString("N"));
                            break;

                        case "txtImpuesto2":
                            stamper.AcroFields.SetField(name, ((pdf.importe + pdf.actualizacion).Truncate2()).ToString("N"));
                            break;

                        case "txtTotalPagar":
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToRoundTwo().ToString("N"));
                            break;

                        case "txtTotalPagar2":
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToRoundTwo().ToString("N"));
                            break;

                        case "txtConceptoCobro":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtConceptoCobro2":
                            stamper.AcroFields.SetField(name, "IMPUESTO SOBRE ADQUISICIÓN DE INMUEBLES");
                            break;

                        case "txtLineaCaptura":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;

                        case "txtLineaCaptura2":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;


                        case "txtQR":

                            string cuenta2 = pdf.lineaCaptura;
                            DateTime fechavalida = DateTime.Now;
                            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                            qrCodeEncoder.QRCodeScale = 2;
                            qrCodeEncoder.QRCodeVersion = 0;
                            System.Drawing.Image imagen;
                            StringBuilder datos = new StringBuilder(2000);
                            string validador = pdf.cuenta + fechavalida.ToString("dd-MM-yyyy HH:mm:ss");

                            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            byte[] data = System.Text.Encoding.ASCII.GetBytes(validador);
                            data = x.ComputeHash(data);
                            string ret = "";
                            for (int i = 0; i < data.Length; i++)
                                ret += data[i].ToString("x2").ToLower();

                            string val = ret;

                            string datas = string.Format("CONCEPTO DE COBRO: IMPUESTO SOBRE ADQUISICION DE INMUEBLES      CUENTA: {0}      FECHA CAUSACION: {10}      EXPEDICION DE LINEA DE CAPTURA: {1}      IMPUESTO: {9}      IMPUESTO ACTUALIZADO: {2}      RECARGOS: {3}      BENEFICIOS: {4}      TOTAL A PAGAR: {5}      VIGENCIA: {6}      LINEA: {7}      VALIDADOR: {8} ",
                                pdf.cuenta,
                                fechavalida.ToString("dd-MM-yyyy HH:mm:ss"),
                                (pdf.importe + pdf.actualizacion).Truncate2().toDecimalString(),
                                pdf.recargo.Truncate2().toDecimalString(),
                                pdf.beneficios.Truncate2().toDecimalString(),
                                pdf.impuesto.ToRoundTwo().toDecimalString(),
                                pdf.vigencia,
                                pdf.lineaCaptura,
                                val.ToUpper().Substring(0, 19),
                                pdf.importe.Truncate2().toDecimalString(),
                                pdf.fechaCausacion.ToString("dd-MM-yyyy")); 
                                //"EXPEDICION DE LINEA DE CAPTURA:" + fechavalida.ToString("dd-MM-yyyy HH:mm:ss") + "      " + "CUENTA:" + pdf.cuenta + " " + "TOTAL A PAGAR:" + pdf.impuesto + "                 " + "VIGENCIA:" + pdf.vigencia + "                                " + "LINEA DE CAPTURA:" + pdf.lineaCaptura + "  " + "VALIDADOR:" + val.ToUpper().Substring(0, 19) + "   " + "   " + "   " + "   ";

                            imagen = qrCodeEncoder.Encode(datas, System.Text.Encoding.UTF8);

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagen, iTextSharp.text.Color.BLACK);

                            jpg.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                            jpg.SetAbsolutePosition(215, 625);
                            jpg.ScaleAbsoluteHeight(135);
                            jpg.ScaleAbsoluteWidth(135);//
                            PdfContentByte contenido;

                            contenido = stamper.GetOverContent(1);
                            contenido.AddImage(jpg);

                            break;


                        case "txtCodigo":


                            Barcode128 code128 = new Barcode128();
                            code128.CodeType = Barcode.CODE128;
                            code128.ChecksumText = true;
                            code128.GenerateChecksum = true;


                            code128.Code = pdf.cadenaveri;

                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));

                            iTextSharp.text.Image code128a = iTextSharp.text.Image.GetInstance(bm, iTextSharp.text.Color.BLACK);
                            code128a.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                            code128a.ScaleAbsoluteHeight(60);
                            code128a.ScaleAbsoluteWidth(650);
                            code128a.SetAbsolutePosition(550, 650);

                            PdfContentByte contenidobarras;
                            contenidobarras = stamper.GetOverContent(1);
                            contenidobarras.AddImage(code128a);


                            break;


                        default:
                            break;
                    }
                }
            }

        }//Cierra onpcion 4
        if (TipoPago.Equals(TipoPago.isr))
        {
            if (!pdf.path.Equals(string.Empty))
            {
                foreach (string name in stamper.AcroFields.Fields.Keys)
                {
                    switch (name)
                    {
                        case "txtFechaCausacion":
                            stamper.AcroFields.SetField(name, pdf.fechaCausacion.ToString("dd-MM-yyyy"));
                            break;

                        case "txtCuenta":
                            stamper.AcroFields.SetField(name, pdf.cuenta);
                            break;

                        case "txtVigencia":
                            stamper.AcroFields.SetField(name, string.Format("VIGENCIA HASTA: {0}", pdf.vigencia));
                            break;

                        case "txtDetalle":
                            stamper.AcroFields.SetField(name, pdf.cadenaveri);
                            break;

                        case "txtImpuesto":
                            //stamper.AcroFields.SetField(name, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO.ToString("N"));
                            stamper.AcroFields.SetField(name, (pdf.importe.Truncate2()).ToString("N"));
                            break;

                        case "txtImpuesto2":
                            //stamper.AcroFields.SetField(name, DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO.ToString("N"));
                            stamper.AcroFields.SetField(name, (pdf.importe.Truncate2()).ToString("N"));
                            break;

                        case "txtTotalPagar":
                            //stamper.AcroFields.SetField(name, RedondearImporte(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO).ToString("N"));
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtTotalPagar2":
                            //stamper.AcroFields.SetField(name, RedondearImporte(DseDeclaracionIsaiMant.FEXNOT_DECLARACION[0].IMPUESTO).ToString("N"));
                            stamper.AcroFields.SetField(name, pdf.impuesto.ToRoundTwo().ToString("N"));
                            break;

                        case "txtConceptoCobro":
                            stamper.AcroFields.SetField(name, "ISR ENAJENACIÓN DE BIENES INMUEBLES");
                            break;

                        case "txtConceptoCobro2":
                            stamper.AcroFields.SetField(name, "ISR ENAJENACIÓN DE BIENES INMUEBLES");
                            break;

                        case "txtLineaCaptura":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;

                        case "txtLineaCaptura2":
                            stamper.AcroFields.SetField(name, pdf.lineaCaptura);
                            break;


                        case "txtQR":

                            string cuenta2 = pdf.lineaCaptura;
                            DateTime fechavalida = DateTime.Now;
                            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                            qrCodeEncoder.QRCodeScale = 2;
                            qrCodeEncoder.QRCodeVersion = 0;
                            System.Drawing.Image imagen;
                            StringBuilder datos = new StringBuilder(2000);
                            string validador = pdf.cuenta + fechavalida.ToString("dd-MM-yyyy HH:mm:ss");

                            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            byte[] data = System.Text.Encoding.ASCII.GetBytes(validador);
                            data = x.ComputeHash(data);
                            string ret = "";
                            for (int i = 0; i < data.Length; i++)
                                ret += data[i].ToString("x2").ToLower();

                            string val = ret;

                            string datas = string.Format("CONCEPTO DE COBRO: ISR ENAJENACIÓN DE BIENES INMUEBLES     CUENTA: {0}      FECHA CAUSACION: {7}      EXPEDICION DE LINEA DE CAPTURA: {1}      IMPUESTO: {2}      TOTAL A PAGAR: {3}      VIGENCIA: {4}      LINEA: {5}      VALIDADOR: {6} ",
                                pdf.cuenta,
                                fechavalida.ToString("dd-MM-yyyy HH:mm:ss"),
                                pdf.importe.Truncate2().toDecimalString(),
                                pdf.impuesto.ToRoundTwo().toDecimalString(),
                                pdf.vigencia,
                                pdf.lineaCaptura,
                                val.ToUpper().Substring(0, 19),
                                pdf.fechaCausacion.ToString("dd-MM-yyyy"));
                            //datas= " EXPEDICION DE LINEA DE CAPTURA:" + fechavalida.ToString("dd-MM-yyyy HH:mm:ss") + "      " + "CUENTA:" + pdf.cuenta + " " + "TOTAL A PAGAR:" + pdf.impuesto.ToRoundTwo().ToString("N") + "                 " + "VIGENCIA:" + pdf.vigencia + "                                " + "LINEA DE CAPTURA:" + pdf.lineaCaptura + "  " + "VALIDADOR:" + val.ToUpper().Substring(0, 19) + "   " + "   " + "   " + "   ";

                            imagen = qrCodeEncoder.Encode(datas, System.Text.Encoding.UTF8);

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagen, iTextSharp.text.Color.BLACK);

                            jpg.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                            jpg.SetAbsolutePosition(215, 625);
                            jpg.ScaleAbsoluteHeight(135);
                            jpg.ScaleAbsoluteWidth(135);//
                            PdfContentByte contenido;

                            contenido = stamper.GetOverContent(1);
                            contenido.AddImage(jpg);

                            break;


                        case "txtCodigo":
                            Barcode128 code128 = new Barcode128();
                            code128.CodeType = Barcode.CODE128;
                            code128.ChecksumText = true;
                            code128.GenerateChecksum = true;
                            code128.Code = pdf.cadenaveri;
                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));

                            iTextSharp.text.Image code128a = iTextSharp.text.Image.GetInstance(bm, iTextSharp.text.Color.BLACK);
                            code128a.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                            code128a.ScaleAbsoluteHeight(60);
                            code128a.ScaleAbsoluteWidth(650);
                            code128a.SetAbsolutePosition(550, 650);

                            PdfContentByte contenidobarras;
                            contenidobarras = stamper.GetOverContent(1);
                            contenidobarras.AddImage(code128a);
                            break;

                        case "txtRFC":
                            stamper.AcroFields.SetField(name, string.Format("RFC: {0}", pdf.rfc) );
                            break;

                        default:
                            break;
                    }
                }
            }

        }

        stamper.FormFlattening = true;
        stamper.SetFullCompression();
        //contenido1.EndText();

        stamper.Close();

        return _MemoryStream.ToArray();
    }

    public static Decimal RedondearImporte(Decimal impTotal)
    {
        // Nos quedamos con los dos primeros dígitos de la parte decimal.
        string auxImpDec = impTotal.ToString("F2");
        Int32 auxParteDecimal = Convert.ToInt32(auxImpDec.Substring(auxImpDec.IndexOf(".") + 1, 2));

        Decimal auxImporte = 0;

        // Si la parte decimal en menor de 50 
        if (auxParteDecimal <= 50)
        {
            auxImporte = Decimal.Truncate(impTotal);
        }
        // Si la parte decimal es mayor o igual que 51
        else
        {
            auxImporte = Decimal.Truncate(impTotal) + 1;
        }
        return auxImporte;
    }

    private decimal porcentajeReduccion
    {
        get
        {
            decimal ret = 0M;
            if (string.IsNullOrEmpty(dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION.ToString()))
                ret = dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
            else if (string.IsNullOrEmpty(dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION.ToString()))
                ret = dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION;
            else if (string.IsNullOrEmpty(dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJEREDUCCION.ToString()))
                ret = dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJEREDUCCION;
            else if (string.IsNullOrEmpty(dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO.ToString()))
                ret = dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO;
            return ret;
        }
    }

    private decimal totalactojuridico { get; set; }
    private decimal valoradquisicion { get; set; }
    private decimal valorcatastral { get; set; }
    decimal par_valoravaluo { get; set; }
    string regla { get; set; }
    string habitacional { get; set; }
    private decimal? codactojuridico { get; set; }
    private decimal? exencion { get; set; }
    private decimal? reduccion { get; set; }
    private decimal? subsidio { get; set; }
    private decimal? disminucion { get; set; }
    private decimal? condonacion { get; set; }
    private decimal? condonacionJornada { get; set; }
    private decimal impuesto { get; set;}
    private decimal? resultadoRecargo { get; set; }
    private decimal? resultadoActualizacion { get; set; }
    private decimal? resultadoImporteActualizacion { get; set; }
    private decimal? resultadoImporteRecargo { get; set; }
    private decimal? resultadoBaseGravable { get; set; }
    private decimal? resultadoReduccion1995 { get; set; }
    private decimal? resultadoTasa1995 { get; set; }
    private decimal? resultadoImpuesto { get; set; }
    private decimal? resultadoReduccionArt309 { get; set; }
    private decimal? resultadoExencionImporte { get; set; }
    private decimal? resultadoImporteCondonacion { get; set; }

    //public void GetImpuesto(DseDeclaracionIsai dseDeclaracion)
    //{
    //    decimal? parresultadoRecargo = null;
    //    decimal? parresultadoActualizacion = null;
    //    decimal? parresultadoImporteActualizacion = null;
    //    decimal? parresultadoImporteRecargo = null;
    //    decimal? parresultadoBaseGravable = null;
    //    decimal? parresultadoReduccion1995 = null;
    //    decimal? parresultadoTasa1995 = null;
    //    decimal? parresultadoImpuesto = null;
    //    decimal? parresultadoReduccionArt309 = null;
    //    decimal? parresultadoExencionImporte = null;
    //    decimal? parresultadoImporteCondonacion = null;
    //    decimal? resultadoImporteCondonacionxFecha = null;
    //    //decimal decimalParse;
    //    string EsTasaCero = null;
    //    DateTime fecha = new DateTime();
    //    DateTime fechaParseada;
    //    try
    //    {
    //        if (dseDeclaracion.FEXNOT_DECLARACION[0].CODTIPODECLARACION.ToString() != Constantes.PAR_VAL_TIPODECLARACION_ANTI)
    //        {
    //            if (DateTime.TryParse(dseDeclaracion.FEXNOT_DECLARACION[0].FECHACAUSACION.ToString(), out fechaParseada))
    //            {

    //                fecha = fechaParseada;
    //            }
    //            else
    //                fecha = DateTime.Now;
    //        }
    //        else
    //        {
    //            fecha = DateTime.Now;
    //        }
    //        if (!string.IsNullOrEmpty(dseDeclaracion.FEXNOT_DECLARACION[0].ACTOJURIDICO))
    //            codactojuridico = Convert.ToDecimal( dseDeclaracion.FEXNOT_DECLARACION[0].ACTOJURIDICO );

    //        //Solo puede existir 1 ó 0 beneficios fiscales. Los demas tienen que ser nulos.
    //        if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsIDEXENCIONNull())
    //            exencion = dseDeclaracion.FEXNOT_DECLARACION[0].IDEXENCION;
    //        else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsESTASACERONull())
    //            EsTasaCero = Constantes.PAR_VAL_TRUE;
    //        else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
    //            reduccion = dseDeclaracion.FEXNOT_DECLARACION[0].IDREDUCCION;
    //        else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull())
    //            subsidio = dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJESUBSIDIO;
    //        else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull())
    //            disminucion = dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION;
    //        else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
    //            condonacion = dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
    //        else
    //            condonacionJornada = string.IsNullOrEmpty(CondonacionJornada.ToString()) ? null : (decimal?)CondonacionJornada;

            

    //        //valor referido <> null,valor del avalúo se debe coger el valor referido
    //        impuesto = ClienteDeclaracionIsai.CalcularImpuestoDeclaracion(
    //          dseDeclaracion.FEXNOT_DECLARACION[0].CODTIPODECLARACION,
    //          totalactojuridico,
    //          valoradquisicion,
    //          valorcatastral,
    //          par_valoravaluo,
    //          habitacional,
    //          EsTasaCero,
    //          exencion,
    //          reduccion,
    //          condonacionJornada,
    //          subsidio,
    //          disminucion,
    //          condonacion,
    //          fecha,
    //          Convert.ToDecimal(regla),
    //          out parresultadoRecargo,
    //           out parresultadoImporteRecargo,
    //            out parresultadoActualizacion,
    //            out parresultadoImporteActualizacion,
    //            out parresultadoBaseGravable,
    //            out parresultadoReduccion1995,
    //            out parresultadoTasa1995,
    //            out parresultadoImpuesto,
    //            out parresultadoReduccionArt309,
    //            out parresultadoExencionImporte,
    //            out parresultadoImporteCondonacion,
    //            out resultadoImporteCondonacionxFecha);

    //         resultadoRecargo = parresultadoRecargo;
    //         resultadoImporteRecargo=parresultadoImporteRecargo;
    //         resultadoActualizacion=parresultadoActualizacion;
    //         resultadoImporteActualizacion=parresultadoImporteActualizacion;
    //         resultadoBaseGravable=parresultadoBaseGravable;
    //         resultadoReduccion1995=parresultadoReduccion1995;
    //         resultadoTasa1995=parresultadoTasa1995;
    //         resultadoImpuesto=parresultadoImpuesto;
    //         resultadoReduccionArt309=parresultadoReduccionArt309;
    //         resultadoExencionImporte=parresultadoExencionImporte;
    //         resultadoImporteCondonacion = parresultadoImporteCondonacion;

    //}catch(Exception e)
    //{
    
    //}
    //}

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
    private DeclaracionIsaiClient clienteDeclaracionIsai = null;
    private DseDeclaracionIsai dseDeclaracion
    {
        get
        {
            return _dseDeclaracion ;
        }
        set
        {
            if (_dseDeclaracion == null)
                _dseDeclaracion= new DseDeclaracionIsai();
            
             _dseDeclaracion = value;
                 
        }
    }
    private DseDeclaracionIsai _dseDeclaracion
    {
        get;
        set;
    }

    private string digitoVerificador
    {
        get
        {
            if (string.IsNullOrEmpty(_digitoVerificador))
                _digitoVerificador = DigitoVerificadorUtils.ObtenerDigitoVerificador(dseDeclaracion.FEXNOT_DECLARACION[0].REGION
                                         , dseDeclaracion.FEXNOT_DECLARACION[0].MANZANA
                                            , dseDeclaracion.FEXNOT_DECLARACION[0].LOTE
                                                , dseDeclaracion.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA);
            return _digitoVerificador;

        }
    }
    private string _digitoVerificador
    { get; set; }

    private string cuenta
    {
        get
        {
            return dseDeclaracion.FEXNOT_DECLARACION[0].REGION.ToString() + dseDeclaracion.FEXNOT_DECLARACION[0].MANZANA.ToString() + dseDeclaracion.FEXNOT_DECLARACION[0].LOTE.ToString() + dseDeclaracion.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA.ToString() + digitoVerificador;
        }
    }

    private string path
    {
        get
        {
            string _path = string.Empty;
            switch (TipoPago)
            {
                case TipoPago.Vigente:
                    _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVigente;
                    break;
                case TipoPago.Reduccion:
                    _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVigenteReduccion;
                    break;
                case TipoPago.Vencido:
                    _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVencido;
                    break;
                case TipoPago.VencidoReduccion:
                    _path = HttpContext.Current.Server.MapPath("~\\DOCS\\") + archivoVencidoReduccion;
                    break;
            }
            return _path;
        }
    }

    private string archivoVigente
    {
        get
        {
            string cad = ConfigurationManager.AppSettings["fmtVigente"].ToString();
            return string.IsNullOrEmpty(cad) ? string.Empty : cad;
        }
    }

    private string archivoVencido
    {
        get
        {
            string cad = ConfigurationManager.AppSettings["fmtVencido"].ToString();
            return string.IsNullOrEmpty(cad) ? string.Empty : cad;
        }
    }

    private string archivoVigenteReduccion
    {
        get
        {
            string cad = ConfigurationManager.AppSettings["fmtVigenteReduccion"].ToString();
            return string.IsNullOrEmpty(cad) ? string.Empty : cad;
        }
    }

    private string archivoVencidoReduccion
    {
        get
        {
            string cad = ConfigurationManager.AppSettings["fmtVencidoReduccion"].ToString();
            return string.IsNullOrEmpty(cad) ? string.Empty : cad;
        }
    }

    private TipoPago TipoPago
    {
        get;
        set;
    }

    private decimal beneficios
    {
        get
        {
            decimal _beneficios=0M;
            if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsIMPORTEEXENCIONNull() )
            {
                _beneficios = dseDeclaracion.FEXNOT_DECLARACION[0].IMPORTEEXENCION;
            }
            else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
            {
                if (resultadoReduccion1995 != null && resultadoReduccionArt309 != null)
                    _beneficios = (decimal)(resultadoReduccion1995 + resultadoReduccionArt309);
                else if (resultadoReduccion1995 != null)
                    _beneficios = (decimal)resultadoReduccion1995;
                else if (resultadoReduccionArt309 != null)
                    _beneficios = (decimal)resultadoReduccionArt309;
            }
            else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsPORCENTAJESUBSIDIONull())
            {
                _beneficios =  (impuesto/(1+ dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION))*dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
            }
            else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsPORCENTAJEDISMINUCIONNull())
            {
                _beneficios = (impuesto / (1 + dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION)) * dseDeclaracion.FEXNOT_DECLARACION[0].PORCENTAJEDISMINUCION;
            }
            else if (!dseDeclaracion.FEXNOT_DECLARACION[0].IsIMPORTECONDONACIONNull())
                _beneficios =  dseDeclaracion.FEXNOT_DECLARACION[0].IMPORTECONDONACION;

            return _beneficios;
        }
    }

    private decimal recargo
    {
        get 
        {
            return dseDeclaracion.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP;
        }
    }

    private decimal actualizacion
    {
        get
        {
            return dseDeclaracion.FEXNOT_DECLARACION[0].IsACTUALIZACIONIMPORTENull() ? 0M : dseDeclaracion.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE;
        }
    }

    private decimal importe
    {
        get
        {
            decimal _importe = 0M;
            switch (TipoPago)
            { 
                case TipoPago.Vigente:
                    _importe= impuesto;
                    break;
                case TipoPago.Reduccion:
                    _importe =(impuesto - beneficios);
                    break;
                case TipoPago.Vencido:
                    
                    _importe = impuesto - recargo - actualizacion;
                    break;
                case TipoPago.VencidoReduccion:
                    _importe = impuesto + beneficios - recargo - actualizacion;
                    break;
            }
            return _importe;
        }
    }

}