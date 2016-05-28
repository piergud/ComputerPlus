namespace ComputerPlus
{
    partial class ComputerLoginTemplate
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.input_user = new System.Windows.Forms.TextBox();
            this.input_pass = new System.Windows.Forms.TextBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.label_invalid = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(181, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please login below to access this computer.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // input_user
            // 
            this.input_user.Location = new System.Drawing.Point(166, 63);
            this.input_user.Name = "input_user";
            this.input_user.Size = new System.Drawing.Size(260, 20);
            this.input_user.TabIndex = 3;
            this.input_user.Text = "PieRGud";
            this.input_user.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // input_pass
            // 
            this.input_pass.Location = new System.Drawing.Point(166, 89);
            this.input_pass.Name = "input_pass";
            this.input_pass.Size = new System.Drawing.Size(260, 20);
            this.input_pass.TabIndex = 4;
            this.input_pass.Text = "DoNuTz";
            this.input_pass.UseSystemPasswordChar = true;
            this.input_pass.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(242, 139);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(83, 23);
            this.btn_login.TabIndex = 5;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_invalid
            // 
            this.label_invalid.AutoSize = true;
            this.label_invalid.ForeColor = System.Drawing.Color.Red;
            this.label_invalid.Location = new System.Drawing.Point(162, 118);
            this.label_invalid.Name = "label_invalid";
            this.label_invalid.Size = new System.Drawing.Size(254, 13);
            this.label_invalid.TabIndex = 6;
            this.label_invalid.Text = "Invalid username and/or password. Please try again.";
            this.label_invalid.Visible = false;
            // 
            // ComputerLoginTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 174);
            this.Controls.Add(this.label_invalid);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.input_pass);
            this.Controls.Add(this.input_user);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputerLoginTemplate";
            this.Text = "MDT Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox input_user;
        private System.Windows.Forms.TextBox input_pass;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.Label label_invalid;
    }
}