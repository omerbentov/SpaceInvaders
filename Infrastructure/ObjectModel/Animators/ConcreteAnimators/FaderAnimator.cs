using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FaderAnimator : SpriteAnimator
    {
        public FaderAnimator(string i_Name, TimeSpan i_AnimationLength) 
            : base(i_Name, i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float proportion = (float)(TimeLeft.TotalSeconds / AnimationLength.TotalSeconds);

            this.BoundSprite.Opacity *= proportion;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }
    }
}
