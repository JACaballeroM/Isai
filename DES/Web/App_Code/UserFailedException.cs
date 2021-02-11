using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Excepción para la gestión de usuarios en la aplicacción.
/// </summary>
public class UserFailedException : SystemException
{
    /// <summary>
    /// Excepción para cuando no existe el usuario.
    /// </summary>
    public UserFailedException()
        : base(Constantes.MSJ_USUARIONOEXISTE_EXECP)
    {

    }

}
