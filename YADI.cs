using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Text.RegularExpressions;

using YADI.Injection;
using YADI.Enums;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YADI
{
    public partial class YADI : Form
    {
        private String  selectedDllPath = null;
        private Process selectedProcess = null;
        private InjectionMethod selectedInjectMeth = InjectionMethod.LoadLibrary;
        private Structs.Config config;

        public YADI()
        {
            config = new Structs.Config();

            InitializeComponent();
            InitializeDllPathInput();
            InitializeInjectionMethod();
            InitializeProcessList();
        }

        private void InitializeDllPathInput()
        {
            if (DllPathText != null && config != null && config.RememberLastDllPath())
            {
                DllPathText.Text = (String)config.LastDllPath();
            }
        }

        private void InitializeInjectionMethod()
        {
            if (config.RememberLastMethod())
            {
                switch (config.LastInjectionMethod())
                {
                    case (ushort)InjectionMethod.LoadLibrary:
                        {
                            loadLibraryRadioButton.Checked = true;
                            break;
                        }
                    case (ushort)InjectionMethod.SetWindowsHook:
                        {
                            setWindowsHookExMethButton.Checked = true;
                            break;
                        }
                    case (ushort)InjectionMethod.ThreadHijack:
                        {
                            threadHijackMethButton.Checked = true;
                            break;
                        }
                }
            }
        }

        private void InitializeProcessList()
        {
            Process[] processCollection = Process.GetProcesses();
            List<String> processStrings = new List<String>();

            foreach (Process process in processCollection)
            {
                if (process.Id == 0)
                {
                    continue;   
                }

                processListBox.Items.Add(process.Id.ToString() + " - " + process.ProcessName);
            }

            processListBox.EndUpdate();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "YADI DLL Injector - Browse DLLs";
            fdlg.InitialDirectory = @Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            fdlg.Filter = "All files (*.*)|*.*|Dll files (*.dll)|*.dll";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;

            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                DllPathText.Text = fdlg.FileName;
            }
        }

        private void InjectButton_Click(object sender, EventArgs e)
        {
            if (selectedProcess != null && DllPathText.Text.Length > 0)
            {
                switch(this.selectedInjectMeth)
                {
                    case InjectionMethod.LoadLibrary:
                    {
                        (new LoadLibrary(this.selectedProcess.Id))
                            .inject(DllPathText.Text);
                        break;
                    }
                    case InjectionMethod.SetWindowsHook:
                    {
                        // @TODO
                        break;
                    }
                    case InjectionMethod.ThreadHijack:
                    {
                        (new ThreadHijack(this.selectedProcess.Id))
                            .inject(DllPathText.Text);
                        break;
                    }
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            Process[] processCollection = Process.GetProcesses();

            if (searchTextBox.Text.Length == 0)
            {
                InitializeProcessList();
                return;
            }

            processListBox.Items.Clear();

            foreach (Process p in processCollection)
            {
                if (p.Id == 0)
                {
                    continue;
                }

                if (p.ProcessName.Contains(searchTextBox.Text))
                {
                    processListBox.Items.Add(p.Id.ToString() + " - " + p.ProcessName);
                }
            }

            processListBox.EndUpdate();
        }

        private void DllPathText_TextChanged(object sender, EventArgs e)
        {
            if (config != null && config.RememberLastDllPath())
            {
                config.SetLastDllPath(DllPathText.Text);
            }
        }

        private void loadLibraryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedInjectMeth = InjectionMethod.LoadLibrary;

            if (config != null && config.RememberLastMethod())
            {
                config.SetLastMethod((ushort)InjectionMethod.LoadLibrary);
            }
        }

        private void threadBypassRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedInjectMeth = InjectionMethod.ThreadHijack;

            if (config != null && config.RememberLastMethod())
            {
                config.SetLastMethod((ushort)InjectionMethod.ThreadHijack);
            }
        }
        private void setWindowsHookExMethButton_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedInjectMeth = InjectionMethod.SetWindowsHook;

            if (config != null && config.RememberLastMethod())
            {
                config.SetLastMethod((ushort)InjectionMethod.SetWindowsHook);
            }
        }

        private void processListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selected = (String)processListBox.SelectedItem;

            if (selected == null)
            {
                return;
            }

            int loc = selected.IndexOf(" ");

            if (loc > 0)
            {
                String pid_str = selected.Substring(0, loc);

                if (int.TryParse(pid_str, out int p))
                {
                    this.selectedProcess = Process.GetProcessById(p);
                }
                else
                {
                    this.selectedProcess = null;
                }
            }
        }
    }
}
