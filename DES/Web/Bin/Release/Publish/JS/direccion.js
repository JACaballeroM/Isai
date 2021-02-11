//Función encargada de abrir la ventana de especificar dirección y de pasar los parámetros
function AbrirUrlDireccion(url, textEspClientId, xml, soloLectura, idDireccion) {

    valores = new Array();
    valores[0] = xml;

    ValoresDevueltos = window.open(url + "?ReadOnly=" + soloLectura + "&IdDireccion=" + idDireccion,
                                       valores,
        "dialogWidth:640px;dialogHeight:550px;resizable:no;toolbar:no;menubar:no;scroll:1;status=1");

    //Solo se llama a la funcion si se ha devuelto valores
    //En la posicion 1 está el xml en base 64 si es de forma transaccional
    if (ValoresDevueltos) {
        if (textEspClientId != undefined && textEspClientId != "") {
            //Obtenemos el control TextBox de especificación y ponemos en él el xml generado
            var controlTextBox = document.getElementById(textEspClientId);
            controlTextBox.value = ValoresDevueltos[0];
            //Si hay valores lanza el evento
            if (ValoresDevueltos[0] != null) {
                //Disparamos el evento onChanged del hiddenfield
                controlTextBox.fireEvent("onchange");
            }
        }
    }
}