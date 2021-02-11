function prueba()
{
alert('Prueba');
}

function modalWindow(url, title, width, heigth) {
	if (window.showModalDialog) {
		window.showModalDialog(
			url,
			title,
			'dialogWidth:'+ width + 'px;dialogHeight:' + heigth + 'px');
	} else {
		child = window.open(
			url,
			title,
			'height=' + heigth + ',width=' + width + ',toolbar=no,directories=no,status=no,	menubar=no,scrollbars=no,resizable=no ,modal=yes');
	}
} 
function activarControles(control, controles)
{

    var arrayControles = controles.split(';');
    for(i=0;i< controles.length; i++)
    {
        document.getElementById(ddlReutilizar).enabled = control.cheched;
    }
   
}
function MostrarOcultarControl(idControl,mostrar)
{ 
    if(mostrar)
        document.getElementById(idControl).style.display='block';        
    else if(!mostrar)
        document.getElementById(idControl).style.display='none';        
}

//Función que comprueba el rango máximo entre dos fecha que no sea superior a 30 días
//parametros de entrada las dos fechas a comparar y la cultura o formato de la fecha
function rangoFechasMaxMes(fechaI, fechaF, dateFormat) 
{
    var fechaInicio = document.getElementById(fechaI).value;
    var fechaFin = document.getElementById(fechaF).value;


        //se comprueba que este bien completa todos los caracteres del campo fecha inicio
        if (fechaInicio.split('/')[0].length == 2) {
            var diaI = fechaInicio.split('/')[0];
        }
        else {
            var diaI = '0' + fechaInicio.split('/')[0];
        }
        if (fechaInicio.split('/')[1].length == 2) {
            var mesI = fechaInicio.split('/')[1];
        }
        else {
            var mesI = '0' + fechaInicio.split('/')[1];
        }
        fechaInicio = diaI + '/' + mesI + '/' + fechaInicio.split('/')[2]

        //se comprueba que este bien completa todos los caracteres del campo fecha fin
        if (fechaFin.split('/')[0].length == 2) {
            var diaI = fechaFin.split('/')[0];
        }
        else {
            var diaI = '0' + fechaFin.split('/')[0];
        }
        if (fechaFin.split('/')[1].length == 2) {
            var mesI = fechaFin.split('/')[1];
        }
        else {
            var mesI = '0' + fechaFin.split('/')[1];
        }
        fechaFin = diaI + '/' + mesI + '/' + fechaFin.split('/')[2]

    if (Date.parseExact(fechaFin, dateFormat).addDays(-30).compareTo(Date.parseExact(fechaInicio, dateFormat)) <= 0)
    {
        return true;
    }
    else
    {
        return false;
    }
}

//Función que comprueba el rango máximo entre dos fecha que no sea superior a 365 días
//parametros de entrada las dos fechas a comparar y la cultura o formato de la fecha
function rangoFechasMaxAno(fechaI,fechaF,dateFormat)
{ 
    if(Date.parseExact(document.getElementById(fechaF).value,dateFormat).addDays(-365).compareTo(Date.parseExact(document.getElementById(fechaI).value,dateFormat))<=0)
    {
        return true;
    }
    else
    {
        return false;
    }
}

function camposObligatoriosCuentaCatastral(txtRegionID, txtManzanaID, txtLoteID, txtUnidadID) {

    var txtRegion = document.getElementById(txtRegionID);
    var txtmanzana = document.getElementById(txtManzanaID);
    var txtlote = document.getElementById(txtLoteID);
    var txtunidad = document.getElementById(txtUnidadID);

    if ((txtRegion.value == "") &&
       (txtmanzana.value == "") &&
       (txtlote.value == "") &&
       (txtunidad.value == "")) {
        return true;
    }
    else if ((txtRegion.value != "") &&
       (txtmanzana.value != "") &&
       (txtlote.value != "") &&
       (txtunidad.value != "")) {
    return true;
    }
    else {
        return false;
    }
}

//Cambio error 1925
//function AbrirUrlFut(urlFrame, urlFUT) {
//    ventana = window.open(urlFUT, "FUT", "location=0,status=0,scrollbars=1,width=640,height=900,resizable=1");
//}


