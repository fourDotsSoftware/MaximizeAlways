using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace MaximizeAlways
{
    class TranslateHelper
    {
        private static System.Resources.ResourceManager rm = null;
        private static System.Resources.ResourceManager rm2 = null;

        public static string Translate(string str)
        {
            if (str == "Αλλαγή στοιχείων εικόνας")
            {
                return "100000";
            }            

            if (rm == null)
            {
                TranslateHelper cm = new TranslateHelper();
                rm = new System.Resources.ResourceManager("MaximizeAlways.ResTranslate", cm.GetType().Assembly);
                

            }

            if (rm2 == null)
            {
                TranslateHelper cm = new TranslateHelper();                
                rm2 = new System.Resources.ResourceManager("MaximizeAlways.ResRegister", cm.GetType().Assembly);
            }

            try
            {
                string trnstr = "";

                //if (System.Threading.Thread.CurrentThread.CurrentUICulture.ToString() == "en-US")
                if (true)
                {
                    trnstr = rm.GetString(str, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    trnstr = rm.GetString(str);
                }

                if (trnstr == null || trnstr == "")
                {
                    //if (System.Threading.Thread.CurrentThread.CurrentUICulture.ToString() == "en-US")
                    if (true)
                    {
                        trnstr = rm2.GetString(str, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        trnstr = rm2.GetString(str);
                    }

                    if (trnstr == null || trnstr == "")
                    {
                        trnstr = rm.GetString(str, System.Globalization.CultureInfo.InvariantCulture);

                        if (trnstr == null)
                        {
                            trnstr = rm2.GetString(str, System.Globalization.CultureInfo.InvariantCulture);

                            if (trnstr == null)
                            {
                                return str;
                            }
                            else
                            {
                                return trnstr;
                            }
                        }
                        else
                        {
                            return trnstr;
                        }
                    }
                    else
                    {
                        return trnstr;
                    }
                }
                else
                {
                    return trnstr;
                }
            }
            catch
            {
                return str;
            }
        }
    }
}
