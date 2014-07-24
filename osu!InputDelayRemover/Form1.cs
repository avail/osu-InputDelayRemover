using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Diagnostics;

namespace osu_InputDelayRemover
{
    public partial class Form1 : Form
    {
        bool hacked = false;

        string regPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";
        string file = "";

        public Form1()
        {
            InitializeComponent();
            initialPopulation();
            openTheFile();
        }

        void initialPopulation()
        {
            labelStatus.Text = "Unknown";
            labelStatus.ForeColor = Color.Yellow;
            buttonChange.Text = "Enable";
            buttonChange.ForeColor = Color.LightGray;
            buttonChange.Enabled = false;
        }

        void openTheFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "osu!|osu!.exe|osu!test|osu!test.exe";

            DialogResult OpenFile = openFileDialog1.ShowDialog();
            if (OpenFile == DialogResult.OK)
            {
                file = openFileDialog1.FileName;
                isRegistryHackedCheck(file);
            }
            else
            {
                Application.Exit();
            }
        }

        void refreshRegistry()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "Rundll32 apphelp.dll,ShimFlushCache";
            process.StartInfo = startInfo;
            process.Start();
        }

        void isRegistryHackedCheck(string file)
        {
            object val = Registry.GetValue(regPath, file, null);
            if (val == null)
            {
                labelStatus.Text = "Not enabled";
                labelStatus.ForeColor = Color.Red;
                buttonChange.Text = "Enable";
                buttonChange.ForeColor = Color.Black;
                buttonChange.Enabled = true;
                hacked = false;
            }
            else
            {
                labelStatus.Text = "Enabled";
                labelStatus.ForeColor = Color.Green;
                buttonChange.Text = "Disable";
                buttonChange.ForeColor = Color.Black;
                buttonChange.Enabled = true;
                hacked = true;
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (hacked == true)
            {
                //Registry.LocalMachine.OpenSubKey(regPath).DeleteValue(file, true);
                RegistryKey regkeyOpen = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true);
                regkeyOpen.DeleteValue(file, true);
                isRegistryHackedCheck(file);
                refreshRegistry();
            }
            else
            {
                Registry.SetValue(regPath, file, "NoDTToDITMouseBatch");
                isRegistryHackedCheck(file);
                refreshRegistry();
            }
        }
    }
}
