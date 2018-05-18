using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace space_invaders
{
    class GameInstance
    {
        private int screenLines;
        private int screenCols;
        private EnemyController EnemySystem;
        private List<Enemy> Enemies;
        private Player player;


        public GameInstance(int screenLines, int screenCols)
        {
            this.screenLines = screenLines;
            this.screenCols = screenCols;
            EnemySystem = new EnemyController(5, this.screenCols, this.screenLines, 4);
            player = new Player();
        }

        public void Update(bool Left, bool Right, bool shoot)
        {
            EnemySystem.Update();
            Enemies = EnemySystem.GetEnemies();
        }

        public List<Enemy> GetEnemies()
        {
            return this.Enemies;
        }
    }

    class GameObject
    {
        protected int x;
        protected int y;

        public void GetPos(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }
    }

    class Player : GameObject
    {
        
    }
     
    class Enemy: GameObject
    {
        private int x;
        private int y;
        private int timeToShoot;
        private readonly int maxTimeShoot;
        private Random rdm;


        public Enemy(int _x, int _y, int _maxTimeShoot)
        {
            x = _x;
            y = _y;
            maxTimeShoot = _maxTimeShoot;
            rdm = new Random();
            timeToShoot = rdm.Next(maxTimeShoot);
        }

        public void Move(int _x, int _y)
        {
            x += _x;
            y += _y;
            timeToShoot--;
            if (timeToShoot <= 0)
                Shoot();
        }

        

        private void Shoot()
        {
            //shoot!!!!!
            timeToShoot = rdm.Next(maxTimeShoot);
        }

        public void Destroy()
        {
            // explosion here
        }
    }


    //This class is the entity that controls the enemies
    class EnemyController
    {
        //enemies Info
        private int nEnemies;
        private List<Enemy> Enemies;
        //movement information
        private bool left;
        private bool down;
        //screenInfo
        private int screenCols, screenLines;

        public EnemyController(int nLines, int screenCols, int screenLines, int margin)
        {
            this.screenCols = screenCols;
            this.screenLines = screenLines;
            left = false;
            down = false;
            nEnemies = 0;
            Enemies = new List<Enemy>();

            for (int i = 0; i < nLines; i++)
            {
                for (int j = margin; j < screenCols - margin; j++)
                {
                    Enemies.Add(new Enemy(i + 1, j + 1, 20));
                    nEnemies++;
                }
            }
        }

        public void Update()
        {
            foreach (Enemy e in Enemies)
            {
                int x, y;
                e.GetPos(out x, out y);
                if (x == 0 || x == screenCols)
                {
                    left = !left;
                    down = true;
                    break;
                }
            }

            foreach (Enemy e in Enemies)
            {
                if (down)
                {
                    e.Move(0, 1);
                }
                else if (left)
                {
                    e.Move(-1, 0);
                }
                else
                {
                    e.Move(1, 0);
                }
            }

            down = false;
        }

        public List<Enemy> GetEnemies()
        {
            return this.Enemies;
        }
    }
}
