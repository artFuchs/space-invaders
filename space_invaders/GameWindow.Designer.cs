namespace space_invaders
{
    partial class GameWindow
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
            this.start_button = new System.Windows.Forms.Button();
            this.help_button = new System.Windows.Forms.Button();
            this.quit_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // start_button
            // 
            this.start_button.BackColor = System.Drawing.Color.SeaShell;
            this.start_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.start_button.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.start_button.Location = new System.Drawing.Point(320, 226);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(124, 45);
            this.start_button.TabIndex = 0;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = false;
            this.start_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // help_button
            // 
            this.help_button.BackColor = System.Drawing.Color.SeaShell;
            this.help_button.Cursor = System.Windows.Forms.Cursors.Default;
            this.help_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.help_button.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.help_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.help_button.Location = new System.Drawing.Point(320, 277);
            this.help_button.Name = "help_button";
            this.help_button.Size = new System.Drawing.Size(124, 45);
            this.help_button.TabIndex = 1;
            this.help_button.Text = "Help";
            this.help_button.UseVisualStyleBackColor = false;
            // 
            // quit_button
            // 
            this.quit_button.BackColor = System.Drawing.Color.SeaShell;
            this.quit_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quit_button.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quit_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.quit_button.Location = new System.Drawing.Point(320, 328);
            this.quit_button.Name = "quit_button";
            this.quit_button.Size = new System.Drawing.Size(124, 45);
            this.quit_button.TabIndex = 2;
            this.quit_button.Text = "Quit";
            this.quit_button.UseVisualStyleBackColor = false;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::space_invaders.Properties.Resources.logo1;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.quit_button);
            this.Controls.Add(this.help_button);
            this.Controls.Add(this.start_button);
            this.Name = "GameWindow";
            this.Text = "Space Invaders";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Button help_button;
        private System.Windows.Forms.Button quit_button;
    }
}

