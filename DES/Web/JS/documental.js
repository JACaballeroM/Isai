//Constantes integración documental
var nombreVentanaDocumental = 'Documento de la declaración';
var widthVentanaDocumental = '800';
var heigthVentanaDocumental = '600';


//Función encargada de abrir la ventana de especificar documentos y de pasar los parámetros
//Se agrega el nombre de la pagina "NombrePagina"
function AbrirUrlDestino(
url, idTipoDocDigital, idTipoDocJuridico, idDocDigital, transaccional, textBoxEspClientId, textBoxTipoDocClientId, textBoxListaIdClientId, Edicion, xml, listaIdsFich, provisional, idNotario, NombrePagina) {
    var parametros = new Array();
    parametros[0]=xml;
    parametros[1]=listaIdsFich;
    ValoresDevueltos = showModalDialog(url
        + "?IdTipoDocumentoDigital=" + idTipoDocDigital
        + "&IdTipoDocumentoJuridico=" + idTipoDocJuridico
        + "&Transaccional=" + transaccional
        + "&IdDocumentoDigital=" + idDocDigital
        + "&Edicion=" + Edicion
        + "&Provisional=" + provisional
        + "&IdNotario=" + idNotario
        + "&NombrePagina=" + NombrePagina,
        parametros,
        'dialogWidth:640px;dialogHeight:480px;resizable:no;toolbar:no;menubar:no;scrollbars:1;status=1');

    //Solo se llama a la siguiente funcion si se han devuelto valores
    //ValoresDevueltos es un array de 3 posiciones
    //En la posicion 1 esta el id del documento digital si es no transaccional o
    //el xml en base 64 si es transaccional
    //En la posicion 2 esta el Tipo de Documento digital
    //En la posicion 3 esta la lista de los ids de ficheros documentos
//    document.getElementById(txtIdDocDig).value = "";
    if (ValoresDevueltos) 
    {
//            document.getElementById(txtIdDocDig).value = ValoresDevueltos[0];
            GuardarDatos(
textBoxEspClientId, textBoxTipoDocClientId, textBoxListaIdClientId, ValoresDevueltos[0], ValoresDevueltos[1], ValoresDevueltos[2]);       
    }
}


function GuardarDatos(
textBoxEspClientId, textBoxTipoDocClientId, textBoxListaIdClientId, valorEspecificado, tipoDocumento, listaIdsFicheros)
{
    //Guardamos en los textboxes los valores de tipo documento y lista de ids
    //ficheros
    document.getElementById(textBoxTipoDocClientId).value = tipoDocumento;
    document.getElementById(textBoxListaIdClientId).value = listaIdsFicheros; 
    
    //Obtenemos el control TextBox de txtEspecificaion
    var controlTextBox = document.getElementById(textBoxEspClientId);

    //Guarda el valor actual del textbox
    var valorAnteriorTextBox = controlTextBox.value;

    //Pone en el textbox valorEspecificado, que contendra un string con
    //el xml en base64
    controlTextBox.value = valorEspecificado;
    
    //Si el valorEspecificado no es nulo y los valores han
    //cambiado lanza el evento
    if (valorEspecificado!=null)
    {
        //Disparamos el evento onChanged del TextBox txtEspecificacion
     //   Event.fire(controlTextBox, 'onchange');
          
//        controlTextBox.fireEvent("onchange");
    }
}









