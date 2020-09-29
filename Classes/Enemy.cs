using System;
using C20_Ex01_Roe_313510489_Omer_206126138.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace C20_Ex01_Roe_313510489_Omer_206126138
{
    public class Enemy : Sprite
    {
        private int k_NumOfFrames = 6;
        protected float k_EnemyVelocityPerSecond = 60;
        private eEnemyModels m_EnemyModel;
        private Bullet m_Bullet;
        private bool m_ImageFliped;
        private double m_GameTotalSeconds;
        private double m_TimeToJump;

        private bool m_Animating;

        public bool DuringAnimation
        {
            get { return m_Animating; }
            set { m_Animating = value; }
        }


        public Enemy(string i_Asset, Game i_Game) : base(i_Asset, i_Game)
        {
            m_Bullet = new Bullet(Color.Blue, i_Game);
            Visible = true;
            m_ImageFliped = false;
            m_GameTotalSeconds = 0;
            m_TimeToJump = 1;
            m_Animating = false;
        }

        public int Model 
        { 
            get 
            { 
                return (int)m_EnemyModel; 
            } 
        }

        public bool IsAlive 
        { 
            get 
            { 
                return Visible; 
            } 

            set 
            { 
                Visible = value; 
            } 
        }

        public Bullet Bullet 
        { 
            get 
            { 
                return m_Bullet; 
            } 

            set 
            { 
                m_Bullet = value; 
            } 
        }

        public enum eEnemyModels
        {
            Red = 600,
            Pink = 300,
            Blue = 200,
            Yellow = 70
        }

        public void Initialize(string i_Model, float i_DeltaX, float i_DeltaY)
        {
            base.Initialize();

            // Enemy
            Color enemyColor = Color.Black;

            switch (Enum.Parse(typeof(eEnemyModels), i_Model))
            {
                case eEnemyModels.Pink:
                    enemyColor = Color.LightPink;
                    break;
                case eEnemyModels.Blue:
                    enemyColor = Color.LightBlue;
                    break;
                case eEnemyModels.Yellow:
                    enemyColor = Color.LightYellow;
                    break;
                case eEnemyModels.Red:
                    enemyColor = Color.Red;
                    break;
            }

            m_EnemyModel = (eEnemyModels)Enum.Parse(typeof(eEnemyModels), i_Model);

            TintColor = enemyColor;

            InitAnimations();
            LoadContent(i_Model);
            InitPositions(i_DeltaX, i_DeltaY);
        }

        public void InitAnimations()
        {
            ShrinkAnimator shrinkAnimation = new ShrinkAnimator("shrink1", TimeSpan.FromSeconds(1.7));
            RotateAnimator rotateAnimator = new RotateAnimator("rotate1", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(1.7));

            this.Animations.Add(shrinkAnimation);
            this.Animations.Add(rotateAnimator);

            rotateAnimator.Finished += new EventHandler(rotateAnimation_Finished);
        }

        private void rotateAnimation_Finished(object sender, EventArgs e)
        {
            this.Animations["rotate1"].Pause();
            this.Animations["shrink1"].Pause();

            this.Visible = false;
            DuringAnimation = false;
        }

        public virtual void InitPositions(float i_DeltaX, float i_DeltaY)
        {
            const int margin = 20;

            float x = i_DeltaX * (m_SourceRectangle.Width + margin);
            float y = (3 * m_SourceRectangle.Height) + (i_DeltaY * (m_SourceRectangle.Height + margin));

            m_Position = new Vector2(x, y);
        }

        public virtual void LoadContent(string i_Model)
        {
            m_EnemyModel = (eEnemyModels)Enum.Parse(typeof(eEnemyModels), i_Model);
        }

        public bool Update(GameTime i_GameTime, bool i_LeftToRight, int i_Distance)
        {
            return TimeToJump(i_GameTime) ? Jump(i_GameTime, i_LeftToRight, i_Distance) : false;
        }

        public bool TimeToJump(GameTime I_GameTime)
        {
            bool jump = true;

            if (I_GameTime.TotalGameTime.TotalSeconds - m_GameTotalSeconds <= m_TimeToJump)
            {
                jump = false;
            }
            else
            {
                m_GameTotalSeconds = I_GameTime.TotalGameTime.TotalSeconds;
            }

            return jump;
        }

        public bool Jump(GameTime i_GameTime, bool i_LeftToRight, int i_Distance)
        {
            SwitchImage();

            if (i_LeftToRight)
            {
                MoveRight(i_GameTime, i_Distance);
            }
            else
            {
                MoveLeft(i_GameTime, i_Distance);
            }

            return true;
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();

            this.SourceRectangle = new Rectangle(
                    getOffsetImageEenemy(),
                    0,
                    (int)(m_SourceRectangle.Width / k_NumOfFrames),
                    (int)m_HeightBeforeScale);
        }

        public void SwitchImage()
        {
            int offsetX = getOffsetImageEenemy();

            offsetX += m_ImageFliped ? 0 : 32;

            this.SourceRectangle = new Rectangle(
            offsetX,
            0,
            (int)m_SourceRectangle.Width,
            (int)m_HeightBeforeScale);

            m_ImageFliped = !m_ImageFliped;
        }

        private int getOffsetImageEenemy()
        {
            int offsetX = 0;

            switch (m_EnemyModel)
            {
                case eEnemyModels.Blue:
                    offsetX += 64;
                    break;
                case eEnemyModels.Yellow:
                    offsetX += 128;
                    break;
            }

            return offsetX;
        }

        public bool UpdateBullet(GameTime gameTime, GraphicsDevice i_GraphicDevice)
        {
            if (m_Bullet.IsActive)
            {
                 return m_Bullet.UpdateForEnemy(i_GraphicDevice, gameTime);
            }

            return false;
        }

        public void MoveRight(GameTime i_GameTime, int i_Distance)
        {
            m_Position.X += i_Distance;
        }

        public void MoveLeft(GameTime i_GameTime, int i_Distance)
        {
            m_Position.X -= i_Distance;
        }

        public void MoveDown()
        {
            m_Position.Y += Texture.Height / 2;
            m_TimeToJump -= 0.05 * m_TimeToJump;
        }

        public void Shot()
        {
            if (!m_Bullet.IsActive)
            {
                m_Bullet.ChangedToActive(new Vector2(m_Position.X + (SourceRectangle.Width / 2), m_Position.Y));
            }
        }

        public void Draw(SpriteBatch i_SpriteBatch)
        {
            // Bullet
            if(m_Bullet.IsActive)
            {
                m_Bullet.Draw(i_SpriteBatch);
            }
        }
    }
}
