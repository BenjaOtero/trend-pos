using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;



namespace CustomActions
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        private void escribir(string ruta)
        {
            System.IO.StreamWriter sw = System.IO.File.CreateText("c:\\Windows\\Temp\\backup.bat"); // creo el archivo .bat
            sw.Close();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ruta);
            sb.AppendLine("ocote");
            using (StreamWriter outfile = new StreamWriter("c:\\Windows\\Temp\\backup.bat", true)) // escribo en el archivo .bat
            {
                outfile.Write(sb.ToString());
            }
        /*    Process process = new Process();
            process.StartInfo.FileName = "C:\\mysql-connector-net-6.5.4.msi /quiet";
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.EnableRaisingEvents = true;
            process.WaitForExit();
         //   process.Exited += new EventHandler(processBackupExited);*/
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            System.IO.StreamWriter sw = System.IO.File.CreateText("c:\\Windows\\Temp\\install_mysql.bat"); // creo el archivo .bat
            sw.Close();
            StringBuilder sb = new StringBuilder();
            string installMysql = this.Context.Parameters["destino"];// destino fue configurado en la propiedad 'CustomActionData: /destino="[SourceDir]\"' en propiedades en acciones personalizadas
            string unidad = installMysql.Substring(0, 1); //obtengo la letra de unidad de disco
            unidad = unidad + ":";
            sb.AppendLine(unidad);
            installMysql = installMysql.Substring(0, installMysql.Length - 1); //quito la ultima barra invertida (\)
            installMysql = installMysql + "Mysql";
            installMysql = "\"" + installMysql + "\""; // agrego comillas
            installMysql = "cd " + installMysql;
            sb.AppendLine(installMysql);
            string msi = "mysql-essential-5.5.0-m2-win32.msi /quiet";
            sb.AppendLine(msi);
            string configMysql = "cd " + "\"C:\\Program Files\\MySQL\\MySQL Server 5.5\\bin\"";
            sb.AppendLine(configMysql);
            configMysql = "mysqlinstanceconfig.exe -i -q ServiceName=MySQL root Password=8953#AFjn ServerType=DEVELOPER DatabaseType=INODB Port=myport Charset=utf8";
            sb.AppendLine(configMysql);
            string rutaDB = installMysql.Substring(0, installMysql.Length - 1);
            rutaDB = rutaDB.Substring(3, rutaDB.Length - 3);
            rutaDB = rutaDB + "\\pos.sql\"";
            string restaurarDB = "mysql.exe -u root < " + rutaDB;
            sb.AppendLine(restaurarDB);
            sb.AppendLine("pause");
            using (StreamWriter outfile = new StreamWriter("c:\\Windows\\Temp\\install_mysql.bat", true)) // escribo en el archivo .bat
            {
                outfile.Write(sb.ToString());
            }
            Process process = new Process();
            process.StartInfo.FileName = "c:\\Windows\\Temp\\install_mysql.bat";
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
            process.EnableRaisingEvents = true;
            process.WaitForExit();
          //  process.Exited += new EventHandler(processBackupExited);
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            // la variable ruta guarda el valor de la ruta donde está el setup.exe
         /*   string ruta = this.Context.Parameters["SrcDir"]; // SrcDir fue configurado en la propiedad 'CustomActionData: /SrcDir="[SourceDir]\"' en acciones personalizadas
            ruta = ruta.Substring(0, ruta.Length - 1);
            escribir(ruta);*/
        }
    }
}