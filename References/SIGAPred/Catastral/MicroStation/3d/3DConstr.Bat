REM #############################################
REM				Visualizacion 3D 
REM #############################################

REM ----------------------------------------------------
REM Ubicacion de aplicaciones
REM ----------------------------------------------------

SET APP_PATH=%programfiles%\SIGAPred\GraphicalTools
SET V8ust_path=%programfiles%\Bentley\MicroStation
SET V8ust_file="%programfiles%\Bentley\MicroStation\ustation.exe"

REM ----------------------------------------------------
REM Ubicacion de los ficheros a procesar
REM ----------------------------------------------------

SET fileDGN_IN=%1
SET fileDGN_IN_AUX=%1.aux


SET EXPORT_TODGN=%2
SET EXPORT_TOKML=%3

SET pathOUT=%~d1%~p13D
SET fileDGN="%pathOUT%\%~n1%~x1"

SET pathOUT_TEMP=%~d1%~p1TEMP
SET fileDGN_TEMP="%pathOUT_TEMP%\%~n1%~x1"
REM ----------------------------------------------------
REM Ficheros de configuracion
REM ----------------------------------------------------

SET path_CFG=%APP_PATH%\cfg

REM ----------------------------------------------------
REM Proceso
REM ----------------------------------------------------

REM Crear carpeta de salida
mkdir "%pathOUT%"
mkdir "%pathOUT_TEMP%"

del "%pathOUT%\*.*" /S /F /Q
del "%pathOUT_TEMP%\*.*" /S /F /Q

REM Convertir fichero a 3D
copy %fileDGN_IN% %fileDGN_IN_AUX%
%V8ust_file% -wacnvdgn -iinname:%fileDGN_IN% -ioutname:%fileDGN%

REM Generar construcciones en 3D
"%V8ust_path%\runwait" %V8ust_file% -waMCDelete -i%fileDGN% -iLEVEL:1-3 -wuMCMante

"%V8ust_path%\runwait" %V8ust_file% -waMCGenCons3D -i%fileDGN% -iECF:$(MC_PATH_ECF_FILE) -iHEIGHT:3 -iSMB:1,8,0,0,0 -i/TRANSP:50,20 -wuMCMante

REM Borrar lo que sobra
"%V8ust_path%\runwait" %V8ust_file% -waMCDelete -i%fileDGN% -iLEVEL:2-5,8-63 -wuMCMante

REM Hasta aqui, proceso comun: dgn 3D en Lambert 

REM ----------------------------------------------------
REM						PROCESO KML
REM ----------------------------------------------------

if %EXPORT_TOKML%==1 goto TOKML
goto TOKML_NEXT

REM ----------------------------------------------------
:TOKML


copy %fileDGN% %fileDGN_TEMP%

REM Cambio Sistema de referencia y Export a GoogleEarth
SET MC_BAT=MCShellKeyin
SET MS_NOSPLASH=1
REM "%V8ust_path%\runwait" %V8ust_file% -i%fileDGN_TEMP% -iTXT:"%path_CFG%\3DExportGE.txt" -iP1:"%pathOUT%\\" -wuMCMante_BAT
%V8ust_file% -i%fileDGN_TEMP% -iTXT:"%path_CFG%\3DExportGE.txt" -iP1:"%pathOUT%\\" -wuMCMante_BAT

REM Borrar temporal 

del "%pathOUT_TEMP%\*.*" /S /F /Q
rmdir "%pathOUT_TEMP%"

REM ----------------------------------------------------
REM 
REM ----------------------------------------------------

:TOKML_NEXT

REM ----------------------------------------------------
REM						PROCESO DGN
REM ----------------------------------------------------

if %EXPORT_TODGN%==1 goto TODGN
goto TODGN_NEXT

REM ----------------------------------------------------
:TODGN

REM Ajustar Vista
"%V8ust_path%\runwait" %V8ust_file% -waMCSaveSettings -i%fileDGN% -i/3D:ISO -i/VIEWTRANSP -wuMCMante

REM Abrir Salida
SET MS_NOSPLASH=1
%V8ust_file% %fileDGN% -wiMC3D -wuuntitled

REM ----------------------------------------------------
REM 
REM ----------------------------------------------------

:TODGN_NEXT

:FINAL

