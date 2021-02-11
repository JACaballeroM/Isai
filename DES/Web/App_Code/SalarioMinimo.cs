using System;

/// <summary>
/// Clase encargada de gestionar el salario mínimo
/// </summary>
[Serializable]
public class SalarioMinimo
{
    /// <summary>
    /// Propiedad que obtiene y almacena la vigencia 
    /// </summary>
    public DateTime Vigencia
    {
        get;
        set;
    }

    /// <summary>
    /// Propiedad que obtiene y establece el monto del salario mínimo
    /// </summary>
    public decimal Monto
    {
        get;
        set;
    }

    /// <summary>
    /// Constructor de la clase salario mínimo
    /// </summary>
    public SalarioMinimo()
    {
    }
}
