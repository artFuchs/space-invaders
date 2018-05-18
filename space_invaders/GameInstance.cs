using System;
using System.Collections.Generic;
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
        private int nEnemies;
        private Player player;

        public GameInstance(int screenLines, int screenCols, int enemyLines, int margin)
        {
            this.screenLines = screenLines;
            this.screenCols = screenCols;
            EnemySystem = new EnemyController(enemyLines, this.screenCols, this.screenLines, margin);
            player = new Player(screenCols/2, screenLines-1, 3);
        }

        public void Update(bool Left, bool Right, bool shoot)
        {
            EnemySystem.Update();
        }

        public List<Enemy> GetEnemies()
        {
            return EnemySystem.GetEnemies();
        }

        public void GetPlayerPos(out int x, out int y)
        {
            player.GetPos(out x, out y);
        }
    }

    class GameObject
    {
        protected int x;
        protected int y;

        public GameObject(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void GetPos(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }
    }

    class Player : GameObject
    {
        int refresh_time;
        int current_time;

        public Player(int y, int x, int refreshTime) : base(x,y)
        {
            refresh_time = refreshTime;
            current_time = 0;
        }

        public void Update(bool left, bool right, bool shoot)
        {
            if (right && !left)
            {
                x++;
            }

            if (!right && left)
            {
                x--;
            }
        }
    }

    class Bullet : GameObject
    {
        private bool enemyBullet;

        public Bullet(int x, int y, bool enemy, int limit) : base(x,y)
        {
            enemyBullet = enemy;
        }

        public void Update()
        {
            if (enemyBullet)
            {
                y++;
            }
            else
                y--;
        }
    }
     
    class Enemy: GameObject
    {
        private int timeToShoot;
        private readonly int maxTimeShoot;
        private Random rdm;


        public Enemy(int x, int y, int _maxTimeShoot) : base(x,y)
        {
            maxTimeShoot = _maxTimeShoot;
            rdm = new Random();
            timeToShoot = rdm.Next(maxTimeShoot);
        }

        public void Move(int _x, int _y)
        {
            System.Console.WriteLine("moved");
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
                for (int j = margin; j < screenCols - margin; j+=2)
                {
                    Enemies.Add(new Enemy(j, i, 20));
                    nEnemies++;
                }
            }
        }

        public void Update()
        {
            if (down == false)
            {
                foreach (Enemy e in Enemies)
                {
                    int x, y;
                    e.GetPos(out x, out y);
                    if (x == 1 || x == screenCols-1)
                    {
                        left = !left;
                        down = true;
                        break;
                    }
                }
            }
            else
            {
                down = false;
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
        }

        public List<Enemy> GetEnemies()
        {
            return this.Enemies;
        }
    }
}
