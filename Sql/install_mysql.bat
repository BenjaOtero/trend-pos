@echo off
echo Installing MySQL Server. Please wait...
cd c:\
c:\mysql-essential-5.5.0-m2-win32.msi /quiet

echo Configurating MySQL Server...
cd C:\Program Files\MySQL\MySQL Server 5.5\bin\
mysqlinstanceconfig.exe -i -q ServiceName=MySQL Root Password=mypassword ServerType=DEVELOPER DatabaseType=INODB Port=myport Charset=utf8 

echo Installation was successfully

pause