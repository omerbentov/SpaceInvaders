////*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class CellAnimator : SpriteAnimator
    {
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx = 0;
        private readonly int r_NumOfCells = 1;

        // CTORs
        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength)
            : base("CelAnimation", i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        private void goToNextFrame()
        {
            m_CurrCellIdx++;
            if (m_CurrCellIdx >= r_NumOfCells)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = 0;
                }
                else
                {
                    m_CurrCellIdx = r_NumOfCells - 1; /// lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    /// we have elapsed, so blink
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }
    }
}
