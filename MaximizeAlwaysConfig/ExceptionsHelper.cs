using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MaximizeAlwaysConfig
{
    class ExceptionsHelper
    {
        public static void AddUnhandledExceptionHandlers()
        {            
            AppDomain.CurrentDomain.UnhandledException +=
                new System.UnhandledExceptionEventHandler(myExceptionHandler);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(
                myThreadExceptionHandler);


        }

        private static void myExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Module.ShowError("Unspecified Error", ex);
        }
        private static void myThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Module.ShowError("Unspecified Error", e.Exception);            
        }

    }
}
