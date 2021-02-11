using System;
using System.ServiceModel; 
using ServiceDeclaracionIsai;
using ServiceFinanzas;


/// <summary>
/// Clase encargada de gestionar el pago online
/// </summary>
public partial class RespuestaPagoOnline : PageBaseISAI
{

    
    /// <summary>
    /// Carga de la página.
    /// </summary>
    /// <param name="sender">Origen del evento.</param>
    /// <param name="e">Información del evento.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Session.Add("nada","dfe");
            Int32 secuenciaTransmision = 0;
            String referencia = String.Empty;
            Decimal importeTotal = 0;
            Int32 numOperBancaria = 0;
            String fechaPago = String.Empty;
            String cadenaValidacion = String.Empty;


            #region [ Recuperar los valores de la llamada. ]
            if (this.Request.Form.Get("s_transm") != null)
            {
                secuenciaTransmision = Convert.ToInt32(this.Request.Form.Get("s_transm"));
            }

            if (this.Request.Form.Get("c_referencia") != null)
            {
                referencia = this.Request.Form.Get("c_referencia");
            }

            if (this.Request.Form.Get("t_importe") != null)
            {
               importeTotal = Convert.ToDecimal(this.Request.Form.Get("t_importe"));
            //    DseAvaluo.WriteXml(@"C:\Proyectos\kktest" + importeTotal.ToString() + ".xml");

            }
            //else
            //{
            //    DseAvaluo.WriteXml(@"C:\Proyectos\kktestNada.xml");
            //}



            if (this.Request.Form.Get("n_autoriz") != null)
            {
                numOperBancaria = Convert.ToInt32(this.Request.Form.Get("n_autoriz"));
            }

            if (this.Request.Form.Get("val_10") != null)
            {
                fechaPago = this.Request.Form.Get("val_10");
            }

            if (this.Request.Form.Get("val_13") != null)
            {
                cadenaValidacion = this.Request.Form.Get("val_13");
            }

            #endregion


        //NEG.BLPagos.RespuestasPagosOnline gestorRespuesta = new NEG.BLPagos.RespuestasPagosOnline();
        //ENT.HistoricoPagos entHistPagos = new ENT.HistoricoPagos();
        //NEG.BLPagos.Pagos gestorPagos = new NEG.BLPagos.Pagos();
        //ENT.Pago entPago = new ENT.Pago();

        //// Almacenar la respuesta en BBDD.
        ////gestorRespuesta.InsertarRespuestaPagoOnline(respuesta);

        //// Se comprueba que el pago estaba registrado y no ha sufrido alteraciones durante el envío.
        if (ClienteDeclaracionIsai.ValidarRespuestaOnline(cadenaValidacion,fechaPago,importeTotal,numOperBancaria,referencia,secuenciaTransmision))
        {

            tipo_pregunta preguntaPago = new tipo_pregunta();
            tipo_respuesta respuestaPago = new tipo_respuesta();
            preguntaPago.importe = importeTotal;
            preguntaPago.lc = "";
            preguntaPago.password = "";
            preguntaPago.@ref = "";
            preguntaPago.sucursal = 0;
            preguntaPago.tipopago = 0;
            preguntaPago.usuario = "";
        //    entHistPagos.IdPagos = respuesta.SecuenciaTransmision;
        //    entHistPagos.FechaEstado = DateTime.Now;
        //    entHistPagos.IdEstadoPago = (Int32)ENUM.enumEstadoPago.Pagado;
        //    entPago.IdPagos = respuesta.SecuenciaTransmision;
        //    entPago.IdEstadoPago = (Int32)ENUM.enumEstadoPago.Pagado;
        //    string[] fecha = respuesta.FechaPago.Split('-');
        //    entPago.FechaPago = new DateTime(Convert.ToInt32(fecha[2][0]),
        //                                     Convert.ToInt32(fecha[2][1]),
        //                                     Convert.ToInt32(fecha[1] + fecha[0]));

         // gestorPagos.ActualizarEstadoDelPago(entPago, entHistPagos);

          respuestaPago = ClienteFinanzas.registrar_pago(preguntaPago);
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

    private void MostrarMensajeInfoExcepcion(string mensaje)
    {
        errorTareas.TextoBasicoMostrar = "Error en la aplicación";
        errorTareas.TextoAvanzadoMostrar = mensaje;
        mpeErrorTareas.Show();
        uppErrorTareas.Update();
    }

    #endregion

}
