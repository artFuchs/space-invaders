using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace space_invaders.core
{
    class GameInstance
    {
        // screen information
        public enum SizeOf{
            SCREEN = 0,
            ASSETS,
            BULLETS
        }
        private Size screenSize;

        // object control
        private EnemyController EnemySystem;
        private Player player;
        private List<Bullet> bullets;

        // gameStatus
        private bool game_over;
        private int lives;
        private int wait_time;

        // assets information
        private Size baseSize;

        public GameInstance(Dictionary<SizeOf, Size> Sizes, int enemyLines, int margin, int playerVel = 1, int EnemyFireDelay = 30)
        {

            this.screenSize = new Size(Sizes[SizeOf.SCREEN]);
            baseSize = new Size(Sizes[SizeOf.ASSETS]);
            Size bulletSize = new Size (Sizes[SizeOf.BULLETS]);

            EnemySystem = new EnemyController(enemyLines, this.screenSize, margin, baseSize, 2, bulletSize, true);
            foreach (Enemy e in EnemySystem.GetEnemies())
            {
                e.Fire += new Enemy.ShootHandler(HearFire);
            }

            int pX = screenSize.W/2;
            int pY = screenSize.H - baseSize.H*2;
            int limW = screenSize.W - baseSize.W;
            player = new Player(pX, pY, limW, 10, baseSize, 5, bulletSize);
            player.Fire += new Player.ShootHandler(HearFire);

            bullets = new List<Bullet>();
            game_over = false;

            lives = 3;
            wait_time = 0;
        }

        public void Update(bool Left, bool Right, bool shoot)
        {
            if (game_over)
                return;

            //update objects
            foreach (Bullet b in bullets)
            {
                b.Update();
            }
            if (wait_time <= 0)
            {
                EnemySystem.Update();
                player.Update(Left, Right, shoot);
            }
            else
            {
                wait_time--;
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
                    if (b.EnemyBullet && wait_time <= 0)
                    {
                        if (player.Collide(b))
                        {
                            // lose one life
                            lives--;
                            // set wait time
                            wait_time = 30;
                            // reset player position
                            player.resetPos();
                        }
                    }
                    else if (EnemySystem.check_collisions(b))
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
            if (EnemySystem.isEnd() || lives <= 0)
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

        private void HearFire(Bullet b, EventArgs e)
        {
            bullets.Add(b);
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

        public int getLives()
        {
            return lives;
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
        private int x_ini;

        public delegate void ShootHandler(Bullet b, EventArgs e);
        private EventArgs e = null;
        public event ShootHandler Fire;


        public Player(int x, int y, int screen_limit, int refreshTime, Size playerSize = null, int vel = 1, Size bSize = null) : base(x, y)
        {
            x_ini = x;
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
                if (Fire != null)
                {
                    Fire(new Bullet(x + (size.W - bSize.W) / 2, y - size.H, bSize, bSize.H), e);
                }
                current_time = refresh_time;
            }
        }


        public void resetPos()
        {
            x = x_ini;
        }
    }

    class Bullet : GameObject
    {
        public bool EnemyBullet { get; }
        private int Vel;

        public Bullet(int x, int y, bool enemy=false) : base(x,y)
        {
            EnemyBullet = enemy;
            Vel = 1;
        }

        public Bullet(int x, int y, Size size, int vel = 1, bool enemy=false) : base(x, y, size)
        {
            EnemyBullet = enemy;
            Vel = vel;
        }

        public void Update()
        {
            if (EnemyBullet)
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
        private Size bSize;

        public delegate void ShootHandler(Bullet b, EventArgs e);
        private EventArgs e = null;
        public event ShootHandler Fire;

        public Enemy(int x, int y, int _maxTimeShoot) : base(x,y)
        {
            maxTimeShoot = _maxTimeShoot;
            rdm = new Random();
            timeToShoot = rdm.Next(maxTimeShoot);
            bSize = new Size(0,0);
        }

        public Enemy(int x, int y, Size size, int _maxTimeShoot, Size BulletSize) : base(x, y, size)
        {
            maxTimeShoot = _maxTimeShoot;
            rdm = new Random(x+y);
            timeToShoot = rdm.Next(maxTimeShoot);
            bSize = new Size(BulletSize);
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
            if (Fire!=null)
            {
                Fire(new Bullet(x + (size.W - bSize.W) / 2, y + size.H, bSize, bSize.H, true), e);
            }
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
        private bool EnemyChangeVel;
        private int nLines;
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
            this.nLines = nLines;
            for (int i = 0; i < nLines; i++)
            {
                for (int j = margin; j < screenSize.W - margin; j+=2)
                {
                    Enemies.Add(new Enemy(j, i, EnemySize, 100, new Size(0,0)));
                }
            }

            left = false;
            down = false;
            gotToBottom = false;
            EnemyVel = 1;
        }

        public EnemyController(int nLines, Size screenSize, int margin, Size EnemySize, int EnemyVel, Size bulletSize, bool EnemyChangeVel = false)
        {
            this.EnemySize = new Size(EnemySize);
            this.screenSize = new Size(screenSize);
            this.nLines = nLines;

            Enemies = new List<Enemy>();

            int W = this.EnemySize.W;
            int H = this.EnemySize.H;

            for (int i = 0; i < nLines; i += 1)
            {
                for (int j = margin; j < screenSize.W - margin; j += 2*W)
                {
                    Enemies.Add(new Enemy(j, i*H, EnemySize, 500, bulletSize));
                }
            }

            this.EnemyVel = EnemyVel;
            this.EnemyChangeVel = EnemyChangeVel;
            

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
            if (b.EnemyBullet)
            {
                return false;
            }
            foreach (Enemy e in Enemies)
            {
                if (e.Collide(b))
                {
                    Enemies.Remove(e);
                    if (EnemyChangeVel)
                    {
                        if (Enemies.Count > 3)
                            EnemyVel = screenSize.W / (Enemies.Count * EnemySize.W / nLines);

                    }
                    return true;
                }
            }

            return false;
        }

        public List<Enemy> GetEnemies()
        {
            return Enemies;
        }

        public bool isEnd()
        {
            return gotToBottom;
        }
    }
}
