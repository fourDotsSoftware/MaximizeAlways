using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace MaximizeAlwaysConfig
{
    public class Win32Api
    {
        public static Rectangle LastFramedRectangle = Rectangle.Empty;

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT rect);

        

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        

        public static void DrawReversibleFrame(Rectangle re)
        {   
            if (re.X==LastFramedRectangle.X && re.Y==LastFramedRectangle.Y && re.Width==LastFramedRectangle.Width
                && re.Height==LastFramedRectangle.Height)
            {
                return;
            }
            else
            {
                System.Windows.Forms.ControlPaint.DrawReversibleFrame(
                LastFramedRectangle,
                System.Drawing.Color.White,
                System.Windows.Forms.FrameStyle.Thick);

            }

            System.Windows.Forms.ControlPaint.DrawReversibleFrame(
                re,
                System.Drawing.Color.White,
                System.Windows.Forms.FrameStyle.Thick);

            LastFramedRectangle = re;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(System.Drawing.Rectangle rect)
        {
            Left = rect.X;
            Top = rect.Y;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width
        {
            get
            {
                return Right - Left;
            }
            set
            {
                Right = Left + value;
            }
        }
        public int Height
        {
            get
            {
                return Bottom - Top;
            }
            set
            {
                Bottom = Top + value;
            }
        }

        public System.Drawing.Rectangle ToRectangle()
        {
            return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
        }

    }
}
