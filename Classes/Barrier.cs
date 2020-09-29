using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using C20_Ex01_Roe_313510489_Omer_206126138;

namespace C20_Ex01_Roe_313510489_Omer_206126138.Classes
{
    public class Barrier : Sprite
    {
        private const string k_TexturePath = @"Sprites\Barrier_44x32";
        private const int k_BarrierVelocityPerSecond = 35;

        public Barrier(Game i_Game) : base(k_TexturePath, i_Game)
        {
        }

        public void Initialize(Vector2 i_Poition)
        {
            Position = i_Poition;
            Visible = true;
        }

        public void Draw(SpriteBatch i_SpriteBatch)
        {
            if (Visible)
            {
                i_SpriteBatch.Draw(Texture, Position, Color.White);
            }
        }

        public bool BulletIntersectionRectangle(Bullet i_bullet)
        {
            bool hit = false;

            Rectangle bulletRectangle = new Rectangle((int)i_bullet.Position.X, (int)i_bullet.Position.Y, i_bullet.Texture.Width, i_bullet.Texture.Height);
            Rectangle barriesRectangle = new Rectangle((int)this.m_Position.X, (int)this.m_Position.Y, this.Texture.Width, this.Texture.Height);

            if (bulletRectangle.Intersects(barriesRectangle))
            {
                int relativeX = Math.Clamp((int)(i_bullet.Position.X - this.m_Position.X), 0, this.Texture.Width);
                bool fromBottom = (i_bullet.Position.Y - this.m_Position.Y) >= 0;

                if (isBarrierGotHitFromBullet(fromBottom, (int)relativeX, i_bullet.Texture.Width, i_bullet.Texture.Height))
                {
                    hit = true;
                    i_bullet.IsActive = false;
                    (Game as Game1).EnemyBulletDisabled();
                }
            }

            return hit;
        }

        private bool isBarrierGotHitFromBullet(bool i_bottom, int x, int i_bulletWidth, int i_bulletHeight)
        {
            bool hit = false;

            Color[] barrierPixels = new Color[this.Texture.Width * this.Texture.Height];
            this.Texture.GetData<Color>(barrierPixels);

            // max demage is 0.35 of bullet size
            int startCounter = (int)(i_bulletHeight * i_bulletWidth * 0.35);
            int demageCounter = startCounter;

            int yIteration;

            // ship bullet
            if (i_bottom)
            {
                yIteration = this.Texture.Height - 1;

                for (int i = yIteration; i >= 0 && demageCounter > 0; i--) 
                {
                    for (int j = x; j < Math.Clamp(x + i_bulletWidth, 0, this.Texture.Width) && demageCounter > 0; j++)
                    {
                        if (barrierPixels[(i * this.Texture.Width) + j].A != 0)
                        {
                            barrierPixels[(i * this.Texture.Width) + j] = Color.Transparent;
                            demageCounter--;
                        }
                    }
                }
            }

            // Enemy bullet
            else
            {
                for (int i = 0; i < this.Texture.Height && demageCounter > 0; i++) 
                {
                    for (int j = x; j < Math.Clamp(x + i_bulletWidth, 0, this.Texture.Width) && demageCounter > 0; j++)
                    {
                        if (barrierPixels[(i * this.Texture.Width) + j].A != 0)
                        {
                            barrierPixels[(i * this.Texture.Width) + j] = Color.Transparent;
                            demageCounter--;
                        }
                    }
                }
            }

            this.Texture = new Texture2D(GraphicsDevice, this.Texture.Width, this.Texture.Height);
            this.Texture.SetData<Color>(barrierPixels);

            if (demageCounter < startCounter)
            {
                hit = true;
            }

            return hit;
        }

        public bool EnemyIntersectionRectangle(Sprite i_Sprite)
        {
            bool hit = false;

            Rectangle enemyRectangle = new Rectangle((int)i_Sprite.Position.X, (int)i_Sprite.Position.Y, i_Sprite.Texture.Width, i_Sprite.Texture.Height);
            Rectangle barriesRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);

            if (enemyRectangle.Intersects(barriesRectangle))
            {
                Rectangle cut_rectangle = generateRectangleToCut(i_Sprite);
                hit = CutByRectangle(cut_rectangle, i_Sprite);
            }

            return hit;
        }

        private Rectangle generateRectangleToCut(Sprite i_Sprite)
        {
            return new Rectangle(
                (int)Math.Clamp(i_Sprite.Position.X - this.Position.X, 0, this.Width),
                (int)Math.Clamp(i_Sprite.Position.Y - this.Position.Y, 0, this.Height),
                 Math.Clamp((int)(i_Sprite.Position.X + this.Width - this.Position.X), 0, (int)i_Sprite.Width),
                 Math.Clamp((int)(i_Sprite.Position.Y + this.Height - this.Position.Y), 0, (int)i_Sprite.Height));
        }

        public bool CutByRectangle(Rectangle i_RectangleToCut, Sprite i_Sprite)
        {
            bool hit = false;

            Color[] barrierPixels = new Color[this.Texture.Width * this.Texture.Height];
            this.Texture.GetData<Color>(barrierPixels);

            for (int x = i_RectangleToCut.X; x < Math.Clamp(i_RectangleToCut.X + i_RectangleToCut.Width, 0, this.Width); x++)
            {
                for (int y = i_RectangleToCut.Y; y < Math.Clamp(i_RectangleToCut.Y + i_RectangleToCut.Height, 0, this.Height); y++)
                {
                    if (barrierPixels[(y * this.Texture.Width) + x].A != 0)
                    {
                        barrierPixels[(y * this.Texture.Width) + x] = Color.Transparent;
                        hit = true;
                    }
                }
            }

            this.Texture.SetData<Color>(barrierPixels);
            return hit;
        }

        public void Move(bool i_LeftToRight, GameTime i_GameTime)
        {
            float distance = k_BarrierVelocityPerSecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            distance *= i_LeftToRight ? 1 : -1;

            m_Position.X += distance;
        }
    }
}
