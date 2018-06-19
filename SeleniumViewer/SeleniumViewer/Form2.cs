using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        const int AW_SLIDE = 0X40000;
        const int AW_HOR_POSITIVE = 0X1;
        const int AW_HOR_NEGATIVE = 0X2;
        const int AW_BLEND = 0X80000;
        const int AW_HIDE = 0x1;

        [DllImport("user32")]
        static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);

        public Form2()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            //Load the Form At Position of Main Form
            int WidthOfMain = Application.OpenForms["Form1"].Width;
            int HeightofMain = Application.OpenForms["Form1"].Height;
            int LocationMainX = Application.OpenForms["Form1"].Location.X;
            int locationMainy = Application.OpenForms["Form1"].Location.Y;

            //Set the Location
            this.Location = new Point(LocationMainX + WidthOfMain, locationMainy + 1);

            //Animate form
            AnimateWindow(this.Handle, 500, AW_SLIDE | AW_HOR_POSITIVE);
        }
    }
}
