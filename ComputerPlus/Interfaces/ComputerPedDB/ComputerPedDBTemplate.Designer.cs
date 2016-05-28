namespace ComputerPlus
{
    partial class ComputerPedDBTemplate
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
            this.btn_search = new System.Windows.Forms.Button();
            this.input_name = new System.Windows.Forms.TextBox();
            this.btn_main = new System.Windows.Forms.Button();
            this.output_info = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(318, 304);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(88, 25);
            this.btn_search.TabIndex = 0;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.button1_Click);
            // 
            // input_name
            // 
            this.input_name.Location = new System.Drawing.Point(12, 307);
            this.input_name.Name = "input_name";
            this.input_name.Size = new System.Drawing.Size(297, 20);
            this.input_name.TabIndex = 1;
            this.input_name.Text = "Enter the name of the suspect here.";
            this.input_name.TextChanged += new System.EventHandler(this.input_name_TextChanged);
            // 
            // btn_main
            // 
            this.btn_main.Location = new System.Drawing.Point(412, 304);
            this.btn_main.Name = "btn_main";
            this.btn_main.Size = new System.Drawing.Size(88, 25);
            this.btn_main.TabIndex = 3;
            this.btn_main.Text = "Main Menu";
            this.btn_main.UseVisualStyleBackColor = true;
            // 
            // output_info
            // 
            this.output_info.Location = new System.Drawing.Point(12, 12);
            this.output_info.Multiline = true;
            this.output_info.Name = "output_info";
            this.output_info.Size = new System.Drawing.Size(488, 286);
            this.output_info.TabIndex = 4;
            this.output_info.TextChanged += new System.EventHandler(this.output_info_TextChanged);
            // 
            // ComputerPedDBTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 339);
            this.Controls.Add(this.output_info);
            this.Controls.Add(this.btn_main);
            this.Controls.Add(this.input_name);
            this.Controls.Add(this.btn_search);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputerPedDBTemplate";
            this.Text = "Ped Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TextBox input_name;
        private System.Windows.Forms.Button btn_main;
        private System.Windows.Forms.TextBox output_info;
    }
}