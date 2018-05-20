using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace space_invaders.core
{
    class GameInstance
    {
        //screen_information
        private Size screenSize;

        //ObjectControl
        private EnemyController EnemySystem;
        private Player player;
        private List<Bullet> bullets;

        //gameStatus
        private bool game_over;

        //assets information
        private Size baseSize;

        public GameInstance(Size screenSize, int enemyLines, int margin, Size assetsSize, Size bulletSize = null, int playerVel = 1)
        {
            this.screenSize = new Size(screenSize);
            baseSize = new Size(assetsSize);
            EnemySystem = new EnemyController(enemyLines, this.screenSize, margin, baseSize, 5);

            int pX = screenSize.W/2;
            int pY = screenSize.H - baseSize.H*2;
            int limW = screenSize.W - baseSize.W;
            player = new Player(pX, pY, limW, 10, baseSize, 5, bulletSize);

            bullets = new List<Bullet>();
            game_over = false;

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
                if (y > screenSize.W || y < 0)
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
        protected Size size;

        public GameObject(int x, int y, Size s = null)
        {
            this.x = x;
            this.y = y;
            if (s == null)
                size = new Size(0, 0);
            else
                size = new Size(s);
        }

        public void GetPos(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public void GetSize(out int W, out int H)
        {
            H = size.H;
            W = size.W;
        }

        public Size GetSize()
        {
            return new Size(size);
        }

        //check collision with a bounding box
        public bool Collide(GameObject other)
        {
            int _x, _y, _W, _H;
            other.GetPos(out _x, out _y);
            other.GetSize(out _W, out _H);
            bool collide = false;

            if (x == _x && y == _y)
            {
                collide = true;
            }


            int W = size.W;
            int H = size.H;

            if (!collide && W > 0 && H > 0)
            {
                if ((x + W >= _x) && (x <= _x + _W) && (y + H >= _y) && (y <= _y + _H))
                {
                        collide = true;
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
        private Size bSize;
        private int vel;

        public Player(int x, int y, int screen_limit, int refreshTime, Size playerSize = null, int vel = 1, Size bSize = null) : base(x, y)
        {
            refresh_time = refreshTime;
            current_time = 0;
            screenCols = screen_limit;

            size = new Size(playerSize);
            this.bSize = new Size(bSize);
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

                this.shoot = new Bullet(x + (size.W - bSize.W)/2 , y - size.H, bSize, bSize.H);
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

        public Bullet(int x, int y, Size size, int vel = 1, bool enemy=false) : base(x, y, size)
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

        public Enemy(int x, int y, Size size, int _maxTimeShoot) : base(x, y, size)
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
        private Size EnemySize;
        private int EnemyVel;
        //movement information
        private bool left;
        private bool down;
        //screenInfo
        private Size screenSize;
        private bool gotToBottom;

        public EnemyController(int nLines, Size screenSize, int margin)
        {
            this.screenSize = new Size(screenSize);
            
            Enemies = new List<Enemy>();
            EnemySize = new Size(0, 0);

            for (int i = 0; i < nLines; i++)
            {
                for (int j = margin; j < screenSize.W - margin; j+=2)
                {
                    Enemies.Add(new Enemy(j, i, EnemySize, 20));
                }
            }

            left = false;
            down = false;
            gotToBottom = false;
            EnemyVel = 1;
        }

        public EnemyController(int nLines, Size screenSize, int margin, Size EnemySize, int EnemyVel)
        {
            this.EnemySize = new Size(EnemySize);
            this.screenSize = new Size(screenSize);
            
            Enemies = new List<Enemy>();

            int W = this.EnemySize.W;
            int H = this.EnemySize.H;

            for (int i = 0; i < nLines; i += 1)
            {
                for (int j = margin; j < screenSize.W - margin; j += 2*W)
                {
                    Enemies.Add(new Enemy(j, i*H, EnemySize, 20));
                }
            }

            this.EnemyVel = EnemyVel;
            

            left = false;
            down = false;
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
                    if (x <= EnemySize.W || x >= screenSize.W-EnemySize.W)
                    {
                        left = !left;
                        down = true;
                        break;
                    }
                    if (y >= screenSize.H-EnemySize.W*2)
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
                    e.Move(0, EnemySize.H);
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
