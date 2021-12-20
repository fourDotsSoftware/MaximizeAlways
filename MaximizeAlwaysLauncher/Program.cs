using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MaximizeAlwaysLauncher
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

            string dir = Application.StartupPath;

            try
            {
                RegistryKey key = Registry.CurrentUser;
                
                key = key.OpenSubKey(@"Software\4dots Software\MaximizeAlways", false);
                
                bool adminrights = bool.Parse(key.GetValue("LaunchWithAdminRights", false).ToString());

                key = null;

                if (adminrights)
                {
                    string appfile = "MaximizeAlwaysAdminRights.exe";

                    System.Diagnostics.Process.Start("\""+System.IO.Path.Combine(dir, appfile)+"\"");
                }
                else
                {
                    string appfile = "MaximizeAlways.exe";

                    System.Diagnostics.Process.Start("\"" + System.IO.Path.Combine(dir, appfile) + "\"");
                }
            }
            catch
            {
                string appfile = "MaximizeAlways.exe";

                System.Diagnostics.Process.Start("\"" + System.IO.Path.Combine(dir, appfile) + "\"");
            }

            return;
            
        }
    }
}
