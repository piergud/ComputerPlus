namespace ComputerPlus
{
    partial class ComputerCurrentCallDetailsTemplate
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
            this.btn_help = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_help
            // 
            this.btn_help.Location = new System.Drawing.Point(550, 404);
            this.btn_help.Name = "btn_help";
            this.btn_help.Size = new System.Drawing.Size(88, 25);
            this.btn_help.TabIndex = 4;
            this.btn_help.Text = "Help";
            this.btn_help.UseVisualStyleBackColor = true;
            // 
            // ComputerCurrentCallDetailsTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 441);
            this.Controls.Add(this.btn_help);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputerCurrentCallDetailsTemplate";
            this.Text = "Call Details";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_help;
    }
}