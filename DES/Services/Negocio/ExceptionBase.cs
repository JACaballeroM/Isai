using System;
using System.Runtime.Serialization;

namespace SIGAPred.FuentesExternas.Isai.Services.Negocio
{

    /// <summary>
    /// Clase base que define una excepción generada por wcf
    /// </summary>
    [DataContract(Namespace = "http://SIGAPred.FuentesExternas/Isai/Negocio/Exceptions")]
    public class ExceptionBase
    {
        #region Campos

        /// <summary>
        /// Descripción del error
        /// </summary>
        private String descripcion;

        /// <summary>
        /// Fecha en la que se produjo el error
        /// </summary>
        private DateTime fecha;

        #endregion

        #region Propiedades

        /// <summary>
        /// Descripción del error
        /// </summary>
        [DataMember]
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        /// <summary>
        /// Fecha en la que se produjo el error
        /// </summary>
        [DataMember]
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Crea una nueva instancia al objeto
        /// </summary>
        /// <param name="ex">Excepción capturada</param>
        public ExceptionBase(Exception ex)
        {
            this.Descripcion = ex.Message;
            this.Fecha = DateTime.Now;
        }

        /// <summary>
        /// Crea una nueva instancia al objeto
        /// </summary>
        public ExceptionBase()
        {
        }

        /// <summary>
        /// Crea una nueva instancia al objeto
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public ExceptionBase(string message)
        {
            this.Descripcion = message;
            this.fecha = DateTime.Now;
        }

        #endregion
    }

}
