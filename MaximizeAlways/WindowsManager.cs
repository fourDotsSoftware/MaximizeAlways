using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;


namespace MaximizeAlways
{
    public class WindowsManager
    {
        public static string[] args = null;
        //Import the SetForeground API to activate it
        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle,
           uint dwProcessId);
        [DllImport("Psapi.dll", SetLastError = true)]
        [PreserveSig]
        public static extern uint GetModuleFileNameEx([In]IntPtr hProcess, [In] IntPtr hModule, [Out] StringBuilder lpFilename,
            [In][MarshalAs(UnmanagedType.U4)]int nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);


        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        /// <summary>Shows a Window</summary>
        /// <remarks>
        /// <para>To perform certain special effects when showing or hiding a
        /// window, use AnimateWindow.</para>
        ///<para>The first time an application calls ShowWindow, it should use
        ///the WinMain function's nCmdShow parameter as its nCmdShow parameter.
        ///Subsequent calls to ShowWindow must use one of the values in the
        ///given list, instead of the one specified by the WinMain function's
        ///nCmdShow parameter.</para>
        ///<para>As noted in the discussion of the nCmdShow parameter, the
        ///nCmdShow value is ignored in the first call to ShowWindow if the
        ///program that launched the application specifies startup information
        ///in the structure. In this case, ShowWindow uses the information
        ///specified in the STARTUPINFO structure to show the window. On
        ///subsequent calls, the application must call ShowWindow with nCmdShow
        ///set to SW_SHOWDEFAULT to use the startup information provided by the
        ///program that launched the application. This behavior is designed for
        ///the following situations: </para>
        ///<list type="">
        ///    <item>Applications create their main window by calling CreateWindow
        ///    with the WS_VISIBLE flag set. </item>
        ///    <item>Applications create their main window by calling CreateWindow
        ///    with the WS_VISIBLE flag cleared, and later call ShowWindow with the
        ///    SW_SHOW flag set to make it visible.</item>
        ///</list></remarks>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="nCmdShow">Specifies how the window is to be shown.
        /// This parameter is ignored the first time an application calls
        /// ShowWindow, if the program that launched the application provides a
        /// STARTUPINFO structure. Otherwise, the first time ShowWindow is called,
        /// the value should be the value obtained by the WinMain function in its
        /// nCmdShow parameter. In subsequent calls, this parameter can be one of
        /// the WindowShowStyle members.</param>
        /// <returns>
        /// If the window was previously visible, the return value is nonzero.
        /// If the window was previously hidden, the return value is zero.
        /// </returns>
        [DllImport("user32.dll")]
        //private static extern bool ShowWindow(int hWnd, WindowShowStyle nCmdShow);
        private static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);
        

        

        /// <summary>
        /// return windows text
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpWindowText"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetWindowText",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;
        public const uint SHGFI_SMALLICON = 0x1;

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        IntPtr hImgSmall;
        SHFILEINFO shinfo = new SHFILEINFO();

        /// <summary>Enumeration of the different ways of showing a window using
        /// ShowWindow</summary>
        private enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized
            /// or maximized, the system restores it to its original size and
            /// position. An application should specify this flag when displaying
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position.
            /// This value is similar to "ShowNormal", except the window is not
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is
            /// minimized or maximized, the system restores it to its original size
            /// and position. An application should specify this flag when restoring
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
            /// that owns the window is hung. This flag should only be used when
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// See MSDN WINDOWPLACEMENT Structure http://msdn.microsoft.com/en-us/library/ms632611(v=VS.85).aspx
        /// </summary>
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public RECT rcNormalPosition;
        }

        //3public static Dictionary<IntPtr, RECT> dictWindowRects = new Dictionary<IntPtr, RECT>();

        public static Dictionary<IntPtr, IntPtr> dictWindowParents = new Dictionary<IntPtr, IntPtr>();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        private static frmAbout fa = null;

        public static void HideWindow(IntPtr handle)
        {
            //3ShowWindow(handle, WindowShowStyle.Hide);

            MaximizeWindow(handle);

            /*
            if (fa == null)
            {
                fa=new frmAbout();
            }

            IntPtr parent = GetParent(handle);

            dictWindowParents.Add(handle, parent);

            SetParent(handle, fa.Handle);
            */

            //System.Threading.Thread.Sleep(5000);


            //3ShowWindow(handle.ToInt32(), WindowShowStyle.Hide);
            /*
            //RECT rect=new RECT();

            //GetWindowRect(handle, ref rect);

            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = System.Runtime.InteropServices.Marshal.SizeOf(placement);
            GetWindowPlacement(handle, ref placement);
            RECT rect = placement.rcNormalPosition;
            
            MoveWindow(handle, 0, -15000, 0, 0, true);

            dictWindowRects.Add(handle, rect);*/
        }

        public static void MinimizeWindow(IntPtr handle)
        {
            //ShowWindow(handle.ToInt32(), WindowShowStyle.Minimize);

            ShowWindow(handle, WindowShowStyle.Minimize);
        }


        const int SW_SHOWMAXIMIZED = 3;

        private const int SW_MAXIMIZE = 3;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void MaximizeWindow(IntPtr handle)
        {
            //ShowWindow(handle.ToInt32(), WindowShowStyle.Minimize);

            //ShowWindow(handle, WindowShowStyle.Maximize);
        
            /*
             WINDOWPLACEMENT param = new WINDOWPLACEMENT();

            param.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

            param.showCmd=SW_SHOWMAXIMIZED;

            SetWindowPlacement(handle, ref param);*/            

            ShowWindow(handle, SW_MAXIMIZE);
        }

        public static void ShowWindow(IntPtr handle)
        {            
            ShowWindow(handle, WindowShowStyle.Restore);
            SetForegroundWindowNative(handle);

            /*
            IntPtr parent;            

            if (dictWindowParents.TryGetValue(handle, out parent))
            {
                SetParent(handle, parent);

                //3MoveWindow(handle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, true);
                //ShowWindow(handle.ToInt32(), WindowShowStyle.Restore);

                ShowWindow(handle, WindowShowStyle.Restore);
                SetForegroundWindowNative(handle);

                dictWindowParents.Remove(handle);

                //System.Threading.Thread.Sleep(5000);
            }*/           
        }

        public Icon GetApplicationIcon(string file)
        {
            hImgSmall = SHGetFileInfo(file, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
            Icon ico = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            return ico;
        }

        public string GetWindowFilePathFromHandle(IntPtr hwnd)
        {
            uint dwProcessId;
            GetWindowThreadProcessId(hwnd, out dwProcessId);
            IntPtr hProcess = OpenProcess(ProcessAccessFlags.VMRead | ProcessAccessFlags.QueryInformation, false, dwProcessId);
            StringBuilder path = new StringBuilder(1024);
            GetModuleFileNameEx(hProcess, IntPtr.Zero, path, 1024);
            CloseHandle(hProcess);
            return path.ToString();
        }

        public Icon GetWindowIconFromHandle(IntPtr hwnd)
        {
            return GetApplicationIcon(GetWindowFilePathFromHandle(hwnd));
        }

    }

}
