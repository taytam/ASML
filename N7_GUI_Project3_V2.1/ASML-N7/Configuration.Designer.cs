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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // File_Text
            // 
            this.File_Text.Location = new System.Drawing.Point(65, 16);
            this.File_Text.Name = "File_Text";
            this.File_Text.Size = new System.Drawing.Size(245, 20);
            this.File_Text.TabIndex = 1;
            this.File_Text.TextChanged += new System.EventHandler(this.File_Text_TextChanged);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(148, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 27);
            this.button1.TabIndex = 4;
            this.button1.Text = "Open File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Open_File);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(232, 47);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 27);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Cancel_Button);
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 92);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}