using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace SIGAPred.FuentesExternas.Isai.Services.Negocio.Exceptions
{
   
     /// <summary>
    /// Clase que define una excepción en los métodos de comunicación con agilepoint
    /// </summary>
    [DataContract(Namespace = "http://SIGAPred.FuentesExternas/Isai/Negocio/Exceptions")]
    
    public class DeclaracionIsaiInfoException:ExceptionBase
    {
        #region Constructor

        /// <summary>
        /// Crea una nueva instancia al objeto
        /// </summary>
        /// <param name="ex">Excepción capturada</param>
        public DeclaracionIsaiInfoException(Exception ex)
            : base(ex)
        {
        }

        /// <summary>
        /// Crea una nueva instancia al objeto
        /// </summary>
        /// <param name="message">Mensaje de la excepcion</param>
        public DeclaracionIsaiInfoException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
