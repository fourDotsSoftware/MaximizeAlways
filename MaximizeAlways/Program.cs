using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace MaximizeAlways
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ExceptionsHelper.AddUnhandledExceptionHandlers();
            WindowsManager.args = args;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            if (args.Length > 0 && args[0].StartsWith("/uninstall"))
            {
                Module.DeleteApplicationSettingsFile();

                /*
                frmUninstallQuestionnaire fq = new frmUninstallQuestionnaire();
                fq.ShowDialog();
                */

                System.Diagnostics.Process.Start("https://www.4dots-software.com/support/bugfeature.php?uninstall=true&app=" + System.Web.HttpUtility.UrlEncode(Module.ShortApplicationTitle));

                return;
                Environment.Exit(0);
            }            

            // remove commments for paid version            
            
            frmMain f = new frmMain();            

            Application.Run(f);            

            //end            

            return;            
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            // restore everything on exit...
            if (frmMain.Instance != null)
            {
                //frmMain.Instance.cmiMaxAll_Click(null, null);
            }
        }        
    }
}