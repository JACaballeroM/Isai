using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Oracle.DataAccess.Client;
using SIGAPred.Common.DataAccess.Extensions;
using SIGAPred.Common.DataAccess.OracleDataAccess;
using SIGAPred.Common.DigitoVerificador;
using SIGAPred.Common.Extensions;
using SIGAPred.Documental.Services.Negocio.Gestion.AltasDocumentos.Transaccional;
using SIGAPred.Documental.Services.Negocio.Gestion.ConsultasDocumentos.Enum;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseCatalogoTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseDeclaracionIsaiTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfAvaluosTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfConsultaEspecificaTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfDetalleNotariosTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfEnviosTotalesTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfJustificanteTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfNotariosTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfoDetalleLineasCapturaTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfoLineasCapturaTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfPagosTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfSociedadPeritosTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DseInfValoresTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.AccesoDatos.DsePersonasTableAdapters;
using SIGAPred.FuentesExternas.Isai.Services.Negocio.Interfaces;
using SIGAPred.FuentesExternas.Isai.Services.ServiceRCON;
using SIGAPred.RegistroContribuyentes.Services.Negocio;
//using SIGAPred.FuentesExternas.Isai.Services.ServiceLC1;
using SIGAPred.FuentesExternas.Isai.Services.ServiceLC2;
using MyExtentions;
using System.Net;

namespace SIGAPred.FuentesExternas.Isai.Services.Negocio
{

