namespace ComputerPlus.Interfaces.Reports.Arrest
{
    partial class ArrestReportChargeDetailsTemplate
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
            this.lb_charges = new System.Windows.Forms.ListBox();
            this.lbl_notes = new System.Windows.Forms.Label();
            this.tb_notes = new System.Windows.Forms.TextBox();
            this.lbl_availableCharges = new System.Windows.Forms.Label();
            this.btnAddCharge = new System.Windows.Forms.Button();
            this.btnRemoveSelectedCharge = new System.Windows.Forms.Button();
            this.lbl_addedCharges = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_charges
            // 
            this.lb_charges.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lb_charges.FormattingEnabled = true;
            this.lb_charges.ItemHeight = 15;
            this.lb_charges.Location = new System.Drawing.Point(504, 42);
            this.lb_charges.Name = "lb_charges";
            this.lb_charges.Size = new System.Drawing.Size(165, 304);
            this.lb_charges.TabIndex = 3;
            // 
            // lbl_notes
            // 
            this.lbl_notes.AutoSize = true;
            this.lbl_notes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_notes.Location = new System.Drawing.Point(326, 9);
            this.lbl_notes.Name = "lbl_notes";
            this.lbl_notes.Size = new System.Drawing.Size(99, 15);
            this.lbl_notes.TabIndex = 6;
            this.lbl_notes.Text = "Notes for Charge";
            // 
            // tb_notes
            // 
            this.tb_notes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tb_notes.Location = new System.Drawing.Point(329, 42);
            this.tb_notes.Multiline = true;
            this.tb_notes.Name = "tb_notes";
            this.tb_notes.Size = new System.Drawing.Size(165, 130);
            this.tb_notes.TabIndex = 1;
            // 
            // lbl_availableCharges
            // 
            this.lbl_availableCharges.AutoSize = true;
            this.lbl_availableCharges.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_availableCharges.Location = new System.Drawing.Point(12, 9);
            this.lbl_availableCharges.Name = "lbl_availableCharges";
            this.lbl_availableCharges.Size = new System.Drawing.Size(105, 15);
            this.lbl_availableCharges.TabIndex = 8;
            this.lbl_availableCharges.Text = "Available Charges";
            // 
            // btnAddCharge
            // 
            this.btnAddCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnAddCharge.Location = new System.Drawing.Point(329, 204);
            this.btnAddCharge.Name = "btnAddCharge";
            this.btnAddCharge.Size = new System.Drawing.Size(169, 23);
            this.btnAddCharge.TabIndex = 2;
            this.btnAddCharge.Text = "Add Charge";
            this.btnAddCharge.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSelectedCharge
            // 
            this.btnRemoveSelectedCharge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveSelectedCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnRemoveSelectedCharge.Location = new System.Drawing.Point(504, 352);
            this.btnRemoveSelectedCharge.Name = "btnRemoveSelectedCharge";
            this.btnRemoveSelectedCharge.Size = new System.Drawing.Size(44, 35);
            this.btnRemoveSelectedCharge.TabIndex = 4;
            this.btnRemoveSelectedCharge.Text = "Delete";
            this.btnRemoveSelectedCharge.UseVisualStyleBackColor = true;
            // 
            // lbl_addedCharges
            // 
            this.lbl_addedCharges.AutoSize = true;
            this.lbl_addedCharges.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_addedCharges.Location = new System.Drawing.Point(501, 9);
            this.lbl_addedCharges.Name = "lbl_addedCharges";
            this.lbl_addedCharges.Size = new System.Drawing.Size(91, 15);
            this.lbl_addedCharges.TabIndex = 11;
            this.lbl_addedCharges.Text = "Added Charges";
            // 
            // ArrestReportChargeDetailsTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 391);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_addedCharges);
            this.Controls.Add(this.btnRemoveSelectedCharge);
            this.Controls.Add(this.btnAddCharge);
            this.Controls.Add(this.lbl_availableCharges);
            this.Controls.Add(this.lbl_notes);
            this.Controls.Add(this.tb_notes);
            this.Controls.Add(this.lb_charges);
            this.Name = "ArrestReportChargeDetailsTemplate";
            this.ShowIcon = false;
            this.Text = "Charge Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_charges;
        private System.Windows.Forms.Label lbl_notes;
        private System.Windows.Forms.TextBox tb_notes;
        private System.Windows.Forms.Label lbl_availableCharges;
        private System.Windows.Forms.Button btnAddCharge;
        private System.Windows.Forms.Button btnRemoveSelectedCharge;
        private System.Windows.Forms.Label lbl_addedCharges;
    }
}