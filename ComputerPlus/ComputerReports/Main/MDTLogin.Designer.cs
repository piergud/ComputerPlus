namespace ComputerPlus
{
    partial class MainForm
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
            this.HomeButton = new System.Windows.Forms.Button();
            this.ReportCitation = new System.Windows.Forms.Button();
            this.ReportArrest = new System.Windows.Forms.Button();
            this.FIButton = new System.Windows.Forms.Button();
            this.InfoLabel1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HomeButton
            // 
            this.HomeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.HomeButton.Location = new System.Drawing.Point(460, 155);
            this.HomeButton.Margin = new System.Windows.Forms.Padding(2);
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.Size = new System.Drawing.Size(125, 30);
            this.HomeButton.TabIndex = 13;
            this.HomeButton.TabStop = false;
            this.HomeButton.Text = "Main Menu";
            this.HomeButton.UseVisualStyleBackColor = true;
            // 
            // ReportCitation
            // 
            this.ReportCitation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ReportCitation.Location = new System.Drawing.Point(70, 155);
            this.ReportCitation.Name = "ReportCitation";
            this.ReportCitation.Size = new System.Drawing.Size(125, 30);
            this.ReportCitation.TabIndex = 115;
            this.ReportCitation.Text = "Citation Editor";
            this.ReportCitation.UseVisualStyleBackColor = true;
            // 
            // ReportArrest
            // 
            this.ReportArrest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ReportArrest.Location = new System.Drawing.Point(201, 155);
            this.ReportArrest.Name = "ReportArrest";
            this.ReportArrest.Size = new System.Drawing.Size(125, 30);
            this.ReportArrest.TabIndex = 116;
            this.ReportArrest.Text = "Arrest Report";
            this.ReportArrest.UseVisualStyleBackColor = true;
            // 
            // FIButton
            // 
            this.FIButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.FIButton.Location = new System.Drawing.Point(331, 155);
            this.FIButton.Margin = new System.Windows.Forms.Padding(2);
            this.FIButton.Name = "FIButton";
            this.FIButton.Size = new System.Drawing.Size(125, 30);
            this.FIButton.TabIndex = 117;
            this.FIButton.TabStop = false;
            this.FIButton.Text = "Field Interviews";
            this.FIButton.UseVisualStyleBackColor = true;
            // 
            // InfoLabel1
            // 
            this.InfoLabel1.AutoSize = true;
            this.InfoLabel1.BackColor = System.Drawing.Color.Transparent;
            this.InfoLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.InfoLabel1.ForeColor = System.Drawing.Color.Black;
            this.InfoLabel1.Location = new System.Drawing.Point(122, 9);
            this.InfoLabel1.Name = "InfoLabel1";
            this.InfoLabel1.Size = new System.Drawing.Size(433, 17);
            this.InfoLabel1.TabIndex = 118;
            this.InfoLabel1.Text = "Welcome to the report writing system.  Please select a report below!";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(667, 197);
            this.Controls.Add(this.InfoLabel1);
            this.Controls.Add(this.FIButton);
            this.Controls.Add(this.ReportArrest);
            this.Controls.Add(this.ReportCitation);
            this.Controls.Add(this.HomeButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(683, 236);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Report Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button HomeButton;
        private System.Windows.Forms.Button ReportCitation;
        private System.Windows.Forms.Button ReportArrest;
        private System.Windows.Forms.Button FIButton;
        private System.Windows.Forms.Label InfoLabel1;
    }
}