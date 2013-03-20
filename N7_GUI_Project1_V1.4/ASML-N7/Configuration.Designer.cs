namespace ASML_N7
{
    partial class Configuration
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
            this.File_Text = new System.Windows.Forms.TextBox();
            this.File_Select = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // File_Text
            // 
            this.File_Text.Location = new System.Drawing.Point(65, 16);
            this.File_Text.Name = "File_Text";
            this.File_Text.Size = new System.Drawing.Size(193, 20);
            this.File_Text.TabIndex = 1;
            // 
            // File_Select
            // 
            this.File_Select.Location = new System.Drawing.Point(64, 47);
            this.File_Select.Name = "File_Select";
            this.File_Select.Size = new System.Drawing.Size(78, 27);
            this.File_Select.TabIndex = 2;
            this.File_Select.Text = "Select File";
            this.File_Select.UseVisualStyleBackColor = true;
            this.File_Select.Click += new System.EventHandler(this.Select_File);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "File:";
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.File_Select);
            this.Controls.Add(this.File_Text);
            this.Name = "Configuration";
            this.Text = "Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox File_Text;
        private System.Windows.Forms.Button File_Select;
        private System.Windows.Forms.Label label1;
    }
}