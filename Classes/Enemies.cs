using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Invaders.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace C20_Ex01_Roe_313510489_Omer_206126138.Classes
{
    public class Enemies
    {
        private const int k_MaxNumOfBullets = 5;
        private const int k_BulltDifficullty = 1; // 300 is easy - 1 is hard ( every frame)

        private Enemy[,] m_Enemies;
        private bool m_LeftToRight;
        private int m_NumOfBullets;

        private Game m_Game;

        public Enemies(Game i_Game)
        {
            m_Game = i_Game;
            m_LeftToRight = true;
            m_NumOfBullets = 0;
        }

        public int ActiveBullets 
        { 
            get 
            { 
                return m_NumOfBullets; 
            } 

            set 
            {
                m_NumOfBullets = value; 
            } 
        }

        public Enemy[,] Table 
        { 
            get 
            { 
                return m_Enemies; 
            } 

            set 
            { 
                m_Enemies = value; 
            } 
        }

        public void InitAndLoad()
        {
            m_Enemies = new Enemy[5, 9];
            m_LeftToRight = true;

            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    string model = "Pink";
                    string assetName = @"Sprites\Enemy_192x32";

                    if (i == 1 || i == 2)
                    {
                        model = "Blue";
                    }
                    else if (i != 0)
                    {
                        model = "Yellow";
                    }

                    m_Enemies[i, j] = new Enemy(assetName, m_Game);
                    m_Enemies[i, j].Initialize(model, j, i);
                }
            }
        }

        public Enemy GetEnemy(int x, int y)
        {
            return m_Enemies[x, y];
        }

        public void Update(GameTime gameTime)
        {
            EnemiesMovement(gameTime);
            TimeForShot();
        }
        
        public bool IsEndOfGame()
        {
            return EnemyReachedBottom(); // means enemy reached button so we return false for no more updates
        }

        public void EnemiesMovement(GameTime gameTime)
        {
            int distance = isOneOfEnemiesTochesBorder((float)m_Game.GraphicsDevice.Viewport.Width);

            if (distance <= 0)
            {
                ChanegeDirection();
            }

            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    m_Enemies[i, j].Update(gameTime, m_LeftToRight, distance);

                    if (m_Enemies[i, j].UpdateBullet(gameTime, m_Game.GraphicsDevice))
                    {
                        m_NumOfBullets--;
                    }
                }
            }
        }

        private int isOneOfEnemiesTochesBorder(float i_MaxWidth)
        {
            int distance = 0;
            bool touches = false;

            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    int recWidth = m_Enemies[i, j].SourceRectangle.Width;

                    if (m_Enemies[i, j].IsAlive)
                    {
                        if (m_LeftToRight)
                        {
                            float rightMargin = m_Enemies[i, j].Position.X + recWidth;

                            if (rightMargin + recWidth < i_MaxWidth)
                            {
                                distance = recWidth;
                            }
                            else
                            {
                                touches = true;
                                distance = (int)(i_MaxWidth - rightMargin);
                                break;
                            }
                        }
                        else
                        {
                            float positionX = m_Enemies[i, j].Position.X;

                            if (positionX - recWidth > 0)
                            {
                                distance = recWidth;
                            }
                            else
                            {
                                touches = true;
                                distance = (int)positionX;
                                break;
                            }
                        }
                    }
                }

                if (touches)
                {
                    return distance;
                }
            }

            return distance;
        }

        private void TimeForShot()
        {
            bool answer = false;

            Random rnd = new Random();
            if (rnd.Next(0, k_BulltDifficullty) == 0)
            {
                answer = true;
            }

            if (answer && (m_NumOfBullets < k_MaxNumOfBullets))
            {
                RandomEnemyShot();
            }
        }

        private void RandomEnemyShot()
        {
            Random rndI = new Random();
            Random rndJ = new Random();

            int i = rndI.Next(0, m_Enemies.GetLength(0));
            int j = rndJ.Next(0, m_Enemies.GetLength(1));

            while (!m_Enemies[i, j].IsAlive || m_Enemies[i, j].Bullet.IsActive)
            {
                i = rndI.Next(0, m_Enemies.GetLength(0));
                j = rndJ.Next(0, m_Enemies.GetLength(1));
            }

            m_Enemies[i, j].Shot();
            m_NumOfBullets++;
        }

        public void ChanegeDirection()
        {
            m_LeftToRight = !m_LeftToRight;
            MoveDown();
        }

        private void MoveDown()
        {
            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    if (m_Enemies[i, j].IsAlive)
                    {
                        m_Enemies[i, j].MoveDown();
                    }
                }
            }
        }

        public Enemy EnemyGotHitFromBullet(Ship i_ship)
        {
            Enemy enemyGotHit = null;

            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    if(i_ship.Bullet1.IsActive && m_Enemies[i, j].IsAlive && BulletIntersectsEnemy(m_Enemies[i, j], i_ship.Bullet1))
                    {
                        enemyGotHit = m_Enemies[i, j];
                    } else if (i_ship.Bullet2.IsActive && m_Enemies[i, j].IsAlive && BulletIntersectsEnemy(m_Enemies[i, j], i_ship.Bullet2))
                    {
                        enemyGotHit = m_Enemies[i, j];
                    }
                }
            }

            return enemyGotHit;
        }

        public bool ShipIntersection(Ship i_ship)
        {
            bool answer = false;

            Rectangle ShipRectangle = new Rectangle((int)i_ship.Position.X, (int)i_ship.Position.Y, i_ship.Texture.Width, i_ship.Texture.Height);
            Rectangle enemyRectangle;

            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    enemyRectangle = new Rectangle((int)m_Enemies[i, j].Position.X, (int)m_Enemies[i, j].Position.Y, m_Enemies[i, j].SourceRectangle.Width, m_Enemies[i, j].SourceRectangle.Height);
                    if (m_Enemies[i, j].IsAlive && enemyRectangle.Intersects(ShipRectangle))
                    {
                        answer = true;
                    }
                }
            }

            return answer;
        }

        public bool BulletIntersectsEnemy(Enemy i_Enemy, Bullet i_bullet)
        {
            bool hit = false;

            if (!i_Enemy.DuringAnimation)
            {
                Rectangle bulletRectangle = new Rectangle((int)i_bullet.Position.X, (int)i_bullet.Position.Y, i_bullet.SourceRectangle.Width, i_bullet.Texture.Height);
                Rectangle enemyRectangle = new Rectangle((int)i_Enemy.Position.X, (int)i_Enemy.Position.Y, i_Enemy.SourceRectangle.Width, i_Enemy.SourceRectangle.Height);

                if (bulletRectangle.Intersects(enemyRectangle))
                {
                    hit = true;
                    i_Enemy.DuringAnimation = true;
                    i_Enemy.Animations.Restart();
                    i_bullet.IsActive = false;
                }
            }

            return hit;
        }

        private bool EnemyReachedBottom()
        {
            bool answer = false;

            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    Enemy enemy = m_Enemies[i, j];
                    if (enemy.IsAlive && (enemy.Position.Y + enemy.Texture.Height >= m_Game.GraphicsDevice.Viewport.Height))
                    {
                        answer = true;
                    }
                }
            }

            return answer;
        }

        public bool AllEnemiesAreDead()
        {
            bool allDead = true;

            for (int i = 0; i < m_Enemies.GetLength(0); i++)
            {
                for (int j = 0; j < m_Enemies.GetLength(1); j++)
                {
                    if (m_Enemies[i, j].IsAlive)
                    {
                        allDead = false;
                    }
                }
            }

            return allDead;
        }
    }
}
