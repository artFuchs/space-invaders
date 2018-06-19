using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using space_invaders.core;

namespace space_invaders
{
    public partial class GameWindow : Form
    {
        

        GameInstance game;
        bool goleft;
        bool goright;
        bool isPressed;


        public GameWindow()
        {
            
            this.KeyPreview = true;

            InitializeComponent();

            //sound ~ BGM.wav file is on the same folder as the application .exe
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string fullpath = Path.Combine(directory, "BGM.wav");
            if (File.Exists(fullpath))
            {
                System.Media.SoundPlayer bgm = new System.Media.SoundPlayer(fullpath);
                bgm.PlayLooping();
            }

            GameOverLabel.Hide();
            Ready_Go_Label.Left = (Size.Width - Ready_Go_Label.Width) / 2;
            
            invader.Left = -invader.Width;
            bullet.Left = -bullet.Width;
            
        }

        private void setGame()
        {
            Dictionary<GameInstance.SizeOf, core.Size> dictSizes = new Dictionary<GameInstance.SizeOf, core.Size>();
            dictSizes[GameInstance.SizeOf.ASSETS] = new core.Size(invader.Size.Width, invader.Size.Height);
            dictSizes[GameInstance.SizeOf.SCREEN] = new core.Size(Size.Width - 32, Size.Height - 32);
            dictSizes[GameInstance.SizeOf.BULLETS] = new core.Size(bullet.Size.Width, bullet.Size.Height);
            int timeFire = timer1.Interval * 30;
            game = new GameInstance(dictSizes, 3, 2 * invader.Size.Width, 5, timeFire);
            int n = game.GetEnemies().Count;
            for (int i = 1; i < n; i++)
            {
                createEnemy(i);
            }
        }

        //Create assets to draw
        private void createEnemy(int count)
        {
            PictureBox picture = new PictureBox()
            {
                Name = "invaderClone" + count.ToString(),
                Size = invader.Size,
                Location = new Point(-100,-100),
                Image = invader.Image,
                Tag = invader.Tag,
                SizeMode = invader.SizeMode,
                BackColor = invader.BackColor,
            };
            this.Controls.Add(picture);
        }

        //
        private void createBullet(int count, int x, int y)
        {
            PictureBox picture = new PictureBox()
            {
                Name = "bulletClone" + count.ToString(),
                Size = bullet.Size,
                Location = new Point(Size.Width * x, Size.Height * y),
                Image = bullet.Image,
                Tag = bullet.Tag,
                SizeMode = bullet.SizeMode,
                BackColor = bullet.BackColor,
            };
            this.Controls.Add(picture);
        }

        //Handle Input and pass to the game
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (isPressed)
            {
                isPressed = false;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
            if (e.KeyCode == Keys.Space && !isPressed)
            {
                isPressed = true;
            }
            if (e.KeyCode == Keys.Enter && (Ready_Go_Label.Visible || GameOverLabel.Visible))
            {
                Ready_Go_Label.Hide();
                GameOverLabel.Hide();
                setGame();
                timer1.Enabled = true;
            }
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (game.isGameOver())
            {
                GameOverLabel.Show();
                Ready_Go_Label.Show();
                if (game.GetEnemies().Count==0)
                    GameOverLabel.Text = "YOU WIN";
                else
                    GameOverLabel.Text = "GAME OVER";
                GameOverLabel.Left = (this.Width - GameOverLabel.Width) / 2;
                Ready_Go_Label.Text = "Press Enter to Restart";
                Ready_Go_Label.Left = (this.Width - Ready_Go_Label.Width) / 2;
                return;
            }
            game.Update(goleft, goright, isPressed);
            List<Enemy> en = game.GetEnemies();
            List<Bullet> bull = game.GetBullets();
            int Ecount = 0;
            int Bcount = 0;

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && (string)c.Tag == "player")
                {
                    int x, y;
                    game.GetPlayerPos(out x, out y);
                    ((PictureBox)c).Top = y; //((PictureBox)c).Height * y;
                    ((PictureBox)c).Left = x; // ((PictureBox)c).Width * x;
                }

                if (c is PictureBox && (string)c.Tag == "invader")
                {
                    if (Ecount >= en.Count)
                    {
                        //remove from form
                        this.Controls.Remove(c);
                        //release memory by disposing
                        c.Dispose();
                        break;
                    }
                    int x, y;
                    en[Ecount].GetPos(out x, out y);
                    ((PictureBox)c).Top = y; // ((PictureBox)c).Height * y;
                    ((PictureBox)c).Left = x; // ((PictureBox)c).Width * x;
                    Ecount++;
                }

                if (c is PictureBox && (string)c.Tag == "bullet")
                {
                    if (bull.Count > Bcount)
                    {
                        int x, y;
                        bull[Bcount].GetPos(out x, out y);
                        ((PictureBox)c).Top = y; // ((PictureBox)c).Height * y;
                        ((PictureBox)c).Left = x; // ((PictureBox)c).Width * x;
                        Bcount++;
                    }
                    else
                    {
                        ((PictureBox)c).Top = -((PictureBox)c).Height;
                        ((PictureBox)c).Left = -((PictureBox)c).Width;
                    }
                }
            }

            while (bull.Count > Bcount)
            {
                int x, y;
                bull[Bcount].GetPos(out x, out y);
                createBullet(Bcount, x, y);
                Bcount++;
            }

            labelLives.Text = "lives: " + game.getLives().ToString();
        }
    }
}
