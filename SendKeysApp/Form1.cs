using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SendKeysApp
{
    public partial class Form1 : Form
    {
        private MouseHook _mouseHook;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

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
        }

        String targetWindow = "";
        String targetClass = "";

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseClick += new MouseEventHandler(Form1_MouseClick);

            _mouseHook = new MouseHook();
            _mouseHook.SetHook(_mouseHook.HookCallback);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                string windowTitle = getWindowTitleAtCursor();
                label4.Text = windowTitle;
                
                string className = getClassNameAtCursor();
                label5.Text = className;
                
                IntPtr hWnd = getHwndAtCursor();
                label6.Text = hWnd.ToString();

            }

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = "Cur X: " + e.X + ", Cur Y: " + e.Y;



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Find the window by its class name and window title
            IntPtr hWnd = FindWindow("ConsoleWindowClass", "Command Prompt");
            // Set the window as the active window
            SetForegroundWindow(hWnd);
            // Send the keystrokes
            SendKeys.Send("dir{ENTER}");
        }

        private string getWindowTitleAtCursor()
        {
            string returnValue = "";
            POINT cursor = new POINT(Cursor.Position.X, Cursor.Position.Y);
            IntPtr hWnd = WindowFromPoint(cursor);
            if (hWnd != IntPtr.Zero)
            {
                StringBuilder windowTitle = new StringBuilder(256);
                GetWindowText(hWnd, windowTitle, windowTitle.Capacity);
                returnValue = windowTitle.ToString();
            }
            return returnValue;
        }

        private string getClassNameAtCursor()
        {
            string returnValue = "";
            POINT cursor = new POINT(Cursor.Position.X, Cursor.Position.Y);
            IntPtr hWnd = WindowFromPoint(cursor);
            if (hWnd != IntPtr.Zero)
            {
                StringBuilder className = new StringBuilder(256);
                GetClassName(hWnd, className, className.Capacity);
                returnValue = className.ToString();
            }
            return returnValue;
        }

        private IntPtr getHwndAtCursor()
        {
            IntPtr returnValue = (IntPtr)0;
            POINT cursor = new POINT(Cursor.Position.X, Cursor.Position.Y);
            IntPtr hWnd = WindowFromPoint(cursor);
            if (hWnd != IntPtr.Zero)
            {
                StringBuilder className = new StringBuilder(256);
                GetClassName(hWnd, className, className.Capacity);
                returnValue = hWnd;
            }
            return returnValue;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "Tick Cur X: " + Cursor.Position.X + ", Tick Cur Y: " + Cursor.Position.Y;

            label2.Text = "Window Title: " + getWindowTitleAtCursor();
            
            label3.Text = "Window Handle: " + getHwndAtCursor();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