    /// <summary>
    /// Clase encargada de gestionar el negocio del módulo de ISAI
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DeclaracionIsai : IDeclaracionIsai
    {

        #region Enumeraciones
        /// <summary>
        /// Enumeración con los tipos de declaraciones
        /// </summary>
        private enum TiposDeclaraciones
        {
            //0. Normal
            Normal = 0,
            //1. Anticipada
            Anticipada = 1,
            //2. Complementaria
            Complementaria = 2,
            //3. Jornada Notarial
            JornadaNotarial = 3
        }

        /// <summary>
        /// Enumeración con los estados de las declaraciones
        /// </summary>
        private enum EstadosDeclaraciones
        {
            //0. Borrador
            Borrador = 0,
            //1. Pendiente
            Pendiente = 1,
            //2. Presentada
            Presentada = 2,
            //4. Aceptada
            Aceptada = 4,
            //5. Inconsistente
            Inconsistente = 5,
            //3. Pendiente Documentacion
            PendienteDocumentacion = 3
        }
        #endregion

        #region TableAdapters usados en la clase

        private FEXNOT_CATROLESTableAdapter catRolesTA = null;
        private FEXNOT_PARTICIPANTESTableAdapter participanteTA = null;
        private FEXNOT_DECLARACIONTableAdapter declaracionTA = null;
        private FEXNOT_CONDONACIONTableAdapter condonacionTA = null;
        private FEXNOT_CATESTADDECLARACIONTableAdapter catEstadoDeclaracionTA = null;
        private FEXNOT_CATESTADOPAGOTableAdapter catEstadoPagoTA = null;
        private FEXNOT_CATTIPOSDECLARACIONTableAdapter catTiposDeclaracionTA = null;
        private FEXNOT_DOMICILIOTableAdapter domicilioTA = null;
        private FEXNOT_PERSONASTableAdapter personaTA = null;
        private FEXNOT_CATBANCOTableAdapter catBancoTA = null;
        private FEXNOT_CATPROCEDENCIATableAdapter catProcedenciaTA = null;
        private FEXNOT_CATACTOSJURIDICOSTableAdapter catActoJuridicoTA = null;
        private FEXNOT_EXENCIONESTableAdapter exencionTA = null;
        private FEXNOT_REDUCCIONESTableAdapter reduccionTA = null;
        private FEXNOT_JORNADANOTARIALTableAdapter jornadaNotarialTA = null;
        private FEXNOT_INFONOTARIOSTableAdapter infoNotarioTA = null;
        private FEXNOT_INFODETALLENOTARIOSTableAdapter infoDetalleNotarioTA = null;
        private FEXNOT_INFOAVALUOSTOTALESTableAdapter infoAvaluosTotalesTA = null;
        private FEXNOT_INFODECLARACTOTALESTableAdapter infoDeclaracionesTotalesTA = null;
        private FEXNOT_INFOLINEASCAPTURATableAdapter infoLineasCapturaTA = null;
        private FEXNOT_INFODETALLELINEASCAPTURATableAdapter infoDetalleLineasCapturaTA = null;
        private FEXNOT_INFOSOCIEDADPERITOSTableAdapter infoSociedadPeritosTA = null;
        private FEXNOT_INFOVALORESTableAdapter infoValoresTA = null;
        private FEXNOT_INFOCONSESPECIFICATableAdapter infoConsEspecificaTA = null;
        private FEXNOT_CATCALENDARIOTableAdapter catCalendarioTA = null;
        private FEXNOT_INFPAGOSTableAdapter infPagosTA = null;
        private FEXNOT_INFJUSTIFICANTETableAdapter infJustificanteTA = null;
        private FEXNOT_INFACUSETableAdapter infAcuseTA = null;
        private FEXNOT_INF_SOCPERAVALUOS_PTableAdapter avaluosTA = null;
        private FEXNOT_CATMOTIVORECHAZO_TableAdapter catMotivoRechazoTA = null;
        private FEXNOT_CATSALARIOMINIMO_TableAdapter salariosMinimosTA = null;
        private FEXNOT_INF_ACUSEGEN_PTableAdapter infAcuseGenTA = null;
        private FEXNOT_INF_ACUSEPAR_PTableAdapter infAcuseParTA = null;

        /// <summary>
        /// Propiedad que obtiene el TableAdapater  de la tabla FEXNOT_INFACUSE del DataSet DseInfJustificante
        /// </summary>
        protected FEXNOT_INFACUSETableAdapter InfAcuseTA
        {
            get
            {
                if (infAcuseTA == null)
                {
                    infAcuseTA = new FEXNOT_INFACUSETableAdapter();
                }
                return infAcuseTA;
            }

        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de datos generales de la tabla FEXNOT_INFACUSE del DataSet DseInfJustificante
        /// </summary>
        protected FEXNOT_INF_ACUSEGEN_PTableAdapter InfAcuseGenTA
        {
            get
            {
                if (infAcuseGenTA == null)
                {
                    infAcuseGenTA = new FEXNOT_INF_ACUSEGEN_PTableAdapter();
                }
                return infAcuseGenTA;
            }

        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INF_SOCPERAVALUOS del DataSet DseInfAvaluos
        /// </summary>
        protected FEXNOT_INF_ACUSEPAR_PTableAdapter InfAcuseParTA
        {
            get
            {
                if (infAcuseParTA == null)
                {
                    infAcuseParTA = new FEXNOT_INF_ACUSEPAR_PTableAdapter();
                }
                return infAcuseParTA;
            }

        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de SOCPERAVALUOS
        /// </summary>
        protected FEXNOT_INF_SOCPERAVALUOS_PTableAdapter AvaluosTA
        {
            get
            {
                if (avaluosTA == null)
                {
                    avaluosTA = new FEXNOT_INF_SOCPERAVALUOS_PTableAdapter();
                }
                return avaluosTA;
            }
        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATROLESACTOSJUR del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATROLESTableAdapter CatRolesTA
        {
            get
            {
                if (catRolesTA == null)
                {
                    catRolesTA = new FEXNOT_CATROLESTableAdapter();
                }
                return catRolesTA;
            }
        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_PARTICIPANTES del DataSet DseDeclaracionIsai
        /// </summary>
        protected FEXNOT_PARTICIPANTESTableAdapter ParticipanteTA
        {
            get
            {
                if (participanteTA == null)
                {
                    participanteTA = new FEXNOT_PARTICIPANTESTableAdapter();
                }
                return participanteTA;
            }
        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_DECLARACION del DataSet DseDeclaracionIsai
        /// </summary>
        protected FEXNOT_DECLARACIONTableAdapter DeclaracionTA
        {
            get
            {
                if (declaracionTA == null)
                {
                    declaracionTA = new FEXNOT_DECLARACIONTableAdapter();
                }
                return declaracionTA;
            }
        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CONDONACION del DataSet DseDeclaracionIsai
        /// </summary>
        protected FEXNOT_CONDONACIONTableAdapter CondonacionTA
        {
            get
            {
                if (condonacionTA == null)
                {
                    condonacionTA = new FEXNOT_CONDONACIONTableAdapter();
                }
                return condonacionTA;
            }
        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATESTADDECLARACION del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATESTADDECLARACIONTableAdapter CatEstadoDeclaracionTA
        {
            get
            {
                if (catEstadoDeclaracionTA == null)
                {
                    catEstadoDeclaracionTA = new FEXNOT_CATESTADDECLARACIONTableAdapter();
                }
                return catEstadoDeclaracionTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATESTADOPAGO del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATESTADOPAGOTableAdapter CatEstadoPagoTA
        {
            get
            {
                if (catEstadoPagoTA == null)
                {
                    catEstadoPagoTA = new FEXNOT_CATESTADOPAGOTableAdapter();
                }
                return catEstadoPagoTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATTIPOSDECLARACION del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATTIPOSDECLARACIONTableAdapter CatTiposDeclaracionTA
        {
            get
            {
                if (catTiposDeclaracionTA == null)
                {
                    catTiposDeclaracionTA = new FEXNOT_CATTIPOSDECLARACIONTableAdapter();
                }
                return catTiposDeclaracionTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_DOMICILIO del DataSet DseDeclaracionIsai
        /// </summary>
        protected FEXNOT_DOMICILIOTableAdapter DomicilioTA
        {
            get
            {
                if (domicilioTA == null)
                {
                    domicilioTA = new FEXNOT_DOMICILIOTableAdapter();
                }
                return domicilioTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATMOTIVORECHAZO del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATMOTIVORECHAZO_TableAdapter CatMotivoRechazoTA
        {
            get
            {
                if (catMotivoRechazoTA == null)
                    catMotivoRechazoTA = new FEXNOT_CATMOTIVORECHAZO_TableAdapter();
                return catMotivoRechazoTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_PERSONAS del DataSet DsePersonas
        /// </summary>
        protected FEXNOT_PERSONASTableAdapter PersonaTA
        {
            get
            {
                if (personaTA == null)
                {
                    personaTA = new FEXNOT_PERSONASTableAdapter();
                }
                return personaTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATBANCO del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATBANCOTableAdapter CatBancoTA
        {
            get
            {
                if (catBancoTA == null)
                {
                    catBancoTA = new FEXNOT_CATBANCOTableAdapter();
                }
                return catBancoTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATPROCEDENCIA del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATPROCEDENCIATableAdapter CatProcedenciaTA
        {
            get
            {
                if (catProcedenciaTA == null)
                {
                    catProcedenciaTA = new FEXNOT_CATPROCEDENCIATableAdapter();
                }
                return catProcedenciaTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATACTOSJURIDICOS del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATACTOSJURIDICOSTableAdapter CatActoJuridicoTA
        {
            get
            {
                if (catActoJuridicoTA == null)
                {
                    catActoJuridicoTA = new FEXNOT_CATACTOSJURIDICOSTableAdapter();
                }
                return catActoJuridicoTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_EXENCIONES del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_EXENCIONESTableAdapter ExencionTA
        {
            get
            {
                if (exencionTA == null)
                {
                    exencionTA = new FEXNOT_EXENCIONESTableAdapter();
                }
                return exencionTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_REDUCCIONES del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_REDUCCIONESTableAdapter ReduccionTA
        {
            get
            {
                if (reduccionTA == null)
                {
                    reduccionTA = new FEXNOT_REDUCCIONESTableAdapter();
                }
                return reduccionTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_JORNADANOTARIAL del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_JORNADANOTARIALTableAdapter JornadaNotarialTA
        {
            get
            {
                if (jornadaNotarialTA == null)
                {
                    jornadaNotarialTA = new FEXNOT_JORNADANOTARIALTableAdapter();
                }
                return jornadaNotarialTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFONOTARIOS del DataSet DseInfNotarios
        /// </summary>
        protected FEXNOT_INFONOTARIOSTableAdapter InfoNotarioTA
        {
            get
            {
                if (infoNotarioTA == null)
                {
                    infoNotarioTA = new FEXNOT_INFONOTARIOSTableAdapter();
                }
                return infoNotarioTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFODETALLENOTARIOS del DataSet DseInfDetalleNotarios
        /// </summary>
        protected FEXNOT_INFODETALLENOTARIOSTableAdapter InfoDetalleNotarioTA
        {
            get
            {
                if (infoDetalleNotarioTA == null)
                {
                    infoDetalleNotarioTA = new FEXNOT_INFODETALLENOTARIOSTableAdapter();
                }
                return infoDetalleNotarioTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFOAVALUOSTOTALES del DataSet DseInfEnviosTotales
        /// </summary>
        protected FEXNOT_INFOAVALUOSTOTALESTableAdapter InfoAvaluosTotalesTA
        {
            get
            {
                if (infoAvaluosTotalesTA == null)
                {
                    infoAvaluosTotalesTA = new FEXNOT_INFOAVALUOSTOTALESTableAdapter();
                }
                return infoAvaluosTotalesTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFODECLARACTOTALES del DataSet DseInfEnviosTotales
        /// </summary>
        protected FEXNOT_INFODECLARACTOTALESTableAdapter InfoDeclaracionesTotalesTA
        {
            get
            {
                if (infoDeclaracionesTotalesTA == null)
                {
                    infoDeclaracionesTotalesTA = new FEXNOT_INFODECLARACTOTALESTableAdapter();
                }
                return infoDeclaracionesTotalesTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFOLINEASCAPTURA del DataSet DseInfoLineasCaptura
        /// </summary>
        protected FEXNOT_INFOLINEASCAPTURATableAdapter InfoLineasCapturaTA
        {
            get
            {
                if (infoLineasCapturaTA == null)
                {
                    infoLineasCapturaTA = new FEXNOT_INFOLINEASCAPTURATableAdapter();
                }
                return infoLineasCapturaTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFODETALLELINEASCAPTURA del DataSet DseInfoDetallesLineasCaptura
        /// </summary>
        protected FEXNOT_INFODETALLELINEASCAPTURATableAdapter InfoDetalleLineasCapturaTA
        {
            get
            {
                if (infoDetalleLineasCapturaTA == null)
                {
                    infoDetalleLineasCapturaTA = new FEXNOT_INFODETALLELINEASCAPTURATableAdapter();
                }
                return infoDetalleLineasCapturaTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFOSOCIEDADPERITOS del DataSet DseInfSociedadPeritos
        /// </summary>
        protected FEXNOT_INFOSOCIEDADPERITOSTableAdapter InfoSociedadPeritosTA
        {
            get
            {
                if (infoSociedadPeritosTA == null)
                {
                    infoSociedadPeritosTA = new FEXNOT_INFOSOCIEDADPERITOSTableAdapter();
                }
                return infoSociedadPeritosTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFOVALORES del DataSet DseInfValores
        /// </summary>
        protected FEXNOT_INFOVALORESTableAdapter InfoValoresTA
        {
            get
            {
                if (infoValoresTA == null)
                {
                    infoValoresTA = new FEXNOT_INFOVALORESTableAdapter();
                }
                return infoValoresTA;
            }
        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFOCONSESPECIFICA del DataSet DseInfConsultaEspecifica
        /// </summary>
        protected FEXNOT_INFOCONSESPECIFICATableAdapter InfoConsEspecificaTA
        {
            get
            {
                if (infoConsEspecificaTA == null)
                {
                    infoConsEspecificaTA = new FEXNOT_INFOCONSESPECIFICATableAdapter();
                }
                return infoConsEspecificaTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFPAGOS del DataSet DseInfPagos
        /// </summary>
        protected FEXNOT_INFPAGOSTableAdapter InfPagosTA
        {
            get
            {
                if (infPagosTA == null)
                {
                    infPagosTA = new FEXNOT_INFPAGOSTableAdapter();
                }
                return infPagosTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_INFJUSTIFICANTE del DataSet DseInfJustificante
        /// </summary>
        protected FEXNOT_INFJUSTIFICANTETableAdapter InfJustificanteTA
        {
            get
            {
                if (infJustificanteTA == null)
                {
                    infJustificanteTA = new FEXNOT_INFJUSTIFICANTETableAdapter();
                }
                return infJustificanteTA;
            }
        }


        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATCALENDARIO del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATCALENDARIOTableAdapter CatCalendarioTA
        {
            get
            {
                if (catCalendarioTA == null)
                {
                    catCalendarioTA = new FEXNOT_CATCALENDARIOTableAdapter();
                }
                return catCalendarioTA;
            }
        }

        /// <summary>
        /// Propiedad que obtiene el TableAdapater de la tabla FEXNOT_CATSALARIOMINIMO del DataSet DseCatalogo
        /// </summary>
        protected FEXNOT_CATSALARIOMINIMO_TableAdapter SalariosMinimosTA
        {
            get
            {
                if (salariosMinimosTA == null)
                    salariosMinimosTA = new FEXNOT_CATSALARIOMINIMO_TableAdapter();
                return salariosMinimosTA;
            }
        }

        #endregion


        /// <summary>
        /// Metodo de Test para el servicio. Devuelve OK
        /// </summary>
        /// <returns></returns>
        public string Test(string cad)
        {
            DatosWebService datos = new DatosWebService(cad);
            string s = datos.funcionDeCobro;
            return "ok";
        }

        #region Métodos eliminar


        /// <summary>
        /// Elimina una declaración (Únicamente si esta en estado borrador)
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        public void EliminarDeclaracion(int idDeclaracion)
        {
            try
            {
                object o = null;
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByIdDeclaracion(idDeclaracion, out o);

                EliminarDeclaracion(declaracionDT);
            }
            catch (FaultException<Exceptions.DeclaracionIsaiInfoException> diex)
            {
                ExceptionPolicyWrapper.HandleException(diex);
                throw diex;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Método para eliminar una condonación de la declaración
        /// </summary>
        /// <param name="idDeclaracion"> Identificador de la declaración</param>
        public void EliminarCondonacion(int idDeclaracion)
        {
            try
            {
                CondonacionTA.Delete(idDeclaracion);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        ///  Elimina una declaración (Únicamente si esta en estado borrador)
        /// </summary>
        /// <param name="declaracion">Data Table Declaración con la declaración a eliminar en la primera posición</param>
        public void EliminarDeclaracion(DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracion)
        {
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesDT = null;
            object o = null;

            ////verificacion del DATASET no este vacio.
            if (declaracion.Count() > 0)
            {
                //Comprobamos que la declaracion este en estado borrador
                if (declaracion[0].CODESTADODECLARACION.ToInt() != (int)EstadosDeclaraciones.Borrador)
                {
                    throw new FaultException<Exceptions.DeclaracionIsaiInfoException>(new Exceptions.DeclaracionIsaiInfoException("Declaración con estado distinto de borrador, no se puede eliminar"));
                }

            }
            else

                throw new FaultException<Exceptions.DeclaracionIsaiInfoException>(new Exceptions.DeclaracionIsaiInfoException("Si el problema persiste contacte con el administrador del sistema"));

            using (TransactionHelper transactionHelper = new TransactionHelper(DeclaracionTA.Connection))
            {
                try
                {
                    transactionHelper.BeginTransaction();
                    //Asignamos la transaccion a los tableAdapters

                    ParticipanteTA.SetTransaction(transactionHelper);
                    CondonacionTA.SetTransaction(transactionHelper);
                    DeclaracionTA.SetTransaction(transactionHelper);

                    //Obtenemos los participantes de la declaración y los eliminamos
                    participantesDT = ParticipanteTA.GetDataByParticipantesByIdDeclaracion(declaracion[0].IDDECLARACION, out o);
                    foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participantesDR in participantesDT)
                        participantesDR.Delete();
                    ParticipanteTA.Update(participantesDT);

                    //Eliminamos la condonacion de la declaración
                    CondonacionTA.Delete(declaracion[0].IDDECLARACION);

                    //Eliminamos  la declaración
                    declaracion.AcceptChanges();
                    declaracion[0].Delete();
                    DeclaracionTA.Update(declaracion);

                    transactionHelper.Commit();
                }
                catch (FaultException<Exceptions.DeclaracionIsaiInfoException> diex)
                {

                    ExceptionPolicyWrapper.HandleException(diex);
                    throw;
                }
                catch (Exception ex)
                {
                    transactionHelper.RollBack();
                    ExceptionPolicyWrapper.HandleException(ex);
                    throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
                }
            }
        }

        /// <summary>
        /// Eliminar una participante de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="idPersona">Identificador de la persona</param>
        /// <param name="codTipoPersona">Tipo de persona</param>
        public void EliminarParticipanteDeclaracion(int idDeclaracion, int idPersona, string codTipoPersona)
        {
            try
            {
                ParticipanteTA.Delete(idPersona,
                                      idDeclaracion,
                                      codTipoPersona);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        #endregion

        #region Métodos insertar

        /// <summary>
        /// Inserta un participante a la declaración
        /// </summary>
        /// <param name="participante">DataTable con una única fila de declaración</param>
        /// <returns></returns>
        public DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable InsertarParticipanteDeclaracion(DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participante)
        {
            try
            {

                DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participantesNuevos = (DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable)participante.GetChanges(System.Data.DataRowState.Added);

                if (participantesNuevos != null)
                {
                    ParticipanteTA.Update(participantesNuevos);
                }

                return participantesNuevos;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Valida la declaración antes de pasarla a estado pendiente de presentar
        /// </summary>
        /// <param name="dseDeclaracionIsai">Dataset con la declaración</param>
        /// <param name="declaracionRow">Fila de la declaración que  se va a validar</param>
        /// <param name="fechaDoc">Fecha del documento</param>
        /// <returns>Mensaje de la validación. Si la validación a sido correcta el mensaje sera vacio.</returns>
        private string ValidarDeclaracion(DseDeclaracionIsai dseDeclaracionIsai, DseDeclaracionIsai.FEXNOT_DECLARACIONRow declaracionRow, DateTime? fechaDoc)
        {
            Object o = null;
            string mensaje = string.Empty;
            DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow[] rowsParticipantes;
            DseCatalogo.FEXNOT_CATROLESRow[] rowsRoles;

            try
            {
                if (declaracionRow.IsCODPROCEDENCIANull())
                    mensaje += "<LI> Procedencia.";
                //Declaracion tiene seleccionado un acto juridico
                if (declaracionRow.IsCODACTOJURNull())
                {
                    mensaje += "<LI> Acto jurídico.";
                }


                if (declaracionRow.IsFECHACAUSACIONNull() && dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TiposDeclaraciones.Anticipada.ToDecimal())
                    mensaje += "<LI> Fecha de causación.";

                if (declaracionRow.IsVALORCATASTRALNull())
                    mensaje += "<LI> Valor catastral.";

                if (!declaracionRow.IsFECHACAUSACIONNull() && DateTime.Compare(declaracionRow.FECHACAUSACION, new DateTime(1972, 01, 01)) < 0 && dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TiposDeclaraciones.Anticipada.ToDecimal())
                    mensaje += "<LI> La aplicación solamente permite hacer declaraciones de ISAI con fecha igual o posterior a 01/01/1972.";

                if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TiposDeclaraciones.Anticipada.ToDecimal() && declaracionRow.IsFECHAPREVENTIVANull())
                    mensaje += "<LI> Fecha preventiva.";



                if (declaracionRow.CODTIPODECLARACION != TiposDeclaraciones.JornadaNotarial.ToDecimal())
                {


                    if (declaracionRow.IsVALORAVALUONull() || declaracionRow.VALORAVALUO == 0)
                        mensaje += "<LI> Valor del avalúo.";
                }
                else
                {
                    DseCatalogo.FEXNOT_JORNADANOTARIALDataTable jornadaNotarialDT = ObtenerJornadaNotarialByAnio(declaracionRow.VALORCATASTRAL);

                    if (jornadaNotarialDT.Count == 0)
                    {
                        mensaje += "<LI> El valor catastral no tiene condonación, se debe realizar una declaración normal.";
                    }
                }

                //suma de participantes enajenantes es igual al porcentaje de los adquirienes y debe existir al menos uno de cada


                rowsParticipantes = (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow[])dseDeclaracionIsai.FEXNOT_PARTICIPANTES.Select(dseDeclaracionIsai.FEXNOT_PARTICIPANTES.IDDECLARACIONColumn + " = " + declaracionRow.IDDECLARACION);
                if ((from n in rowsParticipantes select n.CODROL).Distinct().Count() != 2)
                {
                    mensaje += "<LI> Deben existir al menos un participante adquiriente y uno enajenante.";
                }
                else
                {
                    if (declaracionRow.CODESTADODECLARACION != 0)
                    {
                        decimal par1 = (from n in rowsParticipantes
                                        group n by n.CODROL into g
                                        select new { Suma = g.Sum(p => p.PORCTRANSMISION) }).First().Suma.ToDecimal();
                        decimal par2 = (from n in rowsParticipantes
                                        group n by n.CODROL into g
                                        select new { Suma = g.Sum(p => p.PORCTRANSMISION) }).Last().Suma.ToDecimal();
                        decimal margenError = Convert.ToDecimal(ConfigurationManager.AppSettings["MargenError"]) * Math.Max(par1, par2);

                        if (Math.Abs(par1 - par2) * 100 > margenError)
                        {
                            mensaje += "<LI> El porcentaje de los adquirientes no es el mismo que la suma de los enajenantes.";
                        }
                    }
                }

                //Validaciones extras para las declaraciones de tipo jornada notarial
                if (declaracionRow.CODTIPODECLARACION == TiposDeclaraciones.JornadaNotarial.ToDecimal())
                {
                    //Validar que la cuenta catastral existe
                    if (!ValidacioneslUtils.ComprobarExisteCuentaCatastral(declaracionRow.REGION, declaracionRow.MANZANA, declaracionRow.LOTE, declaracionRow.UNIDADPRIVATIVA))
                    {
                        mensaje += "<LI> La cuenta catastral introducida no existe.";
                    }

                    //Validar que le valor catastral es correcto según la cuenta y fecha intoducidas
                    decimal? valorCatEsperdao = ValidacioneslUtils.ObtenerValorCatastral(declaracionRow.REGION, declaracionRow.MANZANA, declaracionRow.LOTE, declaracionRow.UNIDADPRIVATIVA, declaracionRow.FECHACAUSACION);
                    if (valorCatEsperdao != null)
                    {
                        if (Convert.ToDecimal(valorCatEsperdao).ToString("0.00").ToDecimal() != declaracionRow.VALORCATASTRAL)
                            mensaje += "<LI> El valor catastral introducido no se corresponde con el valor catastral para la cuenta y fecha indicadas.";
                    }
                }
                //Correcto
                return mensaje;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }



        /// <summary>
        /// Copia la declaración
        /// </summary>
        /// <param name="declaracionOriginalRow">Declaración de la que se va relizar la copia</param>
        /// <param name="declaracionNuevaRow">Declaración resultante</param>
        /// <returns>DataTable con la copia de la declaración</returns>
        private void CopiarDeclaracion(DseDeclaracionIsai.FEXNOT_DECLARACIONRow declaracionOriginalRow, ref DseDeclaracionIsai.FEXNOT_DECLARACIONRow declaracionNuevaRow)
        {
            try
            {
                //DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionesNuevaDT = new DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable();


                declaracionNuevaRow.ACTUALIZACIONIMPORTE = 0;
                declaracionNuevaRow.SetBANCONull();
                declaracionNuevaRow.CODACTOJUR = declaracionOriginalRow.CODACTOJUR;
                declaracionNuevaRow.CODESTADODECLARACION = 0;
                declaracionNuevaRow.CODESTADOPAGO = 0;
                declaracionNuevaRow.CODTIPODECLARACION = declaracionOriginalRow.CODTIPODECLARACION;
                declaracionNuevaRow.SetFECHALINEACAPTURANull();
                declaracionNuevaRow.SetFECHAPAGONull();
                declaracionNuevaRow.SetFECHAPRESENTACIONNull();
                declaracionNuevaRow.IDAVALUO = declaracionOriginalRow.IDAVALUO;
                declaracionNuevaRow.SetIDEXENCIONNull();
                declaracionNuevaRow.IDPERSONA = declaracionOriginalRow.IDPERSONA;
                declaracionNuevaRow.SetIDREDUCCIONNull();
                declaracionNuevaRow.IMPORTEIMPUESTO = 0;
                declaracionNuevaRow.IMPUESTOPAGADO = 0;
                declaracionNuevaRow.SetLINEACAPTURANull();
                declaracionNuevaRow.LOTE = declaracionOriginalRow.LOTE;
                declaracionNuevaRow.MANZANA = declaracionOriginalRow.MANZANA;
                declaracionNuevaRow.SetNUMPRESENTACIONNull();
                declaracionNuevaRow.SetPORCENTAJEDISMINUCIONNull();
                declaracionNuevaRow.GENERADA = "S";
                declaracionNuevaRow.SetPORCENTAJESUBSIDIONull();
                declaracionNuevaRow.SetPORCJERECARGOPAGOEXTEMPNull();
                declaracionNuevaRow.REGION = declaracionOriginalRow.REGION;
                declaracionNuevaRow.SetSUCURSALNull();
                declaracionNuevaRow.UNIDADPRIVATIVA = declaracionOriginalRow.UNIDADPRIVATIVA;
                declaracionNuevaRow.VALORCATASTRAL = declaracionOriginalRow.VALORCATASTRAL;
                declaracionNuevaRow.SetVALORAVALUONull();
                declaracionNuevaRow.IDDOCUMENTODIGITAL = declaracionOriginalRow.IDDOCUMENTODIGITAL;
                declaracionNuevaRow.IDUSUARIO = declaracionOriginalRow.IDUSUARIO;
                declaracionNuevaRow.CODPROCEDENCIA = declaracionOriginalRow.CODPROCEDENCIA;
                declaracionNuevaRow.ENPAPEL = declaracionOriginalRow.ENPAPEL;
                declaracionNuevaRow.ESHABITACIONAL = declaracionOriginalRow.ESHABITACIONAL;
                declaracionNuevaRow.SetFECHAPREVENTIVANull();
                declaracionNuevaRow.SetIDJORNADANOTARIALNull();
                declaracionNuevaRow.ESVCATOBTENIDOFISCAL = declaracionOriginalRow.ESVCATOBTENIDOFISCAL;
                if (!declaracionOriginalRow.IsFECHAESCRITURANull())
                {
                    declaracionNuevaRow.FECHAESCRITURA = declaracionOriginalRow.FECHAESCRITURA;
                }
                else
                    declaracionNuevaRow.SetFECHAESCRITURANull();

                if (!declaracionOriginalRow.IsFECHACAUSACIONNull())
                {
                    declaracionNuevaRow.FECHACAUSACION = declaracionOriginalRow.FECHACAUSACION;
                }
                else
                    declaracionNuevaRow.SetFECHACAUSACIONNull();

                if (!declaracionOriginalRow.IsFECHAPREVENTIVANull())
                {
                    declaracionNuevaRow.FECHAPREVENTIVA = declaracionOriginalRow.FECHAPREVENTIVA;
                }
                else
                    declaracionNuevaRow.SetFECHAPREVENTIVANull();

                //declaracionesNuevaDT.AddFEXNOT_DECLARACIONRow(declaracionNuevaRow);

                if (!declaracionOriginalRow.IsIDDECLARACIONPADRENull())
                    declaracionNuevaRow.IDDECLARACIONPADRE = declaracionOriginalRow.IDDECLARACIONPADRE;
                else
                    declaracionNuevaRow.SetIDDECLARACIONPADRENull();
                if (!declaracionOriginalRow.IsLUGARJORNADANOTARIALNull())
                    declaracionNuevaRow.LUGARJORNADANOTARIAL = declaracionOriginalRow.LUGARJORNADANOTARIAL;
                else
                    declaracionNuevaRow.SetLUGARJORNADANOTARIALNull();
                if (!declaracionOriginalRow.IsNUMPRESENTACIONINICIALNull())
                    declaracionNuevaRow.NUMPRESENTACIONINICIAL = declaracionOriginalRow.NUMPRESENTACIONINICIAL;
                else
                    declaracionNuevaRow.SetNUMPRESENTACIONINICIALNull();
                if (!declaracionOriginalRow.IsPERIODONull())
                    declaracionNuevaRow.PERIODO = declaracionOriginalRow.PERIODO;
                else
                    declaracionNuevaRow.SetPERIODONull();
                if (!declaracionOriginalRow.IsVALORADQUISICIONNull())
                    declaracionNuevaRow.VALORADQUISICION = declaracionOriginalRow.VALORADQUISICION;
                else
                    declaracionNuevaRow.SetVALORADQUISICIONNull();
                if (!declaracionOriginalRow.IsVALORAVALUONull())
                    declaracionNuevaRow.VALORAVALUO = declaracionOriginalRow.VALORAVALUO;
                else
                    declaracionNuevaRow.SetVALORAVALUONull();


            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));

            }
        }


        /// <summary>
        /// Registramos una declaracion con el campo EnPapel a 'S' del cual mas adelante podra ser 
        /// modificada para ser guarda en el sistema.
        /// </summary>
        /// <param name="declaracionDT"></param>
        public void RegistrarDeclaracionEnPapelNormalAnticipada(ref DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT)
        {
            try
            {

                DeclaracionTA.Update(declaracionDT);
                //Para obtener el númeroUnico
                declaracionDT[0].NUMPRESENTACION = ObtenerNumeroPresentacionDec(declaracionDT[0].IDDECLARACION);

            }
            catch (FaultException<Exceptions.DeclaracionIsaiInfoException> diex)
            {
                ExceptionPolicyWrapper.HandleException(diex);
                throw diex;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Registramos una declaracion con el campo EnPapel a 'S' del cual mas adelante podra ser 
        /// modificada para ser guarda en el sistema.
        /// </summary>
        /// <param name="dseDeclaracionIsai">Dataset con la declaración de ISAI</param>
        public void RegistrarDeclaracionEnPapelComplementaria(ref DseDeclaracionIsai dseDeclaracionIsai)
        {
            try
            {
                DeclaracionTA.Update(dseDeclaracionIsai);
                CondonacionTA.Update(dseDeclaracionIsai);
                ParticipanteTA.Update(dseDeclaracionIsai);
                dseDeclaracionIsai.FEXNOT_DECLARACION[0].NUMPRESENTACION = ObtenerNumeroPresentacionDec(dseDeclaracionIsai.FEXNOT_DECLARACION[0].IDDECLARACION);
            }
            catch (FaultException<Exceptions.DeclaracionIsaiInfoException> diex)
            {
                ExceptionPolicyWrapper.HandleException(diex);
                throw diex;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Registra la declaración en la Base de Datos
        /// </summary>
        /// <param name="dseDeclaracionIsai">DataSet declaración ISAI</param>
        /// <param name="xmlDoc">Cadena de texto con el xml de documento</param>
        /// <param name="tipoDocumentoDigital">Cadena de texto con el tipo de documento</param>
        /// <param name="listaFicheros">Cadena de texto con la lista de ficheros</param>
        /// <param name="xmlDocBF">Cadena de texto con el xml del documento de beneficios fiscales</param>
        /// <param name="tipoDocumentoDigitalBF">Cadena de texto con el tipo de documentos de beneficios fiscales</param>
        /// <param name="listaFicherosBF">Cadena de texto con la lista de ficheros</param>
        /// <param name="ImpuestoAnteriorPagado">Impuesto pagado</param>
        /// <param name="valorReferido">Valor referido</param>
        /// <returns></returns>
        public string RegistrarDeclaracion2(
          ref DseDeclaracionIsai dseDeclaracion,
          string xmlDoc,
          string tipoDocumentoDigital,
          string listaFicheros,
          string xmlDocBF,
          string tipoDocumentoDigitalBF,
          string listaFicherosBF,
          Decimal? ImpuestoAnteriorPagado,
          Decimal? valorReferido,
          DateTime? fechaValorReferido,
          Decimal[] valoresIsr,
          Decimal? idusuario = null)
        {
            Decimal? nullable1 = new Decimal?();
            Decimal? nullable2 = new Decimal?();
            Decimal? resultadoRecargo = new Decimal?();
            Decimal? resultadoActualizacion = new Decimal?();
            Decimal? resultadoImporteActualizacion = new Decimal?();
            Decimal? resultadoImporteRecargo = new Decimal?();
            Decimal? resultadoBaseGravable = new Decimal?();
            Decimal? resultadoReduccion1995 = new Decimal?();
            Decimal? resultadoTasa1995 = new Decimal?();
            Decimal? resultadoImpuesto = new Decimal?();
            Decimal? resultadoReduccionArt309 = new Decimal?();
            Decimal? resultadoExencionImporte = new Decimal?();
            Decimal? resultadoImporteCondonacion = new Decimal?();
            Decimal? resultadoImporteCondonacionxFecha = new Decimal?();
            string str = string.Empty;
            string empty = string.Empty;
            SIGAPred.Common.DataAccess.OracleDataAccess.TransactionHelper transactionHelper = (SIGAPred.Common.DataAccess.OracleDataAccess.TransactionHelper)null;
            DseDeclaracionIsai dseDeclaracionIsai = dseDeclaracion;
            try
            {
                transactionHelper = new SIGAPred.Common.DataAccess.OracleDataAccess.TransactionHelper((OracleConnection)this.DeclaracionTA.Connection);
                transactionHelper.BeginTransaction();
                this.DeclaracionTA.SetTransaction(transactionHelper);
                this.CondonacionTA.SetTransaction(transactionHelper);
                this.ParticipanteTA.SetTransaction(transactionHelper);
                if (dseDeclaracionIsai.FEXNOT_DECLARACION == null || dseDeclaracionIsai.FEXNOT_DECLARACION.Count != 1)
                    throw new Exception("Solo se puede registrar una Declaración");
                Decimal? nullable3 = valorReferido;
                if ((nullable3.GetValueOrDefault() != -1M ? 1 : (!nullable3.HasValue ? 1 : 0)) != 0)
                {
                    Decimal? nullable4;
                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION != DeclaracionIsai.TiposDeclaraciones.Anticipada.ToDecimal())
                    {
                        if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].ACTOJURIDICO.ToUpper().Contains("HERENCIA") && dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODACTOJUR.Equals(3M))
                        {
                            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                                throw new Exception("No se definió la fecha causación.");
                            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
                                throw new Exception("No se definió la fecha de escritura");
                            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION > dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHAESCRITURA)
                                throw new Exception("La fecha causación no debe ser superior a la fecha de la escritura pública.");
                        }
                        if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODACTOJUR == 3M && dseDeclaracionIsai.FEXNOT_DECLARACION[0].REGLA == 0M)
                        {
                            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
                                throw new Exception("No se definió la fecha de escritura, no se puede realizar el cálculo");
                            nullable4 = new Decimal?(this.CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHAESCRITURA, valorReferido, fechaValorReferido, out resultadoRecargo, out resultadoImporteRecargo, out resultadoActualizacion, out resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995, out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha));
                        }
                        else if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull() && DateTime.Compare(dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION, new DateTime(1972, 1, 1)) >= 0)
                        {
                            nullable4 = new Decimal?(this.CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION, valorReferido, fechaValorReferido, out resultadoRecargo, out resultadoImporteRecargo, out resultadoActualizacion, out resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995, out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha));
                        }
                        else
                        {
                            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                                dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetFECHACAUSACIONNull();
                            nullable4 = new Decimal?(0M);
                        }
                    }
                    else
                        nullable4 = new Decimal?(this.CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, DateTime.Now, valorReferido, fechaValorReferido, out resultadoRecargo, out resultadoImporteRecargo, out resultadoActualizacion, out resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995, out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha));
                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
                        nullable2 = new Decimal?(dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO);
                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION == DeclaracionIsai.TiposDeclaraciones.Complementaria.ToDecimal())
                    {
                        if (nullable4.HasValue)
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = !dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull() ? (!ImpuestoAnteriorPagado.HasValue ? nullable4.Value : nullable4.Value - ImpuestoAnteriorPagado.Value) : nullable4.Value;
                        else
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetIMPORTEIMPUESTONull();
                    }
                    else if (nullable4.HasValue)
                    {
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = nullable4.Value;
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPUESTOPAGADO = nullable4.Value;
                    }
                    else
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetIMPORTEIMPUESTONull();
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCJERECARGOPAGOEXTEMP = resultadoRecargo.HasValue ? resultadoRecargo.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE = resultadoImporteActualizacion.HasValue ? resultadoImporteActualizacion.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJEINPC = resultadoActualizacion.HasValue ? resultadoActualizacion.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].BASEGRAVABLE = resultadoBaseGravable.HasValue ? resultadoBaseGravable.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].REDUCCION1995 = resultadoReduccion1995.HasValue ? resultadoReduccion1995.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].TASA1995 = resultadoTasa1995.HasValue ? resultadoTasa1995.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPUESTO = resultadoImpuesto.HasValue ? resultadoImpuesto.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].REDUCCIONART309 = resultadoReduccionArt309.HasValue ? resultadoReduccionArt309.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP = resultadoImporteRecargo.HasValue ? resultadoImporteRecargo.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTECONDONACION = resultadoImporteCondonacion.HasValue ? resultadoImporteCondonacion.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEEXENCION = resultadoExencionImporte.HasValue ? resultadoExencionImporte.Value : 0M;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].CONDONACIONXFECHA = resultadoImporteCondonacionxFecha.HasValue ? resultadoImporteCondonacionxFecha.Value : 0M;
                }
                if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() && ((IEnumerable<DseDeclaracionIsai.FEXNOT_CONDONACIONRow>)dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()).Count<DseDeclaracionIsai.FEXNOT_CONDONACIONRow>() == 1)
                {
                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetPORCENTAJEREDUCCIONNull();
                    else
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].PORCENTAJEREDUCCION = dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetFECHANull();
                    else
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].FECHA = dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACONDONACION;
                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetMOTIVONull();
                    else
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].MOTIVO = dseDeclaracionIsai.FEXNOT_DECLARACION[0].MOTIVOCONDONACION;
                }
                else if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() && ((IEnumerable<DseDeclaracionIsai.FEXNOT_CONDONACIONRow>)dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()).Count<DseDeclaracionIsai.FEXNOT_CONDONACIONRow>() == 0)
                {
                    DseDeclaracionIsai.FEXNOT_CONDONACIONRow row = dseDeclaracionIsai.FEXNOT_CONDONACION.NewFEXNOT_CONDONACIONRow();
                    row.FEXNOT_DECLARACIONRow = dseDeclaracionIsai.FEXNOT_DECLARACION[0];
                    row.PORCENTAJEREDUCCION = dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
                        row.FECHA = dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACONDONACION;
                    else
                        row.SetFECHANull();
                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
                        row.MOTIVO = dseDeclaracionIsai.FEXNOT_DECLARACION[0].MOTIVOCONDONACION;
                    else
                        row.SetMOTIVONull();
                    dseDeclaracionIsai.FEXNOT_CONDONACION.AddFEXNOT_CONDONACIONRow(row);
                }
                else if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() && ((IEnumerable<DseDeclaracionIsai.FEXNOT_CONDONACIONRow>)dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()).Count<DseDeclaracionIsai.FEXNOT_CONDONACIONRow>() == 1)
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].Delete();
                foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participantesRow in (TypedTableBase<DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow>)dseDeclaracionIsai.FEXNOT_PARTICIPANTES)
                {
                    if (participantesRow.RowState != DataRowState.Deleted && (!participantesRow.IsXMLDIRNull() && !string.IsNullOrEmpty(participantesRow.XMLDIR)))
                    {
                        if (participantesRow.RowState == DataRowState.Added)
                            participantesRow.IDDIRECCION = (Decimal)(int)this.ObtenerIdDireccion(transactionHelper, participantesRow.XMLDIR);//, idusuario);
                        else
                            this.ActualizarDireccion(transactionHelper, participantesRow.XMLDIR);//, idusuario);
                    }
                }
                if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Pendiente.ToDecimal() || dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Borrador.ToDecimal())
                {
                    str = dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull() ? this.ValidarDeclaracion(dseDeclaracionIsai, dseDeclaracionIsai.FEXNOT_DECLARACION[0], new DateTime?()) : this.ValidarDeclaracion(dseDeclaracionIsai, dseDeclaracionIsai.FEXNOT_DECLARACION[0], new DateTime?(dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION));
                    if (string.IsNullOrEmpty(str))
                    {
                        if (!string.IsNullOrEmpty(xmlDoc) && !string.IsNullOrEmpty(tipoDocumentoDigital))
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL = this.EspecificarDocumentoTransaccional(xmlDoc, tipoDocumentoDigital, listaFicheros, transactionHelper);
                        if (!string.IsNullOrEmpty(xmlDocBF) && !string.IsNullOrEmpty(tipoDocumentoDigitalBF))
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IDDOCBENFISCALES = this.EspecificarDocumentoTransaccional(xmlDocBF, tipoDocumentoDigitalBF, listaFicherosBF, transactionHelper);
                        this.DeclaracionTA.Update(dseDeclaracionIsai);
                        this.CondonacionTA.Update(dseDeclaracionIsai);
                        this.ParticipanteTA.Update(dseDeclaracionIsai);
                        if (!dseDeclaracionIsai.FEXNOT_DECLARACION.FirstOrDefault<DseDeclaracionIsai.FEXNOT_DECLARACIONRow>().IsCAUSAISRNull() && dseDeclaracionIsai.FEXNOT_DECLARACION.FirstOrDefault<DseDeclaracionIsai.FEXNOT_DECLARACIONRow>().CAUSAISR == 1M)
                        {
                            List<Decimal> valores = new List<Decimal>();
                            valores.AddRange((IEnumerable<Decimal>)valoresIsr);
                            Isr.Insert_Update(dseDeclaracionIsai.FEXNOT_DECLARACION.FirstOrDefault<DseDeclaracionIsai.FEXNOT_DECLARACIONRow>().IDDECLARACION, valores);
                        }
                        transactionHelper.Commit();
                        dseDeclaracion = dseDeclaracionIsai;
                    }
                    else
                        transactionHelper.RollBack();
                }
                else
                {
                    this.DeclaracionTA.Update(dseDeclaracionIsai);
                    this.CondonacionTA.Update(dseDeclaracionIsai);
                    this.ParticipanteTA.Update(dseDeclaracionIsai);
                    transactionHelper.Commit();
                }
                return str;
            }
            catch (CommunicationException ex)
            {
                transactionHelper.RollBack();
                ExceptionPolicyWrapper.HandleException((Exception)ex);
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                transactionHelper.RollBack();
                ExceptionPolicyWrapper.HandleException(ex);
                return ex.Message;
            }
            finally
            {
                transactionHelper?.Dispose();
            }
        }

        //public string RegistrarDeclaracion2(ref DseDeclaracionIsai dseDeclaracion, string xmlDoc, string tipoDocumentoDigital, string listaFicheros, string xmlDocBF, string tipoDocumentoDigitalBF, string listaFicherosBF, decimal? ImpuestoAnteriorPagado, decimal? valorReferido, DateTime? fechaValorReferido, decimal[] valoresIsr)
        //{
        //    decimal? impuestoCalculadoNuevoValor = null;
        //    decimal? impuestoCalculadoAnteriorValor = null;
        //    decimal? resultadoRecargo = null;
        //    decimal? resultadoActualizacion = null;
        //    decimal? resultadoImporteActualizacion = null;
        //    decimal? resultadoImporteRecargo = null;
        //    decimal? resultadoBaseGravable = null;
        //    decimal? resultadoReduccion1995 = null;
        //    decimal? resultadoTasa1995 = null;
        //    decimal? resultadoImpuesto = null;
        //    decimal? resultadoReduccionArt309 = null;
        //    decimal? resultadoExencionImporte = null;
        //    decimal? resultadoImporteCondonacion = null;
        //    decimal? resultadoImporteCondonacionxFecha = null;
        //    string mensajeValidacion = string.Empty;
        //    string mensajeAViso = string.Empty;

        //    TransactionHelper transactionHelper = null;
        //    DseDeclaracionIsai dseDeclaracionIsai = dseDeclaracion;
        //    try
        //    {
        //        transactionHelper = new TransactionHelper(DeclaracionTA.Connection);

        //        transactionHelper.BeginTransaction();

        //        DeclaracionTA.SetTransaction(transactionHelper);
        //        CondonacionTA.SetTransaction(transactionHelper);
        //        ParticipanteTA.SetTransaction(transactionHelper);

        //        //Obtenemos las declaraciones nuevas del DataTable y comprobamos que solo viene una declaración
        //        if (dseDeclaracionIsai.FEXNOT_DECLARACION == null || dseDeclaracionIsai.FEXNOT_DECLARACION.Count != 1)
        //            throw new Exception("Solo se puede registrar una Declaración");


        //       //calculo impuesto basado en los datos proporcionados en la declaracion
        //        if (valorReferido != -1 && dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION != 0)
        //        {
        //            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TiposDeclaraciones.Anticipada.ToDecimal())
        //            {

        //                if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODACTOJUR == 3 && dseDeclaracionIsai.FEXNOT_DECLARACION[0].REGLA == 0)//Si es herencia (3) y a parte a plican las reglas vigentes se envia la fecha de escritura, de lo contrario se envia la fecha causacion
        //                {
        //                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
        //                    {
        //                        impuestoCalculadoNuevoValor = CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHAESCRITURA, valorReferido, fechaValorReferido, out  resultadoRecargo, out  resultadoImporteRecargo,
        //                                                                              out  resultadoActualizacion, out   resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995,
        //                                                                              out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("No se definió la fecha de escritura, no se puede realizar el cálculo");
        //                    }
        //                }
        //                else
        //                {
        //                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull() && DateTime.Compare(dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION, new DateTime(1972, 01, 01)) >= 0)

        //                        impuestoCalculadoNuevoValor = CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION, valorReferido, fechaValorReferido, out  resultadoRecargo, out  resultadoImporteRecargo,
        //                                                                                  out  resultadoActualizacion, out   resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995,
        //                                                                                  out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);
        //                    else
        //                    {
        //                        if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
        //                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetFECHACAUSACIONNull();
        //                        impuestoCalculadoNuevoValor = 0;
        //                    }
        //                }
        //            }
        //            else
        //            //Declaracion anticipada
        //            {


        //                impuestoCalculadoNuevoValor = CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, DateTime.Now, valorReferido, fechaValorReferido, out  resultadoRecargo, out  resultadoImporteRecargo,
        //                                                                          out  resultadoActualizacion, out   resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995,
        //                                                                          out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);
        //            }

        //            if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
        //                impuestoCalculadoAnteriorValor = dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO;

        //            //Establecemos el importes calculados

        //            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TiposDeclaraciones.Complementaria.ToDecimal())    //complementaria
        //            {
        //                if (impuestoCalculadoNuevoValor.HasValue)
        //                {
        //                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
        //                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value;
        //                    else
        //                    {
        //                        if (ImpuestoAnteriorPagado.HasValue)
        //                            //restar el importe calculado del importe pagado originalmente
        //                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value - ImpuestoAnteriorPagado.Value;
        //                        else
        //                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value;
        //                    }
        //                }
        //                else
        //                {
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetIMPORTEIMPUESTONull();
        //                }
        //            }
        //            else
        //            {
        //                if (impuestoCalculadoNuevoValor.HasValue)
        //                {
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value;
        //                    //if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
        //                    //{
        //                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPUESTOPAGADO = impuestoCalculadoNuevoValor.Value;
        //                    //}
        //                }
        //                else
        //                {
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetIMPORTEIMPUESTONull();
        //                }
        //            }
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCJERECARGOPAGOEXTEMP = resultadoRecargo.HasValue ? resultadoRecargo.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE = resultadoImporteActualizacion.HasValue ? resultadoImporteActualizacion.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJEINPC = resultadoActualizacion.HasValue ? resultadoActualizacion.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].BASEGRAVABLE = resultadoBaseGravable.HasValue ? resultadoBaseGravable.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].REDUCCION1995 = resultadoReduccion1995.HasValue ? resultadoReduccion1995.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].TASA1995 = resultadoTasa1995.HasValue ? resultadoTasa1995.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPUESTO = resultadoImpuesto.HasValue ? resultadoImpuesto.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].REDUCCIONART309 = resultadoReduccionArt309.HasValue ? resultadoReduccionArt309.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP = resultadoImporteRecargo.HasValue ? resultadoImporteRecargo.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTECONDONACION = resultadoImporteCondonacion.HasValue ? resultadoImporteCondonacion.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEEXENCION = resultadoExencionImporte.HasValue ? resultadoExencionImporte.Value : 0;
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].CONDONACIONXFECHA = resultadoImporteCondonacionxFecha.HasValue ? resultadoImporteCondonacionxFecha.Value : 0;
        //        }

        //        #region [ Insertamos, actualizamos o eliminamos la condonacion. ]
        //        if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() &&
        //             dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows().Count() == 1)
        //        {

        //            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
        //                dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetPORCENTAJEREDUCCIONNull();
        //            else
        //                dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].PORCENTAJEREDUCCION =
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;

        //            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
        //                dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetFECHANull();
        //            else
        //                dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].FECHA =
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACONDONACION;

        //            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
        //                dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetMOTIVONull();
        //            else
        //                dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].MOTIVO =
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].MOTIVOCONDONACION;

        //        }
        //        else if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() &&
        //             dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows().Count() == 0)
        //        {
        //            //TODO: Crear fila condonacion, relacion con declaracion y añadir a la tabla fexnot_condonacion
        //            DseDeclaracionIsai.FEXNOT_CONDONACIONRow rowCondonacion = dseDeclaracionIsai.FEXNOT_CONDONACION.NewFEXNOT_CONDONACIONRow();
        //            rowCondonacion.FEXNOT_DECLARACIONRow = dseDeclaracionIsai.FEXNOT_DECLARACION[0];
        //            rowCondonacion.PORCENTAJEREDUCCION = dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
        //            if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
        //            {
        //                rowCondonacion.FECHA = dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACONDONACION;
        //            }
        //            else
        //            {
        //                rowCondonacion.SetFECHANull();
        //            }
        //            if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
        //            {
        //                rowCondonacion.MOTIVO = dseDeclaracionIsai.FEXNOT_DECLARACION[0].MOTIVOCONDONACION;
        //            }
        //            else
        //            {
        //                rowCondonacion.SetMOTIVONull();
        //            }
        //            dseDeclaracionIsai.FEXNOT_CONDONACION.AddFEXNOT_CONDONACIONRow(rowCondonacion);
        //        }
        //        else if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() &&
        //             dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows().Count() == 1)
        //        {
        //            dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].Delete();
        //        }
        //        #endregion



        //        #region [ Direccion ]
        //        foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participanteDr in dseDeclaracionIsai.FEXNOT_PARTICIPANTES)
        //        {
        //            if (participanteDr.RowState != DataRowState.Deleted)
        //            {
        //                if (!participanteDr.IsXMLDIRNull() && !string.IsNullOrEmpty(participanteDr.XMLDIR))
        //                    if (participanteDr.RowState == DataRowState.Added)
        //                    {
        //                        participanteDr.IDDIRECCION = (int)ObtenerIdDireccion(transactionHelper, participanteDr.XMLDIR);
        //                    }
        //                    else
        //                    {
        //                        ActualizarDireccion(transactionHelper, participanteDr.XMLDIR);
        //                    }
        //            }
        //        }
        //        #endregion

        //        //ESTADO DE LA DECLARACION 
        //        //JAMG
        //        //CODIGO AGREGADO POR JAMG
        //        //DONDE VERIFICO QUE LA DECLARACION SEA ESTATUS
        //        // SOLO AGREGUE LA "OR"
        //        // 0 = BORRADOR
        //        // 1 = PENDIENTE
        //        // 2 = PRESENTADA
        //        //if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Pendiente.ToDecimal()) //CODIGO ORIGINAL
        //        if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Pendiente.ToDecimal() || dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Borrador.ToDecimal())
        //        {

        //            if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
        //            {
        //                mensajeValidacion = ValidarDeclaracion(dseDeclaracionIsai, dseDeclaracionIsai.FEXNOT_DECLARACION[0], dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION);
        //            }
        //            else
        //            {
        //                mensajeValidacion = ValidarDeclaracion(dseDeclaracionIsai, dseDeclaracionIsai.FEXNOT_DECLARACION[0], null);
        //            }
        //            if (string.IsNullOrEmpty(mensajeValidacion))
        //            {


        //                if ((!string.IsNullOrEmpty(xmlDoc)) && (!string.IsNullOrEmpty(tipoDocumentoDigital)))
        //                    //llamada al metodo que contiene la invocacion de un metodo del servicio documental de forma transaccional
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL = EspecificarDocumentoTransaccional(xmlDoc, tipoDocumentoDigital, listaFicheros, transactionHelper);


        //                if ((!string.IsNullOrEmpty(xmlDocBF)) && (!string.IsNullOrEmpty(tipoDocumentoDigitalBF)))
        //                    //llamada al metodo que contiene la invocacion de un metodo del servicio documental de forma transaccional
        //                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IDDOCBENFISCALES = EspecificarDocumentoTransaccional(xmlDocBF, tipoDocumentoDigitalBF, listaFicherosBF, transactionHelper);


        //                //Guardo ya la declaracion en la base de Datps
        //                DeclaracionTA.Update(dseDeclaracionIsai);
        //                CondonacionTA.Update(dseDeclaracionIsai);
        //                ParticipanteTA.Update(dseDeclaracionIsai);

        //                if (!dseDeclaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IsCAUSAISRNull() && dseDeclaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().CAUSAISR == 1)
        //                {
        //                    List<decimal> listaValores = new List<decimal>();
        //                    listaValores.AddRange(valoresIsr);
        //                    Isr.Insert_Update(dseDeclaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IDDECLARACION, listaValores);
        //                }
        //                transactionHelper.Commit();
        //                dseDeclaracion = dseDeclaracionIsai;

        //            }
        //            else
        //            {
        //                transactionHelper.RollBack();
        //            }
        //        }
        //        else
        //        {
        //            DeclaracionTA.Update(dseDeclaracionIsai);
        //            CondonacionTA.Update(dseDeclaracionIsai);
        //            ParticipanteTA.Update(dseDeclaracionIsai);
        //            transactionHelper.Commit();
        //        }

        //        return mensajeValidacion;
        //    }
        //    catch (System.ServiceModel.CommunicationException cex)
        //    {
        //        transactionHelper.RollBack();

        //        ExceptionPolicyWrapper.HandleException(cex);

        //        throw new Exception(cex.ToString());//new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensajeValidacion));
        //    }
        //    catch (Exception ex)
        //    {
        //        transactionHelper.RollBack();

        //        ExceptionPolicyWrapper.HandleException(ex);

        //        string mensaje = null;

        //        System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
        //        if (faultEx != null)
        //        {
        //            if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
        //                mensaje = faultEx.Detail.Descripcion;
        //            else
        //                mensaje = "Se produjo un error al realizar la operación";
        //        }
        //        else
        //        {
        //            if (ex.InnerException != null)
        //            {
        //                mensaje = ex.InnerException.Message;
        //            }
        //            else
        //            {
        //                mensaje = ex.Message;
        //            }
        //        }

        //        throw new Exception(ex.ToString());// throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));
        //    }
        //    finally
        //    {
        //        if (transactionHelper != null)
        //            transactionHelper.Dispose();

        //    }
        //}

        public string RegistrarDeclaracion(ref DseDeclaracionIsai dseDeclaracionIsai, string xmlDoc, string tipoDocumentoDigital, string listaFicheros, string xmlDocBF, string tipoDocumentoDigitalBF, string listaFicherosBF, decimal? ImpuestoAnteriorPagado, decimal? valorReferido, DateTime? fechaValorReferido)
        {
            decimal? impuestoCalculadoNuevoValor = null;
            decimal? impuestoCalculadoAnteriorValor = null;
            decimal? resultadoRecargo = null;
            decimal? resultadoActualizacion = null;
            decimal? resultadoImporteActualizacion = null;
            decimal? resultadoImporteRecargo = null;
            decimal? resultadoBaseGravable = null;
            decimal? resultadoReduccion1995 = null;
            decimal? resultadoTasa1995 = null;
            decimal? resultadoImpuesto = null;
            decimal? resultadoReduccionArt309 = null;
            decimal? resultadoExencionImporte = null;
            decimal? resultadoImporteCondonacion = null;
            decimal? resultadoImporteCondonacionxFecha = null;
            string mensajeValidacion = string.Empty;
            string mensajeAViso = string.Empty;

            TransactionHelper transactionHelper = null;

            try
            {
                transactionHelper = new TransactionHelper(DeclaracionTA.Connection);

                transactionHelper.BeginTransaction();

                DeclaracionTA.SetTransaction(transactionHelper);
                CondonacionTA.SetTransaction(transactionHelper);
                ParticipanteTA.SetTransaction(transactionHelper);

                //Obtenemos las declaraciones nuevas del DataTable y comprobamos que solo viene una declaración
                if (dseDeclaracionIsai.FEXNOT_DECLARACION == null || dseDeclaracionIsai.FEXNOT_DECLARACION.Count != 1)
                    throw new Exception("Solo se puede registrar una Declaración");


                //calculo impuesto basado en los datos proporcionados en la declaracion
                if (valorReferido != -1 && dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION != 0)
                {
                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION != TiposDeclaraciones.Anticipada.ToDecimal())
                    {

                        if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODACTOJUR == 3 && dseDeclaracionIsai.FEXNOT_DECLARACION[0].REGLA == 0)//Si es herencia (3) y a parte a plican las reglas vigentes se envia la fecha de escritura, de lo contrario se envia la fecha causacion
                        {
                            if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHAESCRITURANull())
                            {
                                impuestoCalculadoNuevoValor = CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHAESCRITURA, valorReferido, fechaValorReferido, out resultadoRecargo, out resultadoImporteRecargo,
                                                                                      out resultadoActualizacion, out resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995,
                                                                                      out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);
                            }
                            else
                            {
                                throw new Exception("No se definió la fecha de escritura, no se puede realizar el cálculo");
                            }
                        }
                        else
                        {

                            if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull() && DateTime.Compare(dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION, new DateTime(1972, 01, 01)) >= 0)

                                impuestoCalculadoNuevoValor = CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION, valorReferido, fechaValorReferido, out resultadoRecargo, out resultadoImporteRecargo,
                                                                                          out resultadoActualizacion, out resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995,
                                                                                          out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);
                            else
                            {
                                if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetFECHACAUSACIONNull();
                                impuestoCalculadoNuevoValor = 0;
                            }
                        }
                    }
                    else
                    //Declaracion anticipada
                    {


                        impuestoCalculadoNuevoValor = CalcularImpuestoDeclaracion(dseDeclaracionIsai.FEXNOT_DECLARACION, DateTime.Now, valorReferido, fechaValorReferido, out resultadoRecargo, out resultadoImporteRecargo,
                                                                                  out resultadoActualizacion, out resultadoImporteActualizacion, out resultadoBaseGravable, out resultadoReduccion1995, out resultadoTasa1995,
                                                                                  out resultadoImpuesto, out resultadoReduccionArt309, out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);
                    }

                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPORTEIMPUESTONull())
                        impuestoCalculadoAnteriorValor = dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO;

                    //Establecemos el importes calculados

                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODTIPODECLARACION == TiposDeclaraciones.Complementaria.ToDecimal())    //complementaria
                    {
                        if (impuestoCalculadoNuevoValor.HasValue)
                        {
                            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                                dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value;
                            else
                            {
                                if (ImpuestoAnteriorPagado.HasValue)
                                    //restar el importe calculado del importe pagado originalmente
                                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value - ImpuestoAnteriorPagado.Value;
                                else
                                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value;
                            }
                        }
                        else
                        {
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetIMPORTEIMPUESTONull();
                        }
                    }
                    else
                    {
                        if (impuestoCalculadoNuevoValor.HasValue)
                        {
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEIMPUESTO = impuestoCalculadoNuevoValor.Value;
                            if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsIMPUESTOPAGADONull())
                            {
                                dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPUESTOPAGADO = impuestoCalculadoNuevoValor.Value;
                            }
                        }
                        else
                        {
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].SetIMPORTEIMPUESTONull();
                        }
                    }
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCJERECARGOPAGOEXTEMP = resultadoRecargo.HasValue ? resultadoRecargo.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE = resultadoImporteActualizacion.HasValue ? resultadoImporteActualizacion.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJEINPC = resultadoActualizacion.HasValue ? resultadoActualizacion.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].BASEGRAVABLE = resultadoBaseGravable.HasValue ? resultadoBaseGravable.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].REDUCCION1995 = resultadoReduccion1995.HasValue ? resultadoReduccion1995.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].TASA1995 = resultadoTasa1995.HasValue ? resultadoTasa1995.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPUESTO = resultadoImpuesto.HasValue ? resultadoImpuesto.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].REDUCCIONART309 = resultadoReduccionArt309.HasValue ? resultadoReduccionArt309.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP = resultadoImporteRecargo.HasValue ? resultadoImporteRecargo.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTECONDONACION = resultadoImporteCondonacion.HasValue ? resultadoImporteCondonacion.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].IMPORTEEXENCION = resultadoExencionImporte.HasValue ? resultadoExencionImporte.Value : 0;
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].CONDONACIONXFECHA = resultadoImporteCondonacionxFecha.HasValue ? resultadoImporteCondonacionxFecha.Value : 0;
                }

                #region [ Insertamos, actualizamos o eliminamos la condonacion. ]
                if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() &&
                     dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows().Count() == 1)
                {

                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull())
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetPORCENTAJEREDUCCIONNull();
                    else
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].PORCENTAJEREDUCCION =
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;

                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetFECHANull();
                    else
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].FECHA =
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACONDONACION;

                    if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].SetMOTIVONull();
                    else
                        dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].MOTIVO =
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].MOTIVOCONDONACION;

                }
                else if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() &&
                     dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows().Count() == 0)
                {
                    //TODO: Crear fila condonacion, relacion con declaracion y añadir a la tabla fexnot_condonacion
                    DseDeclaracionIsai.FEXNOT_CONDONACIONRow rowCondonacion = dseDeclaracionIsai.FEXNOT_CONDONACION.NewFEXNOT_CONDONACIONRow();
                    rowCondonacion.FEXNOT_DECLARACIONRow = dseDeclaracionIsai.FEXNOT_DECLARACION[0];
                    rowCondonacion.PORCENTAJEREDUCCION = dseDeclaracionIsai.FEXNOT_DECLARACION[0].PORCENTAJECONDONACION;
                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACONDONACIONNull())
                    {
                        rowCondonacion.FECHA = dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACONDONACION;
                    }
                    else
                    {
                        rowCondonacion.SetFECHANull();
                    }
                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsMOTIVOCONDONACIONNull())
                    {
                        rowCondonacion.MOTIVO = dseDeclaracionIsai.FEXNOT_DECLARACION[0].MOTIVOCONDONACION;
                    }
                    else
                    {
                        rowCondonacion.SetMOTIVONull();
                    }
                    dseDeclaracionIsai.FEXNOT_CONDONACION.AddFEXNOT_CONDONACIONRow(rowCondonacion);
                }
                else if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsPORCENTAJECONDONACIONNull() &&
                     dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows().Count() == 1)
                {
                    dseDeclaracionIsai.FEXNOT_DECLARACION[0].GetFEXNOT_CONDONACIONRows()[0].Delete();
                }
                #endregion



                #region [ Direccion ]
                foreach (DseDeclaracionIsai.FEXNOT_PARTICIPANTESRow participanteDr in dseDeclaracionIsai.FEXNOT_PARTICIPANTES)
                {
                    if (participanteDr.RowState != DataRowState.Deleted)
                    {
                        if (!participanteDr.IsXMLDIRNull() && !string.IsNullOrEmpty(participanteDr.XMLDIR))
                            if (participanteDr.RowState == DataRowState.Added)
                            {
                                participanteDr.IDDIRECCION = (int)ObtenerIdDireccion(transactionHelper, participanteDr.XMLDIR);
                            }
                            else
                            {
                                ActualizarDireccion(transactionHelper, participanteDr.XMLDIR);
                            }
                    }
                }
                #endregion

                //ESTADO DE LA DECLARACION 
                //JAMG
                //CODIGO AGREGADO POR JAMG
                //DONDE VERIFICO QUE LA DECLARACION SEA ESTATUS
                // SOLO AGREGUE LA "OR"
                // 0 = BORRADOR
                // 1 = PENDIENTE
                // 2 = PRESENTADA
                //if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Pendiente.ToDecimal()) //CODIGO ORIGINAL
                if (dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Pendiente.ToDecimal() || dseDeclaracionIsai.FEXNOT_DECLARACION[0].CODESTADODECLARACION == EstadoDeclaracionEnum.Borrador.ToDecimal())
                {

                    if (!dseDeclaracionIsai.FEXNOT_DECLARACION[0].IsFECHACAUSACIONNull())
                    {
                        mensajeValidacion = ValidarDeclaracion(dseDeclaracionIsai, dseDeclaracionIsai.FEXNOT_DECLARACION[0], dseDeclaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION);
                    }
                    else
                    {
                        mensajeValidacion = ValidarDeclaracion(dseDeclaracionIsai, dseDeclaracionIsai.FEXNOT_DECLARACION[0], null);
                    }
                    if (string.IsNullOrEmpty(mensajeValidacion))
                    {


                        if ((!string.IsNullOrEmpty(xmlDoc)) && (!string.IsNullOrEmpty(tipoDocumentoDigital)))
                            //llamada al metodo que contiene la invocacion de un metodo del servicio documental de forma transaccional
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IDDOCUMENTODIGITAL = EspecificarDocumentoTransaccional(xmlDoc, tipoDocumentoDigital, listaFicheros, transactionHelper);


                        if ((!string.IsNullOrEmpty(xmlDocBF)) && (!string.IsNullOrEmpty(tipoDocumentoDigitalBF)))
                            //llamada al metodo que contiene la invocacion de un metodo del servicio documental de forma transaccional
                            dseDeclaracionIsai.FEXNOT_DECLARACION[0].IDDOCBENFISCALES = EspecificarDocumentoTransaccional(xmlDocBF, tipoDocumentoDigitalBF, listaFicherosBF, transactionHelper);


                        //Guardo ya la declaracion en la base de Datps
                        DeclaracionTA.Update(dseDeclaracionIsai);
                        CondonacionTA.Update(dseDeclaracionIsai);
                        ParticipanteTA.Update(dseDeclaracionIsai);

                        transactionHelper.Commit();
                    }
                    else
                    {
                        transactionHelper.RollBack();
                    }
                }
                else
                {
                    DeclaracionTA.Update(dseDeclaracionIsai);
                    CondonacionTA.Update(dseDeclaracionIsai);
                    ParticipanteTA.Update(dseDeclaracionIsai);
                    transactionHelper.Commit();
                }

                return mensajeValidacion;
            }
            catch (System.ServiceModel.CommunicationException cex)
            {
                transactionHelper.RollBack();

                ExceptionPolicyWrapper.HandleException(cex);

                throw new Exception(cex.ToString());//new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensajeValidacion));
            }
            catch (Exception ex)
            {
                transactionHelper.RollBack();

                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (ex.InnerException != null)
                    {
                        mensaje = ex.InnerException.Message;
                    }
                    else
                    {
                        mensaje = ex.Message;
                    }
                }

                throw new Exception(ex.ToString());// throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));
            }
            finally
            {
                if (transactionHelper != null)
                    transactionHelper.Dispose();

            }
        }



        /// <summary>
        /// Función que inserta en RCON la dirección especificada
        /// </summary>
        /// <param name="trans">Transacción</param>
        /// <param name="xml">Cadena de texto del xml que contiene la dirección a insertar</param>
        /// <returns>Identificador de la dirección introducida</returns>
        private Decimal ObtenerIdDireccion(TransactionHelper trans, string xml)
        {
            SIGAPred.RegistroContribuyentes.Services.AccesoDatos.DseInfoDirecciones dseDireccionRcon = new SIGAPred.RegistroContribuyentes.Services.AccesoDatos.DseInfoDirecciones();
            RegistroContribuyentesService registroCon = new RegistroContribuyentesService();
            try
            {
                dseDireccionRcon = registroCon.InsertarDireccionXML(trans, xml);
                return dseDireccionRcon.Direccion[0].IDDIRECCION;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString()); //throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        /// <summary>
        /// Actualiza la dirección de RCON
        /// </summary>
        /// <param name="trans">Transacción</param>
        /// <param name="xml">Cadena de texto del xml con la dirección a actualizar</param>
        private void ActualizarDireccion(TransactionHelper trans, string xml)
        {
            SIGAPred.RegistroContribuyentes.Services.AccesoDatos.DseInfoDirecciones dseDireccionRcon = new SIGAPred.RegistroContribuyentes.Services.AccesoDatos.DseInfoDirecciones();
            RegistroContribuyentesService registroCon = new RegistroContribuyentesService();
            try
            {
                registroCon.ActualizarDireccionXML(trans, xml);
            }
            catch (Exception ex)
            {
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        #endregion

        #region Métodos modificar


        /// <summary>
        /// Modifica un participante de la declaración
        /// </summary>
        /// <param name="participante">DataTable con el participante</param>
        public void ModificarParticipanteDeclaracion(DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable participante)
        {
            try
            {
                ParticipanteTA.Update(participante[0].IDPERSONA,
                                      participante[0].IDDECLARACION,
                                      participante[0].PORCTRANSMISION,
                                      participante[0].CODROL,
                                      participante[0].IDDIRECCION,
                                      participante[0].CODTIPOPERSONA,
                                      participante[0].CODSITUACIONESPERSONA,
                                      participante[0].NOMBRE,
                                      participante[0].APELLIDOPATERNO,
                                      participante[0].APELLIDOMATERNO,
                                      participante[0].RFC,
                                      participante[0].CURP,
                                      participante[0].CLAVEIFE,
                                      participante[0].ACTIVPRINCIP);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        /// <summary>
        /// Modifica el estado de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="codEstadoDeclaracion">Código del estado que se va a modificar en la declaración</param>
        public void ModificarEstadoDeclaracion(decimal idDeclaracion, decimal codEstadoDeclaracion)
        {
            try
            {
                int r = 0;

                r = DeclaracionTA.UPDATE_ESTADODECLARACION(idDeclaracion, codEstadoDeclaracion, null);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        #endregion

        #region Métodos obtener


        public decimal ObtenerPorcentajeReduccion(decimal idReduccion)
        {
            OracleConnection connection = null;
            try
            {
                decimal myField = 0M;
                string connectionString = SecurityCore.SecurityCoreManager.getStringConnection("FEXNOT");
                using (connection = new OracleConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    //OracleCommand command = connection.CreateCommand();
                    //string sql = string.Format("SELECT * FROM FEXNOT_REDUCCIONES WHERE IDREDUCCION = {0}", idReduccion);
                    //command.CommandText = sql;

                    //OracleDataReader reader = command.ExecuteReader();
                    //if (reader.Read())
                    //{
                    //    //myField = (decimal)reader["DESCUENTO"];
                    //    OracleString oraclestring1 = reader.GetOracleString(0);
                    //    OracleString oraclestring2 = reader.GetOracleString(1);
                    //    OracleString oraclestring3 = reader.GetOracleString(2);

                    //}
                    OracleDataAdapter adapter = new OracleDataAdapter(string.Format("SELECT * FROM FEXNOT_REDUCCIONES WHERE IDREDUCCION = {0}", idReduccion), connection);
                    DataSet set = new DataSet();
                    adapter.Fill(set);
                    if (set.Tables != null)
                        if (set.Tables[0].Rows != null)
                            if (set.Tables[0].Rows[0].ItemArray[1] != null)
                                myField = set.Tables[0].Rows[0].ItemArray[1].ToDecimal();

                    return myField;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }


        public string ObtenerArticulo(decimal idReduccion)
        {
            OracleConnection connection = null;
            try
            {
                string myField = string.Empty;

                string connectionString = SecurityCore.SecurityCoreManager.getStringConnection("FEXNOT"); ;
                using (connection = new OracleConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    //OracleCommand command = connection.CreateCommand();
                    //string sql = string.Format("SELECT * FROM FEXNOT_REDUCCIONES WHERE IDREDUCCION = {0}", idReduccion);
                    //command.CommandText = sql;

                    //OracleDataReader reader = command.ExecuteReader();
                    //if (reader.Read())
                    //{
                    //    //myField = (decimal)reader["DESCUENTO"];
                    //    OracleString oraclestring1 = reader.GetOracleString(0);
                    //    OracleString oraclestring2 = reader.GetOracleString(1);
                    //    OracleString oraclestring3 = reader.GetOracleString(2);

                    //}
                    OracleDataAdapter adapter = new OracleDataAdapter(string.Format("SELECT * FROM FEXNOT_REDUCCIONES WHERE IDREDUCCION = {0}", idReduccion), connection);
                    DataSet set = new DataSet();
                    adapter.Fill(set);
                    if (set.Tables != null)
                        if (set.Tables[0].Rows != null)
                            if (set.Tables[0].Rows[0].ItemArray[1] != null)
                                myField = set.Tables[0].Rows[0].ItemArray[2].ToString();

                    return myField;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Obtener de BD Número Presentación de una declaración concreta
        /// </summary>
        /// <param name="idDeclaracion">id identificativo de la declaración en cuestión</param>
        /// <returns>string con el número presentación</returns>
        public string ObtenerNumeroPresentacionDec(decimal idDeclaracion)
        {
            try
            {
                string numeroPresent;
                DeclaracionTA.ObtenerNumPresentacion_byId(idDeclaracion, out numeroPresent);
                return numeroPresent.Trim();
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        /// <summary>
        /// Devolvemos el dataset de Catálogos Cargado
        /// </summary>
        /// <returns>DataSet de Catálogos</returns>
        public DseCatalogo ObtenerCatalogos()
        {
            try
            {
                object cursor = null;
                //instancio el dataSet de mantenimiento de condominios
                DseCatalogo dseCatalogos = new DseCatalogo();

                CatEstadoPagoTA.Fill(dseCatalogos.FEXNOT_CATESTADOPAGO, out cursor);
                CatProcedenciaTA.Fill(dseCatalogos.FEXNOT_CATPROCEDENCIA, out cursor);
                CatEstadoDeclaracionTA.Fill(dseCatalogos.FEXNOT_CATESTADDECLARACION, out cursor);
                CatActoJuridicoTA.Fill(dseCatalogos.FEXNOT_CATACTOSJURIDICOS, out cursor);
                CatRolesTA.Fill(dseCatalogos.FEXNOT_CATROLES, out cursor);
                JornadaNotarialTA.Fill(dseCatalogos.FEXNOT_JORNADANOTARIAL, out cursor);
                ExencionTA.Fill(dseCatalogos.FEXNOT_EXENCIONES, out cursor);
                ReduccionTA.Fill(dseCatalogos.FEXNOT_REDUCCIONES, out cursor);
                CatTiposDeclaracionTA.Fill(dseCatalogos.FEXNOT_CATTIPOSDECLARACION, out cursor);
                CatCalendarioTA.Fill(dseCatalogos.FEXNOT_CATCALENDARIO, out cursor);
                CatMotivoRechazoTA.Fill(dseCatalogos.FEXNOT_CATMOTIVORECHAZO, out cursor);
                CatBancoTA.Fill(dseCatalogos.FEXNOT_CATBANCO, out cursor);


                return dseCatalogos;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }


        /// <summary>
        /// Devuelve el dataTable con los actos jurídicos
        /// </summary>
        /// <returns>DataTable con los actos jurídicos</returns>
        private DseCatalogo.FEXNOT_CATACTOSJURIDICOSDataTable ObtenerCatalogoActosJuridicos()
        {
            object cursor = null;
            //instancio el dataSet de mantenimiento de condominios
            DseCatalogo dseCatalogos = new DseCatalogo();

            CatActoJuridicoTA.Fill(dseCatalogos.FEXNOT_CATACTOSJURIDICOS, out cursor);

            return dseCatalogos.FEXNOT_CATACTOSJURIDICOS;
        }

        /// <summary>
        /// Devuelve el dataTable con los roles de los actos jurídicos
        /// </summary>
        /// <returns>DataTable con los roles de los actos jurídicos</returns>
        private DseCatalogo.FEXNOT_CATROLESDataTable ObtenerCatalogoRoles()
        {
            object cursor = null;
            //instancio el dataSet de mantenimiento de condominios
            DseCatalogo dseCatalogos = new DseCatalogo();

            CatRolesTA.Fill(dseCatalogos.FEXNOT_CATROLES, out cursor);

            return dseCatalogos.FEXNOT_CATROLES;
        }

        /// <summary>
        /// Obtiene una búsqueda de reducciones por año , artículo y descripción
        /// </summary>
        /// <param name="anio">Año</param>
        /// <param name="articulo">Artículo</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las reducciones obtenidas</returns>
        public DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerBusquedaPorReduccion(int anio, decimal? articulo, string descripcion, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object c_reduccion = null;
                DseCatalogo.FEXNOT_REDUCCIONESDataTable reduccioneDataTable;

                rowsTotal = 0;

                reduccioneDataTable = ReduccionTA.GetDataByReduccionesBuscar(anio, (decimal?)articulo, descripcion, (decimal?)pageSize, (decimal?)indice, SortExpression, out c_reduccion);

                if (reduccioneDataTable.Count > 0)
                    rowsTotal = Convert.ToInt32(reduccioneDataTable[0].ROWS_TOTAL);
                return reduccioneDataTable;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene una búsqueda de exenciones por año , artículo y descripción
        /// </summary>
        /// <param name="anio">Año</param>
        /// <param name="articulo">Artículo</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        public DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerBusquedaPorExenciones(int anio, decimal? articulo, string descripcion, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object c_exencion = null;
                DseCatalogo.FEXNOT_EXENCIONESDataTable exencionDataTable;

                rowsTotal = 0;

                exencionDataTable = ExencionTA.GetDataByExencionesBuscar(anio, (decimal?)articulo, descripcion, (decimal?)pageSize, (decimal?)indice, SortExpression, out c_exencion);

                if (exencionDataTable.Count > 0)
                    rowsTotal = Convert.ToInt32(exencionDataTable[0].ROWS_TOTAL);
                return exencionDataTable;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }


        /// <summary>
        /// Obtiene el catálogo de estados de las declaraciones
        /// </summary>
        /// <returns>DataTable con los estados de la declaraciones</returns>
        public DseCatalogo.FEXNOT_CATESTADDECLARACIONDataTable ObtenerCatEstadoDeclaraciones()
        {
            try
            {
                object C_CATESTADODECLARACION = null;

                return CatEstadoDeclaracionTA.GetData(out C_CATESTADODECLARACION);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene las declaraciones pertenecientes a un avalúo especificado
        /// </summary>
        /// <param name="idAvaluo">Identificador del avalúo</param>
        /// <param name="idPersona">Identificador del notario</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesDeAvaluo(int idAvaluo, int idPersona, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;

                // JABS, Aqui es donde no trae la info de las declaraciones, se requiere revisar el ID de Persona.
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT =
                    DeclaracionTA.GetDataByIdAvaluo
                    (
                        idAvaluo,
                        idPersona,
                        (decimal?)pageSize,
                        (decimal?)indice,
                        SortExpression,
                        out C_DECLARACION
                    );

                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }



        /// <summary>
        /// Obtiene las declaraciones pertenecientes a un avalúo especificado consultado desde la secretaría de finanzas
        /// </summary>
        /// <param name="idAvaluo">Identificador del avalúo</param>
        /// <param name="idPersona">Identificador de persona</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesDeAvaluoSF(int idAvaluo, int idPersona, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByIdAvaSF(idAvaluo, idPersona, (decimal?)pageSize, (decimal?)indice, SortExpression, out C_DECLARACION);

                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }


        /// <summary>
        /// Obtiene un DataTable con la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con la declaración</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorIdDeclaracion(int idDeclaracion)
        {
            try
            {
                object C_DECLARACION = null;



                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByIdDeclaracion(idDeclaracion, out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}

                return declaracionDT;


            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        /// <summary>
        /// Regresa la lista de registros que estan en un estado de pago especificado
        /// </summary>
        /// <param name="estadoPago">Estado del pago</param>
        /// <returns>DataSet de la declaración</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorEstadoDePago(Negocio.Interfaces.EstadoPago estadoPago, int startrow, int endrow)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByDeclaracionesPorEstadoDePago(estadoPago.ToDecimal(), startrow.ToDecimal(), endrow.ToDecimal(), out C_DECLARACION);

                return declaracionDT;


            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene el dataSet Completo mediante la busqueda por idDeclaracion
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>Dataset con la declaración</returns>
        public DseDeclaracionIsai ObtenerDseDeclaracionIsaiPorIdDeclaracion(decimal idDeclaracion)
        {
            try
            {
                object C_DECLARACION = null;
                DseDeclaracionIsai dse = new DseDeclaracionIsai();

                DeclaracionTA.FillByIdDeclaracion(dse.FEXNOT_DECLARACION, idDeclaracion, out C_DECLARACION);
                CondonacionTA.FillByIdDeclaracion(dse.FEXNOT_CONDONACION, idDeclaracion, out C_DECLARACION);
                ParticipanteTA.FillByParticipantesByIdDeclaracion(dse.FEXNOT_PARTICIPANTES, idDeclaracion, out C_DECLARACION);

                return dse;
            }
            catch (System.ServiceModel.CommunicationException cex)
            {
                ExceptionPolicyWrapper.HandleException(cex);

                string mensaje = null;
                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = cex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (cex.InnerException != null)
                        mensaje = cex.InnerException.Message;
                    else
                        mensaje = cex.Message;
                }

                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (ex.InnerException != null)
                        mensaje = ex.InnerException.Message;
                    else
                        mensaje = ex.Message;
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));

            }
        }


        /// <summary>
        /// Obtiene un dataTable con el domicilio
        /// </summary>
        /// <param name="idDireccion">Identificador de la dirección</param>
        /// <returns>DataTable con el domicilio</returns>
        public DseDeclaracionIsai.FEXNOT_DOMICILIODataTable ObtenerDomicilioParticipante(decimal idDireccion)
        {
            try
            {
                object cursor = null;
                return DomicilioTA.GetDataById(idDireccion, out cursor);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaraciones de un tipo de declaración
        /// </summary>
        /// <param name="codTipoDeclaracion">Código del tipo de declaración</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorTipo(int codTipoDeclaracion)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByCodTipoDeclaracion(codTipoDeclaracion, out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }



        /// <summary>
        /// Obtiene las declaraciones de tipo jornada notarial por fecha de presentación
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="fechaPresentacionInicial">Fecha inicial</param>
        /// <param name="fechaPresentacionFinal">Fecha final</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorFechaPresentacionTipoJornadaNotarial(int idnotario, DateTime fechaPresentacionInicial, DateTime fechaPresentacionFinal, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByFechaPresentacionJornadaNotarial(idnotario,
                                                                               fechaPresentacionInicial,
                                                                               fechaPresentacionFinal,
                                                                               (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                               out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaraciones de tipo jornada notarial por cuenta catastral
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="cuentaCatastral">Cuenta catastral. Array con los códigos que forma la cuenta</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastralTipoJornadaNotarial(int idnotario, string[] cuentaCatastral, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByCuentaCatastralJornadaNotarial(idnotario,
                                                                             cuentaCatastral[0],
                                                                             cuentaCatastral[1],
                                                                             cuentaCatastral[2],
                                                                             cuentaCatastral[3],
                                                                               (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                             out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene las declaraciones jornada notarial por nombre y apellido paterno
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="nombre">Nombre para filtrar </param>
        /// <param name="apellidoPaterno">Apellido paterno para filtrar</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNombApellPaternoTipoJornadaNotarial(int idnotario, string nombre, string apellidoPaterno, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByNombApellPaternoJornadaNotarial(idnotario,
                                                                              nombre,
                                                                              apellidoPaterno,
                                                                               (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                              out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaracioens por fecha de presentacion
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="fechaPresentacionInicial">Fecha inicial para filtrar</param>
        /// <param name="fechaPresentacionFinal">Fecha final para filtrar</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorFechaPresentacion(int idnotario, DateTime fechaPresentacionInicial, DateTime fechaPresentacionFinal)
        {
            try
            {
                object C_DECLARACION = null;



                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByFechaPresentacion(idnotario,
                                                                               fechaPresentacionInicial,
                                                                               fechaPresentacionFinal,
                                                                               out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}

                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaraciones por cuenta catastral desde la secretaría de finanzas
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="txtregion">Región de la cuenta catastral</param>
        /// <param name="txtmanzana">Manzana de la cuenta catastral</param>
        /// <param name="txtlote">Lote de la cuenta catastral</param>
        /// <param name="txtunidadprivativa">Unidad privativa de la cuenta catastral</param>
        /// <param name="codEstadoDeclaracion">Código del estado de la declaración</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastralCodEstadoDeclaracionSF(int idnotario, string txtregion, string txtmanzana, string txtlote, string txtunidadprivativa, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = new DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable();
                declaracionDT = DeclaracionTA.GetDataByCuentaCatastralEstado(idnotario,
                                                                   txtregion,
                                                                   txtmanzana,
                                                                   txtlote,
                                                                   txtunidadprivativa,
                                                                   codEstadoDeclaracion,
                                                                   (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                   out C_DECLARACION);

                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}

                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);

                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaraciones por la cuenta catastral
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="cuentaCatastral">Cuenta catastral. Array con los códigos de la cuenta</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastral(int idnotario, string[] cuentaCatastral)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByCuentaCatastral(idnotario,
                                                                             cuentaCatastral[0],
                                                                             cuentaCatastral[1],
                                                                             cuentaCatastral[2],
                                                                             cuentaCatastral[3],
                                                                             out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}

                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaraciones por nombre y apellido paterno
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="nombre">Nombre para filtrar</param>
        /// <param name="apellidoPaterno">Apellido paterno para filtrar</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNombApellPaterno(int idnotario, string nombre, string apellidoPaterno)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByNombApellPaterno(idnotario,
                      nombre,
                      apellidoPaterno,
                      out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }

        }


        /// <summary>
        /// Obtiene las declaraciones del notario por fecha y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="fechaPresentacionInicial">Fecha Inicial</param>
        /// <param name="fechaPresentacionFinal">Fecha final para filtrar</param>
        /// <param name="codEstadoDeclaracion">Código del estado para filtrar</param>
        ///  <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorFechaPresentacionCodEstadoDeclaracion(DateTime fechaPresentacionInicial, DateTime fechaPresentacionFinal, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = new DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable();
                declaracionDT = DeclaracionTA.GetDataByFechaPresentacionEstado(
                                                                     fechaPresentacionInicial,
                                                                     fechaPresentacionFinal,
                                                                     codEstadoDeclaracion,
                                                                     (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                     out C_DECLARACION);



                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}


                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaraciones del notario por cuenta catastral y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="cuentaCatastral">Cuenta para filtrar. Array con  los códigos de la cuenta</param>
        /// <param name="codEstadoDeclaracion">Código de estado de la declaración</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        /// <returns>DataTable con las declaraciones</returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorCuentaCatastralCodEstadoDeclaracion(int idnotario, string[] cuentaCatastral, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;


                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByCuentaCatastralEstado(idnotario,
                                                                   cuentaCatastral[0],
                                                                   cuentaCatastral[1],
                                                                   cuentaCatastral[2],
                                                                   cuentaCatastral[3],
                                                                   codEstadoDeclaracion,
                                                                   (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                   out C_DECLARACION);

                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene las declaraciones del notario por nombre, apellido paternal y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="nombre">Nombre para filtar</param>
        /// <param name="apellidoPaterno">Apellido para filtrar</param>
        /// <param name="codEstadoDeclaracion">Código del estado de la declaración</param>
        /// <returns></returns>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNombApellPaternoCodEstadoDeclaracion(int idnotario, int idPerNot, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByNombApellPaternoEstado(idnotario,
                                                                      idPerNot,
                                                                      codEstadoDeclaracion,
                                                                       (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                      out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }

        }


        /// <summary>
        /// Obtiene las declaraciones del notario por numero de presentación y estado de la declaración
        /// </summary>
        /// <param name="idnotario">Identificador del notario al que estan asociadas las declaraciones</param>
        /// <param name="numPresentacion">Numero para filtar</param>
        /// <param name="codEstadoDeclaracion">Código del estado de la declaración</param>
        /// <param name="pageSize">Tamaño de registros para una página del grid</param>
        /// <param name="indice">Número de página del grid a mostrar</param>
        /// <param name="rowsTotal">Número total de registros obtenidos</param>
        /// <param name="SortExpression">Expresión de orden para el grid</param>
        public DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable ObtenerDeclaracionesPorNumPresentacionCodEstadoDeclaracion(int idnotario, string numPresentacion, decimal codEstadoDeclaracion, int pageSize, int indice, ref int rowsTotal, string SortExpression)
        {
            try
            {
                object C_DECLARACION = null;

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByNumPresentacionEstado(idnotario,
                                                                      numPresentacion,
                                                                      codEstadoDeclaracion,
                                                                       (decimal?)pageSize, (decimal?)indice, SortExpression,
                                                                      out C_DECLARACION);
                //foreach (DseDeclaracionIsai.FEXNOT_DECLARACIONRow rowDeclaracion in declaracionDT.Rows)
                //{
                //    if (!rowDeclaracion.IsSUJETONull())
                //    {
                //        if (rowDeclaracion.SUJETO.Trim().Length > 20)
                //        {
                //            rowDeclaracion.SUJETO = string.Concat(rowDeclaracion.SUJETO.Trim().Substring(0, 20), " ...");
                //        }
                //    }
                //    if (!rowDeclaracion.IsPARTICIPACIONNull()) rowDeclaracion.PARTICIPACION = rowDeclaracion.PARTICIPACION * 100;
                //}
                if (declaracionDT.Count > 0)
                    rowsTotal = Convert.ToInt32(declaracionDT[0].ROWS_TOTAL);
                return declaracionDT;


            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }

        }


        /// <summary>
        /// Obtiene las condonaciones del año en función del valor catastral para las jornadas notariales
        /// </summary>
        /// <param name="valorCatastral">Valor catastral</param>
        /// <returns>DataTable con las jornadas notariales</returns>
        public DseCatalogo.FEXNOT_JORNADANOTARIALDataTable ObtenerJornadaNotarialByAnio(decimal valorCatastral)
        {
            try
            {
                object C_JORNADANOTARIAL = null;
                return JornadaNotarialTA.GetDataByJornadaNotarialByAnio(DateTime.Now.Year, valorCatastral, out C_JORNADANOTARIAL);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Consulta el valor catastral en función de la cuenta catastral contra FISCAL
        /// </summary>
        /// <param name="cuentaCatastral">Cuenta Catastral</param>
        /// <returns>Valor Catastral</returns>
        public decimal ObtenerValorCatastral(string[] cuentaCatastral)
        {
            try
            {
                double resultado = 0;
                string[] codes = cuentaCatastral;
                int[] values = new int[4] { 1, 1, 1, 1 };
                foreach (char c in codes[0])
                    values[0] *= Convert.ToInt32(c);
                foreach (char c in codes[1])
                    values[1] *= Convert.ToInt32(c);
                foreach (char c in codes[2])
                    values[2] *= Convert.ToInt32(c);
                foreach (char c in codes[3])
                    values[3] *= Convert.ToInt32(c);

                values[3] = values[3] % 9;
                if (values[3] < 5) values[3] = 5;

                values[2] = values[2] % 1000;
                values[1] = values[1] % 1000;
                values[0] = values[0] % 100;

                resultado = Convert.ToDouble(Math.Pow(10, Convert.ToDouble(values[3])));
                resultado = Convert.ToDouble(values[0]) * resultado;
                resultado = resultado + values[2];
                resultado = resultado + (values[3] * 1000);
                return Convert.ToDecimal(resultado);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }




        /// <summary>
        /// Devolvemos el dataTable del catálogo de bancos cargado
        /// </summary>
        /// <returns>DataTable del catálogo de bancos</returns>
        public DseCatalogo.FEXNOT_CATBANCODataTable ObtenerCatBanco()
        {
            try
            {
                object C_CATBANCO = null;
                return CatBancoTA.GetData(out C_CATBANCO);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene el DataTable con el catálogo de procedencias
        /// </summary>
        /// <returns>DataTable con el catálogo de procendecias</returns>
        public DseCatalogo.FEXNOT_CATPROCEDENCIADataTable ObtenerCatProcedencia()
        {
            try
            {
                object C_CATPROCEDENCIA;
                return CatProcedenciaTA.GetData(out C_CATPROCEDENCIA);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene el catálogo de exenciones
        /// </summary>
        /// <returns>DataTable con todas las exenciones</returns>
        public DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerExenciones()
        {
            try
            {
                object C_EXENCIONES = null;
                return ExencionTA.GetData(out C_EXENCIONES);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene el catálogo de exenciones por ejercicio anual
        /// </summary>
        /// <param name="anio">Año del ejercicio a consultar</param>
        /// <returns>DataTable con las exenciones del ejercicio especificado</returns>
        public DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerExencionesPorEjercicioAnual(decimal anio)
        {
            try
            {
                object C_EXENCIONES = null;
                return ExencionTA.GetDataByEjercicioAnual((decimal?)anio, out C_EXENCIONES);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene el catálogo de exenciones por el identificador de la exención
        /// </summary>
        /// <param name="idExencion">Identificador de la exención</param>
        /// <returns>DataTable con la exención especificada</returns>
        public DseCatalogo.FEXNOT_EXENCIONESDataTable ObtenerExencionesPorId(decimal idExencion)
        {
            try
            {
                object c_exencion = null;
                return ExencionTA.GetDataById((decimal?)idExencion, out c_exencion);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene el catálogo de reducciones
        /// </summary>
        /// <returns>DataTable con todas las reducciones</returns>
        public DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerReducciones()
        {
            try
            {
                object C_REDUCCIONES = null;
                return ReduccionTA.GetData(out C_REDUCCIONES);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene el catálogo de reducciones por ejercicio anual
        /// </summary>
        /// <param name="anio">Año del ejercicio a consultar</param>
        /// <returns>DataTable con las reducciones del ejercicio especificado</returns>
        public DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerReduccionesPorEjercicioAnual(decimal anio)
        {
            try
            {
                object C_REDUCCIONES = null;
                return ReduccionTA.GetDataByEjercicioAnual((decimal?)anio, out C_REDUCCIONES);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene el catálogo de exenciones por el identificador de la reducción
        /// </summary>
        /// <param name="idReduccion">Identificador de la reducción</param>
        /// <returns>DataTable con la reducción especificada</returns>
        public DseCatalogo.FEXNOT_REDUCCIONESDataTable ObtenerReduccionesPorId(decimal idReduccion)
        {
            try
            {
                object C_REDUCCIONES = null;
                return ReduccionTA.GetDataById(idReduccion, out C_REDUCCIONES);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene los participantes de una declaración especificada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los participantes de la declaración solicitada</returns>
        public DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable ObtenerParticipantesPorIdDeclaracion(int idDeclaracion)
        {
            try
            {
                object C_Participantes = null;
                return ParticipanteTA.GetDataByParticipantesByIdDeclaracion(idDeclaracion, out C_Participantes);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }

        }

        /// <summary>
        /// Obtiene un participante de una declaración
        /// </summary>
        /// <param name="idPersona">Identificador de la persona</param>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con el participante</returns>
        public DseDeclaracionIsai.FEXNOT_PARTICIPANTESDataTable ObtenerParticipantesPorIdPersona_IdDeclaracion(int idPersona, int idDeclaracion)
        {
            try
            {
                object C_Participantes = null;
                return ParticipanteTA.GetDataByParticipantesByIdDeclaracionIdPersona(idPersona, idDeclaracion, out C_Participantes);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Busca personas sobre FEXNOT y REGISTRO DE CONTRIBUYENTES
        /// </summary>
        /// <param name="nombre">Nombre de las personas a buscar</param>
        /// <param name="apellidoPaterno">Apellido Paterno de las personas a buscar</param>
        /// <param name="apellidoMaterno">Apellido Materno de las personas a buscar</param>
        /// <param name="rfc">rfc de las personas a buscar</param>
        /// <param name="curp">curpo de las personas a buscar</param>
        /// <param name="ife">ife de las personas a buscar</param>
        /// <returns>DataTable con las personas</returns>
        public DsePersonas.FEXNOT_PERSONASDataTable ObtenerPersonas(string nombre, string apellidoPaterno, string apellidoMaterno, string rfc, string curp, string ife)
        {
            try
            {
                object C_PERSONAS = null;

                return PersonaTA.GetDataByPersonasBuscar(nombre, apellidoPaterno, apellidoMaterno, rfc, curp, ife, out C_PERSONAS);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Busca personas sobre FEXNOT y REGISTRO DE CONTRIBUYENTES
        /// </summary>
        /// <param name="idPersona">Identificador de las persona a buscar</param>
        /// <returns>DataTable con las persona</returns>
        public DsePersonas.FEXNOT_PERSONASDataTable ObtenerPersonasPorId(decimal idPersona)
        {
            try
            {
                object C_PERSONAS = null;

                return PersonaTA.GetDataByPersonasByIdPersona(idPersona, out C_PERSONAS);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene línea de captura para una declaración
        /// </summary>
        /// <param name="declaracionIsai">DataSet con la declaración</param>
        /// <param name="esRevisor">Indica si el presentador de la declaración es notario o revisor</param>
        /// <param name="nombreRevisor">Nombre del revisor</param>
        /// <returns>DataSet con la declaración y la línea de captura</returns>
        public DseDeclaracionIsai ObtenerLineaCaptura(DseDeclaracionIsai declaracionIsai, bool esRevisor, string nombreRevisor, string fechaCausacion)
        {
            SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_respuesta response = new SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_respuesta();
            SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.ServicesPortTypeClient wsProxy = null;

            try
            {
                //reducciones
                DseCatalogo.FEXNOT_REDUCCIONESDataTable reduccionTable = null;
                DseCatalogo.FEXNOT_CATACTOSJURIDICOSDataTable actosTable = ObtenerCatalogoActosJuridicos();
                DseCatalogo.FEXNOT_CATROLESDataTable rolesTable = ObtenerCatalogoRoles();

                SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_pregunta request = new SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_pregunta();

                decimal? idRedux = null;
                if (!declaracionIsai.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
                    idRedux = declaracionIsai.FEXNOT_DECLARACION[0].IDREDUCCION;


                if (!declaracionIsai.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull())
                {
                    reduccionTable = ObtenerReduccionesPorId(declaracionIsai.FEXNOT_DECLARACION[0].IDREDUCCION);
                    if (!reduccionTable[0].IsARTICULONull())
                        request.articulo = reduccionTable[0].ARTICULO.ToString();
                    if (!reduccionTable[0].IsAPARTADONull())
                        request.fraccion = reduccionTable[0].APARTADO;
                    if (!reduccionTable[0].IsDESCUENTONull())
                        request.reduccion = Convert.ToInt32(reduccionTable[0].DESCUENTO * 100);
                }
                else
                {
                    request.articulo = "";
                    request.fraccion = "";
                    request.reduccion = 0;
                }
                //cambios comentados GULE
                //request.ActoJur declaracionIsai.FEXNOT_DECLARACION[0].CODACTOJUR;
                //request.vigencia =  getFechaVencimiento(Convert.ToDateTime(fechaCausacion));
                //request.Propietario = getPropietario(declaracionIsai.FEXNOT_DECLARACION[0].IDDECLARACION);
                request.actualizacion = Convert.ToSingle(declaracionIsai.FEXNOT_DECLARACION[0].ACTUALIZACIONIMPORTE.ToRoundTwoDecimals().ToString("##0.00"));

                if (!declaracionIsai.FEXNOT_DECLARACION[0].IsCODACTOJURNull())
                {
                    request.operacion = actosTable[0].TIPOOPERACION;
                }

                request.clave = Convert.ToInt32(ConfigurationManager.AppSettings["ServicioFUTclave"]);
                string CuentaCatastral = declaracionIsai.FEXNOT_DECLARACION[0].REGION
                                        + declaracionIsai.FEXNOT_DECLARACION[0].MANZANA
                                            + declaracionIsai.FEXNOT_DECLARACION[0].LOTE
                                            + declaracionIsai.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA;

                string digitoVerificador = DigitoVerificadorUtils.ObtenerDigitoVerificador(declaracionIsai.FEXNOT_DECLARACION[0].REGION
                                       , declaracionIsai.FEXNOT_DECLARACION[0].MANZANA
                                          , declaracionIsai.FEXNOT_DECLARACION[0].LOTE
                                              , declaracionIsai.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA);

                request.cuenta = CuentaCatastral + digitoVerificador;

                request.derechos = 0;
                //request.fecha = DateTime.Now.ToString("yyyy-MM-dd");
                request.fecha = fechaCausacion;
                //Importa base gravable con dos decimales
                request.importe = declaracionIsai.FEXNOT_DECLARACION[0].IMPUESTO.ToRoundTwo().ToString();
                //Importe total a pagar
                request.total = declaracionIsai.FEXNOT_DECLARACION[0].IMPUESTOPAGADO.ToRoundTwo().ToString();

                //request.importe = request.importe.Contains(".") ? request.importe.Replace('.', ',') : request.importe;
                //request.total = request.total.Contains(".") ? request.total.Replace('.', ',') : request.total;

                if (esRevisor)
                {
                    request.nombre = nombreRevisor.ToString();
                }
                else
                {

                    //Datos del notario
                    RegistroContribuyentes.Services.AccesoDatos.DseInfoContribuyente dseInfo;
                    using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
                    {
                        dseInfo = clienteRCON.GetInfoContribuyente(declaracionIsai.FEXNOT_DECLARACION.First().IDPERSONA);
                    }
                    if (dseInfo.Contribuyente.Any())
                    {
                        request.nombre = dseInfo.Contribuyente.First().NOMBRE + " " + dseInfo.Contribuyente.First().APELLIDOPATERNO + " " + dseInfo.Contribuyente.First().APELLIDOMATERNO;
                    }
                    //domicilio
                    if (dseInfo.Direcciones.Any())
                    {
                        DseDeclaracionIsai.FEXNOT_DOMICILIODataTable domicilio = ObtenerDomicilioParticipante(dseInfo.Direcciones.First().IDDIRECCION);
                        if (domicilio.Any())
                        {
                            request.calleynum = domicilio[0].VIA + " " + domicilio.First().NUMEROEXTERIOR
                                + (domicilio.First().IsNUMEROINTERIORNull() ? "" : domicilio.First().NUMEROINTERIOR);
                            request.colonia = domicilio.First().COLONIA;
                            request.cp = domicilio.First().IsCODIGOPOSTALNull() ? "" : domicilio.First().CODIGOPOSTAL;
                        }
                    }
                }


                //Importe del beneficio fiscal que aplique. 
                //Solamente puede existir un único beneficio fiscal, pudiendo ser reducción, condonación, disminución o subsidio
                if (!declaracionIsai.FEXNOT_DECLARACION[0].IsIMPORTECONDONACIONNull())
                    request.otros = Convert.ToSingle(declaracionIsai.FEXNOT_DECLARACION[0].IMPORTECONDONACION.ToString("##0.00"));

                request.password = ConfigurationManager.AppSettings["ServicioFUTpwd"].ToString();
                //Importe de los recargos
                request.recargos = Convert.ToSingle(declaracionIsai.FEXNOT_DECLARACION[0].IMPORTERECARGOPAGOEXTEMP.ToRoundTwoDecimals().ToString("##0.00"));
                request.usuario = ConfigurationManager.AppSettings["ServicioFUTuser"].ToString();
                #region TEMP

                request.calleynum = RemoveAcentos(request.calleynum);
                request.colonia = RemoveAcentos(request.colonia);
                request.nombre = RemoveAcentos(request.nombre);

                #endregion

                request.passwordisai = ConfigurationManager.AppSettings["ServicioFUTpwd"].ToString();
                request.usuarioisai = ConfigurationManager.AppSettings["ServicioFUTuser"].ToString();
                wsProxy = new SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.ServicesPortTypeClient();

                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (object sender2,
                                                                                                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                                                                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                                                                    System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };





                response = wsProxy.solicitar_LC(request);
                if (response != null)
                {
                    if (String.IsNullOrEmpty(response.error) && String.IsNullOrEmpty(response.error_descripcion))
                    {
                        declaracionIsai.FEXNOT_DECLARACION[0].LINEACAPTURA = response.linea;
                        declaracionIsai.FEXNOT_DECLARACION[0].FECHALINEACAPTURA = DateTime.Now;
                        //declaracionIsai.FEXNOT_DECLARACION[0].FUT = response.linkfut;


                        #region TEMP redux
                        if (idRedux.HasValue)
                            declaracionIsai.FEXNOT_DECLARACION[0].IDREDUCCION = idRedux.Value;
                        else
                            declaracionIsai.FEXNOT_DECLARACION[0].SetIDREDUCCIONNull();
                        #endregion

                        DateTime fechaVigencia = getFechaVencimiento(Convert.ToDateTime(fechaCausacion));
                        //if (DateTime.TryParseExact(response.vigencia, "yyyy-MM-dd", new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentCulture.Name), System.Globalization.DateTimeStyles.None, out fechaVigencia))
                        declaracionIsai.FEXNOT_DECLARACION[0].FECHAVIGENCIALINEACAPTURA = fechaVigencia;

                        //cambio 1925
                        StringBuilder campoFut = new StringBuilder();
                        campoFut.AppendFormat("cuenta={0}|clave={1}|linea_captura={2}|importe={3}|total={4}|anio_vig={5}|mes_vig={6}|dia_vig={7}|nombre={8}|calleynum={9}|derechos={10}|actual={11}|recargos={12}{13}|total={14}",
                            request.cuenta,
                            request.clave,
                            response.linea,
                            request.importe,
                            request.total,
                            fechaVigencia.Year.ToString(),
                            fechaVigencia.Month.ToString(),
                            fechaVigencia.Day.ToString(),
                            request.nombre,
                            request.calleynum,
                            request.derechos,
                            request.actualizacion,
                            request.recargos,
                            (
                                !declaracionIsai.FEXNOT_DECLARACION[0].IsIDREDUCCIONNull() ?
                                string.Format(
                                    "|reduccion={0}", request.reduccion
                                )
                                :
                                string.Empty
                            ),
                            request.otros);

                        declaracionIsai.FEXNOT_DECLARACION[0].FUT = campoFut.ToString();
                        RegistrarDeclaracion(ref declaracionIsai, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, -1, null);
                        return declaracionIsai;
                    }
                    else
                        throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(response.error_descripcion));
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException("La respuesta obtenida del servicio no es valida."));
            }
            catch (System.ServiceModel.CommunicationException cex)
            {

                ExceptionPolicyWrapper.HandleException(cex);

                string mensaje = null;
                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = cex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (cex.InnerException != null)
                        mensaje = cex.InnerException.Message;
                    else
                        mensaje = cex.Message;
                }

                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));
            }
            catch (Exception ex)
            {

                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (ex.InnerException != null)
                        mensaje = ex.InnerException.Message;
                    else
                        mensaje = ex.Message;
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));

            }
            finally
            {
                if (wsProxy != null)
                    ((IDisposable)wsProxy).Dispose();
            }
        }

        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfNotarios.FEXNOT_INFONOTARIOSDataTable ObtenerInfNotarios(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoNotarioTA.GetData(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas de detalle del numero total de declaraciones.
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfDetalleNotarios.FEXNOT_INFODETALLENOTARIOSDataTable ObtenerInfNotariosTotales(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoDetalleNotarioTA.GetDataByTotales(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas de detalle del número total de declaraciones.
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfDetalleNotarios.FEXNOT_INFODETALLENOTARIOSDataTable ObtenerInfNotariosMayor15(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoDetalleNotarioTA.GetDataByMayorDe15(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información asociada a uno o varios Notarios en un rango de fechas de detalle del número total de declaraciones.
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfDetalleNotarios.FEXNOT_INFODETALLENOTARIOSDataTable ObtenerInfNotariosMayor180(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoDetalleNotarioTA.GetDataByMayorDe180(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información de los avaluos y declaraciones totales para un año
        /// </summary>
        /// <param name="anio">Año</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfEnviosTotales ObtenerInfEnviosTotales(decimal anio)
        {
            try
            {
                object C_ENVIOSTOTALES = null;
                DseInfEnviosTotales dseInfoEnviosTotales = new DseInfEnviosTotales();

                InfoAvaluosTotalesTA.Fill(dseInfoEnviosTotales.FEXNOT_INFOAVALUOSTOTALES, anio, out C_ENVIOSTOTALES);
                InfoDeclaracionesTotalesTA.Fill(dseInfoEnviosTotales.FEXNOT_INFODECLARACTOTALES, anio, out C_ENVIOSTOTALES);

                return dseInfoEnviosTotales;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfoLineasCaptura.FEXNOT_INFOLINEASCAPTURADataTable ObtenerInfLineasCaptura(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoLineasCapturaTA.GetData(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información de detalle basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas mayor 6. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfoDetalleLineasCaptura.FEXNOT_INFODETALLELINEASCAPTURADataTable ObtenerInfDetalleLineasCapturaMayor(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoDetalleLineasCapturaTA.GetDataByMayor(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información de detalle basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas menor 6. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfoDetalleLineasCaptura.FEXNOT_INFODETALLELINEASCAPTURADataTable ObtenerInfDetalleLineasCapturaMenor(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoDetalleLineasCapturaTA.GetDataByMenor(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información de detalle basada en las líneas de captura de declaraciones
        /// realizadas por un notario en un rango de fechas totales. 
        /// </summary>
        /// <param name="idNotario">Identificador de Notario</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfoDetalleLineasCaptura.FEXNOT_INFODETALLELINEASCAPTURADataTable ObtenerInfDetalleLineasCapturaTotales(decimal? idNotario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                object C_INFONOTARIO = null;

                return InfoDetalleLineasCapturaTA.GetDataByTotales(idNotario,
                                             fechaIni,
                                             fechaFin,
                                             out C_INFONOTARIO);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene información de los avalúos que cumplan los requisitos indicados
        /// </summary>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <param name="pagadoEnviado">Pagado/Enviado</param>
        /// <param name="peritoSociedad">Perito/Sociedad</param>
        /// <param name="tipoFecha">Tipo de fecha</param>
        /// <param name="registro">Registro</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfAvaluos.FEXNOT_INF_SOCPERAVALUOS_PDataTable ObtenerInfSociedadPeritosAvaluos(DateTime fechaIni, DateTime fechaFin, decimal pagadoEnviado, decimal peritoSociedad, decimal tipoFecha, string registro)
        {
            try
            {
                object C_INFOSOCIEDADPERITOS = null;
                DseInfAvaluos.FEXNOT_INF_SOCPERAVALUOS_PDataTable resul = AvaluosTA.GetData(fechaIni,
                                                                         fechaFin,
                                                                         pagadoEnviado,
                                                                         peritoSociedad,
                                                                         registro,
                                                                         tipoFecha,
                                                                         out C_INFOSOCIEDADPERITOS);
                return resul;


            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene la información de Sociedades y Peritos (pagadas y eviadas) en un rango de fechas.
        /// </summary>
        /// <param name="fechaIni">Fecha Inicio</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="pagadoEnviado">Pagado/Enviado</param>
        /// <param name="peritoSociedad">Perito/Sociedad</param>
        /// <param name="tipoFecha">Tipo de fecha</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfSociedadPeritos.FEXNOT_INFOSOCIEDADPERITOSDataTable ObtenerInfSociedadPeritos(DateTime fechaIni, DateTime fechaFin, decimal pagadoEnviado, decimal peritoSociedad, decimal tipoFecha)
        {
            try
            {
                object C_INFOSOCIEDADPERITOS = null;

                return InfoSociedadPeritosTA.GetData(fechaIni,
                                                     fechaFin,
                                                     pagadoEnviado,
                                                     peritoSociedad,
                                                     tipoFecha,
                                                     out C_INFOSOCIEDADPERITOS);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obitene la información de los valores unitarios de mercado y los valores unitarios de renta.
        /// </summary>
        /// <param name="fechaIni">Fecha Inicio</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="tipoComparable">Tipo Comparable</param>
        /// <param name="registroSociedad">Registro Sociedad</param>
        /// <param name="registroPerito">Registro Perito</param>
        /// <param name="nombreColonia">Nombre Colonia</param>
        /// <param name="idColonia">IdColonia</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfValores.FEXNOT_INFOVALORESDataTable ObtenerInfValores(DateTime fechaIni, DateTime fechaFin, string tipoComparable, string registroSociedad, string registroPerito, string nombreColonia, decimal? idColonia)
        {
            try
            {
                object C_INFOVALORES = null;
                DseInfValores.FEXNOT_INFOVALORESDataTable resul = InfoValoresTA.GetData(fechaIni,
                                                     fechaFin,
                                                     tipoComparable,
                                                     registroSociedad,
                                                     registroPerito,
                                                     nombreColonia,
                                                     idColonia,
                                                     out C_INFOVALORES);

                return resul;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }




        /// <summary>
        /// Obtiene la información de los pagos conformes.
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="manzana">Manzana</param>
        /// <param name="lote">Lote</param>
        /// <param name="uprivativa">Unidad Privativa</param>
        /// <param name="numdeclaracion">Número Declaración</param>
        /// <param name="lineacaptura">Línea de Captura</param>
        /// <param name="fechaIni">Fecha Inicio</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="tipo">Tipo de consulta</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfPagos.FEXNOT_INFPAGOSDataTable ObtenerInfConsultaPagos(string region, string manzana, string lote, string uprivativa, string numdeclaracion, string lineacaptura, DateTime fechaIni, DateTime fechaFin, string tipo)
        {
            try
            {
                object C_INFPAGOS = null;
                if (tipo == "C")
                    return InfPagosTA.GetDataByCon(region,
                                               manzana,
                                               lote,
                                               uprivativa,
                                               numdeclaracion,
                                               lineacaptura,
                                               fechaIni,
                                               fechaFin,
                                               out C_INFPAGOS);
                else if (tipo == "V")
                    return InfPagosTA.GetDataByVen(region,
                                               manzana,
                                               lote,
                                               uprivativa,
                                               numdeclaracion,
                                               lineacaptura,
                                               fechaIni,
                                               fechaFin,
                                               out C_INFPAGOS);
                else
                    return InfPagosTA.GetDataBySiscor(region,
                                               manzana,
                                               lote,
                                               uprivativa,
                                               numdeclaracion,
                                               lineacaptura,
                                               fechaIni,
                                               fechaFin,
                                               out C_INFPAGOS);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obitene la información de los avaluos filtrado por Clave Catastral
        /// </summary>
        /// <param name="region">Región</param>
        /// <param name="manzana">Manzana</param>
        /// <param name="lote">Lote</param>
        /// <param name="unidad">Unidad</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaCuentaCatastral(string region, string manzana, string lote, string unidad)
        {
            try
            {
                object C_INFOCONSULTAESPECIFICA_C = null;

                return InfoConsEspecificaTA.GetData(region,
                                                     manzana,
                                                     lote,
                                                     unidad,
                                                     out C_INFOCONSULTAESPECIFICA_C);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene la información de los avaluos según filtremos por Sujeto.
        /// </summary>
        /// <param name="sujeto">sujeto</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaSujeto(string sujeto)
        {
            try
            {
                object C_INFOCONSULTAESPECIFICA_S = null;

                return InfoConsEspecificaTA.GetDataBySujeto(sujeto,
                                                     out C_INFOCONSULTAESPECIFICA_S);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información de los avaluos filtrado por Ubicación.
        /// </summary>
        /// <param name="numexterior">Número exterior</param>
        /// <param name="numinterior">Número interior</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaUbicacion(string numexterior, string numinterior)
        {
            try
            {
                object C_INFOCONSULTAESPECIFICA_U = null;

                return InfoConsEspecificaTA.GetDataByUbicacion(numexterior,
                                                                numinterior,
                                                     out C_INFOCONSULTAESPECIFICA_U);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información de los avaluos filtrados por idDeclaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfConsultaEspecifica.FEXNOT_INFOCONSESPECIFICADataTable ObtenerInfConsultaEspecificaByIdDeclaracion(decimal? idDeclaracion)
        {
            try
            {
                object C_INFOCONSULTAESP = null;

                return InfoConsEspecificaTA.GetDataByIdDeclaracion(idDeclaracion,
                                                     out C_INFOCONSULTAESP);

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene la información para realizar el justificante de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfJustificante.FEXNOT_INFJUSTIFICANTEDataTable ObtenerInfJustificante(decimal idDeclaracion)
        {
            try
            {
                //object C_INFOJUSTIFICANTE = null;

                //DseInfJustificante.FEXNOT_INFJUSTIFICANTEDataTable justificanteDT = InfJustificanteTA.GetData(idDeclaracion, out C_INFOJUSTIFICANTE);

                //return justificanteDT;
                return null;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene la información para realizar el justificante de la declaracion
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        public DseInfJustificante.FEXNOT_INFJUSTIFICANTEDataTable ObtenerInfJustificanteGen(decimal idDeclaracion)
        {
            try
            {
                object C_INFOJUSTIFICANTE = null;

                DseInfJustificante.FEXNOT_INFJUSTIFICANTEDataTable justificanteDT = InfJustificanteTA.GetDataJustiGen(idDeclaracion, out C_INFOJUSTIFICANTE);

                return justificanteDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Obtiene la información para realizar el acuse de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <returns>DataTable con los datos para el informe</returns>
        public DseInfJustificante.FEXNOT_INFACUSEDataTable ObtenerInfAcuse(decimal idDeclaracion)
        {
            try
            {
                object C_INFOJUSTIFICANTE = null;
                DseInfJustificante.FEXNOT_INFACUSEDataTable justificanteDT = InfAcuseTA.GetData(idDeclaracion, out C_INFOJUSTIFICANTE);
                return justificanteDT;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información para realizar el acuse de la declaracion, parte de datos generales
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        public DseInfJustificante.FEXNOT_INF_ACUSEGEN_PDataTable ObtenerInfAcuseGen(decimal idDeclaracion)
        {
            try
            {
                object C_INFOJUSTIFICANTE = null;

                DseInfJustificante.FEXNOT_INF_ACUSEGEN_PDataTable justificanteDT = InfAcuseGenTA.GetData(idDeclaracion, out C_INFOJUSTIFICANTE);


                return justificanteDT;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Obtiene la información para realizar el acuse de la declaracion, parte de datos de losp articipantes
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        public DseInfJustificante.FEXNOT_INF_ACUSEPAR_PDataTable ObtenerInfAcusePar(decimal idDeclaracion)
        {
            try
            {
                object C_INFOJUSTIFICANTE = null;

                DseInfJustificante.FEXNOT_INF_ACUSEPAR_PDataTable justificanteDT = InfAcuseParTA.GetData(idDeclaracion, out C_INFOJUSTIFICANTE);


                return justificanteDT;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }



        /// <summary>
        /// Obtiene el catálogo de los salarios mínimos
        /// </summary>
        /// <returns>DataTable con el catálogo de salarios mínimos</returns>
        public DseCatalogo.FEXNOT_CATSALARIOMINIMODataTable ObtenerCatalogoSalarioMinimos()
        {
            try
            {
                object C_cursor = null;
                DseCatalogo.FEXNOT_CATSALARIOMINIMODataTable response = SalariosMinimosTA.GetData(out C_cursor);
                return response;
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }
        #endregion

        #region Otros métodos

        /// <summary>
        /// Llama al servicio de documental para insertar el documento de forma transaccional
        /// </summary>
        /// <param name="xmlDoc">String del xml del documento</param>
        /// <param name="tipoDocumentoDigital">String con el tipo de documento</param>
        /// <param name="listaFicheros">String con la lista de ficheros</param>
        /// <param name="transactionHelper">Transacción</param>
        /// <returns>Identificador que se le asigna en documental al documento insertado</returns>
        private decimal EspecificarDocumentoTransaccional(string xmlDoc, string tipoDocumentoDigital, string listaFicheros, TransactionHelper transactionHelper)
        {
            try
            {
                //Se realizan las conversiones necesarias de los parmetros recibidos a sus tipos correctos
                TipoDocumento tipoDocumento = (TipoDocumento)System.Enum.Parse(typeof(TipoDocumento), tipoDocumentoDigital);
                string[] listaString = listaFicheros.Split(',');
                List<decimal> lista = new List<decimal>();
                foreach (string s in listaString)
                {
                    //Se guarda el valor siempre que no sea una cadena vacia ni nula
                    if (!string.IsNullOrEmpty(s))
                    {
                        lista.Add(Convert.ToDecimal(s));
                    }
                }
                AltasDocumentosTran cliente = new AltasDocumentosTran();
                //Se inserta el documento digital
                decimal idDocumentoDigital = (decimal)cliente.Tran_InsertDocumento(xmlDoc, tipoDocumento, transactionHelper);
                //Se actualiza la tabla doc_ficherosdocumentos
                cliente.Tran_UpdateDocumentoDigitalFicheros(lista, idDocumentoDigital, transactionHelper);
                return idDocumentoDigital;
            }
            catch (Exception e)
            {
                transactionHelper.RollBack();
                throw;
            }
        }



        /// <summary>
        /// Llama al servicio de documental para insertar un fichero de forma transaccional
        /// </summary>
        /// <param name="idDocumentoDigital">String del xml del documento</param>
        /// <param name="listaFicheros">String con la lista de ficheros</param>
        public void EspecificarFicheroTransaccional(decimal idDocumentoDigital, string listaFicheros)
        {
            TransactionHelper transactionHelper = null;
            try
            {
                string[] listaString = listaFicheros.Split(',');
                List<decimal> lista = new List<decimal>();
                foreach (string s in listaString)
                {
                    //Se guarda el valor siempre que no sea una cadena vacia ni nula
                    if (!string.IsNullOrEmpty(s))
                    {
                        lista.Add(Convert.ToDecimal(s));
                    }
                }

                using (transactionHelper = new TransactionHelper(DeclaracionTA.Connection))
                {
                    transactionHelper.BeginTransaction();

                    AltasDocumentosTran cliente = new AltasDocumentosTran();
                    //Se actualiza la tabla doc_ficherosdocumentos
                    cliente.Tran_UpdateDocumentoDigitalFicheros(lista, idDocumentoDigital, transactionHelper);
                    transactionHelper.Commit();
                }
            }
            catch (Exception)
            {
                transactionHelper.RollBack();
                throw;
            }
        }



        /// <summary>
        ///  Valida los datos obtenidos al realizar el pago desde la página del banco
        /// </summary>
        /// <param name="cadValidacion"></param>
        /// <param name="fechaPago"></param>
        /// <param name="importeTotal"></param>
        /// <param name="numOperBancaria"></param>
        /// <param name="referencia"></param>
        /// <param name="secuenciaTransmision"></param>
        /// <returns></returns>
        public bool ValidarRespuestaOnline(string cadValidacion, string fechaPago, Decimal importeTotal, Int32 numOperBancaria, string referencia, Int32 secuenciaTransmision)
        {
            bool resultado = false;
            string claveBanco = "988";
            string[] fecha = fechaPago.Split('-');
            string fechaPagoBanco = fecha[2][0] + fecha[2][1] + fecha[1] + fecha[0];
            string cadenaValidacion = claveBanco +
                                      numOperBancaria.ToString().PadLeft(12, '0') +
                                      fechaPagoBanco +
                                      referencia.PadLeft(20, '0') +
                                      importeTotal.ToString().PadLeft(15, '0');
            if (cadenaValidacion == cadValidacion)
            {
                resultado = true;
            }

            return resultado;
        }


        /// <summary>
        /// Realizar el pago o marcar como pagada la declaración
        /// </summary>
        /// <param name="declaracionDT">DataTable con una única fila con la declaración que se va a pagar</param>
        /// <param name="fechaPago">Fecha del pago</param>
        /// <param name="banco">Banco en el que se ha pagado la declaración</param>
        /// <param name="sucursal">Sucursal en el que se ha pagado la declaración</param>
        /// <param name="formaPago">Forma de pago</param>
        public void PagarDeclaracion(DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT, DateTime fechaPago, string banco, string sucursal, string formaPago)
        {
            TransactionHelper transactionHelper = null;
            string numPresentacion = null;

            try
            {
                //VALIDACIONES
                EstadoPago estado = (EstadoPago)0;

                if (declaracionDT[0].CODESTADODECLARACION != Interfaces.EstadoDeclaracionEnum.Pendiente.ToDecimal())
                    throw new Exception("Estado no correcto");
                if (declaracionDT[0].CODESTADOPAGO != Interfaces.EstadoPago.PagoRecibidoSISCOR.ToDecimal())
                {
                    //throw new Exception("Declaración pagada");
                    if (declaracionDT[0].CODESTADOPAGO == EstadoPago.NoPagado.ToDecimal())
                    {
                        estado = EstadoPago.PendienteSISCOR;
                        fechaPago = DateTime.Now;
                    }
                    else if (declaracionDT[0].CODESTADOPAGO == EstadoPago.PendienteSISCOR.ToDecimal())
                    {
                        estado = EstadoPago.PendienteSISCOR;
                        fechaPago = DateTime.Now;
                    }

                    transactionHelper = new TransactionHelper(DeclaracionTA.Connection);
                    transactionHelper.BeginTransaction();
                    declaracionTA.SetTransaction(transactionHelper);
                    if (formaPago.CompareTo("Cero") != 0)
                    {
                        if (declaracionDT[0].IsFECHALINEACAPTURANull())
                            throw new Exception("Declaración sin linea de captura"); //TODO:
                        if (formaPago.CompareTo("Telematico") == 0)
                        {
                            //TODO: Conectar con pago telematico

                            banco = "Banco Telematico";
                            sucursal = "Sucursal Telematica";

                        }
                        else if (formaPago.CompareTo("Caja") == 0)
                        {
                            if (declaracionDT[0].IsIMPUESTOPAGADONull()) declaracionDT[0].IMPUESTOPAGADO = declaracionDT[0].IMPORTEIMPUESTO;
                        }

                        if (declaracionDT[0].ENPAPEL.CompareTo("S") == 0)
                        {
                            numPresentacion = declaracionDT[0].NUMPRESENTACION;
                        }
                    }

                    DeclaracionTA.UpdatePago(declaracionDT[0].IDDECLARACION, numPresentacion, null, declaracionDT[0].IMPUESTOPAGADO, fechaPago, estado.ToDecimal(), Interfaces.EstadoDeclaracionEnum.Pendiente.ToDecimal(), banco, sucursal);
                    transactionHelper.Commit();
                }
            }
            catch (System.ServiceModel.CommunicationException cex)
            {
                if (transactionHelper != null)
                    transactionHelper.RollBack();

                ExceptionPolicyWrapper.HandleException(cex);

                string mensaje = null;
                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = cex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (cex.InnerException != null)
                        mensaje = cex.InnerException.Message;
                    else
                        mensaje = cex.Message;
                }

                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje), new FaultReason(cex.Message));
            }
            catch (Exception ex)
            {
                if (transactionHelper != null)
                    transactionHelper.RollBack();

                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (ex.InnerException != null)
                        mensaje = ex.InnerException.Message;
                    else
                        mensaje = ex.Message;
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje), new FaultReason(ex.Message));

            }
            finally
            {
                if (transactionHelper != null)
                    transactionHelper.Dispose();
            }
        }

        /// <summary>
        /// Realizar el pago o marcar como pagada la declaración
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="fechaPago">Fecha del pago</param>
        /// <param name="banco">Banco en el que se realiza el pago</param>
        /// <param name="sucursal">Sucursal en el que se realiza el pago</param>
        /// <param name="formaPago">Forma de pago</param>
        public void PagarDeclaracion(int idDeclaracion, DateTime fechaPago, string banco, string sucursal, string formaPago)
        {
            object o = null;
            try
            {

                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = declaracionTA.GetDataByIdDeclaracion(idDeclaracion.ToDecimal(), out o);

                PagarDeclaracion(declaracionDT, fechaPago, banco, sucursal, formaPago);

            }
            catch (System.ServiceModel.CommunicationException cex)
            {

                ExceptionPolicyWrapper.HandleException(cex);

                string mensaje = null;
                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = cex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (cex.InnerException != null)
                        mensaje = cex.InnerException.Message;
                    else
                        mensaje = cex.Message;
                }

                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje), new FaultReason(cex.Message));
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (ex.InnerException != null)
                        mensaje = ex.InnerException.Message;
                    else
                        mensaje = ex.Message;
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje), new FaultReason(ex.Message));

            }
        }


        /// <summary>
        /// Calcula el impuesto de la declaración
        /// </summary>
        /// <param name="idDeclaracion">Idenficador de la declaración que se quiere calcular</param>
        /// <param name="fechaDoc">Fecha del documento</param>
        /// <param name="resultadoRecargo">Resultado Recargo</param>
        /// <param name="resultadoImporteRecargo">Resultado Importe Recargo</param>
        /// <param name="resultadoActualizacion">Resultado Actualización</param>
        /// <param name="resultadoImporteActualizacion">Resultado Importe Actualización</param>
        /// <param name="resultadoBaseGravable">Resultado Base Gravable</param>
        /// <param name="resultadoReduccion1995">Resultado Reduccion 1995</param>
        /// <param name="resultadoTasa1995">Resultado Tasa 1995</param>
        /// <param name="resultadoImpuesto">Resultado Impuesto</param>
        /// <param name="resultadoReduccionArt309">Resultado Reducción Artículo 309</param>
        /// <param name="resultadoExencionImporte">Resultado Exención Importe</param>
        /// <param name="resultadoImporteCondonacion">Resultado importe Condonación</param>
        /// <returns>Impuesto</returns>
        public decimal CalcularImpuestoDeclaracion(int idDeclaracion, DateTime fechaDoc, decimal? valorRef, DateTime? fechaValorRef, out Decimal? resultadoRecargo, out Decimal? resultadoImporteRecargo,
            out Decimal? resultadoActualizacion, out Decimal? resultadoImporteActualizacion, out Decimal? resultadoBaseGravable,
            out Decimal? resultadoReduccion1995, out Decimal? resultadoTasa1995, out Decimal? resultadoImpuesto, out Decimal? resultadoReduccionArt309,
            out Decimal? resultadoExencionImporte, out Decimal? resultadoImporteCondonacion, out Decimal? resultadoImporteCondonacionxFecha)
        {
            object o = null;
            try
            {
                DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT = DeclaracionTA.GetDataByIdDeclaracion(idDeclaracion.ToDecimal(), out o);
                return CalcularImpuestoDeclaracion(declaracionDT, fechaDoc, valorRef, fechaValorRef, out resultadoRecargo, out resultadoImporteRecargo,
                    out resultadoActualizacion, out resultadoImporteActualizacion, out resultadoBaseGravable,
                    out resultadoReduccion1995, out resultadoTasa1995, out resultadoImpuesto, out resultadoReduccionArt309,
                    out resultadoExencionImporte, out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }

        }


        /// <summary>
        ///  Calcula el impuesto de la declaración
        /// </summary>
        /// <param name="declaracionDT">DataTable con una única fila con la declaración que se quiere realizar el calculo</param>
        /// <param name="fecha_doc">Fecha del documento</param>
        /// <param name="valorRef">Valor referido</param>
        /// <param name="resultadoRecargo">Resultado Recargo</param>
        /// <param name="resultadoImporteRecargo">Resultado Importe Recargo</param>
        /// <param name="resultadoActualizacion">Resultado Actualización</param>
        /// <param name="resultadoImporteActualizacion">Resultado Importe Actualización</param>
        /// <param name="resultadoBaseGravable">Resultado Base Gravable</param>
        /// <param name="resultadoReduccion1995">Resultado Reduccion 1995</param>
        /// <param name="resultadoTasa1995">Resultado Tasa 1995</param>
        /// <param name="resultadoImpuesto">Resultado Impuesto</param>
        /// <param name="resultadoReduccionArt309">Resultado Reducción Artículo 309</param>
        /// <param name="resultadoExencionImporte">Resultado Exención Importe</param>
        /// <param name="resultadoImporteCondonacion">Resultado importe Condonación</param>
        /// <returns>Impuesto</returns>
        public decimal CalcularImpuestoDeclaracion(DseDeclaracionIsai.FEXNOT_DECLARACIONDataTable declaracionDT, DateTime fecha_doc, decimal? valorRef, DateTime? fechaValorReferido, out Decimal? resultadoRecargo,
            out Decimal? resultadoImporteRecargo,
            out Decimal? resultadoActualizacion, out Decimal? resultadoImporteActualizacion, out Decimal? resultadoBaseGravable,
            out Decimal? resultadoReduccion1995, out Decimal? resultadoTasa1995, out Decimal? resultadoImpuesto, out Decimal? resultadoReduccionArt309,
            out Decimal? resultadoExencionImporte, out Decimal? resultadoImporteCondonacion, out Decimal? resultadoImporteCondonacionxFecha)
        {
            try
            {
                decimal? par_codActoJur = declaracionDT[0].IsCODACTOJURNull() ? null : (decimal?)declaracionDT[0].CODACTOJUR;
                DateTime? par_fechaEscritura = declaracionDT[0].IsFECHAESCRITURANull() ? null : (DateTime?)declaracionDT[0].FECHAESCRITURA;
                decimal? par_codtipodeclaracion = declaracionDT[0].CODTIPODECLARACION;
                decimal? par_participacion = declaracionDT[0].IsPORCACTOTOTALNull() ? null : (decimal?)declaracionDT[0].PORCACTOTOTAL;
                decimal? par_valoradquisicion = declaracionDT[0].IsVALORADQUISICIONNull() ? 0 : (decimal?)declaracionDT[0].VALORADQUISICION;
                decimal? par_valorcatastral = declaracionDT[0].IsVALORCATASTRALNull() ? 0 : (decimal?)declaracionDT[0].VALORCATASTRAL;
                decimal? par_valoravaluo = declaracionDT[0].IsVALORAVALUONull() ? 0 : (decimal?)declaracionDT[0].VALORAVALUO;
                string par_eshabitacional = declaracionDT[0].ESHABITACIONAL.Equals("1") ? "S" : "N";
                string par_estasacero = declaracionDT[0].IsESTASACERONull() ? null : declaracionDT[0].ESTASACERO;
                decimal? par_regla = declaracionDT[0].IsREGLANull() ? null : (decimal?)declaracionDT[0].REGLA;
                decimal? par_idexencion = declaracionDT[0].IsIDEXENCIONNull() ? null : (decimal?)declaracionDT[0].IDEXENCION;
                decimal? par_idreduccion = declaracionDT[0].IsIDREDUCCIONNull() ? null : (decimal?)declaracionDT[0].IDREDUCCION;
                decimal? par_codjornotarial = declaracionDT[0].IsIDJORNADANOTARIALNull() ? null : (decimal?)declaracionDT[0].IDJORNADANOTARIAL;
                decimal? par_porcentajesubsio = declaracionDT[0].IsPORCENTAJESUBSIDIONull() ? null : (decimal?)declaracionDT[0].PORCENTAJESUBSIDIO;
                decimal? par_porcentajedisminucion = declaracionDT[0].IsPORCENTAJEDISMINUCIONNull() ? null : (decimal?)declaracionDT[0].PORCENTAJEDISMINUCION;
                decimal? par_porcentajecondonacion = declaracionDT[0].IsPORCENTAJECONDONACIONNull() ? null : (decimal?)declaracionDT[0].PORCENTAJECONDONACION;

                decimal res = CalcularImpuestoDeclaracion(par_codtipodeclaracion,
                        par_participacion,
                        par_valoradquisicion,
                        par_valorcatastral,
                        par_valoravaluo,
                        par_eshabitacional,
                        par_estasacero,
                        par_idexencion,
                        par_idreduccion,
                        par_codjornotarial,
                        par_porcentajesubsio,
                        par_porcentajedisminucion,
                        par_porcentajecondonacion,
                        fecha_doc,
                        par_regla,
                        valorRef,
                        fechaValorReferido,
                        par_codActoJur,
                        par_fechaEscritura,
                        out resultadoRecargo, out resultadoImporteRecargo,
                        out resultadoActualizacion,
                        out resultadoImporteActualizacion,
                        out resultadoBaseGravable,
                        out resultadoReduccion1995,
                        out resultadoTasa1995,
                        out resultadoImpuesto,
                        out resultadoReduccionArt309,
                        out resultadoExencionImporte,
                        out resultadoImporteCondonacion,
                        out resultadoImporteCondonacionxFecha);

                return res;

            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="par_codtipodeclaracion"></param>
        /// <param name="par_participacion"></param>
        /// <param name="par_valoradquisicion"></param>
        /// <param name="par_valorcatastral"></param>
        /// <param name="par_valoravaluo"></param>
        /// <param name="par_eshabitacional"></param>
        /// <param name="par_estasacero"></param>
        /// <param name="par_idexencion"></param>
        /// <param name="par_idreduccion"></param>
        /// <param name="par_codjornotarial"></param>
        /// <param name="par_porcentajesubsio"></param>
        /// <param name="par_porcentajedisminucion"></param>
        /// <param name="par_porcentajecondonacion"></param>
        /// <param name="par_fechadocumento"></param>
        /// <param name="par_regla"></param>
        /// <param name="resultadoRecargo"></param>
        /// <param name="resultadoImporteRecargo"></param>
        /// <param name="resultadoActualizacion"></param>
        /// <param name="resultadoImporteActualizacion"></param>
        /// <param name="resultadoBaseGravable"></param>
        /// <param name="resultadoReduccion1995"></param>
        /// <param name="resultadoTasa1995"></param>
        /// <param name="resultadoImpuesto"></param>
        /// <param name="resultadoReduccionArt309"></param>
        /// <param name="resultadoExencionImporte"></param>
        /// <param name="resultadoImporteCondonacion"></param>
        /// <returns></returns>
        public decimal CalcularImpuestoDeclaracion(decimal? par_codtipodeclaracion, decimal? par_participacion,
            decimal? par_valoradquisicion, decimal? par_valorcatastral, decimal? par_valoravaluo, string par_eshabitacional,
            string par_estasacero,
            decimal? par_idexencion, decimal? par_idreduccion, decimal? par_codjornotarial,
            decimal? par_porcentajesubsio, decimal? par_porcentajedisminucion, decimal? par_porcentajecondonacion,
            DateTime? par_fechadocumento, decimal? par_regla, decimal? par_valorReferido, DateTime? par_fechaValorReferido, decimal? par_codActosJuridico, DateTime? par_fechaEscritura,
            out Decimal? resultadoRecargo, out Decimal? resultadoImporteRecargo,
            out Decimal? resultadoActualizacion, out Decimal? resultadoImporteActualizacion, out Decimal? resultadoBaseGravable,
            out Decimal? resultadoReduccion1995, out Decimal? resultadoTasa1995, out Decimal? resultadoImpuesto, out Decimal? resultadoReduccionArt309,
            out Decimal? resultadoExencionImporte, out Decimal? resultadoImporteCondonacion, out Decimal? resultadoImporteCondonacionxFecha)
        {
            try
            {
                Decimal? resultadoImporteImpuesto;
                DeclaracionTA.CalcularDeclaracion(
                    par_codtipodeclaracion,
                    par_participacion,
                    par_valoradquisicion,
                    par_valorcatastral,
                    par_valoravaluo,
                    par_eshabitacional,
                    par_estasacero,
                    par_idexencion,
                    par_idreduccion,
                    par_codjornotarial,
                    par_porcentajesubsio,
                    par_porcentajedisminucion,
                    par_porcentajecondonacion,
                    (DateTime?)par_fechadocumento,
                    par_regla,
                    par_valorReferido,
                    par_fechaValorReferido,
                    par_codActosJuridico,
                    par_fechaEscritura,
                    out resultadoImporteImpuesto,
                    out resultadoRecargo,
                    out resultadoImporteRecargo,
                    out resultadoActualizacion,
                    out resultadoImporteActualizacion,
                    out resultadoBaseGravable,
                    out resultadoReduccion1995,
                    out resultadoTasa1995,
                    out resultadoImpuesto,
                    out resultadoReduccionArt309,
                    out resultadoExencionImporte,
                    out resultadoImporteCondonacion, out resultadoImporteCondonacionxFecha);

                return resultadoImporteImpuesto.HasValue ? resultadoImporteImpuesto.Value : 0;

            }
            catch (System.ServiceModel.CommunicationException cex)
            {

                ExceptionPolicyWrapper.HandleException(cex);
                string mensaje = null;
                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = cex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (cex.InnerException != null)
                        mensaje = cex.InnerException.Message;
                    else
                        mensaje = cex.Message;
                }

                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (ex.InnerException != null)
                        mensaje = ex.InnerException.Message;
                    else
                        mensaje = ex.Message;
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));

            }
        }


        /// <summary>
        /// Cambia la declaracion al estado Presentada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        public void PresentarDeclaracion(decimal idDeclaracion)
        {
            try
            {
                DeclaracionTA.UPDATE_ESTADODECLARACION(idDeclaracion, (decimal?)EstadosDeclaraciones.Presentada, null);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Cambia la declaracion al estado aceptada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        public void AceptarDeclaracion(decimal idDeclaracion)
        {
            try
            {
                DeclaracionTA.UPDATE_ESTADODECLARACION(idDeclaracion, (decimal?)EstadosDeclaraciones.Aceptada, null);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }

        /// <summary>
        /// Cambia la declaracion al estado rechazada pdt de documentación
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        public void RechazarDeclaracionPdteDocumentacion(decimal idDeclaracion)
        {
            try
            {
                DeclaracionTA.UPDATE_ESTADODECLARACION(idDeclaracion, (decimal?)EstadosDeclaraciones.PendienteDocumentacion, null);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Cambia la declaración al estado rechazada
        /// </summary>
        /// <param name="idDeclaracion">Identificador de la declaración</param>
        /// <param name="codMotivoRechazo">Código del motivo de rechazo</param>
        public void RechazarDeclaracion(decimal idDeclaracion, decimal? codMotivoRechazo)
        {
            try
            {
                DeclaracionTA.UPDATE_ESTADODECLARACION(idDeclaracion, (decimal?)EstadosDeclaraciones.Inconsistente, (decimal?)codMotivoRechazo);
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }


        /// <summary>
        /// Actualiza el estado de pago de las declaraciones
        /// </summary>
        /// <param name="LineasCaptura">Líneas de pago a actualizar</param>
        /// <param name="codigoEstadoPago">Nuevo estado de pago</param>
        public void ActualizarPagoDeclaracion(List<string> LineasCaptura, int codigoEstadoPago)
        {
            try
            {

                #region Codigo anterior

                //using (OracleConnection DBConnection = new OracleConnection(SIGAPred.FuentesExternas.Isai.Services.Properties.Settings.Default.ConnectionString))
                //{
                //    DeclaracionTA.Connection = DBConnection;
                //    DBConnection.Open();
                //    string[] lineasCaptura = listaLineasCaptura.Split(',');
                //    foreach (string lineaCaptura in lineasCaptura)
                //    {
                //        DeclaracionTA.UPDATE_PAGODECLARACION(lineaCaptura, codigoEstadoPago);
                //    }
                //}

                #endregion

                using (OracleConnection DBConnection = new OracleConnection(SIGAPred.FuentesExternas.Isai.Services.Properties.Settings.Default.ConnectionString))
                {
                    DeclaracionTA.Connection = DBConnection;
                    DBConnection.Open();
                    foreach (string lineaCaptura in LineasCaptura)
                    {
                        DeclaracionTA.UPDATE_PAGODECLARACION(lineaCaptura, codigoEstadoPago);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
        }





        //temp
        /// <summary>
        /// Elimina los acentos
        /// </summary>
        /// <param name="source">string con el texto del cual se deben eliminar los acentos</param>
        /// <returns></returns>
        private string RemoveAcentos(string source)
        {
            StringBuilder sb = new StringBuilder(source);
            string[] caracteresNoValidos = new string[] { "Á", "É", "Í", "Ó", "Ú", "á", "é", "í", "ó", "ú" };
            string[] caracteresValidos = new string[] { "A", "E", "I", "O", "U", "a", "e", "i", "o", "u" };
            for (int z = 0; z < caracteresNoValidos.Length; z++)
                sb.Replace(caracteresNoValidos[z], caracteresValidos[z]);
            return sb.ToString();
        }


        #endregion

        #region Asignar fecha de vencimiento

        public string Verificarfechavencida(DateTime Fechainicio)
        {
            DateTime FechaVenc;

            string mensaje;
            try
            {
                string cadenaconexion = SecurityCore.SecurityCoreManager.getStringConnection("FEXNOT");
                OracleConnection conexion = new OracleConnection(cadenaconexion);
                OracleCommand comandofechavencimiento = new OracleCommand("FEXNOT.fexnot_catcalendario_pkg.FECHA_CAUSAACCIONp");
                using (comandofechavencimiento)
                {
                    conexion.Open();
                    comandofechavencimiento.CommandType = CommandType.StoredProcedure;
                    comandofechavencimiento.Parameters.Add("PFECHACAUSAACCION", OracleDbType.Date).Value = Fechainicio;
                    comandofechavencimiento.Parameters.Add("PFECHAVENCIMIENTO", OracleDbType.Date).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Parameters.Add("PMENSAJE", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Connection = conexion;
                    comandofechavencimiento.ExecuteNonQuery();
                    FechaVenc = DateTime.Parse(comandofechavencimiento.Parameters["PFECHAVENCIMIENTO"].Value.ToString());
                    mensaje = comandofechavencimiento.Parameters["PMENSAJE"].Value.ToString();
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }



            return mensaje;
        }

        public DateTime getFechaVencimiento(DateTime fechaCausacion)
        {
            DateTime FechaVenc;

            string mensaje;
            try
            {
                string cadenaconexion = SecurityCore.SecurityCoreManager.getStringConnection("FEXNOT");
                OracleConnection conexion = new OracleConnection(cadenaconexion);
                OracleCommand comandofechavencimiento = new OracleCommand("FEXNOT.fexnot_catcalendario_pkg.FECHA_CAUSAACCIONp");
                using (comandofechavencimiento)
                {
                    conexion.Open();
                    comandofechavencimiento.CommandType = CommandType.StoredProcedure;
                    comandofechavencimiento.Parameters.Add("PFECHACAUSAACCION", OracleDbType.Date).Value = fechaCausacion;
                    comandofechavencimiento.Parameters.Add("PFECHAVENCIMIENTO", OracleDbType.Date).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Parameters.Add("PMENSAJE", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Connection = conexion;
                    comandofechavencimiento.ExecuteNonQuery();
                    FechaVenc = DateTime.Parse(comandofechavencimiento.Parameters["PFECHAVENCIMIENTO"].Value.ToString());
                    mensaje = comandofechavencimiento.Parameters["PMENSAJE"].Value.ToString();
                    conexion.Close();

                    if (!mensaje.ToUpper().Equals("ADEUDO VIGENTE"))
                    {
                        fechaCausacion = DateTime.Now;
                        int dia = fechaCausacion.Day;
                        int mes = fechaCausacion.Month;
                        int año = fechaCausacion.Year;
                        if (dia <= 9)
                        {
                            FechaVenc = new DateTime(año, mes, 9);
                        }
                        else
                        {
                            dia = DateTime.DaysInMonth(año, mes);
                            FechaVenc = new DateTime(año, mes, dia);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
            return FechaVenc;
        }


        public DateTime getFechaProximoVencimiento(DateTime fechaCausacion)
        {
            DateTime FechaVenc;

            try
            {
                string cadenaconexion = SecurityCore.SecurityCoreManager.getStringConnection("FEXNOT");
                OracleConnection conexion = new OracleConnection(cadenaconexion);
                OracleCommand comandofechavencimiento = new OracleCommand("fexnot.fexnot_calculodeclaracion_pkg.fexnot_obtnfechproxvencimien_p");
                using (comandofechavencimiento)
                {
                    conexion.Open();
                    comandofechavencimiento.CommandType = CommandType.StoredProcedure;
                    comandofechavencimiento.Parameters.Add("PFECHACAUSAACCION", OracleDbType.Date).Value = fechaCausacion;
                    comandofechavencimiento.Parameters.Add("P_fechaproxvencimiento", OracleDbType.Date).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Connection = conexion;
                    comandofechavencimiento.ExecuteNonQuery();
                    FechaVenc = DateTime.Parse(comandofechavencimiento.Parameters["P_fechaproxvencimiento"].Value.ToString());
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }
            return FechaVenc;
        }

        #endregion

        public DateTime VerificarDiaFestivo(string Fechainicio)
        {
            DateTime fechaout;
            try
            {
                string cadenaconexion = SecurityCore.SecurityCoreManager.getStringConnection("FEXNOT");
                OracleConnection conexion = new OracleConnection(cadenaconexion);
                OracleCommand comandofechavencimiento = new OracleCommand("FEXNOT.FEXNOT_DETERMINADIA_PKG.fexnot_determinadia_p");
                using (comandofechavencimiento)
                {
                    conexion.Open();
                    comandofechavencimiento.CommandType = CommandType.StoredProcedure;
                    comandofechavencimiento.Parameters.Add("P_STRFECHAIN", OracleDbType.Varchar2).Value = Fechainicio;
                    comandofechavencimiento.Parameters.Add("P_DIA", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Parameters.Add("P_STRFECHAOUT", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Parameters.Add("P_NUMORA", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Parameters.Add("P_STRORA", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Connection = conexion;
                    comandofechavencimiento.ExecuteNonQuery();
                    fechaout = DateTime.Parse(comandofechavencimiento.Parameters["P_STRFECHAOUT"].Value.ToString());

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }

            return fechaout;
        }

        public decimal GetPropietario(decimal idDeclaracion)
        {
            decimal ret;
            try
            {
                string cadenaconexion = SecurityCore.SecurityCoreManager.getStringConnection("FEXNOT");
                OracleConnection conexion = new OracleConnection(cadenaconexion);
                OracleCommand comandofechavencimiento = new OracleCommand("FEXNOT.FEXNOT_DETERMINADIA_PKG.fexnot_determinadia_p");
                using (comandofechavencimiento)
                {
                    conexion.Open();
                    comandofechavencimiento.CommandType = CommandType.StoredProcedure;
                    comandofechavencimiento.Parameters.Add("P_STRFECHAIN", OracleDbType.Int32).Value = idDeclaracion;
                    comandofechavencimiento.Parameters.Add("P_STRORA", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                    comandofechavencimiento.Connection = conexion;
                    comandofechavencimiento.ExecuteNonQuery();
                    ret = Convert.ToDecimal(comandofechavencimiento.Parameters["P_STRORA"].Value.ToString());

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicyWrapper.HandleException(ex);
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(ex));
            }

            return ret;
        }


        private void log(detalleCamposMatriz datos, string usuario, string password, string referencia, string usuarioGen, int totalV, string endpoint, string remoteAdress, string state, int intImpuesto)
        {
            string variable = "";
            try
            {

                //variable = "cobranza";
                //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() 
                //    + " : cobranza: " + datos.cobranza != null ? datos.cobranza.ToString() : "null" + "\n\r");
                //variable = "derecho";
                //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() 
                //    + " : derecho: " + datos.derecho != null ? datos.derecho.ToString() : "null" + "\n\r");
                variable = "usuario";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : usuario: " + usuario + "\n\r");
                variable = "passw";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : passw: " + password + "\n\r");
                variable = "referencia";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : referencia: " + referencia + "\n\r");
                variable = "usuarioGen";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : usuarioGen: " + usuarioGen + "\n\r");
                variable = "totalV";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : totalV: " + totalV.ToString() + "\n\r");
                variable = "endpoint";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : endpoint: " + endpoint + "\n\r");
                variable = "remoteAdress";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : remoteAdress: " + remoteAdress + "\n\r");
                variable = "state";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : state: " + state + "\n\r");
                variable = "intImpuesto";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : intImpuesto: " + intImpuesto.ToString() + "\n\r");


                variable = "bonifica";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : bonifica: " + datos.bonifica.ToString() + "\n\r");
                variable = "ctaeconum";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ctaeconum: " + datos.ctaeconum.ToString() + "\n\r");
                variable = "ctapredial";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ctapredial: " + datos.ctapredial.ToString() + "\n\r");
                variable = "ctarfc";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ctarfc: " + datos.ctarfc.ToString() + "\n\r");
                //variable = "embargo";
                //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : embargo: " + datos.embargo.ToString() + "\n\r");
                variable = "liquidacion";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : liquidacion: " + datos.liquidacion.ToString() + "\n\r");
                //variable = "multa";
                //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : multa: " + datos.multa.ToString() + "\n\r");
                variable = "id_pago";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : id_pago: " + datos.id_pago.ToString() + "\n\r");
                //variable = "importe1";
                //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : importe1: " + datos.importe1.ToString() + "\n\r");
                variable = "impuesto";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : impuesto: " + datos.impuesto.ToString() + "\n\r");
                variable = "numfolio";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : numfolio: " + datos.numfolio.ToString() + "\n\r");
                variable = "otros";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : otros: " + datos.otros.ToString() + "\n\r");
                variable = "recargo1";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : recargo1: " + datos.recargo1.ToString() + "\n\r");
                variable = "recargo2";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : recargo2: " + datos.recargo2.ToString() + "\n\r");
                variable = "total";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : total: " + datos.total.ToString() + "\n\r");
                variable = "vencim";
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : vencim: " + datos.vencim.ToString() + "\n\r");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : Exception: " + variable + "\n\r" + ex.Message + "\n\r");
            }
        }


        /// <summary>
        /// Obtiene línea de captura para una declaración
        /// </summary>
        /// <param name="declaracionIsai">DataSet con la declaración</param>
        /// <param name="esRevisor">Indica si el presentador de la declaración es notario o revisor</param>
        /// <param name="nombreRevisor">Nombre del revisor</param>
        /// <returns>DataSet con la declaración y la línea de captura</returns>
        public DseDeclaracionIsai ObtenerLCIsai(DseDeclaracionIsai declaracionIsai, bool esRevisor, string nombreRevisor, string fechaCausacion)
        {
            SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_respuesta response = new SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_respuesta();
            SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.ServicesPortTypeClient wsProxy = null;
            //ServicesPortTypeClient clienteLC = new ServicesPortTypeClient();
            lineaCapturaGenWs_secureServerPortTypeClient clienteLC = new lineaCapturaGenWs_secureServerPortTypeClient();


            var endpoint = clienteLC.Endpoint.Address.ToString();
            var remoteAdress = clienteLC.InnerChannel.RemoteAddress.ToString();
            var state = clienteLC.State.ToString();
            //var = response.

            try
            {
                DateTime hoy = DateTime.Now;
                DateTime fechaLimPP = getFechaProximoVencimiento(declaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION);
                tp_lineaCapturaGeneral pregunta = new tp_lineaCapturaGeneral();
                string funcion = "isai";
                if (!declaracionIsai.FEXNOT_DECLARACION[0].IsCONDONACIONXFECHANull())
                {
                    if (declaracionIsai.FEXNOT_DECLARACION[0].CONDONACIONXFECHA != 0)
                        funcion = "condona";

                }

                DatosWebService datosIdentificativos = new DatosWebService(funcion);
                RegistroContribuyentes.Services.AccesoDatos.DseInfoContribuyente dseInfo;
                RegistroContribuyentes.Services.AccesoDatos.DseInfoNotarios notarios;
                using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
                {
                    dseInfo = clienteRCON.GetInfoContribuyente(declaracionIsai.FEXNOT_DECLARACION.First().IDPERSONA);
                    notarios = clienteRCON.GetInfoNotario(declaracionIsai.FEXNOT_DECLARACION.First().IDPERSONA, true);
                }
                pregunta.usuario = datosIdentificativos.usuario;
                pregunta.password = datosIdentificativos.password;
                pregunta.intImpuesto = datosIdentificativos.claveImpuesto;
                pregunta.referencia = datosIdentificativos.funcionDeCobro;
                pregunta.fechaLimPP = String.Format("{0}-{1:00}-{2:00}", fechaLimPP.Year,
                                                fechaLimPP.Month,
                                                fechaLimPP.Day);

                

                //pregunta.totalV = Convert.ToInt32( Decimal.Round(declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IMPUESTOPAGADO));
                pregunta.concepto = "02";//datosIdentificativos.funcionDeCobro;
                pregunta.usuarioGen = datosIdentificativos.usuarioSolicita;

                detalleCamposMatriz datos = new detalleCamposMatriz();
                datos.ctapredial = string.Format("{0}{1}{2}{3}{4}",
                                                   declaracionIsai.FEXNOT_DECLARACION[0].REGION,
                                                   declaracionIsai.FEXNOT_DECLARACION[0].MANZANA,
                                                   declaracionIsai.FEXNOT_DECLARACION[0].LOTE,
                                                   declaracionIsai.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA,
                                                   DigitoVerificadorUtils.ObtenerDigitoVerificador(
                                                       declaracionIsai.FEXNOT_DECLARACION[0].REGION,
                                                       declaracionIsai.FEXNOT_DECLARACION[0].MANZANA,
                                                       declaracionIsai.FEXNOT_DECLARACION[0].LOTE,
                                                       declaracionIsai.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA
                                                   ));
                if (dseInfo.Contribuyente.Any())
                {
                    datos.ctarfc = dseInfo.Contribuyente.First().IsRFCNull() ? string.Empty : dseInfo.Contribuyente.First().RFC;
                }

                if (!declaracionIsai.FEXNOT_DECLARACION[0].IsCONDONACIONXFECHANull())
                    datos.recargo2 = declaracionIsai.FEXNOT_DECLARACION[0].CONDONACIONXFECHA.ToString();
                datos.vencim = pregunta.fechaLimPP.Replace("-", "");
                datos.numfolio = "0.00";
                datos.liquidacion = "0.00";
                datos.impuesto = declaracionIsai.FEXNOT_DECLARACION[0].IMPUESTO.ToRoundTwoDecimalsString();
                datos.otros = declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IsACTUALIZACIONIMPORTENull() ? "0.00" : declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().ACTUALIZACIONIMPORTE.ToRoundTwoDecimalsString();
                var row = declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault();
                decimal _beneficios = 0M;

                if (!row.IsIMPORTEEXENCIONNull() && row.IMPORTEEXENCION != 0)
                    _beneficios = row.IMPORTEEXENCION;
                else if (!row.IsREDUCCIONART309Null() && row.REDUCCIONART309 != 0)
                    _beneficios = row.REDUCCIONART309;


                datos.bonifica = _beneficios.ToRoundTwoDecimalsString();
                datos.recargo1 = declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IsIMPORTERECARGOPAGOEXTEMPNull() ? "0.00" : declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IMPORTERECARGOPAGOEXTEMP.ToRoundTwoDecimalsString();
                datos.total = declaracionIsai.FEXNOT_DECLARACION[0].IMPUESTOPAGADO.ToRoundTwoDecimalsString();
                datos.id_pago = datosIdentificativos.idPago;

                //Se determina si el impuesto se redondea hacia arriba en caso de que los decimales sean mayores a 0.50
                //O hacia abajo en caso contrario.
                double tot = (double)datos.total.ToDecimal(); //Decimal.Round(declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IMPUESTOPAGADO);
                if ((tot - Math.Truncate(tot)) > 0.50)
                    pregunta.totalV = (int)Math.Ceiling(tot);
                else
                    pregunta.totalV = (int)Math.Truncate(tot);

                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString()
                    + " : ObtenerLCIsai tot: " + tot.ToString()
                    + " | Diferencia : " + (tot - Math.Truncate(tot)).ToString()
                    + " | pregunta.totalV: " + pregunta.totalV.ToString() + " \n\r");

                if (notarios.Notario.Any())
                {
                    datos.ctaeconum = notarios.Notario.First().NUMNOTARIO.ToString();
                }
                pregunta.arrayDatos = new detalleCamposMatriz[1];
                pregunta.arrayDatos[0] = datos;
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                 delegate (object sender2, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                 System.Security.Cryptography.X509Certificates.X509Chain chain,
                 System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };

                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) : " + datos.ToString() + "\n\r");
                log(datos, pregunta.usuario, pregunta.password, pregunta.referencia, pregunta.usuarioGen, pregunta.totalV, endpoint, remoteAdress, state, pregunta.intImpuesto);
                //var lineas = clienteLC.solicitaLineaCapturaGen(pregunta);
                tr_lineaCapturaGeneral[] lineas = clienteLC.solicitaLineaCapturaGen(pregunta);
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) asignacionResponse \n\r");
                //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) lineas.Length : " + lineas.ToString() + "\n\r");
                clienteLC.Close();
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) clienteLC.Close \n\r");
                //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) lineas.Length : " + lineas.Length.ToString() + "\n\r");

                if (lineas.Length > 0)
                {
                    tr_lineaCapturaGeneral linea = lineas.FirstOrDefault();
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) lineas.error : " + linea.error.ToString() + "\n\r");
                    //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) lineas.error_descripcion : " + linea.error_descripcion.ToString() + "\n\r");

                    if (!string.IsNullOrEmpty(linea.lineaCaptura))
                    {
                        declaracionIsai.FEXNOT_DECLARACION[0].LINEACAPTURA = linea.lineaCaptura.Substring(0, 20);
                        declaracionIsai.FEXNOT_DECLARACION[0].FECHALINEACAPTURA = DateTime.Today;
                        declaracionIsai.FEXNOT_DECLARACION[0].FECHAVIGENCIALINEACAPTURA = fechaLimPP;

                        StringBuilder campoFut = new StringBuilder();
                        campoFut.AppendFormat("{9}|caja={0}|error={1}|error_descripcion={2}|fcobro={3}|intImpuesto={4}|lineaCaptura={5}|partida={6}|rafagaPago={7}|referencia={8}",
                            linea.caja,
                            linea.error,
                            linea.error_descripcion,
                            linea.fcobro,
                            linea.intImpuesto,
                            linea.lineaCaptura,
                            linea.partida,
                            linea.rafagaPago,
                            linea.referencia,
                            linea.lineaCaptura);

                        declaracionIsai.FEXNOT_DECLARACION[0].FUT = campoFut.ToString();
                        RegistrarDeclaracion(ref declaracionIsai, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, -1, null);
                        return declaracionIsai;
                    }
                    else
                        throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(response.error_descripcion));
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException("La respuesta obtenida del servicio no es valida."));
            }
            catch (System.ServiceModel.CommunicationException cex)
            {
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) CommunicationException : " + cex.Message + "\n\r");

                ExceptionPolicyWrapper.HandleException(cex);

                string mensaje = null;
                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = cex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                    {
                        System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) faultEx : " + faultEx.Detail.Descripcion + "\n\r");

                        mensaje = faultEx.Detail.Descripcion;
                    }
                    else
                    {
                        System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) faultEx : Se produjo un error al realizar la operación \n\r");

                        mensaje = "Se produjo un error al realizar la operación";
                    }
                }
                else
                {
                    if (cex.InnerException != null)
                        mensaje = cex.InnerException.Message;
                    else
                        mensaje = cex.Message;
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) InnerException : " + mensaje + " \n\r");

                }

                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) Exception : " + ex.Message + "\n\r");

                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) FaultException : " + mensaje + "\n\r");


                }
                else
                {
                    if (ex.InnerException != null)
                        mensaje = ex.InnerException.Message;
                    else
                        mensaje = ex.Message;
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) FaultExceptionELSE : " + mensaje + "\n\r");
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsai(4) FaultExceptionELSE : " + ex.StackTrace + "\n\r");
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));

            }
            finally
            {
                if (clienteLC != null && clienteLC != null)
                    ((IDisposable)clienteLC).Dispose();
            }
        }


        /// <summary>
        /// Obtiene línea de captura para una declaración
        /// </summary>
        /// <param name="declaracionIsai">DataSet con la declaración</param>
        /// <returns>La linea de captura devuelta por el servicio</returns>
        public string ObtenerLCIsr(DseDeclaracionIsai declaracionIsai)
        {
            SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_respuesta response = new SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.tipo_respuesta();
            SIGAPred.FuentesExternas.Isai.Services.ServiceFUT.ServicesPortTypeClient wsProxy = null;
            //ServicesPortTypeClient clienteLC = new ServicesPortTypeClient();
            lineaCapturaGenWs_secureServerPortTypeClient clienteLC = new lineaCapturaGenWs_secureServerPortTypeClient();

            try
            {
                DateTime fechaLimPP = getFechaVencimiento(declaracionIsai.FEXNOT_DECLARACION[0].FECHACAUSACION);
                DateTime hoy = DateTime.Now;
                tp_lineaCapturaGeneral pregunta = new tp_lineaCapturaGeneral();
                DatosWebService datosIdentificativos = new DatosWebService("isr");
                RegistroContribuyentes.Services.AccesoDatos.DseInfoContribuyente dseInfo;
                RegistroContribuyentes.Services.AccesoDatos.DseInfoNotarios notarios;
                using (RegistroContribuyentesClient clienteRCON = new RegistroContribuyentesClient())
                {
                    dseInfo = clienteRCON.GetInfoContribuyente(declaracionIsai.FEXNOT_DECLARACION.First().IDPERSONA);
                    notarios = clienteRCON.GetInfoNotario(declaracionIsai.FEXNOT_DECLARACION.First().IDPERSONA, true);
                }
                pregunta.usuario = datosIdentificativos.usuario;
                pregunta.password = datosIdentificativos.password;
                pregunta.intImpuesto = datosIdentificativos.claveImpuesto;
                pregunta.referencia = datosIdentificativos.funcionDeCobro;
                pregunta.fechaLimPP = String.Format("{0}-{1:00}-{2:00}", hoy.Year,
                                                hoy.Month,
                                                hoy.Day);

                

                //pregunta.totalV = Convert.ToInt32(Decimal.Round(declaracionIsai.FEXNOT_DECLARACION[0].IMPORTELOCALISR));
                pregunta.concepto = "02";//datosIdentificativos.funcionDeCobro;
                pregunta.usuarioGen = datosIdentificativos.usuarioSolicita;

                detalleCamposMatriz datos = new detalleCamposMatriz();
                datos.ctapredial = string.Format("{0}{1}{2}{3}{4}",
                                                   declaracionIsai.FEXNOT_DECLARACION[0].REGION,
                                                   declaracionIsai.FEXNOT_DECLARACION[0].MANZANA,
                                                   declaracionIsai.FEXNOT_DECLARACION[0].LOTE,
                                                   declaracionIsai.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA,
                                                   DigitoVerificadorUtils.ObtenerDigitoVerificador(
                                                       declaracionIsai.FEXNOT_DECLARACION[0].REGION,
                                                       declaracionIsai.FEXNOT_DECLARACION[0].MANZANA,
                                                       declaracionIsai.FEXNOT_DECLARACION[0].LOTE,
                                                       declaracionIsai.FEXNOT_DECLARACION[0].UNIDADPRIVATIVA
                                                   ));
                if (dseInfo.Contribuyente.Any())
                {
                    datos.ctarfc = dseInfo.Contribuyente.First().IsRFCNull() ? string.Empty : dseInfo.Contribuyente.First().RFC;
                }
                datos.vencim = pregunta.fechaLimPP.Replace("-", "");
                datos.liquidacion = "0.00";
                datos.impuesto = declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IMPORTELOCALISR.ToRoundTwoDecimalsString();
                datos.otros = "0.00";
                datos.recargo1 = "0.00";
                datos.total = datos.impuesto;


                //Se determina si el impuesto se redondea hacia arriba en caso de que los decimales sean mayores a 0.50
                //O hacia abajo en caso contrario.
                double tot = (double)datos.total.ToDecimal(); //Decimal.Round(declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IMPUESTOPAGADO);
                if ((tot - Math.Truncate(tot)) > 0.50)
                    pregunta.totalV = (int)Math.Ceiling(tot);
                else
                    pregunta.totalV = (int)Math.Truncate(tot);

                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString()
                    + " : ObtenerLCIsr tot: " + tot.ToString()
                    + " | Diferencia : " + (tot - Math.Truncate(tot)).ToString()
                    + " | pregunta.totalV: " + pregunta.totalV.ToString() + " \n\r");


                datos.id_pago = datosIdentificativos.idPago;
                if (notarios.Notario.Any())
                {
                    datos.ctaeconum = notarios.Notario.First().NUMNOTARIO.ToString();
                }
                pregunta.arrayDatos = new detalleCamposMatriz[1];
                pregunta.arrayDatos[0] = datos;
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                 delegate (object sender2, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                 System.Security.Cryptography.X509Certificates.X509Chain chain,
                  System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsr : " + datos.ToString() + "\n\r");
                log(datos, pregunta.usuario, pregunta.password, pregunta.referencia, pregunta.usuarioGen, pregunta.totalV, "", "", "", 0);
                tr_lineaCapturaGeneral[] lineas = clienteLC.solicitaLineaCapturaGen(pregunta);
                clienteLC.Close();
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsr(1) lineas.Length : " + lineas.Length.ToString() + "\n\r");

                if (lineas.Length > 0)
                {
                    tr_lineaCapturaGeneral linea = lineas.FirstOrDefault();
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsr(1) lineas.error : " + linea.error.ToString() + "\n\r");
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ISAI.log", "\n\r" + DateTime.Now.ToString() + " : ObtenerLCIsr(1) lineas.error_descripcion : " + linea.error_descripcion.ToString() + "\n\r");

                    if (!string.IsNullOrEmpty(linea.lineaCaptura))
                    {
                        string campoFut = string.Empty;
                        campoFut = string.Format("{9}|caja={0}|error={1}|error_descripcion={2}|fcobro={3}|intImpuesto={4}|lineaCaptura={5}|partida={6}|rafagaPago={7}|referencia={8}",
                            linea.caja,
                            linea.error,
                            linea.error_descripcion,
                            linea.fcobro,
                            linea.intImpuesto,
                            linea.lineaCaptura,
                            linea.partida,
                            linea.rafagaPago,
                            linea.referencia,
                            linea.lineaCaptura);
                        UpdateLC(declaracionIsai.FEXNOT_DECLARACION.FirstOrDefault().IDDECLARACION, linea.lineaCaptura, DateTime.Now, campoFut, fechaLimPP);
                        return linea.lineaCaptura.Substring(0, 20);
                    }
                    else
                        throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(response.error_descripcion));
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException("La respuesta obtenida del servicio no es valida."));
            }
            catch (System.ServiceModel.CommunicationException cex)
            {

                ExceptionPolicyWrapper.HandleException(cex);

                string mensaje = null;
                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = cex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (cex.InnerException != null)
                        mensaje = cex.InnerException.Message;
                    else
                        mensaje = cex.Message;
                }

                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));
            }
            catch (Exception ex)
            {

                ExceptionPolicyWrapper.HandleException(ex);

                string mensaje = null;

                System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException> faultEx = ex as System.ServiceModel.FaultException<SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions.DeclaracionIsaiException>;
                if (faultEx != null)
                {
                    if (faultEx.Detail != null && faultEx.Detail.Descripcion != null)
                        mensaje = faultEx.Detail.Descripcion;
                    else
                        mensaje = "Se produjo un error al realizar la operación";
                }
                else
                {
                    if (ex.InnerException != null)
                        mensaje = ex.InnerException.Message;
                    else
                        mensaje = ex.Message;
                }
                throw new FaultException<Exceptions.DeclaracionIsaiException>(new Exceptions.DeclaracionIsaiException(mensaje));

            }
            finally
            {
                if (clienteLC != null)
                    ((IDisposable)clienteLC).Dispose();
            }
        }

        /// <summary>
        /// Actualiza los campos de la linea de captura de isr
        /// </summary>
        /// <param name="idDeclaracion">El id de la declaración que se desea actualizar</param>
        /// <param name="linea">La linea de captura que se va a actualizar</param>
        /// <param name="fechaVencimiento">La fecha de vencimiento de la linea</param>
        /// <param name="fechaObtencion">La fecha de obtención de la linea</param>
        /// <param name="fut">EL campo que se genera con todos los datos regresados por el servicio de la secretaria</param>
        /// <returns>true en caso correcto, false en caso contrario</returns>
        private bool UpdateLC(decimal idDeclaracion, string linea, DateTime fechaObtencion, string fut, DateTime fechaVencimiento)
        {
            bool r = true;
            string mensaje = string.Empty;
            try
            {
                SecurityCore.TransactionHelper tranHelper = new SecurityCore.TransactionHelper();
                OracleCommand comand = new OracleCommand("fexnot.fexnot_declaracion_pkg.fexnot_update_isr_p");
                using (comand)
                {
                    comand.CommandType = CommandType.StoredProcedure;
                    comand.Parameters.Add("P_IDDECLARACION", OracleDbType.Int32, idDeclaracion, ParameterDirection.Input);
                    comand.Parameters.Add("P_LINEACAPTURA_ISR", OracleDbType.Varchar2, 50, linea, ParameterDirection.Input);
                    comand.Parameters.Add("P_FECHALINEACAPTURA_ISR", OracleDbType.Date, fechaVencimiento, ParameterDirection.Input);
                    comand.Parameters.Add("P_FUT_ISR", OracleDbType.Varchar2, 3000, fut, ParameterDirection.Input);
                    comand.Parameters.Add("P_FECVIGENCIALINEACAPTURA_ISR", OracleDbType.Date, fechaVencimiento, ParameterDirection.Input);
                    comand.Parameters.Add("P_MSGERROR", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                    tranHelper.EjecutaNonQuerySP(comand);
                    mensaje = comand.Parameters["P_MSGERROR"].Value.ToString();
                    r = mensaje.ToUpper().Contains("OK");
                    if (!r)
                    {
                        throw new Exception(mensaje);
                    }

                }
            }
            catch (Exception e)
            {
                r = false;
                ExceptionPolicyWrapper.HandleException(e);
                throw new Exception("Ocurrió un error inesperado al actualizar la Linea de captura.");
            }
            return r;
        }

        /// <summary>
        /// Obtiene los datos de la linea de captura de isr
        /// </summary>
        /// <param name="idDeclaracion">El id de la declaración de la que se desea obtener la linea de captura</param>
        /// <returns>Una lista con los siguientes datos:
        ///     Posicion [0]: IDDECLARACION
        ///     Posicion [1]: CAUSAISR
        ///     Posicion [2]: IMPORTELOCALISR
        ///     Posicion [3]: IMPORTEFEDERALISR
        ///     Posicion [4]: LINEACAPTURA_ISR
        ///     Posicion [5]: FECHALINEACAPTURA_ISR
        ///     Posicion [6]: FUT_ISR
        ///     Posicion [7]: FECHAVIGENCIALINEACAPTURA_ISR
        ///     Posicion [8]: FECHAPAGO_ISR
        ///     Posicion [9]: ESTADO
        /// </returns>
        public List<string> GetDatosLCISR(decimal idDeclaracion)
        {
            List<string> lista = new List<string>();
            try
            {
                SecurityCore.TransactionHelper tranHelper = new SecurityCore.TransactionHelper();
                OracleCommand comando = new OracleCommand("fexnot.fexnot_declaracion_pkg.fexnot_obtn_declaracion_isr_p");
                DataSet ds = new DataSet();
                using (comando)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add("P_IDDECLARACION", OracleDbType.Int32).Value = idDeclaracion;
                    comando.Parameters.Add("P_CONSULTA", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    ds = tranHelper.EjecutaConsultaSP(comando);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            lista.Add(dr.ToStringValue("IDDECLARACION"));
                            lista.Add(dr.ToStringValue("CAUSAISR"));
                            lista.Add(dr.ToStringValue("IMPORTELOCALISR"));
                            lista.Add(dr.ToStringValue("IMPORTEFEDERALISR"));
                            lista.Add(dr.ToStringValue("LINEACAPTURA_ISR"));
                            lista.Add(dr.ToShortDateStringValue("FECHALINEACAPTURA_ISR"));
                            lista.Add(dr.ToStringValue("FUT_ISR"));
                            lista.Add(dr.ToStringValue("FECHAVIGENCIALINEACAPTURA_ISR"));
                            lista.Add(dr.ToStringValue("FECHAPAGO_ISR"));
                            lista.Add(dr.ToStringValue("ESTADO"));
                        }
                    }
                }


            }
            catch (Exception e)
            {
                ExceptionPolicyWrapper.HandleException(e);
                throw new Exception("Ocurrió un error inesperado al consultar la Linea de captura.");
            }
            return lista;
        }

        public List<decimal> GetDatosIsr(decimal idDeclaracion)
        {
            List<decimal> lista = new List<decimal>();
            try
            {
                lista = Isr.GetValues(idDeclaracion);
            }
            catch { }
            return lista;
        }


    }

}

