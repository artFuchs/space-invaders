using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            game = new GameInstance(20, 20, 2, 4);
            int n = game.GetEnemies().Count;
            for (int i=1; i<n; i++)
            {
                createEnemy(i);
            }
        }

        private void createEnemy(int count)
        {
            PictureBox picture = new PictureBox()
            {
                Name = "invaderClone" + count.ToString(),
                Size = invader.Size,
                Location = new Point(-100,-100),
                Image = invader.Image,
                Tag = invader.Tag,
                SizeMode = invader.SizeMode
            };
            this.Controls.Add(picture);
        }

        private void createBullet(int count, int x, int y)
        {
            PictureBox picture = new PictureBox()
            {
                Name = "bulletClone" + count.ToString(),
                Size = bullet.Size,
                Location = new Point(Size.Width*x, Size.Height*y),
                Image = bullet.Image,
                Tag = bullet.Tag,
                SizeMode = bullet.SizeMode
            };
            this.Controls.Add(picture);
        }

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
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
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
                    ((PictureBox)c).Top = ((PictureBox)c).Height * y;
                    ((PictureBox)c).Left = ((PictureBox)c).Width * x;
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
                    ((PictureBox)c).Top = ((PictureBox)c).Height * y;
                    ((PictureBox)c).Left = ((PictureBox)c).Width * x;
                    Ecount++;
                }

                if (c is PictureBox && (string)c.Tag == "bullet")
                {
                    if (bull.Count > Bcount)
                    {
                        int x, y;
                        bull[Bcount].GetPos(out x, out y);
                        ((PictureBox)c).Top = ((PictureBox)c).Height * y;
                        ((PictureBox)c).Left = ((PictureBox)c).Width * x;
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
        }
    }
}
