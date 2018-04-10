namespace ComputerPlus
{
    using System.Drawing;
    using System.Windows.Forms;

    partial class ComputerMainTemplate
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
            this.btn_ped_db = new System.Windows.Forms.Button();
            this.btn_veh_db = new System.Windows.Forms.Button();
            this.btn_logout = new System.Windows.Forms.Button();
            this.list_recent = new System.Windows.Forms.ListBox();
            this.btn_request = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_activecalls = new System.Windows.Forms.Button();
            this.label_external_ui = new System.Windows.Forms.Label();
            this.list_external_ui = new System.Windows.Forms.ComboBox();
            this.cb_toggle_pause = new System.Windows.Forms.CheckBox();
            this.cb_toggle_background = new System.Windows.Forms.CheckBox();
            this.btn_citation_history = new System.Windows.Forms.Button();
            this.btn_notepad = new System.Windows.Forms.Button();
            this.btn_arrest_history = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_ped_db
            // 
            this.btn_ped_db.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ped_db.BackColor = System.Drawing.Color.Transparent;
            this.btn_ped_db.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_ped_db.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btn_ped_db.Location = new System.Drawing.Point(142, 176);
            this.btn_ped_db.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_ped_db.Name = "btn_ped_db";
            this.btn_ped_db.Size = new System.Drawing.Size(120, 30);
            this.btn_ped_db.TabIndex = 0;
            this.btn_ped_db.Text = "Ped Database";
            this.btn_ped_db.UseVisualStyleBackColor = false;
            this.btn_ped_db.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_veh_db
            // 
            this.btn_veh_db.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_veh_db.BackColor = System.Drawing.Color.Transparent;
            this.btn_veh_db.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_veh_db.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btn_veh_db.Location = new System.Drawing.Point(266, 176);
            this.btn_veh_db.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_veh_db.Name = "btn_veh_db";
            this.btn_veh_db.Size = new System.Drawing.Size(120, 30);
            this.btn_veh_db.TabIndex = 1;
            this.btn_veh_db.Text = "Vehicle Database";
            this.btn_veh_db.UseVisualStyleBackColor = false;
            this.btn_veh_db.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_logout
            // 
            this.btn_logout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_logout.BackColor = System.Drawing.Color.Transparent;
            this.btn_logout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_logout.ForeColor = System.Drawing.Color.Red;
            this.btn_logout.Location = new System.Drawing.Point(514, 213);
            this.btn_logout.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(120, 30);
            this.btn_logout.TabIndex = 2;
            this.btn_logout.Text = " Logout ";
            this.btn_logout.UseVisualStyleBackColor = false;
            this.btn_logout.Click += new System.EventHandler(this.button3_Click);
            // 
            // list_recent
            // 
            this.list_recent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.list_recent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.list_recent.FormattingEnabled = true;
            this.list_recent.Location = new System.Drawing.Point(15, 32);
            this.list_recent.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.list_recent.Name = "list_recent";
            this.list_recent.Size = new System.Drawing.Size(619, 134);
            this.list_recent.TabIndex = 3;
            this.list_recent.SelectedIndexChanged += new System.EventHandler(this.list_recent_SelectedIndexChanged);
            // 
            // btn_request
            // 
            this.btn_request.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_request.BackColor = System.Drawing.Color.Transparent;
            this.btn_request.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_request.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn_request.Location = new System.Drawing.Point(390, 176);
            this.btn_request.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_request.Name = "btn_request";
            this.btn_request.Size = new System.Drawing.Size(120, 30);
            this.btn_request.TabIndex = 4;
            this.btn_request.Text = "Request Backup";
            this.btn_request.UseVisualStyleBackColor = false;
            this.btn_request.Click += new System.EventHandler(this.btn_request_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Most recent actions:";
            // 
            // btn_activecalls
            // 
            this.btn_activecalls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_activecalls.BackColor = System.Drawing.Color.Transparent;
            this.btn_activecalls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_activecalls.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn_activecalls.Location = new System.Drawing.Point(514, 176);
            this.btn_activecalls.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_activecalls.Name = "btn_activecalls";
            this.btn_activecalls.Size = new System.Drawing.Size(120, 30);
            this.btn_activecalls.TabIndex = 6;
            this.btn_activecalls.Text = "Call Details";
            this.btn_activecalls.UseVisualStyleBackColor = false;
            // 
            // label_external_ui
            // 
            this.label_external_ui.AutoSize = true;
            this.label_external_ui.Location = new System.Drawing.Point(202, 8);
            this.label_external_ui.Name = "label_external_ui";
            this.label_external_ui.Size = new System.Drawing.Size(36, 13);
            this.label_external_ui.TabIndex = 7;
            this.label_external_ui.Text = "Extras";
            this.label_external_ui.Visible = false;
            // 
            // list_external_ui
            // 
            this.list_external_ui.FormattingEnabled = true;
            this.list_external_ui.Location = new System.Drawing.Point(244, 5);
            this.list_external_ui.Name = "list_external_ui";
            this.list_external_ui.Size = new System.Drawing.Size(312, 21);
            this.list_external_ui.TabIndex = 8;
            this.list_external_ui.Visible = false;
            // 
            // cb_toggle_pause
            // 
            this.cb_toggle_pause.Location = new System.Drawing.Point(15, 203);
            this.cb_toggle_pause.Name = "cb_toggle_pause";
            this.cb_toggle_pause.Size = new System.Drawing.Size(120, 17);
            this.cb_toggle_pause.TabIndex = 9;
            this.cb_toggle_pause.Text = "Toggle Pause";
            this.cb_toggle_pause.UseVisualStyleBackColor = true;
            this.cb_toggle_pause.CheckedChanged += new System.EventHandler(this.cb_toggle_pause_CheckedChanged);
            // 
            // cb_toggle_background
            // 
            this.cb_toggle_background.Location = new System.Drawing.Point(15, 226);
            this.cb_toggle_background.Name = "cb_toggle_background";
            this.cb_toggle_background.Size = new System.Drawing.Size(120, 17);
            this.cb_toggle_background.TabIndex = 10;
            this.cb_toggle_background.Text = "Toggle Background";
            this.cb_toggle_background.UseVisualStyleBackColor = true;
            // 
            // btn_citation_history
            // 
            this.btn_citation_history.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_citation_history.BackColor = System.Drawing.Color.Transparent;
            this.btn_citation_history.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_citation_history.ForeColor = System.Drawing.Color.Maroon;
            this.btn_citation_history.Location = new System.Drawing.Point(187, 213);
            this.btn_citation_history.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_citation_history.Name = "btn_citation_history";
            this.btn_citation_history.Size = new System.Drawing.Size(161, 30);
            this.btn_citation_history.TabIndex = 11;
            this.btn_citation_history.Text = "Citation History";
            this.btn_citation_history.UseVisualStyleBackColor = false;
            // 
            // btn_notepad
            // 
            this.btn_notepad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_notepad.BackColor = System.Drawing.Color.Transparent;
            this.btn_notepad.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_notepad.ForeColor = System.Drawing.Color.Teal;
            this.btn_notepad.Location = new System.Drawing.Point(576, 4);
            this.btn_notepad.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_notepad.Name = "btn_notepad";
            this.btn_notepad.Size = new System.Drawing.Size(59, 22);
            this.btn_notepad.TabIndex = 11;
            this.btn_notepad.Text = "Notepad";
            this.btn_notepad.UseVisualStyleBackColor = false;
            // 
            // btn_arrest_history
            // 
            this.btn_arrest_history.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_arrest_history.BackColor = System.Drawing.Color.Transparent;
            this.btn_arrest_history.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_arrest_history.ForeColor = System.Drawing.Color.Maroon;
            this.btn_arrest_history.Location = new System.Drawing.Point(352, 213);
            this.btn_arrest_history.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_arrest_history.Name = "btn_arrest_history";
            this.btn_arrest_history.Size = new System.Drawing.Size(158, 30);
            this.btn_arrest_history.TabIndex = 12;
            this.btn_arrest_history.Text = "Arrest Report History";
            this.btn_arrest_history.UseVisualStyleBackColor = false;
            // 
            // ComputerMainTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(645, 256);
            this.Controls.Add(this.btn_arrest_history);
            this.Controls.Add(this.btn_notepad);
            this.Controls.Add(this.btn_citation_history);
            this.Controls.Add(this.cb_toggle_background);
            this.Controls.Add(this.cb_toggle_pause);
            this.Controls.Add(this.list_external_ui);
            this.Controls.Add(this.label_external_ui);
            this.Controls.Add(this.btn_activecalls);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_request);
            this.Controls.Add(this.btn_logout);
            this.Controls.Add(this.btn_veh_db);
            this.Controls.Add(this.btn_ped_db);
            this.Controls.Add(this.list_recent);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputerMainTemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Menu";
            this.Load += new System.EventHandler(this.Computer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ped_db;
        private System.Windows.Forms.Button btn_veh_db;
        private System.Windows.Forms.Button btn_logout;
        private Button btn_request;
        private Label label1;
        private ListBox list_recent;
        private Button btn_activecalls;
        private Label label_external_ui;
        private ComboBox list_external_ui;
        private CheckBox cb_toggle_pause;
        private CheckBox cb_toggle_background;
        private Button btn_notepad;
        private Button btn_citation_history;
        private Button btn_arrest_history;
    }
}