using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using YADI.Injection;
using YADI.Enums;


namespace YADI
{
    public partial class YADI : Form
    {
        private String selectedDllPath = String.Empty;
        private int selectedProcessID = 0;
        private InjectionMethod selectedInjectMeth = InjectionMethod.LoadLibrary;
        private Structs.Config config;

        public YADI()
        {
            config = new Structs.Config();

            InitializeComponent();
            InitializeDllPathInput();
            InitializeProcessListView();
            InitializeMethodComboBox();
            InitializeArchitectureRadioButtons();
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
            ProcessListView.Columns.Add("Name", 150);
            ProcessListView.Columns.Add("Title", 80);
            ProcessListView.Columns.Add("Path", 150);

            PopulateProcessListView(String.Empty);
        }

        private void PopulateProcessListView(String filter)
        {
            ProcessListView.Items.Clear();

            List<String> processStrings = new List<String>();

            foreach (Process process in Process.GetProcesses())
            {
                if ((filter.Length > 0 && !process.ProcessName.Contains(filter)) || (process.Id == 0))
                {
                    continue;
                }

                String sProcFilename = Helpers.Process.GetFilename(process);

                ListViewItem lvi = new ListViewItem(process.Id.ToString());

                lvi.SubItems.Add(process.ProcessName);
                lvi.SubItems.Add(process.MainWindowTitle);
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

        private void InitializeArchitectureRadioButtons()
        {
            if (!Environment.Is64BitProcess)
            {
                x64RadioButton.Enabled = false;
                x86RadioButton.Checked = true;
            }
        }

        InjectionMethod injectMethodStrToEnum(String methodStr)
        {
            InjectionMethod method = InjectionMethod.Undef;

            if (methodStr.Contains("SetWindowsHook")) { return InjectionMethod.SetWindowsHook; }
            if (methodStr.Contains("Thread Hijack"))  { return InjectionMethod.ThreadHijack;   }
            if (methodStr.Contains("QueueUserAPC"))   { return InjectionMethod.QueueUserAPC;   }
            if (methodStr.Contains("IAT"))            { return InjectionMethod.IATHook;        }

            return method;
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
            if (selectedProcessID > 0 && DllPathText.Text.Length > 0)
            {
                Injectors.Base injector = new Injectors.Base();

                switch(this.selectedInjectMeth)
                {
                    case InjectionMethod.LoadLibrary:    { injector = new LoadLibrary(selectedProcessID); break; }
                    case InjectionMethod.SetWindowsHook: { injector = new SetWindowsHookEx(selectedProcessID); break; }
                    case InjectionMethod.ThreadHijack:   { injector = new ThreadHijack(selectedProcessID); break; }
                    case InjectionMethod.IATHook:        { injector = new IATHook(selectedProcessID); break; }
                    case InjectionMethod.QueueUserAPC:   { injector = new QueueUserAPC(selectedProcessID); break; }
                }

                injector.Inject(DllPathText.Text);
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateProcessListView(SearchTextBox.Text);
            InjectButton_TryEnable();
        }

        private void DllPathText_TextChanged(object sender, EventArgs e)
        {
            if (config != null && config.RememberLastDllPath())
            {
                config.SetLastDllPath(DllPathText.Text);
            }

            selectedDllPath = DllPathText.Text;

            InjectButton_TryEnable();
        }

        private void InjectButton_TryEnable()
        {
            if (File.Exists(selectedDllPath) && selectedProcessID > 0)
            {
                InjectButton.Enabled = true;
            }
            else
            {
                InjectButton.Enabled = false;
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
            foreach (ListViewItem item in ProcessListView.SelectedItems) {
                if (Int32.TryParse(item.Text, out int pid))
                {
                    selectedProcessID = pid;
                    Helpers.PortableExecParser pep = new Helpers.PortableExecParser(pid);
                    pep.Parse();
                    InjectButton_TryEnable();
                    return;
                }
            }
        }

        private void x64RadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
