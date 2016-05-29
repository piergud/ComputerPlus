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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_ped_db
            // 
            this.btn_ped_db.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ped_db.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_ped_db.Location = new System.Drawing.Point(72, 155);
            this.btn_ped_db.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_ped_db.Name = "btn_ped_db";
            this.btn_ped_db.Size = new System.Drawing.Size(125, 30);
            this.btn_ped_db.TabIndex = 0;
            this.btn_ped_db.Text = "Ped Database";
            this.btn_ped_db.UseVisualStyleBackColor = true;
            this.btn_ped_db.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_veh_db
            // 
            this.btn_veh_db.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_veh_db.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_veh_db.Location = new System.Drawing.Point(201, 155);
            this.btn_veh_db.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_veh_db.Name = "btn_veh_db";
            this.btn_veh_db.Size = new System.Drawing.Size(125, 30);
            this.btn_veh_db.TabIndex = 1;
            this.btn_veh_db.Text = "Vehicle Database";
            this.btn_veh_db.UseVisualStyleBackColor = true;
            this.btn_veh_db.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_logout
            // 
            this.btn_logout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_logout.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_logout.Location = new System.Drawing.Point(588, 155);
            this.btn_logout.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(125, 30);
            this.btn_logout.TabIndex = 2;
            this.btn_logout.Text = " Logout ";
            this.btn_logout.UseVisualStyleBackColor = true;
            this.btn_logout.Click += new System.EventHandler(this.button3_Click);
            // 
            // list_recent
            // 
            this.list_recent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.list_recent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.list_recent.FormattingEnabled = true;
            this.list_recent.Location = new System.Drawing.Point(15, 25);
            this.list_recent.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.list_recent.Name = "list_recent";
            this.list_recent.Size = new System.Drawing.Size(770, 121);
            this.list_recent.TabIndex = 3;
            this.list_recent.SelectedIndexChanged += new System.EventHandler(this.list_recent_SelectedIndexChanged);
            // 
            // btn_request
            // 
            this.btn_request.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_request.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_request.Location = new System.Drawing.Point(330, 155);
            this.btn_request.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_request.Name = "btn_request";
            this.btn_request.Size = new System.Drawing.Size(125, 30);
            this.btn_request.TabIndex = 4;
            this.btn_request.Text = "Request Backup";
            this.btn_request.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Most recent actions:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(459, 155);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "Active Calls";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ComputerMainTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(799, 197);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_request);
            this.Controls.Add(this.list_recent);
            this.Controls.Add(this.btn_logout);
            this.Controls.Add(this.btn_veh_db);
            this.Controls.Add(this.btn_ped_db);
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
        private Button button1;
    }
}