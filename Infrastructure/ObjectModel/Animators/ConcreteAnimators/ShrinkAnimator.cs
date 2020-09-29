////*** Guy Ronen © 2008-2011 ***//
using System;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class ShrinkAnimator : SpriteAnimator
    {
        private TimeSpan m_AnimationLength;
        private TimeSpan m_TimeLeftForShrink;

        public TimeSpan ShrinkLength
        {
            get { return m_AnimationLength; }
            set { m_AnimationLength = value; }
        }

        // CTORs
        public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_AnimationLength = i_AnimationLength;
            this.m_TimeLeftForShrink = i_AnimationLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.RotationOrigin = this.BoundSprite.SourceRectangleCenter;
            m_TimeLeftForShrink -= i_GameTime.ElapsedGameTime;

            float proportion = (float)(m_TimeLeftForShrink.TotalSeconds / m_AnimationLength.TotalSeconds);
            this.BoundSprite.Scales = new Vector2(proportion, proportion);
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
