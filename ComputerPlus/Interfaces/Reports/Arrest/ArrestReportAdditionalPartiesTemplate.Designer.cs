namespace ComputerPlus.Interfaces.Reports.Arrest
{
    partial class ArrestReportAdditionalPartiesTemplate
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
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_dob = new System.Windows.Forms.Label();
            this.lbl_last_name = new System.Windows.Forms.Label();
            this.lbl_first_name = new System.Windows.Forms.Label();
            this.lb_additional_parties = new System.Windows.Forms.ListBox();
            this.lbl_party_type = new System.Windows.Forms.Label();
            this.cb_type = new System.Windows.Forms.ComboBox();
            this.btn_addPartyToReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(24, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 17);
            this.label8.TabIndex = 49;
            this.label8.Text = "Information";
            // 
            // lbl_dob
            // 
            this.lbl_dob.AutoSize = true;
            this.lbl_dob.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_dob.Location = new System.Drawing.Point(26, 169);
            this.lbl_dob.Name = "lbl_dob";
            this.lbl_dob.Size = new System.Drawing.Size(38, 17);
            this.lbl_dob.TabIndex = 42;
            this.lbl_dob.Text = "DOB";
            // 
            // lbl_last_name
            // 
            this.lbl_last_name.AutoSize = true;
            this.lbl_last_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_last_name.Location = new System.Drawing.Point(26, 129);
            this.lbl_last_name.Name = "lbl_last_name";
            this.lbl_last_name.Size = new System.Drawing.Size(76, 17);
            this.lbl_last_name.TabIndex = 41;
            this.lbl_last_name.Text = "Last Name";
            // 
            // lbl_first_name
            // 
            this.lbl_first_name.AutoSize = true;
            this.lbl_first_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_first_name.Location = new System.Drawing.Point(25, 94);
            this.lbl_first_name.Name = "lbl_first_name";
            this.lbl_first_name.Size = new System.Drawing.Size(76, 17);
            this.lbl_first_name.TabIndex = 40;
            this.lbl_first_name.Text = "First Name";
            // 
            // lb_additional_parties
            // 
            this.lb_additional_parties.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lb_additional_parties.FormattingEnabled = true;
            this.lb_additional_parties.ItemHeight = 17;
            this.lb_additional_parties.Location = new System.Drawing.Point(499, 12);
            this.lb_additional_parties.Name = "lb_additional_parties";
            this.lb_additional_parties.Size = new System.Drawing.Size(165, 293);
            this.lb_additional_parties.TabIndex = 50;
            // 
            // lbl_party_type
            // 
            this.lbl_party_type.AutoSize = true;
            this.lbl_party_type.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_party_type.Location = new System.Drawing.Point(25, 56);
            this.lbl_party_type.Name = "lbl_party_type";
            this.lbl_party_type.Size = new System.Drawing.Size(77, 17);
            this.lbl_party_type.TabIndex = 51;
            this.lbl_party_type.Text = "Party Type";
            // 
            // cb_type
            // 
            this.cb_type.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cb_type.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cb_type.FormattingEnabled = true;
            this.cb_type.Location = new System.Drawing.Point(111, 56);
            this.cb_type.Name = "cb_type";
            this.cb_type.Size = new System.Drawing.Size(153, 25);
            this.cb_type.TabIndex = 52;
            this.cb_type.Text = "Select One";
            // 
            // btn_addPartyToReport
            // 
            this.btn_addPartyToReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_addPartyToReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btn_addPartyToReport.Location = new System.Drawing.Point(28, 213);
            this.btn_addPartyToReport.Name = "btn_addPartyToReport";
            this.btn_addPartyToReport.Size = new System.Drawing.Size(44, 35);
            this.btn_addPartyToReport.TabIndex = 56;
            this.btn_addPartyToReport.Text = "Add";
            this.btn_addPartyToReport.UseVisualStyleBackColor = true;
            // 
            // ArrestReportAdditionalPartiesTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 391);
            this.Controls.Add(this.btn_addPartyToReport);
            this.Controls.Add(this.cb_type);
            this.Controls.Add(this.lbl_party_type);
            this.Controls.Add(this.lb_additional_parties);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lbl_dob);
            this.Controls.Add(this.lbl_last_name);
            this.Controls.Add(this.lbl_first_name);
            this.Name = "ArrestReportAdditionalPartiesTemplate";
            this.Text = "Additional Parties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_dob;
        private System.Windows.Forms.Label lbl_last_name;
        private System.Windows.Forms.Label lbl_first_name;
        private System.Windows.Forms.ListBox lb_additional_parties;
        private System.Windows.Forms.Label lbl_party_type;
        private System.Windows.Forms.ComboBox cb_type;
        private System.Windows.Forms.Button btn_addPartyToReport;
    }
}