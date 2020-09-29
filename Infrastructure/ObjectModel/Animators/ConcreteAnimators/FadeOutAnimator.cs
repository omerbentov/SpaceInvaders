using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FadeOutAnimator : SpriteAnimator
    {
        private TimeSpan m_AnimationLength;
        private TimeSpan m_TimeLeftForFadeOut;

        public TimeSpan FadeOutLength
        {
            get { return m_AnimationLength; }
            set { m_AnimationLength = value; }
        }

        // CTORs
        public FadeOutAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_AnimationLength = i_AnimationLength;
            this.m_TimeLeftForFadeOut = i_AnimationLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForFadeOut -= i_GameTime.ElapsedGameTime;

            this.BoundSprite.Opacity = (float)(m_TimeLeftForFadeOut.TotalSeconds / m_AnimationLength.TotalSeconds);
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
