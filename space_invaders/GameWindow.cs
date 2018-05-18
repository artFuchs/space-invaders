﻿using System;
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
        

        public GameWindow()
        {
            InitializeComponent();
            game = new GameInstance(20, 20, 2);
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
                Location = new Point(100, 100),
                Image = invader.Image,
                Tag = invader.Tag,
                SizeMode = invader.SizeMode
            };
            this.Controls.Add(picture);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.Update(false, false, false);
            List<Enemy> en = game.GetEnemies();
            int count = 0;
            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && (string)c.Tag == "invader")
                {
                    if (count >= en.Count)
                    {
                        //remove from form
                        this.Controls.Remove(c);
                        //release memory by disposing
                        c.Dispose();
                        break;
                    }
                    int x, y;
                    en[count].GetPos(out x, out y);
                    ((PictureBox)c).Top = ((PictureBox)c).Height * y;
                    ((PictureBox)c).Left = ((PictureBox)c).Width * x;
                    count++;
                }
            }
        }
    }
}
