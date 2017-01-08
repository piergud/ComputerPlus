namespace ComputerPlus
{
    using LSPD_First_Response.Mod.API;
    partial class ComputerRequestBackupTemplate
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
            this.dropdown_resp = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.list_unit = new System.Windows.Forms.ListBox();
            this.btn_request = new System.Windows.Forms.Button();
            this.text_resp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dropdown_resp
            // 
            this.dropdown_resp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdown_resp.FormattingEnabled = true;
            this.dropdown_resp.Location = new System.Drawing.Point(12, 25);
            this.dropdown_resp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dropdown_resp.Name = "dropdown_resp";
            this.dropdown_resp.Size = new System.Drawing.Size(383, 21);
            this.dropdown_resp.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Response Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Unit Type:";
            // 
            // list_unit
            // 
            this.list_unit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.list_unit.FormattingEnabled = true;
            this.list_unit.Location = new System.Drawing.Point(12, 74);
            this.list_unit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.list_unit.Name = "list_unit";
            this.list_unit.Size = new System.Drawing.Size(383, 134);
            this.list_unit.TabIndex = 3;
            this.list_unit.SelectedIndexChanged += new System.EventHandler(this.list_unit_SelectedIndexChanged);
            // 
            // btn_request
            // 
            this.btn_request.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_request.Location = new System.Drawing.Point(108, 235);
            this.btn_request.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_request.Name = "btn_request";
            this.btn_request.Size = new System.Drawing.Size(166, 30);
            this.btn_request.TabIndex = 4;
            this.btn_request.Text = "Request Unit";
            this.btn_request.UseVisualStyleBackColor = true;
            this.btn_request.Click += new System.EventHandler(this.button1_Click);
            // 
            // text_resp
            // 
            this.text_resp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.text_resp.Location = new System.Drawing.Point(12, 211);
            this.text_resp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.text_resp.Name = "text_resp";
            this.text_resp.Size = new System.Drawing.Size(383, 21);
            this.text_resp.TabIndex = 6;
            this.text_resp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.text_resp.Click += new System.EventHandler(this.label3_Click);
            // 
            // ComputerRequestBackupTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 277);
            this.Controls.Add(this.text_resp);
            this.Controls.Add(this.btn_request);
            this.Controls.Add(this.list_unit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dropdown_resp);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputerRequestBackupTemplate";
            this.Text = "Request Backup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dropdown_resp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox list_unit;
        private System.Windows.Forms.Button btn_request;
        private System.Windows.Forms.Label text_resp;
    }
}