using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace C20_Ex01_Roe_313510489_Omer_206126138.Classes
{
    public class Barriers
    {
        private readonly Barrier[] m_Barriers;
        private bool m_LeftToRight;
        private float m_RightBorder;
        private float m_LeftBorder;
        private Game m_Game;

        public Barriers(Game i_Game, int i_NumOfBarriers)
        {
            m_LeftToRight = true;
            m_Barriers = new Barrier[i_NumOfBarriers];
            m_Game = i_Game;

            for (int i = 0; i < i_NumOfBarriers; i++)
            {
                m_Barriers[i] = new Barrier(i_Game);
            }
        }

        public void InitPositions(GraphicsDevice i_graphicDevice)
        {
            int barriersWidth = m_Barriers[0].Texture.Width;

            int allMiddleX = (int)((4 * barriersWidth) + (1.3 * 3 * barriersWidth));
            int firstX = (i_graphicDevice.Viewport.Width - allMiddleX) / 2;

            for(int i = 0; i < m_Barriers.Length; i++)
            {
                m_Barriers[i].Initialize(new Vector2(firstX + (i * (barriersWidth * (float)(1 + 1.3))), (m_Game as Game1).GetBarriersPostionY()));

                if (i == 0)
                {
                    m_LeftBorder = firstX + (i * (barriersWidth * (float)(1 + 1.3))) - (m_Barriers[i].Texture.Width / 2);
                }
                else if (i == m_Barriers.Length - 1)
                {
                    m_RightBorder = firstX + (i * (barriersWidth * (float)(1 + 1.3))) + (m_Barriers[i].Texture.Width / 2);
                }
            }
        }

        public void BulletIntersection(List<Bullet> i_Bullets)
        {
            foreach (Barrier barrier in m_Barriers)
            {
                foreach(Bullet bullet in i_Bullets)
                {
                    barrier.BulletIntersectionRectangle(bullet);
                }
            }
        }

        public void EnemyIntersection(Enemy[,] i_Enemies)
        {
            foreach (Barrier barrier in m_Barriers)
            {
                foreach (Enemy enemy in i_Enemies)
                {
                    barrier.EnemyIntersectionRectangle(enemy);
                }
            }
        }

        public void UpdateBarriers(GameTime i_GameTime)
        {
            Move(i_GameTime);
        }

        public void Move(GameTime i_GameTime)
        {
            if (m_Barriers[m_Barriers.Length - 1].Position.X >= m_RightBorder || m_Barriers[0].Position.X <= m_LeftBorder)
            {
                m_LeftToRight = !m_LeftToRight;
            }

            for (int i = 0; i < m_Barriers.Length; i++)
            {
                m_Barriers[i].Move(m_LeftToRight, i_GameTime);
            }
        }
    }
}
