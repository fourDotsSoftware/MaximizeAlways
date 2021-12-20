using System;using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MaximizeAlways
{
    class Module
    {
        public static string ApplicationName = "Maximize Always";
        public static string Version = "1.3";

        public static string ShortApplicationTitle = ApplicationName + " V" + Version;
        public static string ApplicationTitle = ShortApplicationTitle + " - 4dots Software";

        public static string ProductWebpageURL = "https://www.4dots-software.com/maximizealways/";
        public static string BuyURL = "http://www.4dots-software.com/store/buy-maximizealways.php";


        public static void ShowMessage(string msg)
        {
            return;
            MessageBox.Show(msg,ApplicationTitle,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        public static void ShowError(Exception ex)
        {
            ShowError("Error", ex);
        }

        public static void ShowError(string msg)
        {
            return;
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public static void ShowError(string msg, Exception ex)
        {
            ShowError(msg + "\n\n" + ex.ToString()+ ex.Message);
        }

        public static bool DeleteApplicationSettingsFile()
        {
            try
            {
                string settingsFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Save();

                System.IO.FileInfo fi = new System.IO.FileInfo(settingsFile);
                fi.Attributes = System.IO.FileAttributes.Normal;
                fi.Delete();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
