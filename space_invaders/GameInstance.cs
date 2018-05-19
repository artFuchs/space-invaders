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

        //assets information
        private int W_base;
        private int H_base;
        private int H_bullet;
        private int W_bullet;

        //Default Constructor
        public GameInstance()
        {
            screenLines = 20;
            screenCols = 20;
            EnemySystem = new EnemyController(2, this.screenCols, this.screenLines, 4);
            player = new Player(screenCols / 2, screenLines - 1, screenCols, 3);
            bullets = new List<Bullet>();
            game_over = false;
            W_base = 0;
            H_base = 0;
            W_bullet = 0;
            H_bullet = 0;
        }

        //Alternative Constructor
        public GameInstance(int screenLines, int screenCols, int enemyLines, int margin, int assetsW=0, int assetsH=0, int bulletW = 0, int bulletH = 0, int playerVel = 1)
        {
            this.screenLines = screenLines;
            this.screenCols = screenCols;
            EnemySystem = new EnemyController(enemyLines, this.screenCols, this.screenLines, margin, assetsW, assetsH, 5);
            player = new Player(screenCols/2, screenLines-32, screenCols, 20, bulletW, bulletW, playerVel);
            bullets = new List<Bullet>();
            game_over = false;
            W_base = assetsW;
            H_base = assetsH;
            W_bullet = bulletW;
            H_bullet = bulletH;
        }

        public void Update(bool Left, bool Right, bool shoot)
        {
            if (game_over)
                return;

            //update objects
            EnemySystem.Update();
            player.Update(Left, Right, shoot);
            foreach (Bullet b in bullets)
            {
                b.Update();
            }
            if (shoot)
            {
                Bullet b = player.getShoot();
                if (b!= null)
                    bullets.Add(b);
            }


            List<Bullet> to_remove = new List<Bullet>();
            //check collisions and remove bullets
            foreach (Bullet b in bullets)
            {
                bool outS = false;
                //check if bullet is out of screen
                b.GetPos(out int x, out int y);
                if (y > screenLines || y < 0)
                {
                    to_remove.Add(b);
                    outS = true;
                }
                //if not, check if it collide with some enemies
                if (!outS)
                {
                    if (EnemySystem.check_collisions(b))
                    {
                        to_remove.Add(b);
                    }
                } 
            }
            foreach (Bullet b in to_remove)
            {
                bullets.Remove(b);
            }

            //if enemies reached the botton of the screen, GAMEOVER
            if (EnemySystem.isEnd())
            {
                game_over = true;
                return;
            }
            //if all enemies were destroyed, GAMEOVER
            if (EnemySystem.GetEnemies().Count == 0)
            {
                game_over = true;
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
        protected int W;
        protected int H;

        public GameObject(int x, int y, int W=0, int H=0)
        {
            this.x = x;
            this.y = y;
            this.W = W;
            this.H = H;
        }

        public void GetPos(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public void GetSize(out int W, out int H)
        {
            H = this.H;
            W = this.W;
        }

        //check collision with a bounding box
        public bool Collide(GameObject other)
        {
            int _x, _y, _W, _H;
            other.GetPos(out _x, out _y);
            other.GetPos(out _W, out _H);
            bool collide = false;

            if (x == _x && y == _y)
            {
                collide = true;
            }

            if (!collide && W > 0 && H > 0)
            {
                if ((_x >= x && _x <= x + W) || (x >= _x && x <= _x + _W))
                {
                    if ((_y >= y && _y <= y + H) || (y >= _y && y <= _y + _H))
                    {
                        collide = true;
                    }
                }
            }

            return collide;
        }
    }

    class Player : GameObject
    {
        private int refresh_time;
        private int current_time;
        private int screenCols;
        private Bullet shoot;
        private int bW, bH;
        private int vel;

        public Player(int x, int y, int screenCols, int refreshTime, int bW=0, int bH=0, int vel=1) : base(x, y)
        {
            refresh_time = refreshTime;
            current_time = 0;
            this.screenCols = screenCols;
            this.bW = bW;
            this.bH = bH;
            this.vel = vel;
        }

        public void Update(bool left, bool right, bool shoot)
        {
            current_time--;
            if (right && !left && x < screenCols)
            {
                x+=vel;
            }

            if (left && !right && x > 0)
            {
                x-=vel;
            }

            if (shoot && current_time <= 0)
            {
                this.shoot = new Bullet(x+bW,y-bH,bW,bH+1,bH+1);
                current_time = refresh_time;
            }
        }

        public Bullet getShoot()
        {
            Bullet b = shoot;
            shoot = null;
            return b;
        }
    }

    class Bullet : GameObject
    {
        private bool enemyBullet;
        private int Vel;

        public Bullet(int x, int y, bool enemy=false) : base(x,y)
        {
            enemyBullet = enemy;
            Vel = 1;
        }

        public Bullet(int x, int y, int W, int H, int vel = 1, bool enemy=false) : base(x, y, W, H)
        {
            enemyBullet = enemy;
            Vel = vel;
        }

        public void Update()
        {
            if (enemyBullet)
            {
                y+=Vel;
            }
            else
                y-=Vel;
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

        public Enemy(int x, int y, int W, int H, int _maxTimeShoot) : base(x, y, W, H)
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
        private List<Enemy> Enemies;
        private int W, H;
        private int EnemyVel;
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
            Enemies = new List<Enemy>();
            W = 0;
            H = 0;

            for (int i = 0; i < nLines; i++)
            {
                for (int j = margin; j < screenCols - margin; j+=2)
                {
                    Enemies.Add(new Enemy(j, i, W, H, 20));
                }
            }

            gotToBottom = false;
            EnemyVel = 1;
        }

        public EnemyController(int nLines, int screenCols, int screenLines, int margin, int W, int H, int EnemyVel) : this(nLines, screenCols, screenLines, margin)
        {
            this.W = W;
            this.H = H;
            this.EnemyVel = EnemyVel;

            this.screenCols = screenCols;
            this.screenLines = screenLines;
            left = false;
            down = false;
            Enemies = new List<Enemy>();
            for (int i = 0; i < nLines; i += 1)
            {
                for (int j = margin; j < screenCols - margin; j += 2*W)
                {
                    Enemies.Add(new Enemy(j, i*H, W, H, 20));
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
                    if (x <= W || x >= screenCols-W)
                    {
                        left = !left;
                        down = true;
                        break;
                    }
                    if (y == screenLines-H)
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
                    e.Move(0, H);
                }
                else if (left)
                {
                    e.Move(-EnemyVel, 0);
                }
                else
                {
                    e.Move(EnemyVel, 0);
                }
            }
        }

        public bool check_collisions(Bullet b)
        {
            foreach (Enemy e in Enemies)
            {
                if (e.Collide(b))
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
