using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Claims;
using SIGAPred.Seguridad.Utilidades.ClaimTypes;

/// <summary>
/// Clase para gestionar los usuarios
/// </summary>
public class Usuarios
{

    /// <summary>
    /// Función que obtiene el identificador de la persona logedada
    /// </summary>
    /// <returns>String con el id persona</returns>
    public static string IdPersona()
    {
        string idPersona;
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
        {
            idPersona = "-1";
        }
        else
        {
            IClaimsIdentity identity = (IClaimsIdentity)HttpContext.Current.User.Identity;
            if ((from c in identity.Claims where c.ClaimType == PromocaClaims.IdPersona select c).Any())
                idPersona = (from c in identity.Claims where c.ClaimType == PromocaClaims.IdPersona select c).First().Value;
            else
                throw new UserFailedException();
        }
        return idPersona;
    }


    /// <summary>
    /// Función que obtiene el identificador del usuario logedada
    /// </summary>
    /// <returns>String con el id usuario</returns>
    public static string IdUsuario()
    {
        string idUsuario;
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
        {
            idUsuario = "-1";
        }
        else
        {
            SIGAPred.Common.Token.TokenTransport token = new SIGAPred.Common.Token.TokenBuilder(HttpContext.Current.User.Identity).Token;
            idUsuario = token.Identity;
        }
        return idUsuario;
    }
}
