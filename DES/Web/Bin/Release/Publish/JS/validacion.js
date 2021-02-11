// Comprueba si las claves rfc, curp e ife para una persona son correctas.
function ComprobarClaves(rfc, curp, ife, idDocIdentif, docIdentif, fechaNacimiento, fechaDefuncion, email, tipoPersona) {
   
    var correcto = false;
    var brfc;
    var bcupr;
    var bife;
    var bfechanacimiento;
    var bfechadefuncion;
    var bemail;

    var error = "";
    var expregRFC = "[A-Z]{3,4}[0-9]{6}[A-Z0-9]{3}";
    var expregCURP = "[A-Z]{4}[0-9]{6}[HM]{1}[A-Z]{5}[A-Z0-9]{1}[0-9]{1}";
    var expregIFE = "[A-Z]{6}[0-9]{8}[HM]{1}[0-9]{1}[A-Z0-9]{2}";
    var expregFECHA = "([0][1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}";
    var expregEMAIL = "([0-9a-zA-Z]+(?:[._][0-9a-zA-Z]+)*)@([0-9a-zA-Z]+(?:[._-][0-9a-zA-Z]+)*\.[0-9a-zA-Z]{2,3})";

    if (tipoPersona == "F") {
        brfc = rfc.match(expregRFC) != null;
        bcurp = curp.match(expregCURP) != null;
        bife = ife.match(expregIFE) != null;
        bfechanacimiento = fechaNacimiento.match(expregFECHA) != null;
        bfechadefuncion = fechaDefuncion.match(expregFECHA) != null;
        bemail = email.match(expregEMAIL) != null;

        if (rfc.length > 0 && !brfc)
            error = "El RFC debe contener 3 o 4 letras mayúsculas, 6 número y 3 caracteres alfanúmericos";

        if (curp.length > 0 && !bcurp) {
            if (error.length > 0)
                error = error + "\nEl curp debe contener 4 letras mayúsculas, 6 números, H o M, 5 letras mayúsculas, 1 alfanumérico y 1 número";
            else
                error = "El curp debe contener 4 letras mayúsculas, 6 números, H o M, 5 letras mayúsculas, 1 alfanumérico y 1 número";
        }
        if (ife.length > 0 && !bife) {
            if (error.length > 0)
                error = error + "\nLa clave ife debe contener 6 letras mayúsculas, 8 números, H o M, 1 número y 2 afanuméricos";
            else
                error = "La clave ife debe contener 6 letras mayúsculas, 8 números, H o M, 1 número y 2 afanuméricos";
        }
        if (((idDocIdentif != '0') && (docIdentif.length == 0)) || ((idDocIdentif == '0') && (docIdentif.length != 0))) {
            if (error.length > 0)
                error = error + "\nEn caso de introducir otro documento identificativo, has de rellenar los dos campos";
            else
                error = "En caso de introducir otro documento identificativo, has de rellenar los dos campos";
        }
        if (fechaNacimiento.length > 0 && !bfechanacimiento) {
            if (error.length > 0)
                error = error + "\nLa fecha de nacimiento no tiene un formato adecuado (dd/mm/aaaa)";
            else
                error = "La fecha de nacimiento no tiene un formato adecuado (dd/mm/aaaa)";
        }
        if (fechaDefuncion.length > 0 && !bfechadefuncion) {
            if (error.length > 0)
                error = error + "\nLa fecha de defunción no tiene un formato adecuado (dd/mm/aaaa)";
            else
                error = "La fecha de defunción no tiene un formato adecuado (dd/mm/aaaa)";
        }
        if (email.length > 0 && !bemail) {
            if (error.length > 0)
                error = error + "\nEl email no tiene un formato correcto";
            else
                error = "El email no tiene un formato correcto";
        }

        correcto = ((rfc.length <= 0 || brfc) &&
                    (curp.length <= 0 || bcurp) &&
                    (ife.length <= 0 || bife) &&
                    (((idDocIdentif == '0') && (docIdentif.length == 0)) || ((idDocIdentif != '0') && (docIdentif.length > 0))) &&
                    (fechaNacimiento.length <= 0 || bfechanacimiento) &&
                    (fechaDefuncion.length <= 0 || bfechadefuncion) &&
                    (email.length <= 0 || bemail));
    }
    else {
        brfc = rfc.match(expregRFC) != null;
        if (rfc.length > 0 && !brfc)
            error = "El RFC debe contener 3 o 4 letras mayúsculas, 6 número y 3 caracteres alfanúmericos";
        correcto = (rfc.legth <= 0 || brfc);
    }

    if (error.length > 0) {
        document.getElementById('capaResumenError').style.display = '';
        document.all.txtResumenError.value = error;
    }
    else {
        document.getElementById("capaResumenError").style.display = 'none';
        document.all.txtResumenError.value = error;
    }
    return correcto;
}

