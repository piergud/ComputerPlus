namespace ComputerPlus.Interfaces.ComputerPedDB
{
    partial class ComputerPedSearchTemplate
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
            this.list_manual_results = new System.Windows.Forms.ListBox();
            this.text_manual_name = new System.Windows.Forms.TextBox();
            this.list_collected_ids = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // list_manual_results
            // 
            this.list_manual_results.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.list_manual_results.FormattingEnabled = true;
            this.list_manual_results.ItemHeight = 17;
            this.list_manual_results.Location = new System.Drawing.Point(385, 71);
            this.list_manual_results.Margin = new System.Windows.Forms.Padding(2);
            this.list_manual_results.Name = "list_manual_results";
            this.list_manual_results.Size = new System.Drawing.Size(265, 123);
            this.list_manual_results.TabIndex = 1;
            // 
            // text_manual_name
            // 
            this.text_manual_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.text_manual_name.Location = new System.Drawing.Point(385, 37);
            this.text_manual_name.Margin = new System.Windows.Forms.Padding(2);
            this.text_manual_name.Name = "text_manual_name";
            this.text_manual_name.Size = new System.Drawing.Size(265, 21);
            this.text_manual_name.TabIndex = 0;
            // 
            // list_collected_ids
            // 
            this.list_collected_ids.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.list_collected_ids.FormattingEnabled = true;
            this.list_collected_ids.ItemHeight = 17;
            this.list_collected_ids.Location = new System.Drawing.Point(11, 37);
            this.list_collected_ids.Margin = new System.Windows.Forms.Padding(2);
            this.list_collected_ids.Name = "list_collected_ids";
            this.list_collected_ids.Size = new System.Drawing.Size(265, 157);
            this.list_collected_ids.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Collected IDs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(381, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Manual Search";
            // 
            // ComputerPedSearchTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 231);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.list_collected_ids);
            this.Controls.Add(this.text_manual_name);
            this.Controls.Add(this.list_manual_results);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ComputerPedSearchTemplate";
            this.Text = "Ped Lookup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox list_manual_results;
        private System.Windows.Forms.TextBox text_manual_name;
        private System.Windows.Forms.ListBox list_collected_ids;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}