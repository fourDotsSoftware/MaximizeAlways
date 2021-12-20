using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Drawing;
using System.Collections;

namespace MaximizeAlways
{
    /// &lt;summary>
    /// EnumDesktopWindows Demo - shows the caption of all desktop windows.
    /// Authors: Svetlin Nakov, Martin Kulov
    /// Bulgarian Association of Software Developers - http://www.devbg.org/en/
    /// &lt;/summary>
    public class WindowsEnumeratorHelper
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        /// <summary>
        /// filter function
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        /*
        /// <summary>
        /// check if windows visible
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        */
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

        /// <summary>
        /// enumarator on all desktop windows
        /// </summary>
        /// <param name="hDesktop"></param>
        /// <param name="lpEnumCallbackFunction"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        public static IntPtr LastPPWindowIntPtr = IntPtr.Zero;

        /// <summary>
        /// entry point of the program
        /// </summary>
        public static ArrayList GetOpenWindows()
        {
            ArrayList alOpenWindows = new ArrayList();

            WindowsEnumeratorHelper.EnumDelegate filter = delegate(IntPtr hWnd, int lParam)
            {
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = WindowsEnumeratorHelper.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                string strTitle = strbTitle.ToString();

                if (WindowsEnumeratorHelper.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
                {
                    OpenWindow op = new OpenWindow();
                    op.Handle = hWnd;
                    op.Title = strTitle;

                    alOpenWindows.Add(op);

                }
                return true;
            };

            if (WindowsEnumeratorHelper.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
                return alOpenWindows;
            }

            return null;
        }                              
    }

    public class OpenWindow
    {
        public IntPtr Handle = IntPtr.Zero;
        public string Title = "";
    }
}