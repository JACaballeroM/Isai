//Constantes integración contribuyentes
var nombreVentanaBuscadorPeritos = 'Buscador peritos';
var widthVentanaBuscadorPeritos = '800';
var heigthVentanaBuscadorPeritos = '600';
var nombreVentanaBuscadorNotarios = 'Buscador notarios';
var widthVentanaBuscadorNotarios = '800';
var heigthVentanaBuscadorNotarios = '600';


function abrirBuscadorPeritos(idReturn, urlBuscadorPeritos)
{   
    modalWindow(
        urlBuscadorPeritos + '?idtipodocumento=1&iddocexpediente=1',
        nombreVentanaBuscadorPeritos, 
        widthVentanaBuscadorPeritos, 
        heigthVentanaBuscadorPeritos);   
}

function abrirBuscadorNotarios(idReturn, urlBuscadorNotarios)
{   
    modalWindow(
        urlBuscadorNotarios + '?idtipodocumento=1&iddocexpediente=1',
        nombreVentanaBuscadorNotarios, 
        widthVentanaBuscadorNotarios, 
        heigthVentanaBuscadorNotarios);
}



function abrirUrlBuscadorPersonas(url, textEspClientId) {


    ValoresDevueltos = window.open(url + "?TipoBusqueda=S","",
        "dialogWidth:580px;dialogHeight:550px;resizable:no;toolbar:no;menubar:no;scroll:1;status=1");

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