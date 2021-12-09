
namespace YADI
{
    partial class YADI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YADI));
            this.InjectButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Browse = new System.Windows.Forms.Button();
            this.DllPathText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.processListBox = new System.Windows.Forms.ListBox();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.processSearchrLabel = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.colorDialog3 = new System.Windows.Forms.ColorDialog();
            this.injectionMethComboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // InjectButton
            // 
            this.InjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InjectButton.Location = new System.Drawing.Point(36, 405);
            this.InjectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.InjectButton.Name = "InjectButton";
            this.InjectButton.Size = new System.Drawing.Size(371, 47);
            this.InjectButton.TabIndex = 0;
            this.InjectButton.Text = "Inject";
            this.InjectButton.UseVisualStyleBackColor = true;
            this.InjectButton.Click += new System.EventHandler(this.InjectButton_Click);
            // 
            // Browse
            // 
            this.Browse.Image = ((System.Drawing.Image)(resources.GetObject("Browse.Image")));
            this.Browse.Location = new System.Drawing.Point(316, 62);
            this.Browse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(91, 28);
            this.Browse.TabIndex = 2;
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // DllPathText
            // 
            this.DllPathText.Location = new System.Drawing.Point(36, 62);
            this.DllPathText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DllPathText.Multiline = true;
            this.DllPathText.Name = "DllPathText";
            this.DllPathText.Size = new System.Drawing.Size(271, 25);
            this.DllPathText.TabIndex = 3;
            this.DllPathText.TabStop = false;
            this.DllPathText.WordWrap = false;
            this.DllPathText.TextChanged += new System.EventHandler(this.DllPathText_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "DLL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 17);
            this.label2.TabIndex = 9;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(451, 28);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // processListBox
            // 
            this.processListBox.FormattingEnabled = true;
            this.processListBox.ItemHeight = 16;
            this.processListBox.Location = new System.Drawing.Point(36, 203);
            this.processListBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.processListBox.Name = "processListBox";
            this.processListBox.Size = new System.Drawing.Size(369, 180);
            this.processListBox.TabIndex = 16;
            this.processListBox.SelectedIndexChanged += new System.EventHandler(this.processListBox_SelectedIndexChanged);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(36, 159);
            this.searchTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(369, 22);
            this.searchTextBox.TabIndex = 17;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // processSearchrLabel
            // 
            this.processSearchrLabel.AutoSize = true;
            this.processSearchrLabel.Location = new System.Drawing.Point(32, 139);
            this.processSearchrLabel.Name = "processSearchrLabel";
            this.processSearchrLabel.Size = new System.Drawing.Size(53, 17);
            this.processSearchrLabel.TabIndex = 18;
            this.processSearchrLabel.Text = "Search";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // injectionMethComboBox
            // 
            this.injectionMethComboBox.FormattingEnabled = true;
            this.injectionMethComboBox.Items.AddRange(new object[] {
            "QueueUserAPC",
            "IAT Hook",
            "LoadLibrary",
            "Reflective",
            "SetWindowsHookEx",
            "Thread Hijack / SIR (Suspend, Inject, Resume)"});
            this.injectionMethComboBox.Location = new System.Drawing.Point(36, 107);
            this.injectionMethComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.injectionMethComboBox.Name = "injectionMethComboBox";
            this.injectionMethComboBox.Size = new System.Drawing.Size(369, 24);
            this.injectionMethComboBox.TabIndex = 19;
            this.injectionMethComboBox.SelectedIndexChanged += new System.EventHandler(this.injectionMethComboBox_SelectedIndexChanged);
            // 
            // YADI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 477);
            this.Controls.Add(this.injectionMethComboBox);
            this.Controls.Add(this.processSearchrLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.processListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DllPathText);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.InjectButton);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "YADI";
            this.Text = " ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InjectButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox DllPathText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ListBox processListBox;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label processSearchrLabel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ColorDialog colorDialog3;
        private System.Windows.Forms.ComboBox injectionMethComboBox;
    }
}

