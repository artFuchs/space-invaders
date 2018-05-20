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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.invader = new System.Windows.Forms.PictureBox();
            this.playerImage = new System.Windows.Forms.PictureBox();
            this.bullet = new System.Windows.Forms.PictureBox();
            this.GameOverLabel = new System.Windows.Forms.Label();
            this.Ready_Go_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.invader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bullet)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 33;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // invader
            // 
            this.invader.BackColor = System.Drawing.Color.Transparent;
            this.invader.Image = ((System.Drawing.Image)(resources.GetObject("invader.Image")));
            this.invader.Location = new System.Drawing.Point(0, 0);
            this.invader.Name = "invader";
            this.invader.Size = new System.Drawing.Size(32, 24);
            this.invader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.invader.TabIndex = 0;
            this.invader.TabStop = false;
            this.invader.Tag = "invader";
            // 
            // playerImage
            // 
            this.playerImage.BackColor = System.Drawing.Color.Transparent;
            this.playerImage.Image = ((System.Drawing.Image)(resources.GetObject("playerImage.Image")));
            this.playerImage.Location = new System.Drawing.Point(324, 482);
            this.playerImage.Name = "playerImage";
            this.playerImage.Size = new System.Drawing.Size(32, 24);
            this.playerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.playerImage.TabIndex = 3;
            this.playerImage.TabStop = false;
            this.playerImage.Tag = "player";
            // 
            // bullet
            // 
            this.bullet.BackColor = System.Drawing.Color.Transparent;
            this.bullet.Image = ((System.Drawing.Image)(resources.GetObject("bullet.Image")));
            this.bullet.Location = new System.Drawing.Point(593, 400);
            this.bullet.Name = "bullet";
            this.bullet.Size = new System.Drawing.Size(10, 24);
            this.bullet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.bullet.TabIndex = 4;
            this.bullet.TabStop = false;
            this.bullet.Tag = "bullet";
            // 
            // GameOverLabel
            // 
            this.GameOverLabel.AutoSize = true;
            this.GameOverLabel.BackColor = System.Drawing.Color.Transparent;
            this.GameOverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameOverLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.GameOverLabel.Location = new System.Drawing.Point(154, 236);
            this.GameOverLabel.Name = "GameOverLabel";
            this.GameOverLabel.Size = new System.Drawing.Size(363, 73);
            this.GameOverLabel.TabIndex = 5;
            this.GameOverLabel.Text = "Game Over";
            this.GameOverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Ready_Go_Label
            // 
            this.Ready_Go_Label.AutoSize = true;
            this.Ready_Go_Label.BackColor = System.Drawing.Color.Transparent;
            this.Ready_Go_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ready_Go_Label.ForeColor = System.Drawing.SystemColors.Control;
            this.Ready_Go_Label.Location = new System.Drawing.Point(47, 154);
            this.Ready_Go_Label.Name = "Ready_Go_Label";
            this.Ready_Go_Label.Size = new System.Drawing.Size(601, 73);
            this.Ready_Go_Label.TabIndex = 6;
            this.Ready_Go_Label.Text = "Press Enter To start";
            this.Ready_Go_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(673, 602);
            this.Controls.Add(this.Ready_Go_Label);
            this.Controls.Add(this.bullet);
            this.Controls.Add(this.playerImage);
            this.Controls.Add(this.invader);
            this.Controls.Add(this.GameOverLabel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameWindow";
            this.Text = "Space Invaders";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyisdown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyisup);
            ((System.ComponentModel.ISupportInitialize)(this.invader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bullet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox invader;
        private System.Windows.Forms.PictureBox playerImage;
        private System.Windows.Forms.PictureBox bullet;
        private System.Windows.Forms.Label GameOverLabel;
        private System.Windows.Forms.Label Ready_Go_Label;
    }
}

