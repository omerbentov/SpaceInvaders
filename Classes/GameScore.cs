using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Infrastructure.ObjectModel;

namespace C20_Ex01_Roe_313510489_Omer_206126138.Classes
{
    public class GameScore
    {
        private int m_Score;
        public SpriteFont m_ConsolasFont;
        private ContentManager m_ContentManager;

        public GameScore(ContentManager i_ContentManager)
        {
            m_Score = 0;
            m_ContentManager = i_ContentManager;

            LoadContent();
        }

        public int Score 
        { 
            get 
            { 
                return m_Score; 
            } 
            
            set 
            { 
                m_Score = value; 
            } 
        }

        protected void LoadContent()
        {
            m_ConsolasFont = m_ContentManager.Load<SpriteFont>(@"Fonts\Consolas");
        }

        public void AddScore(int i_ScoreToAdd)
        {
            m_Score += i_ScoreToAdd;
            if (m_Score < 0)
            {
                m_Score = 0;
            }
        }
    }
}
