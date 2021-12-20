using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MaximizeAlwaysConfig
{
    public class CustomForm : System.Windows.Forms.Form
    {
        public CustomForm()
        {
            InitializeComponent();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {

            try
            {
                System.Drawing.Graphics g = e.Graphics;
                int x = this.Width;
                int y = this.Height;

                System.Drawing.Drawing2D.LinearGradientBrush
                    lgBrush = new System.Drawing.Drawing2D.LinearGradientBrush
                    (new System.Drawing.Point(0, 0), new System.Drawing.Point(x, y),
                    Color.White, Color.FromArgb(190, 190, 190));
                lgBrush.GammaCorrection = true;
                g.FillRectangle(lgBrush, 0, 0, x, y);

            }
            catch
            {
                base.OnPaintBackground(e);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomForm));
            this.SuspendLayout();
            // 
            // CustomForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.CustomForm_Load);
            this.ResumeLayout(false);

        }

        private void CustomForm_Load(object sender, EventArgs e)
        {
            if (!(this is frmMain))
            {
                this.ShowInTaskbar = false;
            }
        }
    }
}
