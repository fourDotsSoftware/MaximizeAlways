using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace MaximizeAlwaysConfig
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

            ActivateIfApplicationIsRunning("MaximizeAlwaysConfig");

            Application.Run(new frmMain());
            return;            
        }

        private static void ActivateIfApplicationIsRunning(string applicationName)
        {
            try
            {
                Process[] procs = System.Diagnostics.Process.GetProcessesByName(applicationName);

                int ialive = -1;
                for (int k = 0; k <= procs.GetUpperBound(0); k++)
                {
                    if (System.Diagnostics.Process.GetCurrentProcess().Id != procs[k].Id)
                    {
                        if (!procs[k].HasExited)
                        {
                            ialive = k;
                            break;
                        }
                    }
                }

                if (ialive != -1)
                {
                    MessageHelper2 msg = new MessageHelper2();
                    int result = 0;
                    //First param can be null

                    IntPtr hWnd = procs[ialive].MainWindowHandle;

                    while (hWnd == IntPtr.Zero)
                    {
                        Application.DoEvents();

                        MessageHelper2.PostMessage((IntPtr)MessageHelper2.HWND_BROADCAST, (UInt32)MessageHelper2.WM_ACTIVATEAPP, IntPtr.Zero, IntPtr.Zero);

                        procs = System.Diagnostics.Process.GetProcessesByName(applicationName);

                        ialive = -1;
                        for (int k = 0; k <= procs.GetUpperBound(0); k++)
                        {
                            if (System.Diagnostics.Process.GetCurrentProcess().Id != procs[k].Id)
                            {
                                if (!procs[k].HasExited)
                                {
                                    ialive = k;
                                    break;
                                }
                            }
                        }

                        if (ialive != -1)
                        {
                            hWnd = procs[ialive].MainWindowHandle;
                        }
                    }

                    msg.bringAppToFront(hWnd);

                    result = msg.sendWindowsStringMessage(hWnd, IntPtr.Zero, "SHOW");

                    Environment.Exit(0);
                    return;
                }
            }
            catch { }
        }
    }
}