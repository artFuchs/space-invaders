using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace space_invaders
{
    class GameInstance
    {
        //screen_information
        private int screenLines;
        private int screenCols;

        //ObjectControl
        private EnemyController EnemySystem;
        private Player player;
        private List<Bullet> bullets;

        //gameStatus
        private bool game_over;

        //Default Constructor
        public GameInstance()
        {
            screenLines = 20;
            screenCols = 20;
            EnemySystem = new EnemyController(2, this.screenCols, this.screenLines, 4);
            player = new Player(screenCols / 2, screenLines - 1, screenCols, 3);
            bullets = new List<Bullet>();
            game_over = false;
        }

        //Alternative Constructor
        public GameInstance(int screenLines, int screenCols, int enemyLines, int margin, int assetsW = 1, int assetsH = 1)
        {
            this.screenLines = screenLines;
            this.screenCols = screenCols;
            EnemySystem = new EnemyController(enemyLines, this.screenCols, this.screenLines, margin);
            player = new Player(screenCols/2, screenLines-1, screenCols, 3);
            bullets = new List<Bullet>();
            game_over = false;
        }

        public void Update(bool Left, bool Right, bool shoot)
        {
            if (game_over)
                return;
            //update movements
            EnemySystem.Update();
            if (EnemySystem.isEnd())
            {
                game_over = true;
                return;
            }
            player.Update(Left, Right, shoot);
            foreach (Bullet b in bullets)
            {
                b.Update();
            }

            if (shoot)
                bullets.Add(player.getShoot());

            //check collisions
            for (int i=0; i<bullets.Count; i++)
            {
                if (EnemySystem.check_collisions(bullets[i]))
                {
                    bullets.RemoveAt(i);
                    i--;
                 }   
            }

            
        }

        //get GameObjects informations to draw on screen
        public List<Enemy> GetEnemies()
        {
            return EnemySystem.GetEnemies();
        }

        public void GetPlayerPos(out int x, out int y)
        {
            player.GetPos(out x, out y);
        }

        public List<Bullet> GetBullets()
        {
            return bullets;
        }

        //get game Status
        public bool isGameOver()
        {
            return game_over;
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
        int screenCols;
        Bullet shoot;

        public Player(int x, int y, int screenCols, int refreshTime) : base(x, y)
        {
            refresh_time = refreshTime;
            current_time = 0;
            this.screenCols = screenCols;
        }

        public void Update(bool left, bool right, bool shoot)
        {
            current_time--;
            if (right && !left && x < screenCols)
            {
                x++;
            }

            if (left && !right && x > 0)
            {
                x--;
            }

            if (shoot && current_time <= 0)
            {
                this.shoot = new Bullet(x,y-1,false,0);
                current_time = refresh_time;
            }
        }

        public Bullet getShoot()
        {
            return shoot;
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
        private bool gotToBottom;

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

            gotToBottom = false;
        }

        public void Update()
        {
            if (gotToBottom)
                return;

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
                    if (y == screenLines-1)
                    {
                        gotToBottom = true;
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

        public bool check_collisions(Bullet b)
        {
            int bx, by;
            b.GetPos(out bx, out by);

            foreach (Enemy e in Enemies)
            {
                int ex, ey;
                e.GetPos(out ex, out ey);

                if (bx == ex && by == ey)
                {
                    Enemies.Remove(e);
                    return true;
                }
            }

            return false;
        }

        public List<Enemy> GetEnemies()
        {
            return this.Enemies;
        }

        public bool isEnd()
        {
            return gotToBottom;
        }
    }
}
