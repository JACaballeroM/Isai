using System;


/// <summary>
/// Información adicional de eventos sujeto pasivo.
/// </summary>
public class SujetoPasivoEventArgs : EventArgs
{
    #region Campos

    private decimal? idParticipante;
    private decimal? porcTotal;

    #endregion

    #region Propiedades

    /// <summary>
    /// Obtiene o establece el identificador participante.
    /// </summary>
    /// <value>
    /// The identificador participante.
    /// </value>
    public decimal? IdParticipante
    {
        get { return idParticipante; }
        set { idParticipante = value; }
    }

    /// <summary>
    /// Obtiene o establece el identificador persona.
    /// </summary>
    /// <value>
    /// The identificador persona.
    /// </value>
    public decimal? PorcTotal
    {
        get { return porcTotal; }
        set { porcTotal = value; }
    }

    #endregion

    #region constructores

    /// <summary>
    /// Constructor por defecto.
    /// </summary>
    public SujetoPasivoEventArgs()
        : base()
    {

    }

    /// <summary>
    /// Constructor por defecto.
    /// Inicializa las propiedades de la clase
    /// </summary>
    /// <param name="idParticipante">El identificador participante.</param>
    /// <param name="idPersona">El identificador persona.</param>
    public SujetoPasivoEventArgs(decimal? idParticipante, decimal? porcentajePersona)
        : base()
    {
        this.IdParticipante = idParticipante;
        this.PorcTotal = porcentajePersona;
    }

    #endregion
}


