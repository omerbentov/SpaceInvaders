using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class RotateAnimator : SpriteAnimator
    {
        private const float k_AngularVelocity = (float)MathHelper.TwoPi;
        private TimeSpan m_AnimationLength;
        private TimeSpan m_SingleRotateLength;
        private TimeSpan m_TimeLeftForRotate;
        private double m_LastRotationTime;

        public TimeSpan RotateLength
        {
            get { return m_AnimationLength; }
            set { m_AnimationLength = value; }
        }

        // CTORs
        public RotateAnimator(string i_Name, TimeSpan i_RotateLength, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_AnimationLength = i_AnimationLength;
            this.m_SingleRotateLength = i_RotateLength;
            this.m_TimeLeftForRotate = m_SingleRotateLength;

            this.m_LastRotationTime = 0;
        }

        public RotateAnimator(TimeSpan i_RotateLength, TimeSpan i_AnimationLength)
            : this("Rotate", i_RotateLength, i_AnimationLength)
        {
            this.m_AnimationLength = i_RotateLength;
            this.m_TimeLeftForRotate = i_RotateLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_AnimationLength -= i_GameTime.ElapsedGameTime;
            m_TimeLeftForRotate -= i_GameTime.ElapsedGameTime;

            if (m_TimeLeftForRotate.TotalSeconds < 0)
            {
                // we have elapsed, so blink
                m_TimeLeftForRotate = m_SingleRotateLength;
            }

            this.BoundSprite.RotationOrigin = this.BoundSprite.SourceRectangleCenter;
            this.BoundSprite.Rotation = (float)(MathHelper.TwoPi * (m_TimeLeftForRotate.TotalSeconds / m_SingleRotateLength.TotalSeconds));
            this.BoundSprite.AngularVelocity = k_AngularVelocity;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
