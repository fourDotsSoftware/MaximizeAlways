using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace MaximizeAlwaysConfig
{
    public class CursorManager
    {
        #region Win32Api

        private class Win32Api
        {
            [DllImport("user32.dll")]
            internal static extern IntPtr LoadCursorFromFile(string lpFileName);

            [DllImport("user32.dll")]
            internal static extern bool SetSystemCursor(IntPtr hcur, uint id);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr handle);

            [DllImport("user32.dll")]
            public static extern IntPtr CopyIcon(IntPtr hIcon);                               

            [DllImport("user32.dll", EntryPoint = "LoadCursorW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            public extern static IntPtr LoadCursor(IntPtr hInstance, int type);

            internal const uint OCR_NORMAL = 32512;
            internal const uint OCR_NO = 32648;
            internal const int IDC_ARROW = 32512;
        }

        #endregion Win32Api

        public static IntPtr TargetCursor = IntPtr.Zero;
        private static IntPtr OldCursor = IntPtr.Zero;

        public static string CursorFilepath = "";

        [DllImport("user32.dll")]
        public static extern bool DestroyCursor(IntPtr hCursor);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);

        public static void ClearCursor()
        {            
            Win32Api.SetSystemCursor(OldCursor, Win32Api.OCR_NORMAL);
            DestroyCursor(OldCursor);
            OldCursor = IntPtr.Zero;

            bool b = Win32Api.SetSystemCursor(IntPtr.Zero, Win32Api.OCR_NORMAL);
            CursorManager.DestroyCursor(CursorManager.TargetCursor);
            CursorManager.DestroyIcon(CursorManager.TargetCursor);

            if (CursorManager.CursorFilepath != String.Empty && System.IO.File.Exists(CursorManager.CursorFilepath))
            {
                System.IO.File.Delete(CursorManager.CursorFilepath);
            }
        }

        public static void ChangeSystemCursor(byte[] CursorBinaryArray)
        {
            if (TargetCursor != IntPtr.Zero)
            {
                ClearCursor();
            }

            OldCursor=Win32Api.CopyIcon(Win32Api.LoadCursor(IntPtr.Zero,Win32Api.IDC_ARROW));

            if (CursorFilepath != String.Empty && System.IO.File.Exists(CursorFilepath))
            {
                System.IO.File.Delete(CursorFilepath);
            }

            string ani = string.Empty;
            ani = System.IO.Path.GetTempFileName();//File extension is not mandatory

            CursorFilepath = ani;

            using (System.IO.FileStream wr = System.IO.File.Create(ani))
            {
                using (System.IO.BinaryWriter bwr = new System.IO.BinaryWriter(wr))
                {
                    bwr.Write(CursorBinaryArray);
                }
            }

            TargetCursor = Win32Api.LoadCursorFromFile(ani);
            //to set form cursor set the system cursor set the winform’s cursor as ->  this.Cursor = new Cursor(hAni); and on closing destroy the handle

            bool b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NORMAL);
            //b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NO);
            //System.IO.File.Delete(ani);
        }

        public static void ChangeSystemCursor(System.Drawing.Icon ico)
        {
            if (TargetCursor != IntPtr.Zero)
            {
                ClearCursor();
            }
                        
            OldCursor = Win32Api.CopyIcon(Win32Api.LoadCursor(IntPtr.Zero, Win32Api.IDC_ARROW));

            if (CursorFilepath != String.Empty && System.IO.File.Exists(CursorFilepath))
            {
                System.IO.File.Delete(CursorFilepath);
            }

            string ani = string.Empty;
            ani = System.IO.Path.GetTempFileName();//File extension is not mandatory

            StreamWriter iconWriter = new StreamWriter(ani);            
            ico.Save(iconWriter.BaseStream);
            iconWriter.Close();
            iconWriter.Dispose();

            TargetCursor = Win32Api.LoadCursorFromFile(ani);
            CursorFilepath = ani;
            //to set form cursor set the system cursor set the winform’s cursor as ->  this.Cursor = new Cursor(hAni); and on closing destroy the handle

            bool b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NORMAL);
            //b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NO);
            //System.IO.File.Delete(ani);

            //MessageBox.Show(b.ToString());
            //Win32Api.DestroyCursor(TargetCursor);

        }

        public static void ChangeSystemCursor(string filepath)
        {
            if (TargetCursor != IntPtr.Zero)
            {
                ClearCursor();
            }

            OldCursor = Win32Api.CopyIcon(Win32Api.LoadCursor(IntPtr.Zero, Win32Api.IDC_ARROW));

            TargetCursor = Win32Api.LoadCursorFromFile(filepath);
            //to set form cursor set the system cursor set the winform’s cursor as ->  this.Cursor = new Cursor(hAni); and on closing destroy the handle

            bool b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NORMAL);
            //b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NO);
            //System.IO.File.Delete(ani);

           // MessageBox.Show(b.ToString());

            //Win32Api.DestroyCursor(TargetCursor);

        }
    }
}
