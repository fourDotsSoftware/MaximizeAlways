using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MaximizeAlwaysConfig
{
    public partial class frmLanguage : CustomForm
    {
        public frmLanguage()
        {
            InitializeComponent();
            
        }

        public static List<string> LanguageKeys=new List<string>();

        public int SelectedLanguage = 0;

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedLanguage = cmbLanguage.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            
        }

        private void frmLanguage_Load(object sender, EventArgs e)
        {
            //chinese,dutch,finish,danish,japan,norway,russia,sweden
            cmbLanguage.Items.AddRange(new string[] { "English", "Deutsch", "Français", "Español", "Português", "Italiano", "中文", "Nederlands", "Suomi", "Dansk", "日本の", "Norske", "Pусский", "Svenskt" });

            cmbLanguage.SelectedIndex = 0;
        }

        public static void AddLanguageKeys()
        {
            LanguageKeys.Add("en-US");
            LanguageKeys.Add("de-DE");
            LanguageKeys.Add("fr-FR");
        }
    }
}

