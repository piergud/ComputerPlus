namespace ComputerPlus
{
    partial class ComputerVehDBTemplate
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
            this.output_info = new System.Windows.Forms.TextBox();
            this.input_name = new System.Windows.Forms.TextBox();
            this.btn_main = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // output_info
            // 
            this.output_info.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.output_info.Location = new System.Drawing.Point(12, 12);
            this.output_info.Multiline = true;
            this.output_info.Name = "output_info";
            this.output_info.Size = new System.Drawing.Size(488, 286);
            this.output_info.TabIndex = 5;
            this.output_info.TextChanged += new System.EventHandler(this.output_info_TextChanged);
            // 
            // input_name
            // 
            this.input_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.input_name.Location = new System.Drawing.Point(12, 307);
            this.input_name.Name = "input_name";
            this.input_name.Size = new System.Drawing.Size(297, 20);
            this.input_name.TabIndex = 6;
            this.input_name.Text = "Enter the license plate of the vehicle here.";
            // 
            // btn_main
            // 
            this.btn_main.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_main.Location = new System.Drawing.Point(412, 304);
            this.btn_main.Name = "btn_main";
            this.btn_main.Size = new System.Drawing.Size(88, 25);
            this.btn_main.TabIndex = 8;
            this.btn_main.Text = "Main Menu";
            this.btn_main.UseVisualStyleBackColor = true;
            // 
            // btn_search
            // 
            this.btn_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_search.Location = new System.Drawing.Point(318, 304);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(88, 25);
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            // 
            // ComputerVehDBTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 339);
            this.Controls.Add(this.btn_main);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.input_name);
            this.Controls.Add(this.output_info);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputerVehDBTemplate";
            this.Text = "Vehicle Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox output_info;
        private System.Windows.Forms.TextBox input_name;
        private System.Windows.Forms.Button btn_main;
        private System.Windows.Forms.Button btn_search;
    }
}