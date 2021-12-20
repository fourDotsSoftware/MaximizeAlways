using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MaximizeAlwaysConfig
{
    class Module
    {
        public static string ApplicationTitle = "Maximize Always V1.3";

        public static string WebpageURL = "https://www.4dots-software.com/maximizealways/";

        public static void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        public static void ShowError(Exception ex)
        {
            ShowError("Error", ex);
        }

        public static void ShowError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


        public static void ShowError(string msg, Exception ex)
        {
            ShowError(msg + "\n\n" + ex.Message);
        }

    }

    public class TranslateHelper
    {
        public static string Translate(string str)
        {
            return str;
        }
    }
}
