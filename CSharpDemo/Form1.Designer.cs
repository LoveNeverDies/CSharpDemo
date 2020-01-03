namespace CSharpDemo
{
    partial class Form1
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
            this.DownloadSmallSpeak = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DownloadSmallSpeak
            // 
            this.DownloadSmallSpeak.Location = new System.Drawing.Point(713, 415);
            this.DownloadSmallSpeak.Name = "DownloadSmallSpeak";
            this.DownloadSmallSpeak.Size = new System.Drawing.Size(75, 23);
            this.DownloadSmallSpeak.TabIndex = 0;
            this.DownloadSmallSpeak.Text = "下载小说";
            this.DownloadSmallSpeak.UseVisualStyleBackColor = true;
            this.DownloadSmallSpeak.Click += new System.EventHandler(this.DownloadSmallSpeak_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DownloadSmallSpeak);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DownloadSmallSpeak;
    }
}