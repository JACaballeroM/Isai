using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

/// <summary>
/// Clase que proporciona funciones útiles para el tratamiento de textos
/// </summary>
public static class Utilidades
{
    /// <summary>
    /// Cadena que almacena la URL desencriptada
    /// </summary>
    private static string urlDecripted = string.Empty;
    
    /// <summary>
    /// Cadena que almacena la URL encriptada
    /// </summary>
    private static string urlEncripted = string.Empty;

    /// <summary>
    /// Función que codificara el xml en base64
    /// </summary>
    /// <param name="data">Cadena que contiene el xml el cual queremos convertir</param>
    /// <returns>Cadena de texto con el contenido de la encriptacion en base64</returns>
    public static string base64Encode(string data)
    {
        try
        {
            byte[] encData_byte = new byte[data.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception e)
        {
            throw new Exception("Error en base64Encode" + e.Message);
        }
    }

    /// <summary>
    /// Función que decodifica en base64
    /// </summary>
    /// <param name="data">Cadena con los datos que queremos descifrar</param>
    /// <returns>Cadena que contendra el xml</returns>
    public static string base64Decode(string data)
    {
        try
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error en base64Decode" + e.Message);
        }
    }


    /// <summary>
    /// Función auxiliar para comprimir un array de bytes
    /// </summary>
    /// <param name="info"></param>
    /// <returns>Bytes comprimidos</returns>
    public static byte[] Comprimir(byte[] info)
    {
        MemoryStream ms = new MemoryStream();
        GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
        zip.Write(info, 0, info.Length);
        zip.Close();
        return ms.ToArray();
    }


    /// <summary>
    /// Función auxiliar para descomprimir un array de bytes
    /// </summary>
    /// <param name="info">Bytes a descomprimir</param>
    /// <returns>Bytes descomprimidos</returns>
    public static byte[] Decomprimir(byte[] info)
    {
        MemoryStream ms = new MemoryStream();
        ms.Write(info, 0, info.Length);
        ms.Position = 0;
        GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
        MemoryStream result = new MemoryStream();
        byte[] buffer = new byte[64];
        int bytesRead = -1;
        bytesRead = zip.Read(buffer, 0, buffer.Length);
        while (bytesRead > 0)
        {
            result.Write(buffer, 0, bytesRead);
            bytesRead = zip.Read(buffer, 0, buffer.Length);
        }
        zip.Close();
        return result.ToArray();
    }


    /// <summary>
    /// Función que obtiene el valor del parámetro pasado por parámetro dentro de la url especificada
    /// </summary>
    /// <param name="name">Url de la que obtener el valor del parámetro</param>
    /// <returns>Valor del parámetro solicitado</returns>
    public static string GetParametroUrl(string name)
    {
        string url = System.Web.HttpContext.Current.Request.Url.PathAndQuery;

        if (url != urlEncripted)
        {
            urlEncripted = url;
            urlDecripted = SIGAPred.Common.Web.Redirect.IsEncripted(url) ? SIGAPred.Common.Web.Redirect.DencryptUrl(url) : url;
        }

        string regexS = "[\\?&]" + name + "=([^&#]*)";
        Regex regex = new Regex(regexS);
        Match results = regex.Match(urlDecripted);
        if (results == null)
        {
            return null;
        }
        else
        {
            return results.Groups[1].Value;
        }
    }
}