using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MaximizeAlwaysConfig
{
    public partial class frmSettings : MaximizeAlwaysConfig.CustomForm
    {
        private bool InitialChoice = false;

        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (InitialChoice != chkAllowSharing.Checked)
            {
                if (!chkAllowSharing.Checked)
                {

                    System.Diagnostics.Process proc = new Process();
                    proc.StartInfo.Arguments = "-clearchoice \"Maximize Always\"";
                    proc.StartInfo.FileName = System.IO.Path.Combine(Application.StartupPath, "LuminatiHelper.exe");
                    proc.Start();

                    Module.ShowMessage("Please support us and agree to share safely some of your idle resources.\n\nRestart application if you want to accept and use the application.");
                    Environment.Exit(0);
                }
                else
                {
                    Properties.Settings.Default.FirstTimeRun = true;

                    Program.InitRegistryLuminati();

                    while (true)
                    {
                        string regval = RegistryHelper2.GetKeyValue("Maximize Always", "LuminatiChoice");

                        if (regval == "NOTSET")
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                        else if (regval == "ACCEPT")
                        {
                            break;
                        }
                        else
                        {
                            Environment.Exit(0);
                            return;
                        }

                        Application.DoEvents();
                    }            
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            //chkAllowSharing.Checked = Properties.Settings.Default.LuminatiAgree;

            chkAllowSharing.Checked = false;

            try
            {

                chkAllowSharing.Checked = RegistryHelper2.GetKeyValue("Maximize Always", "LuminatiChoice") == "ACCEPT";

                InitialChoice = chkAllowSharing.Checked;
            }
            catch { }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
