using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using YaDi.Injection;
using YaDi.Enums;

namespace YaDi
{
    public partial class YaDi : Form
    {

        private String selectedDllPath = String.Empty;
        private int selectedProcessID = 0;
        private InjectionMethod selectedInjectMeth = InjectionMethod.LoadLibrary;
        private Structs.Config config;

        public YaDi()
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
            ProcessListView.Columns.Add("PID", 50);
            ProcessListView.Columns.Add("Arch", 50);
            ProcessListView.Columns.Add("Name", 100);
            ProcessListView.Columns.Add("Title", 80);
            ProcessListView.Columns.Add("Path", 150);

            PopulateProcessListView(String.Empty);
        }

        private void PopulateProcessListView(String szFilter)
        {
            ProcessListView.Items.Clear();

            foreach (Process proc in Process.GetProcesses())
            {
                if ((szFilter.Length > 0 && !proc.ProcessName.Contains(szFilter)) || (proc.Id == 0))
                {
                    continue;
                }

                String sProcFilename = Helpers.Process.GetFilename(proc);

                ListViewItem lvi = new ListViewItem(proc.Id.ToString());

                String sArch = (Helpers.Process.Is64Bit(sProcFilename) ? "64" : "32");

                lvi.SubItems.Add(sArch);
                lvi.SubItems.Add(proc.ProcessName);
                lvi.SubItems.Add(proc.MainWindowTitle);
                lvi.SubItems.Add(sProcFilename);

                ProcessListView.Items.Add(lvi);
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
                    case (ushort)InjectionMethod.QueueUserAPC:
                        {
                            methodStr = "QueueUserAPC";
                            break;
                        }
                    case (ushort)InjectionMethod.IATHook:
                        {
                            methodStr = "IATHook";
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
            InjectionMethod method = InjectionMethod.Undef;

            if (methodStr.Contains("IAT"))            { return InjectionMethod.IATHook; }
            if (methodStr.Contains("SetWindowsHook")) { return InjectionMethod.SetWindowsHook; }
            if (methodStr.Contains("Thread Hijack"))  { return InjectionMethod.ThreadHijack; }
            if (methodStr.Contains("QueueUserAPC"))   { return InjectionMethod.QueueUserAPC; }
            if (methodStr.Contains("LoadLibrary"))    { return InjectionMethod.LoadLibrary; }

            return method;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();

            fdlg.Title = "YaDi - Select DLL";
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
            if (selectedProcessID > 0 && DllPathText.Text.Length > 0)
            {
                Injectors.Base injector = null;

                switch(this.selectedInjectMeth)
                {
                    case InjectionMethod.LoadLibrary:    { injector = new LoadLibrary(selectedProcessID); break; }
                    case InjectionMethod.SetWindowsHook: { injector = new SetWindowsHookEx(selectedProcessID); break; }
                    case InjectionMethod.ThreadHijack:   { injector = new ThreadHijack(selectedProcessID); break; }
                    case InjectionMethod.IATHook:        { injector = new IATHook(selectedProcessID); break; }
                    case InjectionMethod.QueueUserAPC:   { injector = new QueueUserAPC(selectedProcessID); break; }
                }

                if (injector != null)
                {
                    injector.Inject(DllPathText.Text);
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateProcessListView(SearchTextBox.Text);
        }

        private void DllPathText_TextChanged(object sender, EventArgs e)
        {
            if (config != null && config.RememberLastDllPath())
            {
                config.SetLastDllPath(DllPathText.Text);
            }

            selectedDllPath = DllPathText.Text;
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
                case InjectionMethod.IATHook:
                    {
                        this.selectedInjectMeth = InjectionMethod.IATHook;

                        if (config != null && config.RememberLastMethod())
                        {
                            config.SetLastMethod((ushort)InjectionMethod.IATHook);
                        }
                        break;
                    }
            }
        }

        private void ProcessListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ProcessListView.SelectedItems)
            {
                if (Int32.TryParse(item.Text, out int pid))
                {
                    selectedProcessID = pid;
                    return;
                }
            }
        }
    }
}
