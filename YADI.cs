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
        private double processListViewLastUpdatedAt = 0;
        private double processListViewUpdateDelay = 0.500; // 500ms
        private InjectionMethod selectedInjectMeth = InjectionMethod.LoadLibrary;
        private Structs.Config config;

        public YADI()
        {
            config = new Structs.Config();

            InitializeComponent();
            InitializeDllPathInput();
            InitializeProcessListView();
            InitializeMethodComboBox();
        }

        private void InitializeDllPathInput()
        {
            if (DllPathText != null && config != null && config.RememberLastDllPath())
            {
                DllPathText.Text = (String)config.LastDllPath();
            }
        }

        private void InitializeProcessListView()
        {
            processListView.Columns.Add("PID", 50);
            processListView.Columns.Add("Name", 100);
            processListView.Columns.Add("Title", 75);
            processListView.Columns.Add("Path", 100);

            PopulateProcessListView("");
        }

        private void PopulateProcessListView(String filter)
        {
            double now = Helpers.Misc.Epoch();

            if ((now - processListViewLastUpdatedAt) < processListViewUpdateDelay)
            {
                return;
            }

            // Clear the list of any items
            processListView.Items.Clear();

            List<String> processStrings = new List<String>();

            foreach (Process process in Process.GetProcesses())
            {
                if (filter.Length > 0 && !process.ProcessName.Contains(filter))
                {
                    continue;
                }

                String sProcFilename = Helpers.Process.GetFilename(process);

                ListViewItem lvi = new ListViewItem(process.Id.ToString());

                lvi.SubItems.Add(process.ProcessName);
                lvi.SubItems.Add(process.MainWindowTitle);
                lvi.SubItems.Add(sProcFilename);

                processListView.Items.Add(lvi);
            }
        }

        private void InitializeMethodComboBox()
        {
            string methodStr = "LoadLibrary";

            if (config.RememberLastMethod())
            {
                switch(config.LastInjectionMethod())
                {
                    case (ushort)InjectionMethod.SetWindowsHook:
                        {
                            methodStr = "SetWindowsHook";
                            break;
                        } 
                    case (ushort)InjectionMethod.ThreadHijack:
                        {
                            methodStr = "Thread Hijack";
                            break;
                        }
                }
            }

            for (int i = 0; i < injectionMethComboBox.Items.Count; i++)
            {
                String itemStr = injectionMethComboBox.Items[i].ToString();

                if (itemStr.Contains(methodStr))
                {
                    injectionMethComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        InjectionMethod injectMethodStrToEnum(String methodStr)
        {
            if (methodStr.Contains("SetWindowsHook"))
            {
                return InjectionMethod.SetWindowsHook;
            }
            else if (methodStr.Contains("Thread Hijack"))
            {
                return InjectionMethod.ThreadHijack;
            }
            else if (methodStr.Contains("QueueUserAPC"))
            {
                Console.WriteLine("QueueUserAPC");
                return InjectionMethod.QueueUserAPC;
            }

            Console.WriteLine("LoadLibrary");
            return InjectionMethod.LoadLibrary;
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
                int pid = selectedProcess.Id;

                switch(this.selectedInjectMeth)
                {
                    case InjectionMethod.LoadLibrary:
                    {
                        (new LoadLibrary(pid)).Inject(DllPathText.Text);
                        break;
                    }
                    case InjectionMethod.SetWindowsHook:
                    {
                        (new SetWindowsHookEx(pid)).Inject(DllPathText.Text);
                        break;
                    }
                    case InjectionMethod.ThreadHijack:
                    {
                        (new ThreadHijack(pid)).Inject(DllPathText.Text);
                        break;
                    }
                    case InjectionMethod.IATHook:
                    {
                        (new IATHook(pid)).Inject(DllPathText.Text);
                        break;
                    }
                    case InjectionMethod.QueueUserAPC:
                    {
                        (new QueueUserAPC(pid)).Inject(DllPathText.Text);
                        break;
                    }
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateProcessListView(searchTextBox.Text);
        }

        private void DllPathText_TextChanged(object sender, EventArgs e)
        {
            if (config != null && config.RememberLastDllPath())
            {
                config.SetLastDllPath(DllPathText.Text);
            }
        }

        private void injectionMethComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectMethodStr = injectionMethComboBox.SelectedItem.ToString();

            switch(injectMethodStrToEnum(selectMethodStr))
            {
                case InjectionMethod.LoadLibrary:
                    {
                        this.selectedInjectMeth = InjectionMethod.LoadLibrary;

                        if (config != null && config.RememberLastMethod())
                        {
                            config.SetLastMethod((ushort)InjectionMethod.LoadLibrary);
                        }
                        break;
                    }
                case InjectionMethod.SetWindowsHook:
                    {
                        this.selectedInjectMeth = InjectionMethod.SetWindowsHook;

                        if (config != null && config.RememberLastMethod())
                        {
                            config.SetLastMethod((ushort)InjectionMethod.SetWindowsHook);
                        }
                        break;
                    }
                case InjectionMethod.ThreadHijack:
                    {
                        this.selectedInjectMeth = InjectionMethod.ThreadHijack;

                        if (config != null && config.RememberLastMethod())
                        {
                            config.SetLastMethod((ushort)InjectionMethod.ThreadHijack);
                        }
                        break;
                    }
                case InjectionMethod.QueueUserAPC:
                    {
                        this.selectedInjectMeth = InjectionMethod.QueueUserAPC;

                        if (config != null && config.RememberLastMethod())
                        {
                            config.SetLastMethod((ushort)InjectionMethod.QueueUserAPC);
                        }
                        break;
                    }
            }
        }

        private void processListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in processListView.SelectedItems)
            {
                item.Selected = true;
                item.Focused = true;
            }
        }
    }
}
