using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace MaximizeAlwaysConfig
{
    public partial class frmMain : MaximizeAlwaysConfig.CustomForm
    {
        private bool InitialChoice = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
            RegistryKey key = Registry.CurrentUser;

            try
            {
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

                if (key == null)
                {
                    Module.ShowMessage("Error. Could not Save if Application will start automatically with Windows");
                    return;
                }

                if (chkStartWithWindows.Checked)
                {
                    if (key.GetValue("MaximizeAlways") == null)
                    {
                        //3key.SetValue("MaximizeAlways", "\""+Application.StartupPath + "\\MaximizeAlways.exe"+"\"");

                        //3key.SetValue("MaximizeAlways", "\"" + Application.StartupPath + "\\MaximizeAlwaysLauncher.exe\" \"" + Application.StartupPath + "\\MaximizeAlways.exe" + "\"");

                        key.SetValue("MaximizeAlways", "\"" + Application.StartupPath + "\\MaximizeAlwaysLauncher.exe\"");
                    }
                }
                else
                {
                    if (key.GetValue("MaximizeAlways") != null)
                    {
                        key.DeleteValue("MaximizeAlways");
                    }
                }

                key.Close();

                key = Registry.CurrentUser;
                key = key.OpenSubKey(@"Software\4dots Software\MaximizeAlways", true);


                key.SetValue("HideMaximizeAlways", chkHideTrayIcon.Checked);
                key.SetValue("AllAppsMaximized", chkAll.Checked);
                key.SetValue("ConfiguredOnce", true.ToString());

                key.SetValue("LaunchWithAdminRights", chkAdminRights.Checked);

                key = key.OpenSubKey("Applications",true);

                string[] valn = key.GetValueNames();

                for (int k = 0; k < valn.Length; k++)
                {
                    key.DeleteValue(valn[k]);
                }

                for (int k = 0; k < lstApps.Items.Count; k++)
                {
                    key.SetValue("Application #" + k.ToString(), lstApps.Items[k].ToString());
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Module.ShowError(ex);
            }
            finally
            {
                try { key.Close(); }
                catch { }
            }                 
            
      
            Process[] procs = System.Diagnostics.Process.GetProcessesByName("MaximizeAlways");

            for (int k=0;k<procs.Length;k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch{}
            }

            procs = System.Diagnostics.Process.GetProcessesByName("MaximizeAlwaysAdminRights");

            for (int k=0;k<procs.Length;k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch{}
            }

            /*
            if (procs.Length > 0)
            {
                MessageHelper msgh=new MessageHelper();
                msgh.SendMessageToMaximizeAlways();
                                
            }
            else
            {
                procs = System.Diagnostics.Process.GetProcessesByName("MaximizeAlwaysAdminRights");

                if (procs.Length > 0)
                {
                    MessageHelper msgh=new MessageHelper();
                    msgh.SendMessageToMaximizeAlways();                                
                }
                e
                System.Diagnostics.Process.Start("\""+Application.StartupPath + "\\MaximizeAlwaysLauncher.exe\"");
            }
            */
            
            System.Diagnostics.Process.Start("\""+Application.StartupPath + "\\MaximizeAlwaysLauncher.exe\"");
            //end

            Application.Exit();
            Environment.Exit(0);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.maximize_always_48;

            RegistryKey key = Registry.CurrentUser;

            try
            {
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

                if (key == null)
                {
                    Module.ShowMessage("Error. Could not specify if Application will start automatically with Windows");                    
                }

                if (key.GetValue("MaximizeAlways") == null)
                {
                    chkStartWithWindows.Checked = false;
                }
                else
                {
                    chkStartWithWindows.Checked = true;
                }

                key = Registry.CurrentUser;
                key = key.OpenSubKey(@"Software\4dots Software\MaximizeAlways", false);


                chkHideTrayIcon.Checked = bool.Parse(key.GetValue("HideMaximizeAlways",false).ToString());
                chkAll.Checked = bool.Parse(key.GetValue("AllAppsMaximized",false).ToString());
                chkAdminRights.Checked = bool.Parse(key.GetValue("LaunchWithAdminRights", false).ToString());

                lstApps.Items.Clear();

                key = key.OpenSubKey("Applications");

                string[] valn = key.GetValueNames();

                for (int k = 0; k < valn.Length; k++)
                {
                    if (valn[k].Trim() != String.Empty && key.GetValue(valn[k]).ToString().Trim()!=string.Empty)
                    {
                        lstApps.Items.Add(key.GetValue(valn[k]));
                    }
                }

                DownloadSuggestionsHelper ds = new DownloadSuggestionsHelper();
                ds.SetupDownloadMenuItems(downloadToolStripMenuItem);
            }
            catch (Exception ex)
            {
                Module.ShowError(ex);
            }            
        }    

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtFilename.Text.Trim()==String.Empty)
            {
                Module.ShowMessage("Please specify the Application's Filename !");
                return;
            }

            string fn=System.IO.Path.GetFileName(txtFilename.Text);
            string ext=System.IO.Path.GetExtension(txtFilename.Text);

            if (ext=="")
            {
                fn+=".exe";
                ext = System.IO.Path.GetExtension(fn);
            }
            
                if (ext.ToLower()!=".exe")
                {
                    Module.ShowMessage("Please specify a valid Executable Application as the Filename");
                    return;
                }
                else
                {
                    bool found = false;
                    for (int d = 0; d < lstApps.Items.Count; d++)
                    {
                        if (lstApps.Items[d].ToString().ToLower() == fn.ToLower())
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        lstApps.Items.Add(fn);
                    }
                }
            
        }

        private void btnBrowseAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Executable Files (*.*)|*.exe";
            openFileDialog1.CheckFileExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fn = System.IO.Path.GetFileName(openFileDialog1.FileName);
                string ext = System.IO.Path.GetExtension(openFileDialog1.FileName);


                if (ext.ToLower() != ".exe")
                {
                    Module.ShowMessage("Please specify a valid Executable Application as the Filename");
                    return;
                }
                else
                {
                    bool found = false;
                    for (int d = 0; d < lstApps.Items.Count; d++)
                    {
                        if (lstApps.Items[d].ToString().ToLower() == fn.ToLower())
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        lstApps.Items.Add(fn);
                    }
                }
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (lstApps.SelectedItems.Count > 0)
            {
                lstApps.Items.Remove(lstApps.SelectedItems[0]);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            //3System.Diagnostics.Process.Start(Application.StartupPath + "\\MaximizeAlways Help Manual.chm");

            System.Diagnostics.Process.Start(Module.WebpageURL);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAbout f = new frmAbout();
            f.ShowDialog();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //CursorManager.ChangeSystemCursor(@"C:\CodeGraphics\Cursors.Vista\Cursors\arrow_m.cur");
            CursorManager.ChangeSystemCursor(Properties.Resources.target_red_321);
            DoDragDrop(this, DragDropEffects.All);

            //DoDragDrop(panel1, DragDropEffects.All);

        }

        private void frmMain_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Drop || e.Action == DragAction.Cancel)
            {
                CursorManager.DestroyCursor(CursorManager.TargetCursor);

                IntPtr hwnd = Win32Api.WindowFromPoint(Win32Api.GetCursorPosition());
                uint procid = 0;
                Win32Api.GetWindowThreadProcessId(hwnd, out procid);

                if (procid != 0)
                {
                    Process pr = System.Diagnostics.Process.GetProcessById((int)procid);
                    string fn = "";
                    try
                    {

                        //fn = System.IO.Path.GetFileName(pr.MainModule.FileName);                        

                        fn = System.IO.Path.GetFileName(ProcessFilenameRetriever.GetExecutablePath(pr));

                        //MessageBox.Show(fn);

                        bool found=false;

                        for (int d = 0; d < lstApps.Items.Count; d++)
                        {
                            if (fn.ToLower() == lstApps.Items[d].ToString().ToLower())
                            {
                                found = true;
                            }
                        }
                        
                        if (!found)
                        {
                            lstApps.Items.Add(fn);
                        }
                    }
                    catch { }
                }

                CursorManager.ClearCursor();

                //CursorManager.ChangeSystemCursor(@"C:\CodeGraphics\Cursors.Vista\Cursors\arrow_m.cur");
            }
            else if (e.Action == DragAction.Continue)
            {
                CursorManager.ChangeSystemCursor(Properties.Resources.target_red_321);

                IntPtr hwnd = Win32Api.WindowFromPoint(Win32Api.GetCursorPosition());
                RECT r;

                Win32Api.GetWindowRect(hwnd, out r);
                Rectangle re = r.ToRectangle();
                Win32Api.DrawReversibleFrame(re);

            }
        }

        private void frmMain_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
        }

        private void downloadMultipleSearchAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.multiplereplace.com");
        }

        private void downloadPDFMergeSplitToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.pdfdocmerge.com/pdfmergesplittool/");

        }

        private void downloadFreeFileUnlockerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.freefileunlocker.com");
        }

        private void downloadImgTransformerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.imgtransformer.com");
        }

        private void downloadFreeImagemapperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/imagemapper2/");
        }

        private void downloadCopyPathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/copypathtoclipboard/");
        }

        private void downloadCopyTextContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/copytextcontents/");
        }

        private void downloadOpenCommandPromptHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/open_command_prompt_here/");
        }

        private void downloadFreeColorwheelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/colorwheel/");
        }

        private void visit4dotsSoftwareWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com");
        }


        private void downloadDocusTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/freedocustree/");
        }

        #region Share

        private void shareOnFacebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareFacebook();
        }

        private void shareOnTwitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareTwitter();
        }

        private void shareOnGooglePlusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareGooglePlus();
        }

        private void shareOnLinkedinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareLinkedIn();
        }

        private void shareWithEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareEmail();
        }

        #endregion        

        private void pleaseDonateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/donate.php");
        }

        private void dotsSoftwarePRODUCTCATALOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/downloads/4dots-Software-PRODUCT-CATALOG.pdf");
        }

        private void followUsOnTwitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.twitter.com/4dotsSoftware");
        }

        private void btnAddWindowText_Click(object sender, EventArgs e)
        {
            string txt = "Window Text";

            if (chkRegExp.Checked)
            {
                txt += " - RegExp";

                try
                {
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(txtWindowText.Text);

                    if (regex.IsMatch(txtWindowText.Text))
                    {
                    }
                }
                catch
                {
                    Module.ShowMessage("Incorrect Regular Expression format !");

                    return;
                }
            }

            if (chkWildcards.Checked)
            {
                txt += " - Wildcards";
            }

            txt += " : "+txtWindowText.Text;

            bool found = false;
            for (int d = 0; d < lstApps.Items.Count; d++)
            {
                if (lstApps.Items[d].ToString()==txt)
                {
                    found = true;
                }
            }

            if (!found)
            {
                lstApps.Items.Add(txt);
            }
        }

        private void chkRegExp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRegExp.Checked)
            {
                chkWildcards.Checked = false;
            }
        }

        private void chkWildcards_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWildcards.Checked)
            {
                chkRegExp.Checked = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper2.WM_COPYDATA)
            {
                MessageHelper2.COPYDATASTRUCT mystr = new MessageHelper2.COPYDATASTRUCT();
                Type mytype = mystr.GetType();
                mystr = (MessageHelper2.COPYDATASTRUCT)m.GetLParam(mytype);

                string arg = mystr.lpData;

                if (arg == "SHOW")
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.BringToFront();
                }
            }
            else if (m.Msg == MessageHelper2.WM_ACTIVATEAPP)
            {
                this.Show();

                this.WindowState = FormWindowState.Normal;
                this.Show();
                this.BringToFront();
            }
            else
            {
                base.WndProc(ref m);
            }

        }
    }
}

