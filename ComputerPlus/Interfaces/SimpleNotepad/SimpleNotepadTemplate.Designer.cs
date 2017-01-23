namespace ComputerPlus.Interfaces.SimpleNotepad
{
    partial class SimpleNotepadTemplate
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
            this.tb_notes = new System.Windows.Forms.TextBox();
            this.cb_toggle_pause = new System.Windows.Forms.CheckBox();
            this.btn_erase = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_notes
            // 
            this.tb_notes.Dock = System.Windows.Forms.DockStyle.Top;
            this.tb_notes.Location = new System.Drawing.Point(0, 0);
            this.tb_notes.Multiline = true;
            this.tb_notes.Name = "tb_notes";
            this.tb_notes.Size = new System.Drawing.Size(422, 489);
            this.tb_notes.TabIndex = 0;
            // 
            // cb_toggle_pause
            // 
            this.cb_toggle_pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_toggle_pause.AutoSize = true;
            this.cb_toggle_pause.Location = new System.Drawing.Point(12, 495);
            this.cb_toggle_pause.Name = "cb_toggle_pause";
            this.cb_toggle_pause.Size = new System.Drawing.Size(102, 19);
            this.cb_toggle_pause.TabIndex = 10;
            this.cb_toggle_pause.Text = "Toggle Pause";
            this.cb_toggle_pause.UseVisualStyleBackColor = true;
            // 
            // btn_erase
            // 
            this.btn_erase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_erase.Location = new System.Drawing.Point(335, 492);
            this.btn_erase.Name = "btn_erase";
            this.btn_erase.Size = new System.Drawing.Size(75, 23);
            this.btn_erase.TabIndex = 11;
            this.btn_erase.Text = "Erase";
            this.btn_erase.UseVisualStyleBackColor = true;
            // 
            // SimpleNotepadTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 526);
            this.Controls.Add(this.btn_erase);
            this.Controls.Add(this.cb_toggle_pause);
            this.Controls.Add(this.tb_notes);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SimpleNotepadTemplate";
            this.ShowIcon = false;
            this.Text = "Notepad";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_notes;
        private System.Windows.Forms.CheckBox cb_toggle_pause;
        private System.Windows.Forms.Button btn_erase;
    }
}