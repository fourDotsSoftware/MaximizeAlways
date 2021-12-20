using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace MaximizeAlways
{
    public partial class frmMain : Form 
    {
        public string MainModuleName = "";

        public ArrayList AppsForTray = new ArrayList();
        public ArrayList AllAppInstancesToTray = new ArrayList();

        public WindowsPlacementHelper WindowsPlacementHelper = new WindowsPlacementHelper();
        public ArrayList NotifyIcons = new ArrayList();
        public ArrayList WindowHandles = new ArrayList();
        public ArrayList ProcNames = new ArrayList();
        public ArrayList ContextMenuStrips = new ArrayList();

        public WindowsManager WindowsManager = new WindowsManager();

        public bool AllMinimizedToTray = false;

        public static frmMain Instance = null;

        public frmMain()
        {
            InitializeComponent();
            Instance = this;
        }

        private void RefreshSettings()
        {
            RegistryKey key = Registry.CurrentUser;

            try
            {
                key = key.OpenSubKey(@"Software\4dots Software\MaximizeAlways", false);

                /*
                try
                {
                    this.notifyIcon1.Visible = !bool.Parse(key.GetValue("HideMaximizeAlways", false).ToString());
                }
                catch { }
                */

                try
                {
                    AllMinimizedToTray = bool.Parse(key.GetValue("AllAppsMaximized", false).ToString());
                }
                catch { }

                bool configured_once = bool.Parse(key.GetValue("ConfiguredOnce", false).ToString());

                if (!configured_once)
                {
                    Module.ShowMessage("At first, please configure which Applications should get maximized automatically when activated.\n\nYou can open the Configuration Screen again by right clicking on the MaximizeAlways Tray Icon and selecting Configure from the Menu.\n\n Alternatively, you can run the Maximize Always Config application.");
                    Cursor.Current = Cursors.WaitCursor;
                    System.Diagnostics.Process.Start("\""+Application.StartupPath + "\\MaximizeAlwaysConfig.exe\"");
                }

                key = key.OpenSubKey("Applications");

                string[] valn = key.GetValueNames();

                for (int k = 0; k < valn.Length; k++)
                {
                    if (!(key.GetValue(valn[k]).ToString()).StartsWith("Window Text "))
                    {
                        AppsForTray.Add(System.IO.Path.GetFileNameWithoutExtension(key.GetValue(valn[k]).ToString()));
                    }
                    else
                    {
                        AppsForTray.Add(key.GetValue(valn[k]).ToString());
                    }
                }

                

            }
            catch (Exception ex)
            {
                Module.ShowError(ex);
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }

                Cursor.Current = null;
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            Hide();
            this.Visible = false;

            RefreshSettings();            

            StartListeningForWindowChanges();
        }

        private bool InLaunch = false;

        private void launchToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (InLaunch) return;

            try
            {
                InLaunch = true;
                IntPtr handle = IntPtr.Zero;

                NotifyIcon n = null;

                if (sender is NotifyIcon)
                {
                    n = (NotifyIcon)sender;
                    handle = (IntPtr)n.Tag;
                }
                else if (sender is ToolStripMenuItem)
                {
                    ToolStripMenuItem ti = (ToolStripMenuItem)sender;
                    handle = (IntPtr)ti.Tag;                    

                }

                int ind = WindowHandles.IndexOf(handle);
                WindowsManager.ShowWindow((IntPtr)WindowHandles[ind]);

                NotifyIcon notf = (NotifyIcon)NotifyIcons[ind];
                ContextMenuStrip cms = (ContextMenuStrip)ContextMenuStrips[ind];

                ProcNames.RemoveAt(ind);
                NotifyIcons.RemoveAt(ind);
                ContextMenuStrips.RemoveAt(ind);
                WindowHandles.RemoveAt(ind);

                if (cms != null)
                {
                    try
                    {
                        cms.Dispose();
                    }
                    catch { }
                }

                if (notf != null)
                {
                    try
                    {
                        notf.Visible = false;

                        if (notf.Icon != null)
                        {
                            notf.Icon.Dispose();
                        }
                        notf.Dispose();
                        
                    }
                    catch { }
                }

                if (n != null)
                {
                    try
                    {
                        n.Visible = false;

                        if (n.Icon != null)
                        {
                            n.Icon.Dispose();
                        }
                        n.Dispose();

                    }
                    catch { }
                }                
            }
            finally
            {
                InLaunch = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            int ind = ContextMenuStrips.IndexOf(ts.Owner);

            Process[] procs = Process.GetProcessesByName(ProcNames[ind].ToString());
            if (procs.Length > 0)
            {
                for (int k = 0; k < procs.Length; k++)
                {
                    procs[k].Kill();
                }
            }

            Application.Exit();
            
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public bool InTimer1_Tick = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            return;

            if (InTimer1_Tick) return;

            try
            {

                InTimer1_Tick = true;

                if (AllMinimizedToTray)
                {
                    Process[] procs = System.Diagnostics.Process.GetProcesses();

                    for (int k = 0; k < procs.Length; k++)
                    {
                        if (procs[k].MainWindowHandle == IntPtr.Zero) continue;

                        try
                        {
                            if (procs[k].ProcessName.ToLower().IndexOf("maximizealways") >= 0)
                                continue;
                        }
                        catch { }
                        try
                        {
                            IntPtr mainwnd = IntPtr.Zero;
                            try
                            {
                                mainwnd = procs[k].MainWindowHandle;
                            }
                            catch { }

                            if (mainwnd != IntPtr.Zero && WindowsPlacementHelper.WindowIsMinimized(mainwnd))
                            {
                                /*
                                if (WindowHandles.IndexOf(procs[k].MainWindowHandle) < 0)
                                {
                                
                                    try
                                    {
                                        string procfn = procs[k].ProcessName;
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                    //1NotifyIcon notf = new NotifyIcon(this.components);
                                    NotifyIcon notf = new NotifyIcon();
                                    try
                                    {
                                        //3notf.Icon = WindowsManager.GetApplicationIcon(procs[k].MainModule.FileName);

                                        notf.Icon = WindowsManager.GetApplicationIcon(ProcessFilenameRetriever.GetExecutablePath(procs[k]));
                                    }
                                    catch 
                                    {
                                    
                                    }

                                    notf.Visible = true;

                                    ContextMenuStrip cms = new ContextMenuStrip();
                                    ToolStripMenuItem tm1 = new ToolStripMenuItem("Maximize");
                                    tm1.Tag = procs[k].MainWindowHandle;

                                    tm1.Click += new EventHandler(launchToolStripMenuItem_Click);

                                    //notf.DoubleClick += new EventHandler(launchToolStripMenuItem_Click);
                                    notf.Click += new EventHandler(launchToolStripMenuItem_Click);
                                    ToolStripMenuItem tm2 = new ToolStripMenuItem("Exit");
                                    tm2.Click += new EventHandler(exitToolStripMenuItem_Click);
                                    cms.Items.Add(tm1);
                                    cms.Items.Add(tm2);

                                    notf.ContextMenuStrip = cms;
                                    notf.Tag = procs[k].MainWindowHandle;

                                    NotifyIcons.Add(notf);
                                    //3ProcNames.Add(System.IO.Path.GetFileNameWithoutExtension(procs[k].MainModule.FileName));

                                    ProcNames.Add(System.IO.Path.GetFileNameWithoutExtension(ProcessFilenameRetriever.GetExecutablePath(procs[k])));
                                    WindowHandles.Add(procs[k].MainWindowHandle);
                                    ContextMenuStrips.Add(cms);

                                    StringBuilder strbTitle = new StringBuilder(255);
                                    int nLength = WindowsManager.GetWindowText(procs[k].MainWindowHandle, strbTitle, strbTitle.Capacity + 1);
                                    string strTitle = strbTitle.ToString();

                                    if (strTitle.Length > 63)
                                    {
                                        strTitle = strTitle.Substring(0, 63);
                                    }
                                    notf.Text = strTitle;

                                    WindowsManager.HideWindow(procs[k].MainWindowHandle);
                                }*/

                                WindowsManager.HideWindow(procs[k].MainWindowHandle);
                            }

                        }
                        catch { }


                    }
                }
                else
                {
                    //1Process[] procs = System.Diagnostics.Process.GetProcessesByName(AppsForTray[j].ToString());

                    Process[] procs = System.Diagnostics.Process.GetProcesses();

                    for (int k = 0; k < procs.Length; k++)
                    {
                        if (procs[k].MainWindowHandle == IntPtr.Zero) continue;
                        try
                        {
                            int j = -1;

                            for (int a = 0; a < AppsForTray.Count; a++)
                            {
                                if (AppsForTray[a].ToString().ToLower() == procs[k].ProcessName.ToLower())
                                {
                                    j = a;
                                    break;
                                }
                            }

                            //3int j = AppsForTray.IndexOf(procs[k].ProcessName);

                            if (j == -1) continue;

                            if (WindowsPlacementHelper.WindowIsMinimized(procs[k].MainWindowHandle))
                            {
                                WindowsManager.HideWindow(procs[k].MainWindowHandle);

                                /*
                                if (WindowHandles.IndexOf(procs[k].MainWindowHandle) < 0)
                                {
                                    try 
                                    {
                                        string procfn = procs[k].ProcessName;
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                    //1NotifyIcon notf = new NotifyIcon(this.components);
                                    NotifyIcon notf = new NotifyIcon();

                                    try
                                    {
                                        //3notf.Icon = WindowsManager.GetApplicationIcon(procs[k].MainModule.FileName);

                                        notf.Icon = WindowsManager.GetApplicationIcon(ProcessFilenameRetriever.GetExecutablePath(procs[k]));

                                    }
                                    catch
                                    {
                                    
                                    }

                                    notf.Visible = true;

                                    ContextMenuStrip cms = new ContextMenuStrip();
                                    ToolStripMenuItem tm1 = new ToolStripMenuItem("Maximize");
                                    tm1.Tag = procs[k].MainWindowHandle;

                                    tm1.Click += new EventHandler(launchToolStripMenuItem_Click);

                                    //notf.DoubleClick += new EventHandler(launchToolStripMenuItem_Click);
                                    notf.Click += new EventHandler(launchToolStripMenuItem_Click);
                                    ToolStripMenuItem tm2 = new ToolStripMenuItem("Exit");
                                    tm2.Click += new EventHandler(exitToolStripMenuItem_Click);
                                    cms.Items.Add(tm1);
                                    cms.Items.Add(tm2);

                                    notf.ContextMenuStrip = cms;
                                    notf.Tag = procs[k].MainWindowHandle;

                                    NotifyIcons.Add(notf);
                                    //3ProcNames.Add(AppsForTray[j].ToString());
                                    ProcNames.Add(procs[k].ProcessName);
                                    WindowHandles.Add(procs[k].MainWindowHandle);
                                    ContextMenuStrips.Add(cms);

                                    StringBuilder strbTitle = new StringBuilder(255);
                                    int nLength = WindowsManager.GetWindowText(procs[k].MainWindowHandle, strbTitle, strbTitle.Capacity + 1);
                                    string strTitle = strbTitle.ToString();

                                    if (strTitle.Length > 63)
                                    {
                                        strTitle = strTitle.Substring(0, 63);
                                    }
                                    notf.Text = strTitle;


                                    if (false)
                                    //if (procs[k].ProcessName == "sManager")
                                    {
                                        int ptr = MessageHelper.FindWindow(null, "Samsung Update");

                                        WindowsManager.HideWindow(new IntPtr(ptr));
                                    }
                                    else
                                    {
                                        WindowsManager.HideWindow(procs[k].MainWindowHandle);
                                    }
                                }*/
                            }

                        }
                        catch { }
                    }
                }
            }
            finally
            {
                InTimer1_Tick = false;
            }
        }

        private void cmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            cmiMinWindows.DropDownItems.Clear();

            Process[] procs = System.Diagnostics.Process.GetProcesses();

            ArrayList al = new ArrayList();

            for (int k = 0; k < procs.Length; k++)
            {
                if (procs[k].MainWindowHandle == IntPtr.Zero) continue;
                try
                {
                    //3string procname = System.IO.Path.GetFileNameWithoutExtension(procs[k].MainModule.FileName);

                    string procfi = ProcessFilenameRetriever.GetExecutablePath(procs[k]);
                    string procname = System.IO.Path.GetFileNameWithoutExtension(procfi);
                    string procexe = System.IO.Path.GetFileName(procfi);

                    if (al.IndexOf(procname) >= 0) continue;

                    al.Add(procname);                    

                    ToolStripMenuItem ti = new ToolStripMenuItem(procname);
                    ti.Tag = procname;
                    ti.Click += new EventHandler(MinWindow_Click);

                    int j = -1;

                    for (int a = 0; a < AppsForTray.Count; a++)
                    {
                        if (AppsForTray[a].ToString().ToLower() == procs[k].ProcessName.ToLower())
                        {
                            j = a;
                            break;
                        }
                    }

                    //3int j = AppsForTray.IndexOf(procs[k].ProcessName);                    

                    ti.Checked = (j != -1);

                    cmiMinWindows.DropDownItems.Add(ti);
                }
                catch { }
            }
        }

        void MinWindow_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem ti = (ToolStripMenuItem)sender;

                ti.Checked = !ti.Checked;

                string procname = ti.Tag.ToString().ToLower();

                Process[] procs = System.Diagnostics.Process.GetProcessesByName(procname);

                if (procs.Length == 0) return;

                string procexe=System.IO.Path.GetFileName(ProcessFilenameRetriever.GetExecutablePath(procs[0]));

                bool found = false;

                for (int a = 0; a < AppsForTray.Count; a++)
                {
                    if (AppsForTray[a].ToString().ToLower() == procname.ToLower())
                    {
                        found = true;

                        if (!ti.Checked)
                        {
                            AppsForTray.RemoveAt(a);
                        }

                        break;
                    }
                }

                //3
                /*
                if (AppsForTray.IndexOf(procname) < 0)
                {
                    AppsForTray.Add(procname);
                }
                */

                if (!found && ti.Checked)
                {
                    AppsForTray.Add(procname);
                }

                RegistryKey key = Registry.CurrentUser;

                RegistryKey key2 = key.OpenSubKey(@"Software\4dots Software\MaximizeAlways", true);

                RegistryKey key3 = key2.OpenSubKey("Applications", true);

                string[] valn = key3.GetValueNames();                

                for (int k = 0; k < valn.Length; k++)
                {                    
                    string keyval=key3.GetValue(valn[k].ToString()).ToString();

                    if ( keyval.ToLower() == procexe.ToLower())
                    {
                        key3.DeleteValue(valn[k]);
                    }
                }

                if (ti.Checked)
                {
                    key3.SetValue("Application #" + (valn.Length + 1).ToString(), procexe);
                }

                key.Close();
                key2.Close();
                key3.Close();

                for (int k = 0; k < procs.Length; k++)
                {
                    if (procs[k].MainWindowHandle == IntPtr.Zero) continue;

                    //WindowsManager.HideWindow(procs[k].MainWindowHandle);

                    //3WindowsManager.MinimizeWindow(procs[k].MainWindowHandle);

                    WindowsManager.MaximizeWindow(procs[k].MainWindowHandle);
                }

            }
            catch (Exception ex)
            {
                Module.ShowError(ex);
            }
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper.WM_COPYDATA)
            {
                MessageHelper.COPYDATASTRUCT mystr = new MessageHelper.COPYDATASTRUCT();

                Type mytype = mystr.GetType();
                mystr = (MessageHelper.COPYDATASTRUCT)m.GetLParam(mytype);

                string arg = mystr.lpData;

                if (arg == "RefreshApps")
                {
                    //MessageBox.Show("RECEIVED MSG !!","",MessageBoxButtons.OK,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
                    RefreshSettings();
                } 
            }              
            else
            {
                base.WndProc(ref m);
            }            
        }

        private void cmiOptions_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("\""+Application.StartupPath + "\\MaximizeAlwaysConfig.exe\"");
        }

        private void cmiMinAll_Click(object sender, EventArgs e)
        {                        
            Process[] procs = System.Diagnostics.Process.GetProcesses();

            for (int k = 0; k < procs.Length; k++)
            {
                if (procs[k].MainWindowHandle == IntPtr.Zero) continue;
                
                try
                {
                    //3WindowsManager.HideWindow(procs[k].MainWindowHandle);

                    WindowsManager.MinimizeWindow(procs[k].MainWindowHandle);

                    /*
                    //if (WindowsPlacementHelper.WindowIsMinimized(procs[k].MainWindowHandle))
                    //{
                        if (WindowHandles.IndexOf(procs[k].MainWindowHandle) < 0)
                        {
                            NotifyIcon notf = new NotifyIcon(this.components);
                            //3notf.Icon = WindowsManager.GetApplicationIcon(procs[k].MainModule.FileName);

                            notf.Icon = WindowsManager.GetApplicationIcon(ProcessFilenameRetriever.GetExecutablePath(procs[k]));

                            notf.Visible = true;

                            ContextMenuStrip cms = new ContextMenuStrip();
                            ToolStripMenuItem tm1 = new ToolStripMenuItem("Maximize");
                            tm1.Tag = procs[k].MainWindowHandle;

                            tm1.Click += new EventHandler(launchToolStripMenuItem_Click);

                            notf.Click += new EventHandler(launchToolStripMenuItem_Click);
                            //notf.DoubleClick += new EventHandler();
                            ToolStripMenuItem tm2 = new ToolStripMenuItem("Exit");
                            tm2.Click += new EventHandler(exitToolStripMenuItem_Click);
                            cms.Items.Add(tm1);
                            cms.Items.Add(tm2);

                            notf.ContextMenuStrip = cms;
                            notf.Tag = procs[k].MainWindowHandle;

                            NotifyIcons.Add(notf);
                            //3ProcNames.Add(System.IO.Path.GetFileNameWithoutExtension(procs[k].MainModule.FileName));

                            ProcNames.Add(System.IO.Path.GetFileNameWithoutExtension(ProcessFilenameRetriever.GetExecutablePath(procs[k])));

                            WindowHandles.Add(procs[k].MainWindowHandle);
                            ContextMenuStrips.Add(cms);

                            StringBuilder strbTitle = new StringBuilder(255);
                            int nLength = WindowsManager.GetWindowText(procs[k].MainWindowHandle, strbTitle, strbTitle.Capacity + 1);
                            string strTitle = strbTitle.ToString();

                            if (strTitle.Length > 63)
                            {
                                strTitle = strTitle.Substring(0, 63);
                            }
                            notf.Text = strTitle;

                            WindowsManager.HideWindow(procs[k].MainWindowHandle);
                        }
                    //}*/

                }
                catch { }
            }
        }

        public void cmiMaxAll_Click(object sender, EventArgs e)
        {
            Process[] procs = System.Diagnostics.Process.GetProcesses();

            for (int k = 0; k < procs.Length; k++)
            {
                if (procs[k].MainWindowHandle == IntPtr.Zero) continue;

                try
                {
                    //3WindowsManager.HideWindow(procs[k].MainWindowHandle);

                    WindowsManager.MaximizeWindow(procs[k].MainWindowHandle);
                }
                catch
                {

                }
            }
            /*
            IntPtr handle = IntPtr.Zero;

            int notcount = NotifyIcons.Count;

            for (int k = 0; k < notcount; k++)
            {                
                handle = (IntPtr)((NotifyIcon)NotifyIcons[0]).Tag;
                                
                int ind = 0;

                if (ind != -1)
                {
                    try
                    {
                        WindowsManager.ShowWindow((IntPtr)WindowHandles[ind]);
                    }
                    catch { }                                        

                    if (((ContextMenuStrip)ContextMenuStrips[ind]) != null)
                    {
                        try
                        {
                            ((ContextMenuStrip)ContextMenuStrips[ind]).Dispose();
                        }
                        catch { }
                    }                    

                    if (NotifyIcons[ind] != null)
                    {
                        try
                        {
                            ((NotifyIcon)NotifyIcons[ind]).Visible = false;

                            if (((NotifyIcon)NotifyIcons[ind]).Icon != null)
                            {
                                ((NotifyIcon)NotifyIcons[ind]).Icon.Dispose();
                            }

                            ((NotifyIcon)NotifyIcons[ind]).Dispose();

                        }
                        catch { }
                    }

                    ProcNames.RemoveAt(ind);
                    NotifyIcons.RemoveAt(ind);
                    ContextMenuStrips.RemoveAt(ind);
                    WindowHandles.RemoveAt(ind);                    
                }
            }*/
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\MaximizeAlways Help Manual.chm");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser;

            try
            {
                key = key.OpenSubKey(@"Software\4dots Software\MaximizeAlways", false);

                try
                {
                    this.notifyIcon1.Visible = !bool.Parse(key.GetValue("HideMaximizeAlways", false).ToString());
                }
                catch { }                

            }
            catch (Exception ex)
            {
                Module.ShowError(ex);
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
                Cursor.Current = null;
            }
        }        
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void cmiMinWindows_Click(object sender, EventArgs e)
        {

        }

        /*
        private void buyApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Module.BuyURL);
        }

        private void enterLicenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdeia f = new frmAdeia();
            f.ShowDialog();
        }*/

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread, uint dwflags);
        [DllImport("user32.dll")]
        internal static extern int UnhookWinEvent(IntPtr hWinEventHook);
        internal delegate void WinEventProc(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime);

        const uint WINEVENT_OUTOFCONTEXT = 0;
        
        const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;
        const uint EVENT_SYSTEM_SWITCHEND = 0x0015;
        const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
        const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
        const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;

        public const int OBJID_WINDOW = 0;


        const int GWL_EXSTYLE = -20;
        const int GWL_STYLE = -16;

        const long WS_EX_APPWINDOW = 0x00040000L;

        private IntPtr winHook;
        private WinEventProc listener;
        private IntPtr winHook2;
        private WinEventProc listener2;
        private IntPtr winHook3;
        private WinEventProc listener3;
        private IntPtr winHook4;
        private WinEventProc listener4;
        private IntPtr winHook5;
        private WinEventProc listener5;

        public void StartListeningForWindowChanges()
        {
            try
            {
                
                listener = new WinEventProc(EventCallback);
                //setting the window hook
                winHook = SetWinEventHook(EVENT_SYSTEM_MINIMIZEEND, EVENT_SYSTEM_MINIMIZEEND, IntPtr.Zero, listener, 0, 0, WINEVENT_OUTOFCONTEXT);

                listener2 = new WinEventProc(EventCallback);
                //setting the window hook
                winHook2 = SetWinEventHook(EVENT_SYSTEM_SWITCHEND, EVENT_SYSTEM_SWITCHEND, IntPtr.Zero, listener2, 0, 0, WINEVENT_OUTOFCONTEXT);

                listener3 = new WinEventProc(EventCallback);
                //setting the window hook
                winHook3 = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, listener3, 0, 0, WINEVENT_OUTOFCONTEXT);

                listener4 = new WinEventProc(EventCallback);
                //setting the window hook
                winHook4 = SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero, listener4, 0, 0, WINEVENT_OUTOFCONTEXT);
                   
                /*             
                listener5 = new WinEventProc(EventCallback);
                //setting the window hook
                winHook5 = SetWinEventHook(EVENT_OBJECT_LOCATIONCHANGE, EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, listener5, 0, 0, WINEVENT_OUTOFCONTEXT);
                */
            }
            catch { }
        }

        public void StopListeningForWindowChanges()
        {
            
            try
            {
                UnhookWinEvent(winHook);
            }
            catch{}

            try {
                UnhookWinEvent(winHook2);
            }
            catch { }

            try
            {
                UnhookWinEvent(winHook3);
            }
            catch { }

            try
            {
                UnhookWinEvent(winHook4);
            }
            catch { }
            
            /*
            try
            {
                UnhookWinEvent(winHook5);
            }
            catch { }
            */
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern long GetWindowLongPtr(IntPtr hWnd, int nIndex);

        public enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        const long WS_CHILD = 0x40000000;
        const long WS_EX_TOOLWINDOW = 0x00000080;
        const long WS_EX_TOPMOST = 0x00000008;
        const long WS_EX_WINDOWEDGE = 0x00000100;

        const long WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        const int GW_OWNER = 4;
        const int GW_HWNDFIRST = 0;
        const int GW_CHILD = 5;

        const long WS_MAXIMIZEBOX = 0x00010000L;
        const long WS_MINIMIZEBOX = 0x00020000L;
        const long WS_THICKFRAME = 0x00040000L;

        const long WS_POPUP = 0x80000000L;
        const long WS_TILED = 0x00000000L;
        const long WS_BORDER = 0x00800000L;
        //const long WS_POPUP = 0x80000000L;


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        enum GetAncestorFlags
        {
            /// <summary>
            /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
            /// </summary>
            GetParent = 1,
            /// <summary>
            /// Retrieves the root window by walking the chain of parent windows.
            /// </summary>
            GetRoot = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
            /// </summary>
            GetRootOwner = 3
        }


        private static void EventCallback(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime)
        {
            /*
            bool is_app = (GetWindowLongPtr(hWnd, GWL_EXSTYLE) & WS_EX_APPWINDOW) == WS_EX_APPWINDOW;

            if (!is_app) return;
            */

            //if (idObject != OBJID_WINDOW) return;            
                                    
            /*
            bool is_child = (GetWindowLongPtr(hWnd, GWL_STYLE) & WS_CHILD) == 1;

            if (is_child) return;
            */

            //if (!hWnd.Equals(GetAncestor(hWnd,GetAncestorFlags.GetRoot)))

            
            bool is_ancestor = hWnd == (GetAncestor(hWnd, GetAncestorFlags.GetRootOwner));

            if (!is_ancestor) return;            
            
            //if (GetWindowLong(hWnd, GWL_EXSTYLE) & WS_EX_TOOLWINDOW) == 0)

            /*
            if ((GetWindowLongPtr(hWnd, GWL_EXSTYLE) & WS_EX_PALETTEWINDOW) != 0)
            {
                return;
            }
            */

            //if (!(GetParent(hWnd) == IntPtr.Zero)) return;

            //3if (!((int)(GetWindow(hWnd,GW_OWNER)) == 0)) return;

            //3int  iptr = (int)GetWindow(hWnd, GW_CHILD);
            /*
            int iptr = (int)GetWindow(hWnd, GW_OWNER);

            if (iptr != 0) return;
            */

            try
            {
                //frmMain.Instance.timer1_Tick(null, null);
                frmMain.Instance.MaximizeAppWindow(hWnd);
                
            }
            catch { }
            finally
            {

            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        private void MaximizeAppWindow(IntPtr hWnd)
        {
            if (AllMinimizedToTray)
            {
                Process p = ProcessFilenameRetriever.GetProcessByHandle(hWnd);

                if (hWnd != p.MainWindowHandle) return;

                string pf = ProcessFilenameRetriever.GetExecutablePath(p);

                if (!pf.ToLower().Contains("maximizealways"))
                {
                    if (WindowsPlacementHelper.WindowIsMinimized(hWnd) && IsWindowVisible(hWnd))
                    {
                        WindowsManager.MaximizeWindow(hWnd);
                    }
                }
            }
            else
            {
                if (WindowsPlacementHelper.WindowIsMinimized(hWnd) && IsWindowVisible(hWnd))
                {
                    Process p = ProcessFilenameRetriever.GetProcessByHandle(hWnd);

                    if (hWnd != p.MainWindowHandle) return;

                    // Allocate correct string length first
                    int length = GetWindowTextLength(hWnd);
                    StringBuilder sb = new StringBuilder(length + 1);
                    GetWindowText(hWnd, sb, sb.Capacity);
                    string wndtxt = sb.ToString();

                    string pf = ProcessFilenameRetriever.GetExecutablePath(p);

                    for (int k = 0; k < AppsForTray.Count; k++)
                    {
                        if (AppsForTray[k].ToString().StartsWith("Window Text"))
                        {
                            string apptxt = AppsForTray[k].ToString();

                            if (apptxt.StartsWith("Window Text - RegExp : "))
                            {
                                apptxt = apptxt.Substring("Window Text - RegExp : ".Length);

                                try
                                {
                                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(apptxt);

                                    if (regex.IsMatch(wndtxt))
                                    {
                                        WindowsManager.MaximizeWindow(hWnd);
                                        break;
                                    }
                                }
                                catch { }
                            }
                            else if (apptxt.StartsWith("Window Text - Wildcards : "))
                            {
                                apptxt = apptxt.Substring("Window Text - Wildcards : ".Length);

                                try
                                {
                                    apptxt = System.Text.RegularExpressions.Regex.Escape(apptxt).Replace(@"\\\ ", " ");

                                    apptxt = apptxt.Replace(@"\?", ".{1}").Replace(@"\*", ".*?\\b");
                                    

                                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(apptxt);

                                    if (regex.IsMatch(wndtxt))
                                    {
                                        WindowsManager.MaximizeWindow(hWnd);
                                        break;
                                    }
                                }
                                catch { }
                            }
                            else if (apptxt.StartsWith("Window Text : "))
                            {
                                apptxt = apptxt.Substring("Window Text : ".Length);

                                if (wndtxt.Contains(apptxt))
                                {
                                    WindowsManager.MaximizeWindow(hWnd);
                                    break;
                                }
                            }


                        }
                        else if (AppsForTray[k].ToString().ToLower() == System.IO.Path.GetFileNameWithoutExtension(pf).ToLower())
                        {
                            if (WindowsPlacementHelper.WindowIsMinimized(hWnd))
                            {
                                WindowsManager.MaximizeWindow(p.MainWindowHandle);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListeningForWindowChanges();
        }        

    }    
}