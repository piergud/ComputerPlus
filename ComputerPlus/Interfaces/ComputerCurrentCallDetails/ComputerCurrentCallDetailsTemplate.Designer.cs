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
            this.btn_main = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_main
            // 
            this.btn_main.Location = new System.Drawing.Point(412, 308);
            this.btn_main.Name = "btn_main";
            this.btn_main.Size = new System.Drawing.Size(88, 25);
            this.btn_main.TabIndex = 3;
            this.btn_main.Text = "Main Menu";
            this.btn_main.UseVisualStyleBackColor = true;
            // 
            // ComputerCurrentCallDetailsTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 340);
            this.Controls.Add(this.btn_main);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputerCurrentCallDetailsTemplate";
            this.Text = "Call Details";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_main;
    }
}