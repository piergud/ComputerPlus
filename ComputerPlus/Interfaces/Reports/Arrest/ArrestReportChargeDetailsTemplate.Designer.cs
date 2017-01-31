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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_notes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddCharge = new System.Windows.Forms.Button();
            this.btnRemoveSelectedCharge = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_charges
            // 
            this.lb_charges.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lb_charges.FormattingEnabled = true;
            this.lb_charges.ItemHeight = 15;
            this.lb_charges.Location = new System.Drawing.Point(504, 72);
            this.lb_charges.Name = "lb_charges";
            this.lb_charges.Size = new System.Drawing.Size(165, 274);
            this.lb_charges.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(339, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Notes";
            // 
            // tb_notes
            // 
            this.tb_notes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tb_notes.Location = new System.Drawing.Point(329, 72);
            this.tb_notes.Multiline = true;
            this.tb_notes.Name = "tb_notes";
            this.tb_notes.Size = new System.Drawing.Size(165, 100);
            this.tb_notes.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Available Charges";
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
            this.btnRemoveSelectedCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnRemoveSelectedCharge.Location = new System.Drawing.Point(504, 356);
            this.btnRemoveSelectedCharge.Name = "btnRemoveSelectedCharge";
            this.btnRemoveSelectedCharge.Size = new System.Drawing.Size(165, 23);
            this.btnRemoveSelectedCharge.TabIndex = 4;
            this.btnRemoveSelectedCharge.Text = "Remove Selected";
            this.btnRemoveSelectedCharge.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(521, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Added Charges";
            // 
            // ArrestReportChargeDetailsTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 391);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRemoveSelectedCharge);
            this.Controls.Add(this.btnAddCharge);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_notes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddCharge;
        private System.Windows.Forms.Button btnRemoveSelectedCharge;
        private System.Windows.Forms.Label label3;
    }
}