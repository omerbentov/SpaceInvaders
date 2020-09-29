using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Invaders.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace C20_Ex01_Roe_313510489_Omer_206126138.Classes
{
    public class MotherShip : Enemy
    {
        private const int k_positionY = 32; 
        private const string k_AssetName = @"Sprites\MotherShip_32x120";

        private bool m_Hit;

        public MotherShip(Game i_Game) : base(k_AssetName, i_Game)
        {
            k_EnemyVelocityPerSecond = 95;
            TintColor = Color.Red;
            Visible = false;
            initPositions();
        }

        private void initPositions()
        {
            Position = new Vector2(0, 32);
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();

            this.SourceRectangle = new Rectangle(
                    0,
                    0,
                    (int)Texture.Width,
                    (int)Texture.Height);
        }

        public void GetReadyToPop()
        {
            initPositions();
            m_Hit = false;

            if (m_Animations["blink1"] == null)
            {
                InitAnimations();
                
            }

            Animations.Enabled = false;
            Visible = true;
        }

        public void MoveRight(GameTime i_GameTime)
        {
            if (!m_Hit)
            {
                MoveRight(i_GameTime, (int)(k_EnemyVelocityPerSecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds));

                if (Position.X >= GraphicsDevice.Viewport.Width)
                {
                    Visible = false;
                }
            }
        }

        public new void InitAnimations()
        {
            BlinkAnimator blinkAnimation = new BlinkAnimator("blink1", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(3));
            ShrinkAnimator shrinkAnimation = new ShrinkAnimator("shrink1", TimeSpan.FromSeconds(3));
            FadeOutAnimator fadeoutAnimator = new FadeOutAnimator("fadeout1", TimeSpan.FromSeconds(3));

            this.Animations.Add(blinkAnimation);
            this.Animations.Add(shrinkAnimation);
            this.Animations.Add(fadeoutAnimator);

            fadeoutAnimator.Finished += new EventHandler(fadeoutAnimator_Finished);
        }

        private void fadeoutAnimator_Finished(object sender, EventArgs e)
        {
            this.Animations.Pause();
            this.Animations.Enabled = false;
            this.Visible = false;

            this.Scales = new Vector2(1, 1);
            this.Opacity = 1;

            initPositions();
        }

        public bool IntersectionWithShipBullets(Ship i_Ship)
        {
            bool hit = false;

            if (!m_Hit)
            {
                Bullet bullet1 = i_Ship.Bullet1;
                Bullet bullet2 = i_Ship.Bullet2;

                Rectangle bulletRectangle1 = new Rectangle((int)bullet1.Position.X, (int)bullet1.Position.Y, bullet1.Texture.Width, bullet1.Texture.Height);
                Rectangle bulletRectangle2 = new Rectangle((int)bullet2.Position.X, (int)bullet2.Position.Y, bullet2.Texture.Width, bullet2.Texture.Height);
                Rectangle MotherShipRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

                if (bulletRectangle1.Intersects(MotherShipRectangle) && bullet1.IsActive)
                {
                    hit = true;
                    bullet1.IsActive = false;
                }
                else if (bulletRectangle2.Intersects(MotherShipRectangle) && bullet2.IsActive)
                {
                    hit = true;

                    bullet2.IsActive = false;
                }

                if (hit)
                {
                    m_Hit = true;
                    DuringAnimation = true;
                    m_Animations.Enabled = true;
                    m_Animations.Restart();
                }
            }

            return hit;
        }
    }
}
